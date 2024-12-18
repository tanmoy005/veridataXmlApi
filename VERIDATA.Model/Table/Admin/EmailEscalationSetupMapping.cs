using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Admin
{
    [Table("escalation_setup", Schema = "admin")]
    public class EmailEscalationSetupMapping
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("setup_id", TypeName = DbDataType._integer)]
        public int Id { get; set; }

        [Required]
        [Column("level_id", TypeName = DbDataType._integer)]
        public int LevelId { get; set; }

        [Required]
        [Column("case_id", TypeName = DbDataType._integer)]
        public int CaseId { get; set; }

        [Column("email_id", TypeName = DbDataType._text50)]
        public string? EmailId { get; set; }

        [DefaultValue(false)]
        [Column("setup_option", TypeName = DbDataType._boolean)]
        public bool? SetupStatus { get; set; }

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