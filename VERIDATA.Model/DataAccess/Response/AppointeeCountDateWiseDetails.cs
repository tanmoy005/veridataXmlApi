using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VERIDATA.Model.DataAccess.Response
{
    public class AppointeeCountDateWiseDetails
    {
        public List<AppointeeCountDateWise>? AppointeeCountDateWise { get; set; }
        public List<AppointeeCountDetails>? AppointeeCountDetails { get; set; }
        public List<AppointeeTotalCount>? AppointeeTotalCount { get; set; }
        public List<AppointeeCountDetailsXls>? appointeeCountDetailsXls { get; set; }
        public List<AppointeeCounteBillReport>? appointeeCounteBillReports { get; set; }
    }
    public class AppointeeTotalCount
    {
        [DisplayName("Date")]
        public string? Date { get; set; }

        [DisplayName("Total Appointee")]
        public int? TotalAppointeeCount { get; set; }

        [DisplayName("Total LinkSent")]
        public int? TotalLinkSentCount { get; set; }

        [DisplayName("Total Link Not Sent")]
        public int? TotalLinkNotSentCount { get; set; }

        [DisplayName("Company Id")]
        public int? CompnayId { get; set; }
    }
    public class AppointeeCountDateWise
    {
        public AppointeeTotalCount? appointeeTotalCount { get; set; }
        public List<AppointeeCountDetails>? AppointeeCountDetails { get; set; }
        public List<AppointeeCountDetailsXls>? appointeeCountXls {  get; set; }
    }
    public class AppointeeCountDetails
    {
        [DisplayName("Date")]
        public string? Date { get; set; }

        [DisplayName("Candidate ID")]
        public string? CandidateId { get; set; }

        [DisplayName("Company Id")]
        public int? CompanyId { get; set; }

        [DisplayName("Entity Name")]
        public string? CompanyName { get; set; }

        [DisplayName("AppointeeName")]
        public string? AppointeeName { get; set; }

        [DisplayName("EmailId")]
        public string? EmailId { get; set; }

        [DisplayName("Appointee Status")]
        public string? AppointeeStatus { get; set; }

        [DisplayName("Action Taken")]
        public string? ActionTaken { get; set; }

    }

    public class AppointeeCountDetailsXls
    {
        [DisplayName("Date")]
        public string? Date { get; set; }

       [DisplayName("Candidate ID")]
        public string? CandidateId { get; set; }

        [DisplayName("Entity Name")]
        public string? CompanyName { get; set; }

        [DisplayName("AppointeeName")]
        public string? AppointeeName { get; set; }

        [DisplayName("EmailId")]
        public string? EmailId { get; set; }

        [DisplayName("Appointee Status")]
        public string? AppointeeStatus { get; set; }

        [DisplayName("Action Taken")]
        public string? ActionTaken { get; set; }
    }
    public class AppointeeCounteBillReport
    {
        [DisplayName("Entity Name")]
        public string? companyName { get; set; }

        [DisplayName("From Date")]
        public string? FromDate { get; set; }
        [DisplayName("To Date")]
        public string? ToDate { get; set; }

        //[NotMapped]
        //[DisplayName("Company Id")]
       // public int? CompanyId { get; set; }
        
        [DisplayName("Total Number of New Appointee")]
        public int? totalAppointeeCount { get; set; }

        [DisplayName("Cost per New Appointee : INR")]
        public int? ratePerTotalAppointeeCount { get; set; }

        [DisplayName("Total Billing : INR")]
        public int? GrandTotal {  get; set; } 
    }
}
