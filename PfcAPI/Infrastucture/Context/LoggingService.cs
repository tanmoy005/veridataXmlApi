using Mustache;
using NLog;
using PfcAPI.Infrastucture.Interfaces;
using System.Diagnostics;
using System.Reflection;
using LogLevel = NLog.LogLevel;

namespace PfcAPI.Infrastucture.Context
{
    public sealed class LoggingService<T> : ILoggingService
    {
        private NLog.ILogger logger;

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(string message)
        {
            //if (ConfigurationManager.AppSettings["DebugLog:Enabled"] == "true")
            this.LogMessage(LogLevel.Debug, message);
        }

        /// <summary>
        /// Debugs if.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="message">The message.</param>
        public void DebugIf(Func<bool> condition, string message)
        {
            if (condition())
                this.LogMessage(LogLevel.Debug, message);
        }

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
            //ScheduleJobLoggerScheduleJobLogger
            this.LogMessage(LogLevel.Info, message);
        }

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="category">The category.</param>
        //public void ServiceCommunicationInfo(EntityServiceCommunicationLog serviceCommunicationLog)
        //{
        //    this.logger = LogManager.GetLogger("ServiceCommunicationLogger");
        //    LogEventInfo evt = new LogEventInfo(LogLevel.Info, this.logger.Name, "Service Communication Log");
        //    SetParameters(serviceCommunicationLog, evt);
        //    this.logger.Log(evt);
        //}

        //public void ServiceCommunicationInfo(EntityServiceCommunicationLog serviceCommunicationLog, char logLevel)
        //{
        //    this.logger = LogManager.GetLogger("ServiceCommunicationLogger");
        //    LogLevel ll = null;

        //    switch (logLevel)
        //    {
        //        case 'I':
        //            ll = LogLevel.Info;
        //            break;

        //        case 'E':
        //            ll = LogLevel.Error;
        //            break;

        //        case 'W':
        //            ll = LogLevel.Warn;
        //            break;

        //        case 'D':
        //            ll = LogLevel.Debug;
        //            break;
        //    }

        //    LogEventInfo evt = new LogEventInfo(ll, this.logger.Name, "Service Communication Log");
        //    SetParameters(serviceCommunicationLog, evt);
        //    this.logger.Log(evt);

        //    //if (ll == LogLevel.Error)
        //    //    DoEmailServiceErrorLog(serviceCommunicationLog);
        //}

        /// <summary>
        /// Traces the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Trace(string message)
        {
            this.LogMessage(LogLevel.Trace, message);
        }

        /// <summary>
        /// Traces if.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="message">The message.</param>
        public void TraceIf(Func<bool> condition, string message)
        {
            if (condition())
                this.LogMessage(LogLevel.Trace, message);
        }

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(string message)
        {
            this.LogMessage(LogLevel.Warn, message);
        }

        /// <summary>
        /// Warns if.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="message">The message.</param>
        public void WarnIf(Func<bool> condition, string message)
        {
            if (condition())
                this.LogMessage(LogLevel.Warn, message);
        }

