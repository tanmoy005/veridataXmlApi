using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Request
{
    public class DoctypeAvailibilityUpdateRequest
    {
        public int AppointeeId { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public bool Value { get; set; }
    }
}
