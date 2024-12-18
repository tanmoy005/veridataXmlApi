using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class RawDataProcessRequest
    {
        public List<RawDataRequest> RawDataList { get; set; }
        public int UserId { get; set; }
        public bool? IsUnprocessed { get; set; }
    }

    public class RawDataRequest
    {
        [Required]
        public int id { get; set; }

        public int fileId { get; set; }
        public bool? isChecked { get; set; }
    }
}