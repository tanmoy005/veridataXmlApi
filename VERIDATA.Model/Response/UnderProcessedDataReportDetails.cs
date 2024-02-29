using System.ComponentModel;

namespace VERIDATA.Model.Response
{
    public class UnderProcessedDataReportDetails
    {
        [DisplayName("Candidate Id")]
        public string? candidateId { get; set; }

        [DisplayName("Appointee Name")]
        public string? AppointeeName { get; set; }

        [DisplayName("Email")]
        public string? EmailId { get; set; }

        [DisplayName("Mobile No")]
        public string? mobileNo { get; set; }

        [DisplayName("Date Of Joining")]
        public string? dateOfJoining { get; set; }

        //[DisplayName("Date Of Offer")]
        //public string? dateOfOffer { get; set; }

        [DisplayName("Link Sent Date")]
        public string? CreatedDate { get; set; }

        [DisplayName("Status")]
        public string? Status { get; set; }
    }
}
