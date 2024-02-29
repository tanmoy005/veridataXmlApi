using VERIDATA.Model.Response.api.Karza.Base;

namespace VERIDATA.Model.Response.api.Karza
{
    public class Karza_AadhaarGenerateOtpResponse : Karza_BaseResponse
    {
        public AadharGenerateOtp? result { get; set; }
    }
    public class AadharGenerateOtp
    {
        public string? message { get; set; }

    }
}
