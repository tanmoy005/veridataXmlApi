using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.DataAccess.Request
{
    public class AppointeeDataSaveInFilesRequset
    {
        [Required]
        public int AppointeeId { get; set; }

        public string? AppointeeCode { get; set; }
        public string? mimetype { get; set; }

        [Required]
        public int UserId { get; set; }

        public string? FileUploaded { get; set; }
        public string? FileTypeAlias { get; set; }
        public string? FileSubType { get; set; }
    }
}