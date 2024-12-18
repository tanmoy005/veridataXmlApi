using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Public
{
    [Table("workflow_details")]
    public class WorkFlowDetails
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("work_flow_det_id", TypeName = DbDataType._biginteger)]
        public int WorkFlowDetId { get; set; }

        [Required]
        //[ForeignKey("AppointeeProcess")]
        [Column("appointee_id", TypeName = DbDataType._biginteger)]
        public int? AppointeeId { get; set; }

        [Required]
        //[ForeignKey("WorkFlowStateMaster")]
        [Column("state_id", TypeName = DbDataType._biginteger)]
        public int StateId { get; set; }

        [Column("appvl_status_id", TypeName = DbDataType._integer)]
        public int AppvlStatusId { get; set; }

        [Column("state_alias", TypeName = DbDataType._text20)]
        public string? StateAlias { get; set; }

        [Column("remarks", TypeName = DbDataType._textmax)]
        public string? Remarks { get; set; }

        [Column("action_taken_at", TypeName = DbDataType._datetime)]
        public DateTime? ActionTakenAt { get; set; }

        [Column("reprocess_count", TypeName = DbDataType._integer)]
        public int? ReprocessCount { get; set; }

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