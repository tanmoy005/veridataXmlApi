namespace VERIDATA.Model.Response
{
    public class AppointeeAadhaarGenerateOtpResponse
    {
        public bool if_number { get; set; }
        public bool otp_sent { get; set; }
        public string? client_id { get; set; }
        public bool valid_aadhaar { get; set; }
    }
}
