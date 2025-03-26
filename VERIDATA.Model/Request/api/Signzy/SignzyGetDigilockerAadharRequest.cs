namespace VERIDATA.Model.Request.api.Signzy
{
    public class SignzyGetDigilockerAadharRequest
    {
        public SignzyGetDigilockerAadharRequest()
        {
            extraDigitalCertificateParams = false;
        }

        public string requestId { get; set; }

        public bool extraDigitalCertificateParams { get; set; }
    }
}