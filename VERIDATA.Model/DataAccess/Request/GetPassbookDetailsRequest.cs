namespace VERIDATA.Model.DataAccess.Request
{
    public class GetPassbookDetailsRequest
    {
        public int AppointeeId { get; set; }
        public int UserId { get; set; }
        public string? UanNumber { get; set; }
        public string? AppointeeCode { get; set; }
        public string? Otp { get; set; }
        public UanSubmitOtpDetails? OtpDetails { get; set; }
    }
}