using Newtonsoft.Json;
using VERIDATA.Model.Response.api.Signzy.Base;

namespace VERIDATA.Model.Response.api.Signzy
{
    public class Signzy_UanGenerateOtpResponse : Signzy_BaseResponse
    {
        public int? ResultCode { get; set; }
        public string? ClientRefNum { get; set; }
        public string? TxnId { get; set; }
    }
    
}
