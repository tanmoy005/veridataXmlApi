using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.DataAccess
{
    public class DrivingLicenseDetails : BaseApiResponse
    {
        public string? Name { get; set; }
        public string? Dob { get; set; }
        public string? FatherOrHusbandName { get; set; }
        public string? LicenseStatus { get; set; }
    }
}