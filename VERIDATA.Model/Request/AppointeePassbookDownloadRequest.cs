using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeePassbookDownloadRequest
    {
        [Required]
        public int AppointeeId { get; set; }
        [Required]
        public int UserId { get; set; }
        public string? Type { get; set; }
    }
}
