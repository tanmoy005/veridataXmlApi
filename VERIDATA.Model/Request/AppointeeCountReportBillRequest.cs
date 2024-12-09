using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Request
{
    public class AppointeeCountReportBillRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<int>? EntityId { get; set; }
    }
}
