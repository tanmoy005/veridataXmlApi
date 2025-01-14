using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Response
{
    public class LapsedDataReportDetails
    {
        [DisplayName("Candidate ID")]
        public string? candidateId { get; set; }

        [DisplayName("Name")]
        public string? AppointeeName { get; set; }

        [DisplayName("Email")]
        public string? EmailId { get; set; }

        [DisplayName("Mobile No.")]
        public string? mobileNo { get; set; }

        [DisplayName("Date of Joining")]
        public string? dateOfJoining { get; set; }

        [DisplayName("Link Sent Date")]
        public string? CreatedDate { get; set; }

        [DisplayName("Status")]
        public string? Status { get; set; }

        //[DisplayName("Verification Type")]
        //public string? verificationType { get; set; }
    }
}
