namespace VERIDATA.Model.Request
{
    public class GetUploadedFileDetailsByIdRequest
    {
        public int AppointeeId { get; set; }
        public string? FileCategory { get; set; }
        public int? FileId { get; set; }
    }
}
