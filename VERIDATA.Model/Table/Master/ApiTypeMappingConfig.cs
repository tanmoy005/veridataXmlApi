using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VERIDATA.Model.utility;
using System.ComponentModel;

namespace VERIDATA.Model.Table.Master
{
    [Table("api_type_mapping", Schema = "master")]
    public class ApiTypeMappingConfig
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = DbDataType._biginteger)]
        public int Id { get; set; }

        [Column("api_type_id", TypeName = DbDataType._text50)]
        public int? TypeId { get; set; }

        [Column("api_name", TypeName = DbDataType._text100)]
        public string? ApiName { get; set; }

        [Column("api_base_url", TypeName = DbDataType._text50)]
        public string? BaseUrl { get; set; }

        [Column("api_url", TypeName = DbDataType._text50)]
        public string? Url { get; set; }

        //[Column("provider", TypeName = DbDataType._text50)]
        //public string? Provider { get; set; }

        [Column("api_Priority", TypeName = DbDataType._integer)]  // mGhosh filed added
        public int apiPriority { get; set; } 

        [Column("active_status", TypeName = DbDataType._boolean)]
        [DefaultValue(true)]
        public bool? ActiveStatus { get; set; }

        [Column("created_on", TypeName = DbDataType._datetime)]
        public DateTime? CreatedOn { get; set; }

        [Column("created_by", TypeName = DbDataType._biginteger)]
        public int CreatedBy { get; set; }

    }
}
