using Newtonsoft.Json;

namespace VERIDATA.Model.Response.api.Karza
{
    public class Karza_UanGenerateOtpResponse 
    {
        [JsonProperty("status-code")]
        public int? statusCode { get; set; }
        
        [JsonProperty("request_id")]
        public string? requestId { get; set; }
        public UanGenerateOtp? result { get; set; }
    }
    public class UanGenerateOtp
    {
        public string? message { get; set; }

    }
}
