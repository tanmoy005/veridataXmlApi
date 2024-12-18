using System.ComponentModel.DataAnnotations;

namespace VERIDATA.Model.Request
{
    public class CompanySaveAppointeeDetailsRequest
    {
        [Required]
        public int Id { get; set; }

        public int AppointeeId { get; set; }
        public string? CandidateId { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfJoining { get; set; }

        //public decimal? EPFWages { get; set; }
        //[EmailAddress]
        //public string? Email { get; set; }
        //public string? Type { get; set; }
        public int UserId { get; set; }
    }
}