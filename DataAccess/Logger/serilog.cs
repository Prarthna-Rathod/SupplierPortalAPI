using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;

namespace DataAccess.Logger
{
    public static class serilog
    {
        public static string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=SupplierPortalDB;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;";

        /// <summary>
        /// Initialize the log configuration
        /// WriteTo : fails silently and execute next task
        /// AuditTo : not executed if task fails (store only Information) 
        /// </summary>
        /// <param name="configuration"></param>
        public static void InitializeLoggers(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Logger(logger => ApplySystemLog(logger, configuration))
                .AuditTo.Logger(logger => ApplySupplierLog(logger, configuration))
                .CreateLogger();
        }
        private static void ApplySystemLog(LoggerConfiguration logger, IConfiguration configuration)
        {
            LogEventLevel logEventLevelSystemLog = GetLogEventLevelFromString(configuration["Serilog:LevelSwitches:$systemLogSwitch"]);

            //create new instance for MSSqlServerSinkOptions (inbuilt db configuration)
            var newSinkOptions = new MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = true,
                SchemaName = "Serilog"
            };
            var newColumnOptions = new ColumnOptions { };

            newColumnOptions.Store.Remove(StandardColumn.Properties);
            newColumnOptions.Store.Add(StandardColumn.LogEvent);
            logger.Filter.ByExcluding(Matching.WithProperty<string>("LoggingType", login => { return LoggingType.AuditAllLog.Contains(login); }))
                .WriteTo.MSSqlServer(ConnectionString, sinkOptions: newSinkOptions, restrictedToMinimumLevel: logEventLevelSystemLog,columnOptions: newColumnOptions);
        }
        private static void ApplySupplierLog(LoggerConfiguration logger, IConfiguration configuration)
        {
            LogEventLevel logLevelSystemLog = GetLogEventLevelFromString(configuration["Serilog:LevelSwitches:$systemLogSwitch"]);

            //create new instance for MSSqlServerSinkOptions (inbuilt db configuration)
            var sinkOptions = new MSSqlServerSinkOptions
            {
                TableName = "SupplierPortalLog",
                AutoCreateSqlTable = true,
                SchemaName = "Serilog"
            };
            var ColumnOptions = new ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn> {
                          new SqlColumn("EntityName", System.Data.SqlDbType.NVarChar),
                    }
            };
            ColumnOptions.Store.Remove(StandardColumn.Properties);
            ColumnOptions.Store.Add(StandardColumn.LogEvent);
            logger.Filter.ByIncludingOnly(Matching.WithProperty("LoggingType", LoggingType.SupplierLog))
                .AuditTo.MSSqlServer(ConnectionString, sinkOptions: sinkOptions, restrictedToMinimumLevel: logLevelSystemLog, columnOptions: ColumnOptions);
        }
        private static LogEventLevel GetLogEventLevelFromString(string logLevelEvent)
        {
            //create new variable of LogEventLevel
            LogEventLevel logEventLevelOutput;
            //logLevelEvent : Information,Warning,Error
            //logLevelEvent,ignoreCase,output variable
            Enum.TryParse<LogEventLevel>(logLevelEvent, true, out logEventLevelOutput);
            return logEventLevelOutput;
        }
    }
}
