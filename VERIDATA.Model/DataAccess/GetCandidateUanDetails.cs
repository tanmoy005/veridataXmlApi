namespace VERIDATA.Model.DataAccess
{
    public class GetCandidateUanDetails : BaseApiResponse
    {
        public string? UanName { get; set; }
        public bool? IsUanAvailable { get; set; }
        public string? UanNumber { get; set; }
        public bool? IsInactiveUan { get; set; }
        public bool? isUanLinkVerified { get; set; }
    }
}