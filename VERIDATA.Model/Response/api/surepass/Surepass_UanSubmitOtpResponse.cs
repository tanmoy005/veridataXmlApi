using VERIDATA.Model.Response.api.surepass.Base;

namespace VERIDATA.Model.Response.api.surepass
{
    public class Surepass_UanSubmitOtpResponse : Surepass_BaseResponse
    {
        public UanSubmitData data { get; set; }
        public class UanSubmitData
        {
            public string client_id { get; set; }
            public bool otp_validated { get; set; }
            public bool is_async { get; set; }

        }
    }
}
