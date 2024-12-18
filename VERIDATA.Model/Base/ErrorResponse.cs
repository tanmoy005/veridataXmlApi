using System.Net;
using System.Text.Json;

namespace VERIDATA.Model.Base
{
    public class ErrorResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        public ErrorResponse()
        {
            InternalMessages = new List<string>();
            UserMessage = string.Empty;
            ErrorCode = (int)HttpStatusCode.OK;
        }

        public string? UserMessage { get; set; }
        public string? InternalMessage { get; set; }
        public List<string> InternalMessages { get; set; }
        public int ErrorCode { get; set; }
        public string? MoreInfo { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}