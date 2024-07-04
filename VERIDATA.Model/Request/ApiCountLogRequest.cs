namespace VERIDATA.Model.Request
{
    public class ApiCountLogRequest
    {
        public int? Id { get; set; }
        public string? Provider { get; set; }
        public string? Url { get; set; }
        public string? Type { get; set; }
        public string? Payload { get; set; }
        public int? Status { get; set; }
        public int UserId { get; set; }
    }
}
