using VERIDATA.Model.DataAccess;

namespace VERIDATA.Model.Response
{
    public class GetUanResponse : BaseApiResponse
    {
        public bool? IsUanAvailable { get; set; }

        //public bool? IsVarified { get; set; }
        public string? UanNumber { get; set; }

        public string? Remarks { get; set; }

        public bool? isAadharUanVerified { get; set; }
    }
}