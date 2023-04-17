using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.UnitTestMappers.ReportingPeriodMappers
{
    public class ReportingPeriodEntityDomainMapperUnitTest: BasicTestClass
    {
        /// <summary>
        /// Convert ReportingPeriodDomain To ReportingPeriodEntity mapper testing
        /// </summary>
        [Fact]
        public void ConvertReportingPeriodDomainToEntity()
        {
            var reportingPeriod = GetReportingPeriodDomain();
            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();

            var reportingPeriodEntity = mapper.ConvertReportingPeriodDomainToEntity(reportingPeriod);

            Assert.NotNull(reportingPeriodEntity);
            Assert.Equal(reportingPeriod.Id, reportingPeriodEntity.Id);
            Assert.Equal(reportingPeriod.DisplayName, reportingPeriodEntity.DisplayName);
            Assert.Equal(reportingPeriod.ReportingPeriodType.Id, reportingPeriodEntity.ReportingPeriodTypeId);
            Assert.Equal(reportingPeriod.CollectionTimePeriod, reportingPeriodEntity.CollectionTimePeriod);
            Assert.Equal(reportingPeriod.ReportingPeriodStatus.Id, reportingPeriodEntity.ReportingPeriodStatusId);
            Assert.Equal(reportingPeriod.StartDate.Date, reportingPeriodEntity.StartDate.Date);
            Assert.Equal(reportingPeriod.EndDate, reportingPeriodEntity.EndDate);
            Assert.Equal(reportingPeriod.IsActive, reportingPeriodEntity.IsActive);
        }

        /// <summary>
        /// Convert ReportingPeriodEntity To ReportingPeriodDomain mapper testing
        /// </summary>
        [Fact]
        public void ConvertReportingPeriodEntityToDomain()
        {
            var reportingPeriodEntity = CreateReportingPeriodEntity();
            var reportingPeriodTypes = GetAndConvertReportingPeriodTypes();
            var reportingPeriodStatuses = GetAndConvertReportingPeriodStatus();
            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();

            var reportingPeriodDomain = mapper.ConvertReportingPeriodEntityToDomain(reportingPeriodEntity, reportingPeriodTypes, reportingPeriodStatuses);

            Assert.NotNull(reportingPeriodDomain);
            Assert.Equal(reportingPeriodEntity.Id, reportingPeriodDomain.Id);
            Assert.Equal(reportingPeriodEntity.DisplayName, reportingPeriodDomain.DisplayName);
            Assert.Equal(reportingPeriodEntity.ReportingPeriodTypeId, reportingPeriodDomain.ReportingPeriodType.Id);
            Assert.Equal(reportingPeriodEntity.CollectionTimePeriod, reportingPeriodDomain.CollectionTimePeriod);
            Assert.Equal(reportingPeriodEntity.ReportingPeriodStatusId, reportingPeriodDomain.ReportingPeriodStatus.Id);
            Assert.Equal(reportingPeriodEntity.StartDate.Date, reportingPeriodDomain.StartDate.Date);
            Assert.Equal(reportingPeriodEntity.EndDate, reportingPeriodDomain.EndDate);
            Assert.Equal(reportingPeriodEntity.IsActive, reportingPeriodDomain.IsActive);
        }
    }
}
