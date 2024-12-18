using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Config
{
    [Table("error_logs", Schema = "config")]
    public class CustomError
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("log_id", TypeName = DbDataType._biginteger)]
        public int Id { get; set; }

        [Column("log_info", TypeName = DbDataType._textmax)]
        public string Values { get; set; }

        [Column("log_date")]
        public DateTime LogDate { get; set; }
    }
}