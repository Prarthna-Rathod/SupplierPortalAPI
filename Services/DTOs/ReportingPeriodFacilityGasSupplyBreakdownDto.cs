namespace Services.DTOs
{
    public class ReportingPeriodFacilityGasSupplyBreakdownDto
    {
        public int PeriodFacilityId { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasureName { get; set; }
        public decimal Content { get; set; }

        public ReportingPeriodFacilityGasSupplyBreakdownDto(int periodFacilityId, int facilityId, string facilityName, int siteId, string siteName, int unitOfMeasureId, string unitOfMeasureName, decimal content)
        {
            PeriodFacilityId = periodFacilityId;
            FacilityId = facilityId;
            FacilityName = facilityName;
            SiteId = siteId;
            SiteName = siteName;
            UnitOfMeasureId = unitOfMeasureId;
            UnitOfMeasureName = unitOfMeasureName;
            Content = content;
        }
    }
}
