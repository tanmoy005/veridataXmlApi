using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VERIDATA.Model.Request
{
    public class AppointeeProfileImageUpdateRequest
    {
        public int AppointeeId { get; set; }
        public int UserId { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public string? FileUploaded { get; set; }
    }
}