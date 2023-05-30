using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var periodSuppliersDomain = reportingPeriod.AddRemovePeriodSupplier(createSupplierEntities()).First();
            ;

            var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();
            var periodSupplierDto = mapper.ConvertPeriodSupplierDomainToDto(periodSuppliersDomain, reportingPeriod.DisplayName);

            Assert.NotNull(periodSupplierDto);
            Assert.Equal(periodSuppliersDomain.Id, periodSupplierDto.Id);
            Assert.Equal(periodSuppliersDomain.Supplier.Id, periodSupplierDto.SupplierId);
            Assert.Equal(periodSuppliersDomain.SupplierReportingPeriodStatus.Id, periodSupplierDto.SupplierReportingPeriodStatusId);
            Assert.Equal(periodSuppliersDomain.InitialDataRequestDate, periodSupplierDto.InitialDataRequestDate);
            Assert.Equal(periodSuppliersDomain.ResendDataRequestDate, periodSupplierDto.ResendDataRequestDate);
        }

            [Fact]
            public void ConvertPeriodFacilityElectricityGridMixEntitiesToValueObjects()
            {
            var electricityGridMixComponent = GetElectricityGridMixComponents();
            var unitOfMeasure = GetUnitOfMeasures();
            var gridMixEntity = CreateReportingPeriodFacilityElecticityGridMixDto();


                var mapper = CreateInstanceOfReportingPeriodDomainDtoMapper();
                var gridMixVo = mapper.ConvertPeriodFacilityElectricityGridMixDtoToValueObject(gridMixEntity, electricityGridMixComponent,unitOfMeasure);

                Assert.NotNull(gridMixVo);
                //Assert.Equal(gridMixEntity.Count(), gridMixVo.Count());
            }





        }
    }

