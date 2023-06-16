using DataAccess.DataActionContext;
using DataAccess.DataActions.Interfaces;
using DataAccess.Entities;
using DataAccess.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace DataAccess.DataActions;

public class ReportingPeriodDataActionsManager : IReportingPeriodDataActions
{
    private readonly SupplierPortalDBContext _context;
    private readonly string REPORTING_PERIOD_STATUS_CLOSE = "Closed";
    private readonly ILogging _logger;
    public ReportingPeriodDataActionsManager(SupplierPortalDBContext context,ILogging logger)
    {
        _context = context;
        _logger = logger;
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
        entity.CreatedBy = "System";
        entity.CreatedOn = DateTime.UtcNow;

        _context.ReportingPeriodEntities.Add(entity);
        //_context.SaveChanges();
        _context.LogExtensionMethod(_logger);
        return true;
    }
    public bool AddRemovePeriodSupplier(IEnumerable<ReportingPeriodSupplierEntity> reportingPeriodSupplierEntity, int id)
    {
        var allSuppliers = _context.ReportingPeriodSupplierEntities.Where(x => x.ReportingPeriodId == id);

        //Add PeriodSupplier
        foreach (var entity in reportingPeriodSupplierEntity)
        {
            var isExist = allSuppliers.FirstOrDefault(x => x.SupplierId == entity.SupplierId);

            if (isExist is null)
            {
                if (entity.Id == 0)
                {
                    _context.ReportingPeriodSupplierEntities.Add(entity);
                }
            }
        }
        //Remove PeriodSupplier
        foreach (var entity in allSuppliers)
        {
            var isExist = reportingPeriodSupplierEntity.Where(x => x.SupplierId
                == entity.SupplierId /*&& x.ReportingPeriodId == entity.ReportingPeriodId*/ ).FirstOrDefault();

            if (isExist is null)
            {
                _context.ReportingPeriodSupplierEntities.Remove(entity);
            }
        }



        _context.LogExtensionMethod(_logger);
        //_context.SaveChanges();
        return true;
    }
    public bool AddRemovePeriodFacility(IEnumerable<ReportingPeriodFacilityEntity> reportingPeriodFacilityEntity, int periodSupplierId)
    {
        var allPeriodfacilities = _context.ReportingPeriodFacilityEntities.Where(x => x.ReportingPeriodSupplierId == periodSupplierId);

        foreach (var entity in reportingPeriodFacilityEntity)
        {
            var isExist = allPeriodfacilities.FirstOrDefault(x => x.FacilityId == entity.FacilityId);
            if (entity.Id == 0 && isExist is null)
            {

                _context.ReportingPeriodFacilityEntities.Add(entity);
            }

        }

        foreach (var existingPeriodFacility in allPeriodfacilities)
        {
            var isExist = reportingPeriodFacilityEntity.FirstOrDefault(x => x.Id == existingPeriodFacility.Id);
            if (isExist is null)
            {
                _context.Remove(existingPeriodFacility);
            }

        }
        _context.LogExtensionMethod(_logger);
        //_context.SaveChanges();
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
        //_context.SaveChanges();
        return updatedPeriodFacilities;
    }
    public bool UpdateReportingPeriodFacilityDataStatus(int periodFacilityId, int periodFacilityDataStatusId)
    {
        var periodFacility = _context.ReportingPeriodFacilityEntities.Where(x => x.Id == periodFacilityId).Include(x => x.ReportingPeriodFacilityDocumentEntities).FirstOrDefault();

        periodFacility.FacilityReportingPeriodDataStatusId = periodFacilityDataStatusId;

        _context.ReportingPeriodFacilityEntities.Update(periodFacility);

        _context.LogExtensionMethod(_logger);
        //_context.SaveChanges();
        return true;
    }
    public bool AddPeriodFacilityElectricityGridMix(IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> periodFacilityElectricityGridMixEntity, int periodFacilityId, int fercRegionId)
    {

        var electicityGridMixEntities = _context.ReportingPeriodFacilityElectricityGridMixEntities.Where(x => x.ReportingPeriodFacilityId == periodFacilityId);

        var periodFacilityEntity = _context.ReportingPeriodFacilityEntities.FirstOrDefault(x => x.Id == periodFacilityId);



        periodFacilityEntity.FercRegionId = fercRegionId; //Update FercRegionId in PeriodFacility

        _context.ReportingPeriodFacilityElectricityGridMixEntities.RemoveRange(electicityGridMixEntities);
        _context.ReportingPeriodFacilityElectricityGridMixEntities.AddRange(periodFacilityElectricityGridMixEntity);
        //_context.SaveChanges();

        _context.LogExtensionMethod(_logger);
        return true;
    }
    public bool AddPeriodFacilityGasSupplyBreakdown(IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownEntity> periodFacilityGasSupplyBreakDownEntities, int periodSupplierId)
    {
        var periodFacilityEntities = _context.ReportingPeriodFacilityEntities.Where(x => x.ReportingPeriodSupplierId == periodSupplierId);


        foreach (var periodFacility in periodFacilityEntities)
        {
            var existingEntity = periodFacility.ReportingPeriodFacilityGasSupplyBreakdownEntities.Where(x => x.PeriodFacilityId == periodFacility.Id).ToList();

            if (existingEntity.Count() != 0)
                _context.ReportingPeriodFacilityGasSupplyBreakDownEntities.RemoveRange(existingEntity);
        }


        foreach (var gasSupplyBreakdownList in periodFacilityGasSupplyBreakDownEntities)
        {
            if (gasSupplyBreakdownList == null)
                throw new Exception("PeriodFacilityGasSupplyBreakdown is not found !!");
            _context.ReportingPeriodFacilityGasSupplyBreakDownEntities.Add(gasSupplyBreakdownList);
        }

        _context.LogExtensionMethod(_logger);
        //_context.SaveChanges();
        return true;


    }
    public int AddReportingPeriodFacilityDocument(ReportingPeriodFacilityDocumentEntity reportingPeriodFacilityDocument)
    {
        var isExist = _context.ReportingPeriodFacilityDocumentEntities.Where(x => x.Id == reportingPeriodFacilityDocument.Id && x.ReportingPeriodFacilityId == reportingPeriodFacilityDocument.ReportingPeriodFacilityId).FirstOrDefault();


        if (isExist is null)
        {
            reportingPeriodFacilityDocument.CreatedBy = "System";
            reportingPeriodFacilityDocument.CreatedOn = DateTime.UtcNow;
            _context.ReportingPeriodFacilityDocumentEntities.Add(reportingPeriodFacilityDocument);
        }
        else
        {
            isExist.ReportingPeriodFacilityId = reportingPeriodFacilityDocument.ReportingPeriodFacilityId;
            isExist.Version = reportingPeriodFacilityDocument.Version;
            isExist.DisplayName = reportingPeriodFacilityDocument.DisplayName;
            isExist.StoredName = reportingPeriodFacilityDocument.StoredName;
            isExist.Path = reportingPeriodFacilityDocument.Path;
            isExist.DocumentStatusId = reportingPeriodFacilityDocument.DocumentStatusId;
            isExist.DocumentTypeId = reportingPeriodFacilityDocument.DocumentTypeId;
            isExist.ValidationError = reportingPeriodFacilityDocument.ValidationError;
            isExist.UpdatedBy = "System";
            isExist.UpdatedOn = DateTime.UtcNow;

            _context.ReportingPeriodFacilityDocumentEntities.Update(isExist);
        }

        _context.LogExtensionMethod(_logger);
       // _context.SaveChanges();
        return reportingPeriodFacilityDocument.Id;
    }
    public int AddReportingPeriodSupplierDocument(ReportingPeriodSupplierDocumentEntity reportingPeriodSupplierDocument)
    {
        var isExist = _context.ReportingPeriodSupplierDocumentEntities.Where(x => x.Id == reportingPeriodSupplierDocument.Id && x.ReportingPeriodSupplierId == reportingPeriodSupplierDocument.ReportingPeriodSupplierId).FirstOrDefault();


        if (isExist == null)
        {
            reportingPeriodSupplierDocument.CreatedBy = "System";
            reportingPeriodSupplierDocument.CreatedOn = DateTime.UtcNow;
            _context.ReportingPeriodSupplierDocumentEntities.Add(reportingPeriodSupplierDocument);
        }
        else
        {
            isExist.ReportingPeriodSupplierId = reportingPeriodSupplierDocument.ReportingPeriodSupplierId;
            isExist.Version = reportingPeriodSupplierDocument.Version;
            isExist.DisplayName = reportingPeriodSupplierDocument.DisplayName;
            isExist.StoredName = reportingPeriodSupplierDocument.StoredName;
            isExist.Path = reportingPeriodSupplierDocument.Path;
            isExist.DocumentStatusId = reportingPeriodSupplierDocument.DocumentStatusId;
            isExist.DocumentTypeId = reportingPeriodSupplierDocument.DocumentTypeId;
            isExist.ValidationError = reportingPeriodSupplierDocument.ValidationError;
            isExist.UpdatedBy = "System";
            isExist.UpdatedOn = DateTime.UtcNow;

            _context.ReportingPeriodSupplierDocumentEntities.Update(isExist);
        }

        _context.LogExtensionMethod(_logger);
        //_context.SaveChanges();
        return reportingPeriodSupplierDocument.Id;
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
            reportingPeriodEntity.ReportingPeriodSupplierEntities = UpdateReportingPeriodSuppliers(reportingPeriod.ReportingPeriodSupplierEntities).ToList();
        }

