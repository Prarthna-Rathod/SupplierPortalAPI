using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class ReportingPeriodFacilityDto
    {
        public int Id { get; set; }
        public IEnumerable<int> FacilityIds { get; set; }
        //public string FacilityName { get; set; }
        public int FacilityReportingPeriodDataStatusId { get; set; }
        public string FacilityReportingPeriodDataStatusName { get; set; }
        public int ReportingPeriodId { get; set; }
        public int ReportingPeriodSupplierId { get; set; }

        public bool FacilityIsRelevantForPeriod { get; set; }

        public ReportingPeriodFacilityDto(int id, IEnumerable<int> facilityIds, 
            //string facilityName, 
            int facilityReportingPeriodDataStatusId, string facilityReportingPeriodDataStatusName, int reportingPeriodId, int reportingPeriodSupplierId, bool facilityIsRelevantForPeriod)
        {
            Id = id;
            FacilityIds = facilityIds;
            //FacilityName = facilityName;
            FacilityReportingPeriodDataStatusId = facilityReportingPeriodDataStatusId;
            FacilityReportingPeriodDataStatusName = facilityReportingPeriodDataStatusName;
            ReportingPeriodId = reportingPeriodId;
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
            FacilityIsRelevantForPeriod = facilityIsRelevantForPeriod;
        }


    }
}
