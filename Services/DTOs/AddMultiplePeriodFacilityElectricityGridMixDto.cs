namespace Services.DTOs
{
    public class AddMultiplePeriodFacilityElectricityGridMixDto
    {
        public int Id { get; set; }
        public int ReportingPeriodFacilityId { get; set; }
        public int ReportingPeriodId { get; set; }
        public int SupplierId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasureName { get; set; }
        public int FercRegionId { get; set; }
        public string FercRegionName { get; set; }
        public IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> ReportingPeriodFacilityElectricityGridMixDtos { get; set; } = new List<ReportingPeriodFacilityElectricityGridMixDto>();

        public AddMultiplePeriodFacilityElectricityGridMixDto(int id, int reportingPeriodFacilityId,int reportingPeriodId,int supplierId, int unitOfMeasureId, string unitOfMeasureName, int fercRegionId, string fercRegionName, IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> reportingPeriodFacilityElectricityGridMixDtos)
        {
            Id = id;
            ReportingPeriodFacilityId = reportingPeriodFacilityId;
            ReportingPeriodId = reportingPeriodId;
            SupplierId = supplierId;
            UnitOfMeasureId = unitOfMeasureId;
            UnitOfMeasureName = unitOfMeasureName;
            FercRegionId = fercRegionId;
            FercRegionName = fercRegionName;
            ReportingPeriodFacilityElectricityGridMixDtos = reportingPeriodFacilityElectricityGridMixDtos;
        }
    }
}