        reportingPeriodEntity.UpdatedOn = DateTime.UtcNow;
        reportingPeriodEntity.UpdatedBy = "System";

        _context.ReportingPeriodEntities.Update(reportingPeriodEntity);
        //_context.SaveChanges();
        _context.LogExtensionMethod(_logger);
        return true;
    }
    public IEnumerable<ReportingPeriodSupplierEntity> UpdateReportingPeriodSuppliers(IEnumerable<ReportingPeriodSupplierEntity> periodSuppliers)
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
        //_context.SaveChanges();
        _context.LogExtensionMethod(_logger);
        return updatedReportingPeriodSuppliers;
    }

    #endregion

    #region Remove Methods
    public bool RemovePeriodFacilityElectricityGridMix(int periodFacilityId)
    {
        var existingFacility = _context.ReportingPeriodFacilityElectricityGridMixEntities.Where(x => x.ReportingPeriodFacilityId == periodFacilityId).ToList();
        foreach (var facility in existingFacility)
        {
            if (facility == null)
                throw new Exception("PeriodFacility is not found !!");

            _context.ReportingPeriodFacilityElectricityGridMixEntities.Remove(facility);
        }
        //_context.SaveChanges();
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
        //_context.SaveChanges();
        _context.LogExtensionMethod(_logger);
        return true;
    }
    public bool RemovePeriodFacilityDocument(int documentId)
    {
        var periodFacilityDocument = _context.ReportingPeriodFacilityDocumentEntities.Where(x => x.Id == documentId).FirstOrDefault();

        var isDeletedFile = DeleteFile(periodFacilityDocument.Path);
        _context.ReportingPeriodFacilityDocumentEntities.Remove(periodFacilityDocument);
        //_context.SaveChanges();
        _context.LogExtensionMethod(_logger);
        return true;
        //if (isDeletedFile)
        //{
            
        //}
        //else
        //    throw new Exception("File Path is Required to remove periodFacilityDocument..!");
    }
    public bool RemovePeriodSupplierDocument(int documentId)
    {
        var periodSupplierDocument=_context.ReportingPeriodSupplierDocumentEntities.Where(x=>x.Id==documentId).FirstOrDefault();

        var isDeletedFile = DeleteFile(periodSupplierDocument.Path);
        _context.ReportingPeriodSupplierDocumentEntities.Remove(periodSupplierDocument);
        //_context.SaveChanges();
        _context.LogExtensionMethod(_logger);
        return true;
    }
    private bool DeleteFile(string path)
    {
        FileInfo file = new FileInfo(path);
        if (file.Exists)
        {
            file.Delete();
            return true;
        }
        else
            return false;
    }

    #endregion

    #region SendMail
    public bool SendInitialAndResendDataRequestEmail(int periodSupplierId)
    {
        var periodSupplierEntity = _context.ReportingPeriodSupplierEntities.Where(x => x.Id == periodSupplierId).FirstOrDefault();

        if (periodSupplierEntity is null)
            throw new Exception("PeriodSupplierEntity is not found !!");

        if (periodSupplierEntity.InitialDataRequestDate is null)
            periodSupplierEntity.InitialDataRequestDate = DateTime.UtcNow;
        else
            periodSupplierEntity.ResendDataRequestDate = DateTime.UtcNow;

        _context.ReportingPeriodSupplierEntities.Update(periodSupplierEntity);
        _context.SaveChanges();
        return true;

    }
    #endregion

    #region GetAll Methods
    public IEnumerable<SupplierEntity> GetSuppliers(IEnumerable<int> id)
    {
        var suppliers = new List<SupplierEntity>();

        foreach (var supplierId in id)
        {
            var supplier = _context.SupplierEntities.Include(x => x.FacilityEntities).FirstOrDefault(x => x.Id == supplierId /*&& x.IsActive*/);
            suppliers.Add(supplier);
        }
        return suppliers;
    }
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
    public IEnumerable<DocumentStatusEntity> GetDocumentStatus()
    {
        return _context.DocumentStatusEntities.ToList();
    }
    public IEnumerable<DocumentTypeEntity> GetDocumentType()
    {
        return _context.DocumentTypeEntities.ToList();
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
    public IEnumerable<EmailTemplateEntity> GetEmailTemplateBynameCode()
    {
        return _context.EmailTemplateEntities;
    }
    public IEnumerable<EmailTemplateEntity> GetEmailBlueprints()
    {
        return _context.EmailTemplateEntities;
    }
    #endregion

    #region GetById
    public ReportingPeriodEntity GetReportingPeriodById(int reportingPeriodId)
    {
        var reportingPeriod = _context.ReportingPeriodEntities
                                .Where(x => x.Id == reportingPeriodId)
                                .Include(x => x.ReportingPeriodSupplierEntities)
                                    .ThenInclude(x => x.ReportingPeriodSupplierDocumentEntities)
                                .Include(x => x.ReportingPeriodFacilityEntities)
                                    .ThenInclude(x => x.ReportingPeriodFacilityElectricityGridMixEntities)
                                .Include(x => x.ReportingPeriodFacilityEntities)
                                    .ThenInclude(x => x.ReportingPeriodFacilityGasSupplyBreakdownEntities)
                                .Include(x => x.ReportingPeriodFacilityEntities)
                                    .ThenInclude(x => x.ReportingPeriodFacilityDocumentEntities)
                                .FirstOrDefault();

        return reportingPeriod;
    }
    public ReportingPeriodSupplierEntity GetPeriodSupplierById(int periodSupplierId)
    {
        var periodSupplier = _context.ReportingPeriodSupplierEntities
                                .Include(x => x.Supplier)
                                .Include(x => x.ReportingPeriod)
                                .Include(x => x.ReportingPeriodFacilityEntities)
                                    .ThenInclude(x => x.ReportingPeriodFacilityGasSupplyBreakdownEntities)
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
                                    .FirstOrDefault(x => x.Id == documentId);
        return periodFacilityDocument;
    }
    public ReportingPeriodSupplierDocumentEntity GetReportingPeriodSuppliersDocumentById(int documentId)
    {
        var periodSupplierDocument = _context.ReportingPeriodSupplierDocumentEntities
                                     .Include(x=>x.ReportingPeriodSupplier)
                                     .FirstOrDefault(x=>x.Id== documentId);
        return periodSupplierDocument;
    }
    public ReportingPeriodFacilityEntity GetReportingPeriodFacility(int periodFacilityId)
    {
        var periodFacility = _context.ReportingPeriodFacilityEntities.Include(x => x.ReportingPeriodSupplier)
                                                                      .Include(x => x.ReportingPeriod)
                                                                      .Include(x => x.ReportingPeriodFacilityElectricityGridMixEntities)
                                                                      .Include(x => x.FercRegion)
                                                                      .FirstOrDefault(x => x.Id == periodFacilityId);


        return periodFacility;
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
