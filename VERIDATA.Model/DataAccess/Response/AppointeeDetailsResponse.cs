

namespace VERIDATA.Model.DataAccess.Response
{
    public class AppointeeDetailsResponse
    {
        public int? AppointeeDetailsId { get; set; }
        public int? AppointeeId { get; set; }
        public string? AppointeeCode { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? CandidateId { get; set; }
        public string? AppointeeName { get; set; }
        public string? AppointeeEmailId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? MobileNo { get; set; } //number that varified with aadhar
        public string? UANNumber { get; set; }
        public string? MaskedUANNumber { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string? MemberName { get; set; } //father or husband name
        public string? MemberRelation { get; set; } //Varified with aadhar
        public string? Nationality { get; set; }
        public decimal EPFWages { get; set; }
        public string? Qualification { get; set; }
        public string? MaratialStatus { get; set; }
        public string? isPassportAvailable { get; set; }
        public string? IsInternationalWorker { get; set; }
        public string? OriginCountry { get; set; }
        public string? PassportNo { get; set; }
        public string? MaskedPassportNo { get; set; }
        public DateTime? PassportValidFrom { get; set; }
        public DateTime? PassportValidTill { get; set; }
        public string? PassportFileNo { get; set; }
        public bool? IsPassportValid { get; set; }
        public string? IsHandicap { get; set; } // yes="Y",no="N"
        public string? HandicapeType { get; set; }
        public string? AadhaarName { get; set; } //Varified with aadhar
        public string? NameFromAadhaar { get; set; } //Varified with aadhar
        public string? DobFromAadhaar { get; set; } //Varified with aadhar
        public string? AadhaarNumber { get; set; }
        public string? AadhaarNumberView { get; set; }
        public string? PANName { get; set; } //Varified with aadhar
        public string? FathersNameFromPan { get; set; } //Varified with Pan
        public string? PANNumber { get; set; }
        public string? MaskedPANNumber { get; set; }
        public bool? IsPensionApplicable { get; set; }
        public bool? IsPFverificationReq { get; set; }
        public bool? isPanVarified { get; set; }
        public bool? IsAadhaarVarified { get; set; }
        public bool? IsUanAvailable { get; set; }
        public bool? IsUanVarified { get; set; }
        public bool? IsFnameVarified { get; set; }
        public bool? IsManualPassbook { get; set; }
        public bool? IsTrustPassbook { get; set; }
        public bool? IsProcessed { get; set; }
        public int SaveStep { get; set; }
        public bool IsSubmit { get; set; }
        public int UserId { get; set; }
        public string workFlowStatus { get; set; } 
        //public List<string>? UploadedFileType { get; set; }
        public List<FileDetailsResponse>? FileUploaded { get; set; }
    }
}
