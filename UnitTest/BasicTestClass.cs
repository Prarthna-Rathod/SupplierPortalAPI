using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using DataAccess.Entities;
using Services.DTOs;
using Services.Mappers.ReportingPeriodMappers;
using Services.Mappers.SupplierMappers;

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
        private DateTime startDate = new DateTime(2023, 04, 12);
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

        protected List<ReportingPeriodActiveSupplierVO> createSupplierEntities()
        {
            var activeSupplierEntities = new List<ReportingPeriodActiveSupplierVO>();

            activeSupplierEntities.Add(new ReportingPeriodActiveSupplierVO()
            {
                ReportingPeriodSupplierId = 1,
                Supplier = GetAndConvertSupplierValueObject(),
                PeriodId = 3,
                SupplierPeriodStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Locked),
                InitialDataRequestDate = new DateTime(2023, 5, 10),
                ResendDataRequestDate = new DateTime(2023, 6, 10),
                IsActive = true
            });
            activeSupplierEntities.Add(new ReportingPeriodActiveSupplierVO()
            {
                ReportingPeriodSupplierId = 2,
                Supplier = GetAndConvertSupplierValueObject(),
                PeriodId = 4,
                SupplierPeriodStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Locked),
                InitialDataRequestDate = new DateTime(2023, 5, 10),
                ResendDataRequestDate = new DateTime(2023, 6, 10),
                IsActive = true
            });

            return activeSupplierEntities;

        }

        protected List<ReportingPeriodRelevantFacilityVO> createFacilityEntities()
        {
            var relevantFacilityEntities = new List<ReportingPeriodRelevantFacilityVO>();

            relevantFacilityEntities.Add(new ReportingPeriodRelevantFacilityVO()
            {
                ReportingPeriodFacilityId = 0,
                ReportingPeriodId = 2,
                FacilityVO = GetAndConvertFacilityValueObject(),
                SupplierId = 2,
                SupplierName = "",
                ReportingPeriodSupplierId = 1020,
                FacilityIsRelevantForPeriod = true,
                FacilityReportingPeriodDataStatusId = 1

            });

            relevantFacilityEntities.Add(new ReportingPeriodRelevantFacilityVO()
            {
                ReportingPeriodFacilityId = 2,
                ReportingPeriodId = 2,
                FacilityVO = GetAndConvertFacilityValueObject(),
                SupplierId = 2,
                SupplierName = "",
                ReportingPeriodSupplierId = 2,
                FacilityIsRelevantForPeriod = true,
                FacilityReportingPeriodDataStatusId = 1,

            });
            return relevantFacilityEntities;
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
                    IsActive = true,
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
                CreatedBy = "System",
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

        protected IEnumerable<FercRegion> GetFercRegions()
        {
            var fercRegion = new List<FercRegion>();
            fercRegion.Add(new FercRegion(1, "None"));
            fercRegion.Add(new FercRegion(2, "CAISO"));
            fercRegion.Add(new FercRegion(3, "MISO"));
            fercRegion.Add(new FercRegion(4, "PJM"));
            fercRegion.Add(new FercRegion(5, "SPP"));
            fercRegion.Add(new FercRegion(6, "ISO-NE"));
            fercRegion.Add(new FercRegion(7, "NYISO"));
            fercRegion.Add(new FercRegion(8, "ERCOT"));
            fercRegion.Add(new FercRegion(9, "Northwest"));
            fercRegion.Add(new FercRegion(10, "Southwest"));
            fercRegion.Add(new FercRegion(11, "Southeast"));
            fercRegion.Add(new FercRegion(12, "Custom Mix"));


            return fercRegion;
        }

        protected IEnumerable<ElectricityGridMixComponent> GetElectricityGridMixComponents()
        {
            var companents = new List<ElectricityGridMixComponent>();
            companents.Add(new ElectricityGridMixComponent(1, "Biomass"));
            companents.Add(new ElectricityGridMixComponent(2, "Coal"));
            companents.Add(new ElectricityGridMixComponent(3, "NaturalGas"));
            companents.Add(new ElectricityGridMixComponent(4, "Geothermal"));
            companents.Add(new ElectricityGridMixComponent(5, "Hydro"));
            companents.Add(new ElectricityGridMixComponent(6, "Nuclear"));
            companents.Add(new ElectricityGridMixComponent(7, "Petroleum"));
            companents.Add(new ElectricityGridMixComponent(8, "Solar"));
            companents.Add(new ElectricityGridMixComponent(9, "Wind"));


            return companents;
        }

        protected IEnumerable<UnitOfMeasure> GetUnitOfMeasures()
        {
            var units = new List<UnitOfMeasure>();
            units.Add(new UnitOfMeasure(1, "kg/m3"));
            units.Add(new UnitOfMeasure(2, "MMbtu/Mcf"));
            units.Add(new UnitOfMeasure(3, "MMbtu/bbl"));
            units.Add(new UnitOfMeasure(4, "MWh"));
            units.Add(new UnitOfMeasure(5, "tonne"));
            units.Add(new UnitOfMeasure(6, "Mass %"));
            return units;

        }

        #region ReportingPeriod methods

        protected ReportingPeriod GetReportingPeriodDomain()
        {
            var reportingPeriodEntity = CreateReportingPeriodEntity();
            var reportingPeriodTypes = GetAndConvertReportingPeriodTypes();
            var reportingPeriodStatuses = GetAndConvertReportingPeriodStatus();
            //var supplierVo = GetAndConvertSupplierValueObject();

            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();
            return mapper.ConvertReportingPeriodEntityToDomain(reportingPeriodEntity, reportingPeriodTypes, reportingPeriodStatuses);
        }

        protected ReportingPeriodEntity CreateReportingPeriodEntity()
        {
            var reportingPeriodEntity = new ReportingPeriodEntity();
            reportingPeriodEntity.Id = reportingPeriodId;
            reportingPeriodEntity.DisplayName = displayName;
            reportingPeriodEntity.ReportingPeriodTypeId = GetAndConvertReportingPeriodTypes().FirstOrDefault(x => x.Name == ReportingPeriodTypeValues.Annual).Id;
            reportingPeriodEntity.ReportingPeriodStatusId = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Open).Id;
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

        protected List<ReportingPeriodActiveSupplierVO> createPeriodSupplierEntities()
        {
            var activeSupplierEntities = new List<ReportingPeriodActiveSupplierVO>();

            activeSupplierEntities.Add(new ReportingPeriodActiveSupplierVO()
            {
                ReportingPeriodSupplierId = 1,
                Supplier = GetAndConvertSupplierValueObject(),
                PeriodId = 1,
                SupplierPeriodStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked),
                InitialDataRequestDate = null,
                ResendDataRequestDate = null,
                IsActive = true
            });
            return activeSupplierEntities;
        }



        #endregion

        #region PeriodFacility methods

        protected List<ReportingPeriodRelevantFacilityVO> CreateReportingPeriodFacilityEntity()
        {
            var periodFacilityEntity = new List<ReportingPeriodRelevantFacilityVO>();

            periodFacilityEntity.Add(new ReportingPeriodRelevantFacilityVO()
            {
                ReportingPeriodFacilityId = 1,
                ReportingPeriodId = 1,
                FacilityVO = GetAndConvertFacilityValueObject(),
                SupplierId = 1,
                SupplierName="",
                ReportingPeriodSupplierId=1,
                FacilityIsRelevantForPeriod=true,
                FacilityReportingPeriodDataStatusId = 1
            });

            return periodFacilityEntity;
        }
        #endregion


        #region PeriodFacilityElctricityGridMix methods

        protected List<ReportingPeriodFacilityElectricityGridMixVO> CreateReportingPeriodFacilityElecticityGridMixEntity()
        {
            var periodFacilityElectricityGridMixVO = new List<ReportingPeriodFacilityElectricityGridMixVO>();

            periodFacilityElectricityGridMixVO.Add(new ReportingPeriodFacilityElectricityGridMixVO()
            {
                Id = 1,
                UnitOfMeasure = GetUnitOfMeasures().FirstOrDefault(),
                ElectricityGridMixComponent = GetElectricityGridMixComponents().FirstOrDefault(),
                Content = 100,
                IsActive = true


            }) ;
            return periodFacilityElectricityGridMixVO;
        }

        protected AddMultiplePeriodFacilityElectricityGridMixDto CreateReportingPeriodFacilityElecticityGridMixDto()
        {
            var electricityGridMixDto = new AddMultiplePeriodFacilityElectricityGridMixDto();

            electricityGridMixDto.ReportingPeriodId = 1;
            electricityGridMixDto.ReportingPeriodFacilityId = 1;
            electricityGridMixDto.ReportingPeriodSupplierId= 1;
            electricityGridMixDto.FercRegionId = 12;


            return electricityGridMixDto;
        }
        #endregion

    #region All ValueObjects methods

    protected SupplierVO GetAndConvertSupplierValueObject()
    {
        var supplierEntity = CreateSupplierEntity();
        var reportingTypes = GenerateReportingType();
        var supplyChainStages = GenerateSupplyChainStage();
        var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();
        var supplierVO = mapper.ConvertSupplierEntityToSupplierValueObject(supplierEntity, supplyChainStages, reportingTypes);
        return supplierVO;
    }

    protected FacilityVO GetAndConvertFacilityValueObject()
    {
        var supplierEntity = CreateSupplierEntity();
        var facilityEntity = GenerateFacilityEntitiesForSupplier(supplierEntity.Id).First();
        var reportingTypes = GenerateReportingType();
        var supplyChainStages = GenerateSupplyChainStage();
        var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();
        var facilityVO = mapper.ConvertFacilityEntityToFacilityValueObject(facilityEntity, supplyChainStages, reportingTypes);

        return facilityVO;
    }

        protected ReportingPeriod AddPeriodSupplierAndPeriodFacilityForReportingPeriod()
        {
            var reportingPeriod = GetReportingPeriodDomain();

            //Get PeriodSupplier Domain
            var supplierVO = GetAndConvertSupplierValueObject();
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddRemovePeriodSupplier(createPeriodSupplierEntities());

            //Add PeriodFacility
            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodStatus = GetFacilityReportingPeriodDataStatus()/*.First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress)*/;
            var fercRegion = GetFercRegions();
            var periodFacility = reportingPeriod.AddRemoveUpdatePeriodFacility(createFacilityEntities(), fercRegion, facilityReportingPeriodStatus,periodSupplier.FirstOrDefault());

            //Update reportingPeriodStatus InActive to Open
            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Open);
            reportingPeriod.ReportingPeriodStatus.Id = reportingPeriodStatus.Id;
            reportingPeriod.ReportingPeriodStatus.Name = reportingPeriodStatus.Name;

            return reportingPeriod;
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