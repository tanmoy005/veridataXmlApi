using VERIDATA.Model.DataAccess;

namespace VERIDATA.Model.Response
{
    public class GetNationalityReportResponse
    {
        public List<AppointeeNationalityDataReportDetails>? AppointeeDetails { get; set; }
        public Filedata? Filedata { get; set; }
    }
}