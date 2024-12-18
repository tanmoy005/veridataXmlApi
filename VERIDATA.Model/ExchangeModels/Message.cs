using MimeKit;

namespace VERIDATA.Model.ExchangeModels
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public List<MailboxAddress> Cc { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(IEnumerable<EmailAddress> to, IEnumerable<EmailAddress> cc, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x.DisplayName, x.Address)));
            Cc = new List<MailboxAddress>();
            Cc.AddRange(cc.Select(x => new MailboxAddress(x.DisplayName, x.Address)));
            Subject = subject;
            Content = content;
        }
    }
}