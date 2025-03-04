namespace VERIDATA.Model.Request
{
    public class ProcessedFilterRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsPfRequired { get; set; }
        public string? ProcessStatus { get; set; }
        public bool? IsManualPassbook { get; set; }
        public bool? IsFiltered { get; set; }
        public int? NoOfDays { get; set; }
        public string? AppointeeName { get; set; }
        public string? CandidateId { get; set; }
        public string? FilePassword { get; set; }
        public bool? IsPensionApplicable { get; set; }
    }
}