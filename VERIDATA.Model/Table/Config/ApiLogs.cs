using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Config
{

    [Table("api_logs", Schema = "config")]
    public class ApiLogs
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("log_id", TypeName = DbDataType._biginteger)]
        public int Id { get; set; }

        [Column("appointeeId", TypeName = DbDataType._biginteger)]
        public int AppointeeId { get; set; }

        [Column("userId", TypeName = DbDataType._biginteger)]
        public int UserId { get; set; }

        [Column("method_name", TypeName = DbDataType._text100)]
        public string MethodName { get; set; }

        [Column("method_type", TypeName = DbDataType._text50)]
        public string MethodType { get; set; }

        [Column("payload", TypeName = DbDataType._textmax)]
        public string Payload { get; set; }

        [Column("created_on")]
        public DateTime LogDate { get; set; }

        [Column("created_by", TypeName = DbDataType._biginteger)]
        public int CreatedBy { get; set; }

    }

}
