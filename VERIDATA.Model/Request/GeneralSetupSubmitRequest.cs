namespace VERIDATA.Model.Request
{
    public class GeneralSetupSubmitRequest
    {
        public List<EmailEscalationLevel> EmailEscalationLevel { get; set; }
        public List<EmailEscalationSetup> EmailEscalationSetup { get; set; }
        public int CriticalDays { get; set; }
        public int GracePeriod { get; set; }
        public int OverlapDays { get; set; }
        public int UserId { get; set; }
    }

    public class EmailEscalationLevel
    {
        public int LevelId { get; set; }
        public List<string> Emailaddress { get; set; }
        public int NoOfDays { get; set; }
        //public string SetupAlias { get; set; }
    }

    public class EmailEscalationSetup
    {
        public int LevelId { get; set; }
        public int CaseId { get; set; }
        public bool CaseOption { get; set; }
        public string? CaseEmail { get; set; }
    }
}