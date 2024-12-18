using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.Table.Admin;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Master
{
    [Table("role_master", Schema = "master")]
    public class RoleMaster
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("role_id", TypeName = DbDataType._integer)]
        public int RoleId { get; set; }

        [Column("role_name", TypeName = DbDataType._text50)]
        public string? RoleName { get; set; }

        [Column("role_desc", TypeName = DbDataType._text100)]
        public string? RoleDesc { get; set; }

        //[Column("roles_email_id", TypeName = DbDataType._text50)]
        //public string? RolesEmailId { get; set; }

        [Column("roles_alias", TypeName = DbDataType._text10)]
        public string? RolesAlias { get; set; }

        [Column("is_company_admin", TypeName = DbDataType._boolean)]
        public bool? IsCompanyAdmin { get; set; }

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

        [ForeignKey("role_id")]
        public ICollection<MenuRoleMapping> MenuRoleMapping { get; set; }

        [ForeignKey("role_id")]
        public ICollection<UserMaster> UserMaster { get; set; }

        [ForeignKey("role_id")]
        public ICollection<RoleUserMapping> RoleUserMapping { get; set; }
    }
}