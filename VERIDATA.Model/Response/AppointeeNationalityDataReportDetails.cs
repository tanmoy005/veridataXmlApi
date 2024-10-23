using System.ComponentModel;

namespace VERIDATA.Model.Response
{
    public class AppointeeNationalityDataReportDetails
    {
        [DisplayName("Appointee Id")]
        public int? AppointeeId { get; set; }

        [DisplayName("Candidate Id")]
        public string? candidateId { get; set; }

        [DisplayName("Name")]
        public string? AppointeeName { get; set; }

        [DisplayName("Email")]
        public string? EmailId { get; set; }

        [DisplayName("Mobile No")]
        public string? MobileNo { get; set; }
        
        [DisplayName("Nationality")]
        public string? Nationality { get; set; } 
        
        [DisplayName("Country")]
        public string? CountryName { get; set; } 

        [DisplayName("Passport No.")]
        public string? PassportNumber { get; set; } 
        
        [DisplayName("Start Date")]
        public string? StartDate { get; set; }

        [DisplayName("Expiry Date")]
        public string? ExpiryDate { get; set; }
    }
}
