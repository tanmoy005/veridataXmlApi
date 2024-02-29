using PfcAPI.Infrastucture.utility;
using PfcAPI.Tools;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PfcAPI.Model.User
{

    [Table("ApplicationUsers")]

    public class ApplicationUsers
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("app_users_id", TypeName = PgDataType._biginteger)]
        public int UserId { get; set; }

        [Required]
        [Column("users_name", TypeName = PgDataType._text50)]
        public string? UserName { get; set; }

        [Required]
        [Column("password", TypeName = PgDataType._text50)]
        public string? Password { get; set; }

        [Required]
        [Column("email_id", TypeName = PgDataType._text50)]
        public string? EmailId { get; set; }

        [Column("phone", TypeName = PgDataType._text50)]
        public string? Phone { get; set; }

        [Column("u_roleid", TypeName = PgDataType._integer)]
        public int? URoleId { get; set; }

        [Column("active_status", TypeName = PgDataType._boolean)]
        [DefaultValue("true")]
        public bool? ActiveStatus { get; set; }

        [Column("created_by", TypeName = PgDataType._integer)]
        public int? CreatedBy { get; set; }

        [Column("created_on", TypeName = PgDataType._datetime)]
        public DateTime? CreatedOn { get; set; }

        [Column("updated_by", TypeName = PgDataType._integer)]
        public int? UpdatedBy { get; set; }

        [Column("updated_on", TypeName = PgDataType._datetime)]
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey("app_users_id")]
        public ICollection<CompanyAppUserMapping> CompanyAppUserMapping { get; set; }
    }
}
