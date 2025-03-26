using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VERIDATA.Model.Request
{
    public class AppointeeDigilockerAadhaarVarifyRequest
    {
        [Required]
        public int AppointeeId { get; set; }

        [Required]
        public string? RequestId { get; set; }

        public string? AadharName { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}