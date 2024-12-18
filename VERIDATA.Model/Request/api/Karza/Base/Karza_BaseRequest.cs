namespace VERIDATA.Model.Request.api.Karza.Base
{
    public class Karza_BaseRequest
    {
        public Karza_BaseRequest()
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