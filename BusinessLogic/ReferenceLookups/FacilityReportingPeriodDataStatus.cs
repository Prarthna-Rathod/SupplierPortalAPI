using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReferenceLookups
{
    public class FacilityReportingPeriodDataStatus : ReferenceLookup
    {
        public FacilityReportingPeriodDataStatus(){}

        public FacilityReportingPeriodDataStatus(int value, string displayName) : base(value, displayName) { }
    }
}
