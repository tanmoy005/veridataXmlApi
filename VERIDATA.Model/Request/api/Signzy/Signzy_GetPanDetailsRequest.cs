using VERIDATA.Model.Request.api.Signzy.Base;

namespace VERIDATA.Model.Request.api.Signzy
{
    public class Signzy_GetPanDetailsRequest : Signzy_BaseRequest
    {
        //public Signzy_GetPanDetailsRequest()
        //{
        //    returnIndividualTaxComplianceInfo = "true";
        //}

        public string? panNumber { get; set; }
        //public string? returnIndividualTaxComplianceInfo { get; set; }
    }
}