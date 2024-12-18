using System.ComponentModel.DataAnnotations;
using VERIDATA.Model.Request.api.Signzy.Base;

namespace VERIDATA.Model.Request.api.Signzy
{
    public class Signzy_GetPassportRequest : Signzy_BaseRequest
    {
        [Required]
        public string? fileNumber { get; set; }

        [Required]
        public string? dob { get; set; }
    }
}