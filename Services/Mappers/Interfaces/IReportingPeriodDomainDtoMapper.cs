using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.SupplierRoot.DomainModels;
using Services.DTOs;
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

    //IEnumerable<ReportingPeriodSupplierDto> ConvertRelevantSuppliersDomainToDtos(IEnumerable<Supplier> suppliersDomain);

    IEnumerable<ReportingPeriodSupplierDto> ConvertPeriodSupplierDomainListToDtos(IEnumerable<PeriodSupplier> periodSuppliersDomain, ReportingPeriod reportingPeriod);
    ReportingPeriodSupplierDto ConvertPeriodSupplierDomainToDto(PeriodSupplier periodSuppliersDomain, string displayName);

    #endregion

    #region PeriodFacility
    #endregion

    #region PeriodDocument
    #endregion


}
