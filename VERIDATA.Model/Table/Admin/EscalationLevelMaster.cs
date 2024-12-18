using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Admin
{
    [Table("escalationlevel_master", Schema = "admin")]
    public class EscalationLevelMaster
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("level_id", TypeName = DbDataType._integer)]
        public int LevelId { get; set; }

        [Column("level_name", TypeName = DbDataType._text100)]
        public string? LevelName { get; set; }

        [Column("level_code", TypeName = DbDataType._text10)]
        public string? LevelCode { get; set; }

        [Column("no_of_days", TypeName = DbDataType._integer)]
        public int? NoOfDays { get; set; }

        [Column("setup_alias", TypeName = DbDataType._text10)]
        public string? SetupAlias { get; set; }

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

        [ForeignKey("level_id")]
        public ICollection<EmailEscalationSetupMapping> EmailEscalationSetupMapping { get; set; }

        [ForeignKey("level_id")]
        public ICollection<EscalationLevelEmailMapping> EscalationLevelEmailMapping { get; set; }
    }
}