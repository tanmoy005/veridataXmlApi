namespace VERIDATA.Model.DataAccess.Response
{
    public class AppointeeLastActivityDetailsResponse
    {
        public int Id { get; set; }
        public int AppointeeId { get; set; }
        public string? ActivityType { get; set; }
        public string? ActivityName { get; set; }
        public string? ActivityInfo { get; set; }
        public string? ActivityDesc { get; set; }
        public string? Color { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
