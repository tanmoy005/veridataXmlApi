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
        public int AppointeeId { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string? UserName { get; set; }
        public string? EmailId { get; set; }
        public string? CandidateId { get; set; }
        public string? userCode { get; set; }
        public string? DefaultPassword { get; set; }
        public string? Password { get; set; }
    }
}
