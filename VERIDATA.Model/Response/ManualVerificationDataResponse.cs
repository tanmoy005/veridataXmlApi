using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VERIDATA.Model.DataAccess;

namespace VERIDATA.Model.Response
{
    public class ManualVerificationDataResponse
    {
        public Filedata? Filedata { get; set; }
        public List<ManualVerificationProcessDetailsResponse>? ManualVerificationList { get; set; }
    }
}
