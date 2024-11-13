
namespace VERIDATA.Model.Request
{
    public class ManualVeificationProcessDataRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? FilterType { get; set; }
    }
}
