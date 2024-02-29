using PfcAPI.Model.Maintainance;

namespace PfcAPI.Infrastucture.DbLog
{
    //DbLoggerExtensions.cs  
    public static class DbLoggerExtensions
    {
        public static ILoggingBuilder AddDbLogger(this ILoggingBuilder builder, Action<DbLoggerOptions> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, DbLoggerProvider>();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}
