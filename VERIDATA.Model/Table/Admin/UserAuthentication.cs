using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Admin
{
    [Table("user_authentication", Schema = "admin")]
    public class UserAuthentication
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_autho_id", TypeName = DbDataType._biginteger)]
        public int UserAuthoId { get; set; }

        [Column("user_id", TypeName = DbDataType._biginteger)]
        public int UserId { get; set; }

        [Column("user_pwd", TypeName = DbDataType._text100)]
        public string? UserPwd { get; set; }

        [Column("user_pwd_txt", TypeName = DbDataType._text10)]
        public string? UserPwdTxt { get; set; }

        [Column("user_profile_pwd", TypeName = DbDataType._text100)]
        public string? UserProfilePwd { get; set; }

        [Column("is_default_pass", TypeName = DbDataType._char)]
        [DefaultValue("Y")]
        public string? IsDefaultPass { get; set; }

        [Column("password_set_date", TypeName = DbDataType._datetime)]
        public DateTime? PasswordSetDate { get; set; }

        [Column("active_status", TypeName = DbDataType._boolean)]
        [DefaultValue(true)]
        public bool? ActiveStatus { get; set; }

        [Column("created_by", TypeName = DbDataType._integer)]
        public int? CreatedBy { get; set; }

        [Column("created_on", TypeName = DbDataType._datetime)]
        public DateTime? CreatedOn { get; set; }

        [Column("updated_by", TypeName = DbDataType._integer)]
        public int? UpdatedBy { get; set; }

        [Column("updated_on", TypeName = DbDataType._datetime)]
        public DateTime? UpdatedOn { get; set; }
    }
}