
using VERIDATA.Model.Response.api.Karza.Base;

namespace VERIDATA.Model.Response.api.Karza
{
    public class Karza_PanDetailsResponse : Karza_BaseResponse
    {
        public PanInfoResult? result { get; set; }
    }
    public class Address
    {
        public string? buildingName { get; set; }
        public string? locality { get; set; }
        public string? streetName { get; set; }
        public string? pinCode { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? country { get; set; }
    }

    public class PanInfoResult
    {
        public string? pan { get; set; }
        public string? name { get; set; }
        public string? firstName { get; set; }
        public string? middleName { get; set; }
        public string? lastName { get; set; }
        public string? gender { get; set; }
        public string? mobileNo { get; set; }
        public string? emailId { get; set; }
        public string? dob { get; set; }
        public Address? address { get; set; }
        public bool? aadhaarLinked { get; set; }
        public bool? isSalaried { get; set; }
    }
}
