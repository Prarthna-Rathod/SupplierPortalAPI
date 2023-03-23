using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.ReadOnlyDTOs
{
    public class ReportingPeriodRelevantFacilityDTO
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }
        public string Facility { get; set; }
        public int FacilityReportingPeriodDataStatusId { get; set; }
        public string FacilityReportingPeriodDataStatus {get; set; }
        public int ReportingTypeId { get; set; }
        public string ReportingType { get; set; }
        public int ReportingPeriodSupplierId { get; set; }
        public string ReportingPeriodSupplier { get;set; }

        public ReportingPeriodRelevantFacilityDTO(int id, int facilityId, string facility, int facilityReportingPeriodDataStatusId, string facilityReportingPeriodDataStatus, int reportingTypeId, string reportingType, int reportingPeriodSupplierId, string reportingPeriodSupplier)
        {
            Id = id;
            FacilityId = facilityId;
            Facility = facility;
            FacilityReportingPeriodDataStatusId = facilityReportingPeriodDataStatusId;
            FacilityReportingPeriodDataStatus = facilityReportingPeriodDataStatus;
            ReportingTypeId = reportingTypeId;
            ReportingType = reportingType;
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
            ReportingPeriodSupplier = reportingPeriodSupplier;
        }
    }
}
