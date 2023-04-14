using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ValueConstants
{
    public class ReportingPeriodStatusValues
    {
        public static string[] ReportingPeriodStatuses =
        {
            InActive,
            Open,
            Close,
            Complete
        };
        public const string InActive = "InActive";
        public const string Open = "Open";
        public const string Close = "Closed";
        public const string Complete = "Complete";

    }
}
