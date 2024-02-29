using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Activity
{

    [Table("appointee_update_activity", Schema = "activity")]
    public class AppointeeDetailsUpdateActivity
    {

        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("activity_id", TypeName = DbDataType._integer)]
        public int ActivityId { get; set; }

        [Column("appointee_id", TypeName = DbDataType._integer)]
        public int AppointeeId { get; set; }

        [Column("update_type", TypeName = DbDataType._text50)]
        public string? Type { get; set; }

        [Column("update_value", TypeName = DbDataType._text100)]
        public string? Value { get; set; }

        [Column("active_status", TypeName = DbDataType._boolean)]
        [DefaultValue(true)]
        public bool? ActiveStatus { get; set; }

        [Column("created_by", TypeName = DbDataType._integer)]
        public int? CreatedBy { get; set; }

        [Column("created_on", TypeName = DbDataType._datetime)]
        public DateTime? CreatedOn { get; set; }

    }
}
