using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Admin
{
    [Table("menu_role_mapping", Schema = "admin")]
    public class MenuRoleMapping
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("menu_role_map_id", TypeName = DbDataType._integer)]
        public int MenuRoleMapId { get; set; }

        [Column("role_id", TypeName = DbDataType._integer)]
        public int RoleId { get; set; }

        [Column("menu_id", TypeName = DbDataType._integer)]
        public int MenuId { get; set; }

        [Column("action_id", TypeName = DbDataType._integer)]
        public int ActionId { get; set; }

        //[Column("is_create", TypeName = DbDataType._char)]
        //[DefaultValue("Y")]
        //public string? IsCreate { get; set; }
        //[Column("is_retrieve", TypeName = DbDataType._char)]
        //[DefaultValue("Y")]
        //public string? IsRetrieve { get; set; }

        //[Column("is_update", TypeName = DbDataType._char)]
        //[DefaultValue("Y")]
        //public string? IsUpdate { get; set; }

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