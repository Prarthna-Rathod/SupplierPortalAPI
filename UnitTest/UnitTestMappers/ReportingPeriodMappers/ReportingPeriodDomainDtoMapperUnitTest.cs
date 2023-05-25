using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ValueConstants;

namespace UnitTest.UnitTestMappers.ReportingPeriodMappers
{
    public class ReportingPeriodDomainDtoMapperUnitTest : BasicTestClass
    {
        #region ReportingPeriod

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

        #endregion

        #region PeriodSupplier
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

        #endregion

        #region PeriodFacility
        [Fact]
        public void ConvertPeriodFacilityDomainToDto()
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
            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();

            //Act
            var periodFacilityDto = mapper.ConvertPeriodFacilityDomainToDto(periodFacility, true);

            //Assert
            Assert.NotNull(periodFacilityDto);
            Assert.Equal(periodFacility.Id, periodFacilityDto.Id);
            Assert.Equal(periodFacility.FacilityVO.Id, periodFacilityDto.FacilityId);
            Assert.Equal(periodFacility.FacilityVO.FacilityName, periodFacilityDto.FacilityName);
            Assert.Equal(periodFacility.FacilityVO.GHGRPFacilityId, periodFacilityDto.GhgrpFacilityId);
            Assert.Equal(periodFacility.FacilityVO.ReportingType.Id, periodFacilityDto.ReportingTypeId);
            Assert.Equal(periodFacility.FacilityVO.ReportingType.Name, periodFacilityDto.ReportingTypeName);
            Assert.Equal(periodFacility.FacilityVO.SupplyChainStage.Id, periodFacilityDto.SupplyChainStageId);
            Assert.Equal(periodFacility.FacilityVO.SupplyChainStage.Name, periodFacilityDto.SupplyChainStageName);
            Assert.Equal(periodFacility.IsActive, periodFacilityDto.IsActive);
            Assert.Equal(periodFacility.ReportingPeriodId, periodFacilityDto.ReportingPeriodId);
            Assert.Equal(periodFacility.FacilityReportingPeriodDataStatus.Id, periodFacilityDto.FacilityReportingPeriodDataStatusId);
            Assert.Equal(periodFacility.FacilityReportingPeriodDataStatus.Name, periodFacilityDto.FacilityReportingPeriodDataStatusName);
            Assert.Equal(periodFacility.FercRegion.Id, periodFacilityDto.FercRegionId);
            Assert.Equal(periodFacility.FercRegion.Name, periodFacilityDto.FercRegionName);

        }

        #endregion

        #region PeriodFacility ElectricityGridMixes
        [Fact]
        public void ConvertPeriodFacilityElectricityGridMixDtosToValueObjectList()
        {
            var dtos = PeriodFacilityElectricityGridMixDtos();
            var gridMixComponents = GetElectricityGridMixComponents();
            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();

            var voList = mapper.ConvertPeriodFacilityElectricityGridMixDtosToValueObjectList(dtos, gridMixComponents);

            Assert.NotNull(voList);
            Assert.Equal(dtos.Count(), voList.Count());

            for (int i = 0; i < dtos.Count(); i++)
            {
                var entityComponent = dtos.ToList()[i].ElectricityGridMixComponentId;
                var addedComponent = voList.ToList()[i].ElectricityGridMixComponent.Id;
                Assert.Equal(entityComponent, addedComponent);
            }
        }

        [Fact]
        public void ConvertElectricityGridMixesDomainListToDto()
        {
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == 1);
            var periodFacility = periodSupplier.PeriodFacilities.FirstOrDefault(x => x.Id == 1);

            //Add new data for gridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.Custom_Mix);
            var gridMixComponentPercents = GetElectricityGridMixComponentPercents();
            var domainList = reportingPeriod.AddRemoveElectricityGridMixComponents(periodSupplier.Id, periodFacility.Id, unitOfMeasure, fercRegion, gridMixComponentPercents);

            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();
            var dto = mapper.GetAndConvertPeriodFacilityElectricityGridMixDomainListToDto(periodFacility, periodSupplier.Supplier.Id);

