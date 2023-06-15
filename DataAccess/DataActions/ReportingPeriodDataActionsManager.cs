using DataAccess.DataActionContext;
using DataAccess.DataActions.Interfaces;
using DataAccess.Entities;
using DataAccess.Extensions;
using DataAccess.LoggingFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataActions;

public class ReportingPeriodDataActionsManager : IReportingPeriodDataActions
{
    private readonly SupplierPortalDBContext _context;
    private readonly string REPORTING_PERIOD_STATUS_CLOSE = "Closed";
    private readonly string FACILITY_REPORTING_PERIOD_STATUS_SUBMITTED = "Submitted";
    private readonly string USER_ROLE_EXTERNAL = "External";
    private readonly IFileUploadDataActions _fileUploadDataActions;
    private readonly ISerilog _logger;
    private readonly IHttpContextAccessor _httpContext;

    public ReportingPeriodDataActionsManager(SupplierPortalDBContext context, IFileUploadDataActions fileUploadDataActions, ISerilog logger, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _fileUploadDataActions = fileUploadDataActions;
        _logger = logger;
        _httpContext = httpContextAccessor;
    }

    private void FindFacilityAndCheckSupplierContactLoginOrNot(int periodFacilityId)
    {
        var periodFacility = _context.ReportingPeriodFacilityEntities
                                    .Where(x => x.Id == periodFacilityId)
                                    .Include(x => x.ReportingPeriodSupplier)
                                        .ThenInclude(x => x.Supplier)
                                            .ThenInclude(x => x.ContactEntities)
                                                .ThenInclude(x => x.User)
                                    .FirstOrDefault();

        var allContacts = periodFacility.ReportingPeriodSupplier.Supplier.ContactEntities;

        //Get details for LoggedIn person
        var loginUserRoleName = _httpContext.HttpContext.FindLoginUserRoleName();
        var loginUserEmail = _httpContext.HttpContext.FindLoginUserEmail();

        if (loginUserRoleName == USER_ROLE_EXTERNAL)
        {
            var isLoginUser = allContacts.Any(x => x.User.Email == loginUserEmail);

            if (!isLoginUser)
                throw new Exception($"You are not authorized for Supplier {periodFacility.ReportingPeriodSupplier.Supplier.Name} !! Can't add/update FacilityDocument data for other Supplier !!");
        }
    }

    private void FindSupplierAndCheckSupplierContactLoginOrNot(int periodSupplierId)
    {
        var periodSupplier = _context.ReportingPeriodSupplierEntities
                                    .Where(x => x.Id == periodSupplierId)
                                    .Include(x => x.Supplier)
                                        .ThenInclude(x => x.ContactEntities)
                                            .ThenInclude(x => x.User)
                                    .FirstOrDefault();

        var allContacts = periodSupplier.Supplier.ContactEntities;

        //Get details for LoggedIn person
        var loginUserRoleName = _httpContext.HttpContext.FindLoginUserRoleName();
        var loginUserEmail = _httpContext.HttpContext.FindLoginUserEmail();

        if (loginUserRoleName == USER_ROLE_EXTERNAL)
        {
            var isLoginUser = allContacts.Any(x => x.User.Email == loginUserEmail);

            if (!isLoginUser)
                throw new Exception($"You are not Authorized person for Supplier {periodSupplier.Supplier.Name}!! Can't add/update SupplierDocument data for other Supplier !!");
        }
    }

    private string CreatedOrUpdatedByUser()
    {
        return _httpContext.HttpContext.FindLoginUserEmail();
    }

