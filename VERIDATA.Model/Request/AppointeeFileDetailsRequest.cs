using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeeFileDetailsRequest
    {
        [Required]
        public int? AppointeeDetailsId { get; set; }
        [Required]
        public int AppointeeId { get; set; }
        public string? AppointeeCode { get; set; }
        public bool? TrustPassbookAvailable { get; set; }
        //[DisplayName("PassportFile Number")]
        ////[RequiredIfSubmitString]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "PassportFile Number length must be less than or equal 50 characters.")]
        //public string? PassportFileNo { get; set; } //Varified with aadhar

        //[DisplayName("PAN Name")]
        ////[RequiredIfSubmitString]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be less than or equal 50 characters.")]
        //public string? PANName { get; set; } //Varified with aadhar

        //[DisplayName("PAN Number")]
        ////[RequiredIfSubmitString]
        //[StringLength(10, MinimumLength = 10, ErrorMessage = "PAN Number length must be equal 10 characters.")]
        //[RegularExpression("[A-Z]{5}[0-9]{4}[A-Z]{1}", ErrorMessage = "Pan Number must be properly formatted.")]
        //public string? PANNumber { get; set; }

        //[StringLength(12, MinimumLength = 12, ErrorMessage = "UAN Number length must be equal 12 characters.")]
        //public string? UANNumber { get; set; }

        ////[RequiredAadhaarIfSubmit]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be less than or equal 50 characters.")]
        //public string? AadhaarName { get; set; } //Varified with aadhar

        ////[RequiredAadhaarIfSubmit]
        //[StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhaar Number length must be equal 12 characters.")]
        //public string? AadhaarNumber { get; set; }

        [Required]
        public int UserId { get; set; }
        public List<IFormFile>? FileDetails { get; set; }
        public string? FileUploaded { get; set; }
        public bool? IsSubmit { get; set; }

    }
}
