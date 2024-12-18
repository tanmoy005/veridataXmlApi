using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeeAadhaarSubmitOtpRequest
    {
        [Required]
        public int appointeeId { get; set; }

        [Required]
        public int userId { get; set; }

        [Required]
        public string? client_id { get; set; }

        [Required]
        public string? otp { get; set; }

        [Required]
        public string? aadharName { get; set; }
    }
}