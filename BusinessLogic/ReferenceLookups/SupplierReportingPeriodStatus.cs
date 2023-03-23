using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReferenceLookups
{
    public class SupplierReportingPeriodStatus : ReferenceLookup
    {
        public SupplierReportingPeriodStatus()
        {}

        public SupplierReportingPeriodStatus(int value, string displayName) : base(value, displayName) { }
    }
}
