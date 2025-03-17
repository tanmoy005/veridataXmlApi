using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.DataAccess
{
    public class FirDetails : BaseApiResponse
    {
        public List<PoliceFirDetails>? PoliceFirDetails { get; set; }
    }

    public class PoliceFirDetails
    {
        public string FirNumber { get; set; }
        public string FirDate { get; set; }
        public string PoliceStation { get; set; }
        public List<FirActAndSection> FirActDetails { get; set; }
    }

    public class FirActAndSection
    {
        public string Acts { get; set; }
        public string Sections { get; set; }
    }
}