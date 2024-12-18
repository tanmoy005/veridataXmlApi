using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Master
{
    [Table("workflow_state_master", Schema = "master")]
    public class WorkFlowStateMaster
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("state_id", TypeName = DbDataType._biginteger)]
        public int StateId { get; set; }

        [Required]
        [Column("state_name", TypeName = DbDataType._text100)]
        public string? StateName { get; set; }

        [Column("state_decs", TypeName = DbDataType._text200)]
        public string? StateDecs { get; set; }

        [Column("seq_of_flow", TypeName = DbDataType._integer)]
        public int? SeqOfFLow { get; set; }

        [Column("state_alias", TypeName = DbDataType._text20)]
        public string? StateAlias { get; set; }

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