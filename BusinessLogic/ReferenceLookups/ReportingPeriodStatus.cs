using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReferenceLookups
{
    public class ReportingPeriodStatus : ReferenceLookup
    {

        public ReportingPeriodStatus() { }

        public ReportingPeriodStatus(int value, string displayName) : base(value, displayName) { }
    }
}
