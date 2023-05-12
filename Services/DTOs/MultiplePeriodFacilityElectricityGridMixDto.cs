namespace Services.DTOs
{
    public class MultiplePeriodFacilityElectricityGridMixDto
    {
        public int ReportingPeriodFacilityId { get; set; }
        public int ReportingPeriodId { get; set; }
        public int SupplierId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasureName { get; set; }
        public int FercRegionId { get; set; }
        public string FercRegionName { get; set; }
      
        public IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> ReportingPeriodFacilityElectricityGridMixDtos { get; set; } = new List<ReportingPeriodFacilityElectricityGridMixDto>();

        public MultiplePeriodFacilityElectricityGridMixDto(int reportingPeriodFacilityId, int reportingPeriodId, int supplierId, int unitOfMeasureId, string unitOfMeasureName, int fercRegionId, string fercRegionName,  IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> reportingPeriodFacilityElectricityGridMixDtos)
        {
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