    #region Add Methods
    public bool AddReportingPeriod(ReportingPeriodEntity reportingPeriod)
    {
        //Only for reportingPeriodType is Year
        var existingReportingPeriod = _context.ReportingPeriodEntities.Where(x => x.ReportingPeriodTypeId == reportingPeriod.ReportingPeriodTypeId && x.CollectionTimePeriod == reportingPeriod.CollectionTimePeriod).FirstOrDefault();

        if (existingReportingPeriod != null)
            throw new Exception($"ReportingPeriod with the same CollectionTimePeriod {reportingPeriod.CollectionTimePeriod} and same ReportingPeriodType {reportingPeriod.ReportingPeriodType.Name} is already exists !!");

        var entity = new ReportingPeriodEntity();
        entity.DisplayName = reportingPeriod.DisplayName;
        entity.ReportingPeriodTypeId = reportingPeriod.ReportingPeriodTypeId;
        entity.CollectionTimePeriod = reportingPeriod.CollectionTimePeriod;
        entity.ReportingPeriodStatusId = reportingPeriod.ReportingPeriodStatusId;
        entity.StartDate = reportingPeriod.StartDate.Date;
        entity.EndDate = reportingPeriod.EndDate;
        entity.IsActive = reportingPeriod.IsActive;
        entity.CreatedBy = CreatedOrUpdatedByUser();
        entity.CreatedOn = DateTime.UtcNow;

        _context.ReportingPeriodEntities.Add(entity);
        _context.LogEntryAndSaveChanges(_logger);
        return true;
    }

    public bool AddPeriodSupplier(ReportingPeriodSupplierEntity reportingPeriodSupplierEntity)
    {
        reportingPeriodSupplierEntity.IsActive = true;
        _context.ReportingPeriodSupplierEntities.Add(reportingPeriodSupplierEntity);
        _context.LogEntryAndSaveChanges(_logger);
        return true;
    }

    public bool AddPeriodFacility(ReportingPeriodFacilityEntity reportingPeriodFacilityEntity, bool facilityIsRelaventForPeriod)
    {
        var allPeriodfacilities = _context.ReportingPeriodFacilityEntities;

        foreach (var existingPeriodFacility in allPeriodfacilities)
        {
            if (existingPeriodFacility.FacilityId == reportingPeriodFacilityEntity.FacilityId && existingPeriodFacility.ReportingPeriodId == reportingPeriodFacilityEntity.ReportingPeriodId && !facilityIsRelaventForPeriod)
                _context.ReportingPeriodFacilityEntities.Remove(existingPeriodFacility);
        }
        if (facilityIsRelaventForPeriod)
        {
            _context.ReportingPeriodFacilityEntities.Add(reportingPeriodFacilityEntity);
        }

        _context.LogEntryAndSaveChanges(_logger);
        return true;
    }

    public bool AddPeriodFacilityElectricityGridMix(IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> periodFacilityElectricityGridMixEntities, int periodFacilityId, int fercRegionId)
    {
        FindFacilityAndCheckSupplierContactLoginOrNot(periodFacilityId);
        var periodFacility = _context.ReportingPeriodFacilityEntities.Where(x => x.Id == periodFacilityId).Include(x => x.ReportingPeriodFacilityElectricityGridMixEntities).FirstOrDefault();

        if (periodFacility is null)
            throw new Exception("ReportingPeriodFacilityEntity not found for update fercRegion !!");

        periodFacility.FercRegionId = fercRegionId;

        var periodFacilityGridMixes = periodFacility.ReportingPeriodFacilityElectricityGridMixEntities;

        bool isRemovedGridMix = false;
        if (periodFacilityGridMixes.Count() != 0)
            isRemovedGridMix = RemovePeriodFacilityElectricityGridMix(periodFacilityId);

        if (isRemovedGridMix)
        {
            foreach (var entity in periodFacilityElectricityGridMixEntities)
            {
                entity.CreatedOn = DateTime.UtcNow;
                entity.CreatedBy = CreatedOrUpdatedByUser();
                _context.ReportingPeriodFacilityElectricityGridMixEntities.Add(entity);
            }
        }
        _context.LogEntryAndSaveChanges(_logger);
        return isRemovedGridMix;
    }

    public bool AddRemovePeriodFacilityGasSupplyBreakdown(IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownEntity> facilityGasSupplyBreakDownEntities, int periodSupplierId)
    {
        FindSupplierAndCheckSupplierContactLoginOrNot(periodSupplierId);
        var isRemovedGasSupply = RemovePeriodFacilityGasSupplyBreakdown(periodSupplierId);

        if (isRemovedGasSupply)
        {
            foreach (var entity in facilityGasSupplyBreakDownEntities)
            {
                entity.CreatedBy = CreatedOrUpdatedByUser();
                entity.CreatedOn = DateTime.UtcNow;
                _context.ReportingPeriodFacilityGasSupplyBreakDownEntities.Add(entity);
            }
        }
        _context.LogEntryAndSaveChanges(_logger);
        return isRemovedGasSupply;
    }

