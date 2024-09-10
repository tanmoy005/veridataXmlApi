using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeeTrustPassbookFileDetailsRequest
    {
        [Required]
        public AppointeeFileDetailsRequest FileRequest { get; set; }
        public bool? TrustPassbookAvailable { get; set; }

    }
}
