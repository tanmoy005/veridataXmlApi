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
