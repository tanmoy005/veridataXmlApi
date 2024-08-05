using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.DataAccess;

namespace VERIDATA.Model.Response
{
    public class GetAgingReportResponse
    {
        public List<AppointeeAgingDataReportDetails>? AppointeeDetails { get; set; }
        public Filedata? Filedata { get; set; }
    }
}
