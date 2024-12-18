using VERIDATA.Model.Response.api.Karza.Base;

namespace VERIDATA.Model.Response.api.Karza
{
    public class Karza_AadhaarMobileLinkResponse : Karza_BaseResponsev2
    {
        public AadharMobileLinkData? result { get; set; }
    }

    public class AadharMobileLinkData
    {
        public string? validId { get; set; }
        public string? isMobileLinked { get; set; }
        public string? isVerified { get; set; }
        public string? isPanLinked { get; set; }
    }
}