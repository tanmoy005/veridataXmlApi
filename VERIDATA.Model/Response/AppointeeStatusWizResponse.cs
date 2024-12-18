namespace VERIDATA.Model.Response
{
    public class AppointeeStatusWizResponse
    {
        public int id { get; set; }
        public int fileId { get; set; }
        public int companyId { get; set; }
        public int? appointeeId { get; set; }
        public string? appointeeName { get; set; }
        public string? appointeeEmailId { get; set; }
        public string? mobileNo { get; set; }
        public bool? IsReprocess { get; set; }
        public DateTime? dateOfJoining { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Status { get; set; }
        public int StatusCode { get; set; }
    }
}