namespace VERIDATA.Model.Response
{
    public class GeneralSetupDetailsResponse
    {
        public List<EmailEscalationLevelDetails> EmailEscalationLevelDetails { get; set; }
        public List<EmailEscalationSetupDetails> EmailEscalationSetupDetails { get; set; }
        public List<CaseSetupDetails> CaseSetupDetails { get; set; }
        public int CriticalDays { get; set; }
        public int GracePeriod { get; set; }
        public string? AadharVerificationType { get; set; }

        //public int UserId { get; set; }
    }

    public class EmailEscalationLevelDetails
    {
        public int LevelId { get; set; }
        public string LevelCode { get; set; }
        public string SetupAlias { get; set; }
        public string LevelName { get; set; }
        public List<string> Emailaddress { get; set; }
        public int NoOfDays { get; set; }
    }

    public class EmailEscalationSetupDetails
    {
        public int LevelId { get; set; }
        public string LevelCode { get; set; }
        public List<CaseOptionDetails> SetupCaseDetails { get; set; }
    }

    public class CaseSetupDetails
    {
        public int SetupCaseId { get; set; }
        public string SetupCaseDesc { get; set; }
        public string SetupAlias { get; set; }
    }

    public class CaseOptionDetails
    {
        public int SetupCaseId { get; set; }
        public string? SetupCaseCode { get; set; }
        public string? SetupAlias { get; set; }
        public string? CaseEmailAddress { get; set; }
        public bool SetupCaseOption { get; set; }
    }
}