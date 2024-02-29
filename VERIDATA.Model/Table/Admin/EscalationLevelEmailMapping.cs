using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Admin
{
    [Table("escaltionlevel_email_mapping", Schema = "admin")]
    public class EscalationLevelEmailMapping
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("map_id", TypeName = DbDataType._integer)]
        public int EmailMapId { get; set; }

        [Column("level_id", TypeName = DbDataType._integer)]
        public int LevelId { get; set; }

        [Column("email", TypeName = DbDataType._text100)]
        public string? Email { get; set; }

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
