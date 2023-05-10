using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
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

    public IEnumerable<SupplierVO> ConvertSupplierEntityToSupplierValueObject(IEnumerable<SupplierEntity> supplierEntities)
    {
        var supplierVOs = new List<SupplierVO>();

        foreach (var supplierEntity in supplierEntities)
        {
            supplierVOs.Add(ConvertSupplierEntityToSupplierValueObject(supplierEntity, null, null));
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

    public ReportingPeriodFacilityEntity ConvertReportingPeriodFacilityDomainToEntity(PeriodFacility periodFacility)
    {
        var periodFacilityEntity = new ReportingPeriodFacilityEntity();
        periodFacilityEntity.Id = periodFacility.Id;
        periodFacilityEntity.FacilityId = periodFacility.FacilityVO.Id;
        periodFacilityEntity.FacilityReportingPeriodDataStatusId = periodFacility.FacilityReportingPeriodDataStatus.Id;
        periodFacilityEntity.ReportingPeriodId = periodFacility.ReportingPeriodId;
        periodFacilityEntity.ReportingTypeId = periodFacility.FacilityVO.ReportingType.Id;
        periodFacilityEntity.GhgrpfacilityId = periodFacility.FacilityVO.GHGRPFacilityId;
        periodFacilityEntity.SupplyChainStageId = periodFacility.FacilityVO.SupplyChainStage.Id;
        periodFacilityEntity.ReportingPeriodSupplierId = periodFacility.ReportingPeriodSupplierId;
        periodFacilityEntity.IsActive = periodFacility.IsActive;

        return periodFacilityEntity;
    }

    #endregion

    #region PeriodFacilityElectricityGridMix

    public ReportingPeriodFacilityElectricityGridMixEntity ConvertPeriodFacilityElectricityGridMixDomainToEntity(PeriodFacilityElectricityGridMix facilityElectricityGridMix)
    {
        var entity = new ReportingPeriodFacilityElectricityGridMixEntity();
        entity.ReportingPeriodFacilityId = facilityElectricityGridMix.PeriodFacilityId;
        entity.ElectricityGridMixComponentId = facilityElectricityGridMix.ElectricityGridMixComponent.Id;

        var unitOfMeasureEntity = new UnitOfMeasureEntity();
        unitOfMeasureEntity.Id = facilityElectricityGridMix.UnitOfMeasure.Id;
        unitOfMeasureEntity.Name = facilityElectricityGridMix.UnitOfMeasure.Name;

        var fercRegionEntity = new FercRegionEntity();
        fercRegionEntity.Id = facilityElectricityGridMix.FercRegion.Id;
        fercRegionEntity.Name = facilityElectricityGridMix.FercRegion.Name;

        entity.UnitOfMeasure = unitOfMeasureEntity;
        entity.UnitOfMeasureId = unitOfMeasureEntity.Id;
        entity.FercRegion = fercRegionEntity;
        entity.FercRegionId = fercRegionEntity.Id;
        entity.Content = facilityElectricityGridMix.Content;
        entity.IsActive = true;

        return entity;
    }

    public IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> ConvertPeriodFacilityElectricityGridMixDomainListToEntities(IEnumerable<PeriodFacilityElectricityGridMix> periodFacilityElectricityGridMixes)
    {
        var list = new List<ReportingPeriodFacilityElectricityGridMixEntity>();
        foreach (var gridMixDomain in periodFacilityElectricityGridMixes)
        {
            list.Add(ConvertPeriodFacilityElectricityGridMixDomainToEntity(gridMixDomain));
        }
        return list;
    }

    public IEnumerable<ElectricityGridMixComponentPercent> ConvertPeriodFacilityElectricityGridMixEntitiesToValueObjects(IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> reportingPeriodFacilityElectricityGridMixEntities, IEnumerable<ElectricityGridMixComponent> electricityGridMixComponent)
    {
        var list = new List<ElectricityGridMixComponentPercent>();
        foreach (var entity in reportingPeriodFacilityElectricityGridMixEntities)
        {
            var gridMixComponent = electricityGridMixComponent.FirstOrDefault(x => x.Id == entity.ElectricityGridMixComponentId);
            list.Add(ConvertPeriodFacilityElectricityGridMixEntityToValueObject(entity.Id, gridMixComponent, entity.Content));
        }
        return list;
    }

    public ElectricityGridMixComponentPercent ConvertPeriodFacilityElectricityGridMixEntityToValueObject(int id, ElectricityGridMixComponent periodFacilityElectricityGridMix, decimal content)
    {
        return new ElectricityGridMixComponentPercent(id, periodFacilityElectricityGridMix, content);
    }

    #endregion

    #region PeriodFacilityGasSupplyBreakdown

    public ReportingPeriodFacilityGasSupplyBreakDownEntity ConvertPeriodFacilityGasSupplyBreakdownDomainToEntity(PeriodFacilityGasSupplyBreakdown periodFacilityGasSupplyBreakdown)
    {
        var entity = new ReportingPeriodFacilityGasSupplyBreakDownEntity();
        entity.PeriodFacilityId = periodFacilityGasSupplyBreakdown.PeriodFacilityId;
        entity.SiteId = periodFacilityGasSupplyBreakdown.Site.Id;
        entity.UnitOfMeasureId = periodFacilityGasSupplyBreakdown.UnitOfMeasure.Id;
        entity.Content = periodFacilityGasSupplyBreakdown.Content;
        entity.IsActive = true;
        return entity;
    }

    public IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownEntity> ConvertPeriodFacilityGasSupplyBreakdownDomainListToEntities(IEnumerable<PeriodFacilityGasSupplyBreakdown> periodFacilityGasSupplyBreakdowns)
    {
        var list = new List<ReportingPeriodFacilityGasSupplyBreakDownEntity>();
        foreach (var gasSupplyBreakdown in periodFacilityGasSupplyBreakdowns)
        {
            list.Add(ConvertPeriodFacilityGasSupplyBreakdownDomainToEntity(gasSupplyBreakdown));
        }
        return list;
    }

    public IEnumerable<GasSupplyBreakdownVO> ConvertPeriodFacilityGasSupplyBreakdownEntitiesToValueObjects(IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownEntity> periodFacilityGasSupplyBreakDownEntities,IEnumerable<Site> sites,IEnumerable<UnitOfMeasure> unitOfMeasures)
    {
        var list = new List<GasSupplyBreakdownVO>();
        foreach (var entity in periodFacilityGasSupplyBreakDownEntities)
        {
            var site = sites.FirstOrDefault(x => x.Id == entity.SiteId);
            var unitOfMeasure = unitOfMeasures.FirstOrDefault(x => x.Id == entity.UnitOfMeasureId);
            list.Add(ConvertPeriodFacilityGasSupplyBreakdownEntityToValueObject(entity,site,unitOfMeasure));
        }
        return list;
    }

    public GasSupplyBreakdownVO ConvertPeriodFacilityGasSupplyBreakdownEntityToValueObject(ReportingPeriodFacilityGasSupplyBreakDownEntity entity, Site site,UnitOfMeasure unitOfMeasure)
    {
        var gasSupplyBreakdownVo = new GasSupplyBreakdownVO(entity.Id, entity.PeriodFacilityId, entity.PeriodFacility.FacilityId, site, unitOfMeasure, entity.Content);
        return gasSupplyBreakdownVo;
    }

    #endregion

    #region PeriodDocument
    #endregion

}
