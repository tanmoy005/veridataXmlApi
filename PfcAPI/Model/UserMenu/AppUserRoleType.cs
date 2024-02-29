using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PfcAPI.Infrastucture.utility;
using PfcAPI.Model.Company;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PfcAPI.Model.User
{
    [Table("AppUserRoleType")]
    public class AppUserRoleType
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("u_roleid", TypeName = PgDataType._biginteger)]
        public int URoleId { get; set; }

        [Column("role_name", TypeName = PgDataType._text100)]
        public string? RoleName { get; set; }

        [Column("role_description", TypeName = PgDataType._textmax)]
        public string? Description { get; set; }

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

        [ForeignKey("u_roleid")]
        public ICollection<ApplicationUsers> ApplicationUsers { get; set; }
    }
}
