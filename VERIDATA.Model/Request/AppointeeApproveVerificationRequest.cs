using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Request
{
    public class AppointeeApproveVerificationRequest
    {
        [Required]
        public int AppointeeId { get; set; }

        [Required]
        public int UserId { get; set; }
        public string? VerificationCategory { get; set; }

        [Required]
        public List<VerificationUpdate>? VerificationUpdates { get; set; }
    }

    public class VerificationUpdate
    {
        [Required]
        public string? FieldName { get; set; }
        [Required]
        public bool IsValid { get; set; }
    }
}
