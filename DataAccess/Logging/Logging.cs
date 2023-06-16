using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Logging
{
    public class Logging : ILogging
    {
        private readonly ILogger<Logging> _logger;
        public Logging(ILogger<Logging> logger)  //SupplierPortalDBContext context
        {
            _logger = logger;
        }

        public void PushSerilog(IEnumerable<EntityEntry> entityEntries)
        {
            foreach (var entry in entityEntries)
            {
                var entryState = entry.State.ToString();
                LogLevel logLevel= LogLevel.Information;
                var message = "{Entity} was {State}";
                using (LogContext.PushProperty("LoggingType",LoggingType.AuditSupplierPortalLog))
                using(LogContext.PushProperty("EntityName",entry.Entity.GetType().Name))
                    _logger.Log(logLevel, message,entry.Metadata.GetTableName(),entryState);
            }
        }
    }
}
