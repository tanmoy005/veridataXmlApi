using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VERIDATA.Model.Response.api.Signzy
{
    public class Signzy_GetCandidateBankDetailsResponse
    {
        [JsonProperty("result")]
        public Result? Result { get; set; }
    }

    public class AuditTrail
    {
        [JsonProperty("nature")]
        public string? Nature { get; set; }

        [JsonProperty("value")]
        public string? Value { get; set; }

        [JsonProperty("timestamp")]
        public DateTime? Timestamp { get; set; }
    }

    public class BankTransfer
    {
        [JsonProperty("response")]
        public string? Response { get; set; }

        [JsonProperty("bankRRN")]
        public string? BankRRN { get; set; }

        [JsonProperty("beneName")]
        public string? BeneficiaryName { get; set; }

        [JsonProperty("beneMMID")]
        public string? BeneficiaryMMID { get; set; }

        [JsonProperty("beneMobile")]
        public string? BeneficiaryMobile { get; set; }

        [JsonProperty("beneIFSC")]
        public string? BeneficiaryIFSC { get; set; }
    }

    public class Result
    {
        [JsonProperty("active")]
        public string? Active { get; set; }

        [JsonProperty("reason")]
        public string? Reason { get; set; }

        [JsonProperty("nameMatch")]
        public string? NameMatch { get; set; }

        [JsonProperty("mobileMatch")]
        public string? MobileMatch { get; set; }

        [JsonProperty("signzyReferenceId")]
        public string? SignzyReferenceId { get; set; }

        [JsonProperty("auditTrail")]
        public AuditTrail? AuditTrail { get; set; }

        [JsonProperty("nameMatchScore")]
        public string? NameMatchScore { get; set; }

        [JsonProperty("bankTransfer")]
        public BankTransfer? BankTransfer { get; set; }
    }
}