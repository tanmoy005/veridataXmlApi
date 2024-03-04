using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Response
{
    public class UnzipAadharDataResponse
    {
        public bool IsValid { get; set; }
        public string? FileContent { get; set; }
        public string? Message { get; set; }
    }
}
