using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using DataAccess.Entities;
using Services.DTOs;
using Services.DTOs.ReadOnlyDTOs;
using Services.Mappers.Interfaces;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;

namespace Services.Mappers.ReportingPeriodMappers
{
    public class ReportingPeriodDomainDtoMapper : IReportingPeriodDomainDtoMapper
    {
        #region ReportingPeriod

        public ReportingPeriodDto ConvertReportingPeriodDomainToDto(ReportingPeriod reportingPeriod)
        {
            var reportingPeriodSuppliers = new List<ReportingPeriodSupplierDto>();

            return new ReportingPeriodDto(reportingPeriod.Id, reportingPeriod.DisplayName, reportingPeriod.ReportingPeriodType.Id, reportingPeriod.ReportingPeriodType.Name, reportingPeriod.CollectionTimePeriod, reportingPeriod.ReportingPeriodStatus.Id, reportingPeriod.ReportingPeriodStatus.Name, reportingPeriod.StartDate, reportingPeriod.EndDate, reportingPeriod.IsActive, reportingPeriodSuppliers);
        }

        public IEnumerable<ReportingPeriodDto> ConvertReportingPeriodDomainListToDtos(IEnumerable<ReportingPeriod> reportingPeriods)
        {
            var list = new List<ReportingPeriodDto>();
            foreach (var reportingPeriod in reportingPeriods)
            {
                list.Add(ConvertReportingPeriodDomainToDto(reportingPeriod));
            }
            return list;
        }


        #endregion

        #region PeriodSupplier

        public IEnumerable<ReportingPeriodSupplierDto> ConvertPeriodSupplierDomainListToDtos(IEnumerable<PeriodSupplier> periodSuppliersDomain, ReportingPeriod reportingPeriod)
        {
            var list = new List<ReportingPeriodSupplierDto>();
            foreach (var periodSupplier in periodSuppliersDomain)
            {
                list.Add(ConvertPeriodSupplierDomainToDto(periodSupplier, reportingPeriod.DisplayName));
            }
            return list;
        }

        public ReportingPeriodSupplierDto ConvertPeriodSupplierDomainToDto(PeriodSupplier periodSuppliersDomain, string displayName)
        {
            var dto = new ReportingPeriodSupplierDto(periodSuppliersDomain.Id, periodSuppliersDomain.Supplier.Id, periodSuppliersDomain.Supplier.Name, periodSuppliersDomain.ReportingPeriodId, displayName, periodSuppliersDomain.SupplierReportingPeriodStatus.Id, periodSuppliersDomain.SupplierReportingPeriodStatus.Name, periodSuppliersDomain.InitialDataRequestDate, periodSuppliersDomain.ResendDataRequestDate);

            return dto;
        }

        public IEnumerable<ReportingPeriodRelevantSupplierDto> ConvertReleventPeriodSupplierDomainToDto(IEnumerable<PeriodSupplier> periodSupplierDomainList, IEnumerable<SupplierEntity> inRelevantSupplierList, ReportingPeriod reportingPeriod)
        {
            var periodSuppliersDtos = new List<ReportingPeriodRelevantSupplierDto>();


            foreach (var periodSupplier in periodSupplierDomainList)
            {
                var activeForCurrentPeriod = true;
                var periodSuppliers = new ReportingPeriodRelevantSupplierDto(periodSupplier.Id, periodSupplier.Supplier.Id, periodSupplier.Supplier.Name, periodSupplier.ReportingPeriodId, periodSupplier.SupplierReportingPeriodStatus.Id, periodSupplier.SupplierReportingPeriodStatus.Name, activeForCurrentPeriod, periodSupplier.InitialDataRequestDate, periodSupplier.ResendDataRequestDate);
                periodSuppliersDtos.Add(periodSuppliers);

            }

            foreach (var supplier in inRelevantSupplierList)
            {
                var dto = ConvertSupplierEntityToDto(supplier);
                periodSuppliersDtos.Add(dto);
            }
            return periodSuppliersDtos;
        }

        private ReportingPeriodRelevantSupplierDto ConvertSupplierEntityToDto(SupplierEntity supplierEntity)
        {
            var activeForCurrentPeriod = false;
            return new ReportingPeriodRelevantSupplierDto(null, supplierEntity.Id, supplierEntity.Name, null, null, null, activeForCurrentPeriod, null, null);
        }

        public IEnumerable<GasSupplyBreakdownVO> ConvertPeriodSupplierGasSupplyBreakdownDtosToValueObjectList(IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownDto> gasSupplyBreakDownDtos, IEnumerable<Site> sites, IEnumerable<UnitOfMeasure> unitOfMeasures)
        {
            var voList = new List<GasSupplyBreakdownVO>();

            foreach (var dto in gasSupplyBreakDownDtos)
            {
                var site = sites.FirstOrDefault(x => x.Id == dto.SiteId);
                var unitOfMeasure = unitOfMeasures.FirstOrDefault(x => x.Id == dto.UnitOfMeasureId);

                if (site == null || unitOfMeasure == null)
                    throw new NotFoundException("Site or UnitOfMeasure is not found !!");

                voList.Add(ConvertPeriodSupplierGasSupplyBreakdownDtoToValueObject(dto, site, unitOfMeasure));
            }

            return voList;
        }

