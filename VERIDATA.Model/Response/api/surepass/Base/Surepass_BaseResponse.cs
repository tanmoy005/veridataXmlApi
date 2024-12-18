namespace VERIDATA.Model.Response.api.surepass.Base
{
    public class Surepass_BaseResponse
    {
        public int status_code { get; set; }
        public string? message { get; set; }
        public bool success { get; set; }
        public string? message_code { get; set; }
    }
}