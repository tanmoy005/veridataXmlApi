using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VERIDATA.Model.Request.api.Karza.Base;

namespace VERIDATA.Model.Request.api.Karza
{
    public class Karza_GetCandidateFirDetailsRequest : Karza_BaseRequest
    {
        public Karza_GetCandidateFirDetailsRequest()
        {
            fuzziness = false;
            advancedFuzziness = false;
            confidenceLevel = ["high", "medium", "low"];
            entityRelation = "b";
        }

        public string candidateName { get; set; }
        public string relativeName { get; set; }
        public string address { get; set; }
        public bool fuzziness { get; set; }
        public bool advancedFuzziness { get; set; }
        public List<string> stateCode { get; set; }
        public string district { get; set; }
        public string entityRelation { get; set; }
        public string dob { get; set; }
        public string contactNo { get; set; }
        public List<string> confidenceLevel { get; set; }
        public ClientData clientData { get; set; }
    }
}