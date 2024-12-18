using System.Net;

namespace VERIDATA.Model.Response
{
    public class AppointeePassportValidateResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsValid { get; set; }
        public string? Remarks { get; set; }
    }
}