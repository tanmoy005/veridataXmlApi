namespace VERIDATA.Model.DataAccess
{
    public class GetAadharMobileLinkDetails : BaseApiResponse
    {
        public bool? validId { get; set; }
        public bool? validMobileNo { get; set; }
        public string? remarks { get; set; }
    }
}