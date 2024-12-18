namespace VERIDATA.Model.DataAccess
{
    public class AppointeeJobDetails
    {
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
        public DateTime? CreatedDate { get; set; }
        public string? Status { get; set; }
        public int StatusCode { get; set; }
        public string? DaysToJoin { get; set; }
        public string? lvl1Email { get; set; }
        public string? lvl2Email { get; set; }
        public string? lvl3Email { get; set; }
    }
}