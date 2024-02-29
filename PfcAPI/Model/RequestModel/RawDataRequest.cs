using System.ComponentModel.DataAnnotations;

namespace PfcAPI.Model.RequestModel
{
    public class RawDataRequest
    {
        [Required]
        public int id { get; set; }
        public int fileId { get; set; }
        public bool? isChecked { get; set; }

    }
}
