namespace VERIDATA.Model.DataAccess.Response
{
    public class UnderProcessCandidateReportDataResponse
    {
        public string? AppointeeName { get; set; }
        public string? AppointeeEmail { get; set; }
        public string? CandidateId { get; set; }
        public string? AppvlStatusDesc { get; set; }
        public string? AppvlStatusCode { get; set; }
        public int? SaveStep { get; set; }
        public bool? IsSubmit { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public int? AppvlStatusId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ActionTakenAt { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
