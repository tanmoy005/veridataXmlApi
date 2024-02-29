using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Activity
{

    [Table("activity_master", Schema = "activity")]
    public class ActivityMaster
    {

        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("activity_id", TypeName = DbDataType._integer)]
        public int ActivityId { get; set; }

        [Column("activity_code", TypeName = DbDataType._text10)]
        public string? ActivityCode { get; set; }

        [Column("activity_type", TypeName = DbDataType._text50)]
        public string? ActivityType { get; set; }

        [Column("activity_name", TypeName = DbDataType._text100)]
        public string? ActivityName { get; set; }

        [Column("activity_info", TypeName = DbDataType._text200)]
        public string? ActivityInfo { get; set; }

        [Column("activity_color", TypeName = DbDataType._text100)]
        public string? ActivityColor { get; set; }

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
