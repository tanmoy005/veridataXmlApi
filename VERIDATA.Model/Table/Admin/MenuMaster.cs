using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Admin
{
    [Table("menu_master", Schema = "admin")]

    public class MenuMaster
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("menu_id", TypeName = DbDataType._integer)]
        public int MenuId { get; set; }

        [Column("parent_menu_id", TypeName = DbDataType._integer)]
        public int ParentMenuId { get; set; }

        [Column("menu_title", TypeName = DbDataType._text50)]
        public string? MenuTitle { get; set; }

        [Column("menu_alias", TypeName = DbDataType._text20)]
        public string? MenuAlias { get; set; }


        [Column("menu_desc", TypeName = DbDataType._text100)]
        public string? MenuDesc { get; set; }

        [Column("menu_level", TypeName = DbDataType._integer)]
        public int menu_level { get; set; }

        [Column("menu_action", TypeName = DbDataType._text100)]
        public string? menu_action { get; set; }

        [Column("menu_icon_url", TypeName = DbDataType._text200)]
        public string? menu_icon_url { get; set; }

        [Column("seq_no", TypeName = DbDataType._integer)]
        public int SeqNo { get; set; }

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

        [ForeignKey("menu_id")]
        public ICollection<MenuRoleMapping> MenuRoleMapping { get; set; }

    }
}
