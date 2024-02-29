namespace PfcAPI.Model.ResponseModel
{
    public class CompanyDetails
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public string? City { get; set; }
        public bool? ActiveStatus { get; set; }
    }
}
