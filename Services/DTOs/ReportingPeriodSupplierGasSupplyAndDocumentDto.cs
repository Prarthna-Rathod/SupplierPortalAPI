using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class ReportingPeriodSupplierGasSupplyAndDocumentDto
    {
        public int PeriodSupplierId { get; set; }
        public IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownDto> ReportingPeriodFacilityGasSupplyBreakdowns { get; set; } = new List<ReportingPeriodFacilityGasSupplyBreakdownDto>();
        public IEnumerable<PeriodSupplierDocumentDto> PeriodSupplierDocuments { get; set; } = new List<PeriodSupplierDocumentDto>();

        public ReportingPeriodSupplierGasSupplyAndDocumentDto(int id, IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownDto> reportingPeriodFacilityGasSupplyBreakdowns, IEnumerable<PeriodSupplierDocumentDto> periodSupplierDocuments)
        {
            PeriodSupplierId = id;
            ReportingPeriodFacilityGasSupplyBreakdowns = reportingPeriodFacilityGasSupplyBreakdowns;
            PeriodSupplierDocuments = periodSupplierDocuments;
        }
    }
}
