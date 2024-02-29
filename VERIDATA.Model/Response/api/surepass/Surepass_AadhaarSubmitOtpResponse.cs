using VERIDATA.Model.Response.api.surepass.Base;

namespace VERIDATA.Model.Response.api.surepass
{
    public class Surepass_AadhaarSubmitOtpResponse : Surepass_BaseResponse
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public Aadhaardata data { get; set; }

        public class Aadhaardata
        {
            public string gender { get; set; }
            public Address address { get; set; }
            public string aadhaar_number { get; set; }
            public string dob { get; set; }
            public string client_id { get; set; }
            public string zip { get; set; }
            public string full_name { get; set; }
            public string zip_data { get; set; }
            public string care_of { get; set; }
            //public string profile_image { get; set; }
            //public string raw_xml { get; set; }
            public string share_code { get; set; }
        }
        public class Address
        {
            public string loc { get; set; }
            public string country { get; set; }
            public string house { get; set; }
            public string subdist { get; set; }
            public string vtc { get; set; }
            public string po { get; set; }
            public string state { get; set; }
            public string street { get; set; }
            public string dist { get; set; }
        }

    }
}
