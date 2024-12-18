using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AppointeeApproveVerificationRequest
    {
        [Required]
        public int AppointeeId { get; set; }

        [Required]
        public int UserId { get; set; }

        public string? VerificationCategory { get; set; }
        public string? Remarks { get; set; }

        [Required]
        public List<VerificationUpdatesubCategory>? VerificationSubCategoryList { get; set; }
    }

    public class VerificationUpdate
    {
        [Required]
        public string? FieldName { get; set; }

        [Required]
        public bool Value { get; set; }
    }

    public class VerificationUpdatesubCategory
    {
        [Required]
        public string? SubCategory { get; set; }

        [Required]
        public List<VerificationUpdate>? VerificationQueries { get; set; }
    }
}