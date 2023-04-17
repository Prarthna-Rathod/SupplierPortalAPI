using BusinessLogic.ReportingPeriodRoot.DomainModels;
using Services.DTOs;
using Services.DTOs.ReadOnlyDTOs;
using Services.Mappers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers.ReportingPeriodMappers
{
    public class ReportingPeriodDomainDtoMapper : IReportingPeriodDomainDtoMapper
    {
        public ReportingPeriodDto ConvertReportingPeriodDomainToDto(ReportingPeriod reportingPeriod)
        {
            var reportingPeriodSuppliers = new List<ReportingPeriodSupplierDto>();
            
            return new ReportingPeriodDto(reportingPeriod.Id,reportingPeriod.DisplayName,reportingPeriod.ReportingPeriodType.Id,reportingPeriod.ReportingPeriodType.Name,reportingPeriod.CollectionTimePeriod,reportingPeriod.ReportingPeriodStatus.Id,reportingPeriod.ReportingPeriodStatus.Name,reportingPeriod.StartDate,reportingPeriod.EndDate,reportingPeriod.IsActive,reportingPeriodSuppliers);
        }

        public IEnumerable<ReportingPeriodDto> ConvertReportingPeriodDomainListToDtos(IEnumerable<ReportingPeriod> reportingPeriods)
        {
            var list = new List<ReportingPeriodDto>();
            foreach(var reportingPeriod in reportingPeriods)
            {
                list.Add(ConvertReportingPeriodDomainToDto(reportingPeriod));
            }
            return list;
        }
    }
}
