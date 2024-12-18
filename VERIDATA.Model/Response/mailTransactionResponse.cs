namespace VERIDATA.Model.Response
{
    public class mailTransactionResponse
    {
        public int AppointeeId { get; set; }
        public string Type { get; set; }
        public int UserId { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}