    public int AddUpdateReportingPeriodFacilityDocument(ReportingPeriodFacilityDocumentEntity periodFacilityDocument)
    {
        FindFacilityAndCheckSupplierContactLoginOrNot(periodFacilityDocument.ReportingPeriodFacilityId);

        var existingFacilityDocument = _context.ReportingPeriodFacilityDocumentEntities.Where(x => x.DocumentTypeId == periodFacilityDocument.DocumentTypeId && x.ReportingPeriodFacilityId == periodFacilityDocument.ReportingPeriodFacilityId).FirstOrDefault();

        if (existingFacilityDocument is null)
        {
            periodFacilityDocument.CreatedBy = CreatedOrUpdatedByUser();
            periodFacilityDocument.CreatedOn = DateTime.UtcNow;

            _context.ReportingPeriodFacilityDocumentEntities.Add(periodFacilityDocument);
        }
        else
        {
            existingFacilityDocument.ReportingPeriodFacilityId = periodFacilityDocument.ReportingPeriodFacilityId;
            existingFacilityDocument.Version = periodFacilityDocument.Version;
            existingFacilityDocument.DisplayName = periodFacilityDocument.DisplayName;
            existingFacilityDocument.StoredName = periodFacilityDocument.StoredName;
            existingFacilityDocument.Path = periodFacilityDocument.Path;
            existingFacilityDocument.DocumentStatusId = periodFacilityDocument.DocumentStatusId;
            existingFacilityDocument.DocumentTypeId = periodFacilityDocument.DocumentTypeId;
            existingFacilityDocument.ValidationError = periodFacilityDocument.ValidationError;
            existingFacilityDocument.UpdatedBy = CreatedOrUpdatedByUser();
            existingFacilityDocument.UpdatedOn = DateTime.UtcNow;
        }
        _context.LogEntryAndSaveChanges(_logger);

        return periodFacilityDocument.Id;
    }

    public bool AddUpdateReportingPeriodSupplierDocument(ReportingPeriodSupplierDocumentEntity periodSupplierDocument)
    {
        FindSupplierAndCheckSupplierContactLoginOrNot(periodSupplierDocument.ReportingPeriodSupplierId);

        var existingSupplierDocument = _context.ReportingPeriodSupplierDocumentEntities.Where(x => x.DocumentTypeId == periodSupplierDocument.DocumentTypeId && x.ReportingPeriodSupplierId == periodSupplierDocument.ReportingPeriodSupplierId).FirstOrDefault();

        if (existingSupplierDocument is null)
        {
            periodSupplierDocument.CreatedBy = CreatedOrUpdatedByUser();
            periodSupplierDocument.CreatedOn = DateTime.UtcNow;

            _context.ReportingPeriodSupplierDocumentEntities.Add(periodSupplierDocument);
        }
        else
        {
            existingSupplierDocument.ReportingPeriodSupplierId = periodSupplierDocument.ReportingPeriodSupplierId;
            existingSupplierDocument.Version = periodSupplierDocument.Version;
            existingSupplierDocument.DisplayName = periodSupplierDocument.DisplayName;
            existingSupplierDocument.StoredName = periodSupplierDocument.StoredName;
            existingSupplierDocument.Path = periodSupplierDocument.Path;
            existingSupplierDocument.DocumentStatusId = periodSupplierDocument.DocumentStatusId;
            existingSupplierDocument.DocumentTypeId = periodSupplierDocument.DocumentTypeId;
            existingSupplierDocument.ValidationError = periodSupplierDocument.ValidationError;
            existingSupplierDocument.IsActive = periodSupplierDocument.IsActive;
            existingSupplierDocument.UpdatedBy = CreatedOrUpdatedByUser();
            existingSupplierDocument.UpdatedOn = DateTime.UtcNow;

        }

        _context.LogEntryAndSaveChanges(_logger);
        return true;
    }

