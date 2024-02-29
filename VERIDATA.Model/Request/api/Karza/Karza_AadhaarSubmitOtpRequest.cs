
using VERIDATA.Model.Request.api.Karza.Base;

namespace VERIDATA.Model.Request.api.Karza
{
    public class Karza_AadhaarSubmitOtpRequest : Karza_BaseRequest
    {
        public string? otp { get; set; }
        public string? accessKey { get; set; }
        public string? aadhaarNo { get; set; }
        public string? shareCode { get; set; }
    }
}
