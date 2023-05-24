using BusinessLogic.ReferenceLookups;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class PeriodFacilityGasSupplyBreakDown
    {
        public int Id { get; set; }
        public int PeriodFacilityId { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public Site Site { get; set; }
        public decimal Content { get; set; }


        internal PeriodFacilityGasSupplyBreakDown() { }


        public PeriodFacilityGasSupplyBreakDown(int id, int periodFacilityId, UnitOfMeasure unitOfMeasure, Site site, decimal content)
        {
            Id = id;
            PeriodFacilityId = periodFacilityId;
            UnitOfMeasure = unitOfMeasure;
            Site = site;
            Content = content;
        }
    }
}
