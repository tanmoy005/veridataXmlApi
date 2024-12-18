using VERIDATA.Model.Response.api.Signzy.Base;

namespace VERIDATA.Model.Response.api.Signzy
{
    public class Signzy_UanSubmitOtpResponse : Signzy_BaseResponse
    {
        public int? ResultCode { get; set; }
        public string? TxnId { get; set; }
    }
}