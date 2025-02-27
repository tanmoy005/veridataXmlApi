﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace VERIDATA.Model.Request
{
    public class AppointeeFileDetailsRequest
    {
        [Required]
        public int AppointeeId { get; set; }

        public string? AppointeeCode { get; set; }

        //public bool? TrustPassbookAvailable { get; set; }
        public bool? IsManualPassbookUploaded { get; set; }

        [Required]
        public int UserId { get; set; }

        public List<IFormFile>? FileDetails { get; set; }
        public string? FileUploaded { get; set; }
        public bool? IsSubmit { get; set; }
    }
}