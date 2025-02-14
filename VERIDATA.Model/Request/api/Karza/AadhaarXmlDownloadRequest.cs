using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Cmp;
using VERIDATA.Model.Request.api.Karza.Base;
using VERIDATA.Model.Response.api.Karza.Base;

namespace VERIDATA.Model.Request.api.Karza
{
    public class AadhaarXmlDownloadRequest : Karza_BaseRequest
    {
        public AadhaarXmlDownloadRequest()
        {
            aadhaarUpdateHistory = "N";
        }

        public string otp { get; set; }
        public string aadhaarNo { get; set; }
        public string aadhaarUpdateHistory { get; set; }
        public string shareCode { get; set; }
        public string requestId { get; set; }
    }
}