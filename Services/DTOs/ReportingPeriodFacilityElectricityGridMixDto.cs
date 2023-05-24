namespace Services.DTOs;

public class ReportingPeriodFacilityElectricityGridMixDto
{
    public int Id { get; set; }
    public int ElectricityGridMixComponentId { get; set; }
    public string ElectricityGridMixComponentName { get; set; }
    public int UnitOfMeasureId { get; set; }
    public string UnitOfMeasureName { get; set; }
    public decimal Content { get; set; }
    public bool IsActive { get; set; }


}
