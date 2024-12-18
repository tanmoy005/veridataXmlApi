using System.ComponentModel;

namespace VERIDATA.Model.DataAccess
{
    public class AppointeeBasicInfo
    {
        [DisplayName("Candidate ID")]
        public string? CandidateID { get; set; }

        [DisplayName("Name")]
        public string? AppointeeName { get; set; }

        [DisplayName("EmailId")]
        public string? AppointeeEmailId { get; set; }

        [DisplayName("Phone No")]
        public string? MobileNo { get; set; }

        //[DisplayName("Mobile No")]
        //public Decimal? EPFWages { get; set; }

        //[DisplayName("Mobile No")]
        //public DateTime? DateOfOffer { get; set; }

        [DisplayName("Date Of Joining")]
        public string? DateOfJoining { get; set; }

        [DisplayName("Fresher")]
        public string? IsFresher { get; set; }

        [DisplayName("Company Name")]
        public string? CompanyName { get; set; }

        [DisplayName("level1 Email")]
        public string? lvl1Email { get; set; }

        [DisplayName("level2 Email")]
        public string? lvl2Email { get; set; }

        [DisplayName("level3 Email")]
        public string? lvl3Email { get; set; }
    }
}