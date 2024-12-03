using System.ComponentModel;

namespace VERIDATA.Model.Response
{
    public class ProcessedDataReportDetailsResponse
    {
        [DisplayName("Candidate ID")]
        public string? CandidateId { get; set; }

        [DisplayName("Name")]
        public string? AppointeeName { get; set; }
        [DisplayName("Email")]
        public string? EmailId { get; set; }

        [DisplayName("Date Of Birth")]
        public string? DateOfBirth { get; set; }

        [DisplayName("Gender Name")]
        public string? GenderName { get; set; }

        [DisplayName("Mobile No.")]
        public string? MobileNo { get; set; } //number that varified with aadhar

        [DisplayName("UAN")]
        public string? UANNumber { get; set; }

        [DisplayName("Aadhar UAN Linked")]
        public string? AadharUANLink { get; set; }

        [DisplayName("EPS Membership")]
        public string? PensionAvailable { get; set; }
        
        [DisplayName("EPS Gap")]
        public string? PensionGapAvailable { get; set; }

        [DisplayName("Date Of Joining")]
        public string? DateOfJoining { get; set; }

        [DisplayName("Husband/Father Name")]
        public string? MemberName { get; set; } //father or husband name

        [DisplayName("Relation")]
        public string? MemberRelationName { get; set; }

        [DisplayName("Nationality")]
        public string? Nationality { get; set; }

        [DisplayName("Qualification")]
        public string? QualificationName { get; set; }

        [DisplayName("Maratial Status")]
        public string? MaratialStatusName { get; set; }

        [DisplayName("Is International Worker")]
        public string? IsInternationalWorker { get; set; }

        [DisplayName("Origin Country")]
        public string? OriginCountry { get; set; }

        [DisplayName("Passport No")]
        public string? PassportNo { get; set; }

        [DisplayName("Passport Valid From")]
        public string? PassportValidFrom { get; set; }

        [DisplayName("Passport Valid Till")]
        public string? PassportValidTill { get; set; }

        [DisplayName("Is Handicap")]
        public string? IsHandicap { get; set; } // yes="Y",no="N"

        [DisplayName("Handicape Type")]
        public string? HandicapeType { get; set; }
        
        [DisplayName("Aadhaar Name")]
        public string? AadhaarName { get; set; } //Varified with aadhar

        [DisplayName("Aadhaar No.")]
        public string? AadhaarNumber { get; set; }

        [DisplayName("PAN Name")]
        public string? PANName { get; set; } //Varified with aadhar

        [DisplayName("PAN No.")]
        public string? PANNumber { get; set; }
        
        [DisplayName("Verification Type")]
        public string? VerificationType { get; set; }

        [DisplayName("Remarks")]
        public string? Remarks { get; set; }
    }
}
