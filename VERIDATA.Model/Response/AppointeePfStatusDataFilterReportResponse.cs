using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Response
{
    public class AppointeePfStatusDataFilterReportResponse
    {
        [DisplayName("Appointee Id")]
        public int? AppointeeId { get; set; }

        [DisplayName("Candidate Id")]
        public string? candidateId { get; set; }

        [DisplayName("Appointee Name")]
        public string? AppointeeName { get; set; }

        [DisplayName("Email")]
        public string? EmailId { get; set; }

        [DisplayName("Mobile No")]
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

        [DisplayName("Aadhar Number")]
        public string? AadharNumber { get; set; }

        [DisplayName("uan_aadhar_link")]
        public string? IsUanAadharLink { get; set; }

        [DisplayName("EPS_membership")]
        public string? isEpsMember { get; set; }

    }
}
