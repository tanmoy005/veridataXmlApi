using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace VERIDATA.Model.Response.api.Karza.Base
{
    public class Karza_BaseResponse
    {
        public string? request_id { get; set; }

        [JsonProperty("status-code")]
        public string? statusCode { get; set; }
    }
}
