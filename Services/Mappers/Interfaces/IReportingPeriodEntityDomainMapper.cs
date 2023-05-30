﻿using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
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

    IEnumerable<SupplierVO> ConvertSupplierEntityToSupplierValueObjectList(IEnumerable<SupplierEntity   > supplierEntities, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<ReportingType> reportingTypes);

   /* PeriodSupplier ConvertPeriodSupplierEntityToDomain(ReportingPeriodSupplierEntity reportingPeriodSupplierEntity, IEnumerable<SupplierReportingPeriodStatus> supplierReportingPeriodStatuses, SupplierVO supplierVO);

    IEnumerable<PeriodSupplier> ConvertPeriodSuppliersEntitiesToDomainList(IEnumerable<ReportingPeriodSupplierEntity> reportingPeriodSupplierEntities,
        IEnumerable<SupplierReportingPeriodStatus> supplierReportingPeriodStatuses,
        IEnumerable<ReportingType> reportingTypes, IEnumerable<SupplyChainStage> supplyChainStages);*/

    #endregion

    #region PeriodFacility

    FacilityVO ConvertFacilityEntityToFacilityValueObject(FacilityEntity facilityEntity, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<ReportingType> reportingTypes);

    IEnumerable<FacilityVO> ConvertFacilityEntityToFacilityValueObjectList(IEnumerable<FacilityEntity> facilityEntities, IEnumerable<SupplyChainStage> supplyChainStages,IEnumerable<ReportingType> reportingTypes);

    FacilityVO ConvertFacilityEntityToFacilityVO(FacilityEntity facilityEntity, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<ReportingType> reportingTypes);

    ReportingPeriodFacilityEntity ConvertReportingPeriodFacilityDomainToEntity(PeriodFacility periodFacility,IEnumerable<FercRegion> fercRegions);

    IEnumerable<ReportingPeriodFacilityEntity> ConvertReportingPeriodFacilityDomainListToEntity(IEnumerable<PeriodFacility> periodFacilities, IEnumerable<FercRegion> fercRegions);

    #endregion

    #region PeriodFacility ElectricityGridMix

    ReportingPeriodFacilityElectricityGridMixEntity ConvertPeriodFacilityElectricityGridMixDomainToEntity(PeriodFacilityElectricityGridMix facilityElectricityGridMix);

    IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> ConvertPeriodFacilityElectricityGridMixDomainListToEntity(IEnumerable<PeriodFacilityElectricityGridMix> facilityElectricityGridMix);



    #endregion

    #region PeriodFaciity GasSupplyBreakDown
    ReportingPeriodFacilityGasSupplyBreakdownEntity ConvertPeriodFacilityGasSupplyBreakDownDomainToEntity(PeriodFacilityGasSupplyBreakDown periodFacilityGasSupplyBreakDown);

    IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownEntity> ConvertPeriodFacilityGasSupplyBreakDownSupplyDomainListToEntity(IEnumerable<PeriodFacilityGasSupplyBreakDown> periodFacilityGasSupplyBreakDown);
    #endregion

    #region PeriodDocument
    #endregion

}
