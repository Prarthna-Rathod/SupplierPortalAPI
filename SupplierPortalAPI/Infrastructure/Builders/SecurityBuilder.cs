using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SupplierPortalAPI.Infrastructure.Builders
{
    internal static class SecurityBuilder
    {
        internal static WebApplicationBuilder SecutitySchema(this WebApplicationBuilder builder)
        {

            string domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority= domain;
                options.Audience = builder.Configuration["Auth0:Audience"];
            });
            return builder;
        }
    }
}
