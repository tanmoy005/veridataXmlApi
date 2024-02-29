namespace VERIDATA.Model.Configuration
{
    public class TokenConfiguration
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? Key { get; set; }
        public int? Timeout { get; set; }
    }
}
