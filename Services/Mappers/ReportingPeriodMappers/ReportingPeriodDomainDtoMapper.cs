using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.SupplierRoot.DomainModels;
using Services.DTOs;
using Services.DTOs.ReadOnlyDTOs;
using Services.Mappers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var dto = new ReportingPeriodSupplierDto(periodSuppliersDomain.Id, periodSuppliersDomain.Supplier.Id, periodSuppliersDomain.Supplier.Name, periodSuppliersDomain.ReportingPeriodId, displayName, periodSuppliersDomain.SupplierReportingPeriodStatus.Id, periodSuppliersDomain.SupplierReportingPeriodStatus.Name);

            return dto;
        }

        //public IEnumerable<ReportingPeriodSupplierDto> ConvertRelevantSuppliersDomainToDtos(IEnumerable<Supplier> suppliersDomain)
        //{
          
        //}

        #endregion

        #region PeriodFacility
        #endregion

        #region PeriodDocument
        #endregion


    }
}
