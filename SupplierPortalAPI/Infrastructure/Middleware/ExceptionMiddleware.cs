using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;
using System.Net;

namespace SupplierPortalAPI.Infrastructure.Middleware;

public static class ExceptionMiddleware
{
    public static void AddExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        NoContentException => StatusCodes.Status204NoContent,
                        AlreadyExistException => StatusCodes.Status409Conflict,
                        NotFoundException => StatusCodes.Status404NotFound,
                        BadRequestException => StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    };
                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message,
                    }.ToString());
                }
            });
        });
    }
}

public class Errorinfo
{
    public int StatusCode { get; set; } = 0;
    public string ErrorMessage { get; set; } = string.Empty;
}
