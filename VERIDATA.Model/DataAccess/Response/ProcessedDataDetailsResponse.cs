using VERIDATA.Model.Table.Public;

namespace VERIDATA.Model.DataAccess.Response
{
    public class ProcessedDataDetailsResponse
    {
        public AppointeeDetails? AppointeeData { get; set; }
        public int? CompanyId { get; set; }
        public int? AppointeeId { get; set; }
        public string? CandidateId { get; set; }
        public string? AppointeeName { get; set; }
        public string? AppointeeEmailId { get; set; }
        public string? MobileNo { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public decimal? EpfWages { get; set; }
        public int? ProcessedId { get; set; }
        public string? StateAlias { get; set; }
    }
}
