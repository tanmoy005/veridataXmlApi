using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeeSubmitDetailsRequest
    {
        [Required]
        public int AppointeeId { get; set; }
        public int UserId { get; set; }
        public bool? IsSubmit { get; set; }

    }
}
