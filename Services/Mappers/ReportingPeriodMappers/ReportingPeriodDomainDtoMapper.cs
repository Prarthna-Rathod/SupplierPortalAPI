using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
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
                //var activeForCurrentPeriod = false;
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

        public IEnumerable<ReportingPeriodActiveSupplierVO> ConvertMultiplePeriodSupplierDtosToValueObject(IEnumerable<MultiplePeriodSuppliersDto> multiplePeriodSuppliersDtos, IEnumerable<SupplierVO> supplierVO, IEnumerable<SupplierReportingPeriodStatus> supplierReportingPeriodStatuses)
        {
            var reportingPeriodActiveSupplier = new List<ReportingPeriodActiveSupplierVO>();

            foreach (var periodSupplier in multiplePeriodSuppliersDtos)
            {
                var suppliers = supplierVO.FirstOrDefault(x => x.Id == periodSupplier.SupplierId);
                var status = supplierReportingPeriodStatuses.FirstOrDefault(x => x.Id == periodSupplier.SupplierReportingPeriodStatusId);

                var periodSupplierVO = new ReportingPeriodActiveSupplierVO(periodSupplier.Id, suppliers, periodSupplier.ReportingPeriodId, periodSupplier.ReportingPeriodName, status, periodSupplier.InitialDataRequestDate, periodSupplier.ResendDataRequestDate, periodSupplier.ActiveForCurrentPeriod);

                reportingPeriodActiveSupplier.Add(periodSupplierVO);

            }
            return reportingPeriodActiveSupplier;

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
                var periodId = periodFacilities.Select(x => x.ReportingPeriodId).FirstOrDefault();
                var periodFacility = periodFacilities.FirstOrDefault(x => x.ReportingPeriodId == periodId);
                isRelaventForPeriodStatus = false;
                list.Add(ConvertFacilityEntityToDto(facility, isRelaventForPeriodStatus, periodFacility));
            }

            return list;
        }

        public ReportingPeriodSupplierRelaventFacilityDto ConvertPeriodFacilityDomainToDto(PeriodFacility periodFacility, bool isRelaventForPeriodStatus)
        {
            var periodFacilityDto = new ReportingPeriodSupplierRelaventFacilityDto(periodFacility.Id, periodFacility.FacilityVO.Id, periodFacility.FacilityVO.FacilityName, periodFacility.FacilityVO.GHGRPFacilityId, periodFacility.FacilityVO.ReportingType.Id, periodFacility.FacilityVO.ReportingType.Name, periodFacility.FacilityVO.SupplyChainStage.Id, periodFacility.FacilityVO.SupplyChainStage.Name, periodFacility.FacilityVO.IsActive, periodFacility.ReportingPeriodId, periodFacility.FacilityReportingPeriodDataStatus?.Id, periodFacility.FacilityReportingPeriodDataStatus?.Name, isRelaventForPeriodStatus);

            return periodFacilityDto;
        }

        private ReportingPeriodSupplierRelaventFacilityDto ConvertFacilityEntityToDto(FacilityEntity facilityEntity, bool isRelaventForPeriodStatus, PeriodFacility periodFacility)
        {
            var periodFacilityDto = new ReportingPeriodSupplierRelaventFacilityDto(periodFacility.ReportingPeriodSupplierId, facilityEntity.Id, facilityEntity.Name, facilityEntity.GhgrpfacilityId, facilityEntity.ReportingTypeId, facilityEntity.ReportingType.Name, facilityEntity.SupplyChainStageId, facilityEntity.SupplyChainStage.Name, facilityEntity.IsActive, periodFacility.ReportingPeriodId, null, null, isRelaventForPeriodStatus);

            return periodFacilityDto;
        }

        public ReportingPeriodSupplierFacilitiesDto ConvertReportingPeriodSupplierFacilitiesDomainToDto(PeriodSupplier periodSupplier, IEnumerable<ReportingPeriodSupplierRelaventFacilityDto> periodFacilitiesDtos)
        {
            var periodSupplierFacilitiesDto = new ReportingPeriodSupplierFacilitiesDto(periodSupplier.Id, periodSupplier.ReportingPeriodId, periodSupplier.Supplier.Id, periodSupplier.Supplier.Name, periodFacilitiesDtos);

            return periodSupplierFacilitiesDto;
        }

        public IEnumerable<ReportingPeriodRelevantFacilityVO> ConvertPeriodFacilityDtoToValueObject(ReportingPeriodFacilityDto reportingPeriodFacilityDto, IEnumerable<FacilityVO> facilityVOs, IEnumerable<FacilityReportingPeriodDataStatus> facilityReportingPeriodDataStatus, PeriodSupplier periodSupplier)
        {
            var reportingPeriodRelevantFacilityVO = new List<ReportingPeriodRelevantFacilityVO>();

            foreach (var facility in reportingPeriodFacilityDto.reportingPeriodSupplierRelaventFacilityDtos)
            {
                var facilities = facilityVOs.FirstOrDefault(x => x.Id == facility.FacilityId);


                var status = facilityReportingPeriodDataStatus.FirstOrDefault();

                reportingPeriodRelevantFacilityVO.Add(ConvertPeriodFacilityDtoToVo(reportingPeriodFacilityDto.reportingPeriodSupplierRelaventFacilityDtos.FirstOrDefault(), status, periodSupplier, facilities, reportingPeriodFacilityDto.ReportingPeriodId));
            }


            return reportingPeriodRelevantFacilityVO;
        }

        private ReportingPeriodRelevantFacilityVO ConvertPeriodFacilityDtoToVo(ReportingPeriodSupplierRelaventFacilityDto reportingPeriodSupplierRelaventFacilityDtos, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatuses, PeriodSupplier periodSupplier, FacilityVO facilityVO, int id)
        {

            var facilityValueObject = new ReportingPeriodRelevantFacilityVO(reportingPeriodSupplierRelaventFacilityDtos.Id, id, facilityVO, periodSupplier.Supplier.Id, periodSupplier.Supplier.Name, periodSupplier.Id, reportingPeriodSupplierRelaventFacilityDtos.FacilityIsRelevantForPeriod, facilityReportingPeriodDataStatuses.Id);



            return facilityValueObject;
        }





        #endregion

        #region PeriodFacilityElectricityGridMix

        public IEnumerable<ReportingPeriodFacilityElectricityGridMixVO> ConvertPeriodFacilityElectricityGridMixDtoToValueObject(AddMultiplePeriodFacilityElectricityGridMixDto addMultiplePeriodFacilityElectricityGridMixDtos, IEnumerable<ElectricityGridMixComponent> electricityGridMixComponent, IEnumerable<UnitOfMeasure> unitOfMeasure)
        {

            var reportingPeriodFacilityElectricityGridMixVO = new List<ReportingPeriodFacilityElectricityGridMixVO>();

            foreach (var electricityGridMix in addMultiplePeriodFacilityElectricityGridMixDtos.ReportingPeriodFacilityElectricityGridMixDtos)
            {
                var gridMixComponent = electricityGridMixComponent.FirstOrDefault(x => x.Id == electricityGridMix.ElectricityGridMixComponentId);
                var measure = unitOfMeasure.FirstOrDefault(x => x.Id == electricityGridMix.UnitOfMeasureId);

                reportingPeriodFacilityElectricityGridMixVO.Add(ConvertPeriodFacilityElectricityGridMixDtoToVO(electricityGridMix, gridMixComponent, measure));
            }

            return reportingPeriodFacilityElectricityGridMixVO;
        }

        protected ReportingPeriodFacilityElectricityGridMixVO ConvertPeriodFacilityElectricityGridMixDtoToVO(ReportingPeriodFacilityElectricityGridMixDto reportingPeriodFacilityElectricityGridMixDto, ElectricityGridMixComponent electricityGridMixComponent, UnitOfMeasure unitOfMeasure)
        {
            var electricityGridMixVO = new ReportingPeriodFacilityElectricityGridMixVO();

            electricityGridMixVO.Id = reportingPeriodFacilityElectricityGridMixDto.Id;
            electricityGridMixVO.UnitOfMeasure = unitOfMeasure;
            electricityGridMixVO.ElectricityGridMixComponent = electricityGridMixComponent;
            electricityGridMixVO.Content = reportingPeriodFacilityElectricityGridMixDto.Content;
            electricityGridMixVO.IsActive = reportingPeriodFacilityElectricityGridMixDto.IsActive;

            return electricityGridMixVO;
        }


        IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> ConvertElectricityGridMixes(IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> reportingPeriods, IEnumerable<UnitOfMeasure> unitOfMeasures, IEnumerable<ElectricityGridMixComponent> electricityGridMixComponents)
        {
            var dtos = new List<ReportingPeriodFacilityElectricityGridMixDto>();

            foreach (var period in reportingPeriods)
            {
                var component = electricityGridMixComponents.FirstOrDefault(x => x.Id == period.ElectricityGridMixComponentId);
                var unit = unitOfMeasures.FirstOrDefault(x => x.Id == period.UnitOfMeasureId);

                dtos.Add(ConvertElectricityGridMix(period, unit, component));
            }
            return dtos;
        }

        ReportingPeriodFacilityElectricityGridMixDto ConvertElectricityGridMix(ReportingPeriodFacilityElectricityGridMixEntity gridMixEntity, UnitOfMeasure unitOfMeasure, ElectricityGridMixComponent electricityGridMixComponent)
        {
            var dto = new ReportingPeriodFacilityElectricityGridMixDto();

            dto.Id = gridMixEntity.Id;
            dto.Content = gridMixEntity.Content;
            dto.IsActive = gridMixEntity.IsActive;
            dto.ElectricityGridMixComponentName = electricityGridMixComponent.Name;
            dto.ElectricityGridMixComponentId = electricityGridMixComponent.Id;
            dto.UnitOfMeasureName = unitOfMeasure.Name;
            dto.UnitOfMeasureId = unitOfMeasure.Id;

            return dto;
        }


        public AddMultiplePeriodFacilityElectricityGridMixDto ConvertReportingPeriodFacilityElectricityGridMixEntityToDto(ReportingPeriodFacilityEntity reportingPeriodFacilityEntity, IEnumerable<UnitOfMeasure> unitOfMeasures, IEnumerable<ElectricityGridMixComponent> electricityGridMixComponents)
        {
            var dto = new AddMultiplePeriodFacilityElectricityGridMixDto();

            dto.ReportingPeriodId = reportingPeriodFacilityEntity.ReportingPeriodSupplier.ReportingPeriodId;
            dto.ReportingPeriodFacilityId = reportingPeriodFacilityEntity.Id;
            dto.ReportingPeriodSupplierId = reportingPeriodFacilityEntity.ReportingPeriodSupplierId;
            dto.FercRegionId = reportingPeriodFacilityEntity.FercRegionId;
            dto.ReportingPeriodFacilityElectricityGridMixDtos = ConvertElectricityGridMixes(reportingPeriodFacilityEntity.ReportingPeriodFacilityElectricityGridMixEntities, unitOfMeasures, electricityGridMixComponents);

            return dto;
        }
        #endregion

        #region PeriodFacilityGasSupplyBreakdown

            public IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownVO> ConvertPeriodFacilityGasSupplyBreakDownDtoToValueObject(IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownDto> periodFacilityGasSupplyBreakDownDto, IEnumerable<Site> sites, IEnumerable<UnitOfMeasure> unitOfMeasures)
            {
                var reportingPeriodFacilityGasSupplyBreakDownVO = new List<ReportingPeriodFacilityGasSupplyBreakDownVO>();

                foreach (var gasSupplyBreakDown in periodFacilityGasSupplyBreakDownDto)
                {
                    var site = sites.FirstOrDefault(x => x.Id == gasSupplyBreakDown.SiteId);
                    var unitOfMeasure = unitOfMeasures.FirstOrDefault(x => x.Id == gasSupplyBreakDown.UnitOfMeasureId);

                    reportingPeriodFacilityGasSupplyBreakDownVO.Add(ConvertPeriodFacilityGasSupplyBreakDown(gasSupplyBreakDown, site, unitOfMeasure));
                }

                return reportingPeriodFacilityGasSupplyBreakDownVO;
            }

            protected ReportingPeriodFacilityGasSupplyBreakDownVO ConvertPeriodFacilityGasSupplyBreakDown(ReportingPeriodFacilityGasSupplyBreakdownDto gasSupplyBreakDown, Site site, UnitOfMeasure unitOfMeasure)
            {
                return new ReportingPeriodFacilityGasSupplyBreakDownVO(0,gasSupplyBreakDown.ReportingPeriodFacilityId,unitOfMeasure,site,gasSupplyBreakDown.Content);
            }

        public MultiplePeriodFacilityGasSupplyBreakDownDto ConvertReportingPeriodGasSupplyBreakDownDomainListToDto(PeriodSupplier periodSupplier, IEnumerable<PeriodFacility> periodFacilities)
        {
            var domainlist = new List<PeriodFacilityGasSupplyBreakDown>();
            var dtos = new List<ReportingPeriodFacilityGasSupplyBreakdownDto>();

            foreach(var periodFacility in periodFacilities)
            {
                var gasSupplyBreakdown = periodFacility.periodFacilityGasSupplyBreakDowns;

                //if (gasSupplyBreakdown != null) throw new Exception("GasSupplyBreakdown not found..!");
                domainlist.AddRange(gasSupplyBreakdown);

                dtos.AddRange(ConvertPeriodFacilityGasSupplyDomainToDto(gasSupplyBreakdown, periodFacility));
            }

            var periodFacilityBreakdownDto = new MultiplePeriodFacilityGasSupplyBreakDownDto(periodSupplier.Id, periodSupplier.ReportingPeriodId, periodSupplier.Supplier.Id, periodSupplier.Supplier.Name, dtos);

           return periodFacilityBreakdownDto;
        }
        private IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownDto> ConvertPeriodFacilityGasSupplyDomainToDto(IEnumerable<PeriodFacilityGasSupplyBreakDown> facilityGasSupplyBreakdowns, PeriodFacility periodFacility)
        {
            var gasSupplyDto = new List<ReportingPeriodFacilityGasSupplyBreakdownDto>();
            foreach (var gasSupplyBreakdown in facilityGasSupplyBreakdowns)
            {
                var gasSupplyBreakdownDto = new ReportingPeriodFacilityGasSupplyBreakdownDto(gasSupplyBreakdown.PeriodFacilityId, periodFacility.FacilityVO.Id, periodFacility.FacilityVO.FacilityName, gasSupplyBreakdown.Site.Id, gasSupplyBreakdown.Site.Name, gasSupplyBreakdown.UnitOfMeasure.Id, gasSupplyBreakdown.UnitOfMeasure.Name, gasSupplyBreakdown.Content);
                gasSupplyDto.Add(gasSupplyBreakdownDto);
            }
            return gasSupplyDto;
        }

        #endregion

        #region PeriodDocument
        public ReportingPeriodFacilityGridMixAndDocumentDto ConvertPeriodFacilityElectricityGridMixAndDocumentDomainListToDto(PeriodFacility periodFacility, PeriodSupplier periodSupplier)
        {
            var gridMixDtos = new List<ReportingPeriodFacilityElectricityGridMixDto>();


            foreach (var electricityGridMix in periodFacility.periodFacilityElectricityGridMixes)
            {
                var electricityGridMixDto = new ReportingPeriodFacilityElectricityGridMixDto( electricityGridMix.ElectricityGridMixComponent.Id, electricityGridMix.ElectricityGridMixComponent.Name,
                   electricityGridMix.Content);
                gridMixDtos.Add(electricityGridMixDto);
            }

            UnitOfMeasure unitOfMeasure = new UnitOfMeasure();
            if (periodFacility.periodFacilityElectricityGridMixes.Count() == 0)
            {
                unitOfMeasure.Id = 0;
                unitOfMeasure.Name = "";
            }
            else
                unitOfMeasure = periodFacility.periodFacilityElectricityGridMixes.First().UnitOfMeasure;

            var facilityDocumentDtos = new List<PeriodFacilityDocumentDto>();
         
            foreach (var document in periodFacility.periodFacilityDocuments)
            {
                var facilityDocumentDto = new PeriodFacilityDocumentDto(document.Id, document.Version, document.DisplayName, document.DocumentStatus.Id, document.DocumentStatus.Name, document.DocumentType.Id, document.DocumentType.Name, document.ValidationError);
                facilityDocumentDtos.Add(facilityDocumentDto);
            }


            var facilityGridMixAndDocumentDto = new ReportingPeriodFacilityGridMixAndDocumentDto(periodFacility.Id, periodFacility.ReportingPeriodId, periodSupplier.Supplier.Id, unitOfMeasure.Id, unitOfMeasure.Name, periodFacility.FercRegion.Id, periodFacility.FercRegion.Name, gridMixDtos, facilityDocumentDtos);
            return facilityGridMixAndDocumentDto;
        }

        public ReportingPeriodSupplierGasSupplyAndDocumentDto ConvertPeriodSupplierGasSupplyAndDocumentDomainListToDto(IEnumerable<PeriodFacility> periodFacilities, PeriodSupplier periodSupplier)
        {
            var gasSupplyBreakdownDomainList = new List<PeriodFacilityGasSupplyBreakDown>();
            var gasSupplyBreakdowns = new List<ReportingPeriodFacilityGasSupplyBreakdownDto>();

            foreach (var periodFacility in periodFacilities)
            {
                var gasSupplyBreakdownList = periodFacility.periodFacilityGasSupplyBreakDowns;
                gasSupplyBreakdownDomainList.AddRange(gasSupplyBreakdownList);

                gasSupplyBreakdowns.AddRange(ConvertPeriodFacilityGasSupplyDomainToDto(gasSupplyBreakdownList, periodFacility));

            }

            var periodSupplierDocumentDtos = new List<PeriodSupplierDocumentDto>();
            foreach (var document in periodSupplier.PeriodSupplierDocuments)
            {
                var periodSupplierDocumentDto = new PeriodSupplierDocumentDto(document.Id, document.Version, document.DisplayName, document.DocumentStatus.Id, document.DocumentStatus.Name, document.DocumentType.Id, document.DocumentType.Name, document.ValidationError);
                periodSupplierDocumentDtos.Add(periodSupplierDocumentDto);
            }

            var periodFacilityGasSupplyAndDocumentDto = new ReportingPeriodSupplierGasSupplyAndDocumentDto(periodSupplier.Id, gasSupplyBreakdowns, periodSupplierDocumentDtos);

            return periodFacilityGasSupplyAndDocumentDto;
        }



        #endregion

    }
}
