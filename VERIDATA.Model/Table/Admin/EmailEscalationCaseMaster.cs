using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Admin
{
    [Table("escalation_case_master", Schema = "admin")]
    public class EmailEscalationCaseMaster
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("case_id", TypeName = DbDataType._integer)]
        public int CaseId { get; set; }

        [Column("setup_code", TypeName = DbDataType._text10)]
        public string? SetupCode { get; set; }

        [Column("setup_alias", TypeName = DbDataType._text10)]
        public string? SetupAlias { get; set; }

        [Column("setup_desc", TypeName = DbDataType._text100)]
        public string? SetupDesc { get; set; }

        [DefaultValue(true)]
        [Column("active_status", TypeName = DbDataType._boolean)]
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