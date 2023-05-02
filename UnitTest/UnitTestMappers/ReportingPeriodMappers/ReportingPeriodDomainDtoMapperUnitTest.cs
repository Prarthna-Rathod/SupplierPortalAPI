using BusinessLogic.ValueConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.UnitTestMappers.ReportingPeriodMappers
{
    public class ReportingPeriodDomainDtoMapperUnitTest: BasicTestClass
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

            var mapper= CreateInstanceOfReportingPeriodDomainDtoMapper();
            var periodSupplierDto = mapper.ConvertPeriodSupplierDomainToDto(periodSupplierDomain,reportingPeriod.DisplayName);

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
            var periodFacilityDomain = reportingPeriod.AddPeriodFacility(1,facilityVo,facilityReportingPeriodDataStatus,periodSupplier.Id,true,true);

            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();
            var periodFacilityDto = mapper.ConvertPeriodFacilityDomainToDto(periodFacilityDomain, true);

            Assert.NotNull(periodFacilityDto);
            Assert.Equal(periodFacilityDomain.Id, periodFacilityDto.Id);
            Assert.Equal(periodFacilityDomain.FacilityVO.Id,periodFacilityDto.FacilityId);
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

        }
    }
}
