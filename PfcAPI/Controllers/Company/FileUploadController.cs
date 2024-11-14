using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VERIDATA.BLL.Context;
using VERIDATA.BLL.Interfaces;
using VERIDATA.Model.Base;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Public;
using static VERIDATA.DAL.utility.CommonEnum;

namespace PfcAPI.Controllers.Company
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileContext _fileService;
        private readonly IWorkFlowContext _workflowcontext;
        public FileUploadController(IFileContext fileService, IWorkFlowContext workflowcontext)
        {
            _fileService = fileService;
            _workflowcontext = workflowcontext;
        }

        [Authorize]
        [HttpPost]
        [Route("DownloadSampleXlsFile")]
        public ActionResult DownloadSampleXlsFile()
        {
            try
            {
                string filename = $"Data_Upload_Template.xlsx";
                string Cuurentpath = Directory.GetCurrentDirectory();
                string path = Path.Combine(Cuurentpath, "FileUploaded", "Sampledata.xlsx");

                byte[]? fileData = Task.Run(async () => await _fileService.GetFileDataAsync(path)).GetAwaiter().GetResult();
                Filedata _Filedata = new() { FileData = fileData, FileName = filename, FileType = "xlsx" };
                return Ok(new BaseResponse<Filedata>(HttpStatusCode.OK, _Filedata));
            }
            catch (Exception)
            {
                throw;

            }
        }
        [Authorize]
        [HttpPost]
        [Route("DownloadUpdateSampleXlsFile")]
        public ActionResult DownloadUpdateSampleXlsFile()
        {
            try
            {
                string reportname = $"Update_Data_Upload_Template_File.xlsx";
                string Cuurentpath = Directory.GetCurrentDirectory();
                string path = Path.Combine(Cuurentpath, "FileUploaded", "SampleUpdatedata.xlsx");

                byte[]? fileData = Task.Run(async () => await _fileService.GetFileDataAsync(path)).GetAwaiter().GetResult();
                Filedata _Filedata = new() { FileData = fileData, FileName = reportname, FileType = "xlsx" };
                return Ok(new BaseResponse<Filedata>(HttpStatusCode.OK, _Filedata));
            }
            catch (Exception)
            {
                throw;

            }
        }
        [Authorize]
        [HttpPost]
        [Route("DownloadPassbookFile")]
        public ActionResult DownloadPassbook(AppointeePassbookDownloadRequest reqobj)
        {
            try
            {
                FileDetailsResponse fileDetails = new();

                if (reqobj.Type == FileTypealias.PFPassbookTrust)
                {
                    fileDetails = Task.Run(async () => await _fileService.DownloadTrustPassbook(reqobj.AppointeeId, reqobj.UserId)).GetAwaiter().GetResult();
                }
                Filedata _Filedata = new() { FileData = fileDetails.FileData, FileName = fileDetails.FileData != null ? fileDetails.FileName : string.Empty, FileType = fileDetails.FileData != null ? fileDetails.FileExtention : string.Empty };
                return Ok(new BaseResponse<Filedata>(HttpStatusCode.OK, _Filedata));
            }
            catch (Exception)
            {
                throw;

            }
        }


        /// <summary>
        /// Single xls File Upload
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("UploadxlsFile")]
        public ActionResult UploadxlsFile([FromForm] CompanyFileUploadRequest fileDetails)
        {
            if (fileDetails == null)
            {
                return BadRequest();
            }
            try
            {
                UploadedxlsRawFileDataResponse response = new();

                response = Task.Run(async () => await _fileService.UploadAppointeexlsFile(fileDetails)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<UploadedxlsRawFileDataResponse>(HttpStatusCode.OK, response));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        //[AllowAnonymous]
        [HttpPost("UploadUpdatexlsFile")]
        public ActionResult UploadUpdatedxlsFile([FromForm] CompanyFileUploadRequest fileDetails)
        {
            if (fileDetails == null)
            {
                return BadRequest();
            }
            try
            {
                UploadedxlsRawFileDataResponse response = new();

                response = Task.Run(async () => await _fileService.UploadUpdateAppointeexlsFile(fileDetails)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<UploadedxlsRawFileDataResponse>(HttpStatusCode.OK, response));
            }
            catch (Exception)
            {
                throw;
            }
        }


        // GET: api/FileUpload/GetRawFileData
        [Authorize]
        [HttpGet("GetRawFileData")]
        public ActionResult GetRawFileData(int companyId, int? fileId)
        {
            try
            {
                List<RawFileDataDetailsResponse> RawFileData = new();
                RawFileData = Task.Run(async () => await _workflowcontext.getRawfileData(fileId, companyId)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<RawFileDataDetailsResponse>(HttpStatusCode.OK, RawFileData));
            }
            catch (Exception)
            {
                throw;
            }
        }
        [AllowAnonymous]
        //[Authorize]
        [HttpGet("getUploadFileData")]
        public ActionResult getUploadFileData(int appointeeId)
        {
            try
            {
                if (appointeeId <= 0)
                {
                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, new ErrorResponse
                    {
                        ErrorCode = 500,
                        InternalMessage = "Invalid appointeeId entered.",
                        UserMessage = "Appointee not found"
                    }));
                }

                List<FileCategoryResponse> fileCategories = Task.Run(async () => await _workflowcontext.getFileType(appointeeId)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<List<FileCategoryResponse>>(HttpStatusCode.OK, fileCategories));
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[AllowAnonymous]
        [Authorize]
        [HttpPost("GetUploadedFileDetailsById")]
        public ActionResult GetAppointeeFileDataByName(GetUploadedFileDetailsByIdRequest reqObj)
        {
            try
            {
                FileDetailsResponse _fileData = new();
                _fileData = Task.Run(async () => await _fileService.getFiledetailsByFileUploadId(reqObj.AppointeeId, reqObj.FileId)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<FileDetailsResponse>(HttpStatusCode.OK, _fileData));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [Authorize]
       // [HttpPost]
        [HttpPost("PostReuploadDocuments")]
        public IActionResult PostReuploadDocuments([FromForm]AppointeeReUploadFilesAfterSubmitRequest reqObj)
        {
            if (reqObj != null)
            {

                try
                {
                    bool? _isSubmit = reqObj?.IsSubmit;

                    //Task.Run(async () => await _fileService.postappointeeUploadedFiles(AppointeeDetails)).GetAwaiter().GetResult();
                    Task.Run(async () => await _workflowcontext.PostAppointeeFileDetailsAsync(reqObj)).GetAwaiter().GetResult();
                   
                    return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
                }
                catch (Exception)
                {
                    throw;

                }
            }
            else
            {
                return BadRequest();
            }

        }
    }
}
