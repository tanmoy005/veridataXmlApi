using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Response
{
    public class AppointeePassportValidateResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsValid { get; set; }
        public string? Remarks { get; set; }

    }
}
