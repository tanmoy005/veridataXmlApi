namespace VERIDATA.Model.DataAccess
{
    public class EscalationLevelCaseDetails
    {
        public int CaseId { get; set; }
        public int LevelId { get; set; }
        public string? LevelCode { get; set; }
        public string? SetupCode { get; set; }
        public string? SetupDesc { get; set; }
        public string? SetupAlias { get; set; }
        public bool? SetupStatus { get; set; }
        public string? EmailId { get; set; }
    }
}