using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Public
{
    [Table("appointee_consent_maaping")]
    public class AppointeeConsentMapping
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("consent_id", TypeName = DbDataType._biginteger)]
        public int ConsentId { get; set; }

        [Column("appointee_id", TypeName = DbDataType._biginteger)]
        public int? AppointeeId { get; set; }

        [Column("candidate_id", TypeName = DbDataType._text100)]
        public string? CandidateId { get; set; }

        [Column("consent_status", TypeName = DbDataType._integer)]
        public int? ConsentStatus { get; set; }

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