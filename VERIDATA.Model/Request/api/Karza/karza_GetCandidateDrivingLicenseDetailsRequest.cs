using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VERIDATA.Model.Request.api.Karza.Base;

namespace VERIDATA.Model.Request.api.Karza
{
    public class karza_GetCandidateDrivingLicenseDetailsRequest : Karza_BaseRequest
    {
        public karza_GetCandidateDrivingLicenseDetailsRequest()
        {
            additionalDetails = true;
        }

        public string? dlNo { get; set; }
        public string? dob { get; set; }
        public bool additionalDetails { get; set; }
    }
}