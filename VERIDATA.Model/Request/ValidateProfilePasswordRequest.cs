using System.ComponentModel;

namespace VERIDATA.Model.Request
{
    public class ValidateProfilePasswordRequest
    {
        [PasswordPropertyText]
        public string? ProfilePassword { get; set; }
        public int UserId { get; set; }
    }
}
