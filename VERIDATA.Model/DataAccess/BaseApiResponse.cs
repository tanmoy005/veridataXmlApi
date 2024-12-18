using System.Net;

namespace VERIDATA.Model.DataAccess
{
    public class BaseApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? ReasonPhrase { get; set; }
        public string? UserMessage { get; set; }
    }
}