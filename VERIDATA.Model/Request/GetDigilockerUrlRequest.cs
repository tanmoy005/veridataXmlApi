using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VERIDATA.Model.Request
{
    public class GetDigilockerUrlRequest
    {
        [Required]
        public int appointeeId { get; set; }

        public string? SuccessRedirectUrl { get; set; }
        public string? FailureRedirectUrl { get; set; }

        //public string? RedirectUrl { get; set; }
        [Required]
        public int userId { get; set; }
    }
}