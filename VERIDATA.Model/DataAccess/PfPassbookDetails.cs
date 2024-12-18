using VERIDATA.Model.Response.api.Karza;
using VERIDATA.Model.Response.api.Signzy;
using VERIDATA.Model.Response.api.surepass;

namespace VERIDATA.Model.DataAccess
{
    public class PfPassbookDetails : BaseApiResponse
    {
        public PfPassbookDetails()
        {
            Passbkdata = new PassbookData();
            KarzaPassbkdata = new UanPassbookDetails();
        }

        public PassbookData? Passbkdata { get; set; }
        public UanPassbookDetails? KarzaPassbkdata { get; set; }
        public SignzyUanPassbookDetails? SignzyPassbkdata { get; set; }
    }
}