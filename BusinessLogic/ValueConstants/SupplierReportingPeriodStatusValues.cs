using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ValueConstants;

public class SupplierReportingPeriodStatusValues
{   
    public static string[] SupplierReportingPeriodStatus = {
        Locked,
        Unlocked
    };
    public const string Locked = "Locked";
    public const string Unlocked = "Unlocked";
}
