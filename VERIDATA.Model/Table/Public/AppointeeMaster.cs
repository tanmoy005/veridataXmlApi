using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Public
{
    [Table("appointee_master")]
    public class AppointeeMaster
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("appointee_id", TypeName = DbDataType._biginteger)]
        public int? AppointeeId { get; set; }

        [Column("candidate_id", TypeName = DbDataType._text100)]
        public string? CandidateId { get; set; }

        [Column("appointee_name", TypeName = DbDataType._text100)]
        public string? AppointeeName { get; set; }

        [Column("appointee_email", TypeName = DbDataType._text50)]
        public string? AppointeeEmailId { get; set; }

        [Column("mobile_no", TypeName = DbDataType._text20)]
        public string? MobileNo { get; set; } //number that varified with aadhar

        [Required]
        [Column("file_id", TypeName = DbDataType._biginteger)]
        public int FileId { get; set; }

        [Column("joining_date", TypeName = DbDataType._datetime)]
        public DateTime? DateOfJoining { get; set; }

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

        [ForeignKey("appointee_id")]
        public ICollection<AppointeeDetails> AppointeeDetails { get; set; }

        [ForeignKey("appointee_id")]
        public ICollection<AppointeeUploadDetails> AppointeeUploadDetails { get; set; }

        [ForeignKey("appointee_id")]
        public ICollection<ProcessedFileData> ProcessedFileData { get; set; }

        [ForeignKey("appointee_id")]
        public ICollection<RejectedFileData> RejectedFileData { get; set; }

        [ForeignKey("appointee_id")]
        public ICollection<AppointeeReasonMappingData> AppointeeReasonMappingData { get; set; }
    }
}
