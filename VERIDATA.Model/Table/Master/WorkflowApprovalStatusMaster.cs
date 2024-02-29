using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Master
{

    [Table("workflow_approval_status_master", Schema = "master")]
    public class WorkflowApprovalStatusMaster
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("appvl_status_id", TypeName = DbDataType._biginteger)]
        public int AppvlStatusId { get; set; }

        [Required]
        [Column("appvl_status_code", TypeName = DbDataType._text20)]
        public string? AppvlStatusCode { get; set; }

        [Column("appvl_status_desc", TypeName = DbDataType._text100)]
        public string? AppvlStatusDesc { get; set; }

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
