using DataAccess.DataActionContext;
using DataAccess.DataActions.Interfaces;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataActions;

public class ReportingPeriodDataActionsManager : IReportingPeriodDataActions
{
    private readonly SupplierPortalDBContext _context;
    private readonly string REPORTING_PERIOD_STATUS_CLOSE = "Closed";

    public ReportingPeriodDataActionsManager(SupplierPortalDBContext context)
    {
        _context = context;
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
        _context.SaveChanges();
        return true;
    }

    public bool AddPeriodSupplier(ReportingPeriodSupplierEntity reportingPeriodSupplierEntity)
    {
        reportingPeriodSupplierEntity.IsActive = true;
        _context.ReportingPeriodSupplierEntities.Add(reportingPeriodSupplierEntity);
        _context.SaveChanges();
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

        _context.SaveChanges();
        return true;
    }


    public bool AddPeriodFacilityElectricityGridMix(IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> periodFacilityElectricityGridMixEntities, int periodFacilityId, int fercRegionId)
    {
        var periodFacilityEntity = _context.ReportingPeriodFacilityEntities.Where(x => x.Id == periodFacilityId).Include(x => x.ReportingPeriodFacilityElectricityGridMixEntities).FirstOrDefault();

        periodFacilityEntity.FercRegionId = fercRegionId;

        var electricityGridMix = periodFacilityEntity.ReportingPeriodFacilityElectricityGridMixEntities;

        if (electricityGridMix.Count() != 0)
            RemovePeriodFacilityElectricityGridMix(periodFacilityId);

        foreach (var gridMixEntity in periodFacilityElectricityGridMixEntities)
        {
            gridMixEntity.CreatedBy = "System";
            gridMixEntity.CreatedOn = DateTime.UtcNow;
            _context.ReportingPeriodFacilityElectricityGridMixEntities.Add(gridMixEntity);
        }

        _context.SaveChanges();
        return true;
    }

    public bool AddPeriodFacilityGasSupplyBreakdown(IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownEntity> periodFacilityGasSupplyBreakDownEntities, int periodSupplierId)
    {
        RemovePeriodFacilityGasSupplyBreakdown(periodSupplierId);

        foreach (var gasSupplyBreakdown in periodFacilityGasSupplyBreakDownEntities)
        {
            if (gasSupplyBreakdown == null)
                throw new Exception("GasSupplyBreakdownEntity is not found !!");

            gasSupplyBreakdown.CreatedOn = DateTime.UtcNow;
            gasSupplyBreakdown.CreatedBy = "System";
            _context.ReportingPeriodFacilityGasSupplyBreakDownEntities.Add(gasSupplyBreakdown);
        }
        _context.SaveChanges();
        return true;
    }

    public bool AddUpdateReportingPeriodFacilityDocument(ReportingPeriodFacilityDocumentEntity reportingPeriodFacilityDocumentEntity)
    {
        var existingPeriodFacilityDocument = _context.ReportingPeriodFacilityDocumentEntities.Where(x => x.DocumentTypeId == reportingPeriodFacilityDocumentEntity.DocumentTypeId && x.ReportingPeriodFacilityId == reportingPeriodFacilityDocumentEntity.ReportingPeriodFacilityId).FirstOrDefault();
        
        if (existingPeriodFacilityDocument is null)
        {
            reportingPeriodFacilityDocumentEntity.CreatedBy = "System";
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
            existingPeriodFacilityDocument.UpdatedBy = "System";
            existingPeriodFacilityDocument.UpdatedOn = DateTime.UtcNow;

            _context.ReportingPeriodFacilityDocumentEntities.Update(existingPeriodFacilityDocument);
        }

        _context.SaveChanges();
        return true;
    }

