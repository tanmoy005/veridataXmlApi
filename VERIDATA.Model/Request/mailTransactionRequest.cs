namespace VERIDATA.Model.Request
{
    public class mailTransactionRequest
    {
        public int AppointeeId { get; set; }
        public int UserId { get; set; }
        public string? Type { get; set; }
    }
}
