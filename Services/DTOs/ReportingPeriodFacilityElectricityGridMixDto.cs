namespace Services.DTOs
{
    public class ReportingPeriodFacilityElectricityGridMixDto
    {
        public int? Id { get; set; }
        public int ReportingPeriodFacilityId { get; set; }
        public int ElectricityGridMixComponentId { get; set; }
        public string ElectricityGridMixComponentName { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasureName { get; set; }
        public decimal Content { get; set; }

        public ReportingPeriodFacilityElectricityGridMixDto(int? id, int reportingPeriodFacilityId, int electricityGridMixComponentId, string electricityGridMixComponentName, int unitOfMeasureId, string unitOfMeasureName, decimal content)
        {
            Id = id;
            ReportingPeriodFacilityId = reportingPeriodFacilityId;
            ElectricityGridMixComponentId = electricityGridMixComponentId;
            ElectricityGridMixComponentName = electricityGridMixComponentName;
            UnitOfMeasureId = unitOfMeasureId;
            UnitOfMeasureName = unitOfMeasureName;
            Content = content;
        }
    }
}
