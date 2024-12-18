using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Public
{
    [Table("upload_appointee_counter")]
    public class UploadAppointeeCounter
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = DbDataType._biginteger)]
        public int Id { get; set; }

        [Column("file_id", TypeName = DbDataType._biginteger)]
        public int? FileId { get; set; }

        [Column("count", TypeName = DbDataType._integer)]
        public int Count { get; set; }

        [Column("created_by", TypeName = DbDataType._integer)]
        public int? CreatedBy { get; set; }

        [Column("created_on", TypeName = DbDataType._datetime)]
        public DateTime? CreatedOn { get; set; }
    }
}