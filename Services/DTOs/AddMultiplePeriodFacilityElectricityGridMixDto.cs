namespace Services.DTOs
{
    public class AddMultiplePeriodFacilityElectricityGridMixDto
    {
        public int Id { get; set; }
        public int ReportingPeriodFacilityId { get; set; }
        public int ReportingPeriodSupplierId { get; set; }
        public IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> ReportingPeriodFacilityElectricityGridMixDtos { get; set; }
        public bool IsActive { get; set; }

        public AddMultiplePeriodFacilityElectricityGridMixDto(int id, int reportingPeriodFacilityId, int reportingPeriodSupplierId, IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> reportingPeriodFacilityElectricityGridMixDtos,bool isActive)
        {
            Id = id;
            ReportingPeriodFacilityId = reportingPeriodFacilityId;
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
            ReportingPeriodFacilityElectricityGridMixDtos = reportingPeriodFacilityElectricityGridMixDtos;
            IsActive = isActive;
        }
    }
}
