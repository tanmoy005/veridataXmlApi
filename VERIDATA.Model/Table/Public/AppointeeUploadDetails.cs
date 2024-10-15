using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Public
{
    [Table("upload_details")]
    public class AppointeeUploadDetails
    {

        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("upload_det_id", TypeName = DbDataType._biginteger)]
        public int UploadDetailsId { get; set; }

        [Required]
        // [ForeignKey("AppointeeProcess")]
        [Column("appointee_id", TypeName = DbDataType._biginteger)]
        public int? AppointeeId { get; set; }

        [Required]
        // [ForeignKey("AppointeeDetails")]
        [Column("upload_type_id", TypeName = DbDataType._integer)]
        public int UploadTypeId { get; set; }

        [Column("upload_path", TypeName = DbDataType._text200)]
        public string? UploadPath { get; set; }

        [Column("file_name", TypeName = DbDataType._text200)]
        public string? FileName { get; set; }

        //[Column("file_extension", TypeName = DbDataType._text10)]
        //public string? FileExtension { get; set; }

        [Column("is_path_refered", TypeName = "VARCHAR(1)")]
        public string? IsPathRefered { get; set; }

        [Column("mime_type", TypeName = DbDataType._text100)]
        public string? MimeType { get; set; }

        [Column("upload_type_code", TypeName = DbDataType._text50)]
        public string? UploadTypeCode { get; set; }

        [Column("upload_subtype", TypeName = DbDataType._text50)]
        public string? UploadSubTypeCode { get; set; }

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

        [Column("file_content", TypeName = DbDataType._varbinary)]  // mGhosh Added new col
        public byte[]? Content { get; set; }
    }
}
