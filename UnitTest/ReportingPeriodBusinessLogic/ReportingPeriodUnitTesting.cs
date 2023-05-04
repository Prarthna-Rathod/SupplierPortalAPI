using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;

namespace UnitTest.ReportingPeriodBusinessLogic
{
    public class ReportingPeriodUnitTesting : BasicTestClass
    {

        #region Private Method

        public ReportingPeriod AddPeriodSupplierAndPeriodFacilityForPeriod()
        {
            var reportingPeriod = GetReportingPeriodDomain();

            //Add PeriodSupplier
            var supplierVO = GetAndConvertSupplierValueObject();
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2023, 5, 5), new DateTime(2023, 5, 6));

            //Add PeriodFacility
            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
            var periodFacility = reportingPeriod.AddPeriodFacility(1, facilityVO, facilityReportingPeriodStatus, periodSupplier.Id, true, true);

            //Update ReportingPeriodStatus InActive To Open
            var updatePeriodStatus = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Open);
            reportingPeriod.ReportingPeriodStatus.Id = updatePeriodStatus.Id;
            reportingPeriod.ReportingPeriodStatus.Name = updatePeriodStatus.Name;

            return reportingPeriod;
        }

        #endregion

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
                periodSupplier = reportingPeriod.AddPeriodSupplier(0, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

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
                reportingPeriod.AddPeriodSupplier(0, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));
                reportingPeriod.AddPeriodSupplier(0, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));
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
                reportingPeriod.AddPeriodSupplier(0, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));
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
        /// Update LockUnlockPeriodSupplierStatus Success Case
        /// Updated PeriodSupplierStatus from Unlock to Lock and Viceversa
        /// Check reportingPeriodStatus is Open or Close then only Update PeriodSupplierStatus
        /// </summary>
        [Fact]
        public void LockUnlockPeriodSupplierStatusSucceedCase()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = GetReportingPeriodDomain();
            var supplierVO = GetAndConvertSupplierValueObject();
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            //set this status open and close here
            reportingPeriod.ReportingPeriodStatus.Id = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Open).Id;
            reportingPeriod.ReportingPeriodStatus.Name = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Open).Name;

            var updatedStatus = GetSupplierReportingPeriodStatuses();

            try
            {
                reportingPeriod.UpdateLockUnlockPeriodSupplierStatus(periodSupplier.Id, updatedStatus);
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
        /// Update LockUnlockPeriodSupplierStatus Failure case
        /// If reportingPeriodStatus is InActive or Complete then can't update periodSupplierStatus
        /// </summary>
        [Fact]
        public void LockUnlockPeriodSupplierStatusFailCase()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = GetReportingPeriodDomain();
            var supplierVO = GetAndConvertSupplierValueObject();
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            //set this status complete here
            reportingPeriod.ReportingPeriodStatus.Id = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Complete).Id;
            reportingPeriod.ReportingPeriodStatus.Name = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Complete).Name;

            var updatedStatuses = GetSupplierReportingPeriodStatuses();

            try
            {
                reportingPeriod.UpdateLockUnlockPeriodSupplierStatus(periodSupplier.Id, updatedStatuses);
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
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            //Add PeriodFacility
            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);

            try
            {
                reportingPeriod.AddPeriodFacility(0, facilityVO, facilityReportingPeriodStatus, periodSupplier.Id, true, true);
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
        /// Add ReportingPeriodFacility success case2.
        /// Try to add duplicate record in ReportingPeriodFacility and check FacilityIsRelaventForPeriod value is false, then remove the existing record.
        /// </summary>

        [Fact]
        public void AddDuplicatePeriodFacilityRemoveSuccessCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = GetReportingPeriodDomain();

            //Get PeriodSupplier Domain
            var supplierVO = GetAndConvertSupplierValueObject();
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            //Add PeriodFacility
            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);

            try
            {
                reportingPeriod.AddPeriodFacility(1, facilityVO, facilityReportingPeriodStatus, 1, true, true);
                reportingPeriod.AddPeriodFacility(0, facilityVO, facilityReportingPeriodStatus, 1, false, true);

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

            //Get PeriodSupplier Domain
            var supplierVO = GetAndConvertSupplierValueObject();
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodDataStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.Complete);

            try
            {
                reportingPeriod.AddPeriodFacility(0, facilityVO, facilityReportingPeriodDataStatus, 1, true, true);
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
        /// Add ReportingPeriodFacility failure case2.
        /// Try to add duplicate record and FacilityIsRelaventForPeriod is true than throw exception.
        /// </summary>

        [Fact]
        public void AddPeriodFacilityFailsCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = GetReportingPeriodDomain();

            //Get PeriodSupplier Domain
            var supplierVO = GetAndConvertSupplierValueObject();
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodDataStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);

            try
            {
                reportingPeriod.AddPeriodFacility(1, facilityVO, facilityReportingPeriodDataStatus, 1, true, true);
                reportingPeriod.AddPeriodFacility(0, facilityVO, facilityReportingPeriodDataStatus, 1, true, true);
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
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            //Add new  PeriodFacility
            var supplyChainStage = GenerateSupplyChainStage().First();
            var reportingType = GenerateReportingType().First();
            var facilityVO = new FacilityVO(10, "Test facility", 2, "123", true, supplyChainStage, reportingType);
            var facilityReportingPeriodStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);

            try
            {
                reportingPeriod.AddPeriodFacility(0, facilityVO, facilityReportingPeriodStatus, periodSupplier.Id, true, true);

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

        #region Add PeriodFacility ElectricityGridMixComponents

        /// <summary>
        /// Add PeriodFacilityElectricityGridMixComponents success case1.
        /// FercRegion is CustomMix for add components and contentValues sum is 100
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGridMixSuccessCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //GridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.Custom_Mix);
            var gridMixComponentPercents = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;

            try
            {
                list = reportingPeriod.AddRemoveElectricityGridMixComponents(1, 1, unitOfMeasure, fercRegion, gridMixComponentPercents);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(list);
            Assert.Equal(gridMixComponentPercents.Count(), list.Count());
            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);

        }

        /// <summary>
        /// Add PeriodFacilityElectricityGridMixComponents success case2.
        /// If GridMixComponents are already exists then replace all
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGridMixSuccessCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //GridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.Custom_Mix);
            var gridMixComponentPercents = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;
            reportingPeriod.AddRemoveElectricityGridMixComponents(1, 1, unitOfMeasure, fercRegion, gridMixComponentPercents);
            var newGridMixList = GetElectricityGridMixComponentPercentsList2();

            try
            {
                list = reportingPeriod.AddRemoveElectricityGridMixComponents(1, 1, unitOfMeasure, fercRegion, newGridMixList);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(list);
            Assert.NotEqual(gridMixComponentPercents.Count(), list.Count());
            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);

        }


        /// <summary>
        /// Add PeriodFacilityElectricityGridMixComponents success case3.
        /// If GridMixComponents are already exists and update FercRegion
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGridMixSuccessCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //GridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.Custom_Mix);
            var gridMixComponentPercents = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;
            reportingPeriod.AddRemoveElectricityGridMixComponents(1, 1, unitOfMeasure, fercRegion, gridMixComponentPercents);
            var percents = new List<ElectricityGridMixComponentPercent>();


            try
            {
                fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.None);
                list = reportingPeriod.AddRemoveElectricityGridMixComponents(1, 1, unitOfMeasure, fercRegion, percents);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(list);
            Assert.Equal(gridMixComponentPercents.Count(), list.Count());
            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);

        }


        /// <summary>
        /// Add PeriodFacilityElectricityGridMixComponents failure case1.
        /// If FercRegion is not CustomMix for then throw exception
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGridMixFailsCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //GridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.None);
            var gridMixComponentPercents = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;

            try
            {
                list = reportingPeriod.AddRemoveElectricityGridMixComponents(1, 1, unitOfMeasure, fercRegion, gridMixComponentPercents);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.Equal(gridMixComponentPercents.Count(), list.Count());
            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);

        }

        /// <summary>
        /// Add PeriodFacilityElectricityGridMixComponents failure case2.
        /// If FercRegion is CustomMix and gridMixComponents are empty then throw exception
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGridMixFailsCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //GridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.Custom_Mix);
            var percents = new List<ElectricityGridMixComponentPercent>();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;

            try
            {
                list = reportingPeriod.AddRemoveElectricityGridMixComponents(1, 1, unitOfMeasure, fercRegion, percents);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);

        }


        /// <summary>
        /// Add PeriodFacilityElectricityGridMixComponents failure case3.
        /// If GridMixComponent will repeat then throw exception.
        /// For this test case -> Make any component repeated in BasicTestClass -> GetElectricityGridMixComponentPercents() method
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGridMixFailsCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //GridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.Custom_Mix);
            var gridMixComponentPercents = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;

            try
            {
                list = reportingPeriod.AddRemoveElectricityGridMixComponents(1, 1, unitOfMeasure, fercRegion, gridMixComponentPercents);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);

        }

        /// <summary>
        /// Add PeriodFacilityElectricityGridMixComponents failure case4.
        /// If ContentValues is not 100 then throw exception.
        /// For this test case -> Change ContentValues (sum should be not 100) in BasicTestClass -> GetElectricityGridMixComponentPercents() method
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGridMixFailsCase4()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //GridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.Custom_Mix);
            var gridMixComponentPercents = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;

            try
            {
                list = reportingPeriod.AddRemoveElectricityGridMixComponents(1, 1, unitOfMeasure, fercRegion, gridMixComponentPercents);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);

        }

        /// <summary>
        /// Add PeriodFacilityElectricityGridMixComponents failure case5.
        /// If ReportingPeriodStatus is InActive or Complete then throw exception.
        /// For this test case Go to ->  AddPeriodSupplierAndPeriodFacilityForPeriod() method and set ReportingPeriodStatus InActive or Complete
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGridMixFailsCase5()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //GridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.Custom_Mix);
            var gridMixComponentPercents = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;

            try
            {
                list = reportingPeriod.AddRemoveElectricityGridMixComponents(1, 1, unitOfMeasure, fercRegion, gridMixComponentPercents);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);

        }

        /// <summary>
        /// Add PeriodFacilityElectricityGridMixComponents failure case6.
        /// If SupplierReportingPeriodStatus is Locked then throw exception.
        /// For this test case Go to -> AddPeriodSupplierAndPeriodFacilityForPeriod() method and set SupplierReportingPeriodStatus is Locked.
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGridMixFailsCase6()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //Update SupplierReportingPeriodStatus to Locked
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Locked);
            var unlockedSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == 1);
            unlockedSupplier.SupplierReportingPeriodStatus.Id = supplierReportingPerionStatus.Id;
            unlockedSupplier.SupplierReportingPeriodStatus.Name = supplierReportingPerionStatus.Name;

            //GridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.Custom_Mix);
            var gridMixComponentPercents = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;

            try
            {
                list = reportingPeriod.AddRemoveElectricityGridMixComponents(1, 1, unitOfMeasure, fercRegion, gridMixComponentPercents);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);

        }


        #endregion


    }
}
