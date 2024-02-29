namespace VERIDATA.Model.Response
{
    public class CriticalAppointeeResponse
    {
        public int id { get; set; }
        public int fileId { get; set; }
        public int companyId { get; set; }
        public int? appointeeId { get; set; }
        public string? candidateId { get; set; }
        public string? appointeeName { get; set; }
        public string? appointeeEmailId { get; set; }
        public string? mobileNo { get; set; }
        public DateTime? dateOfJoining { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Status { get; set; }
        public int StatusCode { get; set; }
        public int ConsentStatusCode { get; set; }
        public int? DaysToJoin { get; set; }
    }
}
