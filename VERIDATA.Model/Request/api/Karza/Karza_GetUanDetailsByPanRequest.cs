using VERIDATA.Model.Request.api.Karza.Base;

namespace VERIDATA.Model.Request.api.Karza
{
    public class Karza_GetUanDetailsByPanRequest : Karza_BaseRequest
    {
        public Karza_GetUanDetailsByPanRequest()
        {
            runPanFlow = true;
            showFailures = true;
        }

        public bool runPanFlow { get; set; }
        public string pan { get; set; }
        public string mobile { get; set; }
        public bool showFailures { get; set; }
    }
}