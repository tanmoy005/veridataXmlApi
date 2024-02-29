using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VERIDATA.Model.utility;

namespace VERIDATA.Model.Table.Master
{

    [Table("user_types", Schema = "master")]
    public class UserTypes
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_type_id", TypeName = DbDataType._integer)]
        public int UserTypeId { get; set; }

        [Column("user_type_name", TypeName = DbDataType._text50)]
        public string? UserTypeName { get; set; }

        [Column("user_type_desc", TypeName = DbDataType._text100)]
        public string? UserTypeDesc { get; set; }

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

        //[ForeignKey("user_type_id")]
        //public ICollection<UserMaster> UserMaster { get; set; }
    }
}
