using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Admin
{
    [Table("menu_action_master", Schema = "admin")]
    public class ActionMaster
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("action_id", TypeName = DbDataType._integer)]
        public int ActionId { get; set; }

        [Column("alias", TypeName = DbDataType._text20)]
        public string? ActionAlias { get; set; }

        [Column("action_type", TypeName = DbDataType._text20)]
        public string? ActionType { get; set; }

        [Column("action_name", TypeName = DbDataType._text100)]
        public string ActionName { get; set; }

        [Column("active_status", TypeName = DbDataType._boolean)]
        [DefaultValue(true)]
        public bool? ActiveStatus { get; set; }

        [Column("created_on", TypeName = DbDataType._datetime)]
        public DateTime? CreatedOn { get; set; }
    }
}