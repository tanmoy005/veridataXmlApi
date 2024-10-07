using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.DataAccess
{
    public class EmployementHistoryDetails
    {
        public string? EmployementData { get; set; }
        public int AppointeeId { get; set; }
        public int UserId { get; set; }
        public string? Provider { get; set; }
        public string? SubType { get; set; }
    }
}
