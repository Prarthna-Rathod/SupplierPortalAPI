using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.ReadOnlyDTOs
{
    public class ReportingPeriodSupplierRelaventFacilityDto
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public int FacilityReportingPeriodDataStatusId { get; set; }
        public string FacilityReportingPeriodDataStatusName {get; set; }
        public int ReportingPeriodId { get; set; }
        public int ReportingPeriodSupplierId { get; set; }

        public ReportingPeriodSupplierRelaventFacilityDto(int id, int facilityId, string facilityName, int facilityReportingPeriodDataStatusId, string facilityReportingPeriodDataStatusName, int reportingPeriodId, int reportingPeriodSupplierId)
        {
            Id = id;
            FacilityId = facilityId;
            FacilityName = facilityName;
            FacilityReportingPeriodDataStatusId = facilityReportingPeriodDataStatusId;
            FacilityReportingPeriodDataStatusName = facilityReportingPeriodDataStatusName;
            ReportingPeriodId = reportingPeriodId;
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
        }
    }
}
