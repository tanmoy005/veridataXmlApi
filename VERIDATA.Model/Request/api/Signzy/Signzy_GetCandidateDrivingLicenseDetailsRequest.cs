using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VERIDATA.Model.Response.api.Signzy.Base;

namespace VERIDATA.Model.Request.api.Signzy
{
    public class Signzy_GetCandidateDrivingLicenseDetailsRequest
    {
        public string? number { get; set; }
        public string? dob { get; set; }
    }
}