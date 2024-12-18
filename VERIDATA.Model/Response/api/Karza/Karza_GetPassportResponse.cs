using VERIDATA.Model.Response.api.Karza.Base;

namespace VERIDATA.Model.Response.api.Karza
{
    public class Karza_GetPassportResponse : Karza_BaseResponsev2
    {
        public PassportData? result { get; set; }

        //public ClientData clientData { get; set; }
    }

    public class DateOfIssue
    {
        public string? dispatchedOnFromSource { get; set; }
        public bool? dateOfIssueMatch { get; set; }
    }

    public class Name
    {
        public int? nameScore { get; set; }
        public bool? nameMatch { get; set; }
        public string? surnameFromPassport { get; set; }
        public string? nameFromPassport { get; set; }
    }

    public class PassportNumber
    {
        public string? passportNumberFromSource { get; set; }
        public bool? passportNumberMatch { get; set; }
    }

    public class PassportData
    {
        public PassportNumber? passportNumber { get; set; }
        public string? applicationDate { get; set; }
        public string? typeOfApplication { get; set; }
        public DateOfIssue? dateOfIssue { get; set; }
        public Name? name { get; set; }
    }
}