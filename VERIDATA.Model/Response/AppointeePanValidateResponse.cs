using System.Net;

namespace VERIDATA.Model.Response
{
    public class AppointeePanValidateResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsValid { get; set; }
        public string? Remarks { get; set; }
        public string? UanNumber { get; set; }
        public bool? IsUanAvailable { get; set; }
        public bool IsUanFetchCall { get; set; }
    }
}