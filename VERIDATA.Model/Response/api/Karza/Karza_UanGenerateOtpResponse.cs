using VERIDATA.Model.Response.api.Karza.Base;

namespace VERIDATA.Model.Response.api.Karza
{
    public class Karza_UanGenerateOtpResponse : Karza_BaseResponse
    {
        public UanGenerateOtp? result { get; set; }
    }

    public class UanGenerateOtp
    {
        public string? message { get; set; }
    }
}