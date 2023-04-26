using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.ReadOnlyDTOs
{
    public class ReportingPeriodSupplierFacilitiesDto
    {
        public int ReportingPeriodSupplierId { get; set; }
        public int ReportingPeriodId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public IEnumerable<ReportingPeriodSupplierRelaventFacilityDto> ReportingPeriodFacilities { get; set; }

        public ReportingPeriodSupplierFacilitiesDto(int reportingPeriodSupplierId, int reportingPeriodId, int supplierId, string supplierName, IEnumerable<ReportingPeriodSupplierRelaventFacilityDto> reportingPeriodFacilities)
        {
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
            ReportingPeriodId = reportingPeriodId;
            SupplierId = supplierId;
            SupplierName = supplierName;
            ReportingPeriodFacilities = reportingPeriodFacilities;
        }
    }
}
