using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SupplierPortalAPI.Infrastructure.Builders
{
    internal static class SecurityBuilder
    {
        /*//Auth0
        //internal static WebApplicationBuilder SecutitySchema(this WebApplicationBuilder builder)
        //{

        //    string domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
        //    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        //    {
        //        options.Authority= domain;
        //        options.Audience = builder.Configuration["Auth0:Audience"];
        //    });
        //    return builder;
        //}
        //Auth0*/
        internal static WebApplicationBuilder SecutitySchema(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Adding Jwt (Bearer ...paste token)    
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            return builder;
        }
    }
}
