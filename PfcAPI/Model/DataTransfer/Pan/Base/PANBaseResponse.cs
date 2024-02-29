namespace PfcAPI.Model.DataTransfer.Pan.Base
{
    public class PANBaseResponse
    {
        public int status_code { get; set; }
        public string? message { get; set; }
        public bool success { get; set; }
        public string? message_code { get; set; }
    }
}
