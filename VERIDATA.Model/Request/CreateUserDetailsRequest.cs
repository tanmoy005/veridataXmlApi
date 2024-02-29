using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class CreateUserDetailsRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "User Name length must be less than 50 characters.")]
        public string? UserName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "User Code length must be less than 50 characters.")]
        public string? UserCode { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "Password  length must be between 6 to 10 characters.")]
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
