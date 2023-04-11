using BusinessLogic.ReferenceLookups;
using BusinessLogic.SupplierRoot.DomainModels;
using DataAccess.Entities;
using Services.Mappers.SupplierMappers;

namespace UnitTest
{
    public class BasicTestClass
    {
        private int supplierId = 1;
        private string supplierName = "Reliance";
        private string supplierAlias = "rel";
        private string supplierEmail = "reliance@gmail.com";
        private string supplierContactNo = "+0934734353";

        protected IEnumerable<ReportingType> GenerateReportingType()
        {
            var reportingTypes = new List<ReportingType>();
            reportingTypes.Add(new ReportingType(1, "GHGRP"));
            reportingTypes.Add(new ReportingType(2, "NonGHGRP"));
            return reportingTypes;
        }

        protected IEnumerable<SupplyChainStage> GenerateSupplyChainStage()
        {
            var supplyChainStgaes = new List<SupplyChainStage>();
            supplyChainStgaes.Add(new SupplyChainStage(1, "Production"));
            supplyChainStgaes.Add(new SupplyChainStage(2, "Processing"));
            supplyChainStgaes.Add(new SupplyChainStage(3, "Transmission Compression"));
            supplyChainStgaes.Add(new SupplyChainStage(4, "Transmission Pipeline"));
            return supplyChainStgaes;
        }

        protected IEnumerable<AssociatePipeline> GenerateAssociatePipelines()
        {
            var associatePipelines = new List<AssociatePipeline>();
            associatePipelines.Add(new AssociatePipeline(1, "Pipeline 1"));
            associatePipelines.Add(new AssociatePipeline(2, "Pipeline 2"));
            return associatePipelines;
        }

        protected Supplier GetSupplierDomain()
        {
            var supplierEntity = CreateSupplierEntity();
            var reportingType = GenerateReportingType();
            var associatePipeline = GenerateAssociatePipelines();
            var supplyChainStage = GenerateSupplyChainStage();
            var mapper = CreateInstanceOfSupplierEntityToDomain();
            return mapper.ConvertSupplierEntityToDomain(supplierEntity, reportingType, supplyChainStage, associatePipeline);
        }

        protected SupplierEntity CreateSupplierEntity()
        {
            var supplier = new SupplierEntity();
            supplier.Id = supplierId;
            supplier.Name = supplierName;
            supplier.Alias = supplierAlias;
            supplier.Email = supplierEmail;
            supplier.IsActive = true;
            supplier.ContactNo = supplierContactNo;
            supplier.CreatedOn = DateTime.UtcNow;
            supplier.CreatedBy = "System";

            var contacts = GenerateContactEntitiesForSupplier(supplier.Id);
            var facilities = GenerateFacilityEntitiesForSupplier(supplier.Id);

            supplier.ContactEntities = contacts;
            supplier.FacilityEntities = facilities;

            return supplier;
        }

        protected List<ContactEntity> GenerateContactEntitiesForSupplier(int supplierId)
        {
            var contactEntities = new List<ContactEntity>();
            contactEntities.Add(new ContactEntity()
            {
                Id = 1,
                SupplierId = supplierId,
                UserId = 1,
                User = new UserEntity()
                {
                    Id = 1,
                    Name = "abc",
                    Email = "abc@gmail.com",
                    ContactNo = "+85940494",
                    RoleId = 2,
                    IsActive= true,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "System"
                }
            });
            contactEntities.Add(new ContactEntity()
            {
                Id = 2,
                SupplierId = supplierId,
                UserId = 2,
                User = new UserEntity()
                {
                    Id = 2,
                    Name = "pqr",
                    Email = "pqr@gmail.com",
                    ContactNo = "+859404949",
                    RoleId = 2,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "System"
                }
            });

            return contactEntities;
        }

        protected List<FacilityEntity> GenerateFacilityEntitiesForSupplier(int supplierId)
        {
            var facilityEntities = new List<FacilityEntity>();
            facilityEntities.Add(new FacilityEntity()
            {
                Id = 1,
                Name = "Test facility 1",
                Description = "Testing Facility 1",
                IsPrimary = true,
                SupplierId = supplierId,
                GhgrpfacilityId = "123",
                ReportingTypeId = 1,
                AssociatePipelineId = 1,
                SupplyChainStageId = 3,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy= "System",
            });
            facilityEntities.Add(new FacilityEntity()
            {
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
            });

            return facilityEntities;
        }

        /*protected IEnumerable<SupplierEntity> CreateSupplierEntities()
        {
            List<SupplierEntity> supplierList = new List<SupplierEntity>
            {
                new SupplierEntity()
                {
                    Id = 1,
                    Name = "Supplier1",
                    Alias = "supplier1",
                    Email = "supplier1@gmail.com",
                    ContactNo = "+34738473",
                    IsActive = true
                },

                new SupplierEntity()
                {
                    Id = 2,
                    Name = "Supplier2",
                    Alias = "supplier2",
                    Email = "supplier2@gmaiil.com",
                    ContactNo = "+78657676",
                    IsActive = true
                }
            };

            return supplierList;
        }*/

        protected SupplierEntityDomainMapper CreateInstanceOfSupplierEntityToDomain()
        {
            return new SupplierEntityDomainMapper();
        }

        protected SupplierDomainDtoMapper CreateInstanceOfSupplierDomainToDto()
        {
            return new SupplierDomainDtoMapper();
        }

    }
}