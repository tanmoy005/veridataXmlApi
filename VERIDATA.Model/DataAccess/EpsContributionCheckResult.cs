using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.DataAccess
{
    public class EpsContributionCheckResult
    {
        public string Company { get; set; }
        //public string EstablismentId { get; set; }
        public string StartDate { get; set; }
        public bool EpsGapfind { get; set; }
        public bool HasEpsContribution { get; set; }
    }
}
