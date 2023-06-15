using DataAccess.DataActionContext;
using DataAccess.DataActions.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccess.Extension
{
    public static class SerilogExtension
    {
        private static SupplierPortalDBContext _context;
        private static ISerilog _serilog;

        public static int LogExtensionMethod(this SupplierPortalDBContext context, ISerilog serilog)
        {
            _context = context;
            _serilog = serilog;
            _serilog.LogPush(_context.ChangeTracker.Entries());
            return _context.SaveChanges();
        }
    }
}
