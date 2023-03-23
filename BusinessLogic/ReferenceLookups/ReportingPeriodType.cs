using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReferenceLookups
{
    public class ReportingPeriodType : ReferenceLookup
    {
        public ReportingPeriodType() { }

        public ReportingPeriodType(int value, string displayName) : base(value, displayName) { }
    }
}
