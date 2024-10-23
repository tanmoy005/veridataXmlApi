namespace VERIDATA.Model.Request
{
    public class AppointeeSeacrhFilterRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? CompanyId { get; set; }
        public bool? IsFiltered { get; set; }
        public bool? IsManualPassbook { get; set; }
        public int? NoOfDays { get; set; }
        public string? FilterType { get; set; }
        public string? AppointeeName { get; set; }
        public string? CandidateId { get; set; }
        public string? StatusCode { get; set; }
    }
}
