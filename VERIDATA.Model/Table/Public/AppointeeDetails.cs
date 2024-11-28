using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Public
{
    [Table("appointee_details")]
    public class AppointeeDetails
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("appointee_details_id", TypeName = DbDataType._biginteger)]
        public int AppointeeDetailsId { get; set; }

        [Required]
        //[ForeignKey("AppointeeProcess")]
        [Column("appointee_id", TypeName = DbDataType._biginteger)]
        public int? AppointeeId { get; set; }

        [Required]
        [Column("company_id", TypeName = DbDataType._integer)]
        // [ForeignKey("company_id")]
        public int CompanyId { get; set; }

        [Column("company_name", TypeName = DbDataType._text50)]
        public string? CompanyName { get; set; }

        [Column("candidate_id", TypeName = DbDataType._text100)]
        public string? CandidateId { get; set; }

        [Column("appointee_name", TypeName = DbDataType._text100)]
        public string? AppointeeName { get; set; }

        [Column("appointee_email", TypeName = DbDataType._text50)]
        public string? AppointeeEmailId { get; set; }

        [Column("date_of_birth", TypeName = DbDataType._datetime)]
        public DateTime? DateOfBirth { get; set; }

        [Column("gender", TypeName = DbDataType._char)]
        public string? Gender { get; set; }

        [Column("mobile_no", TypeName = DbDataType._text20)]
        public string? MobileNo { get; set; } //number that varified with aadhar

        [Column("uan_number", TypeName = DbDataType._text100)]
        public string? UANNumber { get; set; }

        [Column("joining_date", TypeName = DbDataType._datetime)]
        public DateTime? DateOfJoining { get; set; }

        [Column("member_name", TypeName = DbDataType._text50)]
        public string? MemberName { get; set; } //father or husband name

        [Column("member_relation", TypeName = DbDataType._char)]
        public string? MemberRelation { get; set; } //Varified with aadhar

        [Column("nationality", TypeName = DbDataType._text50)]
        public string? Nationality { get; set; }

        [Column("epf_wages", TypeName = DbDataType._numeric)]
        public decimal EPFWages { get; set; }

        [Column("qualification", TypeName = DbDataType._char)]
        public string? Qualification { get; set; }

        [Column("maratialstatus", TypeName = DbDataType._char)]
        public string? MaratialStatus { get; set; }

        [Column("is_passportAvailable", TypeName = DbDataType._char)]
        public string? IsPassportAvailable { get; set; }

        [Column("is_internationalworker", TypeName = DbDataType._char)]
        public string? IsInternationalWorker { get; set; }

        [Column("origincountry", TypeName = DbDataType._text50)]
        public string? OriginCountry { get; set; }

        [Column("passport_no", TypeName = DbDataType._text200)]
        public string? PassportNo { get; set; }

        [Column("passport_validfrom", TypeName = DbDataType._datetime)]
        public DateTime? PassportValidFrom { get; set; }

        [Column("passport_validtill", TypeName = DbDataType._datetime)]
        public DateTime? PassportValidTill { get; set; }

        [Column("passport_fileno", TypeName = DbDataType._text50)]
        public string? PassportFileNo { get; set; }

        [Column("is_handicap", TypeName = DbDataType._char)]
        public string? IsHandicap { get; set; } // yes="Y",no="N"

        [Column("handicape_type", TypeName = DbDataType._text50)]
        public string? HandicapeType { get; set; }

        //[Column("handicape_Name", TypeName = DbDataType._text50)]
        //public string? HandicapeName { get; set; }

        [Column("is_pf_verification_req", TypeName = DbDataType._boolean)]
        public bool? IsPFverificationReq { get; set; }

        [Column("aadhaar_name", TypeName = DbDataType._text100)]
        public string? AadhaarName { get; set; } //Varified with aadhar

        [Column("name_from_aadhaar", TypeName = DbDataType._text100)]
        public string? NameFromAadhaar { get; set; } //name from aadhar

        [Column("dob_from_aadhaar", TypeName = DbDataType._text20)]
        public string? DobFromAadhaar { get; set; } //date of birth from aadhar

        [Column("gender_from_aadhaar", TypeName = DbDataType._text10)]
        public string? GenderFromAadhaar { get; set; } //Gender from aadhar

        [Column("aadhaar_number", TypeName = DbDataType._text100)]
        public string? AadhaarNumber { get; set; }

        [Column("aadhaar_number_view", TypeName = DbDataType._text50)]
        public string? AadhaarNumberView { get; set; }

        [Column("pan_name", TypeName = DbDataType._text100)]
        public string? PANName { get; set; } //Varified with aadhar

        [Column("fathers_name_from_pan", TypeName = DbDataType._text100)]
        public string? FathersNameFromPan { get; set; } //Gender from aadhar

        [Column("pan_number", TypeName = DbDataType._text100)]
        public string? PANNumber { get; set; }

        [Column("is_processed", TypeName = DbDataType._boolean)]
        [DefaultValue(false)]
        public bool? IsProcessed { get; set; }

        [Column("is_pensionapplicable", TypeName = DbDataType._boolean)]
        [DefaultValue(false)]
        public bool? IsPensionApplicable { get; set; }

        [Column("is_pensiongap", TypeName = DbDataType._boolean)]
        [DefaultValue(false)]
        public bool? IsPensionGap { get; set; }

        [Column("is_aadhaarvarified", TypeName = DbDataType._boolean)]
        [DefaultValue(false)]
        public bool? IsAadhaarVarified { get; set; }

        [Column("is_passportvarified", TypeName = DbDataType._boolean)]
        [DefaultValue(false)]
        public bool? IsPasssportVarified { get; set; }

        [Column("is_fnamevarified", TypeName = DbDataType._boolean)]

        [DefaultValue(false)]
        public bool? IsFNameVarified { get; set; }

        [Column("is_uanvarified", TypeName = DbDataType._boolean)]

        [DefaultValue(false)]
        public bool? IsUanVarified { get; set; }

        [Column("is_manual_passbook", TypeName = DbDataType._boolean)]

        [DefaultValue(false)]
        public bool? IsManualPassbook { get; set; }

        [Column("is_panvarified", TypeName = DbDataType._boolean)]

        [DefaultValue(false)]
        public bool? IsPanVarified { get; set; }

        [Column("is_trustpassbook", TypeName = DbDataType._boolean)]

        [DefaultValue(false)]
        public bool? IsTrustPassbook { get; set; }

        [Column("save_step", TypeName = DbDataType._integer)]
        public int? SaveStep { get; set; }

        [Column("process_status", TypeName = DbDataType._integer)]

        [DefaultValue(false)]
        public int? ProcessStatus { get; set; }

        [Column("level1_email", TypeName = DbDataType._text50)]
        public string? lvl1Email { get; set; }

        [Column("level2_email", TypeName = DbDataType._text50)]
        public string? lvl2Email { get; set; }

        [Column("level3_email", TypeName = DbDataType._text50)]
        public string? lvl3Email { get; set; }

        [Column("active_status", TypeName = DbDataType._boolean)]
        [DefaultValue(true)]
        public bool? ActiveStatus { get; set; }

        [Column("is_save", TypeName = DbDataType._boolean)]
        [DefaultValue(false)]
        public bool? IsSave { get; set; }

        [Column("is_submit", TypeName = DbDataType._boolean)]
        [DefaultValue(false)]
        public bool? IsSubmit { get; set; }

        [Column("is_offline_kyc", TypeName = DbDataType._boolean)]
        [DefaultValue(false)]
        public bool? IsOfflineKyc { get; set; }

        [Column("created_by", TypeName = DbDataType._integer)]
        public int? CreatedBy { get; set; }

        [Column("created_on", TypeName = DbDataType._datetime)]
        public DateTime? CreatedOn { get; set; }

        [Column("updated_by", TypeName = DbDataType._integer)]
        public int? UpdatedBy { get; set; }

        [Column("updated_on", TypeName = DbDataType._datetime)]
        public DateTime? UpdatedOn { get; set; }
        [Column("is_uan_available", TypeName = DbDataType._boolean)]
        public bool? IsUanAvailable { get; set; }

        [Column("is_passbook_fetch", TypeName = DbDataType._boolean)]
        [DefaultValue(false)]
        public bool? IsPassbookFetch { get; set; }
        [Column("is_uan_aadhar_link", TypeName = DbDataType._boolean)]
        [DefaultValue(false)]
        public bool? IsUanAadharLink { get; set; }
    }
}
