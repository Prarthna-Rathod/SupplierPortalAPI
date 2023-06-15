using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.Interfaces;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace UnitTest.ReportingPeriodBusinessLogic
{
    public class ReportingPeriodUnitTesting : BasicTestClass
    {
        #region AddUpdate ReportingPeriod

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
                foreach (var updatedStatus in periodSuppliers)
                {
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
                reportingperiod.AddRemoveUpdatePeriodFacility(createFacilityEntities(), fercRegion, facilityReportingPeriodDataStatus, periodSupplier.FirstOrDefault());
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
                reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1, CreateReportingPeriodFacilityElecticityGridMixEntity(), fercRegion.FirstOrDefault(X => X.Id == 12));


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
            reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1, CreateReportingPeriodFacilityElecticityGridMixEntity(), fercRegion);
            var percents = GetElectricityGridMixComponents();

            try
            {
                list = reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1, CreateReportingPeriodFacilityElecticityGridMixEntity(), fercRegion);
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
                list = reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1, CreateReportingPeriodFacilityElecticityGridMixEntity(), fercRegion);
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
                list = reportingPeriod.AddPeriodFacilityElectricityGridMix(1, 1, CreateReportingPeriodFacilityElecticityGridMixEntity(), fercRegion);
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

            var gasSupplyBreakdownVos = GetPeriodFacilityGasSupplyBreakdowns();
            IEnumerable<PeriodFacilityGasSupplyBreakDown>? list = null;

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
        /// replace periodSupplier gasSupplyBreakdown if exist..
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

            var gasSupplyBreakdownVos = GetPeriodFacilityGasSupplyBreakdowns();
            IEnumerable<PeriodFacilityGasSupplyBreakDown>? list = null;
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
        /// Add PeriodFacilityGasSupplyBreakdown Failure case1
        /// If SupplierReportingPeriodStatus is Locked than throw exception
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownFailCase1()
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
            var gasSupplyBreakdownVos = GetPeriodFacilityGasSupplyBreakdowns();
            IEnumerable<PeriodFacilityGasSupplyBreakDown>? list = null;

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
        /// If SupplyChainStage is not Production than throw exception
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownFailCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //Add GasSupplyBreakdownVo
            var gasSupplyBreakdownVos = GetPeriodFacilityGasSupplyBreakdowns();
            IEnumerable<PeriodFacilityGasSupplyBreakDown>? list = null;

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
        /// </summary>
        [Fact]
        public void AddPeriodFacilityGasSupplyBreakdownFailCase4()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            //Add GasSupplyBreakdownVo
            var gasSupplyBreakdownVos = GetPeriodFacilityGasSupplyBreakdowns();
            IEnumerable<PeriodFacilityGasSupplyBreakDown>? list = null;

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
        /// AddPeriodFacilityDocument SuccessCase1
        /// </summary>
        [Fact]
        public void AddPeriodFacilityDocumentSuccessCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.Subpart_C);
            var facilityRequiredDocumentType = GetFacilityRequiredDocumentTypes();

            var displayName = "ABC.xlsx";
            var path = "E:\\SupplierPortalAPI Repo\\DataAccess\\UploadedFiles";

            PeriodFacilityDocument? periodFacilityDocument = null;

            try
            {
                periodFacilityDocument = reportingPeriod.AddDataSubmissionDocumentForReportingPeriod(1,0, displayName, path, null, documentStatuses, documentType, facilityRequiredDocumentType);
            }
            catch (Exception ex) { exceptionCounter++; exceptionMessage = ex.Message; }
            Assert.Equal(0, periodFacilityDocument.ReportingPeriodFacilityId);
            Assert.Equal(displayName, periodFacilityDocument.DisplayName);
            Assert.Equal(null, periodFacilityDocument.Path);
            Assert.Equal(null, periodFacilityDocument.ValidationError);
            Assert.Equal(documentStatuses.First(x => x.Name == DocumentStatusValues.Processing), periodFacilityDocument.DocumentStatus);
            Assert.Equal(documentType, periodFacilityDocument.DocumentType);
            Assert.Null(exceptionMessage);
        }

        /// <summary>
        /// Add PeriodFacilityDocument SucessCase2
        /// existingDocument is already exists than update the facilityDocument
        /// </summary>
        [Fact]
        public void AddPeriodFacilityDocumentSuccessCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();
            var documentStatuses = GetDocumentStatuses();
            var documentTypes = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.Subpart_C);
            var facilityRequiredDocumentType = GetFacilityRequiredDocumentTypes();

            var displayName = "ABC.xlsx";

            reportingPeriod.AddDataSubmissionDocumentForReportingPeriod(1,0, displayName, null, null, documentStatuses, documentTypes, facilityRequiredDocumentType);
            var path = "E:\\SupplierPortalAPI Repo\\DataAccess\\UploadedFiles";

            PeriodFacilityDocument? periodFacilityDocument = null;

            try
            {
                periodFacilityDocument = reportingPeriod.AddDataSubmissionDocumentForReportingPeriod(1,0, displayName, path, null, documentStatuses, documentTypes, facilityRequiredDocumentType);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }
            Assert.Equal(0, periodFacilityDocument.ReportingPeriodFacilityId);
            Assert.Equal(displayName, periodFacilityDocument.DisplayName);
            Assert.Equal(path, periodFacilityDocument.Path);
            Assert.Equal(null, periodFacilityDocument.ValidationError);
            Assert.Equal(documentStatuses.First(x => x.Name == DocumentStatusValues.Validated), periodFacilityDocument.DocumentStatus);
            Assert.Equal(documentTypes, periodFacilityDocument.DocumentType);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }
        /// <summary>
        /// AddPeriodFacilityDocument SuccessCase 3
        /// if facilityDataStatus is Submitted than update existing Document
        /// </summary>
        [Fact]
        public void AddPeriodFacilityDocumentSuccessCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();
            var documentStatuses = GetDocumentStatuses();
            var documentTypes = GetDocumentTypes().First(x=>x.Name==DocumentTypeValues.Subpart_C);
            var facilityReqiredDocumentType = GetFacilityRequiredDocumentTypes();

            var displayName = "ABC.xlsx";

            reportingPeriod.AddDataSubmissionDocumentForReportingPeriod(1,0,displayName,null,null,documentStatuses,documentTypes,facilityReqiredDocumentType);

            var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == 1);
            var periodFacility = periodSupplier.PeriodFacilities.FirstOrDefault(x => x.Id == 1);
            var facilityDataStatus = GetFacilityReportingPeriodDataStatus().First(x=>x.Name == FacilityReportingPeriodDataStatusValues.Submitted);
            periodFacility.FacilityReportingPeriodDataStatus.Id=facilityDataStatus.Id;
            periodFacility.FacilityReportingPeriodDataStatus.Name =facilityDataStatus.Name;

            var path = "E:\\SupplierPortalAPI Repo\\DataAccess\\UploadedFiles";

            PeriodFacilityDocument? periodFacilityDocument= null;

            try
            {
                periodFacilityDocument = reportingPeriod.AddDataSubmissionDocumentForReportingPeriod(1,0,displayName,path,null,documentStatuses,documentTypes,facilityReqiredDocumentType);
            }
            catch(Exception ex)
            {
                exceptionCounter++;
                exceptionMessage= ex.Message;
            }
            Assert.Equal(1, periodFacilityDocument.ReportingPeriodFacilityId);
            Assert.Equal(displayName, periodFacilityDocument.DisplayName);
            Assert.Equal(null, periodFacilityDocument.Path);
            Assert.Equal(null, periodFacilityDocument.ValidationError);
            Assert.Equal(documentStatuses.First(x => x.Name == DocumentStatusValues.Processing), periodFacilityDocument.DocumentStatus);
            Assert.Equal(documentTypes, periodFacilityDocument.DocumentType);
            Assert.Equal(0,exceptionCounter);
            Assert.Null(exceptionMessage);

        }

        /// <summary>
        /// AddUpdate PeriodFacilityDocument success case4
        /// existingDocument is already exists than update the facilityDocument 
        /// path is null and validationError is null than update the documentStatus is NotValidated
        /// </summary>
        [Fact]
        public void AddPeiodFacilityDocumentSuccessCase4()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();
            var documentStatuses = GetDocumentStatuses();
            var documentTypes = GetDocumentTypes().First(x=>x.Name ==DocumentTypeValues.Subpart_C);
            var facilityRequiredDocumentTypes = GetFacilityRequiredDocumentTypes();

            var displayName = "abc.xlsx";
            reportingPeriod.AddDataSubmissionDocumentForReportingPeriod(1,0,displayName,null,null,documentStatuses,documentTypes,facilityRequiredDocumentTypes);

            PeriodFacilityDocument?periodFacilityDocument= null;

            try
            {
                periodFacilityDocument = reportingPeriod.AddDataSubmissionDocumentForReportingPeriod(1,0, displayName, null, null, documentStatuses, documentTypes, facilityRequiredDocumentTypes);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(0, periodFacilityDocument.ReportingPeriodFacilityId);
            Assert.Equal(displayName, periodFacilityDocument.DisplayName);
            Assert.Equal(null, periodFacilityDocument.Path);
            Assert.Equal(null, periodFacilityDocument.ValidationError);
            Assert.Equal(documentStatuses.First(x => x.Name == DocumentStatusValues.NotValidated), periodFacilityDocument.DocumentStatus);
            Assert.Equal(documentTypes, periodFacilityDocument.DocumentType);
            Assert.Equal(exceptionCounter, 0);
            Assert.Null(exceptionMessage);


        }

        /// <summary>
        /// AddPeriodFacilityDocument SuccessCase5 
        /// validation error & Has Error
        /// </summary>
        [Fact]
        public void AddPeriodFacilityDocumentSuccessCase5()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();
            var documentStatuses = GetDocumentStatuses();
            var documentTypes = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.Subpart_C);
            var facilityRequiredDocumentType = GetFacilityRequiredDocumentTypes();

            var displayName = "ABC.xlsx";
            reportingPeriod.AddDataSubmissionDocumentForReportingPeriod(1,0, displayName, null, null,documentStatuses,documentTypes,facilityRequiredDocumentType);
            var path = "E:\\SupplierPortalAPI Repo\\DataAccess\\UploadedFiles";
            var validationError = "Unable to save the uploaded Document..!";
            PeriodFacilityDocument? periodFacilityDocument = null;

            try
            {
                periodFacilityDocument=reportingPeriod.AddDataSubmissionDocumentForReportingPeriod(1,0,displayName,path,validationError,documentStatuses,documentTypes,facilityRequiredDocumentType);
            }
            catch (Exception ex) { 
                exceptionCounter++;
                exceptionMessage= ex.Message;
            }
            Assert.Equal(0, periodFacilityDocument.ReportingPeriodFacilityId);
            Assert.Equal(displayName, periodFacilityDocument.DisplayName);
            Assert.Equal(null, periodFacilityDocument.Path);
            Assert.Equal(validationError, periodFacilityDocument.ValidationError);
            Assert.Equal(documentStatuses.First(x => x.Name == DocumentStatusValues.HasErrors), periodFacilityDocument.DocumentStatus);
            Assert.Equal(documentTypes, periodFacilityDocument.DocumentType);
            Assert.Equal(exceptionCounter, 0);
            Assert.Null(exceptionMessage);
       


        }
        /// <summary>
        /// AddPeriodFacilityDcoument FailureCase1
        /// if File path is already existed in system then throw error
        /// </summary>
        [Fact]
        public void AddPeriodFacilityDocumentFailureCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();
            var documentStatuses = GetDocumentStatuses();
            var documentTypes = GetDocumentTypes().First(x=>x.Name == DocumentTypeValues.Subpart_C);
            var facilityRequiredDocumentTypes = GetFacilityRequiredDocumentTypes();

            var displayName = "ABC.xlsx";
            reportingPeriod.AddDataSubmissionDocumentForReportingPeriod(1,0, displayName, null, null, documentStatuses, documentTypes, facilityRequiredDocumentTypes);
            var path = "E:\\SupplierPortalAPI Repo\\DataAccess\\UploadedFiles";

            try
            {
                reportingPeriod.AddDataSubmissionDocumentForReportingPeriod(1,0,displayName,path,null,documentStatuses, documentTypes, facilityRequiredDocumentTypes);
                path = "E:\\SupplierPortalAPI Repo\\DataAccess\\UploadedFiles";
                reportingPeriod.AddDataSubmissionDocumentForReportingPeriod(1,0,displayName,path,null,documentStatuses,documentTypes,facilityRequiredDocumentTypes);
            }
            catch (Exception ex){
                exceptionCounter++;
                exceptionMessage= ex.Message;
            }
            Assert.Equal(exceptionCounter, 0);
            Assert.Null(exceptionMessage);  

        }
        #endregion

        #region PeriodSupplierDocument
        /// <summary>
        /// Add PeriodSupplierDocument Success case1
        /// </summary>
        [Fact]
        public void AddUpdatePeriodSupplierDocumentSuccessCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.Supplemental);

            var displayName = "ABC.xlsx";
            var path = "E:\\SupplierPortalAPI Repo\\DataAccess\\UploadedFiles\\Ambani-2023-Supplemental-1.xlsx";

            PeriodSupplierDocument? periodSupplierDocument = null;

            try
            {
                periodSupplierDocument = reportingPeriod.AddSupplementalDataDocumentToReportingPeriodSupplier(1, path,null,displayName, documentType, documentStatuses);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(1, periodSupplierDocument.ReportingPeriodSupplierId);
            Assert.Equal(displayName, periodSupplierDocument.DisplayName);
            Assert.Equal(null, periodSupplierDocument.Path);
            Assert.Equal(null, periodSupplierDocument.ValidationError);
            Assert.Equal(documentType, periodSupplierDocument.DocumentType);
            Assert.Equal(documentStatuses.First(x => x.Name == DocumentStatusValues.Processing), periodSupplierDocument.DocumentStatus);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }

        /// <summary>
        /// AddUpdate PeriodSupplierDocument success case2
        /// existingDocument is already exists than update the supplierDocument 
        /// path is not null and validationError is null than update the documentStatus is Validated
        /// </summary>
        [Fact]
        public void AddUpdatePeriodSupplierDocumentSuccessCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.Supplemental);

            var displayName = "abc.xlsx";

            reportingPeriod.AddSupplementalDataDocumentToReportingPeriodSupplier(1, null, null,displayName , documentType,documentStatuses);

            var path = "E:\\SupplierPortalAPI Repo\\DataAccess\\UploadedFiles\\Ambani-2023-Supplemental-1.xlsx";

            PeriodSupplierDocument? periodSupplierDocument = null;

            try
            {
                periodSupplierDocument = reportingPeriod.AddSupplementalDataDocumentToReportingPeriodSupplier(1,  path, null, displayName, documentType, documentStatuses);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(1, periodSupplierDocument.ReportingPeriodSupplierId);
            Assert.Equal(displayName, periodSupplierDocument.DisplayName);
            Assert.Equal(path, periodSupplierDocument.Path);
            Assert.Equal(null, periodSupplierDocument.ValidationError);
            Assert.Equal(documentStatuses.First(x => x.Name == DocumentStatusValues.Validated), periodSupplierDocument.DocumentStatus);
            Assert.Equal(documentType, periodSupplierDocument.DocumentType);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }

        /// <summary>
        /// AddUpdate PeriodSupplierDocument success case3
        /// if SupplierReportingPeriodStatus is locked than update the document displayName
        /// </summary>
        [Fact]
        public void AddUpdatePeriodSupplierDocumentSuccessCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.Supplemental);

            var displayName = "ABC.xlsx";

            reportingPeriod.AddSupplementalDataDocumentToReportingPeriodSupplier(1, null, null, displayName, documentType, documentStatuses);

            var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == 1);
            var supplierReportingPeriodStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Locked);
            periodSupplier.SupplierReportingPeriodStatus.Id = supplierReportingPeriodStatus.Id;
            periodSupplier.SupplierReportingPeriodStatus.Name = supplierReportingPeriodStatus.Name;

            var path = "E:\\SupplierPortalAPI Repo\\DataAccess\\UploadedFiles\\Ambani-2023-Supplemental-1.xlsx";

            PeriodSupplierDocument? periodSupplierDocument = null;

            try
            {
                periodSupplierDocument = reportingPeriod.AddSupplementalDataDocumentToReportingPeriodSupplier(1, path,null,displayName,documentType,documentStatuses);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(1, periodSupplierDocument.ReportingPeriodSupplierId);
            Assert.Equal(displayName, periodSupplierDocument.DisplayName);
            Assert.Equal(null, periodSupplierDocument.Path);
            Assert.Equal(null, periodSupplierDocument.ValidationError);
            Assert.Equal(documentStatuses.First(x => x.Name == DocumentStatusValues.Processing), periodSupplierDocument.DocumentStatus);
            Assert.Equal(documentType, periodSupplierDocument.DocumentType);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }

        
        /// <summary>
        /// AddUpdate PeriodSupplierDocument success case4
        /// existingDocument is already exists than update the supplierDocument 
        /// path is null and validationError is null than update the documentStatus is NotValidated
        /// </summary>
        [Fact]
        public void AddUpdatePeriodSupplierDocumentSuccessCase4()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.Supplemental);

            var displayName = "ABC.xlsx";

            reportingPeriod.AddSupplementalDataDocumentToReportingPeriodSupplier(1,  null, null, displayName, documentType ,documentStatuses);

            PeriodSupplierDocument? periodSupplierDocument = null;

            try
            {
                periodSupplierDocument = reportingPeriod.AddSupplementalDataDocumentToReportingPeriodSupplier(1,  null, null, displayName, documentType, documentStatuses);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(1, periodSupplierDocument.ReportingPeriodSupplierId);
            Assert.Equal(displayName, periodSupplierDocument.DisplayName);
            Assert.Equal(null, periodSupplierDocument.Path);
            Assert.Equal(null, periodSupplierDocument.ValidationError);
            Assert.Equal(documentStatuses.First(x => x.Name == DocumentStatusValues.NotValidated), periodSupplierDocument.DocumentStatus);
            Assert.Equal(documentType, periodSupplierDocument.DocumentType);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }

        /// <summary>
        /// AddUpdate PeriodSupplierDocument fail case1
        /// if file path is already exists in the system and supplierReportingPeriodStatus is locked than throw exception
        /// </summary>
        [Fact]
        public void AddUpdatePeriodSupplierDocumentFailCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.Supplemental);

            reportingPeriod.AddSupplementalDataDocumentToReportingPeriodSupplier(1, null, null, "ABC.xlsx", documentType, documentStatuses);

            var path = "E:\\SupplierPortalAPI Repo\\DataAccess\\UploadedFiles\\Ambani-2023-Supplemental-1.xlsx";

            try
            {
                reportingPeriod.AddSupplementalDataDocumentToReportingPeriodSupplier(1, path, null, "ABC.xlsx", documentType, documentStatuses);

                var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == 1);
                var supplierReportingPeriodStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Locked);
                periodSupplier.SupplierReportingPeriodStatus.Id = supplierReportingPeriodStatus.Id;
                periodSupplier.SupplierReportingPeriodStatus.Name = supplierReportingPeriodStatus.Name;

                path = "E:\\SupplierPortalAPI Repo\\DataAccess\\UploadedFiles\\Ambani-2023-Supplemental-1.xlsx";


                reportingPeriod.AddSupplementalDataDocumentToReportingPeriodSupplier(1, path, null,"ABC.xlsx", documentType, documentStatuses);
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
        /// AddUpdate PeriodSupplierDocument fail case2
        /// if documentType is not Supplemental than throw exception
        /// </summary>
        [Fact]
        public void AddUpdatePeriodSupplierDocumentFailCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.Subpart_C);

            try
            {
                reportingPeriod.AddSupplementalDataDocumentToReportingPeriodSupplier(1,null, null, "ABC.xlsx", documentType, documentStatuses);
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

        #region SendMail

        /// <summary>
        /// SendInitialDataRequestAndResendDataRequestEmail Success Case1
        /// If InitialDataRequest is null than return emails
        /// </summary>
        [Fact]
        public void SendInitialDataRequestAndResendDataRequestEmailSuccessCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();
            var list = new List<string>();

            try
            {
                list = reportingPeriod.CheckInitialAndResendDataRequest(1);
            }

            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
            Assert.NotEqual(0, list.Count());
        }

        /// <summary>
        /// SendEmailInitialAndResendDataRequest success case2
        /// Send Reminder Mail after duedate
        /// </summary>
        [Fact]
        public void SendEmailInitialAndResendDataRequestSuccessCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();
            var list = new List<string>();

            try
            {
                list = reportingPeriod.CheckInitialAndResendDataRequest(1);
            }

            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
            Assert.NotEqual(0, list.Count());
        }

        /// <summary>
        /// SendInitialDataRequestAndResendDataRequest failure case1
        /// reminder send mail after deadline {timelimit}
        /// if endDate is null and dueDate is greaterthan currentDate than throw exception
        /// Set null endDate in CreateReportingPeriodEntity() method
        /// Set null ResendDataRequestDate in createPeriodSupplierEntities() method
        /// Set InitialDataRequestDate is currentDate
        /// </summary>
        [Fact]
        public void SendInitialDataRequestAndResendDataRequestFailureCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();
            var list = new List<string>();

            try
            {
                list = reportingPeriod.CheckInitialAndResendDataRequest(1);
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
        /// SendInitialDataRequestAndResendDataRequest failure case2
        /// Reminder Mail Due date : {dueDate}.
        /// if endDate is not null and endDate>currentDate than throw exception
        /// set endDate - CreateReportingPeriodEntity() method
        /// </summary>
        [Fact]
        public void SendInitialDataRequestAndResendDataRequestEmailFailureCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            try
            {
                reportingPeriod.CheckInitialAndResendDataRequest(1);
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
        /// SendInitialDataRequestAndResendDataRequestEnail fail case3
        /// InitialDataRequest and ResendDataRequest mail is already send
        /// if InitialDateRequest and ResendDataRequest date is not null than throw exception
        /// set InitialDateRequestDate and ResendDataRequestDate - createPeriodSupplierEntities() method
        /// </summary>
        [Fact]
        public void SendInitialDataRequestAndResendDataRequestEmailFailure()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            try
            {
                reportingPeriod.CheckInitialAndResendDataRequest(1);
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