    #endregion

    #region Update Methods
    public bool UpdateReportingPeriod(ReportingPeriodEntity reportingPeriod)
    {
        var reportingPeriodEntity = _context.ReportingPeriodEntities.FirstOrDefault(x => x.Id == reportingPeriod.Id);

        if (reportingPeriodEntity == null)
            throw new Exception("ReportingPeriodEntity not found !!");

        reportingPeriodEntity.DisplayName = reportingPeriod.DisplayName;
        reportingPeriodEntity.ReportingPeriodTypeId = reportingPeriod.ReportingPeriodTypeId;
        reportingPeriodEntity.CollectionTimePeriod = reportingPeriod.CollectionTimePeriod;
        reportingPeriodEntity.ReportingPeriodStatusId = reportingPeriod.ReportingPeriodStatusId;
        reportingPeriodEntity.StartDate = reportingPeriod.StartDate;
        reportingPeriodEntity.EndDate = reportingPeriod.EndDate;
        reportingPeriodEntity.IsActive = reportingPeriod.IsActive;

        if (reportingPeriod.ReportingPeriodStatus.Name == REPORTING_PERIOD_STATUS_CLOSE)
        {
            reportingPeriodEntity.ReportingPeriodSupplierEntities = UpdateReportingPeriodSuppliersLockUnlockStatuses(reportingPeriod.ReportingPeriodSupplierEntities).ToList();
        }

        reportingPeriodEntity.UpdatedOn = DateTime.UtcNow;
        reportingPeriodEntity.UpdatedBy = CreatedOrUpdatedByUser();

        _context.ReportingPeriodEntities.Update(reportingPeriodEntity);
        _context.LogEntryAndSaveChanges(_logger);
        return true;
    }

    public bool UpdateReportingPeriodFacilityDataStatus(int periodFacilityId, int facilityPeriodDataStatusId)
    {
        var periodFacility = _context.ReportingPeriodFacilityEntities.FirstOrDefault(x => x.Id == periodFacilityId);

        if (periodFacility is null)
            throw new Exception("ReportingPeriodFacilityEntity not found !!");

        periodFacility.FacilityReportingPeriodDataStatusId = facilityPeriodDataStatusId;

        _context.LogEntryAndSaveChanges(_logger);
        return true;
    }

    public bool CheckAndUpdateReportingPeriodFacilityStatus(int periodFacilityId, int facilityStatusInProgressId)
    {
        var periodFacilityEntity = _context.ReportingPeriodFacilityEntities
                        .Where(x => x.Id == periodFacilityId)
                        .Include(x => x.FacilityReportingPeriodDataStatus)
                        .First();

        if (periodFacilityEntity.FacilityReportingPeriodDataStatus.Name == FACILITY_REPORTING_PERIOD_STATUS_SUBMITTED)
            periodFacilityEntity.FacilityReportingPeriodDataStatusId = facilityStatusInProgressId;

        _context.LogEntryAndSaveChanges(_logger);
        return true;
    }

    public IEnumerable<ReportingPeriodSupplierEntity> UpdateReportingPeriodSuppliersLockUnlockStatuses(IEnumerable<ReportingPeriodSupplierEntity> periodSuppliers)
    {
        var updatedReportingPeriodSuppliers = new List<ReportingPeriodSupplierEntity>();

        var reportingPeriodId = periodSuppliers.First().ReportingPeriodId;
        var allPeriodSuppliers = _context.ReportingPeriodSupplierEntities.Where(x => x.ReportingPeriodId == reportingPeriodId).ToList();

        foreach (var periodSupplier in periodSuppliers)
        {
            var updatePeriodSupplier = allPeriodSuppliers
            .Where(x => x.Id == periodSupplier.Id).FirstOrDefault();

            if (updatePeriodSupplier == null)
                throw new Exception("ReportingPeriodSupplierEntity not found for update !!");

            updatePeriodSupplier.SupplierReportingPeriodStatusId = periodSupplier.SupplierReportingPeriodStatusId;

            updatedReportingPeriodSuppliers.Add(updatePeriodSupplier);
        }

        _context.LogEntryAndSaveChanges(_logger);
        return updatedReportingPeriodSuppliers;
    }

