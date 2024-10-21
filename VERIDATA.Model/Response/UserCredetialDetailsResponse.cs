using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Response
{
    public class UserCredetialDetailsResponse
    {
        [Required]
        public int appointeeId { get; set; }

        [Required]
        public int userId { get; set; }
        [Required]
        public int companyId { get; set; }
        public string? UserName { get; set; }
        public string? EmailId { get; set; }
        public string? appointeeCode { get; set; }
        public string? userCode { get; set; }
        public string? defaultPassword { get; set; }
    }
}
