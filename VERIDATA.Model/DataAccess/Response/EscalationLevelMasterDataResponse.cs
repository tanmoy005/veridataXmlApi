namespace VERIDATA.Model.DataAccess.Response
{
    public class EscalationLevelMasterDataResponse
    {
        public int LevelId { get; set; }
        public string? LevelName { get; set; }

        //public string? Email { get; set; }
        public int NoOfDays { get; set; }

        public string? LevelCode { get; set; }
        public string? SetupAlias { get; set; }
        public string? Emailaddress { get; set; }
    }
}