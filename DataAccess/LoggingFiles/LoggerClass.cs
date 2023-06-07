using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;

namespace DataAccess.LoggingFiles
{
    public static class LoggerClass
    {
        public static string connectionString = "Data Source=prarthna-rathod\\sqlexpress;Initial Catalog=SupplierPortalDB;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;";

        //Initialize the Log configurations
        //WriteTo : fails silently and execute next task
        //AuditTo : not executed if task fails (store only information)
        public static void InitializeLoggers(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Logger(logger => ApplySystemLog(logger, configuration))
                .AuditTo.Logger(logger => ApplyAuditSupplierPortalLog(logger, configuration))
                .CreateLogger();
        }
        private static void ApplySystemLog(LoggerConfiguration logger, IConfiguration configuration)
        {
            LogEventLevel logEventLevelSystemLog = GetLogEventLevelFromString(configuration["Serilog:LevelSwitches:$systemLogSwitch"]);

            //Create new instance for MSSqlServerSinkOptions (inbuilt DB configurations)
            var newsinkOptions = new MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = true,
                SchemaName = "Serilog"
            };
            var newcolumnOptions = new ColumnOptions {};
            newcolumnOptions.Store.Remove(StandardColumn.Properties);
            newcolumnOptions.Store.Add(StandardColumn.LogEvent);
            logger.Filter.ByExcluding(Matching.WithProperty<string>("LoggingType", login => { return LoggingType.AuditAllLog.Contains(login); }))
                .WriteTo.MSSqlServer(connectionString, sinkOptions: newsinkOptions, restrictedToMinimumLevel: logEventLevelSystemLog, columnOptions: newcolumnOptions);
        }
        private static void ApplyAuditSupplierPortalLog(LoggerConfiguration logger, IConfiguration configuration)
        {
            LogEventLevel logLevelSystemLog = GetLogEventLevelFromString(configuration["Serilog:LevelSwitches:$systemLogSwitch"]);
            var sinkOptions = new MSSqlServerSinkOptions
            {
                TableName = "SupplierPortalLog",
                AutoCreateSqlTable = true,
                SchemaName = "Serilog"
            };
            var ColumnOptions = new ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn> {
                          new SqlColumn("EntityName", System.Data.SqlDbType.NVarChar)
                    }
            };
            ColumnOptions.Store.Remove(StandardColumn.Properties);
            ColumnOptions.Store.Add(StandardColumn.LogEvent);
            logger.Filter.ByIncludingOnly(Matching.WithProperty("LoggingType", LoggingType.AuditSupplierPortalLog))
                .AuditTo.MSSqlServer(connectionString, sinkOptions: sinkOptions, restrictedToMinimumLevel: logLevelSystemLog, columnOptions: ColumnOptions);
        }
        private static LogEventLevel GetLogEventLevelFromString(string logLevelEvent)
        {
            //Create new variable of LogEventLevel
            LogEventLevel logEventLevelOutput;
                                    //LogLevelEvent : Info, Warning, Error etc.
                                        //LogLevelEvent, IgnoreCase, output variable
            Enum.TryParse<LogEventLevel>(logLevelEvent, true, out logEventLevelOutput);
            return logEventLevelOutput;
        }
    }
}
