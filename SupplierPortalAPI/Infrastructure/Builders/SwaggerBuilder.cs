using Microsoft.OpenApi.Models;

namespace SupplierPortalAPI.Infrastructure.Builders
{
    internal static class SwaggerBuilder
    {
        internal static WebApplicationBuilder AddSwaggerBuilder(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "SupplierPortalAPI", Version = "v1" });
                c.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"https://{builder.Configuration["Auth0:Domain"]}/authorize?audience={builder.Configuration["Auth0:Audience"]}"),
                            TokenUrl = new Uri($"https://{builder.Configuration["Auth0:Domain"]}/oauth/token")
                        }
                    }
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="OAuth2"
                            }
                        },
                        new string[]{}
                    }

                });
               
            });
            return builder;
        }
    }
}
