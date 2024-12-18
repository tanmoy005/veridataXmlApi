using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeeApproverRequest
    {
        [Required]
        public int appointeeId { get; set; }

        [StringLength(250, MinimumLength = 10, ErrorMessage = "Remarks should be between  10 to 250 characters.")]
        public string? Remarks { get; set; }

        [Required]
        public int userId { get; set; }
    }
}