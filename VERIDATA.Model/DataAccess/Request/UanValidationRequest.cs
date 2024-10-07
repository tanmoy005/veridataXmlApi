using VERIDATA.Model.DataAccess.Response;

namespace VERIDATA.Model.DataAccess.Request
{
    public class UanValidationRequest
    {
        public GetPassbookDetailsResponse? PassbookDetails { get; set; }
        public int AppointeeId { get; set; }
        public int UserId { get; set; }
        public bool IsUanActive { get; set; }
        public bool IsPassbookFetch { get; set; }
    }
}
