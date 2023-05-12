namespace Services.DTOs
{
    public class MultiplePeriodFacilityElectricityGridMixDto
    {
        public int ReportingPeriodFacilityId { get; set; }
        public int ReportingPeriodId { get; set; }
        public int SupplierId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasureName { get; set; }
      
        public IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> ReportingPeriodFacilityElectricityGridMixDtos { get; set; } = new List<ReportingPeriodFacilityElectricityGridMixDto>();

        public MultiplePeriodFacilityElectricityGridMixDto(int reportingPeriodFacilityId, int reportingPeriodId, int supplierId, int unitOfMeasureId, string unitOfMeasureName, IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> reportingPeriodFacilityElectricityGridMixDtos)
        {
            ReportingPeriodFacilityId = reportingPeriodFacilityId;
            ReportingPeriodId = reportingPeriodId;
            SupplierId = supplierId;
            UnitOfMeasureId = unitOfMeasureId;
            UnitOfMeasureName = unitOfMeasureName;
            ReportingPeriodFacilityElectricityGridMixDtos = reportingPeriodFacilityElectricityGridMixDtos;
        }
    }
}
