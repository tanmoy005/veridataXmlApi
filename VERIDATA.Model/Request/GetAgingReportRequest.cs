namespace VERIDATA.Model.Request
{
    public class GetAgingReportRequest
    {
        public DateTime? StartDate { get; set; }
        public int NoOfDays { get; set; }
        public string? ReportType { get; set; }
        public string? FilePassword { get; set; }
    }
}