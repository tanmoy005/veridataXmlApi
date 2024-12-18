using VERIDATA.Model.Request.api.Karza.Base;

namespace VERIDATA.Model.Request.api.Karza
{
    public class Karza_GetUanDetailsByMobileRequest : Karza_BaseRequest
    {
        public string mobile { get; set; }
    }
}