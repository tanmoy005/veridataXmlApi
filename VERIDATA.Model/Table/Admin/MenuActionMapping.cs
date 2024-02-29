using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Admin
{
    [Table("menu_action_mapping", Schema = "admin")]
    public class MenuActionMapping
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("mapping_id", TypeName = DbDataType._integer)]
        public int MenuMappingId { get; set; }

        [Column("menu_id", TypeName = DbDataType._integer)]
        public int MenuId { get; set; }

        [Column("action_id", TypeName = DbDataType._integer)]
        public int ActionId { get; set; }

        [Column("active_status", TypeName = DbDataType._boolean)]
        [DefaultValue(true)]
        public bool? ActiveStatus { get; set; }


    }
}
