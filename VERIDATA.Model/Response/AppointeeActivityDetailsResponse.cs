namespace VERIDATA.Model.Response
{
    public class AppointeeActivityDetailsResponse
    {
        public int id { get; set; }
        public string? ActivityType { get; set; }
        public string? ActivityName { get; set; }
        public string? ActivityInfo { get; set; }
        public string? Color { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}