using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeeHadicapFileDetailsRequest
    {
        [Required]
        public string? IsHandicap { get; set; }
        public string? HandicapType { get; set; }
        [Required]
        public int AppointeeId { get; set; }
        public string? AppointeeCode { get; set; }
        [Required]
        public int UserId { get; set; }
        public List<IFormFile>? FileDetails { get; set; }
        public string? FileUploaded { get; set; }

    }
}
