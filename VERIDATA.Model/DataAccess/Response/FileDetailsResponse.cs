

namespace VERIDATA.Model.DataAccess.Response
{
    public class FileDetailsResponse
    {
        public byte[]? FileData { get; set; }
        public string? FileName { get; set; }
        public int UploadDetailsId { get; set; }
        public int UploadTypeId { get; set; }
        public string? mimeType { get; set; }
        public string? UploadTypeAlias { get; set; }
        public string? FileExtention { get; set; }

    }
}
