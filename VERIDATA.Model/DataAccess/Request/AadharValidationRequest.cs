namespace VERIDATA.Model.DataAccess.Request
{
    public class AadharValidationRequest
    {
        public int AppointeeId { get; set; }
        public string? AppointeeAadhaarName { get; set; }
        //public string? AppointeeAadhaarNo { get; set; }
        public int UserId { get; set; }
        public bool isValidAdhar { get; set; }
        public string sharePhrase { get; set; }
        public AadharSubmitOtpDetails? AadharDetails { get; set; }

    }
}
