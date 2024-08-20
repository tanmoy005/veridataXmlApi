using System.ComponentModel;

namespace VERIDATA.Model.Request
{
    public class SetNewPasswordRequest
    {
        [PasswordPropertyText]
        public string? Password { get; set; }
        public int UserId { get; set; }
        public string? clientId { get; set; }
        public string? otp { get; set; }
    }
}
