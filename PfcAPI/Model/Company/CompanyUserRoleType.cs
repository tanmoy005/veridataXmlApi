using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PfcAPI.Infrastucture.utility;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PfcAPI.Model.Company
{
    [Table("CompanyUserRoleType")]
    public class CompanyUserRoleType
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("c_roleid", TypeName = PgDataType._biginteger)]
        public int CompanyRoleId { get; set; }

        [Column("c_rolename", TypeName = PgDataType._text50)]
        public string? CompanyRoleName { get; set; }

        [Column("role_description", TypeName = PgDataType._text200)]
        public string? CompanyRoleDescription { get; set; }

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

        [ForeignKey("c_roleId")]
        public ICollection<CompanyUsers> CompanyUsers { get; set; }
    }
}
