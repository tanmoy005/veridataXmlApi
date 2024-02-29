using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class UserSignInRequest
    {
        [Required]
        [StringLength(15)]
        public string? UserCode { get; set; }

        [Required]
        [StringLength(10)]
        public string? Password { get; set; }

        //[StringLength(50)]
        //public string? IPAddress { get; set; }

        //[StringLength(50)]
        //public string? BrowserName { get; set; }

    }
}
