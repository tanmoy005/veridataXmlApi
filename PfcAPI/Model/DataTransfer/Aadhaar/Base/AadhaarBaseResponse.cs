namespace PfcAPI.Model.DataTransfer.Aadhaar.Base
{
    public class AadhaarBaseResponse
    {
        public int status_code { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        //public string message_code { get; set; }
        public string type { get; set; }
    }
}
