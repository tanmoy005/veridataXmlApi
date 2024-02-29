using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using VERIDATA.Model.Base;

namespace VERIDATA.BLL.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static readonly ILogger _logger;
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            _ = app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    IExceptionHandlerFeature? contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {

                        ErrorResponse _errorResponse = new()
                        {
                            ErrorCode = context.Response.StatusCode,
                            UserMessage = contextFeature.Error.Message,
                            InternalMessage = contextFeature.Error.StackTrace ?? string.Empty,
                        };
                        string response = JsonConvert.SerializeObject(new BaseResponse<ErrorResponse>(HttpStatusCode.InternalServerError, _errorResponse)).ToString();
                        await context.Response.WriteAsync(response);
                    }
                });
            });
        }
    }
}