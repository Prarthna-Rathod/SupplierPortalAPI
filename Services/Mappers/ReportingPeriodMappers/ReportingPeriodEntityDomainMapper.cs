using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using DataAccess.Entities;
using Services.Mappers.Interfaces;


namespace Services.Mappers.ReportingPeriodMappers;

public class ReportingPeriodEntityDomainMapper : IReportingPeriodEntityDomainMapper
{

    //public PeriodSupplier ConvertPeriodSuppliersEntityToDomain(ReportingPeriod reportingPeriod, ReportingPeriodSupplierEntity reportingPeriodSupplierEntity, IEnumerable<SupplierReportingPeriodStatus> supplierReportingPeriodStatuses, SupplierVO supplierVO)
    //{
    //    var supplierReportingPeriodSelectedStatus = supplierReportingPeriodStatuses.Where(x => x.Id == reportingPeriodSupplierEntity.SupplierReportingPeriodStatusId).FirstOrDefault();

    //    var periodSupplier = new PeriodSupplier(reportingPeriodSupplierEntity.Id, supplierVO, reportingPeriod.Id, supplierReportingPeriodSelectedStatus);

    //    return periodSupplier;
    //}

    /* Commented by Prarthna
    public void ConvertPeriodSupplierEntityToDomain(ReportingPeriod periodDomain, ICollection<ReportingPeriodSupplierEntity> reportingPeriodSupplierEntities, IEnumerable<SupplierReportingPeriodStatus> supplierReportingPeriodStatuses)
    {
        foreach (var item in reportingPeriodSupplierEntities)
        {
            var supplierVO = GenerateSupplierVO(item.Supplier);
            //var facilityVO = GenerateFacilityVO(item.ReportingPeriodFacilityEntities);
            var selectedSupplierStatus = supplierReportingPeriodStatuses.First(x => x.Id == item.SupplierReportingPeriodStatusId);

            periodDomain.LoadPeriodSupplier(item.Id, supplierVO, item.ReportingPeriodId, selectedSupplierStatus);

        }

    }
    */

    //private FacilityVO GenerateFacilityVO(FacilityEntity facility)
    //{
    //    return new FacilityVO;
    //}

    private SupplierVO GenerateSupplierVO(SupplierEntity supplier)
    {
        var facilityVOs = new List<FacilityVO>();
        return new SupplierVO(supplier.Id, supplier.Name, supplier.IsActive, facilityVOs);
    }

    public ReportingPeriodEntity ConvertReportingPeriodDomainToEntity(ReportingPeriod reportingPeriod)
    {
        var reportingPeriodSuppliers = ConvertReportingPeriodSuppliersDomainToEntity(reportingPeriod.PeriodSuppliers ?? new List<PeriodSupplier>());

        var reportingPeriodType = new ReportingPeriodTypeEntity();
        reportingPeriodType.Id = reportingPeriod.ReportingPeriodType.Id;
        reportingPeriodType.Name = reportingPeriod.ReportingPeriodType.Name;

        var reportingPeriodStatus = new ReportingPeriodStatusEntity();
        reportingPeriodStatus.Id = reportingPeriod.ReportingPeriodStatus.Id;
        reportingPeriodStatus.Name = reportingPeriod.ReportingPeriodStatus.Name;

        return new ReportingPeriodEntity()
        {
            Id = reportingPeriod.Id,
            DisplayName = reportingPeriod.DisplayName,
            ReportingPeriodTypeId = reportingPeriodType.Id,
            ReportingPeriodType = reportingPeriodType,
            CollectionTimePeriod = reportingPeriod.CollectionTimePeriod,
            ReportingPeriodStatusId = reportingPeriodStatus.Id,
            ReportingPeriodStatus = reportingPeriodStatus,
            StartDate = reportingPeriod.StartDate,
            EndDate = reportingPeriod.EndDate,
            IsActive = reportingPeriod.IsActive,
            ReportingPeriodSupplierEntities = reportingPeriodSuppliers.ToList(),
        };
    }

