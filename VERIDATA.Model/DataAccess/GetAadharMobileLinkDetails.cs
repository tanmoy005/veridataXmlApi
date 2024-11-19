using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.DataAccess
{
    public class GetAadharMobileLinkDetails : BaseApiResponse
    {
        public bool? validId { get; set; }
        public bool? validMobileNo { get; set; }
        public string? remarks { get; set; }

    }
}
