using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.UnitTestMappers.ReportingPeriodMappers
{
    public class ReportingPeriodDomainDtoMapperUnitTest: BasicTestClass
    {
        [Fact]
        public void ConvertReportingPeriodDomainToDto()
        {
            var reportingPeriod = GetReportingPeriodDomain();
            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();
            
            var reportingPeriodDto = mapper.ConvertReportingPeriodDomainToDto(reportingPeriod);

            Assert.NotNull(reportingPeriodDto);
            Assert.Equal(reportingPeriod.Id, reportingPeriodDto.Id);
            Assert.Equal(reportingPeriod.DisplayName, reportingPeriodDto.DisplayName);
            Assert.Equal(reportingPeriod.ReportingPeriodType.Id, reportingPeriodDto.ReportingPeriodTypeId);
            Assert.Equal(reportingPeriod.CollectionTimePeriod, reportingPeriodDto.CollectionTimePeriod);
            Assert.Equal(reportingPeriod.ReportingPeriodStatus.Id, reportingPeriodDto.ReportingPeriodStatusId);
            Assert.Equal(reportingPeriod.StartDate.Date, reportingPeriodDto.StartDate.Date);
            Assert.Equal(reportingPeriod.EndDate, reportingPeriodDto.EndDate);
            Assert.Equal(reportingPeriod.IsActive, reportingPeriodDto.IsActive);
        }
    }
}
