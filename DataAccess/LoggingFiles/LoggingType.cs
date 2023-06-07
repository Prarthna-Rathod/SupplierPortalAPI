using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.LoggingFiles
{
    public static class LoggingType
    {
        public const string AuditSupplierPortalLog = "SupplierPortalLog";
        public static string[] AuditAllLog = new[] { AuditSupplierPortalLog };
    }
}
