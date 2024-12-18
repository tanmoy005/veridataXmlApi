using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeeUANSubmitOtpRequest
    {
        [Required]
        public int appointeeId { get; set; }

        [Required]
        public string AppointeeCode { get; set; }

        [Required]
        public int userId { get; set; }

        [Required]
        public string client_id { get; set; }

        [Required]
        public string otp { get; set; }
    }
}