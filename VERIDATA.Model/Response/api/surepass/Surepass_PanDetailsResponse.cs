using VERIDATA.Model.Response.api.surepass.Base;

namespace VERIDATA.Model.Response.api.surepass
{
    public class Surepass_PanDetailsResponse : Surepass_BaseResponse
    {
        public PanDataDetails data { get; set; }
    }

    public class PanDataDetails
    {
        public string client_id { get; set; }
        public string pan_number { get; set; }
        public string full_name { get; set; }
        public List<string> full_name_split { get; set; }
        public string masked_aadhaar { get; set; }
        public PanAddress address { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string gender { get; set; }
        public string dob { get; set; }
        public object input_dob { get; set; }
        public bool aadhaar_linked { get; set; }
        public bool dob_verified { get; set; }
        public bool dob_check { get; set; }
        public string category { get; set; }
        public bool less_info { get; set; }
    }

    public class PanAddress
    {
        public string line_1 { get; set; }
        public string line_2 { get; set; }
        public string street_name { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string full { get; set; }
    }
}