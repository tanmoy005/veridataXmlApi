using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class SetNewPasswordRequest
    {
        [PasswordPropertyText]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,12}$", ErrorMessage = "Password is not in Correct format")]
        public string? Password { get; set; }

        public int UserId { get; set; }
        public string? clientId { get; set; }
        public string? otp { get; set; }
    }
}