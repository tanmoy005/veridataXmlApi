using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class GetEmployemntDetailsRequest
    {
        [Required]
        public int appointeeId { get; set; }
        [Required]
        public int userId { get; set; }
        [Required]
        public string? uanNummber { get; set; }
    }
}
