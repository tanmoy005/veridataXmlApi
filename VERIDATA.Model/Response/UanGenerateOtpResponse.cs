namespace VERIDATA.Model.Response
{
    public class UanGenerateOtpResponse
    {
        public string client_id { get; set; }
        public bool otp_sent { get; set; }
        public string masked_mobile_number { get; set; }
        public bool is_async { get; set; }
    }
}