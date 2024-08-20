
namespace VERIDATA.Model.Request.api.Signzy.Base
{
    public class Signzy_BaseRequest
    {
        public Signzy_BaseRequest()
        {
            consent = "Y";
        }
        public ClientData? clientData { get; set; }
        public string? consent { get; set; }
    }
    public class ClientData
    {
        public string caseId { get; set; }
    }
}
