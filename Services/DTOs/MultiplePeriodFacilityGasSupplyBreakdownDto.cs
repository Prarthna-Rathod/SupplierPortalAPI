using BusinessLogic.ReportingPeriodRoot.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class MultiplePeriodFacilityGasSupplyBreakdownDto
    {
        public int ReportingPeriodSupplierId { get; set; }
        public int ReporingPeriodId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownDto> PeriodFacilityGasSupplyBreakdowns { get; set; }

        public MultiplePeriodFacilityGasSupplyBreakdownDto(int reportingPeriodSupplierId, int reporingPeriodId, int supplierId, string supplierName, IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownDto> periodFacilityGasSupplyBreakdowns)
        {
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
            ReporingPeriodId = reporingPeriodId;
            SupplierId = supplierId;
            SupplierName = supplierName;
            PeriodFacilityGasSupplyBreakdowns = periodFacilityGasSupplyBreakdowns;
        }
    }
}