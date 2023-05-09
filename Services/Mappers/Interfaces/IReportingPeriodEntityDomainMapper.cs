using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers.Interfaces;

public interface IReportingPeriodEntityDomainMapper
{
    #region ReportingPeriod
    ReportingPeriodEntity ConvertReportingPeriodDomainToEntity(ReportingPeriod reportingPeriod);

    ReportingPeriod ConvertReportingPeriodEntityToDomain(ReportingPeriodEntity reportingPeriodEntity,IEnumerable<ReportingPeriodType> reportingPeriodTypes, IEnumerable<ReportingPeriodStatus> reportingPeriodStatuses);

    IEnumerable<ReportingPeriod> ConvertReportingPeriodEntitiesToDomain(IEnumerable<ReportingPeriodEntity> reportingPeriodEntities, IEnumerable<ReportingPeriodType> reportingPeriodTypes, IEnumerable<ReportingPeriodStatus> reportingPeriodStatuses);

    #endregion

    #region PeriodSupplier
    IEnumerable<ReportingPeriodSupplierEntity> ConvertReportingPeriodSuppliersDomainToEntity(IEnumerable<PeriodSupplier> periodSuppliers);
    ReportingPeriodSupplierEntity ConvertReportingPeriodSupplierDomainToEntity(PeriodSupplier periodSupplier);

    SupplierVO ConvertSupplierEntityToSupplierValueObject(SupplierEntity supplierEntity, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<ReportingType> reportingTypes);

    IEnumerable<SupplierVO> ConvertSupplierEntityToSupplierValueObject(IEnumerable<SupplierEntity> supplierEntities);

    #endregion

    #region PeriodFacility

    FacilityVO ConvertFacilityEntityToFacilityValueObject(FacilityEntity facilityEntity, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<ReportingType> reportingTypes);

    ReportingPeriodFacilityEntity ConvertReportingPeriodFacilityDomainToEntity(PeriodFacility periodFacility);

    IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> ConvertPeriodFacilityElectricityGridMixDomainListToEntities(IEnumerable< PeriodFacilityElectricityGridMix> facilityElectricityGridMixes);

    ReportingPeriodFacilityElectricityGridMixEntity ConvertPeriodFacilityElectricityGridMixDomainToEntity(PeriodFacilityElectricityGridMix facilityElectricityGridMix);

    IEnumerable<ElectricityGridMixComponentPercent> ConvertPeriodFacilityElectricityGridMixEntityListToValueObjectList(IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> facilityElectricityGridMixeEntities, IEnumerable<ElectricityGridMixComponent> electricityGridMixesLookUps);

    ElectricityGridMixComponentPercent ConvertPeriodFacilityElectricityGridMixEntityToValueObject(int id, decimal content, ElectricityGridMixComponent electricityGridMixLookUp);

    IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownEntity> ConvertPeriodFacilityGasSupplyBreakdownDomainListToEntities(IEnumerable<PeriodFacilityGasSupplyBreakdown> facilityGasSupplyBreakdowns);

    ReportingPeriodFacilityGasSupplyBreakDownEntity ConvertPeriodFacilityGasSupplyBreakdownDomainToEntity(PeriodFacilityGasSupplyBreakdown facilityGasSupplyBreakdown);

    IEnumerable<GasSupplyBreakdownVO> ConvertPeriodFacilityGasSupplyBreakdownEntitiesToValueObjectList(IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownEntity> gasSupplyEntities, IEnumerable<Site> sites, IEnumerable<UnitOfMeasure> unitOfMeasures);

    GasSupplyBreakdownVO ConvertPeriodFacilityGasSupplyBreakdownEntityToValueObject(ReportingPeriodFacilityGasSupplyBreakDownEntity gasSupplyEntity, Site site, UnitOfMeasure unitOfMeasure);

    #endregion

    #region PeriodDocument
    #endregion

}
