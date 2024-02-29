using PfcAPI.Model.DataTransfer.UAN.Base;

namespace PfcAPI.Model.DataTransfer.UAN.Response
{
    public class UanValidateResponse : UANBaseResponse
    {
        public string age_range { get; set; }
        public string state { get; set; }
        public bool is_mobile { get; set; }
        public string gender { get; set; }
        public string aadhaar_number { get; set; }
        public string last_digits { get; set; }
        public string client_id { get; set; }

    }
}
