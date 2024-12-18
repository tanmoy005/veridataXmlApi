using System.ComponentModel.DataAnnotations;
using VERIDATA.Model.Request.api.Karza.Base;

namespace VERIDATA.Model.Request.api.Karza
{
    public class Karza_GetPassportRequest : Karza_BaseRequest
    {
        //public string? name { get; set; }
        //public string? passportNo { get; set; }
        [Required]
        public string? fileNo { get; set; }

        [Required]
        public string? dob { get; set; }

        //public string? doi { get; set; }
    }
}