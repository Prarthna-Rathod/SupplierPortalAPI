using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Logging
{
    public static class LoggingType
    {
        //represents a specific type of log related to a supplier portal
        public const string AuditSupplierPortalLog = "SupplierPortalLog";

        //it contains the AuditSupplierPortalLog value.
        public static string[] AuditAllLog = new[] { AuditSupplierPortalLog };
    }
}
