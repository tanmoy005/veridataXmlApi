using VERIDATA.Model.Table.Public;

namespace VERIDATA.Model.DataAccess.Response
{
    public class UnderProcessQueryDataResponse
    {
        public UnderProcessFileData? UnderProcess { get; set; }
        public AppointeeDetails? AppointeeDetails { get; set; }
        public int? AppvlStatusId { get; set; }
        public string? AppvlStatusCode { get; set; }
        public int? ConsentStatusId { get; set; }
        public int? AppointeeId { get; set; }
        public bool IsJoiningDateLapsed { get; set; }
        //public bool IsReupload { get; set; }
    }
}
