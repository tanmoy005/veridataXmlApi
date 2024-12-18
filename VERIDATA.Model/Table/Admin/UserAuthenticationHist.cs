using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Admin
{
    [Table("user_authentication_hist", Schema = "admin")]
    public class UserAuthenticationHist
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("autho_hist_id", TypeName = DbDataType._integer)]
        public int AuthoHistId { get; set; }

        [Column("user_id", TypeName = DbDataType._biginteger)]
        public int UserId { get; set; }

        [Column("client_id", TypeName = DbDataType._text100)]
        public string? ClientId { get; set; }

        [Column("entry_time", TypeName = DbDataType._datetime)]
        public DateTime? EntryTime { get; set; }

        [Column("exit_time", TypeName = DbDataType._datetime)]
        public DateTime? ExitTime { get; set; }

        [Column("ip_address", TypeName = DbDataType._text50)]
        public string? IPAddress { get; set; }

        [Column("gip_address", TypeName = DbDataType._text50)]
        public string? GIPAddress { get; set; }

        [Column("browser_name", TypeName = DbDataType._text50)]
        public string? BrowserName { get; set; }

        [Column("token_no", TypeName = DbDataType._text50)]
        public string? TokenNo { get; set; }

        [Column("refresh_token_expiry_time", TypeName = DbDataType._datetime)]
        public DateTime? RefreshTokenExpiryTime { get; set; }

        [Column("otp_no", TypeName = DbDataType._text10)]
        public string? Otp { get; set; }

        [Column("otp_expiry_time", TypeName = DbDataType._datetime)]
        public DateTime? OtpExpiryTime { get; set; }

        [Column("exit_status", TypeName = DbDataType._text10)]
        [DefaultValue("N")]
        public string? ExitStatus { get; set; } //COMMENT 'N-Normal logout, A-Abnormal',

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