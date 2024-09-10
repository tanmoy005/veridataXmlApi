using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeeHadicapFileDetailsRequest
    {
        [Required]
        public AppointeeFileDetailsRequest FileRequest { get; set; }
        public bool? IsHandicap { get; set; }
        public string? HandicapType { get; set; }

    }
}
