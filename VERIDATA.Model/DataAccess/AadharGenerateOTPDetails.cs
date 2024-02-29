
namespace VERIDATA.Model.DataAccess
{
    public class AadharGenerateOTPDetails : BaseApiResponse
    {
        public string? client_id { get; set; }
        public bool otp_sent { get; set; }
        public bool if_number { get; set; }
        public bool valid_aadhaar { get; set; }
    }
}
