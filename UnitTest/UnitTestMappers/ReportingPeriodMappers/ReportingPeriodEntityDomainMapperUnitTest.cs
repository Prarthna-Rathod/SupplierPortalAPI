using BusinessLogic.ValueConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.UnitTestMappers.ReportingPeriodMappers
{
    public class ReportingPeriodEntityDomainMapperUnitTest: BasicTestClass
    {
        #region ReportingPeriod mappers

        /// <summary>
        /// Convert ReportingPeriodDomain To ReportingPeriodEntity mapper testing
        /// </summary>
        [Fact]
        public void ConvertReportingPeriodDomainToEntity()
        {
            var reportingPeriod = GetReportingPeriodDomain();
            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();

            var reportingPeriodEntity = mapper.ConvertReportingPeriodDomainToEntity(reportingPeriod);

            Assert.NotNull(reportingPeriodEntity);
            Assert.Equal(reportingPeriod.Id, reportingPeriodEntity.Id);
            Assert.Equal(reportingPeriod.DisplayName, reportingPeriodEntity.DisplayName);
            Assert.Equal(reportingPeriod.ReportingPeriodType.Id, reportingPeriodEntity.ReportingPeriodTypeId);
            Assert.Equal(reportingPeriod.CollectionTimePeriod, reportingPeriodEntity.CollectionTimePeriod);
            Assert.Equal(reportingPeriod.ReportingPeriodStatus.Id, reportingPeriodEntity.ReportingPeriodStatusId);
            Assert.Equal(reportingPeriod.StartDate.Date, reportingPeriodEntity.StartDate.Date);
            Assert.Equal(reportingPeriod.EndDate, reportingPeriodEntity.EndDate);
            Assert.Equal(reportingPeriod.IsActive, reportingPeriodEntity.IsActive);
        }

        /// <summary>
        /// Convert ReportingPeriodEntity To ReportingPeriodDomain mapper testing
        /// </summary>
        [Fact]
        public void ConvertReportingPeriodEntityToDomain()
        {
            var reportingPeriodEntity = CreateReportingPeriodEntity();
            var reportingPeriodTypes = GetAndConvertReportingPeriodTypes();
            var reportingPeriodStatuses = GetAndConvertReportingPeriodStatus();
            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();

            var reportingPeriodDomain = mapper.ConvertReportingPeriodEntityToDomain(reportingPeriodEntity, reportingPeriodTypes, reportingPeriodStatuses);

            Assert.NotNull(reportingPeriodDomain);
            Assert.Equal(reportingPeriodEntity.Id, reportingPeriodDomain.Id);
            Assert.Equal(reportingPeriodEntity.DisplayName, reportingPeriodDomain.DisplayName);
            Assert.Equal(reportingPeriodEntity.ReportingPeriodTypeId, reportingPeriodDomain.ReportingPeriodType.Id);
            Assert.Equal(reportingPeriodEntity.CollectionTimePeriod, reportingPeriodDomain.CollectionTimePeriod);
            Assert.Equal(reportingPeriodEntity.ReportingPeriodStatusId, reportingPeriodDomain.ReportingPeriodStatus.Id);
            Assert.Equal(reportingPeriodEntity.StartDate.Date, reportingPeriodDomain.StartDate.Date);
            Assert.Equal(reportingPeriodEntity.EndDate, reportingPeriodDomain.EndDate);
            Assert.Equal(reportingPeriodEntity.IsActive, reportingPeriodDomain.IsActive);
        }

        #endregion

        #region PeriodSupplier

        /// <summary>
        /// Convert ReportingPeriodSupplierDomain to Entity
        /// </summary>
        [Fact]
        public void ConvertPeriodSupplierDomainToEntity()
        {
            var reportingPeriod = GetReportingPeriodDomain();
            var supplierVO = GetAndConvertSupplierValueObject();
            var supplierReportingPeriodStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplierDomain = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPeriodStatus,new DateTime(2024,02,11), new DateTime(2024, 02, 11));

            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();

            var periodSupplierEntity = mapper.ConvertReportingPeriodSupplierDomainToEntity(periodSupplierDomain);

            Assert.NotNull(periodSupplierEntity);
            Assert.Equal(periodSupplierDomain.Id, periodSupplierEntity.Id);
            Assert.Equal(periodSupplierDomain.Supplier.Id, periodSupplierEntity.SupplierId);
            Assert.Equal(periodSupplierDomain.ReportingPeriodId, periodSupplierEntity.ReportingPeriodId);
            Assert.Equal(periodSupplierDomain.SupplierReportingPeriodStatus.Id, periodSupplierEntity.SupplierReportingPeriodStatusId);
            Assert.Equal(periodSupplierDomain.IsActive, periodSupplierEntity.IsActive);
            Assert.Equal(periodSupplierDomain.InitialDataRequestDate, periodSupplierEntity.InitialDataRequestDate);
            Assert.Equal(periodSupplierDomain.ResendDataRequestDate, periodSupplierEntity.ResendDataRequestDate);
        }

        [Fact]
        public void ConvertSupplierEntityToSupplierVO()
        {
            var supplierEntity = CreateSupplierEntity();
            var reportingTypes = GenerateReportingType();
            var supplyChainStages = GenerateSupplyChainStage();
            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();

            var supplierVO = mapper.ConvertSupplierEntityToSupplierValueObject(supplierEntity, supplyChainStages,reportingTypes );

            Assert.NotNull(supplierVO);
            Assert.Equal(supplierEntity.Id, supplierVO.Id);
            Assert.Equal(supplierEntity.Name, supplierVO.Name);
            Assert.Equal(supplierEntity.IsActive, supplierVO.IsActive);
            Assert.Equal(supplierEntity.FacilityEntities.Count(), supplierVO.Facilities.Count());
        }

        #endregion

        #region PeriodFacility

        [Fact]
        public void ConvertFacilityEntityToFacilityVO()
        {
            var facilityEntity = GenerateFacilityEntitiesForSupplier(1).First();
            var reportingTypes = GenerateReportingType();
            var supplyChainStages = GenerateSupplyChainStage();
            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();

            var facilityVO = mapper.ConvertFacilityEntityToFacilityValueObject(facilityEntity, supplyChainStages, reportingTypes);

            Assert.NotNull(facilityVO);
            Assert.Equal(facilityEntity.Id, facilityVO.Id);
            Assert.Equal(facilityEntity.Name, facilityVO.FacilityName);
            Assert.Equal(facilityEntity.GhgrpfacilityId, facilityVO.GHGRPFacilityId);
            Assert.Equal(facilityEntity.SupplierId, facilityVO.SupplierId);
            Assert.Equal(facilityEntity.IsActive, facilityVO.IsActive);
            Assert.Equal(facilityEntity.SupplyChainStageId, facilityVO.SupplyChainStage.Id);
            Assert.Equal(facilityEntity.ReportingTypeId, facilityVO.ReportingType.Id);

        }

        [Fact]
        public void ConvertPeriodFacilityDomainToEntity()
        {
            var reportingPeriod = GetReportingPeriodDomain();
           // var periodSupplierDomain = reportingPeriod.PeriodSuppliers.First();
            
            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodDataStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.CustomMix);
            var periodFacility = reportingPeriod.AddPeriodFacility(1, facilityVO, facilityReportingPeriodDataStatus, 1, true,fercRegion, true);
            
            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();

            var periodFacilityEntity = mapper.ConvertReportingPeriodFacilityDomainToEntity(periodFacility);

            Assert.NotNull(periodFacilityEntity);
            Assert.Equal(periodFacility.Id, periodFacilityEntity.Id);
            Assert.Equal(periodFacility.FacilityVO.Id, periodFacilityEntity.FacilityId);
            Assert.Equal(periodFacility.FacilityReportingPeriodDataStatus.Id, periodFacilityEntity.FacilityReportingPeriodDataStatusId);
            Assert.Equal(periodFacility.ReportingPeriodId, periodFacilityEntity.ReportingPeriodId);
            Assert.Equal(periodFacility.ReportingPeriodSupplierId, periodFacilityEntity.ReportingPeriodSupplierId);
            Assert.Equal(periodFacility.IsActive, periodFacilityEntity.IsActive);

        }

        #endregion

        #region PeriodFacilityElectricityGridMix

        [Fact]
        public void ConvertPeriodFacilityElectricityGridMixDomainToEntity()
        {
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.CustomMix);
            var percent = GetElectricityGridMixComponentPercents();

            var facilityElectricityGridMixDomain = reportingPeriod.AddPeriodFacilityElectricityGridMix(1,1,unitOfMeasure,fercRegion,percent);

            var gridMixDomain = facilityElectricityGridMixDomain.First();

            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();
            var periodFacilityElectricityGridMixEntity = mapper.ConvertPeriodFacilityElectricityGridMixDomainToEntity(gridMixDomain);

            Assert.NotNull(periodFacilityElectricityGridMixEntity);
            Assert.Equal(gridMixDomain.Id,periodFacilityElectricityGridMixEntity.Id);
            Assert.Equal(gridMixDomain.PeriodFacilityId, periodFacilityElectricityGridMixEntity.ReportingPeriodFacilityId);
            Assert.Equal(gridMixDomain.ElectricityGridMixComponent.Id, periodFacilityElectricityGridMixEntity.ElectricityGridMixComponentId);
            Assert.Equal(gridMixDomain.UnitOfMeasure.Id, periodFacilityElectricityGridMixEntity.UnitOfMeasureId);
            Assert.Equal(gridMixDomain.Content, periodFacilityElectricityGridMixEntity.Content);
        }

        [Fact]
        public void ConvertPeriodFacilityElectricityGridMixEntitiesToValueObjects()
        {
            var electricityGridMixComponent = GetElectricityGridMixComponents();
            var gridMixEntity = CreatePeriodFacilityElectricityGridMixEntity();

            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();
            var gridMixVo = mapper.ConvertPeriodFacilityElectricityGridMixEntitiesToValueObjects(gridMixEntity,electricityGridMixComponent);

            Assert.NotNull(gridMixVo);
            Assert.Equal(gridMixEntity.Count(), gridMixVo.Count());
        }

        #endregion

        #region PeriodFacilityGasSupplyBreakdown

        [Fact]
        public void ConvertPeriodFacilityGasSupplyBreakdownDomainToEntity()
        {
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var gasSupplyBreakdownVos = GetGasSupplyBreakdowns();

            var gasSupplyBreakdownDomain = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1,gasSupplyBreakdownVos);

            var periodFacilityGasSupplyBreakdownDomain = gasSupplyBreakdownDomain.First();

            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();
            var gasSupplyBreakdownEntity = mapper.ConvertPeriodFacilityGasSupplyBreakdownDomainToEntity(periodFacilityGasSupplyBreakdownDomain);

            Assert.NotNull(gasSupplyBreakdownEntity);
            Assert.Equal(periodFacilityGasSupplyBreakdownDomain.Id, gasSupplyBreakdownEntity.Id);
            Assert.Equal(periodFacilityGasSupplyBreakdownDomain.PeriodFacilityId, gasSupplyBreakdownEntity.PeriodFacilityId);
            Assert.Equal(periodFacilityGasSupplyBreakdownDomain.Site.Id, gasSupplyBreakdownEntity.SiteId);
            Assert.Equal(periodFacilityGasSupplyBreakdownDomain.UnitOfMeasure.Id, gasSupplyBreakdownEntity.UnitOfMeasureId);
            Assert.Equal(periodFacilityGasSupplyBreakdownDomain.Content,gasSupplyBreakdownEntity.Content);
        }

        [Fact]
        public void ConvertPeriodFacilityGasSupplyBreakdownEntityToValueObject()
        {
            var gasSupplyBreakdownEntity = CreatePeriodFacilityGasSupplyBreakdownEntity();
            var site = GetSites().First();
            var unitOfMeasure = GetUnitOfMeasures().First();

            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();
            var gasSupplyBreakdownVo = mapper.ConvertPeriodFacilityGasSupplyBreakdownEntityToValueObject(gasSupplyBreakdownEntity,site,unitOfMeasure);

            Assert.NotNull(gasSupplyBreakdownVo);
            Assert.Equal(gasSupplyBreakdownEntity.Id, gasSupplyBreakdownVo.Id);
            Assert.Equal(gasSupplyBreakdownEntity.PeriodFacilityId,gasSupplyBreakdownVo.PeriodFacilityId);
            Assert.Equal(gasSupplyBreakdownEntity.PeriodFacility.FacilityId, gasSupplyBreakdownVo.FacilityId);
            Assert.Equal(gasSupplyBreakdownEntity.SiteId, gasSupplyBreakdownVo.Site.Id);
            Assert.Equal(gasSupplyBreakdownEntity.UnitOfMeasureId, gasSupplyBreakdownVo.UnitOfMeasure.Id);
            Assert.Equal(gasSupplyBreakdownEntity.Content,gasSupplyBreakdownVo.Content);
        }

        #endregion

        #region PeriodFacilityDocument

        [Fact]
        public void ConvertReportingPeriodFacilityDocumentDomainToEntity()
        {
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForReportingPeriod();

            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().FirstOrDefault(x => x.Name == DocumentTypeValues.SubpartC);
            var facilityRequiredDocumentTypeVOs = GetFacilityRequiredDocumentTypeVOs();

            var periodFacilityDocumentDomain = reportingPeriod.AddPeriodFacilityDocument(1,1,"abc.xlsx",null,null,documentStatuses,documentType,facilityRequiredDocumentTypeVOs);

            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();
            var periodFacilityDocumentEntity = mapper.ConvertReportingPeriodFacilityDocumentDomainToEntity(periodFacilityDocumentDomain);

            Assert.NotNull(periodFacilityDocumentEntity);
            Assert.Equal(periodFacilityDocumentDomain.Id, periodFacilityDocumentEntity.Id);
            Assert.Equal(periodFacilityDocumentDomain.ReportingPeriodFacilityId, periodFacilityDocumentEntity.ReportingPeriodFacilityId);
            Assert.Equal(periodFacilityDocumentDomain.Version, periodFacilityDocumentEntity.Version);
            Assert.Equal(periodFacilityDocumentDomain.DisplayName, periodFacilityDocumentEntity.DisplayName);
            Assert.Equal(periodFacilityDocumentDomain.StoredName, periodFacilityDocumentEntity.StoredName);
            Assert.Equal(periodFacilityDocumentDomain.Path, periodFacilityDocumentEntity.Path);
            Assert.Equal(periodFacilityDocumentDomain.DocumentStatus.Id, periodFacilityDocumentEntity.DocumentStatusId);
            Assert.Equal(periodFacilityDocumentDomain.DocumentType.Id, periodFacilityDocumentEntity.DocumentTypeId);
            Assert.Equal(periodFacilityDocumentDomain.ValidationError, periodFacilityDocumentEntity.ValidationError);
        }

        [Fact]
        public void ConvertFacilityRequiredDocumentTypeEntityToValueObject()
        {
            var reportingType = GenerateReportingType().First();
            var supplyChainStage = GenerateSupplyChainStage().First();
            var documentType = GetDocumentTypes().First();
            var documentRequiredStatus = GetDocumentRequiredStatuses().First();

            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();
            var facilityRequiredDocumentTypeVo = mapper.ConvertFacilityRequiredDocumentTypeEntityToValueObject(reportingType, supplyChainStage, documentType, documentRequiredStatus);

            Assert.NotNull(facilityRequiredDocumentTypeVo);
            Assert.Equal(reportingType.Id, facilityRequiredDocumentTypeVo.ReportingType.Id);
            Assert.Equal(reportingType.Name, facilityRequiredDocumentTypeVo.ReportingType.Name);
            Assert.Equal(supplyChainStage.Id, facilityRequiredDocumentTypeVo.SupplyChainStage.Id);
            Assert.Equal(supplyChainStage.Name, facilityRequiredDocumentTypeVo.SupplyChainStage.Name);
            Assert.Equal(documentType.Id, facilityRequiredDocumentTypeVo.DocumentType.Id);
            Assert.Equal(documentType.Name, facilityRequiredDocumentTypeVo.DocumentType.Name);
            Assert.Equal(documentRequiredStatus.Id, facilityRequiredDocumentTypeVo.DocumentRequiredStatus.Id);
            Assert.Equal(documentRequiredStatus.Name, facilityRequiredDocumentTypeVo.DocumentRequiredStatus.Name);
        }

        #endregion
    }
}
