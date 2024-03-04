using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeeAadhaarAadharXmlVarifyRequest
    {
        [Required]
        public int appointeeId { get; set; }
        //public string? appointeeCode { get; set; }
        [Required]
        public string? aadharName { get; set; }
        public string? shareCode { get; set; }
        public IFormFile? aadharFileDetails { get; set; }
        //public string? fileUploaded { get; set; }
        //public string? uploadTypeAlias { get; set; }
        [Required]
        public int userId { get; set; }

    }
}
