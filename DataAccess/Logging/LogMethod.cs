using DataAccess.DataActionContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Logging
{
    public static class LogMethod
    {
        //private static SupplierPortalDBContext _context;
        private static ILogging _logging;

        public static int LogExtensionMethod(this SupplierPortalDBContext context, ILogging logging)
        {
            //_context = context;
            _logging = logging;

            var allEntries = context.ChangeTracker.Entries();
            var filterEntries = allEntries.Where(entry => entry.State.ToString() != "Unchanged").ToList();
            _logging.PushSerilog(filterEntries);
            return context.SaveChanges();
        }
    }
}