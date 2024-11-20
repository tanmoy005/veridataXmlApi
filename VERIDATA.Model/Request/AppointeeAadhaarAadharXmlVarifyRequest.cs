using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeeAadhaarAadharXmlVarifyRequest
    {
        [Required]
        public int appointeeId { get; set; }
        [Required]
        public string? aadharName { get; set; }
        //public string? aadharNumber { get; set; }
        public string? shareCode { get; set; }
        public IFormFile? aadharFileDetails { get; set; }
        [Required]
        public int userId { get; set; }

    }
}
