namespace PfcAPI.Infrastucture.DbLog
{
    public static class CustomLoggingMiddlewareExtensions
    {
        public static void UseRequestResponseLogging(this IApplicationBuilder app) => app.UseMiddleware<RequestResponseLoggerMiddleware>();

    }
}
