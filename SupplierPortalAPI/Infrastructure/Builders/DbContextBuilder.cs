using DataAccess.DataActionContext;
using Microsoft.EntityFrameworkCore;

namespace SupplierPortalAPI.Infrastructure.Builders;

internal static class DbContextBuilder
{
    internal static WebApplicationBuilder BuilderDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<SupplierPortalDBContext>(o => o.UseSqlServer(
        builder.Configuration.GetConnectionString("SupplierConnnection")));
        return builder;
    }

}
