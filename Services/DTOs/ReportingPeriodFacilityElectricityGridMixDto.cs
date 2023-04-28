namespace Services.DTOs
{
    public class ReportingPeriodFacilityElectricityGridMixDto
    {
        public int ElectricityGridMixComponentId { get; set; }
        public string ElectricityGridMixComponentName { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasureName { get; set; }
        public int FercRegionId { get; set; }
        public string FercRegionName { get; set; }
        public decimal Content { get; set; }

        public ReportingPeriodFacilityElectricityGridMixDto(int electricityGridMixComponentId, string electricityGridMixComponentName, int unitOfMeasureId, string unitOfMeasureName,int fercRegionId, string fercRegionName, decimal content)
        {
            ElectricityGridMixComponentId = electricityGridMixComponentId;
            ElectricityGridMixComponentName = electricityGridMixComponentName;
            UnitOfMeasureId = unitOfMeasureId;
            UnitOfMeasureName = unitOfMeasureName;
            FercRegionId = fercRegionId;
            FercRegionName = fercRegionName;
            Content = content;
        }
    }
}
