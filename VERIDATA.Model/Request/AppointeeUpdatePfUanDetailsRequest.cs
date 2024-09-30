using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeeUpdatePfUanDetailsRequest
    {
        [Required]
        public bool? TrustPassbookAvailable { get; set; }
        [Required]
        public bool? IsUanAvailable { get; set; }
        public int AppointeeId { get; set; }
        public string? AppointeeCode { get; set; }
        [Required]
        public int UserId { get; set; }
        public List<IFormFile>? FileDetails { get; set; }
        public string? FileUploaded { get; set; }
    }
}
