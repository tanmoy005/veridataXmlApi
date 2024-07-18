//using PfcAPI.Model.User;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.Table.Public;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Admin
{
    [Table("company", Schema = "admin")]
    public class CompanyInfo
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("company_id", TypeName = DbDataType._integer)]
        public int Id { get; set; }
        [Column("company_name", TypeName = DbDataType._text100)]
        public string? CompanyName { get; set; }

        [Column("company_alias", TypeName = DbDataType._text50)]
        public string? CompanyAlias { get; set; }

        [Column("company_address", TypeName = DbDataType._text200)]
        public string? CompanyAddress { get; set; }

        [Column("company_city", TypeName = DbDataType._text50)]
        public string? City { get; set; }

        [Column("no_doc_upld_req", TypeName = DbDataType._integer)]
        public int? NoDocUpldReq { get; set; }

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

        //[ForeignKey("company_id")]
        //public ICollection<CompanyUsers> CompanyUsers { get; set; }

        //[ForeignKey("company_id")]
        //public ICollection<CompanyAppUserMapping> CompanyAppUserMapping { get; set; } 

        [ForeignKey("company_id")]
        public ICollection<AppointeeDetails> AppointeeDetails { get; set; }

        [ForeignKey("company_id")]
        public ICollection<RawFileData> RawFileData { get; set; }

        [ForeignKey("company_id")]
        public ICollection<UnderProcessFileData> UnderProcessFileData { get; set; }

        [ForeignKey("company_id")]
        public ICollection<UnProcessedFileData> UnProcessedFileData { get; set; }

        [ForeignKey("company_id")]
        public ICollection<AppointeeUploadDetails> AppointeeUploadDetails { get; set; }


    }
}
