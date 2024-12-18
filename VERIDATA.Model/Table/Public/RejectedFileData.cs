using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Public
{
    [Table("rejected_file_data")]
    public class RejectedFileData
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("rejected_id", TypeName = DbDataType._biginteger)]
        public int RejectedId { get; set; }

        [Column("appointee_id", TypeName = DbDataType._biginteger)]
        public int? AppointeeId { get; set; }

        [Column("reject_state", TypeName = DbDataType._integer)]
        public int RejectState { get; set; }

        [Column("reject_reason", TypeName = DbDataType._textmax)]
        public string? RejectReason { get; set; }

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