using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReferenceLookups
{
    public class SupplyChainStage : ReferenceLookup
    {
        public SupplyChainStage() { }

        public SupplyChainStage(int value, string displayName) : base(value, displayName) { }
    }
}
