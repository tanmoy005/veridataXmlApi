using System.ComponentModel;
using VERIDATA.Model.DataAccess;

namespace VERIDATA.Model.Response
{
    public class AppointeeDataFilterReportResponse
    {
        public List<AppointeeDataFilterReportDetails>? AppointeeDetails { get; set; }
        public Filedata? Filedata { get; set; }

    }
}
