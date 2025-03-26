using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VERIDATA.Model.Response.api.Signzy.Base;

namespace VERIDATA.Model.Response.api.Signzy
{
    public class SignzyGenerateDigilockerUrlResponse : Signzy_BaseResponse
    {
        public DigilockerUrlDetails? result { get; set; }
    }

    public class DigilockerUrlDetails
    {
        public string? requestId { get; set; }
        public string? url { get; set; }
    }
}