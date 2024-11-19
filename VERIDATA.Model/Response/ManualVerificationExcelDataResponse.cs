using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VERIDATA.Model.DataAccess;

namespace VERIDATA.Model.Response
{
    public class ManualVerificationExcelDataResponse
    {
        public int id { get; set; }
        [DisplayName("Appointee Id")]
        public string? candidateId { get; set; }
        public int? appointeeId { get; set; }
        [DisplayName("Name")]
        public string? appointeeName { get; set; }
        [DisplayName("Email")]
        public string? appointeeEmailId { get; set; }
        [DisplayName("Mobile No")]
        public string? mobileNo { get; set; } //number that varified with aadhar
        [DisplayName("Joining Date")]
        public string? dateOfJoining { get; set; }
        [DisplayName("Submitted")] // Yes/No
        public bool? isDocSubmitted { get; set; }
        [DisplayName("Issue")]
        public bool? isNoIsuueinVerification { get; set; } //  Yes/No
        [DisplayName("Status")]
        public string? Status { get; set; }

    
        public Filedata? Filedata { get; set; }

       // public List<ManualVerificationProcessDetailsResponse>? ManualVerificationList { get; set; }
    }
}
