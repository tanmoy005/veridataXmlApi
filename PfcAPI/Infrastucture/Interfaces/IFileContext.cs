
using PfcAPI.Model.DataAccess;
using PfcAPI.Model.RequestModel;
using System.Data;
using static PfcAPI.Infrastucture.CommonEnum;

namespace PfcAPI.Infrastucture.Interfaces
{
    public interface IFileContext
    {
        /// <summary>
        /// save excel file details in to db
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filepath"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public Task<UploadedXSLfileDetailsResponse> GetDataTableFromxlsFile(int? CompanyId, IFormFile fileData, string? subfolder);

        public Task<int> PostUploadedXSLfileAsync(string? fileName, string? filepath, int? companyid);
        public Task<XSLfileDetailsListResponse> ValidateFromxlsFile(UploadedXSLfileDetailsResponse data);
        public Task<XSLfileDetailsListResponse> ValidateUpdatexlsFile(UploadedXSLfileDetailsResponse data);
        [Obsolete]
        public Task<string> SaveDataTabletofile(DataTable data, string category, ValidationType type);
        public Task<Filedata> GenerateDataTableTofile(DataTable data, string category, ValidationType type);

        //public List<AppointeeUploadDetails> postappointeeUploadedFiles(AppointeeDetailsRequest AppointeeDetails);
        public Task postappointeeUploadedFiles(AppointeeFileDetailsRequest AppointeeDetails);
        public Task postappointeeDataInFiles(AppointeeDataSaveInFilesRequset UploadDetails);
        public Task updateDocFile(int appointeeId, int userId);
        public Task<FilekDownloadDetails> DownloadPassbook(int appointeeId, int userId);
        public Task<FilekDownloadDetails> DownloadTrustPassbook(int appointeeId, int userId);
        //public Task<byte[]?> GetFileDataAsync(string filePath);

        //public Task PostFileAsync(IFormFile fileData, FileType fileType);

        //public Task PostMultiFileAsync(List<CompanyWiseFileUpload> fileData);

        //public Task DownloadFileById(int fileName);
    }
}
