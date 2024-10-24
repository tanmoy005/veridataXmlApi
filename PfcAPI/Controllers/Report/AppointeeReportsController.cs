using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using VERIDATA.BLL.Context;
using VERIDATA.BLL.Interfaces;
using VERIDATA.BLL.utility;
using VERIDATA.Model.Base;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;

namespace PfcAPI.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointeeReportsController : ControllerBase
    {
        private readonly IReportingContext _reportContext;
        private readonly IWorkFlowContext _workflowcontext;
        private readonly ErrorResponse _ErrorResponse = new();

        public AppointeeReportsController(IReportingContext reportContext, IWorkFlowContext workflowcontext)
        {
            _reportContext = reportContext;
            _workflowcontext = workflowcontext;

        }


        [Authorize]
        [HttpPost]
        [Route("ApprovedApponteeReport")]
        public ActionResult ApprovedApponteeReport(ProcessedFilterRequest filter)
        {
            try
            {
                filter.ToDate = filter?.ToDate != null ? filter?.ToDate.Value.AddDays(1) : filter?.ToDate;
                //string reportname = $"approved_appointee_{_currDateString}.xlsx";
                DateTime _currDate = DateTime.Now;
                string _currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
                string reportname = $"Approved_Appointee_{_currDateString}.xlsx";
                List<ProcessedDataReportDetailsResponse> appointeeList = Task.Run(async () => await _reportContext.GetApporvedAppointeeDetails(filter)).GetAwaiter().GetResult();
                if (appointeeList.Count > 0)
                {
                    DataTable _exportdt = CommonUtility.ToDataTable<ProcessedDataReportDetailsResponse>(appointeeList);
                    byte[] exportbytes = CommonUtility.ExportFromDataTableToExcel(_exportdt, reportname, filter?.FilePassword ?? string.Empty);
                    Filedata _Filedata = new() { FileData = exportbytes, FileName = reportname, FileType = "xlsx" };
                    return Ok(new BaseResponse<Filedata>(HttpStatusCode.OK, _Filedata));
                }
                else
                {
                    _ErrorResponse.ErrorCode = 400;
                    _ErrorResponse.UserMessage = "There is no data to export a report";
                    _ErrorResponse.InternalMessage = "There is no data to export a report";

                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, _ErrorResponse));
                }

            }
            catch (Exception)
            {
                throw;

            }
        }


        [Authorize]
        [HttpPost]
        [Route("RejectedApponteeReport")]
        public ActionResult RejectedApponteeReport(FilterRequest filter)
        {
            try
            {
                filter.ToDate = filter?.ToDate != null ? filter?.ToDate.Value.AddDays(1) : filter?.ToDate;
                DateTime _currDate = DateTime.Now;
                string _currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
                string reportname = $"Rejected_Appointee_{_currDateString}.xlsx";

                List<RejectedDataReportDetailsResponse> appointeeList = Task.Run(async () => await _reportContext.GetRejectedAppointeeDetails(filter)).GetAwaiter().GetResult();
                if (appointeeList.Count > 0)
                {
                    DataTable _exportdt = CommonUtility.ToDataTable<RejectedDataReportDetailsResponse>(appointeeList);
                    byte[] exportbytes = CommonUtility.ExportFromDataTableToExcel(_exportdt, reportname, filter?.FilePassword ?? string.Empty);
                    Filedata _Filedata = new() { FileData = exportbytes, FileName = reportname, FileType = "xlsx" };
                    return Ok(new BaseResponse<Filedata>(HttpStatusCode.OK, _Filedata));
                }
                else
                {
                    _ErrorResponse.ErrorCode = 400;
                    _ErrorResponse.UserMessage = "There is no data to export a report";
                    _ErrorResponse.InternalMessage = "There is no data to export a report";

                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, _ErrorResponse));
                }

            }
            catch (Exception)
            {
                throw;

            }
        }
        [Authorize]
        [HttpPost]
        [Route("GetPfCreationApponteeReport")]
        public ActionResult GetPfCreationApponteeReport(PfUserListRequest filter)
        {
            try
            {
                filter.ToDate = filter?.ToDate != null ? filter?.ToDate.Value.AddDays(1) : filter?.ToDate;
                List<PfUserListResponse> appointeeList = Task.Run(async () => await _reportContext.GetPfUserCreateAppointeeDetails(filter)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<PfUserListResponse>(HttpStatusCode.OK, appointeeList));
            }
            catch (Exception)
            {
                throw;

            }
        }

        [Authorize]
        [HttpPost]
        [Route("DownloadPfCreationApponteeReport")]
        public ActionResult ApprovedPfCreationApponteeReport(PfUserListRequest filter)
        {
            try
            {
                filter.ToDate = filter?.ToDate != null ? filter?.ToDate.Value.AddDays(1) : filter?.ToDate;
                DateTime _currDate = DateTime.Now;
                string _currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
                string reportname = $"PF_Creation_Appointee_{_currDateString}.xlsx";
                List<PfCreateAppointeeDetailsResponse> appointeeList = Task.Run(async () => await _reportContext.DownloadedPfUserCreateAppointeeDetails(filter)).GetAwaiter().GetResult();
                if (appointeeList.Count > 0)
                {
                    DataTable _exportdt = CommonUtility.ToDataTable<PfCreateAppointeeDetailsResponse>(appointeeList);
                    byte[] exportbytes = CommonUtility.ExportFromDataTableToExcel(_exportdt, reportname, filter?.FilePassword ?? string.Empty);
                    Filedata _Filedata = new() { FileData = exportbytes, FileName = reportname, FileType = "xlsx" };
                    return Ok(new BaseResponse<Filedata>(HttpStatusCode.OK, _Filedata));
                }
                else
                {
                    _ErrorResponse.ErrorCode = 400;
                    _ErrorResponse.UserMessage = "There is no data to export a report";
                    _ErrorResponse.InternalMessage = "There is no data to export a report";

                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, _ErrorResponse));
                }
            }
            catch (Exception)
            {
                throw;

            }
        }

        [AllowAnonymous]
        [Authorize]
        [HttpPost]
        [Route("GetUnderProcessReport")]
        public ActionResult GetUnderProcessReport(AppointeeSeacrhFilterRequest reqObj)
        {
            try
            {
                DateTime _currDate = DateTime.Now;
                string _currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
                string reportname = $"UnderProcess_Appointee_{_currDateString}.xlsx";
                reqObj.FilterType = string.IsNullOrEmpty(reqObj?.FilterType) ? string.Empty : reqObj?.FilterType;
                List<UnderProcessDetailsResponse> UnderProcessAppointeeList = Task.Run(async () => await _workflowcontext.GetUnderProcessDataAsync(reqObj)).GetAwaiter().GetResult();
                if (UnderProcessAppointeeList.Count > 0)
                {
                    List<UnderProcessedDataReportDetails> appointeeList = _reportContext.GetUnderProcessDetails(UnderProcessAppointeeList);
                    DataTable _exportdt = CommonUtility.ToDataTable<UnderProcessedDataReportDetails>(appointeeList);
                    byte[] exportbytes = CommonUtility.ExportFromDataTableToExcel(_exportdt, reportname, string.Empty);
                    Filedata _Filedata = new() { FileData = exportbytes, FileName = reportname, FileType = "xlsx" };
                    return Ok(new BaseResponse<Filedata>(HttpStatusCode.OK, _Filedata));
                }
                else
                {
                    _ErrorResponse.ErrorCode = 400;
                    _ErrorResponse.UserMessage = "There is no data to export a report";
                    _ErrorResponse.InternalMessage = "There is no data to export a report";

                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, _ErrorResponse));
                }

            }
            catch (Exception)
            {
                throw;

            }
        }

        [AllowAnonymous]
        [Authorize]
        [HttpPost]
        [Route("GetLapsedDataReport")]
        public ActionResult GetLapsedDataReport(AppointeeSeacrhFilterRequest reqObj)
        {
            try
            {
                DateTime _currDate = DateTime.Now;
                string _currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
                string reportname = $"Lapsed_Appointee_{_currDateString}.xlsx";
                reqObj.FilterType = string.IsNullOrEmpty(reqObj?.FilterType) ? string.Empty : reqObj?.FilterType;
                List<UnderProcessDetailsResponse> LapsedAppointeeList = Task.Run(async () => await _workflowcontext.GetExpiredProcessDataAsync(reqObj)).GetAwaiter().GetResult();
                if (LapsedAppointeeList.Count > 0)
                {
                    List<UnderProcessedDataReportDetails> appointeeList = _reportContext.GetUnderProcessDetails(LapsedAppointeeList);
                    DataTable _exportdt = CommonUtility.ToDataTable<UnderProcessedDataReportDetails>(appointeeList);
                    byte[] exportbytes = CommonUtility.ExportFromDataTableToExcel(_exportdt, reportname, string.Empty);
                    Filedata _Filedata = new() { FileData = exportbytes, FileName = reportname, FileType = "xlsx" };
                    return Ok(new BaseResponse<Filedata>(HttpStatusCode.OK, _Filedata));
                }
                else
                {
                    _ErrorResponse.ErrorCode = 400;
                    _ErrorResponse.UserMessage = "There is no data to export a report";
                    _ErrorResponse.InternalMessage = "There is no data to export a report";

                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, _ErrorResponse));
                }

            }
            catch (Exception)
            {
                throw;

            }
        }

        [Authorize]
        [HttpPost]
        [Route("ApiCounterReport")]
        public ActionResult ApiCounterReport(DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                ApiCountReportResponse res = new();
                //List<ApiCountJobResponse> apiList = new();
                Filedata _Filedata = new();
                ToDate = ToDate != null ? ToDate.Value.AddDays(1) : ToDate;
                DateTime _currDate = DateTime.Now;
                string _currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
                string reportname = $"Api_Count_Report_{_currDateString}.xlsx";
                res = Task.Run(async () => await _reportContext.ApiCountReport(FromDate, ToDate)).GetAwaiter().GetResult();
                if (res?.ApiCountList?.Count > 0)
                {
                    DataTable _exportdt = CommonUtility.ToDataTable<ApiCountJobResponse>(res.ApiCountList);
                    byte[] exportbytes = CommonUtility.ExportFromDataTableToExcel(_exportdt, reportname, string.Empty);
                    _Filedata = new Filedata() { FileData = exportbytes, FileName = reportname, FileType = "xlsx" };
                    res.Filedata = _Filedata;
                }
                return Ok(new BaseResponse<ApiCountReportResponse>(HttpStatusCode.OK, res));
            }
            catch (Exception)
            {
                throw;

            }
        }

        [Authorize]
        [AllowAnonymous]
        //[Authorize(Roles = $"{RoleTypeAlias.SuperAdmin},{RoleTypeAlias.CompanyAdmin},{RoleTypeAlias.GeneralAdmin}")]
        [HttpPost]
        [Route("AppointeeCounterReport")]
        // public ActionResult AppointeeCounterReport(DateTime? FromDate, DateTime? ToDate)
        public ActionResult AppointeeCounterReport(AppointeeCountReportSearchRequest reqObj)
        {
            try
            {
                AppointeeCountJobResponse Response = new();
                reqObj.ToDate = reqObj.ToDate != null ? reqObj.ToDate.Value.AddDays(1) : reqObj.ToDate;
                List<DataTable> _exportdt = new();
                //string reportname = $"approved_appointee_{_currDateString}.xlsx";
                DateTime _currDate = DateTime.Now;
                string _currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
                string reportname = $"Appointee_Count_Report_{_currDateString}.xlsx";
                AppointeeCountDateWiseDetails apiList = Task.Run(async () => await _reportContext.AppointeeCountReport(reqObj)).GetAwaiter().GetResult();
                if (apiList?.AppointeeCountDateWise?.Count > 0)
                {
                    DataTable _exportdt1 = CommonUtility.ToDataTable<AppointeeTotalCount>(apiList.AppointeeTotalCount);
                    DataTable _exportdt2 = CommonUtility.ToDataTable<AppointeeCountDetails>(apiList.AppointeeCountDetails);
                    _exportdt.Add(_exportdt1);
                    _exportdt.Add(_exportdt2);
                    byte[] exportbytes = CommonUtility.ExportFromDataTableListToExcel(_exportdt);
                    //var _file = file(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    //var _filedata = new Filedata() { FileData = pdf, FileName = reportname, FileType = "pdf" };
                    Filedata _filedata = new() { FileData = exportbytes, FileName = reportname, FileType = "xlsx" };
                    Response.AppointeeCountDateWises = apiList.AppointeeCountDateWise;
                    Response.AppointeeCountListDetails = apiList.AppointeeCountDetails;
                    Response.Filedata = _filedata;
                }
                return Ok(new BaseResponse<AppointeeCountJobResponse>(HttpStatusCode.OK, Response));
            }
            catch (Exception)
            {
                throw;

            }
        }

        [Authorize]
        //[AllowAnonymous]
        //[Authorize(Roles = $"{RoleTypeAlias.SuperAdmin},{RoleTypeAlias.CompanyAdmin},{RoleTypeAlias.GeneralAdmin}")]
        [HttpPost]
        [Route("AdminAppointeeCounterReport")]
        public ActionResult AdminAppointeeCounterReport(AppointeeCountReportSearchRequest reqObj)
        {
            try
            {
                AppointeeCountJobResponse Response = new();
                reqObj.ToDate = reqObj.ToDate != null ? reqObj.ToDate.Value.AddDays(1) : reqObj.ToDate;
                List<DataTable> _exportdt = new();
                //string reportname = $"approved_appointee_{_currDateString}.xlsx";
                DateTime _currDate = DateTime.Now;
                string _currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
                string reportname = $"Appointee_Count_Report_{_currDateString}.xlsx";
                AppointeeCountDateWiseDetails? apiList = Task.Run(async () => await _reportContext.AppointeeCountReport(reqObj)).GetAwaiter().GetResult();
                if (apiList?.AppointeeCountDateWise?.Count > 0)
                {
                    DataTable _exportdt1 = CommonUtility.ToDataTable<AppointeeTotalCount>(apiList.AppointeeTotalCount);
                    DataTable _exportdt2 = CommonUtility.ToDataTable<AppointeeCountDetails>(apiList.AppointeeCountDetails);
                    _exportdt.Add(_exportdt1);
                    _exportdt.Add(_exportdt2);
                    byte[] exportbytes = CommonUtility.ExportFromDataTableListToExcel(_exportdt);
                    //var _file = file(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    Filedata _filedata = new() { FileData = exportbytes, FileName = "sheet", FileType = "xlsx" };
                    Response.AppointeeCountDateWises = apiList.AppointeeCountDateWise;
                    Response.Filedata = _filedata;
                }
                return Ok(new BaseResponse<AppointeeCountJobResponse>(HttpStatusCode.OK, Response));
                //   return Ok(new BaseResponse<AppointeeCountDateWiseDetails>(HttpStatusCode.OK, apiList));

            }
            catch (Exception)
            {
                throw;

            }

        }
        [Authorize]
        [HttpPost]
        [Route("AppointeeAgingFilterReport")]
        public ActionResult AppointeeAgingFilterReport(GetAgingReportRequest reqObj)
        {
            try
            {
                GetAgingReportResponse Response = new();
                List<DataTable> _exportdt = new();
                //string reportname = $"approved_appointee_{_currDateString}.xlsx";
                DateTime _currDate = DateTime.Now;
                string _currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
                string reportname = $"Appointee_Aging_Report_{_currDateString}.xlsx";
                List<AppointeeAgingDataReportDetails>? appointeeList = Task.Run(async () => await _reportContext.AppointeeDetailsAgingReport(reqObj)).GetAwaiter().GetResult();
                if (appointeeList?.Count > 0)
                {
                    DataTable _exportdt1 = CommonUtility.ToDataTable<AppointeeAgingDataReportDetails>(appointeeList);
                    byte[] exportbytes = CommonUtility.ExportFromDataTableToExcel(_exportdt1, reportname, reqObj.FilePassword ?? string.Empty);
                    //var _file = file(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
                    Filedata _filedata = new() { FileData = exportbytes, FileName = reportname, FileType = "xlsx" };
                    Response.AppointeeDetails = appointeeList;
                    Response.Filedata = _filedata;
                    return Ok(new BaseResponse<GetAgingReportResponse>(HttpStatusCode.OK, Response));
                }

                return Ok(new BaseResponse<AppointeeAgingDataReportDetails>(HttpStatusCode.OK, appointeeList));
            }
            catch (Exception)
            {
                throw;

            }
        }
        [Authorize]
        [AllowAnonymous]
        [HttpPost]
        [Route("NationalityFilterReport")]
        public ActionResult NationalityFilterReport(GetNationalityReportRequest reqObj)
        {
            try
            {
                GetNationalityReportResponse Response = new();
                List<DataTable> _exportdt = new();
                DateTime _currDate = DateTime.Now;
                string _currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
                string reportname = $"Appointee_Nationality_Report_{_currDateString}.xlsx";

                // Fetching the appointee nationality data
                List<AppointeeNationalityDataReportDetails>? appointeeList = Task.Run(async () => await _reportContext.AppointeeNationalityDetailsReport(reqObj)).GetAwaiter().GetResult();

                if (appointeeList?.Count > 0)
                {
                    DataTable _exportdt1 = CommonUtility.ToDataTable<AppointeeNationalityDataReportDetails>(appointeeList);
                    byte[] exportbytes = CommonUtility.ExportFromDataTableToExcel(_exportdt1, reportname, string.Empty);

                    Filedata _filedata = new() { FileData = exportbytes, FileName = reportname, FileType = "xlsx" };
                    Response.AppointeeDetails = appointeeList;
                    Response.Filedata = _filedata;
                }
                return Ok(new BaseResponse<GetNationalityReportResponse>(HttpStatusCode.OK, Response));
            }
            catch (Exception)
            {
                throw;
            }

        }
        [Authorize]
        //[AllowAnonymous]
        [HttpPost]
        [Route("AppointeeDataFilterReport")]
        public ActionResult AppointeeDataFilterReport(AppointeeDataFilterReportRequest reqObj)
        {
            try
            {
                AppointeeDataFilterReportResponse Response = new();
                List<DataTable> _exportdt = new();
                //string reportname = $"approved_appointee_{_currDateString}.xlsx";
                DateTime _currDate = DateTime.Now;
                string _currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
                string reportname = $"Appointee_Details_Report_{_currDateString}.xlsx";
                List<AppointeeDataFilterReportDetails>? appointeeList = Task.Run(async () => await _reportContext.AppointeeDetailsReport(reqObj)).GetAwaiter().GetResult();

                if (appointeeList?.Count > 0)
                {
                    DataTable _exportdt1 = CommonUtility.ToDataTable<AppointeeDataFilterReportDetails>(appointeeList);
                    byte[] exportbytes = CommonUtility.ExportFromDataTableToExcel(_exportdt1, reportname, string.Empty);

                    Filedata _filedata = new() { FileData = exportbytes, FileName = reportname, FileType = "xlsx" };
                    Response.AppointeeDetails = appointeeList;
                    Response.Filedata = _filedata;
                }
                return Ok(new BaseResponse<AppointeeDataFilterReportResponse>(HttpStatusCode.OK, Response));
            }
            catch (Exception)
            {
                throw;

            }
        }
        [Authorize]
        //[AllowAnonymous]
        [HttpPost]
        [Route("AppointeeDataPfFilterReport")]
        public ActionResult AppointeeDataPfFilterReport(AppointeeDataFilterReportRequest reqObj)
        {
            try
            {
                AppointeeDataFilterReportResponse Response = new();
                List<DataTable> _exportdt = new();
                //string reportname = $"approved_appointee_{_currDateString}.xlsx";
                DateTime _currDate = DateTime.Now;
                string _currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
                string reportname = $"Appointee_PF_Details_Report_{_currDateString}.xlsx";
                List<AppointeeDataPfReportResponse>? appointeeList = Task.Run(async () => await _reportContext.AppointeePfDetailsReport(reqObj)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<AppointeeDataPfReportResponse>(HttpStatusCode.OK, appointeeList));
            }
            catch (Exception)
            {
                throw;

            }
        }
    }
}
