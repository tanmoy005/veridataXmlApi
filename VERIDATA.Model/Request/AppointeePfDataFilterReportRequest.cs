namespace VERIDATA.Model.Request
{
    public class AppointeePfDataFilterReportRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? PfType { get; set; }
        public int? IsManual { get; set; }
        public bool? PensionStatus { get; set; }
        public int? EpsGap { get; set; }
    }
}