    public bool UpdateReportingPeriodSupplierLockUnlockStatus(ReportingPeriodSupplierEntity periodSupplierEntity)
    {
        var periodSupplier = _context.ReportingPeriodSupplierEntities.FirstOrDefault(x => x.Id == periodSupplierEntity.Id);

        if (periodSupplier == null)
            throw new Exception("ReportingPeriodSupplierEntity not found !!");

        periodSupplier.SupplierReportingPeriodStatusId = periodSupplierEntity.SupplierReportingPeriodStatusId;

        _context.LogEntryAndSaveChanges(_logger);
        return true;
    }

    public bool UpdatePeriodSupplierInitialDataRequestDate(int periodSupplierId)
    {
        var periodSupplier = _context.ReportingPeriodSupplierEntities.FirstOrDefault(x => x.Id == periodSupplierId);

        if (periodSupplier is null)
            throw new Exception("ReportingPeriodSupplierEntity not found !!");

        if (periodSupplier.InitialDataRequestDate == null)
            periodSupplier.InitialDataRequestDate = DateTime.UtcNow;
        else
            periodSupplier.ResendDataRequestDate = DateTime.UtcNow;

        _context.LogEntryAndSaveChanges(_logger);
        return true;
    }
    #endregion

    #region Remove Methods

    public bool RemovePeriodSupplier(int periodSupplierId)
    {
        var periodSupplier = _context.ReportingPeriodSupplierEntities.Where(x => x.Id == periodSupplierId)
            .Include(x => x.ReportingPeriodFacilityEntities).FirstOrDefault();

        if (periodSupplier == null)
        {
            return false;
        }

        _context.ReportingPeriodSupplierEntities.Remove(periodSupplier);
        _context.LogEntryAndSaveChanges(_logger);
        return true;

    }

    public bool RemovePeriodFacilityElectricityGridMix(int periodFacilityId)
    {
        bool isRemoved = false;
        var entities = _context.ReportingPeriodFacilityElectricityGridMixEntities.Where(x => x.ReportingPeriodFacilityId == periodFacilityId).ToList();

        foreach (var entity in entities)
        {
            if (entity == null)
                throw new Exception("RemovePeriodFacilityElectricityGridMix not found !!");

            _context.ReportingPeriodFacilityElectricityGridMixEntities.Remove(entity);

            isRemoved = true;
        }
        _context.LogEntryAndSaveChanges(_logger);
        return isRemoved;
    }

    public bool RemovePeriodFacilityGasSupplyBreakdown(int periodSupplierId)
    {
        bool isRemoved = false;
        var periodSupplierEntity = _context.ReportingPeriodSupplierEntities.Where(x => x.Id == periodSupplierId).FirstOrDefault();
        var periodFacilityEntities = periodSupplierEntity.ReportingPeriodFacilityEntities;
        var gasSupplyBreakdownEntities = _context.ReportingPeriodFacilityGasSupplyBreakDownEntities;

        foreach (var periodFacilityEntity in periodFacilityEntities)
        {
            var entities = gasSupplyBreakdownEntities.Where(x => x.PeriodFacilityId == periodFacilityEntity.Id).ToList();

            if (entities.Count() != 0)
                _context.ReportingPeriodFacilityGasSupplyBreakDownEntities.RemoveRange(entities);

            isRemoved = true;
        }

        _context.LogEntryAndSaveChanges(_logger);
        return isRemoved;
    }

    public bool RemovePeriodFacilityDocument(int documentId)
    {
        var documentEntity = _context.ReportingPeriodFacilityDocumentEntities.FirstOrDefault(x => x.Id == documentId);

        if (documentEntity is null)
            throw new Exception("Document not found !!");

        var fileDeleted = _fileUploadDataActions.RemoveDocumentFromFolder(documentEntity.Path);

        //If file deleted successfully then remove from database
        if (fileDeleted)
            _context.ReportingPeriodFacilityDocumentEntities.Remove(documentEntity);

        _context.LogEntryAndSaveChanges(_logger);

        return true;
    }

