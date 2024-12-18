namespace VERIDATA.Model.Response
{
    public class UnderProcessJobResponse
    {
        public string? candidateId { get; set; }
        public string? appointeeName { get; set; }
        public string? appointeeEmailId { get; set; }
        public string? mobileNo { get; set; } //number that varified with aadhar
        public string? dateOfJoining { get; set; }
        public string? dateOfOffer { get; set; }
        public string? CreatedDate { get; set; }
        public string? Status { get; set; }
        //public int StatusCode { get; set; }
    }
}