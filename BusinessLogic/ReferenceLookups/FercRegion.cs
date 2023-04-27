using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReferenceLookups
{
    public class FercRegion: ReferenceLookup
    {
        public FercRegion() { }
        public FercRegion(int value, string displayName): base(value, displayName) { }
    }
}
