using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.Table.Public;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Master
{
    [Table("upload_type_master", Schema = "master")]
    public class UploadTypeMaster
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("upload_type_id", TypeName = DbDataType._integer)]
        public int UploadTypeId { get; set; }

        [Column("upload_type_name", TypeName = DbDataType._text50)]
        public string? UploadTypeName { get; set; }

        [Column("upload_type_code", TypeName = DbDataType._text50)]
        public string? UploadTypeCode { get; set; }     //COMMENT 'Describes the upload type and the contex from where the upload is done.',

        [Column("upload_type_desc", TypeName = DbDataType._text200)]
        public string? UploadTypeDesc { get; set; }  //COMMENT 'Detail descrintion of the upload context. Example- addhar upload or pan upload etc.',

        [Column("upload_type_category", TypeName = DbDataType._text50)]
        public string? UploadTypeCategory { get; set; }

        [Column("category_name", TypeName = DbDataType._text50)]
        public string? CategoryName { get; set; }

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

        //[Column("upload_doc_type", TypeName = DbDataType._text50)]
        //  public string? UploadDocType { get; set; }

        [ForeignKey("upload_type_id")]
        public ICollection<AppointeeUploadDetails> AppointeeUploadDetails { get; set; }
    }
}