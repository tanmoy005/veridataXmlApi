﻿using System.ComponentModel;

namespace VERIDATA.Model.Response
{
    public class AppointeeAgingDataReportDetails
    {
        [DisplayName("Candidate ID")]
        public string? candidateId { get; set; }

        [DisplayName("Appointee ID")]
        public int? AppointeeId { get; set; }

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

        [DisplayName("Status")]
        public string? Status { get; set; }

        [DisplayName("Last Activity at")]
        public DateTime? LastActionDate { get; set; }

        [DisplayName("Last Activity Info")]
        public string? LastActivityDesc { get; set; }
    }
}