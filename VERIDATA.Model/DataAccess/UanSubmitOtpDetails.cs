namespace VERIDATA.Model.DataAccess
{
    public class UanSubmitOtpDetails : BaseApiResponse
    {
        public string? ClientId { get; set; }
        public bool? OtpValidated { get; set; }
    }
}
