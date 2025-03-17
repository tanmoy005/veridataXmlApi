using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VERIDATA.Model.DataAccess;

namespace VERIDATA.Model.Response
{
    public class AppointeeFirDetailsResponse : BaseApiResponse
    {
        public List<PoliceFirDetails> PoliceFirDetails { get; set; }
        public bool IsValid { get; set; }
        public string? Remarks { get; set; }
    }
}