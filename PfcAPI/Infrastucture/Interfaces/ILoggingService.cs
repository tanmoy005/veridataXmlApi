
namespace PfcAPI.Infrastucture.Interfaces
{
    public interface ILoggingService
    {
        void Debug(string message);

        void DebugIf(Func<bool> condition, string message);

        void Info(string message);

        //void ServiceCommunicationInfo(EntityServiceCommunicationLog serviceCommunicationLog);

        //void ServiceCommunicationInfo(EntityServiceCommunicationLog serviceCommunicationLog, char logLevel);

        void Trace(string message);

        void TraceIf(Func<bool> condition, string message);

        void Warn(string message);

        void WarnIf(Func<bool> condition, string message);

        void Error(Exception ex, string arguments, string url);

        //void ScheduleJobLogger(ScheduleJobLogEntity scheduleJobLogEntity);

        //void ScheduleJobActivityLogger(ScheduleJobActivityLogEntity scheduleJobActivityLogEntity, LogLevel logLevel);
    }
}
