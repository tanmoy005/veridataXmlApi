using System.ComponentModel;

namespace VERIDATA.Model.Response
{
    public class AppointeeAgingDataReportDetails
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

        //[DisplayName("Date Of Offer")]
        //public string? dateOfOffer { get; set; }

        [DisplayName("Link Sent Date")]
        public DateTime? CreatedDate { get; set; }

        [DisplayName("Status")]
        public string? Status { get; set; }

        [DisplayName("Activity at")]
        public DateTime? LastActionDate { get; set; }

        [DisplayName("Activity Desc")]
        public string? LastActivityDesc { get; set; }


    }
}
