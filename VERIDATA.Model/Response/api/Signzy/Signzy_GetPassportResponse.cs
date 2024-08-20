using VERIDATA.Model.Response.api.Signzy.Base;

namespace VERIDATA.Model.Response.api.Signzy
{
    public class Signzy_GetPassportResponse : Signzy_BaseResponse
    {
        public PassportResult? Result { get; set; }

    }
    public class PassportResult
    {
        public string? FileNumber { get; set; }
        public string? GivenName { get; set; }
        public string? Surname { get; set; }
        public string? TypeOfApplication { get; set; }
        public string? ApplicationReceivedOnDate { get; set; }
        public string? Name { get; set; }
        public string? Dob { get; set; }
    }
}
