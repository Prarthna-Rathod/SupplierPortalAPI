using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using DataAccess.Entities;
using Services.DTOs;
using Services.DTOs.ReadOnlyDTOs;


namespace Services.Mappers.Interfaces;

public interface IReportingPeriodDomainDtoMapper
{
    #region ReportingPeriod
    ReportingPeriodDto ConvertReportingPeriodDomainToDto(ReportingPeriod reportingPeriod);
    IEnumerable<ReportingPeriodDto> ConvertReportingPeriodDomainListToDtos(IEnumerable<ReportingPeriod> reportingPeriods);

    #endregion

    #region PeriodSupplier

   

    IEnumerable<ReportingPeriodSupplierDto> ConvertPeriodSupplierDomainListToDtos(IEnumerable<PeriodSupplier> periodSuppliersDomain, ReportingPeriod reportingPeriod);
    ReportingPeriodSupplierDto ConvertPeriodSupplierDomainToDto(PeriodSupplier periodSuppliersDomain, string displayName);

    IEnumerable<ReportingPeriodRelevantSupplierDto> ConvertReleventPeriodSupplierDomainToDto(IEnumerable<PeriodSupplier> periodSupplierDomainList,IEnumerable<SupplierEntity> inRelevantSupplierList, ReportingPeriod reportingPeriod);

    IEnumerable<ReportingPeriodActiveSupplierVO> ConvertMultiplePeriodSupplierDtosToValueObject(IEnumerable<MultiplePeriodSuppliersDto> multiplePeriodSuppliersDtos,IEnumerable<SupplierVO>supplierVO,IEnumerable<SupplierReportingPeriodStatus> supplierReportingPeriodStatuses);



    #endregion

    #region PeriodFacility

    IEnumerable<ReportingPeriodSupplierRelaventFacilityDto> ConvertPeriodFacilityDomainListToDtos(IEnumerable<PeriodFacility> periodFacilities, IEnumerable<FacilityEntity> facilityEntities);

    ReportingPeriodSupplierRelaventFacilityDto ConvertPeriodFacilityDomainToDto(PeriodFacility periodFacility, bool isRelaventForPeriodStatus);

    ReportingPeriodSupplierFacilitiesDto ConvertReportingPeriodSupplierFacilitiesDomainToDto(PeriodSupplier periodSupplier, IEnumerable<ReportingPeriodSupplierRelaventFacilityDto> periodFacilitiesDtos);

    IEnumerable<ReportingPeriodRelevantFacilityVO> ConvertPeriodFacilityDtoToValueObject(ReportingPeriodFacilityDto reportingPeriodFacilityDtos, IEnumerable<FacilityVO> facilityVOs, IEnumerable<FacilityReportingPeriodDataStatus> facilityReportingPeriodDataStatus, PeriodSupplier periodSupplier);

    #endregion

    #region PeriodFacilityElectricityGridMix
    IEnumerable<ReportingPeriodFacilityElectricityGridMixVO> ConvertPeriodFacilityElectricityGridMixDtoToValueObject(AddMultiplePeriodFacilityElectricityGridMixDto addMultiplePeriodFacilityElectricityGridMixDtos, IEnumerable<ElectricityGridMixComponent> electricityGridMixComponent, IEnumerable<UnitOfMeasure> unitOfMeasure);

    AddMultiplePeriodFacilityElectricityGridMixDto ConvertReportingPeriodFacilityElectricityGridMixEntityToDto(ReportingPeriodFacilityEntity reportingPeriodFacilityEntity, IEnumerable<UnitOfMeasure> unitOfMeasures, IEnumerable<ElectricityGridMixComponent> electricityGridMixComponents);

    #endregion

    #region PeriodFacilityGasSupplyBreakDown

    IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownVO> ConvertPeriodFacilityGasSupplyBreakDownDtoToValueObject(IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownDto> periodFacilityGasSupplyBreakDownDto, IEnumerable<Site> sites, IEnumerable<UnitOfMeasure> unitOfMeasures);
    MultiplePeriodFacilityGasSupplyBreakDownDto ConvertReportingPeriodGasSupplyBreakDownDomainListToDto(PeriodSupplier periodSupplier, IEnumerable<PeriodFacility> periodFacility);

    #endregion

    #region PeriodDocument
    ReportingPeriodFacilityGridMixAndDocumentDto ConvertPeriodFacilityElectricityGridMixAndDocumentDomainListToDto(PeriodFacility periodFacility, PeriodSupplier periodSupplier);
    ReportingPeriodSupplierGasSupplyAndDocumentDto ConvertPeriodSupplierGasSupplyAndDocumentDomainListToDto(IEnumerable<PeriodFacility> periodFacilities, PeriodSupplier periodSupplier);
    #endregion



}
