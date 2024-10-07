using System.Net;

namespace VERIDATA.Model.Response
{
    public class EmployementHistoryDetailsRespons
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Remarks { get; set; }
        public string? EmployementData { get; set; }
        public int AppointeeId { get; set; }
        public int UserId { get; set; }
        public string? Provider { get; set; }
        public string? SubType { get; set; }
    }
}