    public ReportingPeriod ConvertReportingPeriodEntityToDomain(ReportingPeriodEntity reportingPeriodEntity, IEnumerable<ReportingPeriodType> reportingPeriodTypes, IEnumerable<ReportingPeriodStatus> reportingPeriodStatuses)
    {
        var reportingPeriodSelectedType = reportingPeriodTypes.Where(x => x.Id == reportingPeriodEntity.ReportingPeriodTypeId).FirstOrDefault();
        var reportingPeriodSelectedStatus = reportingPeriodStatuses.Where(x => x.Id == reportingPeriodEntity.ReportingPeriodStatusId).FirstOrDefault();

        var reportingPeriod = new ReportingPeriod(reportingPeriodEntity.Id, 
            reportingPeriodEntity.DisplayName, 
            reportingPeriodSelectedType, 
            reportingPeriodEntity.CollectionTimePeriod, 
            reportingPeriodSelectedStatus, 
            reportingPeriodEntity.StartDate, 
            reportingPeriodEntity.EndDate, 
            reportingPeriodEntity.IsActive);

        return reportingPeriod;

    }

    public IEnumerable<ReportingPeriod> ConvertReportingPeriodEntitiesToDomain(IEnumerable<ReportingPeriodEntity> reportingPeriodEntities, IEnumerable<ReportingPeriodType> reportingPeriodTypes, IEnumerable<ReportingPeriodStatus> reportingPeriodStatuses)
    {
        var list = new List<ReportingPeriod>();
        foreach(var reportingPeriod in reportingPeriodEntities)
        {
            list.Add(ConvertReportingPeriodEntityToDomain(reportingPeriod, reportingPeriodTypes, reportingPeriodStatuses));
        }
        return list;
    }

    public ReportingPeriodSupplierEntity ConvertReportingPeriodSupplierDomainToEntity(PeriodSupplier periodSupplier)
    {
        return new ReportingPeriodSupplierEntity()
        {
            Id = periodSupplier.Id,
            SupplierId = periodSupplier.Supplier.Id,
            ReportingPeriodId = periodSupplier.ReportingPeriodId,
            SupplierReportingPeriodStatusId = periodSupplier.SupplierReportingPeriodStatus.Id,
            IsActive = periodSupplier.IsActive
        };
    }

    public IEnumerable<ReportingPeriodSupplierEntity> ConvertReportingPeriodSuppliersDomainToEntity(IEnumerable<PeriodSupplier> periodSuppliers)
    {
        var suppliers = new List<ReportingPeriodSupplierEntity>();
        foreach (var periodSupplier in periodSuppliers)
        {
            suppliers.Add(ConvertReportingPeriodSupplierDomainToEntity(periodSupplier));
        }
        return suppliers;
    }

    public SupplierVO ConvertSupplierToSupplierValueObject(SupplierEntity supplierEntity, IEnumerable<SupplyChainStage>? supplyChainStages = null, IEnumerable<ReportingType>? reportingTypes = null)
    {
        var facilityVO = new List<FacilityVO>();

        foreach (var facilityEntity in supplierEntity.FacilityEntities)
        {
            var selectedSupplyChainStage = supplyChainStages != null ? supplyChainStages.FirstOrDefault(x => x.Id == facilityEntity.SupplyChainStageId) : null;
            var selectedReprtingType = reportingTypes != null ? reportingTypes.FirstOrDefault(x => x.Id ==facilityEntity.ReportingTypeId) : null;
        }
        var supplierVO = new SupplierVO(supplierEntity.Id,supplierEntity.Name,supplierEntity.IsActive,facilityVO);
        return supplierVO;
    }

    public IEnumerable<SupplierVO> ConvertSupplierEntityToSupplierValueObject(IEnumerable<SupplierEntity> supplierEntities)
    {
        var supplierVOs = new List<SupplierVO>();

        foreach(var supplierEntity in supplierEntities)
        {
            supplierVOs.Add(ConvertSupplierToSupplierValueObject(supplierEntity));
        }
        return supplierVOs;
    }

    public PeriodSupplier ConvertPeriodSuppliersEntityToDomain(ReportingPeriod reportingPeriod, ReportingPeriodSupplierEntity reportingPeriodSupplierEntity, IEnumerable<SupplierReportingPeriodStatus> supplierReportingPeriodStatuses,SupplierVO supplierVO)
    {
        var supplierReportingPeriodSelectedStatus = supplierReportingPeriodStatuses.Where(x => x.Id == reportingPeriodSupplierEntity.SupplierReportingPeriodStatusId).FirstOrDefault();

        var periodSupplier = new PeriodSupplier(reportingPeriodSupplierEntity.Id,supplierVO, reportingPeriod.Id,supplierReportingPeriodSelectedStatus);

        return periodSupplier;
    }
}
