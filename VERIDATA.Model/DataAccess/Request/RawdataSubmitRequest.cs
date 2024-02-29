
namespace VERIDATA.Model.DataAccess.Request
{
    public class RawdataSubmitRequest
    {
        public List<AppointeeBasicInfo>? ApnteFileData { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int FileId { get; set; }
    }
}
