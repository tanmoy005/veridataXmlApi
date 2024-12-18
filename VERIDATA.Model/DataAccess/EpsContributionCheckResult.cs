namespace VERIDATA.Model.DataAccess
{
    public class EpsContributionCheckResult
    {
        //public string Company { get; set; }
        ////public string EstablismentId { get; set; }
        //public string StartDate { get; set; }
        public List<EpsContributionSummary> EpsContributionSummary { get; set; }

        public bool? HasDualEmplyement { get; set; }
    }
}