    public bool RemovePeriodSupplierDocument(int documentId)
    {
        var documentEntity = _context.ReportingPeriodSupplierDocumentEntities.FirstOrDefault(x => x.Id == documentId);

        if (documentEntity is null)
            throw new Exception("Document not found !!");

        var fileDeleted = _fileUploadDataActions.RemoveDocumentFromFolder(documentEntity.Path);

        //If file deleted successfully then remove from database
        if (fileDeleted)
        {
            _context.ReportingPeriodSupplierDocumentEntities.Remove(documentEntity);
            _context.LogEntryAndSaveChanges(_logger);
            return true;
        }
        return false;
    }

    #endregion

    #region GetAll Methods

    public IEnumerable<ReportingPeriodTypeEntity> GetReportingPeriodTypes()
    {
        return _context.ReportingPeriodTypeEntities;
    }

    public IEnumerable<ReportingPeriodStatusEntity> GetReportingPeriodStatus()
    {
        return _context.ReportingPeriodStatusEntities;
    }

    public IEnumerable<ReportingPeriodEntity> GetReportingPeriods()
    {
        var reportingPeriods = _context.ReportingPeriodEntities
                                .Include(x => x.ReportingPeriodType)
                                .Include(x => x.ReportingPeriodStatus)
                                .Include(x => x.ReportingPeriodSupplierEntities)
                                .ToList();

        return reportingPeriods;
    }

    public IEnumerable<SupplierReportingPeriodStatusEntity> GetSupplierReportingPeriodStatus()
    {
        return _context.SupplierReportingPeriodStatusEntities;
    }

    public IEnumerable<ReportingPeriodSupplierEntity> GetPeriodSuppliers()
    {
        var periodSuppliers = _context.ReportingPeriodSupplierEntities
                                .Include(x => x.Supplier)
                                .Include(x => x.ReportingPeriod)
                                .Include(x => x.SupplierReportingPeriodStatus)
                                .ToList();
        return periodSuppliers;
    }

    public IEnumerable<DocumentRequiredStatusEntity> GetDocumentRequiredStatus()
    {
        return _context.DocumentRequiredStatusEntities.ToList();
    }

    public IEnumerable<DocumentStatusEntity> GetDocumentStatusEntities()
    {
        return _context.DocumentStatusEntities;
    }

    public IEnumerable<DocumentTypeEntity> GetDocumentTypeEntities()
    {
        return _context.DocumentTypeEntities;
    }

    public IEnumerable<FacilityReportingPeriodDataStatusEntity> GetFacilityReportingPeriodDataStatus()
    {
        return _context.FacilityReportingPeriodDataStatusEntities.ToList();
    }

    public IEnumerable<FacilityRequiredDocumentTypeEntity> GetFacilityRequiredDocumentType()
    {
        return _context.FacilityRequiredDocumentTypeEntities
                                    .Include(x => x.ReportingType)
                                    .Include(x => x.SupplyChainStage)
                                    .Include(x => x.DocumentType)
                                    .Include(x => x.DocumentRequiredStatus)
                                    .ToList();
    }

    public IEnumerable<ReportingTypeEntity> GetReportingTypes()
    {
        return _context.ReportingTypeEntities;
    }

    public IEnumerable<ElectricityGridMixComponentEntity> GetElectricityGridMixComponentEntities()
    {
        return _context.ElectricityGridMixComponentEntities;
    }

    public IEnumerable<UnitOfMeasureEntity> GetUnitOfMeasureEntities()
    {
        return _context.UnitOfMeasureEntities;
    }
    public IEnumerable<FercRegionEntity> GetFercRegionEntities()
    {
        return _context.FercRegionEntities;
    }

    public IEnumerable<SiteEntity> GetSiteEntities()
    {
        return _context.SiteEntities;
    }

    public IEnumerable<ReportingPeriodFacilityDocumentEntity> GetReportingPeriodFacilityDocuments()
    {
        var documentEntities = _context.ReportingPeriodFacilityDocumentEntities
                                    .Include(x => x.ReportingPeriodFacility)
                                    .Include(x => x.DocumentStatus)
                                    .Include(x => x.DocumentType)
                                    .ToList();

        return documentEntities;
    }

