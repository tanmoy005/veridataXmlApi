namespace VERIDATA.Model.DataAccess
{
    public class FileUploadDataModel
    {
        public string? fileName { get; set; }
        public int? uploadDetailsId { get; set; }
        public int uploadTypeId { get; set; }
        public string? mimeType { get; set; }
        public string? uploadTypeAlias { get; set; }
        public int fileLength { get; set; }
        public bool isFileUploaded { get; set; }
    }
}
