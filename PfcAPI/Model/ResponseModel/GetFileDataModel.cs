namespace PfcAPI.Model.ResponseModel
{
    public class GetFileDataModel
    {
        public byte[]? FileData { get; set; }
        public string FileName { get; set; }
        public int UploadTypeId { get; set; }
        public string mimeType { get; set; }
        public string UploadTypeAlias { get; set; }
    }
}
