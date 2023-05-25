using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class ReportingPeriodSupplierGasSupplyBreakdownAndDocumentDto
    {
        public int ReportingPeriodSupplierId { get; set; }
        public int ReportingPeriodId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }

        public IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownDto> PeriodFacilityGasSupplyBreakdownDtos { get; set; }
        public IEnumerable<GetReportingPeriodSupplierDocumentDto > PeriodSupplierDocumentDtos { get; set; }

        public ReportingPeriodSupplierGasSupplyBreakdownAndDocumentDto(int reportingPeriodSupplierId, int reportingPeriodId, int supplierId, string supplierName, IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownDto> periodFacilityGasSupplyBreakdownDtos, IEnumerable<GetReportingPeriodSupplierDocumentDto> periodSupplierDocumentDtos)
        {
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
            ReportingPeriodId = reportingPeriodId;
            SupplierId = supplierId;
            SupplierName = supplierName;
            PeriodFacilityGasSupplyBreakdownDtos = periodFacilityGasSupplyBreakdownDtos;
            PeriodSupplierDocumentDtos = periodSupplierDocumentDtos;
        }
    }
}
