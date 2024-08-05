using VERIDATA.Model.Table.Public;

namespace VERIDATA.Model.DataAccess.Response
{
    public class UnderProcessWithActionQueryDataResponse
    {
        public UnderProcessFileData? UnderProcess { get; set; }
        public AppointeeDetails? AppointeeDetails { get; set; }
        public int? AppvlStatusId { get; set; }
        public string? ActivityType { get; set; }
        public string? ActivityInfo { get; set; }
        public string? ActivityDesc { get; set; }
        public DateTime? LastActionDate { get; set; }
        public int? AppointeeId { get; set; }
        public bool IsJoiningDateLapsed { get; set; }
    }
}
