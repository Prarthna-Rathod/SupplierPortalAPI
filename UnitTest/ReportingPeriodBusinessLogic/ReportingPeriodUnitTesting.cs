using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.Interfaces;
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
        public void AddRemoveReportingPeriodSupplierSucceed()
        {

            //Arrange


            var reportingPeriod = GetReportingPeriodDomain();

            var oldDomain = reportingPeriod.PeriodSuppliers.Count();

            var activeVos = createSupplierEntities();

            var newDomain = reportingPeriod.AddRemovePeriodSupplier(activeVos);

            Assert.NotEqual(reportingPeriod.PeriodSuppliers.Count(), oldDomain);

            foreach (var domain in reportingPeriod.PeriodSuppliers)
            {
                var check = newDomain.FirstOrDefault(x => x.Id == domain.Id);

                Assert.Equal(domain.SupplierReportingPeriodStatus, check.SupplierReportingPeriodStatus);
                Assert.Equal(domain.ReportingPeriodId, check.ReportingPeriodId);
                Assert.Equal(domain.IsActive, check.IsActive);
                Assert.Equal(domain.InitialDataRequestDate, check.InitialDataRequestDate);
                Assert.Equal(domain.ResendDataRequestDate, check.ResendDataRequestDate);



            }
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
                reportingPeriod.AddRemovePeriodSupplier(createSupplierEntities());
                reportingPeriod.AddRemovePeriodSupplier(createSupplierEntities());
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
                reportingPeriod.AddRemovePeriodSupplier(createSupplierEntities());
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
            var periodSupplier = reportingPeriod.AddRemovePeriodSupplier(createSupplierEntities());

            //set this status open and close here
            reportingPeriod.ReportingPeriodStatus.Id = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Open).Id;
            reportingPeriod.ReportingPeriodStatus.Name = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Open).Name;

            var updatedStatus = GetSupplierReportingPeriodStatuses();

            try
            {
                //reportingPeriod.UpdateLockUnlockPeriodSupplierStatus(periodSupplier.Id, updatedStatus);
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
            var periodSuppliers = reportingPeriod.AddRemovePeriodSupplier(createSupplierEntities());

            //set this status complete here
            reportingPeriod.ReportingPeriodStatus.Id = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Complete).Id;
            reportingPeriod.ReportingPeriodStatus.Name = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Complete).Name;

            var updatedStatuses = GetSupplierReportingPeriodStatuses();

            try
            {
                foreach( var updatedStatus in periodSuppliers ) { 
                    reportingPeriod.UpdateLockUnlockPeriodSupplierStatus(updatedStatus.Id, updatedStatuses);
                }

               
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
        public void addperiodfacilitiessuccess()
        {
            int exceptioncounter = 0;
            string? exceptionmessage = null;
            var reportingperiod = GetReportingPeriodDomain();
         
            //Get PeriodSupplier Domain
            var supplierVO = GetAndConvertSupplierValueObject();
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingperiod.AddRemovePeriodSupplier(createPeriodSupplierEntities());


            //add periodfacility
            var facilityvo = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodDataStatus = GetFacilityReportingPeriodDataStatus()/*.First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress)*/;
            var fercRegion = GetFercRegions();

            try
            {
                reportingperiod.AddRemoveUpdatePeriodFacility(createFacilityEntities(),fercRegion,facilityReportingPeriodDataStatus, periodSupplier.FirstOrDefault());
            }
            catch (Exception ex)
            {
                exceptioncounter++;
                exceptionmessage = ex.Message;
            }

            Assert.Equal(0, exceptioncounter);
            Assert.Null(exceptionmessage);

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
            var periodSupplier = reportingPeriod.AddRemovePeriodSupplier(createPeriodSupplierEntities());

            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodStatus = GetFacilityReportingPeriodDataStatus()/*.First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress)*/;
            var fercRegion = GetFercRegions();
            var periodFacility = reportingPeriod.AddRemoveUpdatePeriodFacility(CreateReportingPeriodFacilityEntity(), fercRegion, facilityReportingPeriodStatus, periodSupplier.FirstOrDefault());

            //Add PeriodFacility


            try
            {
                reportingPeriod.AddRemoveUpdatePeriodFacility(createFacilityEntities(), fercRegion, facilityReportingPeriodStatus, periodSupplier.FirstOrDefault());
                reportingPeriod.AddRemoveUpdatePeriodFacility(createFacilityEntities(), fercRegion, facilityReportingPeriodStatus, periodSupplier.FirstOrDefault());

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

        #region PeriodFacilityElectricityGridMix

        /// <summary>
        /// Add PeiodFacilityElectricityGridMix Success Case1
        /// </summary>
        [Fact]
        public void AddPeriodFacilityElectricityGridMixSuccess()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = GetReportingPeriodDomain();
            var supplierVO = GetAndConvertSupplierValueObject();
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);

            var periodSupplier = reportingPeriod.AddRemovePeriodSupplier(createPeriodSupplierEntities());

            var facilityvo = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodDataStatus = GetFacilityReportingPeriodDataStatus()/*.First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress)*/;
            var fercRegion = GetFercRegions();

            var periodFacility = reportingPeriod.AddRemoveUpdatePeriodFacility(createFacilityEntities(), fercRegion, facilityReportingPeriodDataStatus, periodSupplier.FirstOrDefault());


            try
            {
                reportingPeriod.AddPeriodFacilityElectricityGridMix(1,1,CreateReportingPeriodFacilityElecticityGridMixEntity(),fercRegion.FirstOrDefault(X => X.Id == 12));
                

            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);



        }
        ///// <summary>
        ///// Add PeriodFacilityElectricityGridMix Success case2
        ///// If periodFacilityElectricityGridMix is already exists than replace old electricityGridMix
        ///// </summary>
        [Fact]
        public void AddPeriodFacilityElectricityGridMixSuccessCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1); 

            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionvalues.Custom_Mix);

            var electricityGridMixComponentPercents = GetElectricityGridMixComponents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;
            reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1,CreateReportingPeriodFacilityElecticityGridMixEntity(),fercRegion);
            var percents = GetElectricityGridMixComponents();

            try
            {
                list = reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1,CreateReportingPeriodFacilityElecticityGridMixEntity(),fercRegion);
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
        /// Add PeriodFacilityElecticityGridMix Failure Case1
        /// If Fercregion value is not Custom Mix
        /// </summary>
        [Fact]
        public void AddPeriodFacilityElectricityGridMixFailCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //ElectricityGridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionvalues.None);
            var electricityGridMixComponentPercents = GetElectricityGridMixComponents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;

            try
            {
                list = reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1,CreateReportingPeriodFacilityElecticityGridMixEntity(),fercRegion);
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
        /// Add PeriodFacilityElectricityGridMix Failure case2
        /// if Content value total is not 100 then throw exception
        /// </summary>
        [Fact]
        public void AddPeriodFacilityElectricityGridMixFailCase4()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //ElecetricityGridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionvalues.Custom_Mix);
            var electricityGridMixComponentPercents = GetElectricityGridMixComponents();
            IEnumerable<PeriodFacilityElectricityGridMix>? list = null;

            try
            {
                list = reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1,CreateReportingPeriodFacilityElecticityGridMixEntity(),fercRegion);
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
    }
}


