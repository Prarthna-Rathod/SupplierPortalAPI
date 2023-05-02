namespace Services.DTOs
{
    public class ReportingPeriodFacilityElectricityGridMixDto
    {
        public int ElectricityGridMixComponentId { get; set; }
        public string ElectricityGridMixComponentName { get; set; }
        public decimal Content { get; set; }

        public ReportingPeriodFacilityElectricityGridMixDto(int electricityGridMixComponentId, string electricityGridMixComponentName, decimal content)
        {
            ElectricityGridMixComponentId = electricityGridMixComponentId;
            ElectricityGridMixComponentName = electricityGridMixComponentName;
            Content = content;
        }
    }
}
