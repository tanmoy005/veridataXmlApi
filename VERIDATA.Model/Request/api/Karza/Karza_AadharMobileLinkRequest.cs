using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VERIDATA.Model.Request.api.Karza.Base;

namespace VERIDATA.Model.Request.api.Karza
{
    public class Karza_AadharMobileLinkRequest : Karza_BaseRequest
    {
        public string? mobile { get; set; }
        public string? aadhaar { get; set; }
    }
}
