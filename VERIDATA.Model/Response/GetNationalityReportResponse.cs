using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VERIDATA.Model.DataAccess;

namespace VERIDATA.Model.Response
{
    public class GetNationalityReportResponse
    {
        public List<AppointeeNationalityDataReportDetails>? AppointeeDetails { get; set; }
        public Filedata? Filedata { get; set; }
    }
}
