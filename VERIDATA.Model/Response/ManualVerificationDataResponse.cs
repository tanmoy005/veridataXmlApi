using VERIDATA.Model.DataAccess;

namespace VERIDATA.Model.Response
{
    public class ManualVerificationDataResponse
    {
        public Filedata? Filedata { get; set; }
        public List<ManualVerificationProcessDetailsResponse>? ManualVerificationList { get; set; }
    }
}
