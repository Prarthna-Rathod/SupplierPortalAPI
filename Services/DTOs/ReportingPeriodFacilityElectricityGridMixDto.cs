namespace Services.DTOs;

public class ReportingPeriodFacilityElectricityGridMixDto
{
    public ReportingPeriodFacilityElectricityGridMixDto()
    {
    }

    //public ReportingPeriodFacilityElectricityGridMixDto(int id, int componentId, string componentName, int unitOfMeasureId, string unitOfMeasureName, decimal content, bool isActive)
    //{
    //    Id = id;
    //    ElectricityGridMixComponentId = componentId;
    //    ElectricityGridMixComponentName = componentName;
    //    UnitOfMeasureId = unitOfMeasureId;
    //    UnitOfMeasureName = unitOfMeasureName;
    //    Content = content;
    //    IsActive = isActive;
    //}

    public ReportingPeriodFacilityElectricityGridMixDto(int electricityGridMixComponentId, string electricityGridMixComponentName, decimal content)
    {
        ElectricityGridMixComponentId = electricityGridMixComponentId;
        ElectricityGridMixComponentName = electricityGridMixComponentName;
        Content = content;
    }

    public int Id { get; set; }
    public int ElectricityGridMixComponentId { get; set; }
    public string ElectricityGridMixComponentName { get; set; }
    public int UnitOfMeasureId { get; set; }
    public string UnitOfMeasureName { get; set; }
    public decimal Content { get; set; }
    public bool IsActive { get; set; }


}
