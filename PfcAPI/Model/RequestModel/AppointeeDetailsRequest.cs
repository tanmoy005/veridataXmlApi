using System.ComponentModel.DataAnnotations;

namespace PfcAPI.Model.RequestModel
{
    public class AppointeeDetailsRequest
    {
        public int? AppointeeDetailsId { get; set; }
        [Required]
        public int AppointeeId { get; set; }
        public string? AppointeeCode { get; set; }
        public int CompanyId { get; set; }

        [StringLength(50, ErrorMessage = "Appointee Name length must be less than or equal 50 characters.")]
        public string? AppointeeName { get; set; }
        [EmailAddress]
        public string? AppointeeEmailId { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
        [StringLength(1)]
        public string? Gender { get; set; }
        [Phone]
        public string? MobileNo { get; set; } //number that varified with aadhar

        [StringLength(12, MinimumLength = 12, ErrorMessage = "UAN Number length must be equal 12 characters.")]
        public string? UANNumber { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfJoining { get; set; }

        [StringLength(50, ErrorMessage = "Member Name length must be less than or equal 50 characters.")]
        public string? MemberName { get; set; } //father or husband name

        [StringLength(1, ErrorMessage = "Member Relation Code length must be equal 1 characters.")]
        public string? MemberRelation { get; set; } //Varified with aadhar

        [StringLength(50)]
        public string? Nationality { get; set; }
        public decimal EPFWages { get; set; }

        [StringLength(1)]
        public string? Qualification { get; set; }
        [StringLength(1, ErrorMessage = "Maratial Status Code length must be equal 1 characters.")]
        public string? MaratialStatus { get; set; }
        [StringLength(1)]
        public string? IsInternationalWorker { get; set; }
        public string? OriginCountry { get; set; }
        public string? PassportNo { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PassportValidFrom { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PassportValidTill { get; set; }
        [StringLength(1)]
        public string? IsHandicap { get; set; } // yes="Y",no="N"

        [StringLength(1, ErrorMessage = "Handicape Type Code length must be equal 1 characters.")]
        public string? HandicapeType { get; set; }

        [StringLength(50, ErrorMessage = "Name length must be less than or equal 50 characters.")]
        public string? AadhaarName { get; set; } //Varified with aadhar

        [StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhaar Number length must be equal 12 characters.")]
        public string? AadhaarNumber { get; set; }

        [StringLength(50, ErrorMessage = "Name length must be less than or equal 50 characters.")]
        public string? PANName { get; set; } //Varified with aadhar

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Pan Number length must be equal 10 characters.")]
        [RegularExpression("[A-Z]{5}[0-9]{4}[A-Z]{1}", ErrorMessage = "Pan Number must be properly formatted.")]
        public string? PANNumber { get; set; }
        public bool IsSubmit { get; set; }
        public int UserId { get; set; }
        public List<IFormFile>? FileDetails { get; set; }
        //public IFormFile? FileDetails_2 { get; set; }
        public string? FileUploaded { get; set; }


    }
}
