using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ValueConstants;

namespace UnitTest.UnitTestMappers.ReportingPeriodMappers
{
    public class ReportingPeriodDomainDtoMapperUnitTest : BasicTestClass
    {
        [Fact]
        public void ConvertReportingPeriodDomainToDto()
        {
            var reportingPeriod = GetReportingPeriodDomain();
            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();

            var reportingPeriodDto = mapper.ConvertReportingPeriodDomainToDto(reportingPeriod);

            Assert.NotNull(reportingPeriodDto);
            Assert.Equal(reportingPeriod.Id, reportingPeriodDto.Id);
            Assert.Equal(reportingPeriod.DisplayName, reportingPeriodDto.DisplayName);
            Assert.Equal(reportingPeriod.ReportingPeriodType.Id, reportingPeriodDto.ReportingPeriodTypeId);
            Assert.Equal(reportingPeriod.CollectionTimePeriod, reportingPeriodDto.CollectionTimePeriod);
            Assert.Equal(reportingPeriod.ReportingPeriodStatus.Id, reportingPeriodDto.ReportingPeriodStatusId);
            Assert.Equal(reportingPeriod.StartDate.Date, reportingPeriodDto.StartDate.Date);
            Assert.Equal(reportingPeriod.EndDate, reportingPeriodDto.EndDate);
            Assert.Equal(reportingPeriod.IsActive, reportingPeriodDto.IsActive);
        }

        [Fact]
        public void ConvertPeriodSupplierDomainToDto()
        {
            var reportingPeriod = GetReportingPeriodDomain();
            var supplierVo = GetAndConvertSupplierValueObject();
            var supplierReportingPeriodStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplierDomain = reportingPeriod.AddPeriodSupplier(1, supplierVo, supplierReportingPeriodStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();
            var periodSupplierDto = mapper.ConvertPeriodSupplierDomainToDto(periodSupplierDomain, reportingPeriod.DisplayName);

            Assert.NotNull(periodSupplierDto);
            Assert.Equal(periodSupplierDomain.Id, periodSupplierDto.Id);
            Assert.Equal(periodSupplierDomain.Supplier.Id, periodSupplierDto.SupplierId);
            Assert.Equal(periodSupplierDomain.Supplier.Name, periodSupplierDto.SupplierName);
            Assert.Equal(periodSupplierDomain.SupplierReportingPeriodStatus.Id, periodSupplierDto.SupplierReportingPeriodStatusId);
            Assert.Equal(periodSupplierDomain.SupplierReportingPeriodStatus.Name, periodSupplierDto.SupplierReportingPeriodStatusName);
            Assert.Equal(periodSupplierDomain.ReportingPeriodId, periodSupplierDto.ReportingPeriodId);
            Assert.Equal(periodSupplierDomain.InitialDataRequestDate, periodSupplierDto.InitialDataRequestDate);
            Assert.Equal(periodSupplierDomain.ResendDataRequestDate, periodSupplierDto.ResendDataRequestDate);

        }

        [Fact]
        public void ConvertPeriodFacilityDomainToDto()
        {
            var reportingPeriod = GetReportingPeriodDomain();
            //Add PeriodSupplier
            var supplierVo = GetAndConvertSupplierValueObject();
            var supplierReportingPeriodStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVo, supplierReportingPeriodStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));
            //Add PeriodFacility
            var facilityVo = GetAndConvertFacilityValueObject();

            var facilityReportingPeriodDataStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.CustomMix);

            var periodFacilityDomain = reportingPeriod.AddPeriodFacility(1, facilityVo, facilityReportingPeriodDataStatus, periodSupplier.Id, true, fercRegion, true);

            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();
            var periodFacilityDto = mapper.ConvertPeriodFacilityDomainToDto(periodFacilityDomain, true);

