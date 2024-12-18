using VERIDATA.Model.Request.api.Karza.Base;

namespace VERIDATA.Model.Request.api.Karza
{
    public class Karza_AadhaarGenerateOTPRequest : Karza_BaseRequest
    {
        public string? aadhaarNo { get; set; }
    }
}