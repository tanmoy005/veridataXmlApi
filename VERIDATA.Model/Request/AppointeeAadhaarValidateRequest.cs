using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeeAadhaarValidateRequest
    {
        [Required]
        public int appointeeId { get; set; }

        [Required]
        public int userId { get; set; }

        //[Required]
        //[StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhaar Number length must be equal 12 characters.")]
        public string aadharNumber { get; set; }

        public string aadharName { get; set; }
    }
}