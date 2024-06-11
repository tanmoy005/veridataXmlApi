using System.ComponentModel.DataAnnotations;


namespace VERIDATA.Model.Response
{
    public class UserDetailsResponse
    {

        public int UserId { get; set; }
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
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public int ConsentStatus { get; set; }
        public bool? IsConsentProcessed { get; set; }
        public bool? IsProcessed { get; set; }
        public bool? IsSubmit { get; set; }
        //public int Createdby { get; set; }
        public int? AppointeeId { get; set; }
        public string? Status { get; set; }
        public bool? IsSetProfilePassword { get; set; }
        public bool? IsDefaultPassword { get; set; }
        public bool IsPasswordExpire { get; set; }


    }
}
