namespace VERIDATA.Model.DataAccess.Request
{
    public class PfDataFilterReportRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? StatusCode { get; set; }
        public bool? IsTrustPassbook { get; set; }
        public bool? IsEpfoPassbook { get; set; }
        public bool? IsManual { get; set; }
        public bool? IsPensionApplicable { get; set; }
        public bool? IsPensionGap { get; set; }
    }
}