namespace VERIDATA.Model.ExchangeModels
{
    public class MailDetails
    {
        public string? Subject { get; set; }
        public string? BodyDetails { get; set; }
        public string? MailType { get; set; }
        public List<EmailAddress> ToEmailList { get; set; }
        public List<EmailAddress> ToCcEmailList { get; set; }
        public MailBodyParseDataDetails? ParseData { get; set; }
    }
}
