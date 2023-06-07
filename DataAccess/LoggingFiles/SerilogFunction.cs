using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace DataAccess.LoggingFiles
{
    public class SerilogFunction : ISerilog
    {
        private readonly ILogger<SerilogFunction> _logger;

        public SerilogFunction(ILogger<SerilogFunction> logger)//SupplierPortalDBContext context, 
        {
            _logger = logger;
        }

        public void PushSerilog(IEnumerable<EntityEntry> entityEntries)
        {
            foreach (var entry in entityEntries)
            {
                var entryState = entry.State.ToString();
                LogLevel logLevel = LogLevel.Information;
                var message = "{Entity} was {State}";
                using (LogContext.PushProperty("LoggingType", LoggingType.AuditSupplierPortalLog))
                using (LogContext.PushProperty("EntityName", entry.Entity.GetType().Name))
                    _logger.Log(logLevel, message, entry.Metadata.GetTableName(), entryState);
            }
        }
    }
}
