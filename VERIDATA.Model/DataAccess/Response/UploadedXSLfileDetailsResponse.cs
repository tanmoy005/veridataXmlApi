using System.Data;

namespace VERIDATA.Model.DataAccess.Response
{
    public class UploadedXSLfileDetailsResponse
    {
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public DataTable? dataTable { get; set; }
    }
}