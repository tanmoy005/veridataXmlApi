using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VERIDATA.Model.DataAccess;

namespace VERIDATA.Model.Response
{
    public class AppointeeBankValidateResponse : BaseApiResponse
    {
        public bool IsValid { get; set; }
        public string? Remarks { get; set; }
    }
}
