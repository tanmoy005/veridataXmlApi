using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PfcAPI.Infrastucture.utility;

namespace PfcAPI.Model.Appointee
{
    [Table("AppointeeProcess")]
    public class AppointeeProcess
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("appointee_id", TypeName = PgDataType._biginteger)]
        public int AppointeeId { get; set; }

        [Column("underprocess_id", TypeName = PgDataType._biginteger)]
        public int UnderProcessId { get; set; }

        [Required]
        //[ForeignKey("FileId")]
        [Column("file_id", TypeName = PgDataType._biginteger)]
        public int FileId { get; set; }

        [Column("processedfile_id", TypeName = PgDataType._biginteger)]
        public int? ProcessedFileId { get; set; }

        [Column("isPF_verification_req", TypeName = PgDataType._boolean)]
        public bool? IsPFverificationReq { get; set; }


        [Column("is_processed", TypeName = PgDataType._boolean)]
        [DefaultValue("true")]
        public bool? IsProcessed { get; set; }

        [Column("created_by", TypeName = PgDataType._integer)]
        public int? CreatedBy { get; set; }

        [Column("created_on", TypeName = PgDataType._datetime)]
        public DateTime? CreatedOn { get; set; }

        [Column("updated_by", TypeName = PgDataType._integer)]
        public int? UpdatedBy { get; set; }

        [Column("updated_on", TypeName = PgDataType._datetime)]
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey("appointee_id")]
        public ICollection<AppointeeDetails> AppointeeDetails { get; set; }
        [ForeignKey("appointee_id")]
        public ICollection<AppointeeUploadDetails> AppointeeUploadDetails { get; set; } 
        
        [ForeignKey("appointee_id")]
        public ICollection<WorkFlowDetails> WorkFlowDetails { get; set; }
    }
}
