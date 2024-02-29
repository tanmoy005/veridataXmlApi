using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PfcAPI.Infrastucture.utility;

namespace PfcAPI.Model.Appointee
{
    [Table("Appointee")]
    public class Appointee
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("appointee_id", TypeName = PgDataType._biginteger)]
        public int Appointee_Id { get; set; }

        [Required]
        [Column("underprocess_fileid", TypeName = PgDataType._biginteger)]
        public int UnderProcessFileId { get; set; }

        
        [Column("processedfile_id", TypeName = PgDataType._biginteger)]
        public int? ProcessedFileId { get; set; }

        [Column("IsEmailSent", TypeName = PgDataType._biginteger)]
        public bool? IsEmailSent { get; set; }

        
        public string? Appointee_Name { get; set; }
        [Required]
        [ForeignKey("Company")]
        public int? AssociatedCompanyId { get; set; }
        [Required]
        [EmailAddress]
        public string? EmailId { get; set; }
        [Phone]
        public string? Phone { get; set; }
       
        [DefaultValue("true")]
        public bool? ActiveStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

}
