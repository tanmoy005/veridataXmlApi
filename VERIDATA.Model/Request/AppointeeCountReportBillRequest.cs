namespace VERIDATA.Model.Request
{
    public class AppointeeCountReportBillRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<int>? EntityId { get; set; }
    }
}
