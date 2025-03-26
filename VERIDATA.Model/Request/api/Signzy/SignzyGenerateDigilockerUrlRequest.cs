namespace VERIDATA.Model.Request.api.Signzy
{
    public class SignzyGenerateDigilockerUrlRequest
    {
        public SignzyGenerateDigilockerUrlRequest()
        {
            //DocType = new List<string>();
            signup = true;
            //RedirectUrl = string.Empty;
            callbackUrl = string.Empty;
            redirectTime = 1;
            successRedirectTime = 5;
            failureRedirectTime = 5;
            logo = "https://rise.barclays/content/dam/thinkrise-com/images/rise-stories/Signzy-16_9.full.high_quality.jpg";
            LogoVisible = true;
            SupportEmailVisible = true;
            supportEmail = "support@signzy.com";
            purpose = "kyc";
            showLoaderState = true;
            companyName = "Elogix";
        }

        public bool signup { get; set; }
        public string redirectUrl { get; set; }
        public int redirectTime { get; set; }
        public string callbackUrl { get; set; }
        public string successRedirectUrl { get; set; }
        public int successRedirectTime { get; set; }
        public string failureRedirectUrl { get; set; }
        public int failureRedirectTime { get; set; }
        public bool LogoVisible { get; set; }
        public string logo { get; set; }
        public bool SupportEmailVisible { get; set; }
        public string supportEmail { get; set; }

        //public List<string> DocType { get; set; }
        public string purpose { get; set; }

        //public bool GetScope { get; set; }
        //public long ConsentValidTill { get; set; }
        public bool showLoaderState { get; set; }

        //public string InternalId { get; set; }
        public string companyName { get; set; }

        //public string PersistPassword { get; set; }
    }
}