using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;

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
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);

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
                var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
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
                var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
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
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            //set this status open and close here
            reportingPeriod.ReportingPeriodStatus.Id = GetAndConvertReportingPeriodStatus().First(x => x.Name == ReportingPeriodStatusValues.Open).Id;
            reportingPeriod.ReportingPeriodStatus.Name = GetAndConvertReportingPeriodStatus().First(x => x.Name == ReportingPeriodStatusValues.Open).Name;

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
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            //set this status complete here
            reportingPeriod.ReportingPeriodStatus.Id = GetAndConvertReportingPeriodStatus().First(x => x.Name == ReportingPeriodStatusValues.Complete).Id;
            reportingPeriod.ReportingPeriodStatus.Name = GetAndConvertReportingPeriodStatus().First(x => x.Name == ReportingPeriodStatusValues.Complete).Name;

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
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            //Add PeriodFacility
            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.None);

            try
            {
                reportingPeriod.AddPeriodFacility(0, facilityVO, facilityReportingPeriodStatus, periodSupplier.Id, true, fercRegion, true);
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
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            //Add PeriodFacility
            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.None);

            try
            {
                reportingPeriod.AddPeriodFacility(1, facilityVO, facilityReportingPeriodStatus, 1, true, fercRegion, true);
                reportingPeriod.AddPeriodFacility(0, facilityVO, facilityReportingPeriodStatus, 1, false, fercRegion, true);

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
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodDataStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.Complete);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.None);

            try
            {
                reportingPeriod.AddPeriodFacility(0, facilityVO, facilityReportingPeriodDataStatus, 1, true, fercRegion, true);
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
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodDataStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.None);

            try
            {
                reportingPeriod.AddPeriodFacility(1, facilityVO, facilityReportingPeriodDataStatus, 1, true, fercRegion, true);
                reportingPeriod.AddPeriodFacility(0, facilityVO, facilityReportingPeriodDataStatus, 1, true, fercRegion, true);
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
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            //Add new  PeriodFacility
            var supplyChainStage = GenerateSupplyChainStage().First();
            var reportingType = GenerateReportingType().First();
            var facilityVO = new FacilityVO(10, "Test facility", 2, "123", true, supplyChainStage, reportingType);
            var facilityReportingPeriodStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.None);

            try
            {
                reportingPeriod.AddPeriodFacility(0, facilityVO, facilityReportingPeriodStatus, periodSupplier.Id, true, fercRegion, true);

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
            var unitOfMeasure = GetUnitOfMeasures().First(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.Custom_Mix);
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

            for (int i = 0; i < gridMixComponentPercents.Count(); i++)
            {
                var originalComponent = gridMixComponentPercents.ToList()[i].ElectricityGridMixComponent;
                var addedComponent = list.ToList()[i].ElectricityGridMixComponent;
                Assert.Equal(addedComponent, originalComponent);
            }

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
            var unitOfMeasure = GetUnitOfMeasures().First(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.Custom_Mix);
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
            Assert.Equal(newGridMixList.Count(), list.Count());

            for (int i = 0; i < newGridMixList.Count(); i++)
            {
                var originalComponent = newGridMixList.ToList()[i].ElectricityGridMixComponent;
                var addedComponent = list.ToList()[i].ElectricityGridMixComponent;
                Assert.Equal(addedComponent, originalComponent);
            }

            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);

        }

        /// <summary>
        /// Add PeriodFacilityElectricityGridMixComponents success case3.
        /// If GridMixComponents are already exists and updated FercRegion is not 'CustomMix' then remove all existing data
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGridMixSuccessCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //GridMix
            var unitOfMeasure = GetUnitOfMeasures().First(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.Custom_Mix);
            var gridMixComponentPercents = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;
            reportingPeriod.AddRemoveElectricityGridMixComponents(1, 1, unitOfMeasure, fercRegion, gridMixComponentPercents);
            var percents = new List<ElectricityGridMixComponentPercent>();

            //Update FercRegion to None for PeriodFacility
            var updatedFercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.None);

            try
            {
                list = reportingPeriod.AddRemoveElectricityGridMixComponents(1, 1, unitOfMeasure, updatedFercRegion, percents);
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
        /// Add PeriodFacilityElectricityGridMixComponents failure case1.
        /// If FercRegion is not CustomMix for then throw exception.
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGridMixFailsCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //GridMix
            var unitOfMeasure = GetUnitOfMeasures().First(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.None);
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
            var unitOfMeasure = GetUnitOfMeasures().First(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.Custom_Mix);
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
            var unitOfMeasure = GetUnitOfMeasures().First(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.Custom_Mix);
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
            var unitOfMeasure = GetUnitOfMeasures().First(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.Custom_Mix);
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
        /// For this test case Go to BasicTestClass --> AddPeriodSupplierAndPeriodFacilityForPeriod() method and set ReportingPeriodStatus InActive or Complete
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGridMixFailsCase5()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //GridMix
            var unitOfMeasure = GetUnitOfMeasures().First(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.Custom_Mix);
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
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGridMixFailsCase6()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //Update SupplierReportingPeriodStatus to Locked
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Locked);
            var unlockedSupplier = reportingPeriod.PeriodSuppliers.First(x => x.Id == 1);
            unlockedSupplier.SupplierReportingPeriodStatus.Id = supplierReportingPerionStatus.Id;
            unlockedSupplier.SupplierReportingPeriodStatus.Name = supplierReportingPerionStatus.Name;

            //GridMix
            var unitOfMeasure = GetUnitOfMeasures().First(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.Custom_Mix);
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

        #region Add PeriodSupplierFacility GasSupplyBreakdown
        //For all test cases set FacilitySupplyChainStage = 1 ('Production') in BasicTestClass --> GenerateFacilityEntitiesForSupplier() method

        /// <summary>
        /// Add ReportingPeriodSupplierFacility GasSupplyBreakdown success case1.
        /// Per Supplier per site content values are 100.
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownSuccessCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            var gasSupplyBreakdownVOs = GetGasSupplyBreakdownVOs();
            IEnumerable<PeriodFacilityGasSupplyBreakdown>? gasSupplyBreakdowns = null;

            try
            {
                gasSupplyBreakdowns = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVOs);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(gasSupplyBreakdowns);
            Assert.Equal(gasSupplyBreakdownVOs.Count(), gasSupplyBreakdowns.Count());

            for (int i = 0; i < gasSupplyBreakdownVOs.Count(); i++)
            {
                var originalSite = gasSupplyBreakdownVOs.ToList()[i].Site;
                var addedSite = gasSupplyBreakdowns.ToList()[i].Site;
                Assert.Equal(addedSite, originalSite);
            }

            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);
        }


        /// <summary>
        /// Replace existing ReportingPeriodSupplierFacility GasSupplyBreakdown data success case1.
        /// Per Supplier per site content values are 100.
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownSuccessCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            var gasSupplyBreakdownVOs = GetGasSupplyBreakdownVOs();
            var voList2 = GetGasSupplyBreakdownVOsList2();
            //First add new data
            reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVOs);

            IEnumerable<PeriodFacilityGasSupplyBreakdown>? gasSupplyBreakdowns = null;

            try
            {
                //Replace existing data with new data
                gasSupplyBreakdowns = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, voList2);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(gasSupplyBreakdowns);
            Assert.Equal(voList2.Count(), gasSupplyBreakdowns.Count());

            for (int i = 0; i < voList2.Count(); i++)
            {
                var originalSite = voList2.ToList()[i].Site;
                var addedSite = gasSupplyBreakdowns.ToList()[i].Site;
                Assert.Equal(addedSite, originalSite);
            }

            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);
        }

        /// <summary>
        /// Add ReportingPeriodFacility GasSupplyBreakdown Failure case1.
        /// If ReportingPeriodStatus is InActive or Complete then throw exception.
        /// For this test case, changed the ReportingPeriodStatus InActive here
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownFailsCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //Update ReportingPeriodStatus InActive
            var updatePeriodStatus = GetAndConvertReportingPeriodStatus().First(x => x.Name == ReportingPeriodStatusValues.InActive);
            reportingPeriod.ReportingPeriodStatus.Id = updatePeriodStatus.Id;
            reportingPeriod.ReportingPeriodStatus.Name = updatePeriodStatus.Name;

            //GasSupplyBreakdown ValueObjectList
            var gasSupplyBreakdownVOs = GetGasSupplyBreakdownVOs();
            IEnumerable<PeriodFacilityGasSupplyBreakdown>? gasSupplyBreakdowns = null;

            try
            {
                gasSupplyBreakdowns = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVOs);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(gasSupplyBreakdowns);
            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);

        }

        /// <summary>
        /// Add ReportingPeriodFacility GasSupplyBreakdown Failure case2.
        /// If SupplierReportingPeriodStatus is Locked then throw exception.
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownFailsCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //Update SupplierReportingPeriodStatus to Locked
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Locked);
            var unlockedSupplier = reportingPeriod.PeriodSuppliers.First(x => x.Id == 1);
            unlockedSupplier.SupplierReportingPeriodStatus.Id = supplierReportingPerionStatus.Id;
            unlockedSupplier.SupplierReportingPeriodStatus.Name = supplierReportingPerionStatus.Name;

            //GasSupplyBreakdown ValueObjectList
            var gasSupplyBreakdownVOs = GetGasSupplyBreakdownVOs();
            IEnumerable<PeriodFacilityGasSupplyBreakdown>? gasSupplyBreakdowns = null;

            try
            {
                gasSupplyBreakdowns = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVOs);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(gasSupplyBreakdowns);
            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);

        }

        /// <summary>
        /// Add ReportingPeriodFacility GasSupplyBreakdown Failure case3.
        /// If FacilitySupplyChainStage is not "Production" then throw exception.
        /// For this test case change SupplyChainStage not to "Production" in BasicTestClass --> GenerateFacilityEntitiesForSupplier() method
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownFailsCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //GasSupplyBreakdown ValueObjectList
            var gasSupplyBreakdownVOs = GetGasSupplyBreakdownVOs();
            IEnumerable<PeriodFacilityGasSupplyBreakdown>? gasSupplyBreakdowns = null;

            try
            {
                gasSupplyBreakdowns = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVOs);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(gasSupplyBreakdowns);
            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);

        }

        /// <summary>
        /// Add ReportingPeriodFacility GasSupplyBreakdown Failure case4.
        /// If Per Supplier per site contentValues are not 100 then throw exception.
        /// For this test case change contentValues as total not to be 100 (per Site) in BasicTestClass --> GetGasSupplyBreakdownVOs() method
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownFailsCase4()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //GasSupplyBreakdown ValueObjectList
            var gasSupplyBreakdownVOs = GetGasSupplyBreakdownVOs();
            IEnumerable<PeriodFacilityGasSupplyBreakdown>? gasSupplyBreakdowns = null;

            try
            {
                gasSupplyBreakdowns = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVOs);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(gasSupplyBreakdowns);
            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);

        }

        /// <summary>
        /// Add ReportingPeriodFacility GasSupplyBreakdown Failure case5.
        /// If Per Supplier per facility site is repeated then throw exception.
        /// For this test case set facility with duplicate site and also set contentValues 100 in BasicTestClass --> GetGasSupplyBreakdownVOs() method
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownFailsCase5()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //GasSupplyBreakdown ValueObjectList
            var gasSupplyBreakdownVOs = GetGasSupplyBreakdownVOs();
            IEnumerable<PeriodFacilityGasSupplyBreakdown>? gasSupplyBreakdowns = null;

            try
            {
                gasSupplyBreakdowns = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVOs);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(gasSupplyBreakdowns);
            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);

        }


        #endregion

        #region AddUpdate ReportingPeriodDocuments

        /// <summary>
        /// Add ReportingPeriodDocumentSuccess Case1.
        /// Add record with documentStatus "Processing" where path and error both are null.
        /// </summary>

        [Fact]
        public void AddUpdateReportingPeriodDocumentSuccessCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var facilityRequiredDocumentTypeVos = GetFacilityRequiredDocumentTypeVOs();
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.SubpartC);
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            PeriodFacilityDocument? facilityDocument = null;
            var displayName = "filename.xlsx";
            var documentStatusProcessing = documentStatuses.First(x => x.Name == DocumentStatusValues.Processing);

            try
            {
                facilityDocument = reportingPeriod.AddUpdatePeriodFacilityDocuments(1, 1, displayName, null, documentStatuses, documentType, null, facilityRequiredDocumentTypeVos);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(facilityDocument);
            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);
            Assert.Equal(1, facilityDocument.ReportingPeriodFacilityId);
            Assert.Equal(displayName, facilityDocument.DisplayName);
            Assert.Equal(1, facilityDocument.Version);
            Assert.Equal(null, facilityDocument.Path);
            Assert.Equal(documentType, facilityDocument.DocumentType);
            Assert.Equal(documentStatusProcessing, facilityDocument.DocumentStatus);
            Assert.Equal(null, facilityDocument.ValidationError);
        }

        /// <summary>
        /// Add ReportingPeriodDocumentSuccess Case2.
        /// Update existingRecord from "Processing" to "Validated".
        /// For update that record set path and validation error null.
        /// </summary>

        [Fact]
        public void AddUpdateReportingPeriodDocumentSuccessCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var facilityRequiredDocumentTypeVos = GetFacilityRequiredDocumentTypeVOs();
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.SubpartC);
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            PeriodFacilityDocument? facilityDocument = null;
            var displayName = "filename.xlsx";
            var documentStatusValidated = documentStatuses.First(x => x.Name == DocumentStatusValues.Validated);

            //First add record with documentStatus "Processing"
            reportingPeriod.AddUpdatePeriodFacilityDocuments(1, 1, displayName, null, documentStatuses, documentType, null, facilityRequiredDocumentTypeVos);

            //Set path
            var path = "E:\\Sem10_Project\\SupplierPortal_own\\SupplierPortalAPI\\DataAccess\\DocumentFiles\\filename.xlsx";
            try
            {
                facilityDocument = reportingPeriod.AddUpdatePeriodFacilityDocuments(1, 1, displayName, path, documentStatuses, documentType, null, facilityRequiredDocumentTypeVos);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(facilityDocument);
            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);
            Assert.Equal(1, facilityDocument.ReportingPeriodFacilityId);
            Assert.Equal(displayName, facilityDocument.DisplayName);
            //Assert.Equal(2, facilityDocument.Version);
            Assert.Equal(path, facilityDocument.Path);
            Assert.Equal(documentType, facilityDocument.DocumentType);
            Assert.Equal(documentStatusValidated, facilityDocument.DocumentStatus);
            Assert.Equal(null, facilityDocument.ValidationError);
        }

        /// <summary>
        /// Add ReportingPeriodDocumentSuccess Case3.
        /// Documents are managed by FacilityRequiredDocumentType.
        /// FacilityRequiredDocumentType is decided by ReportingType and SupplyChainStage of Facility.
        /// If DocumentRequirementStatus is "Required" or "Optional" in selected FacilityRequiredDocumentType then only document can be upload.
        /// </summary>

        [Fact]
        public void AddUpdateReportingPeriodDocumentSuccessCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var facilityRequiredDocumentTypeVos = GetFacilityRequiredDocumentTypeVOs();
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.SubpartW);
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            PeriodFacilityDocument? facilityDocument = null;
            var displayName = "filename.xlsx";
            var documentStatusValidated = documentStatuses.First(x => x.Name == DocumentStatusValues.Validated);

            //First add record with documentStatus "Processing"
            reportingPeriod.AddUpdatePeriodFacilityDocuments(1, 1, displayName, null, documentStatuses, documentType, null, facilityRequiredDocumentTypeVos);

            //Set path
            var path = "E:\\Sem10_Project\\SupplierPortal_own\\SupplierPortalAPI\\DataAccess\\DocumentFiles\\filename.xlsx";
            try
            {
                facilityDocument = reportingPeriod.AddUpdatePeriodFacilityDocuments(1, 1, displayName, path, documentStatuses, documentType, null, facilityRequiredDocumentTypeVos);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(facilityDocument);
            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);
            Assert.Equal(1, facilityDocument.ReportingPeriodFacilityId);
            Assert.Equal(displayName, facilityDocument.DisplayName);
            //Assert.Equal(2, facilityDocument.Version);
            Assert.Equal(path, facilityDocument.Path);
            Assert.Equal(documentType, facilityDocument.DocumentType);
            Assert.Equal(documentStatusValidated, facilityDocument.DocumentStatus);
            Assert.Equal(null, facilityDocument.ValidationError);
        }

        /// <summary>
        /// Add ReportingPeriodDocumentSuccess Case4.
        /// If any error is occured during fileUpload or checking fileSize, fileType and fileSignature or upload error then update existingRecord from "Processing" to "HasErrors".
        /// For update that record set validation error and path is null.
        /// </summary>

        [Fact]
        public void AddUpdateReportingPeriodDocumentSuccessCase4()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var facilityRequiredDocumentTypeVos = GetFacilityRequiredDocumentTypeVOs();
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.SubpartC);
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            PeriodFacilityDocument? facilityDocument = null;
            var displayName = "filename.xlsx";
            var documentStatusHasErrors = documentStatuses.First(x => x.Name == DocumentStatusValues.HasErrors);

            //First add record with documentStatus "Processing"
            reportingPeriod.AddUpdatePeriodFacilityDocuments(1, 1, displayName, null, documentStatuses, documentType, null, facilityRequiredDocumentTypeVos);

            //Set validationError
            string validationError = "Filetype is not matched !!";
            try
            {
                facilityDocument = reportingPeriod.AddUpdatePeriodFacilityDocuments(1, 1, displayName, null, documentStatuses, documentType, validationError, facilityRequiredDocumentTypeVos);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(facilityDocument);
            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);
            Assert.Equal(1, facilityDocument.ReportingPeriodFacilityId);
            Assert.Equal(displayName, facilityDocument.DisplayName);
            //Assert.Equal(2, facilityDocument.Version);
            Assert.Equal(null, facilityDocument.Path);
            Assert.Equal(documentType, facilityDocument.DocumentType);
            Assert.Equal(documentStatusHasErrors, facilityDocument.DocumentStatus);
            Assert.NotNull(facilityDocument.ValidationError);
        }

        /// <summary>
        /// Add ReportingPeriodDocumentSuccess Case5
        /// If FacilityReportingPeriodDataStatus is "Submitted" and user upload the document then user can add/upload document.
        /// </summary>

        [Fact]
        public void AddUpdateReportingPeriodDocumentSuccessCase5()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var facilityRequiredDocumentTypeVos = GetFacilityRequiredDocumentTypeVOs();
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.SubpartC);
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            PeriodFacilityDocument? facilityDocument = null;
            var displayName = "filename.xlsx";
            var documentStatusProcessing = documentStatuses.First(x => x.Name == DocumentStatusValues.Processing);

            //First add record with documentStatus "Processing"
            reportingPeriod.AddUpdatePeriodFacilityDocuments(1, 1, displayName, null, documentStatuses, documentType, null, facilityRequiredDocumentTypeVos);

            //Set path
            var path = "E:\\Sem10_Project\\SupplierPortal_own\\SupplierPortalAPI\\DataAccess\\DocumentFiles\\filename.xlsx";
            reportingPeriod.AddUpdatePeriodFacilityDocuments(1, 1, displayName, path, documentStatuses, documentType, null, facilityRequiredDocumentTypeVos);

            //Change FacilityReportingPeriodDataStatus to Submitted
            var statusSubmitted = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.Submitted);
            var statusComplete = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.Complete);
            var periodSupplier = reportingPeriod.PeriodSuppliers.First(x => x.Id == 1);
            var periodFacility = periodSupplier.PeriodFacilities.First(x => x.Id == 1);
            periodFacility.FacilityReportingPeriodDataStatus.Id = statusComplete.Id;
            periodFacility.FacilityReportingPeriodDataStatus.Name = statusComplete.Name;
            reportingPeriod.UpdateAllPeriodFacilityDataStatus(1, statusSubmitted);

            try
            {
                //Try to update that record
                facilityDocument = reportingPeriod.AddUpdatePeriodFacilityDocuments(1, 1, displayName, path, documentStatuses, documentType, null, facilityRequiredDocumentTypeVos);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(facilityDocument);
            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);
            Assert.Equal(1, facilityDocument.ReportingPeriodFacilityId);
            Assert.Equal(displayName, facilityDocument.DisplayName);
            //Assert.Equal(2, facilityDocument.Version);
            Assert.Equal(null, facilityDocument.Path);
            Assert.Equal(documentType, facilityDocument.DocumentType);
            Assert.Equal(documentStatusProcessing, facilityDocument.DocumentStatus);
            Assert.Equal(null, facilityDocument.ValidationError);
        }


        /// <summary>
        /// Add ReportingPeriodDocumentFailure Case1.
        /// If DocumentRequiredStatus is NotAllowed in FacilityRequiredDocumentType then throw exception.
        /// </summary>

        [Fact]
        public void AddUpdateUpdateReportingPeriodDocumentFailsCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var facilityRequiredDocumentTypeVos = GetFacilityRequiredDocumentTypeVOs();
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.NonGHGRP);
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            PeriodFacilityDocument? facilityDocument = null;

            try
            {
                facilityDocument = reportingPeriod.AddUpdatePeriodFacilityDocuments(1, 1, "filename.xlsx", null, documentStatuses, documentType, null, facilityRequiredDocumentTypeVos);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(facilityDocument);
            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);
        }


        /// <summary>
        /// Add ReportingPeriodDocumentFailure Case2.
        /// If existing record path is not null then throw exception.
        /// </summary>

        [Fact]
        public void AddUpdateReportingPeriodDocumentFailsCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var facilityRequiredDocumentTypeVos = GetFacilityRequiredDocumentTypeVOs();
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.SubpartC);
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            PeriodFacilityDocument? facilityDocument = null;

            //First add record with documentStatus "Processing"
            reportingPeriod.AddUpdatePeriodFacilityDocuments(1, 1, "filename.xlsx", null, documentStatuses, documentType, null, facilityRequiredDocumentTypeVos);

            //Set path
            var path = "E:\\Sem10_Project\\SupplierPortal_own\\SupplierPortalAPI\\DataAccess\\DocumentFiles\\filename.xlsx";
            reportingPeriod.AddUpdatePeriodFacilityDocuments(1, 1, "filename.xlsx", path, documentStatuses, documentType, null, facilityRequiredDocumentTypeVos);
            try
            {
                facilityDocument = reportingPeriod.AddUpdatePeriodFacilityDocuments(1, 1, "filename.xlsx", path, documentStatuses, documentType, null, facilityRequiredDocumentTypeVos);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(facilityDocument);
            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);
        }

        [Fact]
        public void RemovePeriodFacilityDocumentSuccessCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var facilityRequiredDocumentTypeVos = GetFacilityRequiredDocumentTypeVOs();
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.SubpartC);
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            var displayName = "filename.xlsx";
            var documentStatusProcessing = documentStatuses.First(x => x.Name == DocumentStatusValues.Processing);
            var facilityDocument = reportingPeriod.AddUpdatePeriodFacilityDocuments(1, 1, displayName, null, documentStatuses, documentType, null, facilityRequiredDocumentTypeVos);
            facilityDocument.Id = 1;
            bool isRemoved = false;

            //Update FacilityReportingPeriodDataStatus Submitted
            var statusSubmitted = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.Submitted);
            var statusComplete = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.Complete);
            var periodSupplier = reportingPeriod.PeriodSuppliers.First(x => x.Id == 1);
            var periodFacility = periodSupplier.PeriodFacilities.First(x => x.Id == 1);
            periodFacility.FacilityReportingPeriodDataStatus.Id = statusComplete.Id;
            periodFacility.FacilityReportingPeriodDataStatus.Name = statusComplete.Name;
            reportingPeriod.UpdateAllPeriodFacilityDataStatus(1, statusSubmitted);

            try
            {
                isRemoved = reportingPeriod.RemovePeriodFacilityDocument(1, 1);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(true, isRemoved);

        }

        #endregion

        #region AddUpdate ReportingPeriodSupplierDocument

        /// <summary>
        /// AddUpdate ReportingPeriodSupplierDocumentSuccess Case1
        /// Add new record with DocumentStatus "Processing" where path and validationError should be null
        /// </summary>
        [Fact]
        public void AddUpdateReportingPeriodSupplierDocumentSuccessCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.Supplemental);
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            UpdateReportingPeriodClosed(reportingPeriod);
            PeriodSupplierDocument? supplierDocument = null;
            var displayName = "filename.xlsx";
            var documentStatusProcessing = documentStatuses.First(x => x.Name == DocumentStatusValues.Processing);

            try
            {
                supplierDocument = reportingPeriod.AddUpdatePeriodSupplierDocument(1, displayName, null, documentStatuses, documentType, null);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(supplierDocument);
            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);
            Assert.Equal(1, supplierDocument.ReportingPeriodSupplierId);
            Assert.Equal(displayName, supplierDocument.DisplayName);
            Assert.Equal(1, supplierDocument.Version);
            Assert.Equal(null, supplierDocument.Path);
            Assert.Equal(documentType, supplierDocument.DocumentType);
            Assert.Equal(documentStatusProcessing, supplierDocument.DocumentStatus);
            Assert.Equal(null, supplierDocument.ValidationError);
        }

        /// <summary>
        /// Add ReportingPeriodSupplierDocumentSuccess Case2.
        /// Update existingRecord from "Processing" to "Validated".
        /// For update that record set path and validation error null.
        /// </summary>

        [Fact]
        public void AddUpdateReportingPeriodSupplierDocumentSuccessCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.Supplemental);
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            UpdateReportingPeriodClosed(reportingPeriod);
            PeriodSupplierDocument? supplierDocument = null;
            var displayName = "filename.xlsx";
            var documentStatusProcessing = documentStatuses.First(x => x.Name == DocumentStatusValues.Validated);

            //Add record with status "Processing"
            reportingPeriod.AddUpdatePeriodSupplierDocument(1, displayName, null, documentStatuses, documentType, null);

            //Set path
            var path = "E:\\Sem10_Project\\SupplierPortal_own\\SupplierPortalAPI\\DataAccess\\DocumentFiles\\filename.xlsx";
            try
            {
                supplierDocument = reportingPeriod.AddUpdatePeriodSupplierDocument(1, displayName, path, documentStatuses, documentType, null);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(supplierDocument);
            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);
            Assert.Equal(1, supplierDocument.ReportingPeriodSupplierId);
            Assert.Equal(displayName, supplierDocument.DisplayName);
            //Assert.Equal(1, supplierDocument.Version);
            Assert.Equal(path, supplierDocument.Path);
            Assert.Equal(documentType, supplierDocument.DocumentType);
            Assert.Equal(documentStatusProcessing, supplierDocument.DocumentStatus);
            Assert.Equal(null, supplierDocument.ValidationError);
        }

        /// <summary>
        /// Add ReportingPeriodSupplierDocumentSuccess Case3
        /// If any error is occured during fileUpload or checking fileSize, fileType and   fileSignature or upload error then update existingRecord from "Processing" to "HasErrors".
        /// For update that record set validation error and path is null.
        /// </summary>

        [Fact]
        public void AddUpdateReportingPeriodSupplierDocumentSuccessCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.Supplemental);
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            UpdateReportingPeriodClosed(reportingPeriod);
            PeriodSupplierDocument? supplierDocument = null;
            var displayName = "filename.xlsx";
            var documentStatusProcessing = documentStatuses.First(x => x.Name == DocumentStatusValues.HasErrors);

            //Add record with status "Processing"
            reportingPeriod.AddUpdatePeriodSupplierDocument(1, displayName, null, documentStatuses, documentType, null);

            //Set validationError
            string validationError = "Filetype is not matched !!";
            try
            {
                supplierDocument = reportingPeriod.AddUpdatePeriodSupplierDocument(1, displayName, null, documentStatuses, documentType, validationError);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(supplierDocument);
            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);
            Assert.Equal(1, supplierDocument.ReportingPeriodSupplierId);
            Assert.Equal(displayName, supplierDocument.DisplayName);
            //Assert.Equal(1, supplierDocument.Version);
            Assert.Equal(null, supplierDocument.Path);
            Assert.Equal(documentType, supplierDocument.DocumentType);
            Assert.Equal(documentStatusProcessing, supplierDocument.DocumentStatus);
            Assert.NotNull(supplierDocument.ValidationError);
        }

        /// <summary>
        /// AddUpdate ReportingPeriodSupplierDocumentFailure Case1
        /// If documentType is not "Supplemental" then throw exception
        /// </summary>

        [Fact]
        public void AddUpdateReportingPeriodSupplierDocumentFailsCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.NonGHGRP);
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            UpdateReportingPeriodClosed(reportingPeriod);
            PeriodSupplierDocument? supplierDocument = null;
            var displayName = "filename.xlsx";

            try
            {
                supplierDocument = reportingPeriod.AddUpdatePeriodSupplierDocument(1, displayName, null, documentStatuses, documentType, null);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(supplierDocument);
            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);
        }

        /// <summary>
        /// AddUpdate ReportingPeriodSupplierDocumentFailure Case2
        /// If ReportingPeriod is not closed then throw exception
        /// </summary>

        [Fact]
        public void AddUpdateReportingPeriodSupplierDocumentFailsCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.Supplemental);
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            PeriodSupplierDocument? supplierDocument = null;
            var displayName = "filename.xlsx";

            try
            {
                supplierDocument = reportingPeriod.AddUpdatePeriodSupplierDocument(1, displayName, null, documentStatuses, documentType, null);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(supplierDocument);
            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);
        }
       
        /// <summary>
        /// AddUpdate ReportingPeriodSupplierDocumentFailure Case2
        /// If SupplierReportingPeriodStatus is locked and existing record path is not null then throw exception
        /// </summary>

        [Fact]
        public void AddUpdateReportingPeriodSupplierDocumentFailsCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.Supplemental);
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            UpdateReportingPeriodClosed(reportingPeriod);
            PeriodSupplierDocument? supplierDocument = null;
            var displayName = "filename.xlsx";

            //First add record with documentStatus "Processing"
            reportingPeriod.AddUpdatePeriodSupplierDocument(1, displayName, null, documentStatuses, documentType, null);

            //Set path
            var path = "E:\\Sem10_Project\\SupplierPortal_own\\SupplierPortalAPI\\DataAccess\\DocumentFiles\\filename.xlsx";
            reportingPeriod.AddUpdatePeriodSupplierDocument(1, displayName, path, documentStatuses, documentType, null);

            //Update SupplierReportingPeriodStatus Locked
            var lockedStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Locked);
            UpdateSupplierReportingPeriodStatusLocked(reportingPeriod, lockedStatus);

            try
            {
                supplierDocument = reportingPeriod.AddUpdatePeriodSupplierDocument(1, displayName, path, documentStatuses, documentType, null);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(supplierDocument);
            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);
        }

        [Fact]
        public void RemovePeriodSupplierDocumentSuccessCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.Supplemental);
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            UpdateReportingPeriodClosed(reportingPeriod);
            var displayName = "filename.xlsx";
            var supplierDocument = reportingPeriod.AddUpdatePeriodSupplierDocument(1, displayName, null, documentStatuses, documentType, null);
            supplierDocument.Id = 1;
            bool isRemoved = false;

            try
            {
                isRemoved = reportingPeriod.RemovePeriodSupplierDocument(1, 1);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(true, isRemoved);
        }


        #endregion

        #region Delete methods

        /// <summary>
        /// Delete ReportingPeriodFacility ElectricityGridMixes
        /// </summary>
        [Fact]

        public void DeletePeriodFacilityElectricityGridMix()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            //GridMix
            var unitOfMeasure = GetUnitOfMeasures().First(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.Custom_Mix);
            var gridMixComponentPercents = GetElectricityGridMixComponentPercents();
            var list = reportingPeriod.AddRemoveElectricityGridMixComponents(1, 1, unitOfMeasure, fercRegion, gridMixComponentPercents);
            int count = 0;

            try
            {
                count = reportingPeriod.DeletePeriodFacilityElectricityGridMixes(1, 1);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);
            Assert.Equal(0, count);

        }

        /// <summary>
        /// Delete ReportingPeriodSupplier GasSupplyBreakdowns
        /// </summary>

        [Fact]
        public void DeletePeriodSupplierGasSupplyBreakdown()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            var gasSupplyBreakdownVOs = GetGasSupplyBreakdownVOs();
            var gasSupplyBreakdowns = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVOs);
            int count = 0;

            try
            {
                count = reportingPeriod.DeletePeriodSupplierGasSupplyBreakdowns(1);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(0, exceptionCounter);
            Assert.Equal(0, count);
            Assert.Null(exceptionMessage);
        }

        #endregion

        #region SendMail
        /// <summary>
        /// Check InitialDataRequestDate null or not success Case1.
        /// If InitialDataRequestDate is null then it will return the SupplierContact EmailIds list
        /// InitialDataRequestDate and ResendDataRequestDate should be null.
        /// </summary>
        [Fact]
        public void CheckInitialOrResendDataRequestDateAndGetContactEmailsSuccessCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            var list = new List<string>();

            try
            {
                list = reportingPeriod.CheckInitialOrResendDataRequestDateAndGetContactEmails(1);
            }
            catch(Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);
            Assert.NotEqual(0, list.Count());
        }

        /// <summary>
        /// Check InitialDataRequestDate null or not success Case2.
        /// If ResendDataRequestDate is null and ReportingPeriodEndDate is null then check for ResendDataRequest mail can send or not.
        /// If timelimitDate is less than CurrentDate then return contactEmailsList.
        /// For this UnitTesting set InitialDataRequestDate to one month PastDate in BasicTestClass -> AddPeriodSupplierAndPeriodFacilityForPeriod() -> AddPeriodSupplier(....)
        /// ReportingPeriodEndDate and ResendDataRequestDate should be null by default.
        /// </summary>
        [Fact]
        public void CheckInitialOrResendDataRequestDateAndGetContactEmailsSuccessCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            var list = new List<string>();

            try
            {
                list = reportingPeriod.CheckInitialOrResendDataRequestDateAndGetContactEmails(1);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);
            Assert.NotEqual(0, list.Count());
        }

        /// <summary>
        /// Check InitialDataRequestDate null or not success Case3.
        /// If ResendDataRequestDate is null and ReportingPeriodEndDate is not null then check for ResendDataRequest mail can send or not.
        /// If endDate is less than CurrentDate then return contactEmailsList.
        /// For this UnitTesting set InitialDataRequestDate to one month PastDate in BasicTestClass -> AddPeriodSupplierAndPeriodFacilityForPeriod() -> AddPeriodSupplier(....)
        /// ReportingPeriodEndDate should be in past.
        /// ResendDataRequestDate should be null by default.
        /// </summary>
        [Fact]
        public void CheckInitialOrResendDataRequestDateAndGetContactEmailsSuccessCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            var list = new List<string>();

            try
            {
                list = reportingPeriod.CheckInitialOrResendDataRequestDateAndGetContactEmails(1);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(exceptionMessage);
            Assert.Equal(0, exceptionCounter);
            Assert.NotEqual(0, list.Count());
        }

        /// <summary>
        /// Check ResendDataRequestDate null or not failure Case1.
        /// If supplier contacts not found then throw exception.
        /// For this UnitTesting set IsActive = False in BasicTestClass --> GenerateContactEntitiesForSupplier() method
        /// </summary>

        [Fact]
        public void CheckInitialOrResendDataRequestDateAndGetContactEmailsFailureCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            var list = new List<string>();

            try
            {
                list = reportingPeriod.CheckInitialOrResendDataRequestDateAndGetContactEmails(1);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);
            Assert.Equal(0, list.Count());
        }

        /// <summary>
        /// Check ResendDataRequestDate null or not failure Case2.
        /// If ResendDataRequest date is null and ReportingPeriodEndDate is null then check for ResendDataRequest mail can send or not.
        /// If timelimitDate is greater than CurrentDate then throw exception.
        /// For this UnitTesting set InitialDataRequestDate to currentDate in BasicTestClass -> AddPeriodSupplierAndPeriodFacilityForPeriod() -> AddPeriodSupplier(....)
        /// InitialDataRequestDate should be currentDate
        /// </summary>

        [Fact]
        public void CheckInitialOrResendDataRequestDateAndGetContactEmailsFailureCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            var list = new List<string>();

            try
            {
                list = reportingPeriod.CheckInitialOrResendDataRequestDateAndGetContactEmails(1);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);
            Assert.Equal(0, list.Count());
        }

        /// <summary>
        /// Check ResendDataRequestDate null or not failure Case3.
        /// If ResendDataRequest date is null and ReportingPeriodEndDate is not null then check for ResendDataRequest mail can send or not.
        /// If endDate is greater than CurrentDate then throw exception.
        /// For this UnitTesting set InitialDataRequestDate to currentDate in BasicTestClass -> AddPeriodSupplierAndPeriodFacilityForPeriod() -> AddPeriodSupplier(....)
        /// ReportingPeriodEndDate in BasicTestClass -> set endDate variable (top of the code) should be in future
        /// </summary>

        [Fact]
        public void CheckInitialOrResendDataRequestDateAndGetContactEmailsFailureCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            var list = new List<string>();

            try
            {
                list = reportingPeriod.CheckInitialOrResendDataRequestDateAndGetContactEmails(1);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);
            Assert.Equal(0, list.Count());
        }

        /// <summary>
        /// Check ResendDataRequestDate null or not failure Case4.
        /// If ResendDataRequest date and InitialDataRequestDate both are not null then throw exception.
        /// For this UnitTesting set InitialDataRequestDate & ResendDataRequestDate in BasicTestClass -> AddPeriodSupplierAndPeriodFacilityForPeriod() -> AddPeriodSupplier(....)
        /// </summary>

        [Fact]
        public void CheckInitialOrResendDataRequestDateAndGetContactEmailsFailureCase4()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            var list = new List<string>();

            try
            {
                list = reportingPeriod.CheckInitialOrResendDataRequestDateAndGetContactEmails(1);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(exceptionMessage);
            Assert.NotEqual(0, exceptionCounter);
            Assert.Equal(0, list.Count());
        }

        #endregion
    }
}
