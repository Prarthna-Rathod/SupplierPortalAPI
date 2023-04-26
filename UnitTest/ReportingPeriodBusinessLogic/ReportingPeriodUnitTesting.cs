using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.ReportingPeriodBusinessLogic
{
    public class ReportingPeriodUnitTesting : BasicTestClass
    {
        #region Update ReportingPeriod

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
            var reportingPeriodType = GetAndConvertReportingPeriodTypes().First(x => x.Name == ReportingPeriodTypeValues.Annual);
            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().First(x => x.Name == ReportingPeriodStatusValues.Open);
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
        /// For this UnitTest set ReportingPeriodStatus Open in BasicTestClass -> CreateReportingPeriodEntity
        /// </summary>
        [Fact]
        public void UpdateReportingPeriodSucceedCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = GetReportingPeriodDomain();
            var reportingPeriodType = GetAndConvertReportingPeriodTypes().First(x => x.Name == ReportingPeriodTypeValues.Annual);
            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().First(x => x.Name == ReportingPeriodStatusValues.Close);
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
        /// Update ReportingPeriod Failed Case1
        /// If ReportingPeriodStatus changed from InActive to Complete then throw exception
        /// </summary>
        [Fact]
        public void UpdateReportingPeriodFailsCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = GetReportingPeriodDomain();
            var reportingPeriodType = GetAndConvertReportingPeriodTypes().First(x => x.Name == ReportingPeriodTypeValues.Annual);
            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().First(x => x.Name == ReportingPeriodStatusValues.Complete);
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
        /// For this UnitTest set ReportingPeriodStatus Closed in BasicTestClass -> CreateReportingPeriodEntity
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

        #endregion

        #region Add PeriodSupplier

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
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);

            PeriodSupplier? periodSupplier = null;

            //Act
            try
            {
                periodSupplier = reportingPeriod.AddPeriodSupplier(0,supplierVO, supplierReportingPerionStatus,true,true,true);

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
        /// Add ReportingPeriodSupplier failure case.
        /// In this case duplicate ReportingPeriodSupplier can not be add.
        /// </summary>
        [Fact]
        public void AddDuplicatePeriodSupplierFailsCase1()
        {
            int exceptionCounter = 0;
            var reportingPeriod = GetReportingPeriodDomain();
            string? exceptionMessage = null;

            try
            {
                var supplierVO = GetAndConvertSupplierValueObject();
                var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
                reportingPeriod.AddPeriodSupplier(0,supplierVO, supplierReportingPerionStatus,true,true,true);
                reportingPeriod.AddPeriodSupplier(0, supplierVO, supplierReportingPerionStatus, true, true, true);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }
            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }

        /// <summary>
        /// Add ReportingPeriodSupplier failure case2.
        /// If Supplier IsActive false or ReportingPeriodStatus is not InActive and try to add any data then throw exception
        /// For this UnitTest set Supplier IsActive false in BasicTestClass
        /// </summary>
        [Fact]
        public void AddPeriodSupplierFailsCase2()
        {
            int exceptionCounter = 0;
            var reportingPeriod = GetReportingPeriodDomain();
            string? exceptionMessage = null;

            try
            {
                var supplierVO = GetAndConvertSupplierValueObject();
                var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
                reportingPeriod.AddPeriodSupplier(0,supplierVO, supplierReportingPerionStatus,true,true,true);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }


        #endregion

        #region Add PeriodFacilities

        /// <summary>
        /// Add ReportingPeriodFacility success case
        /// Add new record in ReportingPeriodFacility and check FacilityIsRelaventForPeriod value is true, FacilityReportingPeriodStatus is InProgress
        /// </summary>
        [Fact]
        public void AddPeriodFacilitiesSuccess()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = GetReportingPeriodDomain();
            //Get PeriodSupplier Domain
            var supplierVO = GetAndConvertSupplierValueObject();
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1,supplierVO, supplierReportingPerionStatus,true,true,true);

            //Add PeriodFacility
            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);

            try
            {
                reportingPeriod.AddPeriodFacility(0, facilityVO, facilityReportingPeriodStatus, periodSupplier.Id, true);
            }
            catch(Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);

        }

        /// <summary>
        /// Add ReportingPeriodFacility success case2.
        /// Try to add duplicate record in ReportingPeriodFacility and check FacilityIsRelaventForPeriod value is false, then remove the existing record.
        /// </summary>
        [Fact]
        public void AddDuplicatePeriodFacilityRemoveSuccessCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = GetReportingPeriodDomain();
            
            //Add PeriodFacility
            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);

            try
            {
                reportingPeriod.AddPeriodFacility(1, facilityVO, facilityReportingPeriodStatus, 1, true);
                reportingPeriod.AddPeriodFacility(0, facilityVO, facilityReportingPeriodStatus, 1, false);

            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);

        }

        /// <summary>
        /// Add ReportingPeriodFacility failure case1.
        /// If add new record and FacilityReportingPeriodDataStatus is not InProgress then throw exception.
        /// </summary>

        [Fact]
        public void AddPeriodFacilityFailsCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = GetReportingPeriodDomain();
            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodDataStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.Complete);
            
            try
            {
                reportingPeriod.AddPeriodFacility(0, facilityVO, facilityReportingPeriodDataStatus, 1, true);
            }
            catch(Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }

        /// <summary>
        /// Add ReportingPeriodFacility failure case2.
        /// Try to add duplicate record and FacilityIsRelaventForPeriod is true than throw exception.
        /// </summary>

        [Fact]
        public void AddPeriodFacilityFailsCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = GetReportingPeriodDomain();
            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodDataStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);

            try
            {
                reportingPeriod.AddPeriodFacility(1, facilityVO, facilityReportingPeriodDataStatus, 1, true);
                reportingPeriod.AddPeriodFacility(0, facilityVO, facilityReportingPeriodDataStatus, 1, true);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }


        /// <summary>
        /// Try to add facility which is not relavent with given PeriodSupplier Facilities then throw exception.
        /// For this UnitTesting I have created new FacilityVO with different facilityId and supplierId.
        /// </summary>

        [Fact]
        public void AddPeriodFacilityFailsCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = GetReportingPeriodDomain();
            //Add periodSupplier
            var supplierVO = GetAndConvertSupplierValueObject();
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus,true,true,true);

            //Add new  PeriodFacility
            var supplyChainStage = GenerateSupplyChainStage().First();
            var reportingType = GenerateReportingType().First();
            var facilityVO = new FacilityVO(10, "Test facility", 2, "123", true, supplyChainStage, reportingType);
            var facilityReportingPeriodStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);

            try
            {
                reportingPeriod.AddPeriodFacility(0, facilityVO, facilityReportingPeriodStatus, periodSupplier.Id, true);

            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);

        }




        #endregion
    }
}
