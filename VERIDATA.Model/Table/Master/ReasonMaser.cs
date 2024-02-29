using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Master
{

    [Table("reason_master", Schema = "master")]
    public class ReasonMaser
    {

        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("reason_id", TypeName = DbDataType._integer)]
        public int ReasonId { get; set; }

        [Column("reason_type", TypeName = DbDataType._text10)]
        public string? ReasonType { get; set; }

        [Column("reason_category", TypeName = DbDataType._text10)]
        public string? ReasonCategory { get; set; }

        [Column("reason_code", TypeName = DbDataType._text10)]
        public string? ReasonCode { get; set; }

        [Column("reason_info", TypeName = DbDataType._text200)]
        public string? ReasonName { get; set; }

        [Column("reason_remedy", TypeName = DbDataType._textmax)]
        public string? ReasonRemedy { get; set; }

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
