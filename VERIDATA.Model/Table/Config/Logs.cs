using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;


namespace VERIDATA.Model.Table.Config
{
    [Table("logs", Schema = "config")]
    public class Logs
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("log_id", TypeName = DbDataType._biginteger)]
        public int? AppointeeId { get; set; }

        [Column("log_date", TypeName = DbDataType._datetime)]
        public DateTime? LogDate { get; set; }

        [Column("level", TypeName = DbDataType._textmax)]
        public string? Level { get; set; }

        [Column("category", TypeName = DbDataType._textmax)]
        public string? Category { get; set; }

        [Column("call_site", TypeName = DbDataType._textmax)]
        public string? CallSite { get; set; }

        [Column("line_number", TypeName = DbDataType._textmax)]
        public string? LineNumber { get; set; }

        [Column("message", TypeName = DbDataType._textmax)]
        public string? Message { get; set; }

        [Column("stack_trace", TypeName = DbDataType._textmax)]
        public string? StackTrace { get; set; }

        [Column("arguments", TypeName = DbDataType._textmax)]
        public string? Arguments { get; set; }

        [Column("url", TypeName = DbDataType._textmax)]
        public string? Url { get; set; }

    }
}
