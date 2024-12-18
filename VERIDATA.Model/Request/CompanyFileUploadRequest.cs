using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace VERIDATA.Model.Request
{
    public class CompanyFileUploadRequest
    {
        [Required]
        public int CompanyId { get; set; }

        [Required]
        public IFormFile? FileDetails { get; set; }

        [Required]
        public int UserId { get; set; }

        //public FileType FileType { get; set; }
    }
}