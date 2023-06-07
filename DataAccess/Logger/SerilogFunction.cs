using DataAccess.DataActionContext;
using DataAccess.DataActions.Interfaces;
using DataAccess.Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace DataAccess.DataActions
{
    public class SerilogFunction : ISerilog
    {
        private readonly SupplierPortalDBContext _context;
        private readonly ILogger<SerilogFunction> _logger;

        public SerilogFunction(SupplierPortalDBContext context,ILogger<SerilogFunction> logger)
        {
            _context = context;
            _logger = logger;
        }

        /*public void PushSerilog(object previousValue, object updatedValue)
        {
            var entries = _context.ChangeTracker.Entries().ToList();
            entries.ForEach(entry =>
            {
                LogLevel level = LogLevel.Information;
                var messageTemplate = "Entity was added/modified";
                //using (LogContext.PushProperty("PreviousStatus", previousValue.GetType().GetProperties().Select(x => x.GetValue(previousValue))))
                using (LogContext.PushProperty("PreviousStatus", previousValue.GetType().Name))
                using (LogContext.PushProperty("UpdatedStatus", updatedValue.GetType().Name))
                    _logger.Log(level,messageTemplate, entry.Metadata.GetTableName(), entry.State.ToString());
            });
        }*/

        public void LogPush(IEnumerable<EntityEntry> entityEntries)
        {
            foreach(var entry in entityEntries)
            {
                if(entry.State.ToString() != "Unchanged")
                {
                    LogLevel level = LogLevel.Information;
                    var messageTemplate = "{Entity} was {State}";
                    using (LogContext.PushProperty("LoggingType", LoggingType.SupplierLog))
                    using (LogContext.PushProperty("EntityName", entry.Entity.GetType().Name))
                        _logger.Log(level, messageTemplate, entry.Metadata.GetTableName(), entry.State.ToString());
                }
            }
        }
    }
}
