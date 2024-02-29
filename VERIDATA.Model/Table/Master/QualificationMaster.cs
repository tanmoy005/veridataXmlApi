using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Master
{
    [Table("qualification_master", Schema = "master")]
    public class QualificationMaster
    {

        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("qualification_id", TypeName = DbDataType._integer)]
        public int QualificationId { get; set; }

        [Column("qualification_name", TypeName = DbDataType._text100)]
        public string? QualificationName { get; set; }

        [Column("qualification_code", TypeName = DbDataType._char)]
        public string? QualificationCode { get; set; }

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
