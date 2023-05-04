using BusinessLogic.ReferenceLookups;

namespace BusinessLogic.ReportingPeriodRoot.ValueObjects
{
    public class ElectricityGridMixComponentPercent
    {
        public int Id { get; set; }
        public ElectricityGridMixComponent ElectricityGridMixComponent { get; set; }
        public decimal Content { get; set; }

        public ElectricityGridMixComponentPercent(int id, ElectricityGridMixComponent electricityGridMixComponent, decimal content)
        {
            Id = id;
            ElectricityGridMixComponent = electricityGridMixComponent;
            Content = content;
        }

    }
}
