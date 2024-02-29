
using VERIDATA.Model.Request.api.Karza.Base;

namespace VERIDATA.Model.Request.api.Karza
{
    public class Karza_UanSubmitOtpRequest : Karza_BaseRequest
    {
        public Karza_UanSubmitOtpRequest()
        {
            is_pdf_required = "N";
            partial_data = "N";
            epf_balance = "Y";
        }
        public string? request_id { get; set; }
        public string? otp { get; set; }
        public string? is_pdf_required { get; set; }
        public string? partial_data { get; set; }
        public string? epf_balance { get; set; }
    }
}
