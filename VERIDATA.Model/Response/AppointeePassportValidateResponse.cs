using System.Net;
using VERIDATA.Model.DataAccess;

namespace VERIDATA.Model.Response
{
    public class AppointeePassportValidateResponse : BaseApiResponse
    {
        //public HttpStatusCode StatusCode { get; set; }
        public bool IsValid { get; set; }

        public string? Remarks { get; set; }
        public PassportDetails? passportDetails { get; set; } = new PassportDetails();
    }
}