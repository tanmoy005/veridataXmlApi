using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Request
{
    public class AppointeeDLValidateRequest
    {
        public string? DLNumber { get; set; }

        //public string? Dob { get; set; }
        public int AppointeeId { get; set; }

        public int UserId { get; set; }
    }
}