using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReferenceLookups
{
    public class ElectricityGridMixComponent: ReferenceLookup
    {
        public ElectricityGridMixComponent() { }
        public ElectricityGridMixComponent(int value, string displayName): base(value, displayName) { }
    }
}
