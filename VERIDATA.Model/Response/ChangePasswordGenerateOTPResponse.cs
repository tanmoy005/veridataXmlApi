using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Response
{
    public class ChangePasswordGenerateOTPResponse
    {
        public string? clientId { get; set; }
        public int? userId { get; set; }
        public int dbUserType { get; set; }
    }
}
