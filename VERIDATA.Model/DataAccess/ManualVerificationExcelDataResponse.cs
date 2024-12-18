using System.ComponentModel;

namespace VERIDATA.Model.DataAccess
{
    public class ManualVerificationExcelDataResponse
    {
        [DisplayName("Candidate ID")]
        public string? candidateId { get; set; }

        [DisplayName("Name")]
        public string? appointeeName { get; set; }

        [DisplayName("Email")]
        public string? appointeeEmailId { get; set; }

        [DisplayName("Mobile No.")]
        public string? mobileNo { get; set; } //number that varified with aadhar

        [DisplayName("Date Of Joining")]
        public string? dateOfJoining { get; set; }

        //[DisplayName("Submitted")] // Yes/No
        //public bool? isDocSubmitted { get; set; }
        //[DisplayName("Issue")]
        //public bool? isNoIsuueinVerification { get; set; } //  Yes/
        [DisplayName("Link Sent Date")]
        public string? linkSentDate { get; set; }

        [DisplayName("Status")]
        public string? status { get; set; }

        [DisplayName("Remarks")]
        public string? remarks { get; set; }
    }
}