using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.DataAccess
{
    public class DigilockerAccessDetails : BaseApiResponse
    {
        public string? DigilockerUrl { get; set; }
        public string? RequestId { get; set; }
    }
}