        public GasSupplyBreakdownVO ConvertPeriodSupplierGasSupplyBreakdownDtoToValueObject(ReportingPeriodFacilityGasSupplyBreakdownDto gasSupplyBreakDownDto, Site site, UnitOfMeasure unitOfMeasure)
        {
            return new GasSupplyBreakdownVO(0, gasSupplyBreakDownDto.ReportingPeriodFacilityId, gasSupplyBreakDownDto.FacilityId, site, unitOfMeasure, gasSupplyBreakDownDto.Content);
        }

        public MultiplePeriodFacilityGasSupplyBreakdownDto GetAndConvertPeriodFacilityGasSupplyBreakdownDomainListToDto(IEnumerable<PeriodFacility> periodFacilityList, PeriodSupplier periodSupplier)
        {
            var gasSupplyDtos = new List<ReportingPeriodFacilityGasSupplyBreakdownDto>();
            foreach (var facility in periodFacilityList)
            {
                var gasSupplyBreakdowns = facility.periodFacilityGasSupplyBreakdowns;
                if (gasSupplyBreakdowns.Count() != 0)
                    gasSupplyDtos.AddRange(ConvertPeriodFacilityGasSupplyBreakdownDomainListToDtos(gasSupplyBreakdowns, facility));
            }

            if (gasSupplyDtos.Count() == 0)
                throw new NotFoundException("GasSupplyBreakdown are not available in this Supplier !!");

            var periodSupplierFacilityGasSupplyBreakdownDto = new MultiplePeriodFacilityGasSupplyBreakdownDto(periodSupplier.Id, periodSupplier.ReportingPeriodId, periodSupplier.Supplier.Id, periodSupplier.Supplier.Name, gasSupplyDtos);
            return periodSupplierFacilityGasSupplyBreakdownDto;
        }

        public IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownDto> ConvertPeriodFacilityGasSupplyBreakdownDomainListToDtos(IEnumerable<PeriodFacilityGasSupplyBreakdown> gasSupplyBreakdowns, PeriodFacility periodFacility)
        {
            var dtoList = new List<ReportingPeriodFacilityGasSupplyBreakdownDto>();

            foreach (var item in gasSupplyBreakdowns)
            {
                var dto = new ReportingPeriodFacilityGasSupplyBreakdownDto(item.PeriodFacilityId, periodFacility.FacilityVO.Id, periodFacility.FacilityVO.FacilityName, item.Site.Id, item.Site.Name, item.UnitOfMeasure.Id, item.UnitOfMeasure.Name, item.Content);

                dtoList.Add(dto);
            }

            return dtoList;
        }

        #endregion

        #region PeriodFacility

        public IEnumerable<ReportingPeriodSupplierRelaventFacilityDto> ConvertPeriodFacilityDomainListToDtos(IEnumerable<PeriodFacility> periodFacilities, IEnumerable<FacilityEntity> facilityEntities)
        {
            var list = new List<ReportingPeriodSupplierRelaventFacilityDto>();
            bool isRelaventForPeriodStatus = true;
            foreach (var periodFacility in periodFacilities)
            {
                list.Add(ConvertPeriodFacilityDomainToDto(periodFacility, isRelaventForPeriodStatus));
            }

            foreach (var facility in facilityEntities)
            {
                isRelaventForPeriodStatus = false;
                list.Add(ConvertFacilityEntityToDto(facility, isRelaventForPeriodStatus));
            }

            return list;
        }

        public ReportingPeriodSupplierRelaventFacilityDto ConvertPeriodFacilityDomainToDto(PeriodFacility periodFacility, bool isRelaventForPeriodStatus)
        {
            var periodFacilityDto = new ReportingPeriodSupplierRelaventFacilityDto(periodFacility.Id, periodFacility.FacilityVO.Id, periodFacility.FacilityVO.FacilityName, periodFacility.FacilityVO.GHGRPFacilityId, periodFacility.FacilityVO.ReportingType.Id, periodFacility.FacilityVO.ReportingType.Name, periodFacility.FacilityVO.SupplyChainStage.Id, periodFacility.FacilityVO.SupplyChainStage.Name, periodFacility.FacilityVO.IsActive, periodFacility.ReportingPeriodId, periodFacility.FacilityReportingPeriodDataStatus?.Id, periodFacility.FacilityReportingPeriodDataStatus?.Name,
              isRelaventForPeriodStatus, periodFacility.FercRegion?.Id, periodFacility.FercRegion?.Name);

            return periodFacilityDto;
        }

        private ReportingPeriodSupplierRelaventFacilityDto ConvertFacilityEntityToDto(FacilityEntity facilityEntity, bool isRelaventForPeriodStatus)
        {
            var periodFacilityDto = new ReportingPeriodSupplierRelaventFacilityDto(null, facilityEntity.Id, facilityEntity.Name, facilityEntity.GhgrpfacilityId, facilityEntity.ReportingTypeId, facilityEntity.ReportingType.Name, facilityEntity.SupplyChainStageId, facilityEntity.SupplyChainStage.Name, facilityEntity.IsActive, null, null, null, isRelaventForPeriodStatus, null, null);

            return periodFacilityDto;
        }

