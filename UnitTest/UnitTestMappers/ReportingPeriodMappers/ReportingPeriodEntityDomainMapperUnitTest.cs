using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.UnitTestMappers.ReportingPeriodMappers
{
    public class ReportingPeriodEntityDomainMapperUnitTest: BasicTestClass
    {
        #region ReportingPeriod

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
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            var periodFacility = reportingPeriod.PeriodSuppliers.First().PeriodFacilities.First();
            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();

            var periodFacilityEntity = mapper.ConvertReportingPeriodFacilityDomainToEntity(periodFacility);

            Assert.NotNull(periodFacilityEntity);
            Assert.Equal(periodFacility.Id, periodFacilityEntity.Id);
            Assert.Equal(periodFacility.FacilityVO.Id, periodFacilityEntity.FacilityId);
            Assert.Equal(periodFacility.FacilityReportingPeriodDataStatus.Id, periodFacilityEntity.FacilityReportingPeriodDataStatusId);
            Assert.Equal(periodFacility.ReportingPeriodId, periodFacilityEntity.ReportingPeriodId);
            Assert.Equal(periodFacility.ReportingPeriodSupplierId, periodFacilityEntity.ReportingPeriodSupplierId);
            Assert.Equal(periodFacility.IsActive, periodFacilityEntity.IsActive);
            Assert.Equal(periodFacility.FercRegion.Id, periodFacilityEntity.FercRegionId);

        }
        
        #endregion

        #region PeriodFacility ElectricityGridMixes

        [Fact]
        public void ConvertPeriodFacilityElectricityGridMixDomainToEntity()
        {
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.Custom_Mix);
            var gridMixComponentPercents = GetElectricityGridMixComponentPercents();
            var list = reportingPeriod.AddRemoveElectricityGridMixComponents(1, 1, unitOfMeasure, fercRegion, gridMixComponentPercents);
            var singleDomain = list.FirstOrDefault();
            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();

            //Act
            var gridMixEntity = mapper.ConvertPeriodFacilityElectricityGridMixDomainToEntity(singleDomain);

            //Assert
            Assert.NotNull(gridMixEntity);
            Assert.Equal(singleDomain.Id, gridMixEntity.Id);
            Assert.Equal(singleDomain.PeriodFacilityId, gridMixEntity.ReportingPeriodFacilityId);
            Assert.Equal(singleDomain.ElectricityGridMixComponent.Id, gridMixEntity.ElectricityGridMixComponentId);
            Assert.Equal(singleDomain.UnitOfMeasure.Id, gridMixEntity.UnitOfMeasureId);
            Assert.Equal(singleDomain.Content, gridMixEntity.Content);
        }
        
        [Fact]
        public void ConvertPeriodFacilityElectricityGridMixEntityToValueObject()
        {
            var entities = CreatePeriodFacilityElectricityGridMixEntities();
            var electricityGridMixComponent = GetElectricityGridMixComponents();
            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();

            var voList = mapper.ConvertPeriodFacilityElectricityGridMixEntityListToValueObjectList(entities, electricityGridMixComponent);

            Assert.NotNull(voList);
            Assert.Equal(entities.Count(), voList.Count());

            for (int i = 0; i < entities.Count(); i++)
            {
                var entity = entities.ToList()[i].ElectricityGridMixComponentId;
                var vo = voList.ToList()[i].ElectricityGridMixComponent.Id;
                Assert.Equal(entity, vo);
            }
        }

        #endregion

        #region PeriodSupplierFacility GasSupplyBreakdown
        [Fact]
        public void ConvertPeriodFacilityGasSupplyBreakdownDomainToEntity()
        {
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();

            var gasSupplyBreakdownVOs = GetGasSupplyBreakdownVOs();
            var list = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(1, gasSupplyBreakdownVOs);
            var facilityGasSupplyDomain = list.FirstOrDefault();
            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();

            //Act
            var periodFacilityGasSupplyBreakdownEntity = mapper.ConvertPeriodFacilityGasSupplyBreakdownDomainToEntity(facilityGasSupplyDomain);

            //Assert
            Assert.NotNull(periodFacilityGasSupplyBreakdownEntity);
            Assert.Equal(facilityGasSupplyDomain.Id, periodFacilityGasSupplyBreakdownEntity.Id);
            Assert.Equal(facilityGasSupplyDomain.PeriodFacilityId, periodFacilityGasSupplyBreakdownEntity.PeriodFacilityId);
            Assert.Equal(facilityGasSupplyDomain.Site.Id, periodFacilityGasSupplyBreakdownEntity.SiteId);
            Assert.Equal(facilityGasSupplyDomain.UnitOfMeasure.Id, periodFacilityGasSupplyBreakdownEntity.UnitOfMeasureId);
            Assert.Equal(facilityGasSupplyDomain.Content, periodFacilityGasSupplyBreakdownEntity.Content);

        }
        
        [Fact]
        public void ConvertPeriodFacilityGasSupplyBreakdownEntityToValueObject()
        {
            var entity = CreatePeriodFacilityGasSupplyBreakdownEntity();
            var site = GetSites().FirstOrDefault(x => x.Id == entity.SiteId);
            var uom = GetUnitOfMeasures().FirstOrDefault(x => x.Id == entity.UnitOfMeasureId);
            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();

            var vo = mapper.ConvertPeriodFacilityGasSupplyBreakdownEntityToValueObject(entity, site, uom);

            Assert.NotNull(vo);
            Assert.Equal(entity.Id, vo.Id);
            Assert.Equal(entity.PeriodFacilityId, vo.PeriodFacilityId);
            Assert.Equal(entity.SiteId, vo.Site.Id);
            Assert.Equal(entity.UnitOfMeasureId, vo.UnitOfMeasure.Id);
            Assert.Equal(entity.Content, vo.Content);

        }

        #endregion

    }
}
