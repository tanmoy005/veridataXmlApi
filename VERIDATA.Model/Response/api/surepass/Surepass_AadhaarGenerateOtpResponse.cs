using VERIDATA.Model.Response.api.surepass.Base;

namespace VERIDATA.Model.Response.api.surepass
{
    public class Surepass_AadhaarGenerateOtpResponse : Surepass_BaseResponse
    {
        public AadharGenerateOtp data { get; set; }
    }

    public class AadharGenerateOtp
    {
        public string client_id { get; set; }
        public bool otp_sent { get; set; }
        public bool if_number { get; set; }
        public bool valid_aadhaar { get; set; }
        public string status { get; set; }
    }
}