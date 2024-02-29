using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Response
{
    public class TokenDetailsResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public bool? IsValid { get; set; }
    }
}
