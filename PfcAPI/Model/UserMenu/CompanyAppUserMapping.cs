using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PfcAPI.Infrastucture.utility;

namespace PfcAPI.Model.User
{
    [Table("CompanyAppUserMapping")]
    public class CompanyAppUserMapping
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = PgDataType._biginteger)]
        public int Id { get; set; }
        [Required]

        [Column("app_users_id", TypeName = PgDataType._biginteger)]
        public int UserId { get; set; }

        [Column("company_id", TypeName = PgDataType._biginteger)]
        public int CompanyId { get; set; }

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
