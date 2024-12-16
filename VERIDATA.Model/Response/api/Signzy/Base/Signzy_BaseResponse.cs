namespace VERIDATA.Model.Response.api.Signzy.Base
{
    public class Signzy_BaseResponse
    {
        public BaseErrorResponse? Error { get; set; }
        public string? Message { get; set; }
    }

    public class BaseErrorResponse
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public int? StatusCode { get; set; }
    }
}
