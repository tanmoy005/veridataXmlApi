using System.Data;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VERIDATA.BLL.Interfaces;
using VERIDATA.BLL.utility;
using VERIDATA.Model.Base;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using static VERIDATA.DAL.utility.CommonEnum;

namespace PfcAPI.Controllers.Appoientee
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppoienteeWorkFlowController : ControllerBase
    {
        private readonly IWorkFlowContext _workflowContext;
        private readonly ICandidateContext _candidateContext;
        private readonly ErrorResponse _ErrorResponse = new();
        private readonly IReportingContext _reportingContext;

        public AppoienteeWorkFlowController(IWorkFlowContext workflowContext, ICandidateContext candidateContext
            , IReportingContext reportingContext)
        {
            _candidateContext = candidateContext;
            _workflowContext = workflowContext;
            _reportingContext = reportingContext;
        }

        /// <summary>
        /// Single xls File Upload
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("RawDataProcess")]
        public ActionResult RawDataProcessAsync(RawDataProcessRequest rawdfileata)
        {
            if (rawdfileata.RawDataList.Count == 0)
            {
                return BadRequest();
            }
            try
            {
                List<UnderProcessDetailsResponse> response = new();
                response = Task.Run(async () => await _workflowContext.ProcessRawData(rawdfileata)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<UnderProcessDetailsResponse>(HttpStatusCode.OK, response));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("CompanyAppointeeDetailsUpdate")]
        public ActionResult PostCompanyAppointeeDetailsUpdate(CompanySaveAppointeeDetailsRequest AppointeeDetails)
        {
            if (AppointeeDetails == null)
            {
                return BadRequest();
            }
            try
            {
                Task.Run(async () => await _workflowContext.UpdateAppointeeDojByAdmin(AppointeeDetails)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("PostAppointeeDetailsSave")]
        public ActionResult PostAppointeeDetailsSave(AppointeeSaveDetailsRequest AppointeeDetails)
        {
            if (AppointeeDetails == null)
            {
                return BadRequest();
            }
            try
            {
                Task.Run(async () => await _workflowContext.PostAppointeeSaveDetailsAsync(AppointeeDetails)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("PostAppointeeDetailsSubmit")]
        public ActionResult PostAppointeeDetailsSubmit([FromForm] AppointeeFileDetailsRequest AppointeeDetails)
        {
            if (AppointeeDetails != null)
            {
                try
                {
                    bool? _isSubmit = AppointeeDetails?.IsSubmit;

                    Task.Run(async () => await _workflowContext.PostAppointeeFileDetailsAsync(AppointeeDetails)).GetAwaiter().GetResult();

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

        [Authorize]
        [HttpGet("GetAppointeeDetails")]
        public ActionResult GetAppointeeDetails(int appointeeId)
        {
            try
            {
                AppointeeDetailsResponse appointeedetail = Task.Run(async () => await _candidateContext.GetAppointeeDetailsAsync(appointeeId)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<AppointeeDetailsResponse>(HttpStatusCode.OK, appointeedetail));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [Authorize]
        [HttpPost("GetUnderProcessFileData")]
        public ActionResult GetUnderProcessFileData(AppointeeSeacrhFilterRequest reqObj)
        {
            try
            {
                reqObj.FilterType = string.IsNullOrEmpty(reqObj?.FilterType) ? string.Empty : reqObj?.FilterType;
                List<UnderProcessDetailsResponse> _getunderProcessData = Task.Run(async () => await _workflowContext.GetUnderProcessDataAsync(reqObj)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<UnderProcessDetailsResponse>(HttpStatusCode.OK, _getunderProcessData));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("GetNoResPosedFileData")]
        public ActionResult GetNoResPosedFileData(AppointeeSeacrhFilterRequest reqObj)
        {
            try
            {
                reqObj.FilterType = "NORES";
                List<UnderProcessDetailsResponse> _getunderProcessData = Task.Run(async () => await _workflowContext.GetUnderProcessDataAsync(reqObj)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<UnderProcessDetailsResponse>(HttpStatusCode.OK, _getunderProcessData));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("GetExpiredProcessFileData")]
        public ActionResult GetExpiredProcessFileData(AppointeeSeacrhFilterRequest reqObj)
        {
            try
            {
                List<UnderProcessDetailsResponse> _getunderProcessData = Task.Run(async () => await _workflowContext.GetExpiredProcessDataAsync(reqObj)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<UnderProcessDetailsResponse>(HttpStatusCode.OK, _getunderProcessData));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("GetUnProcessedFileData")]
        public ActionResult GetUnProcessedFileData(AppointeeSeacrhFilterRequest reqObj)
        {
            try
            {
                List<RawFileDataDetailsResponse> _getunProcessData = Task.Run(async () => await _workflowContext.GetNonProcessDataAsync(reqObj)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<RawFileDataDetailsResponse>(HttpStatusCode.OK, _getunProcessData));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("PostAppointeeApproved")]
        public ActionResult PostAppointeeApproved(AppointeeApproverRequest request)
        {
            try
            {
                string? appointeeCurrentState = Task.Run(async () => await _workflowContext.AppointeeWorkflowCurrentState(request.appointeeId)).GetAwaiter().GetResult();
                if (appointeeCurrentState is WorkFlowStatusType.Approved or WorkFlowStatusType.Rejected or
                   WorkFlowStatusType.ProcessClose or WorkFlowStatusType.ForcedApproved)
                {
                    _ErrorResponse.ErrorCode = (int)HttpStatusCode.BadRequest;
                    _ErrorResponse.UserMessage = "can not Appoved,this Appointee already processed";
                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, _ErrorResponse));
                }

                Task.Run(async () => await _workflowContext.PostAppointeeApprove(request)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = $"{RoleTypeAlias.SuperAdmin},{RoleTypeAlias.CompanyAdmin},{RoleTypeAlias.GeneralAdmin}")]
        [HttpPost("PostAppointeeRejected")]
        public ActionResult PostAppointeeRejected(AppointeeApproverRequest request)
        {
            try
            {
                string Remarks = string.Empty;
                string? appointeeCurrentState = Task.Run(async () => await _workflowContext.AppointeeWorkflowCurrentState(request.appointeeId)).GetAwaiter().GetResult();
                if (appointeeCurrentState is WorkFlowStatusType.Approved or WorkFlowStatusType.Rejected or
                   WorkFlowStatusType.ProcessClose or WorkFlowStatusType.ForcedApproved)
                {
                    _ErrorResponse.ErrorCode = (int)HttpStatusCode.BadRequest;
                    _ErrorResponse.UserMessage = "can not Reject,this Appointee already processed";
                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, _ErrorResponse));
                }
                Task.Run(async () => await _workflowContext.PostAppointeeRejected(request)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = $"{RoleTypeAlias.SuperAdmin},{RoleTypeAlias.CompanyAdmin}")]
        [HttpPost("PostAppointeeClose")]
        public ActionResult PostAppointeeClose(AppointeeApproverRequest request)
        {
            try
            {
                Task.Run(async () => await _workflowContext.PostAppointeeClose(request)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("GetVerifiedData")]
        public ActionResult GetVerifiedData(ProcessedFilterRequest filter)
        {
            try
            {
                filter.ToDate = filter?.ToDate != null ? filter?.ToDate.Value.AddDays(1) : filter?.ToDate;
                List<ProcessDataResponse> _getProcessData = Task.Run(async () => await _workflowContext.GetProcessDataAsync(filter)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<ProcessDataResponse>(HttpStatusCode.OK, _getProcessData));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("GetRejectedFileData")]
        public ActionResult GetRejectedFileData(FilterRequest filter)
        {
            try
            {
                filter.ToDate = filter?.ToDate != null ? filter?.ToDate.Value.AddDays(1) : filter?.ToDate;
                List<RejectedDataResponse> _getRejectedData = Task.Run(async () => await _workflowContext.GetRejectedDataAsync(filter)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<RejectedDataResponse>(HttpStatusCode.OK, _getRejectedData));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetRemarks")]
        public ActionResult GetRemarks(int AppointeeId)
        {
            try
            {
                List<GetRemarksResponse> _getRemarksData = Task.Run(async () => await _candidateContext.GetRemarks(AppointeeId)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<List<GetRemarksResponse>>(HttpStatusCode.OK, _getRemarksData));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("GetRemarksRemedy")]
        public ActionResult GetRemarksRemedy(GetRemarksRemedyRequest reqObj)
        {
            try
            {
                string? _getRemedyResponse = Task.Run(async () => await _candidateContext.GetRemarksRemedy(reqObj)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<string>(HttpStatusCode.OK, _getRemedyResponse));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("GetCriticalAppointeeList")]
        public ActionResult GetCriticalAppointeeList(CriticalFilterDataRequest reqObj)
        {
            try
            {
                List<CriticalAppointeeResponse> datalist = Task.Run(async () => await _workflowContext.GetCriticalAppointeeList(reqObj)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<CriticalAppointeeResponse>(HttpStatusCode.OK, datalist));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetAppointeeActivity")]
        public ActionResult GetAppointeeActivity(int appointeeId)
        {
            try
            {
                List<AppointeeActivityDetailsResponse> datalist = Task.Run(async () => await _candidateContext.GetActivityDetails(appointeeId)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<AppointeeActivityDetailsResponse>(HttpStatusCode.OK, datalist));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("PostRemainderMail")]
        public ActionResult PostRemainderMail(int AppointeeId, int UserId)
        {
            try
            {
                var validateMailRequest = Task.Run(async () => await _workflowContext.ValidateRemainderMail(AppointeeId, UserId)).GetAwaiter().GetResult();
                if (!validateMailRequest.IsVarified)
                {
                    _ErrorResponse.ErrorCode = (int)HttpStatusCode.BadRequest;
                    _ErrorResponse.UserMessage = validateMailRequest.Remarks;
                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, _ErrorResponse));
                }
                Task.Run(async () => await _workflowContext.PostRemainderMail(AppointeeId, UserId)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("AppointeeSearch")]
        public ActionResult GlobalAppointeeSearch(string appointeeName)
        {
            if (string.IsNullOrEmpty(appointeeName))
            {
                return BadRequest();
            }
            try
            {
                List<GetAppointeeGlobalSearchResponse> appointeeSearchList = Task.Run(async () => await _workflowContext.GetAppointeeSearchGlobal(appointeeName)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<GetAppointeeGlobalSearchResponse>(HttpStatusCode.OK, appointeeSearchList));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllReportFilterStatus")]
        public ActionResult GetAllReportFilterStatus()
        {
            try
            {
                List<DropDownDetailsResponse> _data = Task.Run(_workflowContext.GetAllReportFilterStatus).GetAwaiter().GetResult();
                return Ok(new BaseResponse<DropDownDetailsResponse>(HttpStatusCode.OK, _data));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("GetPassbookDetails")]
        public ActionResult GetPassbookDetails(int AppointeeId)
        {
            try
            {
                AppointeePassbookDetailsViewResponse passbookDetails = Task.Run(async () => await _candidateContext.GetPassbookDetailsByAppointeeId(AppointeeId)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<AppointeePassbookDetailsViewResponse>(HttpStatusCode.OK, passbookDetails));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("GetEmployementDetails")]
        public ActionResult GetEmployementDetails(int AppointeeId, int userId)
        {
            try
            {
                AppointeeEmployementDetailsViewResponse employementDetails = Task.Run(async () => await _candidateContext.GetEmployementDetailsByAppointeeId(AppointeeId, userId)).GetAwaiter().GetResult();
                if (!employementDetails.isEmployementDetailsAvailable ?? false)
                {
                    _ErrorResponse.ErrorCode = (int)HttpStatusCode.BadRequest;
                    _ErrorResponse.UserMessage = employementDetails.remarks;
                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, _ErrorResponse));
                }
                return Ok(new BaseResponse<AppointeeEmployementDetailsViewResponse>(HttpStatusCode.OK, employementDetails));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("UploadHandicapDetails")]
        public ActionResult UploadHandicapDetails(AppointeeHadicapFileDetailsRequest reqObj)
        {
            try
            {
                bool _isHandicap = reqObj?.IsHandicap?.ToString()?.ToUpper() == CheckType.yes;
                if (_isHandicap && reqObj?.FileDetails?.Count > 0)
                {
                    AppointeeFileDetailsRequest fileReqObj = new()
                    {
                        FileDetails = reqObj.FileDetails,
                        FileUploaded = reqObj.FileUploaded,
                        AppointeeCode = reqObj.AppointeeCode,
                        AppointeeId = reqObj.AppointeeId,
                        IsSubmit = false,
                        UserId = reqObj.UserId,
                    };
                    Task.Run(async () => await _candidateContext.PostAppointeefileUploadAsync(fileReqObj)).GetAwaiter().GetResult();
                }
                Task.Run(async () => await _candidateContext.PostAppointeeHandicapDetailsAsync(reqObj)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateDocWithUanDetails")]
        public ActionResult UpdateDocWithUanDetails(AppointeeUpdatePfUanDetailsRequest reqObj)
        {
            try
            {
                //if (reqObj?.FileDetails?.Count > 0)
                //{
                AppointeeFileDetailsRequest fileReqObj = new()
                {
                    FileDetails = reqObj?.FileDetails,
                    FileUploaded = reqObj?.FileUploaded,
                    AppointeeCode = reqObj?.AppointeeCode,
                    AppointeeId = reqObj.AppointeeId,
                    IsSubmit = false,
                    UserId = reqObj.UserId,
                };
                Task.Run(async () => await _candidateContext.PostAppointeefileUploadAsync(fileReqObj)).GetAwaiter().GetResult();
                //}

                Task.Run(async () => await _candidateContext.PostAppointeeTrusUanDetailsAsync(reqObj)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        // [AllowAnonymous]
        [HttpPost]
        [Route("PostCandidateMailResend")]
        public ActionResult PostCandidateMailResend(int AppointeeId, int UserId)
        {
            try
            {
                var validateMailRequest = Task.Run(async () => await _workflowContext.ValidateRemainderMail(AppointeeId, UserId)).GetAwaiter().GetResult();
                if (!validateMailRequest.IsVarified)
                {
                    _ErrorResponse.ErrorCode = (int)HttpStatusCode.BadRequest;
                    _ErrorResponse.UserMessage = validateMailRequest.Remarks;
                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, _ErrorResponse));
                }
                Task.Run(async () => await _workflowContext.PostMailResend(AppointeeId, UserId)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[AllowAnonymous]
        [Authorize]
        [HttpPost]
        [Route("PostAppointeePensionVerification")]
        public ActionResult PostAppointeePensionVerification(AppointeeApprovePensionRequest reqObj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedAppointeeDetails = Task.Run(async () => await _candidateContext.PostAppointeepensionAsync(reqObj)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateAppointeeManualVerification")]
        public ActionResult UpdateAppointeeManualVerification(AppointeeApproveVerificationRequest reqObj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var isvalid = Task.Run(async () => await _workflowContext.VerifyAppointeeManualAsync(reqObj)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<bool>(HttpStatusCode.OK, isvalid));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("GetAppointeeUploadedUnverifiedFiles")]
        public ActionResult GetAppointeeUploadedUnverifiedFiles(int AppointeeId)
        {
            try
            {
                List<FileCategoryResponse> _fileData = Task.Run(async () => await _workflowContext.GetNotVeriedfileView(AppointeeId)).GetAwaiter().GetResult();

                var filteredFileData = _fileData
                    .Where(fileCategoryResponse => fileCategoryResponse.FileCategory != null)
                    .ToList();
                return Ok(new BaseResponse<FileCategoryResponse>(HttpStatusCode.OK, filteredFileData));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("GetManualVeificationProcessData")]
        public ActionResult GetManualVeificationProcessData(ManualVeificationProcessDataRequest reqObj)
        {
            try

            {
                ManualVerificationDataResponse Response = new();
                DateTime _currDate = DateTime.Now;
                string _currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
                string reportName = $"Manual_Verification_Report_{_currDateString}.xlsx";

                List<ManualVerificationProcessDetailsResponse> _getunderProcessData = Task.Run(async () => await _workflowContext.GetManualVeificationProcessData(reqObj)).GetAwaiter().GetResult();
                Response.ManualVerificationList = _getunderProcessData;
                if (_getunderProcessData.Count > 0)
                {
                    List<ManualVerificationExcelDataResponse> excelData = Task.Run(async () => await _reportingContext.GetAppointeeManualVerificationExcelReport(_getunderProcessData)).GetAwaiter().GetResult();
                    if (excelData.Count > 0)
                    {
                        DataTable _exportdt1 = CommonUtility.ToDataTable<ManualVerificationExcelDataResponse>(excelData);
                        byte[] exportbytes = CommonUtility.ExportFromDataTableToExcel(_exportdt1, reportName, string.Empty);

                        Filedata _filedata = new() { FileData = exportbytes, FileName = reportName, FileType = "xlsx" };

                        Response.Filedata = _filedata;
                    }
                }

                return Ok(new BaseResponse<ManualVerificationDataResponse>(HttpStatusCode.OK, Response));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}