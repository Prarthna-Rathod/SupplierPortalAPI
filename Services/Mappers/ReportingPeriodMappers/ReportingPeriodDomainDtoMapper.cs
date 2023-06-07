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
            var periodFacilityDto = new ReportingPeriodSupplierRelaventFacilityDto(periodFacility.Id, periodFacility.FacilityVO.Id, periodFacility.FacilityVO.FacilityName, periodFacility.FacilityVO.GHGRPFacilityId, periodFacility.FacilityVO.ReportingType.Id, periodFacility.FacilityVO.ReportingType.Name, periodFacility.FacilityVO.SupplyChainStage.Id, periodFacility.FacilityVO.SupplyChainStage.Name, periodFacility.FacilityVO.IsActive, periodFacility.ReportingPeriodId, periodFacility.FacilityReportingPeriodDataStatus?.Id, periodFacility.FacilityReportingPeriodDataStatus?.Name,periodFacility.FercRegion?.Id,periodFacility.FercRegion?.Name, isRelaventForPeriodStatus);

            return periodFacilityDto;
        }

        private ReportingPeriodSupplierRelaventFacilityDto ConvertFacilityEntityToDto(FacilityEntity facilityEntity, bool isRelaventForPeriodStatus)
        {
            var periodFacilityDto = new ReportingPeriodSupplierRelaventFacilityDto(null, facilityEntity.Id, facilityEntity.Name, facilityEntity.GhgrpfacilityId, facilityEntity.ReportingTypeId, facilityEntity.ReportingType.Name, facilityEntity.SupplyChainStageId, facilityEntity.SupplyChainStage.Name, facilityEntity.IsActive, null, null, null,null,null, isRelaventForPeriodStatus);

            return periodFacilityDto;
        }

        public ReportingPeriodSupplierFacilitiesDto ConvertReportingPeriodSupplierFacilitiesDomainToDto(PeriodSupplier periodSupplier, IEnumerable<ReportingPeriodSupplierRelaventFacilityDto> periodFacilitiesDtos)
        {
            var periodSupplierFacilitiesDto = new ReportingPeriodSupplierFacilitiesDto(periodSupplier.Id, periodSupplier.ReportingPeriodId, periodSupplier.Supplier.Id, periodSupplier.Supplier.Name, periodFacilitiesDtos);

            return periodSupplierFacilitiesDto;
        }

        #endregion

        #region PeriodFacilityElectricityGridMix

        public IEnumerable<ElectricityGridMixComponentPercent> ConvertPeriodElectricityGridMixDtosToValueObjects(IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> reportingPeriodFacilityElectricityGridMixDtos, IEnumerable<ElectricityGridMixComponent> electricityGridMixComponents)
        {
            var list = new List<ElectricityGridMixComponentPercent>();
            foreach (var electricityGridMix in reportingPeriodFacilityElectricityGridMixDtos)
            {
                var electricityGridMixComponent = electricityGridMixComponents.FirstOrDefault(x => x.Id == electricityGridMix.ElectricityGridMixComponentId);

                list.Add(ConvertPeriodElectricityGridMixDtoToValueObject(electricityGridMixComponent, electricityGridMix.Content));
            }
            return list;
        }

        public ElectricityGridMixComponentPercent ConvertPeriodElectricityGridMixDtoToValueObject(ElectricityGridMixComponent electricityGridMixComponent, decimal content)
        {
            var gridMixDomain = new ElectricityGridMixComponentPercent(0, electricityGridMixComponent, content);
            return gridMixDomain;
        }

        public MultiplePeriodFacilityElectricityGridMixDto ConvertPeriodFacilityElectricityGridMixDomainListToDto(IEnumerable<PeriodFacilityElectricityGridMix> periodFacilityElectricityGridMixes,PeriodFacility periodFacility,PeriodSupplier periodSupplier)
        {
            var gridMixDtos = new List<ReportingPeriodFacilityElectricityGridMixDto>();

            foreach (var electricityGridMix in periodFacilityElectricityGridMixes)
            {
                var electricityGridMixDto = new ReportingPeriodFacilityElectricityGridMixDto(electricityGridMix.ElectricityGridMixComponent.Id,electricityGridMix.ElectricityGridMixComponent.Name,electricityGridMix.Content);
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

            var periodFacilityElectricityGridMixDto = new MultiplePeriodFacilityElectricityGridMixDto(periodFacility.Id, periodSupplier.ReportingPeriodId, periodSupplier.Supplier.Id, unitOfMeasure.Id, unitOfMeasure.Name,periodFacility.FercRegion.Id,periodFacility.FercRegion.Name, gridMixDtos);
            return periodFacilityElectricityGridMixDto;

        }

        #endregion

        #region PeriodFacilityGasSupplyBreakdown

        public IEnumerable<GasSupplyBreakdownVO> ConvertPeriodFacilityGasSupplyBreakDownDtosToValueObjects(IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownDto> periodFacilityGasSupplyBreakdownDtos, IEnumerable<Site> sites,IEnumerable<UnitOfMeasure> unitOfMeasures)
        {
            var list = new List<GasSupplyBreakdownVO>();
            foreach(var dto in periodFacilityGasSupplyBreakdownDtos)
            {
                var site = sites.FirstOrDefault(x => x.Id == dto.SiteId);
                var unitOfMeasure = unitOfMeasures.FirstOrDefault(x => x.Id == dto.UnitOfMeasureId);

                if (site is null || unitOfMeasure is null)
                    throw new NotFoundException("Site or UnitOfMeasure is not found !!");

                list.Add(ConvertPeriodFacilityGasSupplyBreakDownDtoToValueObject(dto,site,unitOfMeasure));
            }
            return list;
        }

        public GasSupplyBreakdownVO ConvertPeriodFacilityGasSupplyBreakDownDtoToValueObject(ReportingPeriodFacilityGasSupplyBreakdownDto periodFacilityGasSupplyBreakdownDto,Site site,UnitOfMeasure unitOfMeasure)
        {
            return new GasSupplyBreakdownVO(0, periodFacilityGasSupplyBreakdownDto.PeriodFacilityId, periodFacilityGasSupplyBreakdownDto.FacilityId, site,unitOfMeasure, periodFacilityGasSupplyBreakdownDto.Content);
        }

        public MultiplePeriodFacilityGasSupplyBreakdownDto ConvertPeriodFacilityGasSupplyBreakdownDoaminListToDto(IEnumerable<PeriodFacility> periodFacilities, PeriodSupplier periodSupplier)
        {
            var gasSupplyDomainList = new List<PeriodFacilityGasSupplyBreakdown>();
            var gasSupplyDtos = new List<ReportingPeriodFacilityGasSupplyBreakdownDto>();

            foreach(var periodFacility in periodFacilities)
            {
                var gasSupplyList = periodFacility.PeriodFacilityGasSupplyBreakdowns;
                gasSupplyDomainList.AddRange(gasSupplyList);

                gasSupplyDtos.AddRange(ConvertPeriodFacilityGasSupplyDomainToDto(gasSupplyList, periodFacility));

            }

            var periodFacilityGasSupplyBreakdownDto = new MultiplePeriodFacilityGasSupplyBreakdownDto(periodSupplier.Id, periodSupplier.ReportingPeriodId, periodSupplier.Supplier.Id, periodSupplier.Supplier.Name, gasSupplyDtos);
            return periodFacilityGasSupplyBreakdownDto;
        }

        private IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownDto> ConvertPeriodFacilityGasSupplyDomainToDto(IEnumerable<PeriodFacilityGasSupplyBreakdown> facilityGasSupplyBreakdowns, PeriodFacility periodFacility)
        {
            var gasSupplyDto = new List<ReportingPeriodFacilityGasSupplyBreakdownDto>();
            foreach(var gasSupplyBreakdown in facilityGasSupplyBreakdowns)
            {
                var gasSupplyBreakdownDto = new ReportingPeriodFacilityGasSupplyBreakdownDto(gasSupplyBreakdown.PeriodFacilityId,periodFacility.FacilityVO.Id,periodFacility.FacilityVO.FacilityName,gasSupplyBreakdown.Site.Id,gasSupplyBreakdown.Site.Name,gasSupplyBreakdown.UnitOfMeasure.Id,gasSupplyBreakdown.UnitOfMeasure.Name,gasSupplyBreakdown.Content);
                gasSupplyDto.Add(gasSupplyBreakdownDto);
            }
            return gasSupplyDto;
        }

        #endregion

        #region PeriodFacilityDocument

        public ReportingPeriodFacilityGridMixAndDocumentDto ConvertPeriodFacilityElectricityGridMixAndDocumentDomainListToDto(PeriodFacility periodFacility, PeriodSupplier periodSupplier)
        {
            var gridMixDtos = new List<ReportingPeriodFacilityElectricityGridMixDto>();

            /*if (periodFacility.periodFacilityElectricityGridMixes.Count() == 0)
                throw new Exception("PeriodFacilityElectricityGridMix is not connetcted !!");*/

            foreach (var electricityGridMix in periodFacility.periodFacilityElectricityGridMixes)
            {
                var electricityGridMixDto = new ReportingPeriodFacilityElectricityGridMixDto(electricityGridMix.ElectricityGridMixComponent.Id, electricityGridMix.ElectricityGridMixComponent.Name, electricityGridMix.Content);
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
            /*if(periodFacility.PeriodFacilityDocuments.Count() == 0)
                throw new Exception("PeriodFacilityDocum`ent is not found !!");*/
            foreach(var document in periodFacility.PeriodFacilityDocuments)
            {
                var facilityDocumentDto = new PeriodFacilityDocumentDto(document.Id, document.Version, document.DisplayName, document.DocumentStatus.Id, document.DocumentStatus.Name, document.DocumentType.Id, document.DocumentType.Name, document.ValidationError);
                facilityDocumentDtos.Add(facilityDocumentDto);
            }

            
            var facilityGridMixAndDocumentDto = new ReportingPeriodFacilityGridMixAndDocumentDto(periodFacility.Id,periodFacility.ReportingPeriodId,periodSupplier.Supplier.Id,unitOfMeasure.Id,unitOfMeasure.Name,periodFacility.FercRegion.Id,periodFacility.FercRegion.Name,gridMixDtos,facilityDocumentDtos);
            return facilityGridMixAndDocumentDto;
        }

        #endregion

        #region PeriodSupplierDocument

        public ReportingPeriodSupplierGasSupplyAndDocumentDto ConvertPeriodSupplierGasSupplyAndDocumentDomainToDto(IEnumerable<PeriodFacility> periodFacilities, PeriodSupplier periodSupplier)
        {
            var gasSupplyDomainList = new List<PeriodFacilityGasSupplyBreakdown>();
            var gasSupplyDtos = new List<ReportingPeriodFacilityGasSupplyBreakdownDto>();

            foreach (var periodFacility in periodFacilities)
            {
                var gasSupplyList = periodFacility.PeriodFacilityGasSupplyBreakdowns;
                gasSupplyDomainList.AddRange(gasSupplyList);

                gasSupplyDtos.AddRange(ConvertPeriodFacilityGasSupplyDomainToDto(gasSupplyList, periodFacility));

            }

            var periodSupplierDocumentDtos = new List<PeriodSupplierDocumentDto>();
            foreach(var periodSupplierDocument in periodSupplier.PeriodSupplierDocuments)
            {
                var periodSupplierDocumentDto = new PeriodSupplierDocumentDto(periodSupplierDocument.Id,periodSupplierDocument.Version,periodSupplierDocument.DisplayName,periodSupplierDocument.DocumentStatus.Id,periodSupplierDocument.DocumentStatus.Name,periodSupplierDocument.DocumentType.Id,periodSupplierDocument.DocumentType.Name,periodSupplierDocument.ValidationError);
                periodSupplierDocumentDtos.Add(periodSupplierDocumentDto);
            }

            var periodFacilityGasSupplyAndDocumentDto = new ReportingPeriodSupplierGasSupplyAndDocumentDto(periodSupplier.Id, periodSupplier.ReportingPeriodId, periodSupplier.Supplier.Id, periodSupplier.Supplier.Name, gasSupplyDtos,periodSupplierDocumentDtos);

            return periodFacilityGasSupplyAndDocumentDto;
        }

        #endregion

    }
}
