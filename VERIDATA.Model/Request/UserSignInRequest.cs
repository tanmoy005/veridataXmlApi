using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class UserSignInRequest
    {
        [Required]
        [StringLength(15, ErrorMessage = "Invalid Username")]
        public string? UserCode { get; set; }

        [Required]
        [StringLength(12)]
        public string? Password { get; set; }

        //[StringLength(50)]
        //public string? IPAddress { get; set; }

        //[StringLength(50)]
        //public string? BrowserName { get; set; }

    }
}
