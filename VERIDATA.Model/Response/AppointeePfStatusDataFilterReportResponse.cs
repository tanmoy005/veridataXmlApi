using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Response
{
    public  class AppointeePfStatusDataFilterReportResponse
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

        [DisplayName("Status")]
        public string? Status { get; set; }

        [DisplayName("Trust Passbook Status")]
        public string? TrustPassBookStatus { get; set; }

        [DisplayName("EPFO Passbook Status")]
        public string? EPFOPassBookStatus { get; set; }

        [DisplayName("Manual Y/N")]
        public string isManual { get; set; }

        //public int? AppointeeId { get; set; }
        //public string? CandidateId { get; set; }
        //public string? AppointeeName { get; set; }
        //public string? EmailId { get; set; }
        //public string? MobileNo { get; set; }
        //public DateTime? DateOfJoining { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public string? Status { get; set; }
        //public bool TrustPassBookStatus { get; set; }
        //public string? EPFOPassBookStatus { get; set; }
        //public bool isManual { get; set; }
    }
}
