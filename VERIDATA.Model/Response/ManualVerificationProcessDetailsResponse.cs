namespace VERIDATA.Model.Response
{
    public class ManualVerificationProcessDetailsResponse
    {
        public int id { get; set; }
        public int companyId { get; set; }
        public string? candidateId { get; set; }
        public int? appointeeId { get; set; }
        public string? appointeeName { get; set; }
        public string? appointeeEmailId { get; set; }
        public string? mobileNo { get; set; } //number that varified with aadhar
        public DateTime? dateOfJoining { get; set; }
        public DateTime? dateOfOffer { get; set; }
        public bool? isDocSubmitted { get; set; }
        public bool? isNoIsuueinVerification { get; set; }
        public DateTime? createdDate { get; set; }
        public int? verificationAttempted { get; set; }
        public string? status { get; set; }
        public string? remarks { get; set; }
    }
}