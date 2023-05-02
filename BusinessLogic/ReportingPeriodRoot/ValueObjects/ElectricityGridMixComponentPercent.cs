using BusinessLogic.ReferenceLookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReportingPeriodRoot.ValueObjects
{
    public class ElectricityGridMixComponentPercent
    {
        public ElectricityGridMixComponent ElectricityGridMixComponent { get; set; }
        public decimal Content { get; set; }

        public ElectricityGridMixComponentPercent(ElectricityGridMixComponent electricityGridMixComponent,decimal content)
        {
            ElectricityGridMixComponent = electricityGridMixComponent;
            Content = content;
        }

    }
}
