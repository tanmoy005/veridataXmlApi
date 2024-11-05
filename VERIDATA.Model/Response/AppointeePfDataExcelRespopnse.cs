using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Response
{
    public class AppointeePfDataExcelRespopnse
    {
        [DisplayName("Candidate Id")]
        public string? CandidateId { get; set; }
        [DisplayName("Appointee Name")]
        public string? AppointeeName { get; set; }
        [DisplayName("Email")]
        public string? AppointeeEmailId { get; set; }
        [DisplayName("Mobile No")]
        public string? MobileNo { get; set; }
        [DisplayName("Date of Joining")]
        public DateTime? DateOfJoining { get; set; }
        [DisplayName("EPFO")]
        public string? Status { get; set; }
        [DisplayName("Trust Passbook")]
        public bool? IsTrustPassbook { get; set; }
        [DisplayName("UAN")]
        public string? Uan { get; set; }
        [DisplayName("Aadhaar No")]
        public string? AadhaarNumberView { get; set; }
        [DisplayName("Manual Passbook")]
        public bool? IsManualPassbook { get; set; }
        [DisplayName("Pension Gap")]
        public bool? PensionGapIdentified { get; set; }
    }
}
