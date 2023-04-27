namespace Services.DTOs
{
    public class AddMultiplePeriodFacilityElectricityGridMixDto
    {
        public int ReportingPeriodFacilityId { get; set; }
        public int ReportingPeriodId { get; set; }
        public int SupplierId { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string? GHGRPFacilityId { get; set; }
        public IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> ReportingPeriodFacilityElectricityGridMixDtos { get; set; }
        public bool IsActive { get; set; }

        public AddMultiplePeriodFacilityElectricityGridMixDto(int reportingPeriodFacilityId, int reportingPeriodId, int supplierId, int facilityId, string facilityName, string? ghgrpFacilityId, IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> reportingPeriodFacilityElectricityGridMixDtos,bool isActive)
        {
            ReportingPeriodFacilityId = reportingPeriodFacilityId;
            ReportingPeriodId = reportingPeriodId;
            SupplierId = supplierId;
            FacilityId = facilityId;
            FacilityName = facilityName;
            GHGRPFacilityId = ghgrpFacilityId;
            ReportingPeriodFacilityElectricityGridMixDtos = reportingPeriodFacilityElectricityGridMixDtos;
            IsActive = isActive;
        }
    }
}
