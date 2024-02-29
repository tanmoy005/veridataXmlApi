using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Master
{
    [Table("company_wise_upld_master", Schema = "master")]

    public class CompanyWiseUpldMaster
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("req_document_id", TypeName = DbDataType._biginteger)]
        public int ReqDocumentId { get; set; }

        [Column("company_id", TypeName = DbDataType._integer)]
        [ForeignKey("company_id")]
        public int CompanyId { get; set; }

        [Required]
        [Column("document_name", TypeName = DbDataType._text50)]
        public string? DocumentName { get; set; }

        [Required]
        [Column("doc_desc", TypeName = DbDataType._text200)]
        public string? DocumentDescription { get; set; }

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

        //[ForeignKey("req_document_id")]
        //public ICollection<AppointeeUploadDetails> AppointeeUploadDetails { get; set; }

    }
}
