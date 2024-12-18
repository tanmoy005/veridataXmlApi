using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeePanValidateRequest
    {
        [Required]
        public int appointeeId { get; set; }

        [Required]
        public int userId { get; set; }

        [Required]
        public string? panNummber { get; set; }

        [Required]
        public string? panName { get; set; }
    }
}