using VERIDATA.Model.Response.api.surepass.Base;

namespace VERIDATA.Model.Response.api.surepass
{
    public class Surepass_GetPassportResponse : Surepass_BaseResponse
    {
        public PassportData data { get; set; }
    }
    public class PassportData
    {
        public string date_of_application { get; set; }
        public string file_number { get; set; }
        public string client_id { get; set; }
        public string full_name { get; set; }
        public string passport_number { get; set; }
        public string dob { get; set; }
    }
}
