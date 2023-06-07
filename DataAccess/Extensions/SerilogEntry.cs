using DataAccess.DataActionContext;
using DataAccess.LoggingFiles;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Extensions
{
    public static class SerilogEntry
    {
        private static SupplierPortalDBContext _context;
        private static ISerilog _logger;
        private static string ENTITY_STATE_UNCHANGED = "Unchanged";

        public static int LogEntryAndSaveChanges(this SupplierPortalDBContext context, ISerilog logger)
        {
            _context = context;
            _logger = logger;
            var entityEntries = _context.ChangeTracker.Entries().Where(x => x.State.ToString() != ENTITY_STATE_UNCHANGED).ToList();
            _logger.PushSerilog(entityEntries);
            return _context.SaveChanges();
        }

    }
}
