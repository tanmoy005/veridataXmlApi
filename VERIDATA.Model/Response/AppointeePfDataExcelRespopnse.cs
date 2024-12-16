using System.ComponentModel;

namespace VERIDATA.Model.Response
{
    public class AppointeePfDataExcelRespopnse
    {
        [DisplayName("Candidate ID")]
        public string? CandidateId { get; set; }
        [DisplayName("Appointee Name")]
        public string? AppointeeName { get; set; }
        [DisplayName("Email")]
        public string? AppointeeEmailId { get; set; }
        [DisplayName("Mobile No")]
        public string? MobileNo { get; set; }
        [DisplayName("Date of Joining")]
        public string? DateOfJoining { get; set; }
        [DisplayName("Trust Passbook")]
        public string? IsTrustPassbook { get; set; }
        [DisplayName("EPFO Passbook")]
        public string? Status { get; set; }
        [DisplayName("Verification Type")]
        public string? IsManualPassbook { get; set; }
        [DisplayName("UAN")]
        public string? Uan { get; set; }
        [DisplayName("Aadhaar No")]
        public string? AadhaarNumberView { get; set; }
        [DisplayName("EPS Gap")]
        public string? PensionGapIdentified { get; set; }

        [DisplayName("Aadhar UAN Linked")]
        public string? AadharuanLink { get; set; }
        [DisplayName("EPS Membership")]
        public string EpsMember { get; set; }
    }
}