        /// <summary>
        /// Errors the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="url">The URL.</param>
        public void Error(Exception ex, string arguments, string url)
        {
            this.logger = LogManager.GetLogger("ErrorLogger");
            LogEventInfo evt = new LogEventInfo(LogLevel.Error, this.logger.Name, "Error Log");
            evt.Properties.Add("Category", ex?.GetType().ToString());
            evt.Properties.Add("CallSite","");
            evt.Properties.Add("LineNumber", "");
            evt.Properties.Add("Message", ex?.Message);
            evt.Properties.Add("StackTrace", ex?.StackTrace);
            evt.Properties.Add("Arguments", arguments);
            evt.Properties.Add("Url", url);
            this.logger.Log(evt);
            //DoEmailErrorLog(ex, arguments, url);
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        /// <param name="category">The category.</param>
        private void LogMessage(LogLevel level, string message)
        {
            this.logger = LogManager.GetLogger("MessageLogger");
            StackFrame frame = new StackFrame(2, true);
            MethodBase method = frame.GetMethod();

            LogEventInfo evt = new LogEventInfo(level, this.logger.Name, "Message Log");
            evt.Properties.Add("Category", "");
            evt.Properties.Add("CallSite", $"{method.ReflectedType.Namespace}.{method.ReflectedType.Name}.{method.Name}()");
            evt.Properties.Add("LineNumber", frame.GetFileLineNumber().ToString());
            evt.Properties.Add("Message", message);
            this.logger.Log(evt);
        }

        /// <summary>
        /// Schedules the job logger.
        /// </summary>
        /// <param name="scheduleJobLogEntity">The schedule job log entity.</param>
        //public void ScheduleJobLogger(ScheduleJobLogEntity scheduleJobLogEntity)
        //{
        //    this.logger = LogManager.GetLogger("ScheduleJobLogger");
        //    LogEventInfo evt = new LogEventInfo(LogLevel.Info, this.logger.Name, "Schedule Job Log");
        //    evt.Properties.Add("JobName", scheduleJobLogEntity.JobName);
        //    evt.Properties.Add("StartedOn", scheduleJobLogEntity.StartedOn);
        //    evt.Properties.Add("EndedOn", scheduleJobLogEntity.EndedOn);
        //    evt.Properties.Add("ElapsedTime", scheduleJobLogEntity.ElapsedTime);
        //    this.logger.Log(evt);
        //}

        /// <summary>
        /// Schedules the job activity logger.
        /// </summary>
        /// <param name="scheduleJobActivityLogEntity">The schedule job activity log entity.</param>
        /// <param name="logLevel">The log level.</param>
        //public void ScheduleJobActivityLogger(ScheduleJobActivityLogEntity scheduleJobActivityLogEntity, LogLevel logLevel)
        //{
        //    this.logger = LogManager.GetLogger("ScheduleJobActivityLogger");
        //    LogEventInfo evt = new LogEventInfo(logLevel, this.logger.Name, "Schedule Job Activity Log");
        //    evt.Properties.Add("JobName", scheduleJobActivityLogEntity.JobName);
        //    evt.Properties.Add("ActivityType", logLevel.ToString());
        //    evt.Properties.Add("Activity", scheduleJobActivityLogEntity.Activity);
        //    evt.Properties.Add("ActivityStartsOn", scheduleJobActivityLogEntity.ActivityStartsOn.HasValue ?
        //        scheduleJobActivityLogEntity.ActivityStartsOn.ToString("yyyy-MM-dd hh:mm tt") : "");
        //    evt.Properties.Add("ActivityEndsOn", scheduleJobActivityLogEntity.ActivityEndsOn.HasValue ?
        //        scheduleJobActivityLogEntity.ActivityEndsOn.ToString("yyyy-MM-dd hh:mm tt") : "");
        //    evt.Properties.Add("Duration", scheduleJobActivityLogEntity.Duration);
        //    this.logger.Log(evt);
        //}

        //private void //DoEmailErrorLog(Exception ex, string arguments, string url)
        //{
        //    if (ConfigurationManager.AppSettings["ErrorLog:EmailNotification:Enabled"] == "true")
        //    {
        //        var _logger = LogManager.GetLogger("ErrorEmailLogger");
        //        var data = new
        //        {
        //            LogDate = DateTime.Now,
        //            Category = ex?.GetType().ToString(),
        //            CallSite = ex?.GetCallingFunctionDetails() ?? string.Empty,
        //            LineNumber = ex?.GetCallingFunctionLineNumber(),
        //            Message = ex?.Message,
        //            StackTrace = ex?.StackTrace,
        //            Arguments = arguments,
        //            Url = url
        //        };
        //        LogEventInfo evt = new LogEventInfo(LogLevel.Error,
        //            _logger.Name, ParseMessage("ApplicationErrorEmailLogger", data));
        //        if (!(data.CallSite.Contains("System.Web.Mvc.Controller.HandleUnknownAction()") || data.Category.Contains("System.Data.SqlTypes.SqlTypeException")
        //            || data.Category.Contains("System.Web.HttpException")))
        //        {
        //            SetEmailParameters(evt);
        //        }
        //        _logger.Log(evt);
        //    }
        //}

        //private void DoEmailServiceErrorLog(EntityServiceCommunicationLog serviceCommunicationLog)
        //{
        //    if (ConfigurationManager.AppSettings["ErrorLog:EmailNotification:Enabled"] == "true")
        //    {
        //        var _logger = LogManager.GetLogger("ServiceCommunicationEmailLogger");
        //        LogEventInfo evt = new LogEventInfo(LogLevel.Error,
        //            _logger.Name, ParseMessage("IntegrationErrorEmailLogger", serviceCommunicationLog));
        //        if (serviceCommunicationLog.Provider.Contains("ACCELAERO") || serviceCommunicationLog.Provider.Contains("SAP HRMS"))
        //            SetEmailParameters(evt);
        //        _logger.Log(evt);
        //    }
        //}

        //private string ParseMessage<T1>(string messageName, T1 payload)
        //{
        //    try
        //    {
        //        string content = GetEmbeddedResource($"Logger\\NotificationTemplates\\{messageName}.txt");

        //        if (!string.IsNullOrEmpty(content))
        //        {
        //            FormatCompiler compiler = new FormatCompiler();
        //            Generator generator = compiler.Compile(content);

        //            generator.KeyNotFound += (sender, e) =>
        //            {
        //                e.Substitute = "";
        //                e.Handled = true;
        //            };
        //            generator.KeyFound += (sender, e) =>
        //            {
        //                e.Substitute = e.Substitute == null ? "" : e.Substitute.ToString() == "True" ?
        //                    Convert.ToString(e.Substitute).ToLowerInvariant() : e.Substitute;
        //            };
        //            generator.ValueRequested += (sender, e) => e.Value = e.Value ?? "";

        //            return generator.Render(payload);
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    return string.Empty;
        //}

        //private string GetEmbeddedResource(string resourceName)
        //{
        //    Assembly assembly = Assembly.GetExecutingAssembly();
        //    resourceName = FormatResourceName(assembly, resourceName);
        //    using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
        //    {
        //        if (resourceStream == null)
        //            return null;

        //        using (StreamReader reader = new StreamReader(resourceStream))
        //        {
        //            return reader.ReadToEnd();
        //        }
        //    }
        //}

        //private string FormatResourceName(Assembly assembly, string resourceName)
        //{
        //    return assembly.GetName().Name + "." +
        //        resourceName
        //            .Replace(" ", "_")
        //            .Replace("\\", ".")
        //            .Replace("/", ".");
        //}

        //private void SetParameters(EntityServiceCommunicationLog serviceCommunicationLog, LogEventInfo evt)
        //{
        //    evt.Properties.Add("Provider", serviceCommunicationLog.Provider);
        //    evt.Properties.Add("RequestUrl", serviceCommunicationLog.RequestUrl);
        //    evt.Properties.Add("RequestCookie", serviceCommunicationLog.RequestCookie);
        //    evt.Properties.Add("RequestMethod", serviceCommunicationLog.RequestMethod);
        //    evt.Properties.Add("ServiceMethodName", serviceCommunicationLog.ServiceMethodName);
        //    evt.Properties.Add("ServiceResponseTypeName", serviceCommunicationLog.ServiceResponseTypeName);
        //    evt.Properties.Add("RequestPayload", serviceCommunicationLog.RequestPayload);
        //    evt.Properties.Add("ResponsePayload", serviceCommunicationLog.ResponsePayload);
        //    evt.Properties.Add("ReferenceNo", serviceCommunicationLog.ReferenceNo);
        //    evt.Properties.Add("RequestUserId", serviceCommunicationLog.RequestUserId);
        //    evt.Properties.Add("ElapsedTime", serviceCommunicationLog.ElapsedTime);
        //}

        //private void SetEmailParameters(LogEventInfo evt)
        //{
        //    evt.Properties.Add("SmtpServer", ConfigurationManager.AppSettings["EmailService:Server"]);
        //    evt.Properties.Add("SmtpPort", ConfigurationManager.AppSettings["EmailService:Port"]);
        //    evt.Properties.Add("EnableSsl", ConfigurationManager.AppSettings["EmailService:EnableSSLorTLS"]);
        //    evt.Properties.Add("SmtpUserName", ConfigurationManager.AppSettings["EmailService:UserName"]);
        //    evt.Properties.Add("SmtpPassword", ConfigurationManager.AppSettings["EmailService:Password"]);
        //    evt.Properties.Add("SenderAddress", ConfigurationManager.AppSettings["EmailService:SenderId"]);
        //    evt.Properties.Add("RecipientsAddress", ConfigurationManager.AppSettings["ErrorLog:EmailNotification:Recipients"]);
        //}
    }
}
