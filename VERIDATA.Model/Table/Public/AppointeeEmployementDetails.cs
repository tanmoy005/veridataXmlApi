using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Public
{
    [Table("appointee_employement_details")]
    public class AppointeeEmployementDetails
    {

        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("employement_det_id", TypeName = DbDataType._biginteger)]
        public int EmploymentDetailsId { get; set; }

        [Required]
        [Column("appointee_id", TypeName = DbDataType._biginteger)]
        public int? AppointeeId { get; set; }

        [Column("type_code", TypeName = DbDataType._text50)]
        public string? TypeCode { get; set; }

        [Column("subtype", TypeName = DbDataType._text50)]
        public string? SubTypeCode { get; set; }
        
        [Column("data_info", TypeName = DbDataType._varbinary)]
        public byte[]? DataInfo { get; set; }

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
