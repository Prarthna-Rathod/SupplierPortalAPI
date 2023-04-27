using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReferenceLookups
{
    public class UnitOfMeasure: ReferenceLookup
    {
        public UnitOfMeasure() { }
        public UnitOfMeasure(int value, string displayName): base(value, displayName) { }
    }
}
