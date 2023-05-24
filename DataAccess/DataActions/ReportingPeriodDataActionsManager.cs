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




        _context.SaveChanges();
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
        _context.SaveChanges();
        return true;
    }


    public bool AddPeriodFacilityElectricityGridMix(IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> periodFacilityElectricityGridMixEntity,int periodFacilityId,int fercRegionId)
    {
        // periodFacility = GetReportingPeriodFacility(periodFacilityId);

        var electicityGridMixEntities = _context.ReportingPeriodFacilityElectricityGridMixEntities.Where(x => x.ReportingPeriodFacilityId == periodFacilityId);

        var periodFacilityEntity = _context.ReportingPeriodFacilityEntities.FirstOrDefault(x => x.Id == periodFacilityId);

        periodFacilityEntity.FercRegionId=fercRegionId; //Update FercRegionId in PeriodFacility

        _context.ReportingPeriodFacilityElectricityGridMixEntities.RemoveRange(electicityGridMixEntities);

        _context.ReportingPeriodFacilityElectricityGridMixEntities.AddRange(periodFacilityElectricityGridMixEntity); 

        _context.SaveChanges();


        return true;
    }

    public async Task<bool> AddReportingPeriodFacilityDocument(ReportingPeriodFacilityDocumentEntity reportingPeriodFacilityDocument)
    {
        await _context.ReportingPeriodFacilityDocumentEntities.AddAsync(reportingPeriodFacilityDocument);

        reportingPeriodFacilityDocument.CreatedBy = "System";
        reportingPeriodFacilityDocument.CreatedOn = DateTime.UtcNow;

        await _context.SaveChangesAsync();
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



    public async Task<bool> UpdateReportingPeriodFacilityDocument(ReportingPeriodFacilityDocumentEntity reportingPeriodFacilityDocument)
    {
        var existingfacilitydocument = await _context.ReportingPeriodFacilityDocumentEntities.FirstOrDefaultAsync(x => x.Id == reportingPeriodFacilityDocument.Id);

        if (existingfacilitydocument == null)
        {
            throw new Exception("Facility Document Not found");
        }

        existingfacilitydocument.ReportingPeriodFacilityId = reportingPeriodFacilityDocument.ReportingPeriodFacilityId;
        existingfacilitydocument.Version = reportingPeriodFacilityDocument.Version;
        existingfacilitydocument.DisplayName = reportingPeriodFacilityDocument.DisplayName;
        existingfacilitydocument.StoredName = reportingPeriodFacilityDocument.StoredName;
        existingfacilitydocument.Path = reportingPeriodFacilityDocument.Path;
        existingfacilitydocument.DocumentStatusId = reportingPeriodFacilityDocument.DocumentStatusId;
        existingfacilitydocument.DocumentTypeId = reportingPeriodFacilityDocument.DocumentTypeId;
        existingfacilitydocument.ValidationError = reportingPeriodFacilityDocument.ValidationError;
        existingfacilitydocument.UpdatedBy = "System";
        existingfacilitydocument.UpdatedOn = DateTime.UtcNow;

        _context.ReportingPeriodFacilityDocumentEntities.Update(existingfacilitydocument);
        await _context.SaveChangesAsync();

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

    #endregion

    #region Remove Methods


    public bool RemovePeriodFacilityElectricityGridMix(int periodFacilityElectricityGridMixId)
    {
        var entity = _context.ReportingPeriodFacilityElectricityGridMixEntities.FirstOrDefault(x => x.Id == periodFacilityElectricityGridMixId);
        if (entity == null)
            throw new Exception("RemovePeriodFacilityElectricityGridMix not found !!");

        _context.ReportingPeriodFacilityElectricityGridMixEntities.Remove(entity);
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
                                /*.ThenInclude(x => x.ReportingPeriodFacilityElectricityGridMixEntities)*/
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

    public async Task<IEnumerable<ReportingPeriodFacilityEntity>> GetReportingPeriodFacilities(int SupplierId, int ReportingPeriodId)
    {
        return await _context.ReportingPeriodFacilityEntities
                                    .Include(x => x.Facility)
                                    .Include(x => x.FacilityReportingPeriodDataStatus)
                                    .Include(x => x.ReportingPeriod)
                                    .Include(x => x.ReportingPeriodSupplier)
                                    .ToListAsync();
    }

    public async Task<IEnumerable<ReportingPeriodFacilityEntity>> GetReportingPeriodFacilities()
    {
        return await _context.ReportingPeriodFacilityEntities.Include(x => x.Facility)
            .Include(x => x.FacilityReportingPeriodDataStatus)
            .Include(x => x.ReportingPeriod)
            .Include(x => x.ReportingPeriodSupplier).ToListAsync();

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

    public ReportingPeriodFacilityEntity GetReportingPeriodFacility(int periodFacilityId)
    {
        var periodFacility = _context.ReportingPeriodFacilityEntities.Include(x => x.ReportingPeriodSupplier)
                                                                      .Include(x => x.ReportingPeriod)
                                                                      .Include(x => x.ReportingPeriodFacilityElectricityGridMixEntities)
                                                                      .Include(x=>x.FercRegion)
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
