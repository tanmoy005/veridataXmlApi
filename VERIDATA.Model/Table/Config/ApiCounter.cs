using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Config
{

    [Table("api_couter_log", Schema = "config")]
    public class ApiCounter
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = DbDataType._biginteger)]
        public int Id { get; set; }

        [Column("provider_name", TypeName = DbDataType._text50)]
        public string? ProviderName { get; set; }

        [Column("api_name", TypeName = DbDataType._text100)]
        public string? ApiName { get; set; }

        [Column("api_url", TypeName = DbDataType._text50)]
        public string? Url { get; set; }

        [Column("api_type", TypeName = DbDataType._text50)]
        public string? Type { get; set; }

        [Column("payload", TypeName = DbDataType._textmax)]
        public string? Payload { get; set; }

        [Column("api_status", TypeName = DbDataType._integer)]
        public int? Status { get; set; }

        [Column("created_on", TypeName = DbDataType._datetime)]
        public DateTime? CreatedOn { get; set; }

        [Column("created_by", TypeName = DbDataType._biginteger)]
        public int CreatedBy { get; set; }

    }

}
