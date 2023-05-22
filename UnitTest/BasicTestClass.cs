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
            var unitOfMeasures = new List<UnitOfMeasure>();
            unitOfMeasures.Add(new UnitOfMeasure(1, "kg/m3"));
            unitOfMeasures.Add(new UnitOfMeasure(2, "MMbtu/Mcf"));
            unitOfMeasures.Add(new UnitOfMeasure(3, "MMbtu/bbl"));
            unitOfMeasures.Add(new UnitOfMeasure(4, "MWh"));
            unitOfMeasures.Add(new UnitOfMeasure(5, "tonne"));
            unitOfMeasures.Add(new UnitOfMeasure(6, "Mass %"));
            return unitOfMeasures;
        }

        protected IEnumerable<FercRegion> GetFercRegions()
        {
            var fercRegions = new List<FercRegion>();
            fercRegions.Add(new FercRegion(1, "None"));
            fercRegions.Add(new FercRegion(2, "CAISO"));
            fercRegions.Add(new FercRegion(3, "PJM"));
            fercRegions.Add(new FercRegion(4, "Southeast"));
            fercRegions.Add(new FercRegion(5, "Custom Mix"));
            return fercRegions;
        }

        protected IEnumerable<ElectricityGridMixComponent> GetElectricityGridMixComponents()
        {
            var components = new List<ElectricityGridMixComponent>();
            components.Add(new ElectricityGridMixComponent(1, "Biomass"));
            components.Add(new ElectricityGridMixComponent(2, "Coal"));
            components.Add(new ElectricityGridMixComponent(3, "NaturalGas"));
            components.Add(new ElectricityGridMixComponent(4, "Geothermal"));
            components.Add(new ElectricityGridMixComponent(5, "Hydro"));

            return components;
        }

        protected IEnumerable<Site> GetSites()
        {
            var sites = new List<Site>();
            sites.Add(new Site(1, "SPL"));
            sites.Add(new Site(2, "CCL"));

            return sites;
        }

        protected IEnumerable<DocumentStatus> GetDocumentStatuses() 
        {
            var documentStatuses = new List<DocumentStatus>();
            documentStatuses.Add(new DocumentStatus(1, "Not-validated"));
            documentStatuses.Add(new DocumentStatus(2, "Validated"));
            documentStatuses.Add(new DocumentStatus(3, "Has errors"));
            documentStatuses.Add(new DocumentStatus(4, "Processing"));

            return documentStatuses;
        }

        protected IEnumerable<DocumentType> GetDocumentTypes()
        {
            var documentTypes = new List<DocumentType>();
            documentTypes.Add(new DocumentType(1, "Subpart C"));
            documentTypes.Add(new DocumentType(2, "Subpart W"));
            documentTypes.Add(new DocumentType(3, "Non-GHGRP"));
            documentTypes.Add(new DocumentType(4, "Supplemental"));

            return documentTypes;
        }

        protected IEnumerable<DocumentRequiredStatus> GetDocumentRequiredStatuses()
        {
            var documentRequiredStatuses = new List<DocumentRequiredStatus>();
            documentRequiredStatuses.Add(new DocumentRequiredStatus(1, "Optional"));
            documentRequiredStatuses.Add(new DocumentRequiredStatus(2, "Required"));
            documentRequiredStatuses.Add(new DocumentRequiredStatus(3, "Not-allowed"));

            return documentRequiredStatuses;
        }

        #region ReportingPeriod methods

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
            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.Custom_Mix);
            var periodFacility = reportingPeriod.AddPeriodFacility(1, facilityVO, facilityReportingPeriodStatus, periodSupplier.Id, true, fercRegion, true);

            //Update ReportingPeriodStatus InActive To Open
            var updatePeriodStatus = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Open);
            reportingPeriod.ReportingPeriodStatus.Id = updatePeriodStatus.Id;
            reportingPeriod.ReportingPeriodStatus.Name = updatePeriodStatus.Name;

            return reportingPeriod;
        }


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
            periodFacilityEntity.FercRegionId = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.Custom_Mix).Id;

            return periodFacilityEntity;
        }

        protected ReportingPeriodFacilityGasSupplyBreakDownEntity CreatePeriodFacilityGasSupplyBreakdownEntity()
        {
            var periodFacilityEntity = CreateReportingPeriodFacilityEntity();
            var entity = new ReportingPeriodFacilityGasSupplyBreakDownEntity();
            entity.Id = 1;
            entity.PeriodFacilityId = periodFacilityEntity.Id;
            entity.PeriodFacility = periodFacilityEntity;
            entity.SiteId = 1;
            entity.UnitOfMeasureId = 1;
            entity.Content = (decimal)100.00;
            entity.IsActive = true;
            entity.CreatedOn = DateTime.UtcNow;
            entity.CreatedBy = "System";

            return entity;

        }

        protected IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> CreatePeriodFacilityElectricityGridMixEntities()
        {
            var entityList = new List<ReportingPeriodFacilityElectricityGridMixEntity>();
            entityList.Add(new ReportingPeriodFacilityElectricityGridMixEntity()
            {
                Id = 1,
                ReportingPeriodFacilityId = 1,
                UnitOfMeasureId = 1,
                ElectricityGridMixComponentId = 1,
                Content = (decimal)50.00,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System"
            });
            entityList.Add(new ReportingPeriodFacilityElectricityGridMixEntity()
            {
                Id = 2,
                ReportingPeriodFacilityId = 1,
                UnitOfMeasureId = 1,
                ElectricityGridMixComponentId = 2,
                Content = (decimal)50.00,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System"
            });

            return entityList;
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

        protected IEnumerable<ElectricityGridMixComponentPercent> GetElectricityGridMixComponentPercents()
        {
            var list = new List<ElectricityGridMixComponentPercent>();
            var electricityGridMixComponents = GetElectricityGridMixComponents();

            list.Add(new ElectricityGridMixComponentPercent(1, electricityGridMixComponents.First(), (decimal)20.00));
            list.Add(new ElectricityGridMixComponentPercent(2, electricityGridMixComponents.FirstOrDefault(x => x.Id == 2), (decimal)20.00));
            list.Add(new ElectricityGridMixComponentPercent(3, electricityGridMixComponents.FirstOrDefault(x => x.Id == 3), (decimal)20.00));
            list.Add(new ElectricityGridMixComponentPercent(4, electricityGridMixComponents.FirstOrDefault(x => x.Id == 4), (decimal)20.00));
            list.Add(new ElectricityGridMixComponentPercent(5, electricityGridMixComponents.FirstOrDefault(x => x.Id == 5), (decimal)20.00));

            return list;
        }

        protected IEnumerable<ElectricityGridMixComponentPercent> GetElectricityGridMixComponentPercentsList2()
        {
            var list = new List<ElectricityGridMixComponentPercent>();
            var electricityGridMixComponents = GetElectricityGridMixComponents();

            list.Add(new ElectricityGridMixComponentPercent(1, electricityGridMixComponents.First(), (decimal)50.00));
            list.Add(new ElectricityGridMixComponentPercent(2, electricityGridMixComponents.FirstOrDefault(x => x.Id == 2), (decimal)25.00));
            list.Add(new ElectricityGridMixComponentPercent(3, electricityGridMixComponents.FirstOrDefault(x => x.Id == 3), (decimal)25.00));

            return list;
        }

        protected IEnumerable<GasSupplyBreakdownVO> GetGasSupplyBreakdownVOs()
        {
            var list = new List<GasSupplyBreakdownVO>();
            var sites = GetSites();
            var unitOfMeasure = GetUnitOfMeasures().First();

            list.Add(new GasSupplyBreakdownVO(0, 1, 1, sites.First(), unitOfMeasure, (decimal)100.00));
            list.Add(new GasSupplyBreakdownVO(0, 1, 1, sites.FirstOrDefault(x => x.Id == 2), unitOfMeasure, (decimal)100.00));

            return list;
        }

        protected IEnumerable<GasSupplyBreakdownVO> GetGasSupplyBreakdownVOsList2()
        {
            var list = new List<GasSupplyBreakdownVO>();
            var sites = GetSites();
            var unitOfMeasure = GetUnitOfMeasures().First();

            list.Add(new GasSupplyBreakdownVO(0, 1, 1, sites.First(), unitOfMeasure, (decimal)100.00));

            return list;
        }

        protected IEnumerable<FacilityRequiredDocumentTypeVO> GetFacilityRequiredDocumentTypeVOs()
        {
            var voList = new List<FacilityRequiredDocumentTypeVO>();
            var reportingType = GenerateReportingType().First();
            var supplyChainStage = GenerateSupplyChainStage().First();
            //var documentStatus = GetD

            return voList;
        }

        #endregion

        #region Created Dto for mapper testing

        protected IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> PeriodFacilityElectricityGridMixDtos()
        {
            var electricityGridMixDtos = new List<ReportingPeriodFacilityElectricityGridMixDto>();
            electricityGridMixDtos.Add(new ReportingPeriodFacilityElectricityGridMixDto(1, "string", (decimal)50.00));
            electricityGridMixDtos.Add(new ReportingPeriodFacilityElectricityGridMixDto(2, "string", (decimal)50.00));

            return electricityGridMixDtos;

        }

        protected ReportingPeriodFacilityGasSupplyBreakdownDto CreatePeriodFacilityGasSupplyBreakdownDto()
        {
            return new ReportingPeriodFacilityGasSupplyBreakdownDto(1, 1, "FacilityName", 1, "siteName", 1, "UnitOfMeasureName", (decimal)100.00);
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