namespace VERIDATA.Model.Request
{
    public class ValidateUserSignInRequest
    {
        public string? clientId { get; set; }
        public string? OTP { get; set; }
        public int dbUserType { get; set; }
    }
}