namespace Services.DTOs
{
    public class AddMultiplePeriodFacilityElectricityGridMixDto
    {
        public int Id { get; set; }
        public int ReportingPeriodFacilityId { get; set; }
        public int ReportingPeriodSupplierId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasureName { get; set; }
        public int FercRegionId { get; set; }
        public string FercRegionName { get; set; }
        public IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> ReportingPeriodFacilityElectricityGridMixDtos { get; set; }
        public bool IsActive { get; set; }

        public AddMultiplePeriodFacilityElectricityGridMixDto(int id, int reportingPeriodFacilityId, int reportingPeriodSupplierId, int unitOfMeasureId, string unitOfMeasureName, int fercRegionId, string fercRegionName, IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> reportingPeriodFacilityElectricityGridMixDtos,bool isActive)
        {
            Id = id;
            ReportingPeriodFacilityId = reportingPeriodFacilityId;
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
            UnitOfMeasureId = unitOfMeasureId;
            UnitOfMeasureName = unitOfMeasureName;
            FercRegionId = fercRegionId;
            FercRegionName = fercRegionName;
            ReportingPeriodFacilityElectricityGridMixDtos = reportingPeriodFacilityElectricityGridMixDtos;
            IsActive = isActive;
        }
    }
}
