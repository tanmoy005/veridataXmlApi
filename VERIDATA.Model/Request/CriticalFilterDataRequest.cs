namespace VERIDATA.Model.Request
{
    public class CriticalFilterDataRequest
    {
        public int CompanyId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}