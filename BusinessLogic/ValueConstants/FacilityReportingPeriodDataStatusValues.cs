using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ValueConstants
{
    public class FacilityReportingPeriodDataStatusValues
    {
        public static string[] FacilityReportingPeriodDataStatus =
       {
            InProgress,
            Complete,
            Submitted
        };
        public const string InProgress = "In-progress";
        public const string Complete = "Complete";
        public const string Submitted = "Submitted";
    }
}
