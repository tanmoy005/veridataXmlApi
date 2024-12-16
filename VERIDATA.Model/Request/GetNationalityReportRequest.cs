namespace VERIDATA.Model.Request
{
    public class GetNationalityReportRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? nationalityType { get; set; }
    }
}
