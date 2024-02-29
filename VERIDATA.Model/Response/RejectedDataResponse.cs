namespace VERIDATA.Model.Response
{
    public class RejectedDataResponse
    {
        public int id { get; set; }
        public int companyId { get; set; }
        public int? appointeeId { get; set; }
        public string? candidateId { get; set; }
        public string? appointeeName { get; set; }
        public string? appointeeEmailId { get; set; }
        public string? mobileNo { get; set; } //number that varified with aadhar

        public string? adhaarNo { get; set; }
        public string? panNo { get; set; }
        public DateTime? dateOfJoining { get; set; }
        public string? RejectReason { get; set; }
        public string? RejectFrom { get; set; }
    }
}
