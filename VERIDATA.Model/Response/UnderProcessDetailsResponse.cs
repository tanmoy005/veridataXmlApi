namespace VERIDATA.Model.Response
{
    public class UnderProcessDetailsResponse
    {
        public int id { get; set; }
        public int fileId { get; set; }
        public int companyId { get; set; }
        public string? candidateId { get; set; }
        public int? appointeeId { get; set; }
        public string? appointeeName { get; set; }
        public string? appointeeEmailId { get; set; }
        public string? mobileNo { get; set; } //number that varified with aadhar
        public bool? isPFverificationReq { get; set; }
        public DateTime? dateOfJoining { get; set; }
        public DateTime? dateOfOffer { get; set; }
        public decimal? epfWages { get; set; }
        public bool? isDocSubmitted { get; set; }
        public bool? isReprocess { get; set; }
        public bool? isNoIsuueinVerification { get; set; }
        public DateTime? createdDate { get; set; }
        public int? consentStatusCode { get; set; }
        public string? status { get; set; }
        public string? passbookStatus { get; set; }
        public int statusCode { get; set; }
        public string? verificationStatusCode { get; set; }
        public string? lvl1Email { get; set; }
        public string? lvl2Email { get; set; }
        public string? lvl3Email { get; set; }
    }
}