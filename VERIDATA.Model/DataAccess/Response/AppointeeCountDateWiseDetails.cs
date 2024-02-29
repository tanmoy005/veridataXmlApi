using System.ComponentModel;

namespace VERIDATA.Model.DataAccess.Response
{
    public class AppointeeCountDateWiseDetails
    {
        public List<AppointeeCountDateWise>? AppointeeCountDateWise { get; set; }
        public List<AppointeeCountDetails>? AppointeeCountDetails { get; set; }
        public List<AppointeeTotalCount>? AppointeeTotalCount { get; set; }
    }
    public class AppointeeTotalCount
    {
        [DisplayName("Date")]
        public string? Date { get; set; }

        [DisplayName("Total Appointee Count")]
        public int? TotalAppointeeCount { get; set; }

        [DisplayName("Total Link Sent Count")]
        public int? TotalLinkSentCount { get; set; }

        [DisplayName("Link Not Sent Count")]
        public int? TotalLinkNotSentCount { get; set; }
    }
    public class AppointeeCountDateWise
    {
        public AppointeeTotalCount? appointeeTotalCount { get; set; }
        public List<AppointeeCountDetails>? AppointeeCountDetails { get; set; }
    }
    public class AppointeeCountDetails
    {
        [DisplayName("Date")]
        public string? Date { get; set; }

        [DisplayName("Candidate Id")]
        public string? CandidateId { get; set; }

        [DisplayName("Appointee Name")]
        public string? AppointeeName { get; set; }

        [DisplayName("Appointee Email")]
        public string? EmailId { get; set; }

        [DisplayName("Appointee Status")]
        public string? AppointeeStatus { get; set; }

        [DisplayName("Last Action Taken")]
        public string? ActionTaken { get; set; }

    }
}
