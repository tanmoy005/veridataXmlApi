using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Request
{
    public class AppointeeReUploadFilesAfterSubmitRequest
    {
        [Required]
        public int AppointeeId { get; set; }
        public string? AppointeeCode { get; set; }
      
        [Required]
        public int UserId { get; set; }
        public List<IFormFile>? FileDetails { get; set; }
        public string? FileUploaded { get; set; }
        //public bool? IsSubmit { get; set; }
    }
}
