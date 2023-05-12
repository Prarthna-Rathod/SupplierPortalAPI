using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.ReadOnlyDTOs
{
    public class ReportingPeriodSupplierRelaventFacilityDto
    {
        public int? Id { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string? GhgrpFacilityId { get; set; }
        public int ReportingTypeId { get; set; }
        public string ReportingTypeName { get; set; }
        public int SupplyChainStageId { get; set; }
        public string SupplyChainStageName { get; set; }
        public bool IsActive { get; set; }
        public int? ReportingPeriodId { get; set; }
        public int? FacilityReportingPeriodDataStatusId { get; set; }
        public string? FacilityReportingPeriodDataStatusName { get; set; }
        public int? FercRegionId { get; set; }
        public string? FercRegionName { get; set; }
        public bool FacilityIsRelevantForPeriod { get; set; }

        public ReportingPeriodSupplierRelaventFacilityDto(int? id,
            int facilityId, string facilityName, string? ghgrpFacilityId, int reportingTypeId, string reportingTypeName, int supplyChainStageId, string supplyChainStageName, bool isActive, int? reportingPeriodId, 
            int? facilityReportingPeriodDataStatusId, string? facilityReportingPeriodDataStatusName, bool facilityIsRelaventForPeriod, int? fercRegionId, string? fercRegionName)
        {
            Id = id;
            FacilityId = facilityId;
            FacilityName = facilityName;
            GhgrpFacilityId = ghgrpFacilityId;
            ReportingTypeId = reportingTypeId;
            ReportingTypeName = reportingTypeName;
            SupplyChainStageId = supplyChainStageId;
            SupplyChainStageName = supplyChainStageName;
            IsActive = isActive;
            ReportingPeriodId = reportingPeriodId;
            FacilityReportingPeriodDataStatusId = facilityReportingPeriodDataStatusId;
            FacilityReportingPeriodDataStatusName = facilityReportingPeriodDataStatusName;
            FacilityIsRelevantForPeriod = facilityIsRelaventForPeriod;
            FercRegionId = fercRegionId;
            FercRegionName = fercRegionName;
        }
    }
}
