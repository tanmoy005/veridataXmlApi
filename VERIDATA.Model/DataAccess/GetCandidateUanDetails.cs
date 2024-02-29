
namespace VERIDATA.Model.DataAccess
{
    public class GetCandidateUanDetails : BaseApiResponse
    {
        public bool? IsUanAvailable { get; set; }
        public string? UanNumber { get; set; }
        public bool? IsInactiveUan { get; set; }
    }
}
