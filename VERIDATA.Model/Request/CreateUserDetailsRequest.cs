using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class CreateUserDetailsRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "User Name length must be less than 50 characters.")]
        public string? UserName { get; set; }

        public string? UserCode { get; set; }
        public string? Password { get; set; }

        [Required]
        [EmailAddress]
        public string? EmailId { get; set; }

        [Required]
        [Phone]
        public string? ContactNo { get; set; }

        public string? CandidateId { get; set; }

        [Required]
        public int RoleId { get; set; }

        public int CompanyId { get; set; }
        public int UserTypeId { get; set; }
        public int? RefAppointeeId { get; set; }
        public int UserId { get; set; }
    }
}