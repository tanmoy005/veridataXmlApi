using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.DataAccess.Response
{
    public class PfStatusDataFilterQueryResponse
    {

        public int ProcessedId { get; set; }
        public int? AppointeeId { get; set; }

        public string? CandidateId { get; set; }

        public string? AppointeeName { get; set; }

        public string? AppointeeEmailId { get; set; }

        public string? MobileNo { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? Status { get; set; }

        public bool? IsTrustPassbook { get; set; }

        public string? Uan { get; set; }
        public string? AadhaarNumberView { get; set; }

        public bool? IsManualPassbook { get; set; }
        public bool? PensionGapIdentified { get; set; }
        public bool? IsUanAadharLink { get; set; }
    }
}
