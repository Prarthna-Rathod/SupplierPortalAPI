namespace Services.DTOs;

public class AddMultiplePeriodFacilityElectricityGridMixDto
{
 
    public int ReportingPeriodId { get; set; }
    public int ReportingPeriodFacilityId { get; set; }
    public int ReportingPeriodSupplierId { get; set; }

    public int FercRegionId { get; set; }
    public IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> ReportingPeriodFacilityElectricityGridMixDtos { get; set; }


}
