using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Response;

namespace VERIDATA.Model.Response
{
    public class AppointeeCountJobResponse
    {
        public List<AppointeeCountDateWise>? AppointeeCountDateWises { get; set; }
        public List<AppointeeCountDetails>? AppointeeCountListDetails { get; set; }
        public Filedata? Filedata { get; set; }

    }


}
