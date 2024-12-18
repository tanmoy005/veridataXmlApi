using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Response
{
    public class RawFileDataDetailsResponse
    {
        public int id { get; set; }

        [Required]
        public int companyId { get; set; }

        public int fileId { get; set; }

        //[Required]
        //[StringLength(50, ErrorMessage = "Appointee Name length must be less than or equal 50 characters.")]
        public string? CandidateId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Appointee Name length must be less than or equal 50 characters.")]
        public string? appointeeName { get; set; }

        public string? companyName { get; set; }

        [Required]
        [EmailAddress]
        public string? appointeeEmailId { get; set; }

        public string? mobileNo { get; set; } //number that varified with aadhar
        public bool? isPFverificationReq { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dateOfJoining { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dateOfOffer { get; set; }

        public decimal? epfWages { get; set; }
        public bool? isChecked { get; set; }
        public string? lvl1Email { get; set; }
        public string? lvl2Email { get; set; }
        public string? lvl3Email { get; set; }
        public int? userId { get; set; }
        //public string? Reason { get; set; }
    }
}