using System.ComponentModel;

namespace VERIDATA.Model.DataAccess.Response
{
    public class ConsolidateApiCountJobResponse
    {

        [DisplayName("Provider Name")]
        public string? ProviderName { get; set; }
        
        [DisplayName("Api Name")]
        public string? ApiName { get; set; }

        [DisplayName("Total Api Count")]
        public int? TotalApiCount { get; set; }

        [DisplayName("Api Failure Count")]
        public int? TotalFailureCount { get; set; }

        [DisplayName("Api Success Count")]
        public int? TotalSuccessApiCount { get; set; }

        [DisplayName("Api Unprocessable Count")]
        public int? TotalUnprocessableEntityCount { get; set; }

        
    }
}
