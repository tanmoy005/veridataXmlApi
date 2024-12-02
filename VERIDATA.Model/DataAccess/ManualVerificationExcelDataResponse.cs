using System.ComponentModel;

namespace VERIDATA.Model.DataAccess
{
    public class ManualVerificationExcelDataResponse
    {
       
        [DisplayName("Candidate Id")]
        public string? candidateId { get; set; }
        
        [DisplayName("Name")]
        public string? appointeeName { get; set; }
        [DisplayName("Email")]
        public string? appointeeEmailId { get; set; }
        [DisplayName("Mobile No.")]
        public string? mobileNo { get; set; } //number that varified with aadhar
        [DisplayName("Date Of Joining")]
        public string? dateOfJoining { get; set; }
        [DisplayName("Submitted")] // Yes/No
        public bool? isDocSubmitted { get; set; }
        [DisplayName("Issue")]
        public bool? isNoIsuueinVerification { get; set; } //  Yes/No
        [DisplayName("Status")]
        public string? Status { get; set; }
        
    }
}
