using BusinessLogic.ReferenceLookups;
using BusinessLogic.SupplierRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using DataAccess.Entities;
using System.Xml.Linq;
using Xunit;

namespace UnitTest.SupplierBusinessLogic
{
    public class SupplierUnitTesting : BasicTestClass
    {
        /// <summary>
        /// Update Supplier unit testing success
        /// </summary>
        [Fact]  //done
        public void UpdateSupplierSucceed()
        {
            var supplier = GetSupplierDomain();
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            try
            {
                supplier.UpdateSupplier("abc", "ac", "abc@gmail.com", "+397435675", true);
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
        /// Add supplier contact unit testing success
        /// </summary>
        [Fact]  //done
        public void AddSupplierContactSucceed()
        {
            int exceptionCounter = 0;
            var supplier = GetSupplierDomain();
            string? exceptionMessage = null;

            try
            {
                var result = supplier.AddSupplierContact(0, 1, "prarthna", "prarthna@gmail.com", "+4957398", true);
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
                exceptionCounter++;
            }

            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);
        }

        /// <summary>
        /// Add duplicate contacts unit testing 
        /// If user try to add duplicate contact using same email-id then it will throw exception message
        /// </summary>
        [Fact]  //done
        public void AddDuplicateContactFails()
        {
            int exceptionCounter = 0;
            var supplier = GetSupplierDomain();
            string? exceptionMessage = null;

            try
            {
                supplier.AddSupplierContact(0, 1, "prarthna", "prarthna@gmail.com", "+3457348957", true);
                supplier.AddSupplierContact(0, 2, "prarthna", "prarthna@gmail.com", "+374387356", true);
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
        /// Add facility unit testing success case1.
        /// Manage isPrimary status while adding new facility with same GHGRPFacilityId
        /// </summary>
        [Fact]  //done
        public void AddSupplierFacilitySucceedCase1()
        {
            int exceptionCounter = 0;
            var supplier = GetSupplierDomain();
            string? exceptionMessage = null;
            Facility facility = null;

            try
            {
                var reportingType = GenerateReportingType().Where(x => x.Id == 1).FirstOrDefault();
                var supplyChainStage = GenerateSupplyChainStage().Where(x => x.Id == 1).FirstOrDefault();
                facility = supplier.AddSupplierFacility(0, "TestFacility1", "TestFacilityDescription", true, "123", null, reportingType, supplyChainStage, true);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(facility);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);

        }

        /// <summary>
        /// Add facility unit testing success case2.
        /// 
        /// </summary>
        [Fact]  //done
        public void AddSupplierFacilitySucceedCase2()
        {
            int exceptionCounter = 0;
            var supplier = GetSupplierDomain();
            string? exceptionMessage = null;
            Facility facility = null;

            try
            {
                var reportingType = GenerateReportingType().Where(x => x.Id == 1).FirstOrDefault();
                var supplyChainStage = GenerateSupplyChainStage().Where(x => x.Id == 3).FirstOrDefault();
                var associatePipeline = GenerateAssociatePipelines().Where(x => x.Id == 1).FirstOrDefault();
                facility = supplier.AddSupplierFacility(0, "TestFacilityCase2", "TestFacilityDescription", true, "123", associatePipeline, reportingType, supplyChainStage, true);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(facility);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);

        }

        /// <summary>
        /// Add facility unit testing failure case1.
        /// If reportingType is NonGHGRP then GHGRPFacilityId is not allowed
        /// </summary>
        [Fact]  //done
        public void AddSupplierFacilityFailsCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingType = GenerateReportingType().Where(x => x.Id == 2).FirstOrDefault();
            var supplyChainStage = GenerateSupplyChainStage().Where(x => x.Id == 1).FirstOrDefault();

            var supplier = GetSupplierDomain();
            Facility facility = null;
            try
            {
                facility = supplier.AddSupplierFacility(0, "TestFailedFacility", "Test case failure", false, "123", null, reportingType, supplyChainStage, true);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                //GHGRPFacilityId not allowed in NONGHGRP
                exceptionMessage = ex.Message;
            }

            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }

        /// <summary>
        /// Add facility unit testing failure case2.
        /// If supplyChainStage is not transmissionCompression then AssociatePipeline is not allowed
        /// </summary>
        [Fact]  //done
        public void AddSupplierFacilityFailsCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingType = GenerateReportingType().Where(x => x.Id == 1).FirstOrDefault();
            var supplyChainStage = GenerateSupplyChainStage().Where(x => x.Id == 1).FirstOrDefault();
            var associatePipeline = GenerateAssociatePipelines().Where(x => x.Id == 1).FirstOrDefault();

            var supplier = GetSupplierDomain();
            Facility facility = null;

            try
            {
                facility = supplier.AddSupplierFacility(0, "TestFailFacility 2", "faciliy", true, "123", associatePipeline, reportingType, supplyChainStage, true);
            }
            catch (Exception ex)
            {
                //Associate pipeline is not allowed in SupplyChainStage "production"
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);

        }

        /// <summary>
        /// Add facility unit testing failure case3.
        /// If isActive status is false then facility cannot be added
        /// </summary>
        [Fact]  //done
        public void AddSupplierFacilityFailsCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var reportingType = GenerateReportingType().Where(x => x.Id == 1).FirstOrDefault();
            var supplyChainStage = GenerateSupplyChainStage().Where(x => x.Id == 1).FirstOrDefault();

            var supplier = GetSupplierDomain();
            Facility facility = null;

            try
            {
                facility = supplier.AddSupplierFacility(0, "TestFailFacility3", "facility3", false, "123", null, reportingType, supplyChainStage, false);
            }
            catch (Exception ex)
            {
                //InActive facility cannot be add
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }

        /// <summary>
        /// Update facility unit testing success case1.
        /// Managed isPrimary status via update facility
        /// </summary>
        [Fact]  //done
        public void UpdateSupplierFacilitySucceedCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;

            var supplier = GetSupplierDomain();
            var reportingType = GenerateReportingType().Where(x => x.Id == 1).FirstOrDefault();
            var supplyChainStage = GenerateSupplyChainStage().Where(x => x.Id == 3).FirstOrDefault();
            var associatePipeline = GenerateAssociatePipelines().Where(x => x.Id == 1).FirstOrDefault();
            Facility facility = null;

            try
            {
                //IsPrimary = false
                facility = supplier.UpdateSupplierFacility(1, "Update Facility 1", "Updated facility", false, "123", associatePipeline, reportingType, supplyChainStage, true);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(facility);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);

        }

