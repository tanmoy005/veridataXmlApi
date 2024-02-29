using System.ComponentModel.DataAnnotations;

namespace PfcAPI.Model.RequestModel
{
    public class CreateUsereRoleRequest
    {
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Role Name length must be less than 20 characters.")]
        public string? RoleName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Role description length must be less than 50 characters.")]
        public string? RoleDesc { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "Alias  length must be between 6 to 10 characters.")]
        public string? RoleAlias { get; set; }
        public int UserId { get; set; }
    }
}
