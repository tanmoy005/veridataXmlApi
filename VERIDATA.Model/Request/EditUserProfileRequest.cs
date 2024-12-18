using System.ComponentModel;

namespace VERIDATA.Model.Request
{
    public class EditUserProfileRequest
    {
        [PasswordPropertyText]
        public string? ProfilePassword { get; set; }

        public int UserId { get; set; }
    }
}