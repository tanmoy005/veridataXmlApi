using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Request
{
   public  class AppointeeApprovePensionRequest
    {
        [Required]
        public int appointeeId { get; set; }

        [Required]
        public bool IsManualPassbook { get; set; }
        [Required]
        public int userId { get; set; }
    }
}
