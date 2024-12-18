using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Public
{
    [Table("uploaded_xls_file")]
    public class UploadedXSLfile
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("file_id", TypeName = DbDataType._biginteger)]
        public int FileId { get; set; }

        [Column("file_name", TypeName = DbDataType._text100)]
        public string? FileName { get; set; }

        [Column("file_path", TypeName = DbDataType._textmax)]
        public string? FilePath { get; set; }

        [Column("company_id", TypeName = DbDataType._integer)]
        public int? CompanyId { get; set; }

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

        [ForeignKey("file_id")]
        public ICollection<RawFileData> RawFileData { get; set; }

        [ForeignKey("file_id")]
        public ICollection<UnderProcessFileData> UnderProcessFileData { get; set; }

        [ForeignKey("file_id")]
        public ICollection<UnProcessedFileData> UnProcessedFileData { get; set; }

        [ForeignKey("file_id")]
        public ICollection<ProcessedFileData> ProcessedFileData { get; set; }
    }
}