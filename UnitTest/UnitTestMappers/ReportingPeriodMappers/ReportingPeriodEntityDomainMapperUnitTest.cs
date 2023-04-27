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
            var periodSupplierDomain = reportingPeriod.AddPeriodSupplier(1, supplierVO, supplierReportingPeriodStatus,true,true,true);

            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();

            var periodSupplierEntity = mapper.ConvertReportingPeriodSupplierDomainToEntity(periodSupplierDomain);

            Assert.NotNull(periodSupplierEntity);
            Assert.Equal(periodSupplierDomain.Id, periodSupplierEntity.Id);
            Assert.Equal(periodSupplierDomain.Supplier.Id, periodSupplierEntity.SupplierId);
            Assert.Equal(periodSupplierDomain.ReportingPeriodId, periodSupplierEntity.ReportingPeriodId);
            Assert.Equal(periodSupplierDomain.SupplierReportingPeriodStatus.Id, periodSupplierEntity.SupplierReportingPeriodStatusId);
            Assert.Equal(periodSupplierDomain.IsActive, periodSupplierEntity.IsActive);
            Assert.Equal(periodSupplierDomain.ActiveForCurrentPeriod, periodSupplierEntity.ActiveForCurrentPeriod);
            Assert.Equal(periodSupplierDomain.InitialDataRequest, periodSupplierEntity.InitialDataRequest);
            Assert.Equal(periodSupplierDomain.ResendInitialDataRequest, periodSupplierEntity.ResendInitialDataRequest);
        }

        [Fact]
        public void ConertPeriodSupplierEntityToDomain()
        {
            var periodSupplierEntity = CreateReportingPeriodSupplierEntity();
            var supplierReportingPeriodStatuses = GetSupplierReportingPeriodStatuses();
            var supplierVO = GetAndConvertSupplierValueObject();
            var mapper = CreateInstanceOfReportingPeriodEntityDomainMapper();

            var periodSupplierDomain = mapper.ConvertPeriodSupplierEntityToDomain(periodSupplierEntity, supplierReportingPeriodStatuses, supplierVO);

            Assert.NotNull(periodSupplierDomain);
            Assert.Equal(periodSupplierEntity.Id, periodSupplierDomain.Id);
            Assert.Equal(periodSupplierEntity.SupplierId, periodSupplierDomain.Supplier.Id);
            Assert.Equal(periodSupplierEntity.ReportingPeriodId, periodSupplierDomain.ReportingPeriodId);
            Assert.Equal(periodSupplierEntity.SupplierReportingPeriodStatusId, periodSupplierDomain.SupplierReportingPeriodStatus.Id);
            Assert.Equal(periodSupplierEntity.ActiveForCurrentPeriod, periodSupplierDomain.ActiveForCurrentPeriod);
            Assert.Equal(periodSupplierEntity.InitialDataRequest, periodSupplierDomain.InitialDataRequest);
            Assert.Equal(periodSupplierEntity.ResendInitialDataRequest, periodSupplierDomain.ResendInitialDataRequest);
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
            
            var periodSupplierDomain = reportingPeriod.PeriodSuppliers.First();
            
            var facilityVO = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodDataStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
            var periodFacility = periodSupplierDomain.AddPeriodFacility(1, facilityVO, facilityReportingPeriodDataStatus, 1, true, true);
            
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
    }
}
