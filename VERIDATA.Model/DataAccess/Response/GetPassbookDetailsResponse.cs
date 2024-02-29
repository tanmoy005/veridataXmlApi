namespace VERIDATA.Model.DataAccess.Response
{
    public class GetPassbookDetailsResponse : BaseApiResponse
    {
        public string? Name { get; set; }
        public string? FathersName { get; set; }
        public string? DateOfBirth { get; set; }
        public string? PfUan { get; set; }
        public bool? IsPensionApplicable { get; set; }

        public bool? IsPFverificationReq { get; set; }
    }
}
