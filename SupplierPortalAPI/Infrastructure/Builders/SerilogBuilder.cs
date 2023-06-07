using Serilog;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

namespace SupplierPortalAPI.Infrastructure.Builders
{
    internal static class SerilogBuilder
    {
      //  [Obsolete]
        internal static WebApplicationBuilder AddSerilogConfiguration(this WebApplicationBuilder builder)
        {
            var connectionString = "Data Source=prarthna-rathod\\sqlexpress;Initial Catalog=SupplierPortalDB;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;";

#pragma warning disable // Type or member is obsolete
            var logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.MSSqlServer(
                connectionString, "Logs", autoCreateSqlTable: true,
                        columnOptions: new ColumnOptions()
                        {
                            AdditionalColumns = new Collection<SqlColumn>()
                            {
                                new SqlColumn { DataType = SqlDbType.NVarChar, ColumnName = "PreviousValue", AllowNull = true },
                                new SqlColumn{ DataType = SqlDbType.NVarChar, ColumnName = "UpdatedValue", AllowNull = true}
                            }
                        }).Enrich.FromLogContext()
                        .CreateLogger();
 // Type or member is obsolete

            builder.Logging.AddSerilog(logger);
            return builder;
        }
    }
}
