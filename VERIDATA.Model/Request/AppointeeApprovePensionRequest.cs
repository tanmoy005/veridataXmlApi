using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeeApprovePensionRequest
    {
        [Required]
        public int appointeeId { get; set; }

        [Required]
        public bool IsPensionApplicable { get; set; }
        [Required]
        public int userId { get; set; }
    }
}
