namespace VERIDATA.Model.Configuration
{
    public class EmailConfiguration
    {
        public string? SenderName { get; set; }
        public string? SenderId { get; set; }
        public string? SmtpServer { get; set; }
        public int Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? EnableSSLorTLS { get; set; }
        public bool IsMailSend { get; set; }
        public bool AllowServerAuth { get; set; }
        public string? HostUrl { get; set; }
        public string? HostAdminUrl { get; set; }
        public int ReminderResendLockDuration { get; set; }
        public int ReminderAttempt { get; set; }

    }
}
