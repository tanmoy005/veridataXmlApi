using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Request
{
    public class AppointeeBankValidateRequest
    {
        public string? AccountNumber { get; set; }
        public string? Ifsc { get; set; }
        public int UserId { get; set; }
        public int AppointeeId { get; set; }
    }
}