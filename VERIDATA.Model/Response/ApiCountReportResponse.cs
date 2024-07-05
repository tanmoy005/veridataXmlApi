
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Response;

namespace VERIDATA.Model.Response
{
    public class ApiCountReportResponse
    {
        public List<ApiCountJobResponse>? ApiCountList { get; set; }
        public List<ConsolidateApiCountJobResponse>? ApiConsolidateCountList { get; set; }
        public Filedata? Filedata { get; set; }
    }
}
