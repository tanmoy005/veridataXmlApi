using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;
using VERIDATA.Model.Base;

namespace PfcAPI.Infrastucture.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static readonly ILogger _logger;
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
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
                        //await context.Response.WriteAsync(new ErrorResponse()
                        //{
                        //    ErrorCode = context.Response.StatusCode,
                        //    UserMessage = contextFeature.Error.Message,
                        //    InternalMessage = contextFeature.Error.StackTrace ?? string.Empty,
                        //}.ToString());
                        var _errorResponse = new ErrorResponse
                        {
                            ErrorCode = context.Response.StatusCode,
                            UserMessage = contextFeature.Error.Message,
                            InternalMessage = contextFeature.Error.StackTrace ?? string.Empty,
                        };
                        var response = JsonConvert.SerializeObject(new BaseResponse<ErrorResponse>(HttpStatusCode.InternalServerError, _errorResponse)).ToString();
                        await context.Response.WriteAsync(response);
                    }
                });
            });
        }
    }
}