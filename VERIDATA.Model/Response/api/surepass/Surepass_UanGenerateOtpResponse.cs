using VERIDATA.Model.Response.api.surepass.Base;

namespace VERIDATA.Model.Response.api.surepass
{
    public class Surepass_UanGenerateOtpResponse : Surepass_BaseResponse
    {
        public GenerateOtp data { get; set; }

        public class GenerateOtp
        {
            public string client_id { get; set; }
            public bool otp_sent { get; set; }
            public string masked_mobile_number { get; set; }
            public bool is_async { get; set; }
        }
    }
}