    public IEnumerable<FacilityRequiredDocumentTypeEntity> GetFacilityRequiredDocumentTypes()
    {
        var faciltityRequiredDocumentTypes = _context.FacilityRequiredDocumentTypeEntities
                                        .Include(x => x.ReportingType)
                                        .Include(x => x.SupplyChainStage)
                                        .Include(x => x.DocumentRequiredStatus)
                                        .ToList();
        return faciltityRequiredDocumentTypes;
    }

    public IEnumerable<EmailTemplateEntity> GetEmailTemplateEntities()
    {
        return _context.EmailTemplateEntities;
    }

    #endregion

    #region GetById
    public ReportingPeriodEntity GetReportingPeriodById(int reportingPeriodId)
    {
        var reportingPeriod = _context.ReportingPeriodEntities
                                .Where(x => x.Id == reportingPeriodId)
                                .Include(x => x.ReportingPeriodType)
                                .Include(x => x.ReportingPeriodStatus)
                                .Include(x => x.ReportingPeriodSupplierEntities)
                                    .ThenInclude(x => x.ReportingPeriodSupplierDocumentEntities)
                                .Include(x => x.ReportingPeriodFacilityEntities)
                                    .ThenInclude(x => x.ReportingPeriodFacilityElectricityGridMixEntities)
                                .Include(x => x.ReportingPeriodFacilityEntities)
                                    .ThenInclude(x => x.ReportingPeriodFacilityGasSupplyBreakDownEntities)
                                .Include(x => x.ReportingPeriodFacilityEntities)
                                    .ThenInclude(x => x.ReportingPeriodFacilityDocumentEntities)
                                .AsSplitQuery()
                                .FirstOrDefault();

        return reportingPeriod;
    }

    public ReportingPeriodSupplierEntity GetPeriodSupplierById(int periodSupplierId)
    {
        var periodSupplier = _context.ReportingPeriodSupplierEntities
                                .Include(x => x.Supplier)
                                .Include(x => x.ReportingPeriod)
                                .Include(x => x.ReportingPeriodSupplierDocumentEntities)
                                .Include(x => x.ReportingPeriodFacilityEntities)
                                    .ThenInclude(x => x.ReportingPeriodFacilityElectricityGridMixEntities)
                                .Include(x => x.ReportingPeriodFacilityEntities)
                                    .ThenInclude(x => x.ReportingPeriodFacilityGasSupplyBreakDownEntities)
                                .Include(x => x.ReportingPeriodFacilityEntities)
                                    .ThenInclude(x => x.ReportingPeriodFacilityDocumentEntities)
                                .AsSplitQuery()
                                .FirstOrDefault(x => x.Id == periodSupplierId);
        return periodSupplier;
    }

    public ReportingPeriodFacilityEntity GetPeriodFacilityById(int periodFacilityId)
    {
        var periodFacility = _context.ReportingPeriodFacilityEntities
                                .Include(x => x.Facility)
                                .Include(x => x.ReportingType)
                                .Include(x => x.SupplyChainStage)
                                .Include(x => x.ReportingPeriod)
                                .Include(x => x.ReportingPeriodSupplier)
                                .Include(x => x.ReportingPeriodFacilityElectricityGridMixEntities)
                                .Include(x => x.ReportingPeriodFacilityDocumentEntities)
                                .FirstOrDefault(x => x.Id == periodFacilityId);

        return periodFacility;
    }

    public ReportingPeriodFacilityDocumentEntity GetReportingPeriodDocument(int documentId)
    {
        var periodDocument = _context.ReportingPeriodFacilityDocumentEntities.First(x => x.Id == documentId);

        if (periodDocument is null)
            throw new Exception("ReportingPeriodDocumentEntity not found !!");

        return periodDocument;
    }
    public ReportingPeriodSupplierDocumentEntity GetReportingPeriodSupplierDocument(int documentId)
    {
        var periodDocument = _context.ReportingPeriodSupplierDocumentEntities.First(x => x.Id == documentId);

        if (periodDocument is null)
            throw new Exception("ReportingPeriodDocumentEntity not found !!");

        return periodDocument;
    }


    #endregion

    #region Dispose Methods
    protected void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }



    #endregion


}
