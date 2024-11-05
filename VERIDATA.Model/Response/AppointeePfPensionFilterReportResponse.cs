using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VERIDATA.Model.DataAccess;

namespace VERIDATA.Model.Response
{
    public class AppointeePfPensionFilterReportResponse
    {
        public List<AppointeePfStatusDataFilterReportResponse>? AppointeeDetails { get; set; }
        public Filedata? Filedata { get; set; }
        public List<AppointeePfDataExcelRespopnse> appointeeExcelList { get; set; }
    }
}
