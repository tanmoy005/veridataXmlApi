using VERIDATA.Model.DataAccess;

namespace VERIDATA.Model.Response
{
    public class GetAgingReportResponse
    {
        public List<AppointeeAgingDataReportDetails>? AppointeeDetails { get; set; }
        public Filedata? Filedata { get; set; }
    }
}