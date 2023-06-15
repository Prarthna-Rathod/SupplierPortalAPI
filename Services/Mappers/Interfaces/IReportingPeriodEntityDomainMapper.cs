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

    IEnumerable<SupplierVO> ConvertSupplierEntityToSupplierValueObjectList(IEnumerable<SupplierEntity> supplierEntities, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<ReportingType> reportingTypes);

    #endregion

    #region PeriodFacility

    FacilityVO ConvertFacilityEntityToFacilityValueObject(FacilityEntity facilityEntity, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<ReportingType> reportingTypes);

    IEnumerable<FacilityVO> ConvertFacilityEntityToFacilityValueObjectList(IEnumerable<FacilityEntity> facilityEntities, IEnumerable<SupplyChainStage> supplyChainStages,IEnumerable<ReportingType> reportingTypes);

    FacilityVO ConvertFacilityEntityToFacilityVO(FacilityEntity facilityEntity, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<ReportingType> reportingTypes);

    ReportingPeriodFacilityEntity ConvertReportingPeriodFacilityDomainToEntity(PeriodFacility periodFacility,IEnumerable<FercRegion> fercRegions);

    IEnumerable<ReportingPeriodFacilityEntity> ConvertReportingPeriodFacilityDomainListToEntity(IEnumerable<PeriodFacility> periodFacilities, IEnumerable<FercRegion> fercRegions);

    ReportingPeriodFacilityEntity ConvertReportingPeriodFacilityDomainToEntity(PeriodFacility periodFacility);


    IEnumerable<ReportingPeriodFacilityEntity> ConvertReportingPeriodFacilitiesDomainToEntity(IEnumerable<PeriodFacility> periodFacilities);

    #endregion

    #region PeriodFacility ElectricityGridMix

    ReportingPeriodFacilityElectricityGridMixEntity ConvertPeriodFacilityElectricityGridMixDomainToEntity(PeriodFacilityElectricityGridMix facilityElectricityGridMix);

    IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> ConvertPeriodFacilityElectricityGridMixDomainListToEntity(IEnumerable<PeriodFacilityElectricityGridMix> facilityElectricityGridMix);



    #endregion

    #region PeriodFaciity GasSupplyBreakDown
    ReportingPeriodFacilityGasSupplyBreakdownEntity ConvertPeriodFacilityGasSupplyBreakDownDomainToEntity(PeriodFacilityGasSupplyBreakDown periodFacilityGasSupplyBreakDown);

    IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownEntity> ConvertPeriodFacilityGasSupplyBreakDownSupplyDomainListToEntity(IEnumerable<PeriodFacilityGasSupplyBreakDown> periodFacilityGasSupplyBreakDown);
    #endregion

    #region PeriodFacilityDocument
    ReportingPeriodFacilityDocumentEntity ConvertReportingPeriodFacilityDocumentDomainToEntity(PeriodFacilityDocument periodFacilityDocument);

    ReportingPeriodFacilityDocumentEntity ConvertPeriodFacilityDocumentDomainToEntity(PeriodFacilityDocument periodFacilityDocument, int periodFacilityDocumentId);
    #endregion

    #region FacilityRequiredDocumentVO
    FacilityRequiredDocumentType ConvertFacilityRequiredDocumentTypeEntityToValueObject(ReportingType reportingType, SupplyChainStage supplyChainStage, DocumentType documentType, DocumentRequiredStatus documentRequiredStatus);

    IEnumerable<FacilityRequiredDocumentType> ConvertFacilityRequiredDocumentTypeEntitiesToValueObjects(IEnumerable<FacilityRequiredDocumentTypeEntity> facilityRequiredDocumentTypeEntities, IEnumerable<ReportingType> reportingTypes, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<DocumentType> documentTypes, IEnumerable<DocumentRequiredStatus> documentRequiredStatuses);
    #endregion

    #region PeriodSupplierDocument
    ReportingPeriodSupplierDocumentEntity ConvertReportingPeriodSupplierDocumentDomainToEntity(PeriodSupplierDocument periodSupplierDocument);
    ReportingPeriodSupplierDocumentEntity ConvertPeriodSupplierDocumentDomainToEntity(PeriodSupplierDocument updatedPeriodSupplierDocumentDomain, int periodSupplierDocumentId);
    #endregion

}
