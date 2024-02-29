using PfcAPI.Infrastucture.utility;
using PfcAPI.Tools;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PfcAPI.Model.User
{

    [Table("AppointeeUsers")]

    public class AppointeeUsers
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("appointee_userid", TypeName = PgDataType._biginteger)]
        public int Id { get; set; }

        [Column("user_name", TypeName = PgDataType._text100)]
        [Required]
        public string? UserName { get; set; }

        [Column("password", TypeName = PgDataType._text100)]
        [Required]
        public string? Password { get; set; }

        [Required]
        [Column("email_id", TypeName = PgDataType._text100)]
        public string? EmailId { get; set; }

        [Column("phone_no", TypeName = PgDataType._text50)]
        public string? Phone { get; set; }

        [Column("active_status", TypeName = PgDataType._boolean)]
        [DefaultValue("true")]
        public bool? ActiveStatus { get; set; }

        [Column("created_by", TypeName = PgDataType._integer)]
        public int? CreatedBy { get; set; }

        [Column("created_on", TypeName = PgDataType._datetime)]
        public DateTime? CreatedOn { get; set; }

        [Column("updated_by", TypeName = PgDataType._integer)]
        public int? UpdatedBy { get; set; }

        [Column("updated_on", TypeName = PgDataType._datetime)]
        public DateTime? UpdatedOn { get; set; }
    }
}
