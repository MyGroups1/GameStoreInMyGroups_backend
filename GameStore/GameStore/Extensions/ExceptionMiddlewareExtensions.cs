
using Entities.ErrorModel;
using Entities.Exceptions.Abstract;
using LoggingService;
using Microsoft.AspNetCore.Diagnostics;

namespace GameStore.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
        
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            
                            NotFoundException _ => StatusCodes.Status404NotFound,
                            BadRequestException _ => StatusCodes.Status400BadRequest,
                            _ => StatusCodes.Status500InternalServerError
                        };
        
                        logger.LogError($"Something went wrong: {contextFeature.Error}");
        
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
}