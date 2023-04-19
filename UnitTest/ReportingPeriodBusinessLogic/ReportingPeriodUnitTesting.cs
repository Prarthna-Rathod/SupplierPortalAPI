using BusinessLogic.ReportingPeriodRoot.DomainModels;

namespace UnitTest.ReportingPeriodBusinessLogic
{
    public class ReportingPeriodUnitTesting : BasicTestClass
    {
        /// <summary>
        /// Update ReportingPeriod Success Case1
        /// Updated ReportingPeriodStatus from InActive to Open
        /// </summary>
        [Fact]
        public void UpdateReportingPeriodSucceedCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = GetReportingPeriodDomain();
            var reportingPeriodType = GetAndConvertReportingPeriodTypes().First(x => x.Id == reportingPeriod.ReportingPeriodType.Id);
            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().First(x => x.Id == 2);
            var supplierReportingPeriodStatuses = GetSupplierReportingPeriodStatuses();

            try
            {
                reportingPeriod.UpdateReportingPeriod(reportingPeriodType, "2022", reportingPeriodStatus, new DateTime(2023, 4, 12), null, true, supplierReportingPeriodStatuses);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);
        }

        /// <summary>
        /// Update ReportingPeriod Success Case2
        /// If ReportingPeriodStatus is changed from Open to Closed then set SupplierReportingPeriodStatus 'Locked' for all related ReportingPeriodSuppliers.
        /// For this UnitTest set ReportingPeriodStatus Open in BasicTestClass
        /// </summary>
        [Fact]
        public void UpdateReportingPeriodSucceedCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = GetReportingPeriodDomain();
            var reportingPeriodType = GetAndConvertReportingPeriodTypes().First(x => x.Id == reportingPeriod.ReportingPeriodType.Id);
            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().First(x => x.Id == 3);
            var supplierReportingPeriodStatuses = GetSupplierReportingPeriodStatuses();


            try
            {
                reportingPeriod.UpdateReportingPeriod(reportingPeriodType, "2022", reportingPeriodStatus, new DateTime(2023, 4, 12), null, true, supplierReportingPeriodStatuses);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);

        }
        /// <summary>
        /// Add ReportingPeriodSupplier Success case
        /// In this case supplier should be active & reportingPeriodStatus should be InActive
        /// </summary>
        [Fact]
        public void AddReportingPeriodSupplierSucceed()
        {

            //Arrange
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = GetReportingPeriodDomain();
            var supplierVO = GetAndConvertSupplierValueObject();
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Id == 2);

            PeriodSupplier periodSupplier = null;

            //Act
            try
            {
                periodSupplier = reportingPeriod.AddPeriodSupplier(supplierVO, reportingPeriod.Id, supplierReportingPerionStatus);

            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            //Assert
            Assert.NotNull(periodSupplier);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);

        }

        /// <summary>
        /// Update ReportingPeriod Failed Case1
        /// If ReportingPeriodStatus changed from InActive to Complete then throw exception
        /// </summary>
        [Fact]
        public void UpdateReportingPeriodFailsCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = GetReportingPeriodDomain();
            var reportingPeriodType = GetAndConvertReportingPeriodTypes().First(x => x.Id == reportingPeriod.ReportingPeriodType.Id);
            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().First(x => x.Id == 4);
            var supplierReportingPeriodStatuses = GetSupplierReportingPeriodStatuses();

            try
            {
                reportingPeriod.UpdateReportingPeriod(reportingPeriodType, "2022", reportingPeriodStatus, new DateTime(2023, 4, 12), new DateTime(2024, 4, 12), true, supplierReportingPeriodStatuses);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);

        }

        /// <summary>
        /// Update ReportingPeriod Failed Case2
        /// If reportingPeriodStatus is Closed and try to update any data then throw exception
        /// For this UnitTest set ReportingPeriodStatus Closed in BasicTestClass
        /// </summary>
        [Fact]
        public void UpdateReportingPeriodFailsCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = GetReportingPeriodDomain();
            var reportingPeriodType = GetAndConvertReportingPeriodTypes().First(x => x.Id == reportingPeriod.ReportingPeriodType.Id);
            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().First(x => x.Id == reportingPeriod.ReportingPeriodStatus.Id);
            var supplierReportingPeriodStatuses = GetSupplierReportingPeriodStatuses();

            try
            {
                reportingPeriod.UpdateReportingPeriod(reportingPeriodType, "2023", reportingPeriodStatus, new DateTime(2024, 4, 12), new DateTime(2024, 4, 12), true, supplierReportingPeriodStatuses);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);
        }

        /// <summary>
        /// Update ReportingPeriod Failed Case3
        /// If startDate or endDate is in past then throw exception.
        /// For this UnitTest set past StartDate or past EndDate.
        /// </summary>

        [Fact]
        public void UpdateReportingPeriodFailsCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = GetReportingPeriodDomain();
            var reportingPeriodType = GetAndConvertReportingPeriodTypes().First(x => x.Id == reportingPeriod.ReportingPeriodType.Id);
            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().First(x => x.Id == reportingPeriod.ReportingPeriodStatus.Id);
            var supplierReportingPeriodStatuses = GetSupplierReportingPeriodStatuses();

            try
            {
                reportingPeriod.UpdateReportingPeriod(reportingPeriodType, "2022", reportingPeriodStatus, new DateTime(2022, 4, 12), new DateTime(2022, 4, 12), true, supplierReportingPeriodStatuses);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);
        }
    }
}
