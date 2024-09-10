namespace VERIDATA.Model.Request
{
    public class AppointeeCountReportSearchRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? AppointeeName { get; set; }
        public string? StatusCode { get; set; }
        public int? EntityId { get; set; }
    }
}
