using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.SupplierRoot.DomainModels;
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

    IEnumerable<ReportingPeriodSupplierRelaventFacilityDto> ConvertPeriodFacilityDomainListToDtos(IEnumerable<PeriodFacility> periodFacilities);

    ReportingPeriodSupplierRelaventFacilityDto ConvertPeriodFacilityDomainToDto(PeriodFacility periodFacility);

    #endregion

    #region PeriodDocument
    #endregion


}