        /// <summary>
        /// Update facility unit testing success case2.
        /// If isPrimary is false and isActive is false then facility can be changed to InActive
        /// </summary>
        [Fact]  //done
        public void UpdateSupplierFacilitySucceedCase2()
        {
            /*{
                Id = 2,
                Name = "Test facility 2",
                Description = "Testing Facility 2",
                IsPrimary = false,
                SupplierId = supplierId,
                GhgrpfacilityId = "123",
                ReportingTypeId = 1,
                AssociatePipelineId = null,
                SupplyChainStageId = 1,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
            }*/

            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var supplier = GetSupplierDomain();
            var reportingType = GenerateReportingType().Where(x => x.Id == 1).FirstOrDefault();
            var supplyChainStage = GenerateSupplyChainStage().Where(x => x.Id == 1).FirstOrDefault();

            Facility facility = null;

            try
            {
                //IsActive = false (Default IsPrimary = false)
                facility = supplier.UpdateSupplierFacility(2, "Update Facility case2", "Testing Facility 2", false, "123", null, reportingType, supplyChainStage, false);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotNull(facility);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);

        }

        /// <summary>
        /// Update facility unit testing failure case1.
        /// If isPrimary is true and isActive is false then facility cannot be changed to InActive
        /// </summary>
        [Fact]  //done
        public void UpdateSupplierFacilityFailsCase1()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var supplier = GetSupplierDomain();
            var reportingType = GenerateReportingType().Where(x => x.Id == 1).FirstOrDefault();
            var supplyChainStage = GenerateSupplyChainStage().Where(x => x.Id == 3).FirstOrDefault();
            var associatePipeline = GenerateAssociatePipelines().Where(x => x.Id == 1).FirstOrDefault();

            Facility facility = null;

            try
            {
                //IsActive = false (IsPrimary = true)
                facility = supplier.UpdateSupplierFacility(1, "Update test facility 1", "Test facility 1", true, "123", associatePipeline, reportingType, supplyChainStage, false);
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
        /// Update facility unit testing failure case2.
        /// If GHGRPFacilityId is changed then throw exception because it can't be changed once it is added.
        /// </summary>
        [Fact]  //done
        public void UpdateSupplierFacilityFailsCase2()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var supplier = GetSupplierDomain();
            var reportingType = GenerateReportingType().Where(x => x.Id == 1).FirstOrDefault();
            var supplyChainStage = GenerateSupplyChainStage().Where(x => x.Id == 3).FirstOrDefault();
            var associatePipeline = GenerateAssociatePipelines().Where(x => x.Id == 1).FirstOrDefault();

            Facility facility = null;

            try
            {
                //GHGRPFacility = 456   (Default = 123)
                facility = supplier.UpdateSupplierFacility(1, "Update test facility 1", "Test facility 1", true, "456", associatePipeline, reportingType, supplyChainStage, true);
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
        /// Update facility unit testing failure case3.
        /// If supplyChainStage is changed from other to TransmissionCompression then AssociatedPipeline should be null.
        /// </summary>
        [Fact]  //done
        public void UpdateSupplierFacilityFailsCase3()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var supplier = GetSupplierDomain();
            var reportingType = GenerateReportingType().Where(x => x.Id == 1).FirstOrDefault();
            var supplyChainStage = GenerateSupplyChainStage().Where(x => x.Id == 1).FirstOrDefault();
            var associatePipeline = GenerateAssociatePipelines().Where(x => x.Id == 1).FirstOrDefault();

            Facility facility = null;

            try
            {
                //SupplyChainStage = "production" (Default is "transmissionCompression")
                //AssociatePipeline is not null
                facility = supplier.UpdateSupplierFacility(1, "Update Facility 1", "Updated facility", false, "123", associatePipeline, reportingType, supplyChainStage, true);
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
        /// Update facility unit testing failure case4.
        /// If reportingType is changed from GHGRP to NONGHGRP and GHGRPFacilityId is 123 then it can't be null though reportingType is NONGHGRP
        /// </summary>
        [Fact]  //done
        public void UpdateSupplierFacilityFailsCase4()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var supplier = GetSupplierDomain();
            var reportingType = GenerateReportingType().Where(x => x.Id == 2).FirstOrDefault();
            var supplyChainStage = GenerateSupplyChainStage().Where(x => x.Id == 1).FirstOrDefault();

            Facility facility = null;

            try
            {
                //reportingType = 2 (Default is 1)
                //GHGRPFacilityId = null (Default is 123)
                facility = supplier.UpdateSupplierFacility(2, "Update Facility case2", "Testing Facility 2", false, null, null, reportingType, supplyChainStage, true);
            }
            catch (Exception ex)
            {
                exceptionCounter++;
                exceptionMessage = ex.Message;
            }

            Assert.NotEqual(0, exceptionCounter);
            Assert.NotNull(exceptionMessage);
        }
    }
}
