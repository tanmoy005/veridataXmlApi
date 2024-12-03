using System.ComponentModel;

namespace VERIDATA.Model.Response
{
    public class RejectedDataReportDetailsResponse
    {
        [DisplayName("Candidate ID")]
        public string? CandidateId { get; set; }

        [DisplayName("Appointee Name")]
        public string? AppointeeName { get; set; }

        [DisplayName("Email")]
        public string? EmailId { get; set; }

        [DisplayName("Date Of Birth")]
        public string? DateOfBirth { get; set; }

        [DisplayName("Mobile No")]
        public string? MobileNo { get; set; }

        [DisplayName("UAN Number")]
        public string? UANNumber { get; set; }

        [DisplayName("Date Of Joining")]
        public string? DateOfJoining { get; set; }

        [DisplayName("Nationality")]
        public string? Nationality { get; set; }

        [DisplayName("Aadhaar Number")]
        public string? AadhaarNumber { get; set; }

        [DisplayName("PAN Number")]
        public string? PANNumber { get; set; }

        [DisplayName("Remarks")]
        public string? Remarks { get; set; }
    }
}
