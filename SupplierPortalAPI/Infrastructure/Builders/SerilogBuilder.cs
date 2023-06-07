using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

namespace SupplierPortalAPI.Infrastructure.Builders
{
    internal static class SerilogBuilder
    {
        internal static WebApplicationBuilder SerilogConfiguration(this WebApplicationBuilder builder)
        {
            var connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=SupplierPortalDB;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;";

            var logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.MSSqlServer(connectionString, "Logs", autoCreateSqlTable: true,
                columnOptions: new ColumnOptions()
                {
                    AdditionalColumns = new Collection<SqlColumn>()
                    {
                        new SqlColumn {DataType = SqlDbType.NVarChar, ColumnName = "PreviousStatus", AllowNull = true},
                        new SqlColumn {DataType = SqlDbType.NVarChar, ColumnName = "UpdatedStatus", AllowNull = true}
                    }
                }).Enrich.FromLogContext().CreateLogger();

            builder.Logging.AddSerilog(logger);
            return builder;
        }
    }
}
