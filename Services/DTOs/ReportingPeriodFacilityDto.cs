using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class ReportingPeriodFacilityDto
    {
        public ReportingPeriodFacilityDto(int id,int facilityId,string facilityName,int facilityReportingPeriodDataStatusId,string facilityReportingPeriodDataStatus,int reportingTypeId,string reportingType,int reportingPeriodSupplierId)
        {
            Id = id;
            FacilityId = facilityId;
            FacilityName = facilityName;
            FacilityReportingPeriodDataStatusId = facilityReportingPeriodDataStatusId;
            FacilityReportingPeriodDataStatus = facilityReportingPeriodDataStatus;
            ReportingTypeId = reportingTypeId;
            ReportingType = reportingType;
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
        }

        public int Id { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public int FacilityReportingPeriodDataStatusId { get; set; }
        public string FacilityReportingPeriodDataStatus { get; set; }
        public int ReportingTypeId { get; set; }
        public string ReportingType { get; set; }
        public int ReportingPeriodSupplierId { get; set; }
    }
}
