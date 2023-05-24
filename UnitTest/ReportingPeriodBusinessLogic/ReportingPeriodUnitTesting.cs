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
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.CustomMix);

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
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            //Add PeriodFacility
            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.CustomMix);

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
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodDataStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.Complete);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.CustomMix);

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
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodDataStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.CustomMix);

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
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            //Add new  PeriodFacility
            var supplyChainStage = GenerateSupplyChainStage().First();
            var reportingType = GenerateReportingType().First();
            var facilityVO = new FacilityVO(10, "Test facility", 2, "123", true, supplyChainStage, reportingType);
            var facilityReportingPeriodStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.CustomMix);

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

        #region PeriodFacilityElectricityGridMix

        /// <summary>
        /// Add PeriodFacilityElectricityGridMix Success case1
        /// FercRegion is Custom Mix for add and Content value sum is 100
        /// </summary>
        [Fact]
        public void AddPeriodFacilityElectricityGridMixSuccessCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Open);
            reportingPeriod.ReportingPeriodStatus.Id = reportingPeriodStatus.Id;
            reportingPeriod.ReportingPeriodStatus.Name = reportingPeriodStatus.Name;

            //ElectricityGridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.CustomMix);
            var electricityGridMixComponentPercents = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;

            try
            {
                list = reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1, unitOfMeasure,fercRegion, electricityGridMixComponentPercents);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(list);
            Assert.Equal(electricityGridMixComponentPercents.Count(), list.Count());

            for (int i = 0; i < electricityGridMixComponentPercents.Count(); i++)
            {
                var originalElectricityGridMix = electricityGridMixComponentPercents.ToList()[i].ElectricityGridMixComponent;
                var addedElectricityGridMix = list.ToList()[i].ElectricityGridMixComponent;
                Assert.Equal(addedElectricityGridMix, originalElectricityGridMix);
            }

            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }

        /// <summary>
        /// Add PeriodFacilityElectricityGridMix Success case2
        /// If periodFacilityElectricityGridMix is already exists than replace old electricityGridMix
        /// </summary>
        [Fact]
        public void AddPeriodFacilityElectricityGridMixSuccessCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //ElectricityGridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.CustomMix);
            var electricityGridMixComponentPercents = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;
            reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1, unitOfMeasure,fercRegion, electricityGridMixComponentPercents);
            var percents = GetElectricityGridMixComponentPercents2();

            try
            {
                list = reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1, unitOfMeasure,fercRegion, percents);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(list);
            Assert.NotEqual(electricityGridMixComponentPercents.Count(), list.Count());
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }

        /// <summary>
        /// Add PeriodFacilityElectricityGridMix Success case3
        /// If periodFacilityElectricityGridMix is already exists and update fercRegion 
        /// For this test case - FercRegion is CustomMix to None
        /// </summary>
        [Fact]
        public void AddPeriodFacilityElectricityGridMixSuccessCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //ElectricityGridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.CustomMix);
            var electricityGridMixComponentPercents = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;
            reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1, unitOfMeasure,fercRegion, electricityGridMixComponentPercents);
            var changedFercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.None);

            var periodSupplier = reportingPeriod.PeriodSuppliers.First(x => x.Id == 1);
            var periodFacility = periodSupplier.PeriodFacilities.First(x => x.Id == 1);
            periodFacility.FercRegion.Id = fercRegion.Id;
            periodFacility.FercRegion.Name = fercRegion.Name;

            var percents = new List<ElectricityGridMixComponentPercent>();

            try
            {
                list = reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1, unitOfMeasure,changedFercRegion, percents);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(list);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }

        /// <summary>
        /// Add PeriodFacilityElectricityGridMix Fail case1
        /// FercRegion is not Custom Mix then throw exception
        /// </summary>
        [Fact]
        public void AddPeriodFacilityElectricityGridMixFailCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //ElectricityGridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var electricityGridMixComponentPercents = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.None);

            var periodSupplier = reportingPeriod.PeriodSuppliers.First(x => x.Id == 1);
            var periodFacility = periodSupplier.PeriodFacilities.First(x => x.Id == 1);
            //periodFacility.FercRegion.Id = fercRegion.Id;
            //periodFacility.FercRegion.Name = fercRegion.Name;

            try
            {
                list = reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1, unitOfMeasure,fercRegion, electricityGridMixComponentPercents);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }

        /// <summary>
        /// Add PeriodFacilityElectricityGridMix Fail case2
        /// FercRegion is Custom Mix and gridMixComponentPercent is empty then throw exception
        /// </summary>
        [Fact]
        public void AddPeriodFacilityElectricityGridMixFailCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //ElectricityGridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.CustomMix);
            var percents = new List<ElectricityGridMixComponentPercent>();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;

            try
            {
                list = reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1, unitOfMeasure,fercRegion, percents);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }

        /// <summary>
        /// Add PeriodFacilityElectricityGridMix Fail case3
        /// If any gridMixComponentPercent is repeat then throw exception
        /// For this testCase - GridMixComponent Value is repeat.
        /// </summary>
        [Fact]
        public void AddPeriodFacilityElectricityGridMixFailCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //ElectricityGridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.CustomMix);
            var electricityGridMixComponentPercent = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;

            try
            {
                list = reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1, unitOfMeasure,fercRegion, electricityGridMixComponentPercent);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }

        /// <summary>
        /// Add PeriodFacilityElectricityGridMix Fail case4
        /// if Content value total is not 100 then throw exception
        /// For this testCase - Content values is changed.
        /// </summary>
        [Fact]
        public void AddPeriodFacilityElectricityGridMixFailCase4()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //ElecetricityGridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.CustomMix);
            var electricityGridMixComponentPercents = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;

            try
            {
                list = reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1, unitOfMeasure,fercRegion, electricityGridMixComponentPercents);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }

        /// <summary>
        /// Add PeriodFacilityElectricityGridMix Fail case5
        /// If reportingPeriodStatus is InActive or Complete than throw exception
        /// for this testcase go to AddPeriodSupplierAndPeriodFacilityForReportingPeriod method and set the ReportingPeriodStatus is InActive or Complete
        /// </summary>
        [Fact]
        public void AddPeriodFacilityElectricityGridMixFailCase5()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //ElectricityGridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.CustomMix);
            var electricityGridMixComponentPercents = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;

            try
            {
                list = reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1, unitOfMeasure,fercRegion, electricityGridMixComponentPercents);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }

        /// <summary>
        /// Add PeriodFacilityElectricityGridMix Fail case6
        /// If periodSupplierStatus is Locked than throw exception
        /// </summary>
        [Fact]
        public void AddPeriodFacilityElectricityGridMixFailCase6()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //Update SupplierReportingPeriodStatus Unlocked to Locked
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Locked);
            var unlockedPeriodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == 1);
            unlockedPeriodSupplier.SupplierReportingPeriodStatus.Id = supplierReportingPerionStatus.Id;
            unlockedPeriodSupplier.SupplierReportingPeriodStatus.Name = supplierReportingPerionStatus.Name;

            //ElectricityGridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.CustomMix);
            var electricityGridMixComponentPercents = GetElectricityGridMixComponentPercents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;

            try
            {
                list = reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1, unitOfMeasure,fercRegion, electricityGridMixComponentPercents);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }

        #endregion

        #region PeriodFacilityGasSupplyBreakdown

        /// <summary>
        /// Add PeriodFacilityGasSupplyBreakdown Success case1
        /// Per periodSupplier per site ContentValues should be 100
        /// For all test cases set ReportingPeriodStatus should be "Open" , SupplierReportingPeriodStatus should be "Unlocked" and SupplyChainStage should be "Production"
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownSuccessCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var gasSupplyBreakdownVos = GetGasSupplyBreakdowns();
            IEnumerable<PeriodFacilityGasSupplyBreakdown>? list = null;

            try
            {
                list = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVos);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(list);
            Assert.Equal(gasSupplyBreakdownVos.Count(), list.Count());

            for (int i = 0; i < gasSupplyBreakdownVos.Count(); i++)
            {
                var originalGasSupply = gasSupplyBreakdownVos.ToList()[i].Site;
                var addedGasSupply = list.ToList()[i].Site;
                Assert.Equal(addedGasSupply, originalGasSupply);
            }

            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }

        /// <summary>
        /// Add PeriodFacilityGasSupplyBreakdown Success case2
        /// If per PeriodSupplier periodFacilityGasSupplyBreakdown is already exists than replace old GasSupplyBreakdown
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownSuccessCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Open);
            reportingPeriod.ReportingPeriodStatus.Id = reportingPeriodStatus.Id;
            reportingPeriod.ReportingPeriodStatus.Name = reportingPeriodStatus.Name;

            var gasSupplyBreakdownVos = GetGasSupplyBreakdowns();
            IEnumerable<PeriodFacilityGasSupplyBreakdown>? list = null;
            reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVos);
            var gasSupplyBreakdownVos2 = GetGasSupplyBreakdowns2();

            try
            {
                list = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVos2);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(list);
            Assert.NotEqual(gasSupplyBreakdownVos.Count(), list.Count());
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }

        /// <summary>
        /// Add PeriodFacilityGasSupplyBreakdown Fail case1
        /// If ReportingPeriodStatus is InActive than throw exception
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownFailCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //Update reportingPeriodStatus Open to InActive
            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.InActive);
            reportingPeriod.ReportingPeriodStatus.Id = reportingPeriodStatus.Id;
            reportingPeriod.ReportingPeriodStatus.Name = reportingPeriodStatus.Name;

            var gasSupplyBreakdownVos = GetGasSupplyBreakdowns();
            IEnumerable<PeriodFacilityGasSupplyBreakdown>? list = null;

            try
            {
                list = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVos);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }

        /// <summary>
        /// Add PeriodFacilityGasSupplyBreakdown Fail case2
        /// If SupplierReportingPeriodStatus is Locked than throw exception
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownFailCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //Update SupplierReportingPeriodStatus Unlocked to Locked
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Locked);
            var unlockedPeriodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == 1);
            unlockedPeriodSupplier.SupplierReportingPeriodStatus.Id = supplierReportingPerionStatus.Id;
            unlockedPeriodSupplier.SupplierReportingPeriodStatus.Name = supplierReportingPerionStatus.Name;

            //Add GasSupplyBreakdownVo
            var gasSupplyBreakdownVos = GetGasSupplyBreakdowns();
            IEnumerable<PeriodFacilityGasSupplyBreakdown>? list = null;

            try
            {
                list = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVos);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }

        /// <summary>
        /// Add PeriodFacilityGasSupplyBreakdown Fail case3
        /// If SupplyChainStage is not Production than throw exception
        /// For this test case - Changed SupplyChainStage is Production to Others in method GenerateFacilityEntitiesForSupplier()
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownFailCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //Add GasSupplyBreakdownVo
            var gasSupplyBreakdownVos = GetGasSupplyBreakdowns();
            IEnumerable<PeriodFacilityGasSupplyBreakdown>? list = null;

            try
            {
                list = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVos);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }

        /// <summary>
        /// Add PeriodFacilityGasSupplyBreakdown Fail case4
        /// If per PeriodSupplier per site content values is not 100 than throw exception
        /// For this test case - Changed the content values
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownFailCase4()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //Add GasSupplyBreakdownVo
            var gasSupplyBreakdownVos = GetGasSupplyBreakdowns();
            IEnumerable<PeriodFacilityGasSupplyBreakdown>? list = null;

            try
            {
                list = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVos);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }

        /// <summary>
        /// Add PeriodFacilityGasSupplyBreakdown Fail case5
        /// If Per periodSupplier per PeriodFacility site is repeated than throw exception
        /// For this test case - Add repeated data for different content values and site is same in GetGasSupplyBreakdowns() method
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownFailCase5()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //Add GasSupplyBreakdownVo
            var gasSupplyBreakdownVos = GetGasSupplyBreakdowns();
            IEnumerable<PeriodFacilityGasSupplyBreakdown>? list = null;

            try
            {
                list = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVos);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Null(list);
            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }

        #endregion

        #region PeriodFacilityDocument

        /// <summary>
        /// AddUpdate PeriodFacilityDocument success case1
        /// existingDocument count is 0
        /// </summary>
        [Fact]
        public void AddUpdatePeriodFacilityDocumentSuccessCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.SubpartC);
            var facilityRequiredDocumentTypeVOs = GetFacilityRequiredDocumentTypeVOs();

            var displayName = "abc.xlsx";
            var path = "E:\\Bigscal\\ICT_4\\SupplierPortalAPI\\SupplierPortalAPI\\DataAccess\\UploadedFiles\\P6-AmazingMartEU2Geo.xlsx";

            PeriodFacilityDocument? periodFacilityDocument = null;

            try
            {
                periodFacilityDocument = reportingPeriod.AddPeriodFacilityDocument(1, 1, displayName,path, null,documentStatuses,documentType,facilityRequiredDocumentTypeVOs);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(1, periodFacilityDocument.ReportingPeriodFacilityId);
            Assert.Equal(displayName, periodFacilityDocument.DisplayName);
            Assert.Equal(null, periodFacilityDocument.Path);
            Assert.Equal(null, periodFacilityDocument.ValidationError);
            Assert.Equal(documentStatuses.First(x => x.Name == DocumentStatusValues.Processing), periodFacilityDocument.DocumentStatus);
            Assert.Equal(documentType,periodFacilityDocument.DocumentType);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }

        /// <summary>
        /// AddUpdate PeriodFacilityDocument success case2
        /// existingDocument is already exists than update the facilityDocument 
        /// path is not null and validationError is null than update the documentStatus is Validated
        /// </summary>
        [Fact]
        public void AddUpdatePeriodFacilityDocumentSuccessCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.SubpartC);
            var facilityRequiredDocumentTypeVOs = GetFacilityRequiredDocumentTypeVOs();

            var displayName = "abc.xlsx";

            reportingPeriod.AddPeriodFacilityDocument(1, 1, displayName, null, null, documentStatuses, documentType, facilityRequiredDocumentTypeVOs);

            var path = "E:\\Bigscal\\ICT_4\\SupplierPortalAPI\\SupplierPortalAPI\\DataAccess\\UploadedFiles\\P6-AmazingMartEU2Geo.xlsx";

            PeriodFacilityDocument? periodFacilityDocument = null;

            try
            {
                periodFacilityDocument = reportingPeriod.AddPeriodFacilityDocument(1, 1, displayName, path, null, documentStatuses, documentType, facilityRequiredDocumentTypeVOs);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(1, periodFacilityDocument.ReportingPeriodFacilityId);
            Assert.Equal(displayName, periodFacilityDocument.DisplayName);
            Assert.Equal(path, periodFacilityDocument.Path);
            Assert.Equal(null, periodFacilityDocument.ValidationError);
            Assert.Equal(documentStatuses.First(x => x.Name == DocumentStatusValues.Validated), periodFacilityDocument.DocumentStatus);
            Assert.Equal(documentType, periodFacilityDocument.DocumentType);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }

        /// <summary>
        /// AddUpdate PeriodFacilityDocument success case3
        /// if facilityDataStatus is submitted than update the existingDocument
        /// </summary>
        [Fact]
        public void AddUpdatePeriodFacilityDocumentSuccessCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.SubpartC);
            var facilityRequiredDocumentTypeVOs = GetFacilityRequiredDocumentTypeVOs();

            var displayName = "abc.xlsx";

            reportingPeriod.AddPeriodFacilityDocument(1, 1, displayName, null, null, documentStatuses, documentType, facilityRequiredDocumentTypeVOs);

            var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == 1);
            var periodFacility = periodSupplier.PeriodFacilities.FirstOrDefault(x => x.Id == 1);
            var facilityDataStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.Submitted);
            periodFacility.FacilityReportingPeriodDataStatus.Id = facilityDataStatus.Id;
            periodFacility.FacilityReportingPeriodDataStatus.Name = facilityDataStatus.Name;

            var path = "E:\\Bigscal\\ICT_4\\SupplierPortalAPI\\SupplierPortalAPI\\DataAccess\\UploadedFiles\\P6-AmazingMartEU2Geo.xlsx";

            PeriodFacilityDocument? periodFacilityDocument = null;

            try
            {
                periodFacilityDocument = reportingPeriod.AddPeriodFacilityDocument(1, 1, displayName, path, null, documentStatuses, documentType, facilityRequiredDocumentTypeVOs);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(1, periodFacilityDocument.ReportingPeriodFacilityId);
            Assert.Equal(displayName, periodFacilityDocument.DisplayName);
            Assert.Equal(null, periodFacilityDocument.Path);
            Assert.Equal(null, periodFacilityDocument.ValidationError);
            Assert.Equal(documentStatuses.First(x => x.Name == DocumentStatusValues.Processing), periodFacilityDocument.DocumentStatus);
            Assert.Equal(documentType, periodFacilityDocument.DocumentType);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }

        /// <summary>
        /// AddUpdate PeriodFacilityDocument success case4
        /// validation error store and documentstatus is changed in HasError
        /// </summary>
        [Fact]
        public void AddUpdatePeriodFacilityDocumentSuccessCase4()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.SubpartC);
            var facilityRequiredDocumentTypeVOs = GetFacilityRequiredDocumentTypeVOs();

            var displayName = "abc.xlsx";

            reportingPeriod.AddPeriodFacilityDocument(1, 1, displayName, null, null, documentStatuses, documentType, facilityRequiredDocumentTypeVOs);

            var path = "E:\\Bigscal\\ICT_4\\SupplierPortalAPI\\SupplierPortalAPI\\DataAccess\\UploadedFiles\\P6-AmazingMartEU2Geo.xlsx";

            var validationError = "Unable to save the uploaded file at this time.Please attempt the upload again later.";

            PeriodFacilityDocument? periodFacilityDocument = null;

            try
            {
                periodFacilityDocument = reportingPeriod.AddPeriodFacilityDocument(1, 1, displayName, path, validationError, documentStatuses, documentType, facilityRequiredDocumentTypeVOs);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(1, periodFacilityDocument.ReportingPeriodFacilityId);
            Assert.Equal(displayName, periodFacilityDocument.DisplayName);
            Assert.Equal(null, periodFacilityDocument.Path);
            Assert.Equal(validationError, periodFacilityDocument.ValidationError);
            Assert.Equal(documentStatuses.First(x => x.Name == DocumentStatusValues.HasErrors), periodFacilityDocument.DocumentStatus);
            Assert.Equal(documentType, periodFacilityDocument.DocumentType);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }

        /// <summary>
        /// AddUpdate PeriodFacilityDocument success case2
        /// existingDocument is already exists than update the facilityDocument 
        /// path is null and validationError is null than update the documentStatus is NotValidated
        /// </summary>
        [Fact]
        public void AddUpdatePeriodFacilityDocumentSuccessCase5()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.SubpartC);
            var facilityRequiredDocumentTypeVOs = GetFacilityRequiredDocumentTypeVOs();

            var displayName = "abc.xlsx";

            reportingPeriod.AddPeriodFacilityDocument(1, 1, displayName, null, null, documentStatuses, documentType, facilityRequiredDocumentTypeVOs);

            PeriodFacilityDocument? periodFacilityDocument = null;

            try
            {
                periodFacilityDocument = reportingPeriod.AddPeriodFacilityDocument(1, 1, displayName, null, null, documentStatuses, documentType, facilityRequiredDocumentTypeVOs);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(1, periodFacilityDocument.ReportingPeriodFacilityId);
            Assert.Equal(displayName, periodFacilityDocument.DisplayName);
            Assert.Equal(null, periodFacilityDocument.Path);
            Assert.Equal(null, periodFacilityDocument.ValidationError);
            Assert.Equal(documentStatuses.First(x => x.Name == DocumentStatusValues.NotValidated), periodFacilityDocument.DocumentStatus);
            Assert.Equal(documentType, periodFacilityDocument.DocumentType);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }

        /// <summary>
        /// AddUpdate PeriodFacilityDocument fail case1
        /// if file path is already exists in the system than throw exception
        /// </summary>
        [Fact]
        public void AddUpdatePeriodFacilityDocumentFailCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.SubpartC);
            var facilityRequiredDocumentTypeVOs = GetFacilityRequiredDocumentTypeVOs();

            reportingPeriod.AddPeriodFacilityDocument(1, 1, "abc.xlsx", null, null, documentStatuses, documentType, facilityRequiredDocumentTypeVOs);

            var path = "E:\\Bigscal\\ICT_4\\SupplierPortalAPI\\SupplierPortalAPI\\DataAccess\\UploadedFiles\\P6-AmazingMartEU2Geo.xlsx";
            try
            {
                reportingPeriod.AddPeriodFacilityDocument(1, 1, "abc.xlsx", path, null, documentStatuses, documentType, facilityRequiredDocumentTypeVOs);
                path = "E:\\Bigscal\\ICT_4\\SupplierPortalAPI\\SupplierPortalAPI\\DataAccess\\UploadedFiles\\P6-AmazingMartEU2Geo.xlsx";
                reportingPeriod.AddPeriodFacilityDocument(1, 1, "abc.xlsx", path, null, documentStatuses, documentType, facilityRequiredDocumentTypeVOs);
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
        /// AddUpdate PeriodFacilityDocument fail case2
        /// if documentRequiredStatus is NotAllowed than throw exception
        /// </summary>
        [Fact]
        public void AddUpdatePeriodFacilityDocumentFailCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.NonGHGRP);
            var facilityRequiredDocumentTypeVOs = GetFacilityRequiredDocumentTypeVOs();

            var path = "E:\\Bigscal\\ICT_4\\SupplierPortalAPI\\SupplierPortalAPI\\DataAccess\\UploadedFiles\\P6-AmazingMartEU2Geo.xlsx";
            try
            {
                reportingPeriod.AddPeriodFacilityDocument(1, 1, "abc.xlsx", path, null, documentStatuses, documentType, facilityRequiredDocumentTypeVOs);
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
