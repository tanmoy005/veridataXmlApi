using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Request
{
    public class GetRemarksRemedyRequest
    {
        public int? RemarksId { get; set; }
        public string? RemedyType { get; set; }
        public string? RemedySubType { get; set; }
    }
}
