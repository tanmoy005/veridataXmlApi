namespace VERIDATA.Model.Request
{
    public class PfUserListRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsDownloaded { get; set; }
        public string? FilePassword { get; set; }
    }
}
