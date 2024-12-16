using System.ComponentModel;

namespace VERIDATA.Model.Response
{
    public class AppointeePfStatusDataFilterReportResponse
    {

        [DisplayName("Candidate Id")]
        public string? candidateId { get; set; }

        [DisplayName("Name")]
        public string? AppointeeName { get; set; }

        [DisplayName("Email")]
        public string? EmailId { get; set; }

        [DisplayName("Mobile No.")]
        public string? MobileNo { get; set; }

        [DisplayName("Date Of Joining")]
        public DateTime? DateOfJoining { get; set; }

        [DisplayName("Link Sent Date")]
        public DateTime? CreatedDate { get; set; }

        [DisplayName("Pension Gap")]
        public string? PensionStatus { get; set; }

        [DisplayName("Trust Passbook Status")]
        public string? TrustPassBookStatus { get; set; }

        [DisplayName("EPFO Passbook Status")]
        public string? EPFOPassBookStatus { get; set; }

        [DisplayName("Manual Passbook")]
        public string? IsManual { get; set; }

        [DisplayName("UAN")]
        public string? UAN { get; set; }

        [DisplayName("Aadhaar No.")]
        public string? AadharNumber { get; set; }

        [DisplayName("UAN Aadhar Link")]
        public string? IsUanAadharLink { get; set; }

        [DisplayName("EPS Membership")]
        public string? isEpsMember { get; set; }

    }
}
