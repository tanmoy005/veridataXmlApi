using VERIDATA.Model.Request.api.Signzy.Base;

namespace VERIDATA.Model.Request.api.Signzy
{
    public class Signzy_GetPanDetailsRequest : Signzy_BaseRequest
    {
        //public Signzy_GetPanDetailsRequest()
        //{
        //    returnIndividualTaxComplianceInfo = "true";
        //}

        public string? number { get; set; }
        //public string? returnIndividualTaxComplianceInfo { get; set; }
    }
}