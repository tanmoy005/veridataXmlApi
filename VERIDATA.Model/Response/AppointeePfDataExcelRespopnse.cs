﻿using System;
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
        public string? DateOfJoining { get; set; }
        [DisplayName("Trust Passbook")]
        public string? IsTrustPassbook { get; set; }
        [DisplayName("EPFO Passbook")]
        public string? Status { get; set; }
        [DisplayName("Manual Passbook")]
        public string? IsManualPassbook { get; set; }
        [DisplayName("UAN")]
        public string? Uan { get; set; }
        [DisplayName("Aadhaar No")]
        public string? AadhaarNumberView { get; set; }
        [DisplayName("Pension Gap")]
        public string? PensionGapIdentified { get; set; }
    }
}
