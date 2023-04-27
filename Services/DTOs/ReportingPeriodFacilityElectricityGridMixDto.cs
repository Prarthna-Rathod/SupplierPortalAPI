namespace Services.DTOs
{
    public class ReportingPeriodFacilityElectricityGridMixDto
    {
        public int ElectricityGridMixComponentId { get; set; }
        public string ElectricityGridMixComponentName { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasureName { get; set; }
        public decimal Content { get; set; }

        public ReportingPeriodFacilityElectricityGridMixDto(int electricityGridMixComponentId, string electricityGridMixComponentName, int unitOfMeasureId, string unitOfMeasureName, decimal content)
        {
            ElectricityGridMixComponentId = electricityGridMixComponentId;
            ElectricityGridMixComponentName = electricityGridMixComponentName;
            UnitOfMeasureId = unitOfMeasureId;
            UnitOfMeasureName = unitOfMeasureName;
            Content = content;
        }
    }
}
