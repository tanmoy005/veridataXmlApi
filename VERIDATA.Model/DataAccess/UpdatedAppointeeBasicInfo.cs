using System.ComponentModel;

namespace VERIDATA.Model.DataAccess
{
    public class UpdatedAppointeeBasicInfo
    {
        [DisplayName("Candidate ID")]
        public string? CandidateID { get; set; }

        [DisplayName("Updated Name")]
        public string? AppointeeName { get; set; }

        //[DisplayName("EmailId")]
        //public string? AppointeeEmailId { get; set; }

        [DisplayName("Updated Phone No")]
        public string? MobileNo { get; set; }

        [DisplayName("Updated Date Of Joining")]
        public string? DateOfJoining { get; set; }

        //[DisplayName("Fresher")]
        //public string? IsFresher { get; set; }

        //[DisplayName("Company Name")]
        //public string? CompanyName { get; set; }

        //[DisplayName("level1 Email")]
        //public string? lvl1Email { get; set; }

        //[DisplayName("level2 Email")]
        //public string? lvl2Email { get; set; }

        //[DisplayName("level3 Email")]
        //public string? lvl3Email { get; set; }
    }
}