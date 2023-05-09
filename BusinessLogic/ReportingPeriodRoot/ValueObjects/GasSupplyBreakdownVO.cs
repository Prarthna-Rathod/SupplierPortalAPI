using BusinessLogic.ReferenceLookups;

namespace BusinessLogic.ReportingPeriodRoot.ValueObjects
{
    public class GasSupplyBreakdownVO
    {
        public int Id { get; set; }
        public int PeriodFacilityId { get; set; }
        public int FacilityId { get; set; }
        public Site Site { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public decimal Content { get; set; }

        public GasSupplyBreakdownVO(int id, int periodFacilityId, int facilityId, Site site, UnitOfMeasure unitOfMeasure, decimal content)
        {
            Id = id;
            PeriodFacilityId = periodFacilityId;
            FacilityId = facilityId;
            Site = site;
            UnitOfMeasure = unitOfMeasure;
            Content = content;
        }

    }
}
