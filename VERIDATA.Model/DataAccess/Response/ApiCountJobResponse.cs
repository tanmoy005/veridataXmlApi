using System.ComponentModel;

namespace VERIDATA.Model.DataAccess.Response
{
    public class ApiCountJobResponse
    {
        [DisplayName("Date")]
        public string? Date { get; set; }

        [DisplayName("Provider")]
        public string? ProviderName { get; set; }
        
        [DisplayName("Api")]
        public string? ApiName { get; set; }

        [DisplayName("Total")]
        public int? TotalApiCount { get; set; }

        [DisplayName("Failure")]
        public int? TotalFailureCount { get; set; }

        [DisplayName("Success")]
        public int? TotalSuccessApiCount { get; set; }

        [DisplayName("Invalid")]
        public int? TotalUnprocessableEntityCount { get; set; }

       
    }
}
