using DataAccess.DataActionContext;
using DataAccess.DataActions.Interfaces;
using DataAccess.Entities;
using DataAccess.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;

namespace DataAccess.DataActions;

public class ReportingPeriodDataActionsManager : IReportingPeriodDataActions
{
    private readonly SupplierPortalDBContext _context;
    private readonly IUploadDocuments _uploadDocuments;
    private readonly ISerilog _logger;
    private readonly string REPORTING_PERIOD_STATUS_CLOSE = "Closed";
    private readonly string INITIALDATAREQUEST = "InitialDataRequest";
    private readonly IHttpContextAccessor _httpContext;

    public ReportingPeriodDataActionsManager(SupplierPortalDBContext context, IUploadDocuments uploadDocuments, ISerilog logger, IHttpContextAccessor httpContext)
    {
        _context = context;
        _uploadDocuments = uploadDocuments;
        _logger = logger;
        _httpContext = httpContext;
    }

    #region ReportingPeriod

    private string FindLoggedUser()
    {
        return _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

   /* private void FindFacilityAndCheckSupplierContactLoginOrNot(int periodFacilityId)
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
            var loginUserSupplierName = _context.FindLoginUserSupplierName(loginUserEmail);
            var isLoginUser = allContacts.Any(x => x.User.Email == loginUserEmail);

            if (!isLoginUser)
                throw new Exception($"You have LoggedIn for Supplier {loginUserSupplierName} !! Can't add/update FacilityDocument data for other Supplier !!");
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
            var loginUserSupplierName = _context.FindLoginUserSupplierName(loginUserEmail);
            var isLoginUser = allContacts.Any(x => x.User.Email == loginUserEmail);

            if (!isLoginUser)
                throw new Exception($"You have LoggedIn for Supplier {loginUserSupplierName} !! Can't add/update SupplierDocument data for other Supplier !!");
        }
    }*/

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
        entity.CreatedBy = FindLoggedUser();
        entity.CreatedOn = DateTime.UtcNow;

        _context.ReportingPeriodEntities.Add(entity);
        _context.LogExtensionMethod(_logger);
        return true;
    }

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
            reportingPeriodEntity.ReportingPeriodSupplierEntities = UpdateReportingPeriodSupplierLockUnlockStatuses(reportingPeriod.ReportingPeriodSupplierEntities).ToList();
        }

        reportingPeriodEntity.UpdatedOn = DateTime.UtcNow;
        reportingPeriodEntity.UpdatedBy = FindLoggedUser();

        _context.ReportingPeriodEntities.Update(reportingPeriodEntity);
        _context.LogExtensionMethod(_logger);
        return true;
    }

    #endregion

    #region PeriodSupplier

    public bool AddPeriodSupplier(ReportingPeriodSupplierEntity reportingPeriodSupplierEntity)
    {
        reportingPeriodSupplierEntity.IsActive = true;
        _context.ReportingPeriodSupplierEntities.Add(reportingPeriodSupplierEntity);
        _context.LogExtensionMethod(_logger);
        return true;
    }

    public IEnumerable<ReportingPeriodSupplierEntity> UpdateReportingPeriodSupplierLockUnlockStatuses(IEnumerable<ReportingPeriodSupplierEntity> periodSuppliers)
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

        _context.LogExtensionMethod(_logger);
        return updatedReportingPeriodSuppliers;
    }

    public bool UpdateReportingPeriodSupplierLockUnlockStatus(ReportingPeriodSupplierEntity reportingPeriodSupplierEntity)
    {
        var periodSupplierEntity = _context.ReportingPeriodSupplierEntities.Where(x => x.Id == reportingPeriodSupplierEntity.Id).FirstOrDefault();

        if (periodSupplierEntity == null)
            throw new Exception("ReportingPeirodSupplierEntity not found for update !!");

        periodSupplierEntity.SupplierReportingPeriodStatusId = reportingPeriodSupplierEntity.SupplierReportingPeriodStatusId;

        _context.LogExtensionMethod(_logger);
        return true;

    }

    public bool SendEmailInitialAndResendDataRequest(int periodSupplierId)
    {
        var periodSupplierEntity = _context.ReportingPeriodSupplierEntities.Where(x => x.Id == periodSupplierId).FirstOrDefault();

        if (periodSupplierEntity is null)
            throw new Exception("PeriodSupplierEntity is not found !!");

        if (periodSupplierEntity.InitialDataRequestDate is null)
            periodSupplierEntity.InitialDataRequestDate = DateTime.UtcNow;
        else
            periodSupplierEntity.ResendDataRequestDate = DateTime.UtcNow;

        _context.ReportingPeriodSupplierEntities.Update(periodSupplierEntity);
        _context.LogExtensionMethod(_logger);
        return true;

    }

    #endregion

    #region PeriodFacility

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

        _context.LogExtensionMethod(_logger);
        return true;
    }

    public IEnumerable<ReportingPeriodFacilityEntity> UpdatePeriodFacilities(IEnumerable<ReportingPeriodFacilityEntity> periodFacilityEntities)
    {
        var updatedPeriodFacilities = new List<ReportingPeriodFacilityEntity>();

        var reportingPeriodId = periodFacilityEntities.First().ReportingPeriodId;
        var allPeriodFacilities = _context.ReportingPeriodFacilityEntities.Where(x => x.ReportingPeriodId == reportingPeriodId).ToList();

        foreach (var periodFacility in periodFacilityEntities)
        {
            var updatePeriodFacility = allPeriodFacilities
            .Where(x => x.Id == periodFacility.Id).FirstOrDefault();

            if (updatePeriodFacility == null)
                throw new Exception("PeriodFacilityEntity not found for update !!");

            updatePeriodFacility.FacilityReportingPeriodDataStatusId = periodFacility.FacilityReportingPeriodDataStatusId;

            updatedPeriodFacilities.Add(updatePeriodFacility);
        }

        _context.LogExtensionMethod(_logger);
        return updatedPeriodFacilities;
    }

    public bool UpdateReportingPeriodFacilityDataStatus(int periodFacilityId, int periodFacilityDataStatusId)
    {
        var periodFacility = _context.ReportingPeriodFacilityEntities.Where(x => x.Id == periodFacilityId).Include(x => x.ReportingPeriodFacilityDocumentEntities).FirstOrDefault();

        periodFacility.FacilityReportingPeriodDataStatusId = periodFacilityDataStatusId;

        _context.ReportingPeriodFacilityEntities.Update(periodFacility);
        _context.LogExtensionMethod(_logger);

        return true;
    }

    #endregion

    #region PeriodFacilityElectricityGridMix

    public bool AddPeriodFacilityElectricityGridMix(IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> periodFacilityElectricityGridMixEntities, int periodFacilityId, int fercRegionId)
    {
        FindPeriodFacilityAndCheckSupplierContactLoginOrNot(periodFacilityId);

        var periodFacilityEntity = _context.ReportingPeriodFacilityEntities.Where(x => x.Id == periodFacilityId).Include(x => x.ReportingPeriodFacilityElectricityGridMixEntities).FirstOrDefault();

        periodFacilityEntity.FercRegionId = fercRegionId;

        var electricityGridMix = periodFacilityEntity.ReportingPeriodFacilityElectricityGridMixEntities;

        if (electricityGridMix.Count() != 0)
            RemovePeriodFacilityElectricityGridMix(periodFacilityId);

        foreach (var gridMixEntity in periodFacilityElectricityGridMixEntities)
        {
            gridMixEntity.CreatedBy = FindLoggedUser();
            gridMixEntity.CreatedOn = DateTime.UtcNow;
            _context.ReportingPeriodFacilityElectricityGridMixEntities.Add(gridMixEntity);
        }

        _context.LogExtensionMethod(_logger);
        return true;
    }

    public bool RemovePeriodFacilityElectricityGridMix(int periodFacilityId)
    {
        var existingFacility = _context.ReportingPeriodFacilityElectricityGridMixEntities.Where(x => x.ReportingPeriodFacilityId == periodFacilityId).ToList();
        foreach (var facility in existingFacility)
        {
            if (facility == null)
                throw new Exception("PeriodFacility is not found !!");

            _context.ReportingPeriodFacilityElectricityGridMixEntities.Remove(facility);
        }
        _context.LogExtensionMethod(_logger);
        return true;
    }

    #endregion

    #region PeriodFacilityGasSupplyBreakDown

    public bool AddPeriodFacilityGasSupplyBreakdown(IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownEntity> periodFacilityGasSupplyBreakDownEntities, int periodSupplierId)
    {
        FindSupplierAndCheckSupplierContactLoginOrNot(periodSupplierId);

        RemovePeriodFacilityGasSupplyBreakdown(periodSupplierId);

        foreach (var gasSupplyBreakdown in periodFacilityGasSupplyBreakDownEntities)
        {
            if (gasSupplyBreakdown == null)
                throw new Exception("GasSupplyBreakdownEntity is not found !!");

            gasSupplyBreakdown.CreatedOn = DateTime.UtcNow;
            gasSupplyBreakdown.CreatedBy = FindLoggedUser();
            _context.ReportingPeriodFacilityGasSupplyBreakDownEntities.Add(gasSupplyBreakdown);
        }
        _context.LogExtensionMethod(_logger);
        return true;
    }

    public bool RemovePeriodFacilityGasSupplyBreakdown(int periodSupplierId)
    {
        var periodSupplierEntity = _context.ReportingPeriodSupplierEntities.Where(x => x.Id == periodSupplierId).FirstOrDefault();
        var periodFacilityEntities = periodSupplierEntity.ReportingPeriodFacilityEntities;
        var periodFacilityGasSupplyBreakdownEntities = _context.ReportingPeriodFacilityGasSupplyBreakDownEntities;

        foreach (var periodFacility in periodFacilityEntities)
        {
            var existingEntity = periodFacilityGasSupplyBreakdownEntities.Where(x => x.PeriodFacilityId == periodFacility.Id).ToList();

            if (existingEntity.Count() != 0)
                _context.ReportingPeriodFacilityGasSupplyBreakDownEntities.RemoveRange(existingEntity);

        }

        _context.LogExtensionMethod(_logger);
        return true;
    }

    #endregion

    #region PeriodFacilityDocument

    private void FindPeriodFacilityAndCheckSupplierContactLoginOrNot(int periodFacilityId)
    {
        var periodFacility = _context.ReportingPeriodFacilityEntities.Where(x => x.Id == periodFacilityId).Include(x => x.ReportingPeriodSupplier).ThenInclude(x => x.Supplier).FirstOrDefault();
        var loginUser = FindLoggedUser();
        var contacts = periodFacility.ReportingPeriodSupplier.Supplier.ContactEntities;

        var isUserExists = contacts.Any(x => x.User.Email == loginUser);
        if (!isUserExists)
            throw new Exception($"You are not authorized for {periodFacility.ReportingPeriodSupplier.Supplier.Name} !!");
    }

    public bool AddUpdateReportingPeriodFacilityDocument(ReportingPeriodFacilityDocumentEntity reportingPeriodFacilityDocumentEntity)
    {
        FindPeriodFacilityAndCheckSupplierContactLoginOrNot(reportingPeriodFacilityDocumentEntity.ReportingPeriodFacilityId);

        var existingPeriodFacilityDocument = _context.ReportingPeriodFacilityDocumentEntities.Where(x => x.DocumentTypeId == reportingPeriodFacilityDocumentEntity.DocumentTypeId && x.ReportingPeriodFacilityId == reportingPeriodFacilityDocumentEntity.ReportingPeriodFacilityId).FirstOrDefault();

        if (existingPeriodFacilityDocument is null)
        {
            reportingPeriodFacilityDocumentEntity.CreatedBy = FindLoggedUser();
            reportingPeriodFacilityDocumentEntity.CreatedOn = DateTime.UtcNow;            
            _context.ReportingPeriodFacilityDocumentEntities.Add(reportingPeriodFacilityDocumentEntity);
        }
        else
        {
            existingPeriodFacilityDocument.ReportingPeriodFacilityId = reportingPeriodFacilityDocumentEntity.ReportingPeriodFacilityId;
            existingPeriodFacilityDocument.Version = reportingPeriodFacilityDocumentEntity.Version;
            existingPeriodFacilityDocument.DisplayName = reportingPeriodFacilityDocumentEntity.DisplayName;
            existingPeriodFacilityDocument.StoredName = reportingPeriodFacilityDocumentEntity.StoredName;
            existingPeriodFacilityDocument.Path = reportingPeriodFacilityDocumentEntity.Path;
            existingPeriodFacilityDocument.DocumentStatusId = reportingPeriodFacilityDocumentEntity.DocumentStatusId;
            existingPeriodFacilityDocument.DocumentTypeId = reportingPeriodFacilityDocumentEntity.DocumentTypeId;
            existingPeriodFacilityDocument.ValidationError = reportingPeriodFacilityDocumentEntity.ValidationError;
            existingPeriodFacilityDocument.UpdatedBy = FindLoggedUser();
            existingPeriodFacilityDocument.UpdatedOn = DateTime.UtcNow;

            _context.ReportingPeriodFacilityDocumentEntities.Update(existingPeriodFacilityDocument);
        }

        _context.LogExtensionMethod(_logger);
        return true;
    }

    public bool RemovePeriodFacilityDocument(int documentId)
    {
        var periodFacilityDocument = _context.ReportingPeriodFacilityDocumentEntities.Where(x => x.Id == documentId).FirstOrDefault();

        var isDeletedFile = _uploadDocuments.DeleteFile(periodFacilityDocument.Path);

        if (isDeletedFile)
        {
            _context.ReportingPeriodFacilityDocumentEntities.Remove(periodFacilityDocument);
            _context.LogExtensionMethod(_logger);
            return true;
        }
        else
            throw new Exception("PeriodFacilityDocument is not deleted because file path is not exists !!");
    }

    #endregion

    #region DownloadAndDeleteFile

    public FileStreamResult DownloadFile(string path)
    {
        if (path is null)
            throw new Exception("Can't download document because path is null !!");

        FileInfo file = new FileInfo(path);
        var downloadPath = Path.Combine(Directory.GetCurrentDirectory(), path);
        var stream = new MemoryStream(System.IO.File.ReadAllBytes(downloadPath));

        return new FileStreamResult(stream, new MediaTypeHeaderValue("application/octet-stream"))
        {
            FileDownloadName = file.Name
        };
    }

    #endregion

    #region PeriodSupplierDocument

    private void FindSupplierAndCheckSupplierContactLoginOrNot(int periodSupplierId)
    {
        var periodSupplier = _context.ReportingPeriodSupplierEntities.Where(x => x.Id == periodSupplierId).Include(x => x.Supplier).FirstOrDefault();
        var loginUser = FindLoggedUser();
        var contacts = periodSupplier.Supplier.ContactEntities;

        var isUserExists = contacts.Any(x => x.User.Email == loginUser);
        if (!isUserExists)
            throw new Exception($"You are not authorized for {periodSupplier.Supplier.Name} !!");
    }

    public bool AddUpdateReportingPeriodSupplierDocument(ReportingPeriodSupplierDocumentEntity reportingPeriodSupplierDocumentEntity)
    {
        FindSupplierAndCheckSupplierContactLoginOrNot(reportingPeriodSupplierDocumentEntity.ReportingPeriodSupplierId);

        var existingPeriodSupplierDocument = _context.ReportingPeriodSupplierDocumentEntities.Where(x => x.DocumentTypeId == reportingPeriodSupplierDocumentEntity.DocumentTypeId && x.ReportingPeriodSupplierId == reportingPeriodSupplierDocumentEntity.ReportingPeriodSupplierId).FirstOrDefault();

        if (existingPeriodSupplierDocument is null)
        {
            reportingPeriodSupplierDocumentEntity.CreatedOn = DateTime.UtcNow;
            reportingPeriodSupplierDocumentEntity.CreatedBy = FindLoggedUser();
            _context.ReportingPeriodSupplierDocumentEntities.Add(reportingPeriodSupplierDocumentEntity);
        }
        else
        {
            existingPeriodSupplierDocument.ReportingPeriodSupplierId = reportingPeriodSupplierDocumentEntity.ReportingPeriodSupplierId;
            existingPeriodSupplierDocument.Version = reportingPeriodSupplierDocumentEntity.Version;
            existingPeriodSupplierDocument.DisplayName = reportingPeriodSupplierDocumentEntity.DisplayName;
            existingPeriodSupplierDocument.StoredName = reportingPeriodSupplierDocumentEntity.StoredName;
            existingPeriodSupplierDocument.Path = reportingPeriodSupplierDocumentEntity.Path;
            existingPeriodSupplierDocument.DocumentStatusId = reportingPeriodSupplierDocumentEntity.DocumentStatusId;
            existingPeriodSupplierDocument.DocumentTypeId = reportingPeriodSupplierDocumentEntity.DocumentTypeId;
            existingPeriodSupplierDocument.ValidationError = reportingPeriodSupplierDocumentEntity.ValidationError;
            existingPeriodSupplierDocument.UpdatedBy = FindLoggedUser();
            existingPeriodSupplierDocument.UpdatedOn = DateTime.UtcNow;

            _context.ReportingPeriodSupplierDocumentEntities.Update(existingPeriodSupplierDocument);
        }

        _context.LogExtensionMethod(_logger);
        return true;

    }

    public bool RemovePeriodSupplierDocument(int documentId)
    {
        var periodSupplierDocument = _context.ReportingPeriodSupplierDocumentEntities.Where(x => x.Id == documentId).FirstOrDefault();

        var isDeletedFile = _uploadDocuments.DeleteFile(periodSupplierDocument.Path);

        if (isDeletedFile)
        {
            _context.ReportingPeriodSupplierDocumentEntities.Remove(periodSupplierDocument);
            _context.LogExtensionMethod(_logger);
            return true;
        }
        else
            throw new Exception("Can't removed document because path is not exists !!");
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
        return _context.DocumentRequiredStatusEntities;
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

    public IEnumerable<FacilityRequiredDocumentTypeEntity> GetFacilityRequiredDocumentTypeEntities()
    {
        var facilityRequiredDocumentType = _context.FacilityRequiredDocumentTypeEntities
                                    .Include(x => x.ReportingType)
                                    .Include(x => x.SupplyChainStage)
                                    .Include(x => x.DocumentType)
                                    .Include(x => x.DocumentRequiredStatus)
                                    .ToList();
        return facilityRequiredDocumentType;
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

    public IEnumerable<EmailTemplateEntity> GetEmailTemplateBynameCode()
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
                                .Include(x => x.SupplierReportingPeriodStatus)
                                .Include(x => x.ReportingPeriodFacilityEntities)
                                    .ThenInclude(x => x.ReportingPeriodFacilityGasSupplyBreakDownEntities)
                                .Include(x => x.ReportingPeriodSupplierDocumentEntities)
                                .FirstOrDefault(x => x.Id == periodSupplierId);
        return periodSupplier;
    }

    public ReportingPeriodFacilityEntity GetPeriodFacilityById(int periodFacilityId)
    {
        var periodFacility = _context.ReportingPeriodFacilityEntities
                                .Include(x => x.ReportingPeriod)
                                .Include(x => x.ReportingPeriodSupplier)
                                .Include(x => x.ReportingPeriodFacilityElectricityGridMixEntities)
                                .Include(x => x.ReportingPeriodFacilityDocumentEntities)
                                .FirstOrDefault(x => x.Id == periodFacilityId);
        return periodFacility;
    }

    public ReportingPeriodFacilityDocumentEntity GetReportingPeriodFacilityDocumentById(int documentId)
    {
        var periodFacilityDocument = _context.ReportingPeriodFacilityDocumentEntities
                                .Include(x => x.ReportingPeriodFacility)
                                .Include(x => x.DocumentStatus)
                                .Include(x => x.DocumentType)
                                .FirstOrDefault(x => x.Id == documentId);
        return periodFacilityDocument;
    }

    public ReportingPeriodSupplierDocumentEntity GetReportingPeriodSupplierDocumentById(int documentId)
    {
        var periodSupplierDocument = _context.ReportingPeriodSupplierDocumentEntities
                                    .Include(x => x.ReportingPeriodSupplier)
                                    .Include(x => x.DocumentStatus)
                                    .Include(x => x.DocumentType)
                                    .FirstOrDefault(x => x.Id == documentId);
        return periodSupplierDocument;
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
