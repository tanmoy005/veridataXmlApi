namespace VERIDATA.Model.DataAccess
{
    public class PanDetails : BaseApiResponse
    {
        public string? PanNumber { get; set; }
        public string? Name { get; set; }
        public string? DateOfBirth { get; set; }
        public string? MobileNumber { get; set; }
        //public string? UanNumber { get; set; }
    }
}