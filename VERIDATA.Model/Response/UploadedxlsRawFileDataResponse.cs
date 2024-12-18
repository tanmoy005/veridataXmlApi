using VERIDATA.Model.DataAccess;

namespace VERIDATA.Model.Response
{
    public class UploadedxlsRawFileDataResponse
    {
        public List<RawFileDataDetailsResponse>? RawFileData { get; set; }
        public List<Filedata>? DownloadFileData { get; set; }
        public int DuplicateCount { get; set; }
        public int NonExsitingCount { get; set; }
        public int InvalidUserCount { get; set; }
    }
}