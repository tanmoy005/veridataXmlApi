using System.ComponentModel;

namespace VERIDATA.Model.Response
{
    public class AppointeeNationalityDataReportDetails
    {
        [DisplayName("Appointee Id")]
        public int? AppointeeId { get; set; }

        [DisplayName("Candidate Id")]
        public string? candidateId { get; set; }

        [DisplayName("Appointee Name")]
        public string? AppointeeName { get; set; }

        [DisplayName("Email")]
        public string? EmailId { get; set; }

        [DisplayName("Mobile No")]
        public string? MobileNo { get; set; }
        
        [DisplayName("Nationality")]
        public string? Nationality { get; set; } 
        
        [DisplayName("Country Name")]
        public string? CountryName { get; set; } 

        [DisplayName("Passport Number")]
        public string? PassportNumber { get; set; } 
        
        [DisplayName("Start Date")]
        public string? StartDate { get; set; }

        [DisplayName("expiryDate")]
        public string? ExpiryDate { get; set; }
    }
}
