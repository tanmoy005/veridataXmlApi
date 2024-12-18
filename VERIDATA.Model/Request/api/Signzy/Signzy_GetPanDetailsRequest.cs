using VERIDATA.Model.Request.api.Signzy.Base;

namespace VERIDATA.Model.Request.api.Signzy
{
    public class Signzy_GetPanDetailsRequest : Signzy_BaseRequest
    {
        public Signzy_GetPanDetailsRequest()
        {
            getStatusInfo = true;
        }

        public string? panNumber { get; set; }
        public bool? getStatusInfo { get; set; }
    }
}