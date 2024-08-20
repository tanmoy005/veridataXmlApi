namespace VERIDATA.Model.Configuration
{
    public class ApiConfiguration
    {
        public string? EncriptKey { get; set; }
        public string? ApiBaseUrl { get; set; }
        public string? ApiToken { get; set; }
        public string? AuthTokenSignzy { get; set; }
        public string? ApiKey { get; set; }
        public string? ApiKeyValue { get; set; }
        //public string? ApiProvider { get; set; }
        public bool IsNotVarifiedDataSubmit { get; set; }
        public bool IsApiCall { get; set; }
        public bool? ApiDataLog { get; set; }
        //public bool IsValidOtp { get; set; }
        public int ProfileLockDuration { get; set; }
        public int OtpExpiryDuration { get; set; }
        public int WrongOtpAttempt { get; set; }
        public int PasswordExpiryDays { get; set; }
    }
}
