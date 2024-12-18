using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class UanGenerateOtpRequest
    {
        [Required]
        public int appointeeId { get; set; }

        [Required]
        public int userId { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Uan Number length must be equal 12 characters.")]
        public string? UanNumber { get; set; }

        public string? MobileNumber { get; set; }
    }
}