            Assert.NotNull(dto);
            Assert.Equal(domainList.Count(), dto.ReportingPeriodFacilityElectricityGridMixDtos.Count());
        }

        #endregion

        #region PeriodSupplierFacility GasSupplyBreakdown
       
        [Fact]
        public void ConvertPeriodFacilityGasSupplyBreakdownDtoToValueObject()
        {
            var dto = CreatePeriodFacilityGasSupplyBreakdownDto();
            var site = GetSites().FirstOrDefault(x => x.Id == dto.SiteId);
            var uom = GetUnitOfMeasures().FirstOrDefault(x => x.Id == dto.UnitOfMeasureId);
            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();

            //Act
            var vo = mapper.ConvertPeriodSupplierGasSupplyBreakdownDtoToValueObject(dto, site, uom);

            //Assert
            Assert.NotNull(vo);
            Assert.Equal(dto.ReportingPeriodFacilityId, vo.PeriodFacilityId);
            Assert.Equal(dto.FacilityId, vo.FacilityId);
            Assert.Equal(dto.SiteId, vo.Site.Id);
            Assert.Equal(dto.UnitOfMeasureId, vo.UnitOfMeasure.Id);
            Assert.Equal(dto.Content, vo.Content);
        }

        [Fact]
        public void GetAndConvertPeriodFacilityGasSupplyBreakdownDomainListToDto()
        {
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            var periodSupplier = reportingPeriod.PeriodSuppliers.First();
            var periodFacilities = periodSupplier.PeriodFacilities;

            //First add new data for GasSupplyBreakdown
            var gasSupplyBreakdownVos = GetGasSupplyBreakdownVOs();
            reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(periodSupplier.Id, gasSupplyBreakdownVos);
            var facilities = new List<PeriodFacility>();

            foreach (var periodFacility in periodFacilities)
            {
                var gasSupplyData = periodFacility.periodFacilityGasSupplyBreakdowns;
                if (gasSupplyData.Count() != 0)
                    facilities.Add(periodFacility);
            }

            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();
            var dto = mapper.GetAndConvertPeriodFacilityGasSupplyBreakdownDomainListToDto(facilities, periodSupplier);

            Assert.NotNull(dto);

        }

        #endregion

        #region PeriodDocument

        [Fact]
        public void GetAndConvertPeriodFacilityElectricityGridMixAndDocumentsDomainListToDto()
        {
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == 1);
            var periodFacility = periodSupplier.PeriodFacilities.FirstOrDefault(x => x.Id == 1);

            //Add new data for gridMix
            var unitOfMeasure = GetUnitOfMeasures().FirstOrDefault(x => x.Id == 1);
            var fercRegion = GetFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.Custom_Mix);
            var gridMixComponentPercents = GetElectricityGridMixComponentPercents();
            var domainList = reportingPeriod.AddRemoveElectricityGridMixComponents(periodSupplier.Id, periodFacility.Id, unitOfMeasure, fercRegion, gridMixComponentPercents);

            //Add new data for document
            var facilityRequiredDocumentTypeVos = GetFacilityRequiredDocumentTypeVOs();
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.SubpartC);
            var displayName = "filename.xlsx";
            var documentStatusProcessing = documentStatuses.First(x => x.Name == DocumentStatusValues.Processing);

            var facilityDocument = reportingPeriod.AddUpdatePeriodFacilityDocuments(1, 1, displayName, null, documentStatuses, documentType, null, facilityRequiredDocumentTypeVos);

            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();
            var dto = mapper.GetAndConvertPeriodFacilityElectricityGridMixAndDocumentsDomainListToDto(periodFacility, periodSupplier.Supplier.Id);

            Assert.NotNull(dto);
            Assert.Equal(domainList.Count(), dto.ReportingPeriodFacilityElectricityGridMixDtos.Count());
           
        }

        #endregion

        #region PeriodSupplierDocument

        [Fact]
        public void GetAndConvertPeriodSupplierGasSupplyBreakdownAndDocumentsDomainListToDto()
        {
            var reportingPeriod = AddPeriodSupplierAndPeriodFacilityForPeriod();
            var periodSupplier = reportingPeriod.PeriodSuppliers.First();
            var periodFacilities = periodSupplier.PeriodFacilities;

            //First add new data for GasSupplyBreakdown
            var gasSupplyBreakdownVos = GetGasSupplyBreakdownVOs();
            reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(periodSupplier.Id, gasSupplyBreakdownVos);
            var facilities = new List<PeriodFacility>();

            foreach (var periodFacility in periodFacilities)
            {
                var gasSupplyData = periodFacility.periodFacilityGasSupplyBreakdowns;
                if (gasSupplyData.Count() != 0)
                    facilities.Add(periodFacility);
            }

            //Add data for SupplierDocument
            var documentStatuses = GetDocumentStatuses();
            var documentType = GetDocumentTypes().First(x => x.Name == DocumentTypeValues.Supplemental);
            UpdateReportingPeriodClosed(reportingPeriod);
            var displayName = "filename.xlsx";

            var supplierDocument = reportingPeriod.AddUpdatePeriodSupplierDocument(1, displayName, null, documentStatuses, documentType, null);

            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();

            var dto = mapper.GetAndConvertPeriodSupplierGasSupplyBreakdownAndDocumentsDomainListToDto(periodSupplier);

            Assert.NotNull(dto);
            Assert.Equal(periodSupplier.PeriodSupplierDocuments.Count(), dto.PeriodSupplierDocumentDtos.Count());

            }

        #endregion

    }
}
