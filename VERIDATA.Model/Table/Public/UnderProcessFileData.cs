using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Public
{
    [Table("under_process_file_data")]

    public class UnderProcessFileData
    {

        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("underprocess_id", TypeName = DbDataType._biginteger)]
        public int UnderProcessId { get; set; }

        [Column("appointee_id", TypeName = DbDataType._biginteger)]
        public int? AppointeeId { get; set; }

        [Required]
        //[ForeignKey("FileId")]
        [Column("file_id", TypeName = DbDataType._biginteger)]
        public int FileId { get; set; }

        [Column("company_id", TypeName = DbDataType._integer)]
        //[ForeignKey("company_id")]
        public int CompanyId { get; set; }

        [Column("company_name", TypeName = DbDataType._text50)]
        public string? CompanyName { get; set; }

        [Column("candidate_id", TypeName = DbDataType._text100)]
        public string? CandidateId { get; set; }

        [Column("appointee_name", TypeName = DbDataType._text100)]
        public string? AppointeeName { get; set; }

        [Column("appointee_email", TypeName = DbDataType._text50)]
        public string? AppointeeEmailId { get; set; }

        [Column("mobile_no", TypeName = DbDataType._text20)]
        public string? MobileNo { get; set; } //number that varified with aadhar

        [Column("is_pf_verification_req", TypeName = DbDataType._boolean)]
        public bool? IsPFverificationReq { get; set; }

        [Column("joining_date", TypeName = DbDataType._datetime)]
        public DateTime? DateOfJoining { get; set; }

        [Column("offer_date", TypeName = DbDataType._datetime)]
        public DateTime? DateOfOffer { get; set; }

        [Column("epf_wages", TypeName = DbDataType._numeric)]
        public decimal? EPFWages { get; set; }

        [Column("recruitment_hr", TypeName = DbDataType._text50)]
        public string? RecruitmentHR { get; set; }

        [Column("people_manager", TypeName = DbDataType._text50)]
        public string? PeopleManager { get; set; }

        [Column("is_processed", TypeName = DbDataType._boolean)]
        [DefaultValue(true)]
        public bool? IsProcessed { get; set; }

        [Column("level1_email", TypeName = DbDataType._text50)]
        public string? lvl1Email { get; set; }

        [Column("level2_email", TypeName = DbDataType._text50)]
        public string? lvl2Email { get; set; }

        [Column("level3_email", TypeName = DbDataType._text50)]
        public string? lvl3Email { get; set; }

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

    }
}