    public async Task<bool> AddReportingPeriodSupplierDocument(ReportingPeriodSupplierDocumentEntity reportingPeriodSupplierDocument)
    {
        await _context.ReportingPeriodSupplierDocumentEntities.AddAsync(reportingPeriodSupplierDocument);

        reportingPeriodSupplierDocument.CreatedBy = "System";
        reportingPeriodSupplierDocument.CreatedOn = DateTime.UtcNow;

        await _context.SaveChangesAsync();
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
            reportingPeriodEntity.ReportingPeriodSupplierEntities = UpdateReportingPeriodSuppliers(reportingPeriod.ReportingPeriodSupplierEntities).ToList();
        }

        reportingPeriodEntity.UpdatedOn = DateTime.UtcNow;
        reportingPeriodEntity.UpdatedBy = "System";

        _context.ReportingPeriodEntities.Update(reportingPeriodEntity);
        _context.SaveChanges();
        return true;
    }

    public async Task<bool> UpdateReportingPeriodSupplierDocument(ReportingPeriodSupplierDocumentEntity reportingPeriodSupplierDocument)
    {
        var existingsupplierdocument = await _context.ReportingPeriodSupplierDocumentEntities.FirstOrDefaultAsync(x => x.Id == reportingPeriodSupplierDocument.Id);

        if (existingsupplierdocument == null)
        {
            throw new Exception("Supplier Document Not found");
        }

        existingsupplierdocument.ReportingPeriodSupplierId = reportingPeriodSupplierDocument.ReportingPeriodSupplierId;
        existingsupplierdocument.Version = reportingPeriodSupplierDocument.Version;
        existingsupplierdocument.DisplayName = reportingPeriodSupplierDocument.DisplayName;
        existingsupplierdocument.StoredName = reportingPeriodSupplierDocument.StoredName;
        existingsupplierdocument.Path = reportingPeriodSupplierDocument.Path;
        existingsupplierdocument.DocumentStatusId = reportingPeriodSupplierDocument.DocumentStatusId;
        existingsupplierdocument.DocumentTypeId = reportingPeriodSupplierDocument.DocumentTypeId;
        existingsupplierdocument.ValidationError = reportingPeriodSupplierDocument.ValidationError;
        existingsupplierdocument.UpdatedBy = "System";
        existingsupplierdocument.UpdatedOn = DateTime.UtcNow;

        _context.ReportingPeriodSupplierDocumentEntities.Update(existingsupplierdocument);
        await _context.SaveChangesAsync();

        return true;
    }

    /*  public ReportingPeriodSupplierEntity UpdatePeriodSupplier(ReportingPeriodSupplierEntity periodSupplierEntity)
      {
          var periodSupplier = _context.ReportingPeriodSupplierEntities
              .Where(x => x.Id == periodSupplierEntity.Id).FirstOrDefault();

          if (periodSupplier == null)
              throw new Exception("ReportingPeriodSupplierEntity not found for update !!");

          periodSupplier.SupplierReportingPeriodStatusId = periodSupplierEntity.SupplierReportingPeriodStatusId;

          _context.ReportingPeriodSupplierEntities.Update(periodSupplier);
          _context.SaveChanges();
          return periodSupplier;
      }
    */
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

        _context.SaveChanges();
        return updatedReportingPeriodSuppliers;
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

        _context.SaveChanges();
        return updatedPeriodFacilities;
    }

    public bool UpdateReportingPeriodFacilityDataStatus(int periodFacilityId,int periodFacilityDataStatusId)
    {
        var periodFacility = _context.ReportingPeriodFacilityEntities.Where(x => x.Id == periodFacilityId).Include(x => x.ReportingPeriodFacilityDocumentEntities).FirstOrDefault();

        periodFacility.FacilityReportingPeriodDataStatusId = periodFacilityDataStatusId;

        _context.ReportingPeriodFacilityEntities.Update(periodFacility);

        _context.SaveChanges();
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
        _context.SaveChanges();
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
        //_context.SaveChanges();
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

        _context.SaveChanges();
        return true;
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

    #endregion

    #region GetById
    public ReportingPeriodEntity GetReportingPeriodById(int reportingPeriodId)
    {
        var reportingPeriod = _context.ReportingPeriodEntities
                                .Where(x => x.Id == reportingPeriodId)
                                .Include(x => x.ReportingPeriodType)
                                .Include(x => x.ReportingPeriodStatus)
                                .Include(x => x.ReportingPeriodSupplierEntities)
                                .Include(x => x.ReportingPeriodFacilityEntities)
                                    .ThenInclude(x => x.ReportingPeriodFacilityElectricityGridMixEntities)
                                .Include(x => x.ReportingPeriodFacilityEntities)
                                    .ThenInclude(x => x.ReportingPeriodFacilityGasSupplyBreakDownEntities)
                                .Include(x => x.ReportingPeriodFacilityEntities)
                                    .ThenInclude(x => x.ReportingPeriodFacilityDocumentEntities)
                                .FirstOrDefault();

        return reportingPeriod;
    }

    public IEnumerable<ReportingPeriodTypeEntity> GetReportingPeriodTypeById(int reportingPeriodTypeId)
    {
        var reportingPeriodType = _context.ReportingPeriodTypeEntities.Where(x => x.Id == reportingPeriodTypeId).ToList();

        return reportingPeriodType;
    }

    public IEnumerable<ReportingPeriodStatusEntity> GetReportingPeriodStatusById(int reportingPeriodStatusId)
    {
        var reportingPeriodStatus = _context.ReportingPeriodStatusEntities.Where(x => x.Id == reportingPeriodStatusId).ToList();
        return reportingPeriodStatus;
    }

    public IEnumerable<ReportingPeriodSupplierEntity> GetReportingPeriodSuppliers(int reportingPeriodId)
    {
        var reportingPeriodSupplier =
            _context.ReportingPeriodSupplierEntities
                                    .Where(x => x.ReportingPeriodId == reportingPeriodId)
                                    .Include(x => x.Supplier)
                                    .Include(x => x.ReportingPeriod)
                                    .Include(x => x.SupplierReportingPeriodStatus)
                                    .ToList();
        return reportingPeriodSupplier;

    }

    public ReportingPeriodSupplierEntity GetPeriodSupplierById(int periodSupplierId)
    {
        var periodSupplier = _context.ReportingPeriodSupplierEntities
                                .Include(x => x.Supplier)
                                .Include(x => x.ReportingPeriod)
                                .Include(x => x.SupplierReportingPeriodStatus)
                                .Include(x => x.ReportingPeriodFacilityEntities)
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
    public async Task<IEnumerable<ReportingPeriodFacilityEntity>> GetReportingPeriodFacilities(int SupplierId, int ReportingPeriodId)
    {
        return await _context.ReportingPeriodFacilityEntities
                                    .Include(x => x.Facility)
                                    .Include(x => x.FacilityReportingPeriodDataStatus)
                                    .Include(x => x.ReportingPeriod)
                                    .Include(x => x.ReportingPeriodSupplier)
                                    .ToListAsync();
    }

    public async Task<IEnumerable<ReportingPeriodFacilityDocumentEntity>> GetReportingPeriodFacilitiesDocument(int DocumentId)
    {
        return await _context.ReportingPeriodFacilityDocumentEntities
                                .Include(x => x.ReportingPeriodFacility)
                                .Include(x => x.DocumentStatus)
                                .Include(x => x.DocumentType)
                                .ToListAsync();
    }

    public async Task<IEnumerable<ReportingPeriodSupplierDocumentEntity>> GetReportingPeriodSuppliersDocument(int DocumentId)
    {
        return await _context.ReportingPeriodSupplierDocumentEntities
                                    .Include(x => x.ReportingPeriodSupplier)
                                    .Include(x => x.DocumentStatus)
                                    .Include(x => x.DocumentType)
                                    .ToListAsync();
    }

    public async Task<IEnumerable<ReportingPeriodEntity>> GetReportingPeriods(int ReportingPeriodId)
    {
        return await _context.ReportingPeriodEntities
                                .Include(x => x.ReportingPeriodType)
                                .Include(x => x.ReportingPeriodStatus)
                                .ToListAsync();
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
