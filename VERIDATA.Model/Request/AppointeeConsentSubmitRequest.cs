namespace VERIDATA.Model.Request
{
    public class AppointeeConsentSubmitRequest
    {
        public int AppointeeId { get; set; }
        public int? ConsentStatus { get; set; }
        public string? ConsentStatusCode { get; set; }
        public int UserId { get; set; }
    }
}