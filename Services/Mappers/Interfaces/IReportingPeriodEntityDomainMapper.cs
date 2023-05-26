using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using DataAccess.Entities;

namespace Services.Mappers.Interfaces;

public interface IReportingPeriodEntityDomainMapper
{
    #region ReportingPeriod
    ReportingPeriodEntity ConvertReportingPeriodDomainToEntity(ReportingPeriod reportingPeriod);

    ReportingPeriod ConvertReportingPeriodEntityToDomain(ReportingPeriodEntity reportingPeriodEntity, IEnumerable<ReportingPeriodType> reportingPeriodTypes, IEnumerable<ReportingPeriodStatus> reportingPeriodStatuses);

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

    IEnumerable<ReportingPeriodFacilityEntity> ConvertReportingPeriodFacilitiesDomainToEntity(IEnumerable<PeriodFacility> periodFacilities);

    #endregion

    #region PeriodFacilityElectricityGridMix

    ReportingPeriodFacilityElectricityGridMixEntity ConvertPeriodFacilityElectricityGridMixDomainToEntity(PeriodFacilityElectricityGridMix facilityElectricityGridMix);

    IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> ConvertPeriodFacilityElectricityGridMixDomainListToEntities(IEnumerable<PeriodFacilityElectricityGridMix> periodFacilityElectricityGridMixes);

    IEnumerable<ElectricityGridMixComponentPercent> ConvertPeriodFacilityElectricityGridMixEntitiesToValueObjects(IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> reportingPeriodFacilityElectricityGridMixEntities, IEnumerable<ElectricityGridMixComponent> electricityGridMixComponent);

    ElectricityGridMixComponentPercent ConvertPeriodFacilityElectricityGridMixEntityToValueObject(int id, ElectricityGridMixComponent periodFacilityElectricityGridMix, decimal content);

    #endregion

    #region PeriodFacilityGasSupplyBreakdown

    ReportingPeriodFacilityGasSupplyBreakDownEntity ConvertPeriodFacilityGasSupplyBreakdownDomainToEntity(PeriodFacilityGasSupplyBreakdown periodFacilityGasSupplyBreakdown);

    IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownEntity> ConvertPeriodFacilityGasSupplyBreakdownDomainListToEntities(IEnumerable<PeriodFacilityGasSupplyBreakdown> periodFacilityGasSupplyBreakdowns);

    IEnumerable<GasSupplyBreakdownVO> ConvertPeriodFacilityGasSupplyBreakdownEntitiesToValueObjects(IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownEntity> periodFacilityGasSupplyBreakDownEntities, IEnumerable<Site> sites, IEnumerable<UnitOfMeasure> unitOfMeasures);

    GasSupplyBreakdownVO ConvertPeriodFacilityGasSupplyBreakdownEntityToValueObject(ReportingPeriodFacilityGasSupplyBreakDownEntity entity, Site site, UnitOfMeasure unitOfMeasure);

    #endregion

    #region PeriodDocument

    ReportingPeriodFacilityDocumentEntity ConvertReportingPeriodFacilityDocumentDomainToEntity(PeriodFacilityDocument periodFacilityDocument);

    #endregion

    #region FacilityRequiredDocumentType

    IEnumerable<FacilityRequiredDocumentTypeVO> ConvertFacilityRequiredDocumentTypeEntitiesToValueObjects(IEnumerable<FacilityRequiredDocumentTypeEntity> facilityRequiredDocumentTypeEntities, IEnumerable<ReportingType> reportingTypes, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<DocumentType> documentTypes, IEnumerable<DocumentRequiredStatus> documentRequiredStatuses);

    FacilityRequiredDocumentTypeVO ConvertFacilityRequiredDocumentTypeEntityToValueObject(ReportingType reportingType, SupplyChainStage supplyChainStage, DocumentType documentType, DocumentRequiredStatus documentRequiredStatus);

    #endregion

    #region PeriodSupplierDocument

    ReportingPeriodSupplierDocumentEntity ConvertReportingPeriodSupplierDocumentDomainToEntity(PeriodSupplierDocument periodSupplierDocument);

    #endregion

}
