using VERIDATA.Model.Request.api.Karza.Base;

namespace VERIDATA.Model.Request.api.Karza
{
    public class Karza_UanGenerateOtpRequest : Karza_BaseRequest
    {
        public string? uan { get; set; }
        public string? mobile_no { get; set; }
    }
}