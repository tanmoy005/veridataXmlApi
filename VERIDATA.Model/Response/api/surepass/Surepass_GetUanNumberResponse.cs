using VERIDATA.Model.Response.api.surepass.Base;

namespace VERIDATA.Model.Response.api.surepass
{
    public class Surepass_GetUanNumberResponse : Surepass_BaseResponse
    {
        public AadharToUanData data { get; set; }

        public class AadharToUanData
        {
            public string? aadhaar_number { get; set; }
            public string? client_id { get; set; }
            public string? pf_uan { get; set; }
        }
    }
}