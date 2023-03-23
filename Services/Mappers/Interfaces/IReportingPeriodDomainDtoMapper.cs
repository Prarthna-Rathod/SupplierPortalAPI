using BusinessLogic.ReportingPeriodRoot.DomainModels;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers.Interfaces;

public interface IReportingPeriodDomainDtoMapper
{
    ReportingPeriodDto ConvertReportingDTO(ReportingPeriod reportingPeriod);
}
