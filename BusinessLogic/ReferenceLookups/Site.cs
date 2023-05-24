using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReferenceLookups
{
    public class Site :ReferenceLookup
    {
        public Site() { }

        public Site(int value, string displayName) : base(value, displayName) { }
    }
}
