using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using DataAccess.Entities;
using Services.Mappers.Interfaces;


namespace Services.Mappers.ReportingPeriodMappers;

public class ReportingPeriodEntityDomainMapper : IReportingPeriodEntityDomainMapper
{
    #region ReportingPeriod

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
        foreach (var reportingPeriod in reportingPeriodEntities)
        {
            list.Add(ConvertReportingPeriodEntityToDomain(reportingPeriod, reportingPeriodTypes, reportingPeriodStatuses));
        }
        return list;
    }


    #endregion

    #region PeriodSupplier


    public ReportingPeriodSupplierEntity ConvertReportingPeriodSupplierDomainToEntity(PeriodSupplier periodSupplier)
    {
        return new ReportingPeriodSupplierEntity()
        {
            Id = periodSupplier.Id,
            SupplierId = periodSupplier.Supplier.Id,
            ReportingPeriodId = periodSupplier.ReportingPeriodId,
            SupplierReportingPeriodStatusId = periodSupplier.SupplierReportingPeriodStatus.Id,
            InitialDataRequestDate = periodSupplier.InitialDataRequestDate,
            ResendDataRequestDate = periodSupplier.ResendDataRequestDate,
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
    public SupplierVO ConvertSupplierEntityToSupplierValueObject(SupplierEntity supplierEntity, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<ReportingType> reportingTypes)
    {
        var facilityVOs = new List<FacilityVO>();

        foreach (var facilityEntity in supplierEntity.FacilityEntities)
        {
            var selectedSupplyChainStage = supplyChainStages.FirstOrDefault(x => x.Id == facilityEntity.SupplyChainStageId);
            var selectedReprtingType = reportingTypes.FirstOrDefault(x => x.Id == facilityEntity.ReportingTypeId);

            facilityVOs.Add(new FacilityVO(facilityEntity.Id, facilityEntity.Name, facilityEntity.SupplierId, facilityEntity.GhgrpfacilityId, facilityEntity.IsActive, selectedSupplyChainStage, selectedReprtingType));

        }
        var supplierVO = new SupplierVO(supplierEntity.Id, supplierEntity.Name, supplierEntity.IsActive, facilityVOs);
        return supplierVO;
    }

    public IEnumerable<SupplierVO> ConvertSupplierEntityToSupplierValueObjectList(IEnumerable<SupplierEntity> supplierEntities, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<ReportingType> reportingTypes)
    {
        var supplierVOs = new List<SupplierVO>();

        foreach (var supplierEntity in supplierEntities)
        {
            supplierVOs.Add(ConvertSupplierEntityToSupplierValueObject(supplierEntity, supplyChainStages, reportingTypes));
        }
        return supplierVOs;
    }


    #endregion

    #region PeriodFacility

    public FacilityVO ConvertFacilityEntityToFacilityValueObject(FacilityEntity facilityEntity, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<ReportingType> reportingTypes)
    {
        var selectedSupplyChainStage = supplyChainStages.FirstOrDefault(x => x.Id == facilityEntity.SupplyChainStageId);
        var selectedReprtingType = reportingTypes.FirstOrDefault(x => x.Id == facilityEntity.ReportingTypeId);

        var facilityVOs = new FacilityVO(facilityEntity.Id, facilityEntity.Name, facilityEntity.SupplierId, facilityEntity.GhgrpfacilityId, facilityEntity.IsActive, selectedSupplyChainStage, selectedReprtingType);

        return facilityVOs;
    }

    public IEnumerable<FacilityVO> ConvertFacilityEntityToFacilityValueObjectList(IEnumerable<FacilityEntity> facilityEntities, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<ReportingType> reportingTypes)
    {
        var facilityVOs = new List<FacilityVO>();

        foreach (var facilityEntity in facilityEntities)
        {
            var selectedSupplyChainStage = supplyChainStages.FirstOrDefault(x => x.Id == facilityEntity.SupplyChainStageId);
            var selectedReprtingType = reportingTypes.FirstOrDefault(x => x.Id == facilityEntity.ReportingTypeId);

            facilityVOs.Add(new FacilityVO(facilityEntity.Id, facilityEntity.Name, facilityEntity.SupplierId, facilityEntity.GhgrpfacilityId, facilityEntity.IsActive, selectedSupplyChainStage, selectedReprtingType));
        }

        return facilityVOs;



    }


    public FacilityVO ConvertFacilityEntityToFacilityVO(FacilityEntity facilityEntity, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<ReportingType> reportingTypes)
    {

        var selectedSupplyChainStage = supplyChainStages.FirstOrDefault(x => x.Id == facilityEntity.SupplyChainStageId);
        var selectedReprtingType = reportingTypes.FirstOrDefault(x => x.Id == facilityEntity.ReportingTypeId);

        var facilityVO = new FacilityVO(facilityEntity.Id, facilityEntity.Name, facilityEntity.SupplierId, facilityEntity.GhgrpfacilityId, facilityEntity.IsActive, selectedSupplyChainStage, selectedReprtingType);
        return facilityVO;

    }


    public ReportingPeriodFacilityEntity ConvertReportingPeriodFacilityDomainToEntity(PeriodFacility periodFacility, IEnumerable<FercRegion> fercRegions)
    {
        var periodFacilityEntity = new ReportingPeriodFacilityEntity();
        var fercRegion = fercRegions.FirstOrDefault(x => x.Id == periodFacilityEntity.FercRegionId);

        periodFacilityEntity.FacilityId = periodFacility.FacilityVO.Id;
        periodFacilityEntity.FacilityReportingPeriodDataStatusId = periodFacility.FacilityReportingPeriodDataStatus.Id;
        periodFacilityEntity.ReportingPeriodId = periodFacility.ReportingPeriodId;
        periodFacilityEntity.ReportingTypeId = periodFacility.FacilityVO.ReportingType.Id;
        periodFacilityEntity.GhgrpfacilityId = periodFacility.FacilityVO.GHGRPFacilityId;
        periodFacilityEntity.SupplyChainStageId = periodFacility.FacilityVO.SupplyChainStage.Id;
        periodFacilityEntity.ReportingPeriodSupplierId = periodFacility.ReportingPeriodSupplierId;
        periodFacilityEntity.FercRegionId = periodFacility.FercRegion.Id;
        periodFacilityEntity.IsActive = periodFacility.IsActive;
        periodFacilityEntity.Id = periodFacility.Id;

        return periodFacilityEntity;
    }

    public IEnumerable<ReportingPeriodFacilityEntity> ConvertReportingPeriodFacilityDomainListToEntity(IEnumerable<PeriodFacility> periodFacilities, IEnumerable<FercRegion> fercRegions)
    {
        var periodFacilityEntity = new List<ReportingPeriodFacilityEntity>();

        foreach (var periodFacility in periodFacilities)
        {
            periodFacilityEntity.Add(ConvertReportingPeriodFacilityDomainToEntity(periodFacility, fercRegions));
        }
        return periodFacilityEntity;
    }


    #endregion

    #region PeriodFacility ElectricityGridMix


    public ReportingPeriodFacilityElectricityGridMixEntity ConvertPeriodFacilityElectricityGridMixDomainToEntity(PeriodFacilityElectricityGridMix facilityElectricityGridMix)
    {
        var entity = new ReportingPeriodFacilityElectricityGridMixEntity();
        entity.ReportingPeriodFacilityId = facilityElectricityGridMix.PeriodFacilityId;
        entity.ElectricityGridMixComponentId = facilityElectricityGridMix.ElectricityGridMixComponent.Id;
        entity.UnitOfMeasureId = facilityElectricityGridMix.UnitOfMeasure.Id;
        entity.Content = facilityElectricityGridMix.Content;
        entity.IsActive = facilityElectricityGridMix.IsActive;
        entity.CreatedBy = "system";
        entity.CreatedOn = DateTime.UtcNow;

        return entity;
    }
    public IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> ConvertPeriodFacilityElectricityGridMixDomainListToEntity(IEnumerable<PeriodFacilityElectricityGridMix> facilityElectricityGridMix)
    {
        var periodFacilityElectricityGridMixEntity = new List<ReportingPeriodFacilityElectricityGridMixEntity>();

        foreach (var electricityGridMix in facilityElectricityGridMix)
        {
            periodFacilityElectricityGridMixEntity.Add(ConvertPeriodFacilityElectricityGridMixDomainToEntity(electricityGridMix));
        }
        return periodFacilityElectricityGridMixEntity;
    }

    //public IEnumerable<GasSupplyBreakdownVO> ConvertPeriodFacilityGasSupplyBreakdownEntitiesToValueObjects(IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownEntity> periodFacilityGasSupplyBreakDownEntities, IEnumerable<Site> sites, IEnumerable<UnitOfMeasure> unitOfMeasures)
    //{
    //    var list = new List<GasSupplyBreakdownVO>();
    //    foreach (var entity in periodFacilityGasSupplyBreakDownEntities)
    //    {
    //        var site = sites.FirstOrDefault(x => x.Id == entity.SiteId);
    //        var unitOfMeasure = unitOfMeasures.FirstOrDefault(x => x.Id == entity.UnitOfMeasureId);
    //        list.Add(ConvertPeriodFacilityGasSupplyBreakdownEntityToValueObject(entity, site, unitOfMeasure));
    //    }
    //    return list;
    //}

    //public GasSupplyBreakdownVO ConvertPeriodFacilityGasSupplyBreakdownEntityToValueObject(ReportingPeriodFacilityGasSupplyBreakDownEntity entity, Site site, UnitOfMeasure unitOfMeasure)
    //{
    //    var gasSupplyBreakdownVo = new GasSupplyBreakdownVO(entity.Id, entity.PeriodFacilityId, entity.PeriodFacility.FacilityId, site, unitOfMeasure, entity.Content);
    //    return gasSupplyBreakdownVo;
    //}
    #endregion

    #region PeriodFacilityGasSupplyBreakDown
    public ReportingPeriodFacilityGasSupplyBreakdownEntity ConvertPeriodFacilityGasSupplyBreakDownDomainToEntity(PeriodFacilityGasSupplyBreakDown periodFacilityGasSupplyBreakDown)
    {
        var entity = new ReportingPeriodFacilityGasSupplyBreakdownEntity();
        entity.PeriodFacilityId = periodFacilityGasSupplyBreakDown.PeriodFacilityId;
        entity.UnitOfMeasureId = periodFacilityGasSupplyBreakDown.UnitOfMeasure.Id;
        entity.SiteId = periodFacilityGasSupplyBreakDown.Site.Id;
        entity.Content = periodFacilityGasSupplyBreakDown.Content;
        entity.IsActive = true;
        entity.CreatedBy = "System";
        entity.CreatedOn= DateTime.UtcNow;
        return entity;

    }

    public IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownEntity> ConvertPeriodFacilityGasSupplyBreakDownSupplyDomainListToEntity(IEnumerable<PeriodFacilityGasSupplyBreakDown> periodFacilityGasSupplyBreakDown)
    {
        var list= new List<ReportingPeriodFacilityGasSupplyBreakdownEntity>();

        foreach(var gasSupplyBreakdown in periodFacilityGasSupplyBreakDown)
        {
            list.Add(ConvertPeriodFacilityGasSupplyBreakDownDomainToEntity(gasSupplyBreakdown));
        }
        return list;
    }
    #endregion

    #region PeriodDocument
    #endregion

}


