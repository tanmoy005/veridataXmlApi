using System.ComponentModel.DataAnnotations;

namespace PfcAPI.Model.RequestModel
{
    public class AppointeeAadhaarValidateByFileRequest
    {
        [Required]
        public int appointeeId { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhaar Number length must be equal 12 characters.")]
        public string aaddharNumber { get; set; }
        public string aaddharName { get; set; }

        //[Required]
        public IFormFile? aaddharFile { get; set; }
        //public string? fileUploaded { get; set; }
        //public string? appointeecode { get; set; }
        //public int UserId { get; set; }
    }
}
