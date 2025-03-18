using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VERIDATA.Model.Response.api.Signzy.Base;

namespace VERIDATA.Model.Request.api.Signzy
{
    public class Signzy_GetCandidateBankDetailsRequest
    {
        public Signzy_GetCandidateBankDetailsRequest()
        {
            nameMatchScore = "0.9";
            nameFuzzy = "true";
        }

        public string? beneficiaryAccount { get; set; }
        public string? beneficiaryIFSC { get; set; }
        public string? beneficiaryMobile { get; set; }
        public string? beneficiaryName { get; set; }
        public string? nameMatchScore { get; set; }
        public string? nameFuzzy { get; set; }
    }
}