using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using DataAccess.Entities;
using Services.DTOs;
using Services.DTOs.ReadOnlyDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    #endregion

    #region PeriodFacility

    IEnumerable<ReportingPeriodSupplierRelaventFacilityDto> ConvertPeriodFacilityDomainListToDtos(IEnumerable<PeriodFacility> periodFacilities, IEnumerable<FacilityEntity> facilityEntities);

    ReportingPeriodSupplierRelaventFacilityDto ConvertPeriodFacilityDomainToDto(PeriodFacility periodFacility, bool isRelaventForPeriodStatus);

    ReportingPeriodSupplierFacilitiesDto ConvertReportingPeriodSupplierFacilitiesDomainToDto(PeriodSupplier periodSupplier, IEnumerable<ReportingPeriodSupplierRelaventFacilityDto> periodFacilitiesDtos);

    #endregion

    #region PeriodFacilityElectricityGridMix

    IEnumerable<ElectricityGridMixComponentPercent> ConvertPeriodElectricityGridMixDtosToValueObjects(IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> reportingPeriodFacilityElectricityGridMixDtos, IEnumerable<ElectricityGridMixComponent> electricityGridMixComponents);

    ElectricityGridMixComponentPercent ConvertPeriodElectricityGridMixDtoToValueObject(ElectricityGridMixComponent electricityGridMixComponent, decimal content);

    #endregion

    #region PeriodFacilityGasSupplyBreakDown

    IEnumerable<GasSupplyBreakdownVO> ConvertPeriodFacilityGasSupplyBreakDownDtosToValueObjects(IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownDto> periodFacilityGasSupplyBreakdownDtos, IEnumerable<Site> sites, IEnumerable<UnitOfMeasure> unitOfMeasures);

    GasSupplyBreakdownVO ConvertPeriodFacilityGasSupplyBreakDownDtoToValueObject(ReportingPeriodFacilityGasSupplyBreakdownDto periodFacilityGasSupplyBreakdownDto, Site site, UnitOfMeasure unitOfMeasure, decimal content);

    #endregion

    #region PeriodDocument
    #endregion


}
