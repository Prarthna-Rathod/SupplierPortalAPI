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
                AssociatePipelineId = null,
                SupplyChainStageId = 1,
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
            facilityReportingPeriodDataStatuses.Add(new FacilityReportingPeriodDataStatus(2, "Complete"));
            facilityReportingPeriodDataStatuses.Add(new FacilityReportingPeriodDataStatus(3, "Submitted"));

            return facilityReportingPeriodDataStatuses;
        }

        protected IEnumerable<UnitOfMeasure> GetUnitOfMeasures()
        {
            var unitOfMeasure = new List<UnitOfMeasure>();
            unitOfMeasure.Add(new UnitOfMeasure(1, "kg/m3"));
            unitOfMeasure.Add(new UnitOfMeasure(2, "MMbtu/Mcf"));
            unitOfMeasure.Add(new UnitOfMeasure(3, "MMbtu/bbl"));
            unitOfMeasure.Add(new UnitOfMeasure(4, "MWh"));
            unitOfMeasure.Add(new UnitOfMeasure(5, "tonne"));
            unitOfMeasure.Add(new UnitOfMeasure(6, "Mass % "));
            return unitOfMeasure;
        }

        protected IEnumerable<FercRegion> GetFercRegions()
        {
            var fercRegion = new List<FercRegion>();
            fercRegion.Add(new FercRegion(1, "None"));
            fercRegion.Add(new FercRegion(2, "CAISO"));
            fercRegion.Add(new FercRegion(3, "MISO"));
            fercRegion.Add(new FercRegion(4, "PJM"));
            fercRegion.Add(new FercRegion(5, "SPP"));
            fercRegion.Add(new FercRegion(6, "Custom Mix"));
            return fercRegion;
        }

        protected IEnumerable<ElectricityGridMixComponent> GetElectricityGridMixComponents()
        {
            var electricityGridMixComponent = new List<ElectricityGridMixComponent>();
            electricityGridMixComponent.Add(new ElectricityGridMixComponent(1, "Biomass"));
            electricityGridMixComponent.Add(new ElectricityGridMixComponent(2, "Coal"));
            electricityGridMixComponent.Add(new ElectricityGridMixComponent(3, "NaturalGas"));
            electricityGridMixComponent.Add(new ElectricityGridMixComponent(4, "Geothermal"));
            electricityGridMixComponent.Add(new ElectricityGridMixComponent(5, "Hydro"));
            electricityGridMixComponent.Add(new ElectricityGridMixComponent(6, "Nuclear"));
            electricityGridMixComponent.Add(new ElectricityGridMixComponent(7, "Petroleum"));
            electricityGridMixComponent.Add(new ElectricityGridMixComponent(8, "Solar"));
            electricityGridMixComponent.Add(new ElectricityGridMixComponent(9, "Wind"));
            return electricityGridMixComponent;
        }

        protected IEnumerable<Site> GetSites()
        {
            var site = new List<Site>();
            site.Add(new Site(1, "SPL"));
            site.Add(new Site(2, "CCL"));
            return site;
        }

        #region protected method

        protected ReportingPeriod AddPeriodSupplierAndPeriodFacilityForReportingPeriod()
        {
            var reportingPeriod = GetReportingPeriodDomain();

            //Get PeriodSupplier Domain
            var supplierVO = GetAndConvertSupplierValueObject();
            var supplierReportingPerionStatus = GetSupplierReportingPeriodStatuses().FirstOrDefault(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPerionStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            //Add PeriodFacility
            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
            var periodFacility = reportingPeriod.AddPeriodFacility(1, facilityVO, facilityReportingPeriodStatus, periodSupplier.Id, true, true);

            //Update reportingPeriodStatus InActive to Open
            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Open);
            reportingPeriod.ReportingPeriodStatus.Id = reportingPeriodStatus.Id;
            reportingPeriod.ReportingPeriodStatus.Name = reportingPeriodStatus.Name;

            return reportingPeriod;
        }

        #endregion

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
            periodFacilityEntity.FacilityReportingPeriodDataStatusId = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress).Id;
            periodFacilityEntity.ReportingPeriodSupplierId = 1;

            return periodFacilityEntity;
        }

        #endregion

        #region ElectricityGridMix methods

        protected IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> CreatePeriodFacilityElectricityGridMixEntity()
        {
            var list = new List<ReportingPeriodFacilityElectricityGridMixEntity>();
            list.Add(new ReportingPeriodFacilityElectricityGridMixEntity()
            {
                Id = 1,
                ReportingPeriodFacilityId = 1,
                ElectricityGridMixComponentId = 1,
                UnitOfMeasureId = 1,
                FercRegionId = 12,
                Content = (decimal)50.00,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
            });
            list.Add(new ReportingPeriodFacilityElectricityGridMixEntity()
            {
                Id = 2,
                ReportingPeriodFacilityId = 1,
                ElectricityGridMixComponentId = 2,
                UnitOfMeasureId = 1,
                FercRegionId = 12,
                Content = (decimal)50.00,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
            });
            return list;
        }

        #endregion

        #region GasSupplyBreakdown methods

        protected ReportingPeriodFacilityGasSupplyBreakDownEntity CreatePeriodFacilityGasSupplyBreakdownEntity()
        {
            var periodFacilityEntity = CreateReportingPeriodFacilityEntity();
            var gasSupplyBreakdownEntity = new ReportingPeriodFacilityGasSupplyBreakDownEntity();
            gasSupplyBreakdownEntity.Id = 1;
            gasSupplyBreakdownEntity.PeriodFacilityId = 1;
            gasSupplyBreakdownEntity.PeriodFacility = periodFacilityEntity;
            gasSupplyBreakdownEntity.SiteId = 1;
            gasSupplyBreakdownEntity.UnitOfMeasureId = 1;
            gasSupplyBreakdownEntity.Content = (decimal)100.00;

            return gasSupplyBreakdownEntity;
        }

        #endregion

        #region PeriodFacilityElectricityGridMix VO
        protected IEnumerable<ElectricityGridMixComponentPercent> GetElectricityGridMixComponentPercents()
        {
            var list = new List<ElectricityGridMixComponentPercent>();
            var electricityGridMIxComponent = GetElectricityGridMixComponents();
            list.Add(new ElectricityGridMixComponentPercent(1, electricityGridMIxComponent.First(), (decimal)25.00));
            list.Add(new ElectricityGridMixComponentPercent(2, electricityGridMIxComponent.FirstOrDefault(x => x.Id == 2), (decimal)25.00));
            list.Add(new ElectricityGridMixComponentPercent(3, electricityGridMIxComponent.FirstOrDefault(x => x.Id == 3), (decimal)25.00));
            list.Add(new ElectricityGridMixComponentPercent(4, electricityGridMIxComponent.FirstOrDefault(x => x.Id == 4), (decimal)25.00));
            //list.Add(new ElectricityGridMixComponentPercent(5, electricityGridMIxComponent.FirstOrDefault(x => x.Id == 4), (decimal)20.00));
            return list;
        }

        //Repeated method because check testcase in existing AddPeriodFacilityElectricityGridMix are remove it and add new electricityGridMix
        protected IEnumerable<ElectricityGridMixComponentPercent> GetElectricityGridMixComponentPercents2()
        {
            var list = new List<ElectricityGridMixComponentPercent>();
            var electricityGridMIxComponent = GetElectricityGridMixComponents();
            list.Add(new ElectricityGridMixComponentPercent(1, electricityGridMIxComponent.First(), (decimal)50.00));
            list.Add(new ElectricityGridMixComponentPercent(2, electricityGridMIxComponent.FirstOrDefault(x => x.Id == 2), (decimal)50.00));
            return list;
        }

        #endregion

        #region PeriodFacilityGasSupplyBreakdown VO

        protected IEnumerable<GasSupplyBreakdownVO> GetGasSupplyBreakdowns()
        {
            var list = new List<GasSupplyBreakdownVO>();
            var site = GetSites();
            var unitOfMeasure = GetUnitOfMeasures().First();
            list.Add(new GasSupplyBreakdownVO(1, 1, 1, site.First(), unitOfMeasure, (decimal)100.00));
            list.Add(new GasSupplyBreakdownVO(2, 1, 1, site.FirstOrDefault(x => x.Id == 2), unitOfMeasure, (decimal)100.00));
            //list.Add(new GasSupplyBreakdownVO(2, 1, 1, site.FirstOrDefault(x => x.Id == 2), unitOfMeasure, (decimal)50.00));
            return list;
        }

        protected IEnumerable<GasSupplyBreakdownVO> GetGasSupplyBreakdowns2()
        {
            var list = new List<GasSupplyBreakdownVO>();
            var site = GetSites();
            var unitOfMeasure = GetUnitOfMeasures().First();
            list.Add(new GasSupplyBreakdownVO(1, 1, 1, site.First(), unitOfMeasure, (decimal)100.00));
            return list;
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


        #endregion

        #region All Dto mappers methods

        protected ReportingPeriodFacilityGasSupplyBreakdownDto PeriodFacilityGasSupplyBreakdownDto()
        {
            return new ReportingPeriodFacilityGasSupplyBreakdownDto(1, 1, "Test facility 1", 1, "abc", 1, "Biomass", (decimal)100.00);
        }

        protected IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> PeriodFacilityElectricityGridMixDto()
        {
            var list = new List<ReportingPeriodFacilityElectricityGridMixDto>();
            list.Add(new ReportingPeriodFacilityElectricityGridMixDto(1, "string", (decimal)50.00));
            list.Add(new ReportingPeriodFacilityElectricityGridMixDto(2, "string", (decimal)50.00));

            return list;
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