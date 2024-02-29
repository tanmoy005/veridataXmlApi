using PfcAPI.Model.DataTransfer.Aadhaar.Base;

namespace PfcAPI.Model.DataTransfer.Aadhaar.Response
{
    public class AadhaarValidateResponse : AadhaarBaseResponse
    {
        public Data data { get; set; }

    }
    public class Data
    {
        public string client_id { get; set; }
        public string age_range { get; set; }
        public string aadhaar_number { get; set; }
        public string state { get; set; }
        public string gender { get; set; }
        public string last_digits { get; set; }
        public bool is_mobile { get; set; }
        public string remarks { get; set; }
        public bool less_info { get; set; }

    }
}