        public ReportingPeriodSupplierFacilitiesDto ConvertReportingPeriodSupplierFacilityDomainToDto(PeriodSupplier periodSupplier, IEnumerable<ReportingPeriodSupplierRelaventFacilityDto> periodFacilitiesDtos)
        {
            var periodSupplierFacilitiesDto = new ReportingPeriodSupplierFacilitiesDto(periodSupplier.Id, periodSupplier.ReportingPeriodId, periodSupplier.Supplier.Id, periodSupplier.Supplier.Name, periodFacilitiesDtos);

            return periodSupplierFacilitiesDto;
        }

        public IEnumerable<ElectricityGridMixComponentPercent> ConvertPeriodFacilityElectricityGridMixDtosToValueObjectList(IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> gridMixDtos, IEnumerable<ElectricityGridMixComponent> electricityGridMixes)
        {
            var list = new List<ElectricityGridMixComponentPercent>();

            foreach (var gridMixDto in gridMixDtos)
            {
                var gridMixComponent = electricityGridMixes.FirstOrDefault(x => x.Id == gridMixDto.ElectricityGridMixComponentId);

                if (gridMixComponent is null)
                    throw new NotFoundException("ElectricityGridMix component not found !!");

                list.Add(ConvertPeriodFacilityElectricityGridMixDtoToValueObject(gridMixComponent, gridMixDto.Content));
            }

            return list;
        }

        public ElectricityGridMixComponentPercent ConvertPeriodFacilityElectricityGridMixDtoToValueObject(ElectricityGridMixComponent electricityGridMix, decimal content)
        {
            return new ElectricityGridMixComponentPercent(0, electricityGridMix, content);
        }

        public MultiplePeriodFacilityElectricityGridMixDto GetAndConvertPeriodFacilityElectricityGridMixDomainListToDto(PeriodFacility periodFacility, int supplierId)
        {
            var periodFacilityGridMixList = periodFacility.periodFacilityElectricityGridMixes;

            var gridMixDtos = new List<ReportingPeriodFacilityElectricityGridMixDto>();
            foreach (var gridMix in periodFacilityGridMixList)
            {
                var gridMixDto = new ReportingPeriodFacilityElectricityGridMixDto(gridMix.ElectricityGridMixComponent.Id, gridMix.ElectricityGridMixComponent.Name, gridMix.Content);

                gridMixDtos.Add(gridMixDto);
            }
            var unitOfMeasure = periodFacilityGridMixList.First().UnitOfMeasure;

            var periodFacilityGridMixDto = new MultiplePeriodFacilityElectricityGridMixDto(periodFacility.Id, periodFacility.ReportingPeriodId, supplierId, unitOfMeasure.Id, unitOfMeasure.Name, periodFacility.FercRegion.Id, periodFacility.FercRegion.Name, gridMixDtos);
            return periodFacilityGridMixDto;
        }

        public ReportingPeriodFacilityElectricityGridMixAndDocumentDto GetAndConvertPeriodFacilityElectricityGridMixAndDocumentsDomainListToDto(PeriodFacility periodFacility, int supplierId)
        {
            //First load electricityGridMixes
            var periodFacilityGridMixList = periodFacility.periodFacilityElectricityGridMixes;

            var gridMixDtos = new List<ReportingPeriodFacilityElectricityGridMixDto>();
            foreach (var gridMix in periodFacilityGridMixList)
            {
                var gridMixDto = new ReportingPeriodFacilityElectricityGridMixDto(gridMix.ElectricityGridMixComponent.Id, gridMix.ElectricityGridMixComponent.Name, gridMix.Content);

                gridMixDtos.Add(gridMixDto);
            }
            var unitOfMeasure = periodFacilityGridMixList.First().UnitOfMeasure;

            //Load PeriodFacilityDocuments
            var periodFacilityDocuments = periodFacility.periodFacilityDocuments;
            var documentDtos = new List<GetReportingPeriodFacilityDocumentDto>();
            foreach(var document in periodFacilityDocuments)
            {
                var documentDto = new GetReportingPeriodFacilityDocumentDto(document.Id, document.DisplayName, document.Version, document.DocumentStatus.Id, document.DocumentStatus.Name, document.DocumentType.Id, document.DocumentType.Name, document.ValidationError);
                
                documentDtos.Add(documentDto);
            }

            var gridMixAndDocumentDto = new ReportingPeriodFacilityElectricityGridMixAndDocumentDto(periodFacility.Id, periodFacility.ReportingPeriodId, supplierId, unitOfMeasure.Id, unitOfMeasure.Name, periodFacility.FercRegion.Id, periodFacility.FercRegion.Name, gridMixDtos, documentDtos);

            return gridMixAndDocumentDto;
        }

        #endregion

        #region PeriodDocument
        #endregion






    }
}
