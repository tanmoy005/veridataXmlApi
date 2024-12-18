namespace VERIDATA.Model.DataAccess
{
    public class PassportDetails : BaseApiResponse
    {
        public string? PassportNumber { get; set; }
        public string? Name { get; set; }
        public string? DateOfBirth { get; set; }
        public string? MobileNumber { get; set; }

        //public string? ValidFrom { get; set; }
        //public string? ValidTill { get; set; }
        public string? FileNumber { get; set; }
    }
}