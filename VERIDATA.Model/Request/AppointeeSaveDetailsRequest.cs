using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using static VERIDATA.Model.Extensions.CustomModelValidation;

namespace VERIDATA.Model.Request
{
    public class AppointeeSaveDetailsRequest
    {
        public int? AppointeeDetailsId { get; set; }

        [Required]
        public int AppointeeId { get; set; }

        public string? AppointeeCode { get; set; }
        public string? CandidateId { get; set; }

        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }

        [RequiredIfSubmitString]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Appointee Name length must be less than or equal 50 characters.")]
        public string? AppointeeName { get; set; }

        [DisplayName("Email")]
        [EmailAddress]
        [RequiredIfSubmitString]
        public string? AppointeeEmailId { get; set; }

        [DisplayName("Date Of Birth")]
        [Min18Years]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [RequiredIfSubmitString]
        [StringLength(1)]
        public string? Gender { get; set; }

        [DisplayName("Mobile No")]
        [Phone]
        [RequiredIfSubmitString]
        public string? MobileNo { get; set; } //number that varified with aadhar

        [RequiredIfSubmit]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfJoining { get; set; }

        [DisplayName("Member Name")]
        [RequiredIfSubmitString]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Member Name length must be less than or equal 50 characters.")]
        public string? MemberName { get; set; } //father or husband name

        [DisplayName("Member Relation")]
        [RequiredIfSubmitString]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "Member Relation Code length must be equal 1 characters.")]
        public string? MemberRelation { get; set; } //Varified with aadhar

        [RequiredIfSubmitString]
        [StringLength(50)]
        public string? Nationality { get; set; }

        //[RequiredIfSubmit]
        public decimal EPFWages { get; set; }

        [StringLength(1)]
        public string? Qualification { get; set; }

        [DisplayName("Maratial Status")]
        [RequiredIfSubmitString]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "Maratial Status Code length must be equal 1 characters.")]
        public string? MaratialStatus { get; set; }

        [DisplayName("Is Passport Available")]
        [RequiredIfSubmitString]
        [StringLength(1, MinimumLength = 1)]
        public string? IsPassportAvailable { get; set; }

        [DisplayName("International Worker")]
        [RequiredIfPassportString]
        [StringLength(1, MinimumLength = 1)]
        public string? IsInternationalWorker { get; set; }

        [RequiredIfPassportString]
        public string? OriginCountry { get; set; }

        [DisplayName("Passport Number")]
        [RequiredIfPassportString]
        public string? PassportNo { get; set; }

        [DisplayName("Passport Valid From")]
        [RequiredIfPassport]
        [Pastdate]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PassportValidFrom { get; set; }

        [DisplayName("Passport Valid Till")]
        [RequiredIfPassport]
        [Futuredate]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PassportValidTill { get; set; }

        [StringLength(1)]
        [DisplayName("Is Physically handicap")]
        [RequiredIfSubmitString]
        public string? IsHandicap { get; set; } // yes="Y",no="N"

        [DisplayName("Handicap Type")]
        [RequiredIfHandicap]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "Handicap Type Code length must be equal 1 characters.")]
        public string? HandicapeType { get; set; }

        [DisplayName("PF Verification Required")]
        public bool? IsPFverificationReq { get; set; }

        //[RequiredIfSubmitString]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be less than or equal 50 characters.")]
        //public string? AadhaarName { get; set; } //Varified with aadhar

        //[RequiredIfSubmitString]
        //[StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhaar Number length must be equal 12 characters.")]
        //public string? AadhaarNumber { get; set; }

        //[DisplayName("PAN Name")]
        //[RequiredIfSubmitString]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be less than or equal 50 characters.")]
        //public string? PANName { get; set; } //Varified with aadhar

        //[DisplayName("PAN Number")]
        //[RequiredIfSubmitString]
        //[StringLength(10, MinimumLength = 10, ErrorMessage = "PAN Number length must be equal 10 characters.")]
        //[RegularExpression("[A-Z]{5}[0-9]{4}[A-Z]{1}", ErrorMessage = "Pan Number must be properly formatted.")]
        //public string? PANNumber { get; set; }
        public bool? IsAadhaarVarified { get; set; }

        public bool? IsUanVarified { get; set; }
        public bool IsSubmit { get; set; }
        public int UserId { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public string? FileUploaded { get; set; }
    }
}