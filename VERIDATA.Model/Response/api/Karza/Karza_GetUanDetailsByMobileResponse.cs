
using VERIDATA.Model.Response.api.Karza.Base;

namespace VERIDATA.Model.Response.api.Karza
{
    public class Karza_GetUanDetailsByMobileResponse : Karza_BaseResponse
    {
        public MobileToUanResult? result { get; set; }
        public ResponseClientData? ClientData { get; set; }
    }
    public class MobileToUanResult
    {
        public List<string>? Uan { get; set; }
    }
    public class ResponseClientData
    {
        public string CaseId { get; set; }
    }
}