            Assert.NotNull(periodFacilityDto);
            Assert.Equal(periodFacilityDomain.Id, periodFacilityDto.Id);
            Assert.Equal(periodFacilityDomain.FacilityVO.Id, periodFacilityDto.FacilityId);
            Assert.Equal(periodFacilityDomain.FacilityVO.FacilityName, periodFacilityDto.FacilityName);
            Assert.Equal(periodFacilityDomain.FacilityVO.GHGRPFacilityId, periodFacilityDto.GhgrpFacilityId);
            Assert.Equal(periodFacilityDomain.FacilityVO.ReportingType.Id, periodFacilityDto.ReportingTypeId);
            Assert.Equal(periodFacilityDomain.FacilityVO.ReportingType.Name, periodFacilityDto.ReportingTypeName);
            Assert.Equal(periodFacilityDomain.FacilityVO.SupplyChainStage.Id, periodFacilityDto.SupplyChainStageId);
            Assert.Equal(periodFacilityDomain.FacilityVO.SupplyChainStage.Name, periodFacilityDto.SupplyChainStageName);
            Assert.Equal(periodFacilityDomain.IsActive, periodFacilityDto.IsActive);
            Assert.Equal(periodFacilityDomain.ReportingPeriodId, periodFacilityDto.ReportingPeriodId);
            Assert.Equal(periodFacilityDomain.FacilityReportingPeriodDataStatus.Id, periodFacilityDto.FacilityReportingPeriodDataStatusId);
            Assert.Equal(periodFacilityDomain.FacilityReportingPeriodDataStatus.Name, periodFacilityDto.FacilityReportingPeriodDataStatusName);
            Assert.Equal(periodFacilityDomain.FercRegion.Id, periodFacilityDto.FercRegionId);
            Assert.Equal(periodFacilityDomain.FercRegion.Name, periodFacilityDto.FercRegionName);

        }

        [Fact]
        public void ConvertPeriodElectricityGridMixDtosToValueObjects()
        {
            var electricityGridMixComponents = GetElectricityGridMixComponents();
            var gridMixDto = PeriodFacilityElectricityGridMixDto();

            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();
            var gridMixVo = mapper.ConvertPeriodElectricityGridMixDtosToValueObjects(gridMixDto, electricityGridMixComponents);

            Assert.NotNull(gridMixVo);
            Assert.Equal(gridMixDto.Count(), gridMixVo.Count());
        }

        [Fact]
        public void ConvertPeriodFacilityElectricityGridMixDomainListToDto()
        {
            var reportingPeriod = GetReportingPeriodDomain();

            //Add PeriodSupplier
            var supplierVo = GetAndConvertSupplierValueObject();
            var supplierReportingPeriodStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVo, supplierReportingPeriodStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            //Add PeriodFacility
            var facilityVo = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodDataStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.None);

            var periodFacilityDomain = reportingPeriod.AddPeriodFacility(1, facilityVo, facilityReportingPeriodDataStatus, periodSupplier.Id, true, fercRegion, true);

            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Open);
            reportingPeriod.ReportingPeriodStatus.Id = reportingPeriodStatus.Id;
            reportingPeriod.ReportingPeriodStatus.Name = reportingPeriodStatus.Name;

            var unitOfMeasure = GetUnitOfMeasures().First();
            var changedFercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.CustomMix);
            var electricityGridMixComponentPercents = GetElectricityGridMixComponentPercents();

            IEnumerable<PeriodFacilityElectricityGridMix> list = null;
            list = reportingPeriod.AddPeriodFacilityElectricityGridMix(periodFacilityDomain.Id, periodSupplier.Id, unitOfMeasure, changedFercRegion, electricityGridMixComponentPercents);

            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();
            var dto = mapper.ConvertPeriodFacilityElectricityGridMixDomainListToDto(list, periodFacilityDomain, periodSupplier);

            Assert.NotNull(dto);
        }

        [Fact]
        public void ConvertPeriodFacilityGasSupplyBreakDownDtoToValueObject()
        {
            var gasSupplyBreakdownDto = PeriodFacilityGasSupplyBreakdownDto();
            var site = GetSites().First();
            var unitOfMeasure = GetUnitOfMeasures().First();

            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();
            var gasSupplyBreakdownVo = mapper.ConvertPeriodFacilityGasSupplyBreakDownDtoToValueObject(gasSupplyBreakdownDto, site, unitOfMeasure);

            Assert.NotNull(gasSupplyBreakdownVo);
            Assert.Equal(gasSupplyBreakdownDto.PeriodFacilityId, gasSupplyBreakdownVo.PeriodFacilityId);
            Assert.Equal(gasSupplyBreakdownDto.FacilityId, gasSupplyBreakdownVo.FacilityId);
            Assert.Equal(gasSupplyBreakdownDto.SiteId, gasSupplyBreakdownVo.Site.Id);
            Assert.Equal(gasSupplyBreakdownDto.UnitOfMeasureId, gasSupplyBreakdownVo.UnitOfMeasure.Id);
            Assert.Equal(gasSupplyBreakdownDto.Content, gasSupplyBreakdownVo.Content);
        }

        [Fact]
        public void ConvertPeriodFacilityGasSupplyBreakdownDoaminListToDto()
        {
            var reportingPeriod = GetReportingPeriodDomain();

            //Add PeriodSupplier
            var supplierVo = GetAndConvertSupplierValueObject();
            var supplierReportingPeriodStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVo, supplierReportingPeriodStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            var periodFacilities = periodSupplier.PeriodFacilities;
            //Add PeriodFacility
            var facilityVo = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodDataStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.None);

            var periodFacilityDomain = reportingPeriod.AddPeriodFacility(1, facilityVo, facilityReportingPeriodDataStatus, periodSupplier.Id, true, fercRegion, true);

            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Open);
            reportingPeriod.ReportingPeriodStatus.Id = reportingPeriodStatus.Id;
            reportingPeriod.ReportingPeriodStatus.Name = reportingPeriodStatus.Name;

            var gasSupplyVos = GetGasSupplyBreakdowns();
            //Add gasSupplyBreakdown
            IEnumerable<PeriodFacilityGasSupplyBreakdown> list = null;
            list = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(periodSupplier.Id, gasSupplyVos);

            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();
            var dto = mapper.ConvertPeriodFacilityGasSupplyBreakdownDoaminListToDto(periodFacilities, periodSupplier);

            Assert.NotNull(dto);

        }

        [Fact]
        public void ConvertPeriodFacilityElectricityGridMixAndDocumentDomainListToDto()
        {
            var reportingPeriod = GetReportingPeriodDomain();

            //Add PeriodSupplier
            var supplierVo = GetAndConvertSupplierValueObject();
            var supplierReportingPeriodStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVo, supplierReportingPeriodStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            //Add PeriodFacility
            var facilityVo = GetAndConvertFacilityValueObject();
            var facilityReportingPeriodDataStatus = GetFacilityReportingPeriodDataStatus().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
            var fercRegion = GetFercRegions().First(x => x.Name == FercRegionValues.None);

            var periodFacilityDomain = reportingPeriod.AddPeriodFacility(1, facilityVo, facilityReportingPeriodDataStatus, periodSupplier.Id, true, fercRegion, true);

            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Name == ReportingPeriodStatusValues.Open);
            reportingPeriod.ReportingPeriodStatus.Id = reportingPeriodStatus.Id;
            reportingPeriod.ReportingPeriodStatus.Name = reportingPeriodStatus.Name;

            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();
            var dto = mapper.ConvertPeriodFacilityElectricityGridMixAndDocumentDomainListToDto( periodFacilityDomain, periodSupplier);

            Assert.NotNull(dto);
        }

        [Fact]
        public void ConvertPeriodSupplierGasSupplyAndDocumentDomainToDto()
        {
            var reportingPeriod = GetReportingPeriodDomain();

            //Add PeriodSupplier
            var supplierVo = GetAndConvertSupplierValueObject();
            var supplierReportingPeriodStatus = GetSupplierReportingPeriodStatuses().First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(1, supplierVo, supplierReportingPeriodStatus, new DateTime(2024, 02, 11), new DateTime(2024, 02, 11));

            var periodFacilities = periodSupplier.PeriodFacilities;

            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();
            var dto = mapper.ConvertPeriodSupplierGasSupplyAndDocumentDomainToDto(periodFacilities, periodSupplier);

            Assert.NotNull(dto);
        }
    }
}
