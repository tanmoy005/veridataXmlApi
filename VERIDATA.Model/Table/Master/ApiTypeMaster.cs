using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VERIDATA.Model.utility;
using System.ComponentModel;

namespace VERIDATA.Model.Table.Master
{
    [Table("api_type_master", Schema = "master")]
    public class ApiTypeMaster
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = DbDataType._biginteger)]
        public int Id { get; set; }

        [Column("api_type_code", TypeName = DbDataType._text50)]
        public string? TypeCode { get; set; }

        [Column("api_desc", TypeName = DbDataType._text100)]
        public string? ApiDesc { get; set; }

        [Column("provider", TypeName = DbDataType._text50)]
        public string? Provider { get; set; }

        [Column("api_priority", TypeName = DbDataType._integer)]  // mGhosh new field
        public int apiPriotity { get; set; }

        [Column("active_status", TypeName = DbDataType._boolean)]
        [DefaultValue(true)]
        public bool? ActiveStatus { get; set; }

        [Column("created_on", TypeName = DbDataType._datetime)]
        public DateTime? CreatedOn { get; set; }

        [Column("created_by", TypeName = DbDataType._biginteger)]
        public int CreatedBy { get; set; }

    }
}
