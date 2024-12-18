namespace VERIDATA.Model.Response
{
    public class FileCategoryResponse
    {
        public string? FileCategory { get; set; } // Represents upload_doc_type
        public List<FileTypeResponse> Files { get; set; } = new();
    }

    public class FileTypeResponse
    {
        public string? FileType { get; set; } // Represents upload_type_code
        public List<FileInfoResponse> FilesInfo { get; set; } = new();
    }

    public class FileInfoResponse
    {
        public int? UploadDetailId { get; set; }
        public string? FileName { get; set; }
    }
}