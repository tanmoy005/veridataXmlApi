using System.ComponentModel;

namespace VERIDATA.Model.Response
{
    public class PfCreateAppointeeDetailsResponse
    {
        [DisplayName("Candidate ID")]
        public string? CandidateId { get; set; }

        [DisplayName("Name")]
        public string? AppointeeName { get; set; }
        [DisplayName("Email")]
        public string? EmailId { get; set; }

        [DisplayName("Date Of Birth")]
        public string? DateOfBirth { get; set; }

        [DisplayName("Gender")]
        public string? Gender { get; set; }


        [DisplayName("Mobile No.")]
        public string? MobileNo { get; set; } //number that varified with aadhar

        [DisplayName("UAN")]
        public string? UANNumber { get; set; }

        [DisplayName("Date Of Joining")]
        public string? DateOfJoining { get; set; }

        [DisplayName("Husband/Father Name")]
        public string? MemberName { get; set; } //father or husband name

        [DisplayName("Relation")]
        public string? MemberRelation { get; set; }


        [DisplayName("Nationality")]
        public string? Nationality { get; set; }

        [DisplayName("EPF Wages")]
        public decimal EPFWages { get; set; }

        [DisplayName("Qualification")]
        public string? Qualification { get; set; }



        [DisplayName("Maratial Status")]
        public string? MaratialStatus { get; set; }


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
    }
}
