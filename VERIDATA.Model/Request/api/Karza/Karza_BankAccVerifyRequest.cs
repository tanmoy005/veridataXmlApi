using VERIDATA.Model.Request.api.Karza.Base;

namespace VERIDATA.Model.Request.api.Karza
{
    public class Karza_BankAccVerifyRequest : Karza_BaseRequest
    {
        public string? accountNumber { get; set; }
        public string? ifsc { get; set; }
    }
}