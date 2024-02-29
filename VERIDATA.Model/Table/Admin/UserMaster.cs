using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Admin
{
    [Table("user_master", Schema = "admin")]
    public class UserMaster
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id", TypeName = DbDataType._biginteger)]
        public int UserId { get; set; }

        [Column("ref_appointee_id", TypeName = DbDataType._biginteger)]
        public int? RefAppointeeId { get; set; }

        [Column("user_code", TypeName = DbDataType._text50)]
        public string? UserCode { get; set; }

        [Column("candidate_id", TypeName = DbDataType._text50)]
        public string? CandidateId { get; set; }

        [Column("users_name", TypeName = DbDataType._text50)]
        public string? UserName { get; set; }

        [Column("date_of_birth", TypeName = DbDataType._datetime)]
        public DateTime? DOB { get; set; }

        [Column("email_id", TypeName = DbDataType._text50)]
        public string? EmailId { get; set; }

        [Column("contact_no", TypeName = DbDataType._text50)]
        public string? ContactNo { get; set; }

        [Column("user_type_id", TypeName = DbDataType._integer)]
        public int UserTypeId { get; set; }

        [Column("role_id", TypeName = DbDataType._integer)]
        public int RoleId { get; set; }

        [Column("active_status", TypeName = DbDataType._boolean)]
        [DefaultValue(true)]
        public bool? ActiveStatus { get; set; }

        [Column("cur_status", TypeName = DbDataType._boolean)]
        [DefaultValue(true)]
        public bool? CurrStatus { get; set; }

        [Column("created_by", TypeName = DbDataType._integer)]
        public int? CreatedBy { get; set; }

        [Column("created_on", TypeName = DbDataType._datetime)]
        public DateTime? CreatedOn { get; set; }

        [Column("updated_by", TypeName = DbDataType._integer)]
        public int? UpdatedBy { get; set; }

        [Column("updated_on", TypeName = DbDataType._datetime)]
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey("user_id")]
        public ICollection<RoleUserMapping> RoleUserMapping { get; set; }

        [ForeignKey("user_id")]
        public ICollection<UserAuthentication> UserAuthentication { get; set; }

        [ForeignKey("user_id")]
        public ICollection<UserAuthenticationHist> UserAuthenticationHist { get; set; }
    }
}
