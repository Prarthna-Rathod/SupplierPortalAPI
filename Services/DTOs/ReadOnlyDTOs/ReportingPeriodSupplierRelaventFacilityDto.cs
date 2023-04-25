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
        //public int ReportingPeriodSupplierId { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string? GhgrpFacilityId { get; set; }
        public int ReportingTypeId { get; set; }
        public string ReportingTypeName { get; set; }
        public int SupplyChainStageId { get; set; }
        public string SupplyChainStageName { get; set; }
        public bool IsActive { get; set; }
        public int FacilityReportingPeriodDataStatusId { get; set; }
        public string FacilityReportingPeriodDataStatusName {get; set; }
        public bool FacilityIsRelevantForPeriod { get; set; }

        public ReportingPeriodSupplierRelaventFacilityDto(int id,
            //int reportingPeriodSupplierId,
            int facilityId, string facilityName, string? ghgrpFacilityId, int reportingTypeId, string reportingTypeName, int supplyChainStageId, string supplyChainStageName, bool isActive,
            int facilityReportingPeriodDataStatusId, string facilityReportingPeriodDataStatusName, bool facilityIsRelaventForPeriod )
        {
            Id = id;
            //ReportingPeriodSupplierId = reportingPeriodSupplierId;
            FacilityId = facilityId;
            FacilityName = facilityName;
            GhgrpFacilityId = ghgrpFacilityId;
            ReportingTypeId = reportingTypeId;
            ReportingTypeName = reportingTypeName;
            SupplyChainStageId = supplyChainStageId;
            SupplyChainStageName = supplyChainStageName;
            IsActive = isActive;
            FacilityReportingPeriodDataStatusId = facilityReportingPeriodDataStatusId;
            FacilityReportingPeriodDataStatusName = facilityReportingPeriodDataStatusName;
            FacilityIsRelevantForPeriod = facilityIsRelaventForPeriod;
        }
    }
}
