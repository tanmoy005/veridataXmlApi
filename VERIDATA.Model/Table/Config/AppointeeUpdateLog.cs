using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Config
{
    [Table("appointee_update_log", Schema = "config")]
    public class AppointeeUpdateLog
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = DbDataType._biginteger)]
        public long Id { get; set; }

        [Column("appointee_id", TypeName = DbDataType._biginteger)]
        public long? AppointeeId { get; set; }

        [Column("candidate_id", TypeName = DbDataType._text20)]
        public string? CandidateId { get; set; }

        [Column("update_type", TypeName = DbDataType._text50)]
        public string? UpdateType { get; set; }

        [Column("update_value", TypeName = DbDataType._text100)]
        public string? UpdateValue { get; set; }

        [Column("created_on", TypeName = DbDataType._datetime)]
        public DateTime? CreatedOn { get; set; }

        [Column("created_by", TypeName = DbDataType._biginteger)]
        public int CreatedBy { get; set; }
    }
}