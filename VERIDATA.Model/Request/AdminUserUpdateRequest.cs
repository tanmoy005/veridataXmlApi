using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class AdminUserUpdateRequest
    {
        public int Id { get; set; }
        public string? UserCode { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Name length must be less than or equal 50 characters.")]
        public string? UserName { get; set; }
        public string? Password { get; set; }

        [EmailAddress]
        public string? EmailId { get; set; }
        public string? Phone { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public int UserTypeId { get; set; }
        public int UserId { get; set; }
    }
}
