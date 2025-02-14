using System.Net;
using VERIDATA.Model.DataAccess;

namespace VERIDATA.Model.Response
{
    public class AppointeePanValidateResponse : BaseApiResponse
    {
        public bool IsValid { get; set; }
        public string? Remarks { get; set; }
        public string? UanNumber { get; set; }
        public bool? IsUanAvailable { get; set; }
        public bool IsUanFetchCall { get; set; }
        public PanDetails? PanDetails { get; set; } = new PanDetails();
    }
}