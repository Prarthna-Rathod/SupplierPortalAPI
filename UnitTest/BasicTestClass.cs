using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.SupplierRoot.DomainModels;
using BusinessLogic.ValueConstants;
using BusinessLogic.SupplierRoot.ValueObjects;
using DataAccess.DataActions.Interfaces;
using DataAccess.Entities;
using Services.Mappers.Interfaces;
using Services.Mappers.ReportingPeriodMappers;
using Services.Mappers.SupplierMappers;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace UnitTest
{
    public class BasicTestClass
    {
        //Supplier
        private int supplierId = 1;
        private string supplierName = "Reliance";
        private string supplierAlias = "rel";
        private string supplierEmail = "reliance@gmail.com";
        private string supplierContactNo = "+0934734353";

        //ReportingPeriod
        private int reportingPeriodId = 1;
        private string displayName = "Reporting Period Data Year 2022";
        private string collectionTimePeriod = "2022";
        private DateTime startDate = new DateTime(2023,04,12);
        private DateTime? endDate = null;
        private bool isActive = true;

        #region Supplier
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

       
        protected SupplierEntityDomainMapper CreateInstanceOfSupplierEntityToDomain()
        {
            return new SupplierEntityDomainMapper();
        }

        protected SupplierDomainDtoMapper CreateInstanceOfSupplierDomainToDto()
        {
            return new SupplierDomainDtoMapper();
        }

        #endregion

        #region ReportingPeriod

        protected IEnumerable<ReportingPeriodType> GetAndConvertReportingPeriodTypes()
        {
            var reportingPeriodTypes = new List<ReportingPeriodType>();
            reportingPeriodTypes.Add(new ReportingPeriodType(1, "Annual"));
            reportingPeriodTypes.Add(new ReportingPeriodType(2, "Quartly"));
            reportingPeriodTypes.Add(new ReportingPeriodType(3, "Monthly"));

            return reportingPeriodTypes;
        }

        protected IEnumerable<ReportingPeriodStatus> GetAndConvertReportingPeriodStatus()
        {
            var reportingPeriodStatuses = new List<ReportingPeriodStatus>();
            reportingPeriodStatuses.Add(new ReportingPeriodStatus(1, "InActive"));
            reportingPeriodStatuses.Add(new ReportingPeriodStatus(2, "Open"));
            reportingPeriodStatuses.Add(new ReportingPeriodStatus(3, "Closed"));
            reportingPeriodStatuses.Add(new ReportingPeriodStatus(4, "Complete"));

            return reportingPeriodStatuses;
        }

        protected IEnumerable<SupplierReportingPeriodStatus> GetSupplierReportingPeriodStatuses()
        {
            var supplierReportingPeriodStatuses = new List<SupplierReportingPeriodStatus>();
            supplierReportingPeriodStatuses.Add(new SupplierReportingPeriodStatus(1, "Locked"));
            supplierReportingPeriodStatuses.Add(new SupplierReportingPeriodStatus(2, "Unlocked"));

            return supplierReportingPeriodStatuses;
        }

        protected IEnumerable<FacilityReportingPeriodDataStatus> GetFacilityReportingPeriodDataStatus()
        {
            var facilityReportingPeriodDataStatuses = new List<FacilityReportingPeriodDataStatus>();
            facilityReportingPeriodDataStatuses.Add(new FacilityReportingPeriodDataStatus(1, "In-progress"));
            facilityReportingPeriodDataStatuses.Add(new FacilityReportingPeriodDataStatus(1, "Complete"));
            facilityReportingPeriodDataStatuses.Add(new FacilityReportingPeriodDataStatus(1, "Submitted"));

            return facilityReportingPeriodDataStatuses;
        }

        #region ReportingPeriod methods

        protected ReportingPeriod GetReportingPeriodDomain()
        {
            var reportingPeriodEntity = CreateReportingPeriodEntity();
            var reportingPeriodTypes = GetAndConvertReportingPeriodTypes();
            var reportingPeriodStatuses = GetAndConvertReportingPeriodStatus();
            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();
            return mapper.ConvertReportingPeriodEntityToDomain(reportingPeriodEntity, reportingPeriodTypes, reportingPeriodStatuses);
        }

        protected ReportingPeriodEntity CreateReportingPeriodEntity()
        {
            var reportingPeriodEntity = new ReportingPeriodEntity();
            reportingPeriodEntity.Id = reportingPeriodId;
            reportingPeriodEntity.DisplayName = displayName;
            reportingPeriodEntity.ReportingPeriodTypeId = GetAndConvertReportingPeriodTypes().FirstOrDefault(x => x.Name == ReportingPeriodTypeValues.Annual).Id;
            reportingPeriodEntity.ReportingPeriodStatusId = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.InActive).Id;
            reportingPeriodEntity.CollectionTimePeriod = collectionTimePeriod;
            reportingPeriodEntity.StartDate = startDate;
            reportingPeriodEntity.EndDate = endDate;
            reportingPeriodEntity.IsActive = isActive;

            return reportingPeriodEntity;
        }

        #endregion

        #region PeriodSupplier methods

        protected ReportingPeriodSupplierEntity CreateReportingPeriodSupplierEntity()
        {
            var periodSupplierEntity = new ReportingPeriodSupplierEntity();
            periodSupplierEntity.Id = 1;
            periodSupplierEntity.SupplierId = 1;
            periodSupplierEntity.ReportingPeriodId = 1;
            periodSupplierEntity.SupplierReportingPeriodStatusId = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked).Id;
            periodSupplierEntity.IsActive = true;

            return periodSupplierEntity;
        }

        #endregion

        #region PeriodFacility methods

        protected ReportingPeriodFacilityEntity CreateReportingPeriodFacilityEntity()
        {
            var periodFacilityEntity = new ReportingPeriodFacilityEntity();
            periodFacilityEntity.Id = 1;
            periodFacilityEntity.FacilityId = 1;
            periodFacilityEntity.ReportingPeriodId = 1;
            periodFacilityEntity.FacilityReportingPeriodDataStatusId = GetSupplierReportingPeriodStatuses().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress).Id;
            periodFacilityEntity.ReportingPeriodSupplierId = 1;

            return periodFacilityEntity;
        }

        #endregion

        #region All ValueObjects methods

        protected SupplierVO GetAndConvertSupplierValueObject()
        {
            var supplierEntity = CreateSupplierEntity();
            var reportingTypes = GenerateReportingType();
            var supplyChainStages = GenerateSupplyChainStage();
            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();
            var supplierVO = mapper.ConvertSupplierToSupplierValueObject(supplierEntity, supplyChainStages, reportingTypes);
            return supplierVO;
        }

        protected FacilityVO GetAndConvertFacilityValueObject()
        {
            var supplierEntity = CreateSupplierEntity();
            var facilityEntity = GenerateFacilityEntitiesForSupplier(supplierEntity.Id).First();
            var reportingTypes = GenerateReportingType();
            var supplyChainStages = GenerateSupplyChainStage();
            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();
            var facilityVO = mapper.ConvertFacilityToFacilityValueObject(facilityEntity, supplyChainStages, reportingTypes);

            return facilityVO;
        }


        #endregion


        protected ReportingPeriodEntityDomainMapper CreateInstanceOfReportingPeriodEntityDomainMapper()
        {
            return new ReportingPeriodEntityDomainMapper();
        }

        protected ReportingPeriodDomainDtoMapper CreateInstanceOfReportingPeriodDomainDtoMapper()
        {
            return new ReportingPeriodDomainDtoMapper();
        }

        #endregion

    }
}