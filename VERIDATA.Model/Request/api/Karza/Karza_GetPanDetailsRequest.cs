using VERIDATA.Model.Request.api.Karza.Base;

namespace VERIDATA.Model.Request.api.Karza
{
    public class Karza_GetPanDetailsRequest : Karza_BaseRequest
    {
        public Karza_GetPanDetailsRequest()
        {
            getContactDetails = "Y";
            PANStatus = "N";
            isSalaried = "Y";
            isDirector = "N";
            isSoleProp = "N";
            fathersName = "Y";
        }

        public string? pan { get; set; }
        public string? aadhaarLastFour { get; set; }
        public string? dob { get; set; }
        public string? name { get; set; }
        public string? address { get; set; }
        public string? getContactDetails { get; set; }
        public string? PANStatus { get; set; }
        public string? isSalaried { get; set; }
        public string? isDirector { get; set; }
        public string? isSoleProp { get; set; }
        public string? fathersName { get; set; }
    }
}