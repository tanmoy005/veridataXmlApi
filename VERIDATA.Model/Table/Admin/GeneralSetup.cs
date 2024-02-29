using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Admin
{
    [Table("generalSetup", Schema = "admin")]
    public class GeneralSetup
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("id", TypeName = DbDataType._integer)]
        public int Id { get; set; }
        [Column("critical_no_days", TypeName = DbDataType._integer)]
        public int? CriticalNoOfDays { get; set; }

        [Column("grace_period_days", TypeName = DbDataType._integer)]
        public int? GracePeriod { get; set; }

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
