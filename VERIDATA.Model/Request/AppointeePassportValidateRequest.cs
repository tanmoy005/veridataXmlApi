using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeePassportValidateRequest
    {
        [Required]
        public int appointeeId { get; set; }

        [Required]
        public int userId { get; set; }

        [Required]
        public string passportFileNo { get; set; }

        [Required]
        public DateTime dateOfBirth { get; set; }
    }
}