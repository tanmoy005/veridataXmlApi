using VERIDATA.Model.Table.Public;

namespace VERIDATA.Model.DataAccess.Response
{
    public class NationalityQueryDataResponse
    {
        public AppointeeDetails? AppointeeDetails { get; set; }
        public int? AppvlStatusId { get; set; }
        public int? AppointeeId { get; set; }
        public bool IsJoiningDateLapsed { get; set; }
    }
}