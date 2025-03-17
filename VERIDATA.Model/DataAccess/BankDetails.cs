using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.DataAccess
{
    public class BankDetails : BaseApiResponse
    {
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string AccountHolderName { get; set; }
    }
}