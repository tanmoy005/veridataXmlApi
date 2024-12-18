using System.ComponentModel;

namespace VERIDATA.Model.Response
{
    public class AppointeeAgingDataExcelReportDetails
    {
        [DisplayName("Candidate ID")]
        public string? candidateId { get; set; }

        [DisplayName("Name")]
        public string? AppointeeName { get; set; }

        [DisplayName("Email")]
        public string? EmailId { get; set; }

        [DisplayName("Mobile No.")]
        public string? MobileNo { get; set; }

        [DisplayName("Date Of Joining")]
        public string? DateOfJoining { get; set; }

        [DisplayName("Link Sent Date")]
        public string? CreatedDate { get; set; }

        [DisplayName("Status")]
        public string? Status { get; set; }

        [DisplayName("Last Activity Date")]
        public string? LastActionDate { get; set; }

        [DisplayName("Last Activity Info")]
        public string? LastActivityDesc { get; set; }
    }
}