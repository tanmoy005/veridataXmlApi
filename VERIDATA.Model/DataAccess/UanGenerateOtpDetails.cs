namespace VERIDATA.Model.DataAccess
{
    public class UanGenerateOtpDetails : BaseApiResponse
    {
        public string? ClientId { get; set; }
        public bool OtpSent { get; set; }
        public string? MaskedMobileNumber { get; set; }
        public bool IsAsync { get; set; }
    }
}