using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class GetUanNumberDetailsRequest
    {
        [Required]
        public int appointeeId { get; set; }

        [Required]
        public int userId { get; set; }

        //[Required]
        //[StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhaar Number length must be equal 12 characters.")]
        public string? aaddharNumber { get; set; }

        public string? panNumber { get; set; }
        public string? mobileNumber { get; set; }
    }
}