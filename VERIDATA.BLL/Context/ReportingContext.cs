using System.Net;
using VERIDATA.BLL.Interfaces;
using VERIDATA.BLL.utility;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.DAL.utility;
using VERIDATA.Model.Configuration;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Config;
using VERIDATA.Model.Table.Public;
using static VERIDATA.DAL.utility.CommonEnum;

namespace VERIDATA.BLL.Context
{
    public class ReportingContext : IReportingContext
    {
        private readonly IWorkFlowDalContext _workFlowDetailsContext;
        private readonly IReportingDalContext _reportingDalContext;
        private readonly ApiConfiguration _aadhaarConfig;
        private readonly string key;
        public ReportingContext(IWorkFlowDalContext workFlowDetailsContext, ApiConfiguration aadhaarConfig, IReportingDalContext reportingDalContext)
        {
            _workFlowDetailsContext = workFlowDetailsContext;
            _reportingDalContext = reportingDalContext;
            _aadhaarConfig = aadhaarConfig;
            key = aadhaarConfig?.EncriptKey ?? string.Empty;
        }
        public async Task<List<ProcessedDataReportDetailsResponse>> GetApporvedAppointeeDetails(ProcessedFilterRequest filter)
        {
            List<ProcessedDataReportDetailsResponse> res = new();
            List<ProcessedDataDetailsResponse> proceesedAppointeeList = await _workFlowDetailsContext.GetProcessedAppointeeDetailsAsync(filter);
            if (proceesedAppointeeList.Count > 0)
            {
                res = await _reportingDalContext.GetProcessedAppointeeReportDetailsAsync(proceesedAppointeeList);
            }
            return res;
        }

        public async Task<List<RejectedDataReportDetailsResponse>> GetRejectedAppointeeDetails(FilterRequest filter)
        {
            List<RejectedDataReportDetailsResponse> RejectedData = new();
            List<RejectedDataDetailsResponse> RejectedAppointeeList = await _workFlowDetailsContext.GetRejectedAppointeeDetailsAsync(filter);
            if (RejectedAppointeeList.Count > 0)
            {
                RejectedData = GetRejectedViewData(RejectedAppointeeList);
            }
            return RejectedData;
        }

        private List<RejectedDataReportDetailsResponse> GetRejectedViewData(List<RejectedDataDetailsResponse> rejectedAppointeeList)
        {
            List<RejectedDataReportDetailsResponse> response = new();
            List<IGrouping<int?, RejectedDataDetailsResponse>> GroupData = rejectedAppointeeList.GroupBy(x => x.AppointeeId).ToList();
            foreach (var item in GroupData.Select((value, index) => new { Value = value, Index = index }))
            {
                IGrouping<int?, RejectedDataDetailsResponse> x = item.Value;
                List<string?> ResonDetails = x.Select(y => y.Remarks).ToList();
                string Remarks = ResonDetails.Count > 0 ? string.Join(", ", ResonDetails) : "NA";
                RejectedDataReportDetailsResponse? _data = x.Select(r =>

                    new RejectedDataReportDetailsResponse
                    {
                        CandidateId = r.CandidateId,
                        AppointeeName = r.AppointeeName,
                        DateOfBirth = string.IsNullOrEmpty(r.DateOfBirth?.ToShortDateString()) ? "NA" : r.DateOfBirth?.ToShortDateString(),
                        MobileNo = r.MobileNo,
                        EmailId = r.AppointeeEmailId,
                        DateOfJoining = r.DateOfJoining?.ToShortDateString(),
                        Nationality = string.IsNullOrEmpty(r.Nationality) ? "NA" : r.Nationality,
                        UANNumber = string.IsNullOrEmpty(r.UANNumber) ? "NA" : CommonUtility.DecryptString(key, r.UANNumber),
                        PANNumber = string.IsNullOrEmpty(r.PANNumber) ? "NA" : CommonUtility.DecryptString(key, r.PANNumber),
                        AadhaarNumber = string.IsNullOrEmpty(r.AadhaarNumberView) ? "NA" : r.AadhaarNumberView,
                        //AadhaarNumber = string.IsNullOrEmpty(r.AadhaarNumber) ? "NA" : CommonUtility.DecryptString(key, r.AadhaarNumber),
                        Remarks = Remarks

                    }).FirstOrDefault();
                response.Add(_data);
            }

            return response;
        }

        public async Task<List<PfUserListResponse>> GetPfUserCreateAppointeeDetails(PfUserListRequest filter)
        {
            List<PfUserListResponse>? response = new();
            List<PfCreationProcessedReportResponse> proceesedAppointeeList = await _reportingDalContext.GetPfCreationProcessedReportDetailsAsync(filter);
            if (proceesedAppointeeList.Count > 0)
            {
                response = proceesedAppointeeList?.DistinctBy(x => x.AppointeeData.AppointeeId)?.OrderBy(x => x.AppointeeData.DateOfJoining)?.Select(row => new PfUserListResponse
                {
                    id = row.AppointeeData.AppointeeDetailsId,
                    companyId = row?.AppointeeData?.CompanyId ?? 0,
                    candidateId = row?.AppointeeData?.CandidateId,
                    appointeeName = row?.AppointeeData?.AppointeeName,
                    appointeeId = row?.AppointeeData?.AppointeeId,
                    appointeeEmailId = row?.AppointeeData?.AppointeeEmailId,
                    mobileNo = row?.AppointeeData?.MobileNo,
                    adhaarNo = row?.AppointeeData?.AadhaarNumberView,
                    panNo = string.IsNullOrEmpty(row?.AppointeeData?.PANNumber) ? "NA" : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row?.AppointeeData?.PANNumber)),
                    dateOfJoining = row?.AppointeeData?.DateOfJoining,
                    epfWages = row?.AppointeeData?.EPFWages,
                    uanNo = string.IsNullOrEmpty(row?.AppointeeData?.UANNumber) ? "NA" : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row?.AppointeeData?.UANNumber)),
                    status = row?.ProcessData?.DataUploaded ?? false ? "Downloaded" : "NA",
                    isPensionApplicable = row?.AppointeeData?.IsPensionApplicable == null ? "NA" : row?.AppointeeData?.IsPensionApplicable ?? false ? "Yes" : "No",
                })?.ToList();
            }
            return response;
        }

        public async Task<List<PfCreateAppointeeDetailsResponse>> DownloadedPfUserCreateAppointeeDetails(PfUserListRequest filter)
        {
            string? key = _aadhaarConfig.EncriptKey;
            List<PfCreateAppointeeDetailsResponse>? processeddata = new();
            List<PfCreationProcessedReportResponse>? proceesedAppointeeList = await _reportingDalContext.GetPfCreationProcessedReportDetailsAsync(filter);


            List<ProcessedFileData>? processedList = proceesedAppointeeList?.Select(x => x.ProcessData).ToList();

            await _reportingDalContext.UpdateDownloadedProcessData(processedList);

            if (proceesedAppointeeList.Count > 0)
            {

                processeddata = proceesedAppointeeList?.DistinctBy(x => x.AppointeeData.AppointeeId)?
                   .OrderBy(x => x.AppointeeData.DateOfJoining)?.Select(r => new PfCreateAppointeeDetailsResponse
                   {
                       AppointeeName = r?.AppointeeData?.AppointeeName,
                       DateOfBirth = r?.AppointeeData?.DateOfBirth?.ToShortDateString(),
                       MobileNo = r?.AppointeeData?.MobileNo,
                       EmailId = r?.AppointeeData?.AppointeeEmailId,
                       DateOfJoining = r?.AppointeeData?.DateOfJoining?.ToShortDateString(),
                       Nationality = r?.AppointeeData?.Nationality,
                       Qualification = r?.AppointeeData?.Qualification,
                       UANNumber = string.IsNullOrEmpty(r?.AppointeeData?.UANNumber) ? null : CommonUtility.DecryptString(key, r?.AppointeeData?.UANNumber),
                       EPFWages = r?.AppointeeData?.EPFWages ?? 00,
                       Gender = r?.AppointeeData?.Gender,
                       MaratialStatus = r?.AppointeeData?.MaratialStatus,
                       MemberName = r?.AppointeeData?.MemberName,
                       MemberRelation = r?.AppointeeData?.MemberRelation,
                       IsHandicap = r?.AppointeeData?.IsHandicap,
                       HandicapeType = r?.AppointeeData?.HandicapeType,
                       IsInternationalWorker = r?.AppointeeData?.IsInternationalWorker,
                       PassportNo = string.IsNullOrEmpty(r?.AppointeeData?.PassportNo) ? null : CommonUtility.DecryptString(key, r?.AppointeeData?.PassportNo),
                       PassportValidFrom = r?.AppointeeData?.PassportValidFrom?.ToShortDateString(),
                       PassportValidTill = r?.AppointeeData?.PassportValidTill?.ToShortDateString(),
                       OriginCountry = r?.AppointeeData?.OriginCountry,
                       PANName = r?.AppointeeData?.PANName,
                       PANNumber = string.IsNullOrEmpty(r?.AppointeeData?.PANNumber) ? "NA" : CommonUtility.DecryptString(key, r?.AppointeeData?.PANNumber),
                       AadhaarName = r?.AppointeeData?.AadhaarName,
                       AadhaarNumber = string.IsNullOrEmpty(r?.AppointeeData?.AadhaarNumberView) ? "NA" : r?.AppointeeData?.AadhaarNumberView,
                       //AadhaarNumber = string.IsNullOrEmpty(r?.AppointeeData?.AadhaarNumber) ? "NA" : CommonUtility.DecryptString(key, r?.AppointeeData?.AadhaarNumber),
                   }).ToList();
            }

            return processeddata;
        }
        public List<UnderProcessedDataReportDetails> GetUnderProcessDetails(List<UnderProcessDetailsResponse> reqList)
        {
            List<UnderProcessedDataReportDetails>? response = new();
            if (reqList.Count > 0)
            {
                response = reqList?.Select(x => new UnderProcessedDataReportDetails
                {
                    AppointeeName = x?.appointeeName,
                    candidateId = x?.candidateId,
                    EmailId = x?.appointeeEmailId,
                    mobileNo = x?.mobileNo,
                    dateOfJoining = x?.dateOfJoining?.ToShortDateString(),
                    CreatedDate = x?.createdDate?.ToShortDateString() ?? string.Empty,
                    Status = x?.status,
                })?.ToList();
            }
            return response;
        }


        public async Task<ApiCountReportResponse> ApiCountReport(DateTime? FromDate, DateTime? ToDate)
        {
            ApiCountReportResponse ReportRes = new();
            List<ApiCounter>? totalApiList = await _reportingDalContext.GetTotalApiList(FromDate, ToDate);

            var DetailedReportRes = GetDatewiseApiCountDetails(totalApiList);
            var ConsolidateReportRes = GetApiProvider_NameWiseApiCountDetails(totalApiList);
            ReportRes.ApiCountList = DetailedReportRes;
            ReportRes.ApiConsolidateCountList = ConsolidateReportRes;
            return ReportRes;
        }

        private List<ApiCountJobResponse> GetDatewiseApiCountDetails(List<ApiCounter>? totalApiList)
        {
            List<ApiCountJobResponse> DetailedReportRes = new();
            List<IGrouping<string?, ApiCounter>>? DateWiseTotalRequestApiList = totalApiList?.Where(x => x?.Type == "Request")?.GroupBy(x => x.CreatedOn?.ToShortDateString())?.ToList();
            List<ApiCounter>? TotalResponseApiList = totalApiList?.Where(x => x?.Type == "Response")?.ToList();

            foreach (IGrouping<string?, ApiCounter>? TotalApiPerDate in DateWiseTotalRequestApiList)
            {
                string? _currDate = TotalApiPerDate?.Key?.ToString();
                List<ApiCounter>? _currTotalData = TotalApiPerDate?.ToList();
                List<ApiCountJobResponse>? TotalApiCount = new();
                List<ApiCountJobResponse>? TotalSuccessApiCount = new();
                List<ApiCountJobResponse>? TotalUnproceesbleApiCount = new();
                List<ApiCountJobResponse>? TotalFaliureApiCount = new();

                List<ApiCounter>? DateWiseTotalResponseApiList = TotalResponseApiList?.Where(x => x?.CreatedOn?.ToShortDateString() == _currDate)?.ToList();

                var TotalApiCountByName = _currTotalData?.Where(x => x?.Type == "Request")?.GroupBy(x => x.ApiName)?.ToList();

                foreach (var obj in TotalApiCountByName)
                {
                    var TotalApiCountProviderWise = obj?.GroupBy(x => x.ProviderName)?.Select(y => new ApiCountJobResponse
                    {
                        ProviderName = y.Key,
                        ApiName = obj.Key?.ToLower(),
                        TotalApiCount = y?.ToList()?.Count() ?? 0,
                    })?.ToList();



                    TotalUnproceesbleApiCount = DateWiseTotalResponseApiList?.Where(x => x?.Status == (int)HttpStatusCode.UnprocessableEntity && x.ApiName?.ToLower() == obj.Key?.ToLower())?.GroupBy(x => x.ProviderName)?.Select(y => new ApiCountJobResponse
                    {
                        ProviderName = y.Key,
                        ApiName = obj.Key?.ToLower(),
                        TotalUnprocessableEntityCount = y?.ToList()?.Count() ?? 0,
                        //TotalApiCount = TotalApiCount?.Where(x => x.ApiName?.ToLower() == y?.Key?.ToLower())?.ToList()?.Count() ?? 0
                    })?.ToList();

                    TotalFaliureApiCount = DateWiseTotalResponseApiList?.Where(x => x?.Status is not (((int)HttpStatusCode.UnprocessableEntity) or ((int)HttpStatusCode.OK)) && x.ApiName?.ToLower() == obj.Key?.ToLower())?.GroupBy(x => x.ProviderName)?.Select(y => new ApiCountJobResponse
                    {
                        ProviderName = y?.Key,
                        ApiName = obj.Key?.ToLower(),
                        TotalFailureCount = y?.ToList()?.Count() ?? 0,
                    })?.ToList();

                    TotalSuccessApiCount = DateWiseTotalResponseApiList?.Where(x => x?.Status == (int)HttpStatusCode.OK && x.ApiName?.ToLower() == obj.Key?.ToLower())?.GroupBy(x => x.ProviderName)?.Select(y => new ApiCountJobResponse
                    {
                        ProviderName = y.Key,
                        ApiName = obj.Key?.ToLower(),
                        TotalSuccessApiCount = y?.ToList()?.Count() ?? 0,
                    })?.ToList();


                    foreach (ApiCountJobResponse? obj1 in TotalApiCountProviderWise)
                    {
                        var _unprocessebleEntity = TotalUnproceesbleApiCount?.Where(x => x.ProviderName?.ToLower() == obj1?.ProviderName?.ToLower())?.FirstOrDefault()?.TotalUnprocessableEntityCount ?? 0;
                        var _faliure = TotalFaliureApiCount?.Where(x => x.ProviderName?.ToLower() == obj1?.ProviderName?.ToLower())?.FirstOrDefault()?.TotalFailureCount ?? 0;
                        obj1.Date = _currDate;
                        //obj1.TotalSuccessApiCount = TotalSuccessApiCount?.Where(x => x.ProviderName?.ToLower() == obj1?.ProviderName?.ToLower())?.FirstOrDefault()?.TotalSuccessApiCount ?? 0;
                        obj1.TotalSuccessApiCount = obj1.TotalApiCount - (_unprocessebleEntity + _faliure);
                        obj1.TotalUnprocessableEntityCount = _unprocessebleEntity;
                        obj1.TotalFailureCount = _faliure;
                        //count++;
                    }
                    DetailedReportRes.AddRange(TotalApiCountProviderWise);
                }


            }

            return DetailedReportRes;
        }
        private List<ConsolidateApiCountJobResponse> GetApiProvider_NameWiseApiCountDetails(List<ApiCounter>? totalApiList)
        {
            List<ConsolidateApiCountJobResponse> ConsolidateReportRes = new();
            List<IGrouping<string?, ApiCounter>>? ProviderNameWiseTotalRequestApiList = totalApiList?.Where(x => x?.Type == "Request")?.GroupBy(x => x.ProviderName)?.ToList();
            List<ApiCounter>? TotalResponseApiList = totalApiList?.Where(x => x?.Type == "Response")?.ToList();

            foreach (IGrouping<string?, ApiCounter>? TotalApiProvider in ProviderNameWiseTotalRequestApiList)
            {
                string? _currApiProvider = TotalApiProvider?.Key?.ToString();
                List<IGrouping<string?, ApiCounter>>? NameWiseTotalRequestApiList = TotalApiProvider?.Where(x => x?.Type == "Request")?.GroupBy(x => x.ApiName)?.ToList();

                foreach (IGrouping<string?, ApiCounter>? TotalApi in NameWiseTotalRequestApiList)
                {
                    string? _currApi = TotalApi?.Key?.ToString();

                    List<ApiCounter>? _currTotalData = TotalApi?.ToList();
                    ConsolidateApiCountJobResponse obj = new();
                    var TotalApiCount = _currTotalData?.Where(x => x?.Type == "Request")?.ToList();

                    List<ApiCounter>? TotalResponsesApiCount = TotalResponseApiList?.Where(x => x.ApiName?.ToLower() == _currApi?.ToLower() && x.ProviderName?.ToLower() == _currApiProvider?.ToLower())?.ToList();
                    List<ApiCounter>? TotalUnproceesbleApiCount = TotalResponsesApiCount?.Where(x => x?.Status == (int)HttpStatusCode.UnprocessableEntity)?.ToList();
                    List<ApiCounter>? TotalFaliureApiCount = TotalResponsesApiCount?.Where(x => x?.Status is not (((int)HttpStatusCode.UnprocessableEntity) or ((int)HttpStatusCode.OK)))?.ToList();
                    int? TotalSuccessApiCount = (_currTotalData?.Count() - (TotalUnproceesbleApiCount?.Count() + TotalFaliureApiCount?.Count()));
                    obj.ProviderName = _currApiProvider;
                    obj.ApiName = _currApi;
                    obj.TotalApiCount = _currTotalData?.Count() ?? 0;
                    obj.TotalSuccessApiCount = TotalSuccessApiCount ?? 0;
                    obj.TotalUnprocessableEntityCount = TotalUnproceesbleApiCount?.Count() ?? 0;
                    obj.TotalFailureCount = TotalFaliureApiCount?.Count() ?? 0;

                    ConsolidateReportRes.Add(obj);

                }

            }

            return ConsolidateReportRes;
        }
        public async Task<AppointeeCountDateWiseDetails> AppointeeCountReport(AppointeeCountReportSearchRequest reqObj)//DateTime? FromDate, DateTime? ToDate)
        {
            AppointeeCountDateWiseDetails _response = new();
            List<AppointeeCountDetails>? _apntDetailsList = new();
            List<AppointeeTotalCount> _appointeeTotalCountList = new();
            List<AppointeeCountDateWise> _appointeeCountDateWises = new();
            List<AppointeeCountDateWise> _appointeeNonProcessDateWise = new();
            if (reqObj.StatusCode == ReportFilterStatus.LinkNotSent || string.IsNullOrEmpty(reqObj.StatusCode))
            {


                List<NonProcessCandidateReportDataResponse> nonProcessAppointeeList = await _reportingDalContext.GetNonProcessCandidateReport(reqObj);

                List<IGrouping<string?, NonProcessCandidateReportDataResponse>>? _noProcessAppointeeCountdateWise = nonProcessAppointeeList.GroupBy(x => x.CreatedOn?.ToShortDateString())?.ToList();

                foreach ((List<NonProcessCandidateReportDataResponse> _currdata, int _totalCount, string _currDate,
                    AppointeeCountDateWise _currDateWiseCount, AppointeeTotalCount _currAppointeeCount) in from obj in _noProcessAppointeeCountdateWise
                                                                                                           let _currdata = obj?.ToList()
                                                                                                           let _totalCount = _currdata?.Count ?? 0
                                                                                                           let _currDate = obj?.Key
                                                                                                           let _currDateWiseCount = new AppointeeCountDateWise()
                                                                                                           let _currAppointeeCount = new AppointeeTotalCount()
                                                                                                           select (_currdata, _totalCount, _currDate, _currDateWiseCount, _currAppointeeCount))
                {
                    List<AppointeeCountDetails>? _apntDetails = _currdata?.Select(x => new AppointeeCountDetails
                    {
                        AppointeeName = x?.AppointeeName,
                        CandidateId = x?.CandidateId,
                        //CompanyId = x?.CompanyId,
                        CompanyName = x?.CompanyName,
                        EmailId = x?.AppointeeEmail,
                        ActionTaken = x?.CreatedOn?.ToShortDateString(),
                        AppointeeStatus = "Link Not Sent",
                        Date = _currDate,
                    })?.ToList();

                    _currAppointeeCount.Date = _currDate;
                    _currAppointeeCount.TotalLinkNotSentCount = _totalCount;
                    _currAppointeeCount.TotalLinkSentCount = 0;
                    _currAppointeeCount.TotalAppointeeCount = _totalCount + 0;
                    _currDateWiseCount.appointeeTotalCount = _currAppointeeCount;
                    _currDateWiseCount.AppointeeCountDetails = _apntDetails;
                    _apntDetailsList.AddRange(_apntDetails);
                    _appointeeCountDateWises.Add(_currDateWiseCount);
                    _appointeeTotalCountList.Add(_currAppointeeCount);
                }
            }
            if (reqObj.StatusCode != ReportFilterStatus.LinkNotSent || string.IsNullOrEmpty(reqObj.StatusCode))
            {
                string? _statusCode = null;
                bool? _intSubmitCode = null;
                int? _intSubStatusCode = null;
                if (!string.IsNullOrEmpty(reqObj.StatusCode))
                {
                    switch (reqObj.StatusCode)
                    {
                        case ReportFilterStatus.ProcessIniNoResponse:
                            _statusCode = WorkFlowStatusType.ProcessIni?.Trim();
                            _intSubStatusCode = 0;
                            break;
                        case ReportFilterStatus.ProcessIniOnGoing:
                            _statusCode = WorkFlowStatusType.ProcessIni?.Trim();
                            _intSubStatusCode = 1;
                            break;
                        case ReportFilterStatus.ProcessIniSubmit:
                            _statusCode = WorkFlowStatusType.ProcessIni?.Trim();
                            _intSubmitCode = true;
                            break;
                        case ReportFilterStatus.Approved:
                            _statusCode = WorkFlowStatusType.Approved?.Trim();
                            break;
                        case ReportFilterStatus.Rejected:
                            _statusCode = WorkFlowStatusType.Rejected?.Trim();
                            break;
                        case ReportFilterStatus.ForcedApproved:
                            _statusCode = WorkFlowStatusType.ForcedApproved?.Trim();
                            break;
                        default:
                            _statusCode = reqObj.StatusCode;
                            break;

                    }
                }


                List<UnderProcessCandidateReportDataResponse> underProcessAppointeeList = await _reportingDalContext.GetUnderProcessCandidateReport(reqObj, _statusCode, _intSubmitCode, _intSubStatusCode);

                List<IGrouping<string?, UnderProcessCandidateReportDataResponse>>? _appointeeCountdateWise = underProcessAppointeeList.GroupBy(x => x.CreatedOn?.ToShortDateString())?.ToList();
                foreach ((List<UnderProcessCandidateReportDataResponse> _currdata, int _totalCount, string _currDate, AppointeeCountDateWise _currDateWiseCount,
                    AppointeeCountDateWise _nonProcessExsistingData, AppointeeTotalCount _currAppointeeCount) in from obj in _appointeeCountdateWise
                                                                                                                 let _currdata = obj?.ToList()
                                                                                                                 let _totalCount = _currdata?.Count ?? 0
                                                                                                                 let _currDate = obj?.Key
                                                                                                                 let _nonProcessExsistingData = _appointeeCountDateWises?.Where(x => x?.appointeeTotalCount?.Date == _currDate).FirstOrDefault()
                                                                                                                 let _currDateWiseCount = new AppointeeCountDateWise()
                                                                                                                 let _currAppointeeCount = new AppointeeTotalCount()
                                                                                                                 select (_currdata, _totalCount, _currDate, _currDateWiseCount, _nonProcessExsistingData, _currAppointeeCount))
                {

                    //var _date = _currdata;
                    List<AppointeeCountDetails>? _apntListDetails = new();
                    List<AppointeeCountDetails>? _apntDetails = _currdata?.Select(x => new AppointeeCountDetails
                    {
                        AppointeeName = x?.AppointeeName,
                        CandidateId = x?.CandidateId,
                        //CompanyId = x?.CompanyId,
                        CompanyName = x?.CompanyName,
                        EmailId = x?.AppointeeEmail,
                        ActionTaken = (x?.AppvlStatusCode != WorkFlowStatusType.ProcessIni?.Trim() && x?.SaveStep == 1) ? x?.UpdatedOn?.ToShortDateString() ?? x?.ActionTakenAt?.ToShortDateString()
                                      : x?.ActionTakenAt?.ToShortDateString(),
                        AppointeeStatus = x?.AppvlStatusCode == WorkFlowStatusType.ProcessIni?.Trim() ?
                                      x?.AppvlStatusDesc + "(" + (x?.IsSubmit ?? false ? "Submitted" : x?.SaveStep == 1 ? "Ongoing" : "No Response") + ")"
                                      : x?.AppvlStatusDesc,
                        Date = _currDate,
                    })?.ToList();
                    _apntDetailsList.AddRange(_apntDetails);
                    if (_nonProcessExsistingData != null)
                    {
                        _nonProcessExsistingData.appointeeTotalCount.TotalAppointeeCount = (_nonProcessExsistingData != null) ? (_nonProcessExsistingData?.appointeeTotalCount?.TotalAppointeeCount ?? 0) + _totalCount : _totalCount;
                        _nonProcessExsistingData.appointeeTotalCount.TotalLinkSentCount = _totalCount;
                    }
                    else
                    {
                        _currAppointeeCount.TotalAppointeeCount = _totalCount;
                        _currAppointeeCount.TotalLinkNotSentCount = 0;
                        _currAppointeeCount.TotalLinkSentCount = _totalCount;
                        _currAppointeeCount.Date = _currDate;
                        _currDateWiseCount.appointeeTotalCount = _currAppointeeCount;
                        _currDateWiseCount.AppointeeCountDetails = _apntDetails;

                        _appointeeCountDateWises.Add(_currDateWiseCount);
                        _appointeeTotalCountList.Add(_currAppointeeCount);
                    }


                }
            }
            _response.AppointeeCountDateWise = _appointeeCountDateWises;
            _response.AppointeeCountDetails = _apntDetailsList?.OrderBy(x => Convert.ToDateTime(x.Date))?.ToList();
            _response.AppointeeTotalCount = _appointeeTotalCountList;

            return _response;
        }

        public async Task<List<AppointeeAgingDataReportDetails>> AppointeeDetailsAgingReport(GetAgingReportRequest reqObj)//DateTime? FromDate, DateTime? ToDate)
        {
            List<AppointeeAgingDataReportDetails> _response = new();

            //int filterdaysrange = FilterDays;
            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            //var startDate = _currDate.AddDays(-filterdaysrange);
            DateTime CurrDate = Convert.ToDateTime(_currDate);
            DateTime? startDate = reqObj?.StartDate;
            DateTime _actionFilterDate = reqObj.NoOfDays > 0 ? _currDate.AddDays(-reqObj.NoOfDays) : _currDate;

            AppointeeSeacrhFilterRequest req = new()
            {
                FromDate = startDate,
            };
            List<UnderProcessWithActionQueryDataResponse> underProcessData = await _workFlowDetailsContext.GetUnderProcessDataWithActionAsync(req);
            var _lastActionDateFilterList = underProcessData?.Where(x => x.LastActionDate <= _actionFilterDate).ToList();
            if (reqObj.ReportType == ReportFilterStatus.ProcessIniNoResponse)
            {
                List<AppointeeAgingDataReportDetails>? _noResponseViewdata = _lastActionDateFilterList?.Where(X => !X.IsJoiningDateLapsed &&
                X?.AppointeeDetails?.IsSubmit != true && X?.AppointeeDetails?.SaveStep != 1)?.DistinctBy(x => x.AppointeeId).Select(row => new AppointeeAgingDataReportDetails
                {
                    AppointeeId = row?.AppointeeDetails?.AppointeeId ?? row?.UnderProcess?.AppointeeId,
                    AppointeeName = row?.AppointeeDetails?.AppointeeName ?? row?.UnderProcess?.AppointeeName,
                    candidateId = row?.UnderProcess?.CandidateId,
                    EmailId = row?.AppointeeDetails?.AppointeeEmailId ?? row?.UnderProcess?.AppointeeEmailId,
                    MobileNo = row?.UnderProcess?.MobileNo,
                    DateOfJoining = row?.AppointeeDetails?.DateOfJoining ?? row?.UnderProcess?.DateOfJoining,
                    CreatedDate = row?.UnderProcess?.CreatedOn,
                    Status = row?.AppointeeDetails?.IsSubmit ?? false ? "Ongoing" : row?.AppointeeDetails?.SaveStep == 1 ? "Ongoing" : "No Response",
                    LastActionDate = row?.LastActionDate,
                    LastActivityDesc = row?.ActivityDesc,
                }).OrderByDescending(y => y.DateOfJoining).ToList();
                _response = _noResponseViewdata;
            }
            else
            {
                List<AppointeeAgingDataReportDetails>? _underProcessViewdata = _lastActionDateFilterList?.Where(X => !X.IsJoiningDateLapsed && (X?.AppointeeDetails?.IsSubmit == true || X?.AppointeeDetails?.SaveStep == 1))?.DistinctBy(x => x.AppointeeId).Select(row => new AppointeeAgingDataReportDetails
                {
                    AppointeeId = row?.AppointeeDetails?.AppointeeId ?? row?.UnderProcess?.AppointeeId,
                    AppointeeName = row?.AppointeeDetails?.AppointeeName ?? row?.UnderProcess?.AppointeeName,
                    candidateId = row?.UnderProcess?.CandidateId,
                    EmailId = row?.AppointeeDetails?.AppointeeEmailId ?? row?.UnderProcess?.AppointeeEmailId,
                    MobileNo = row?.UnderProcess?.MobileNo,
                    DateOfJoining = row?.AppointeeDetails?.DateOfJoining ?? row?.UnderProcess?.DateOfJoining,
                    CreatedDate = row?.UnderProcess?.CreatedOn,
                    Status = row?.AppointeeDetails?.IsSubmit ?? false ? "Ongoing" : row?.AppointeeDetails?.SaveStep == 1 ? "Ongoing" : "No Response",
                    LastActionDate = row?.LastActionDate,
                    LastActivityDesc = row?.ActivityDesc,
                }).OrderByDescending(y => y.DateOfJoining).ToList();
                _response = _underProcessViewdata;

            }
            return _response;
        }

        public async Task<List<AppointeeNationalityDataReportDetails>> AppointeeNationalityDetailsReport(GetNationalityReportRequest reqObj)//DateTime? FromDate, DateTime? ToDate)
        {
            List<AppointeeNationalityDataReportDetails> _response = new();

            //int filterdaysrange = FilterDays;
            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            //var startDate = _currDate.AddDays(-filterdaysrange);
            DateTime CurrDate = Convert.ToDateTime(_currDate);
            DateTime? startDate = reqObj?.FromDate;
            DateTime? _endDate = reqObj?.ToDate;
            //DateTime _actionFilterDate = reqObj.NoOfDays > 0 ? _currDate.AddDays(-reqObj.NoOfDays) : _currDate;

            List<NationalityQueryDataResponse> nationalityData = await _reportingDalContext.GetCandidateNationalityReport(reqObj);
            var Indian = "Indian";
            if (reqObj.nationalityType == NationalityType.Indian)
            {

                List<NationalityQueryDataResponse>? nationalityDataList = nationalityData?.Where(x => x.AppointeeDetails?.Nationality?.ToUpper() == Indian.ToUpper())?.ToList();
                List<AppointeeNationalityDataReportDetails>? _IndianList = GetNationalityList(nationalityDataList);
                _response = _IndianList;
            }
            else if (reqObj.nationalityType == NationalityType.NonIndian)
            {
                List<NationalityQueryDataResponse>? nationalityDataList = nationalityData?.Where(x => x.AppointeeDetails?.Nationality?.ToUpper() != Indian.ToUpper())?.ToList();
                List<AppointeeNationalityDataReportDetails>? _OthersList = GetNationalityList(nationalityDataList);
                _response = _OthersList;

            }
            else
            {
                List<AppointeeNationalityDataReportDetails>? _AllList = GetNationalityList(nationalityData);
                _response = _AllList;

            }
            return _response;
        }

        private List<AppointeeNationalityDataReportDetails>? GetNationalityList(List<NationalityQueryDataResponse> nationalityData)
        {
            return nationalityData?.Select(row => new AppointeeNationalityDataReportDetails
            {
                AppointeeId = row?.AppointeeDetails?.AppointeeId,
                AppointeeName = row?.AppointeeDetails?.AppointeeName,
                candidateId = row?.AppointeeDetails?.CandidateId,
                EmailId = row?.AppointeeDetails?.AppointeeEmailId,
                MobileNo = row?.AppointeeDetails?.MobileNo,
                Nationality = row?.AppointeeDetails?.Nationality,
                CountryName = string.IsNullOrEmpty(row?.AppointeeDetails?.OriginCountry) ? "N/A" : row?.AppointeeDetails?.OriginCountry,
                StartDate = row?.AppointeeDetails?.PassportValidFrom?.ToShortDateString() ?? "N/A",
                ExpiryDate = row?.AppointeeDetails?.PassportValidTill?.ToShortDateString() ?? "N/A",
                PassportNumber = string.IsNullOrEmpty(row?.AppointeeDetails?.PassportNo) ? "N/A" : CommonUtility.DecryptString(key, row?.AppointeeDetails?.PassportNo),

            }).OrderByDescending(y => y.AppointeeId).ToList();
        }

        public async Task<List<AppointeeDataFilterReportDetails>> AppointeeDetailsReport(AppointeeDataFilterReportRequest reqObj)//DateTime? FromDate, DateTime? ToDate)
        {

            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime CurrDate = Convert.ToDateTime(_currDate);
            string status = string.Empty;
            List<AppointeeDataFilterReportDetails> _response = new();
            bool _IsAllData = reqObj.StatusCode?.ToLower()?.Trim() == FilterCode.All?.ToLower()?.Trim();
            if (reqObj.StatusCode == FilterCode.VERIFIED || _IsAllData)
            {
                ProcessedFilterRequest filterRequest = new ProcessedFilterRequest()
                {
                    IsFiltered = true,
                    FromDate = reqObj.FromDate,
                    ToDate = reqObj.ToDate,
                    AppointeeName = reqObj.AppointeeName,
                    CandidateId = reqObj.CandidateId,
                };

                List<ProcessedDataDetailsResponse> list = await _workFlowDetailsContext.GetProcessedAppointeeDetailsAsync(filterRequest);
                var _processViewdata = list?.DistinctBy(x => x.AppointeeId)?.OrderByDescending(x => x.ProcessedId)?.Select(row => new AppointeeDataFilterReportDetails
                {
                    candidateId = row?.CandidateId,
                    AppointeeName = row?.AppointeeName,
                    AppointeeId = row?.AppointeeId,
                    EmailId = row?.AppointeeEmailId,
                    MobileNo = row?.MobileNo,
                    DateOfJoining = row?.DateOfJoining,
                    Status = row?.StateAlias == WorkFlowStatusType.ForcedApproved ? "Manual Override" : "Verified",
                    CreatedDate = row?.CreatedDate
                }).ToList();
                _response.AddRange(_processViewdata);
            }

            if (reqObj.StatusCode == FilterCode.REJECTED || _IsAllData)
            {
                FilterRequest filterRequest = new FilterRequest()
                {
                    FromDate = reqObj.FromDate,
                    ToDate = reqObj.ToDate,
                    AppointeeName = reqObj.AppointeeName,
                    CandidateId = reqObj.CandidateId,
                };

                List<RejectedDataDetailsResponse> rejectedAppointeeList = await _workFlowDetailsContext.GetRejectedAppointeeDetailsAsync(filterRequest);
                var _rejectedViewdata = rejectedAppointeeList?.DistinctBy(x => x.AppointeeId)?.OrderByDescending(x => x.RejectedId)?.Select(row => new AppointeeDataFilterReportDetails
                {
                    candidateId = row?.CandidateId,
                    AppointeeName = row?.AppointeeName,
                    AppointeeId = row?.AppointeeId,
                    EmailId = row?.AppointeeEmailId,
                    MobileNo = row?.MobileNo,
                    DateOfJoining = row?.DateOfJoining,
                    Status = "Rejected",
                    CreatedDate = row?.CreatedDate
                }).ToList();
                _response.AddRange(_rejectedViewdata);
            }

            AppointeeSeacrhFilterRequest UnderProcessfilterRequest = new()
            {
                FromDate = reqObj.FromDate,
                ToDate = reqObj.ToDate,
                AppointeeName = reqObj.AppointeeName,
                CandidateId = reqObj.CandidateId,

            };

            List<UnderProcessQueryDataResponse> underProcessData = await _workFlowDetailsContext.GetUnderProcessDataAsync(UnderProcessfilterRequest);

            if (reqObj.StatusCode == FilterCode.LAPSED || _IsAllData)
            {
                List<AppointeeDataFilterReportDetails>? _lapsedViewdata = underProcessData?.Where(X => X.IsJoiningDateLapsed)?.DistinctBy(x => x.AppointeeId).Select(row => new AppointeeDataFilterReportDetails
                {
                    AppointeeId = row?.AppointeeDetails?.AppointeeId ?? row?.UnderProcess?.AppointeeId,
                    AppointeeName = row?.AppointeeDetails?.AppointeeName ?? row?.UnderProcess?.AppointeeName,
                    candidateId = row?.UnderProcess?.CandidateId,
                    EmailId = row?.AppointeeDetails?.AppointeeEmailId ?? row?.UnderProcess?.AppointeeEmailId,
                    MobileNo = row?.UnderProcess?.MobileNo,
                    DateOfJoining = row?.AppointeeDetails?.DateOfJoining ?? row?.UnderProcess?.DateOfJoining,
                    CreatedDate = row?.UnderProcess?.CreatedOn,
                    Status = "Lasped",
                }).ToList();
                _response.AddRange(_lapsedViewdata);
            }
            if (reqObj.StatusCode == FilterCode.UNDERPROCESS || _IsAllData)
            {

                List<AppointeeDataFilterReportDetails>? _underProcessViewdata = underProcessData?.Where(X => !X.IsJoiningDateLapsed)?.DistinctBy(x => x.AppointeeId).Select(row => new AppointeeDataFilterReportDetails
                {
                    AppointeeId = row?.AppointeeDetails?.AppointeeId ?? row?.UnderProcess?.AppointeeId,
                    AppointeeName = row?.AppointeeDetails?.AppointeeName ?? row?.UnderProcess?.AppointeeName,
                    candidateId = row?.UnderProcess?.CandidateId,
                    EmailId = row?.AppointeeDetails?.AppointeeEmailId ?? row?.UnderProcess?.AppointeeEmailId,
                    MobileNo = row?.UnderProcess?.MobileNo,
                    DateOfJoining = row?.AppointeeDetails?.DateOfJoining ?? row?.UnderProcess?.DateOfJoining,
                    CreatedDate = row?.UnderProcess?.CreatedOn,
                    Status = string.IsNullOrEmpty(status) ? row?.AppointeeDetails?.IsSubmit ?? false ? "Ongoing" : row?.AppointeeDetails?.SaveStep == 1 ? "Ongoing" : "No Response" : status,
                }).ToList();
                _response.AddRange(_underProcessViewdata);

            }
            var res = _response?.OrderByDescending(y => y?.DateOfJoining)?.ToList();
            return res;
        }
        public async Task<List<AppointeeDataPfReportResponse>> AppointeePfDetailsReport(AppointeeDataFilterReportRequest reqObj)//DateTime? FromDate, DateTime? ToDate)
        {

            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime CurrDate = Convert.ToDateTime(_currDate);
            string status = string.Empty;
            List<AppointeeDataPfReportResponse> _response = new();
            bool _IsAllData = reqObj.StatusCode?.ToLower()?.Trim() == FilterCode.All?.ToLower()?.Trim();
            //if (reqObj.StatusCode == FilterCode.VERIFIED || _IsAllData)
            //{
            ProcessedFilterRequest filterRequest = new ProcessedFilterRequest()
            {
                IsFiltered = true,
                FromDate = reqObj.FromDate,
                ToDate = reqObj.ToDate,
                AppointeeName = reqObj.AppointeeName,
                CandidateId = reqObj.CandidateId,
            };

            List<ProcessedDataDetailsResponse> list = await _workFlowDetailsContext.GetProcessedAppointeeDetailsAsync(filterRequest);
            var _processPfViewdata = list?.DistinctBy(x => x.AppointeeId)?.OrderByDescending(x => x.ProcessedId)?.Select(row => new AppointeeDataPfReportResponse
            {
                candidateId = row?.CandidateId,
                AppointeeName = row?.AppointeeName,
                AppointeeId = row?.AppointeeId,
                EmailId = row?.AppointeeEmailId,
                MobileNo = row?.MobileNo,
                DateOfJoining = row?.DateOfJoining,
                Status = row?.StateAlias == WorkFlowStatusType.ForcedApproved ? "Manual Override" : "Verified",
                TrustPassBookStatus = row?.AppointeeData?.IsTrustPassbook == null ? "NA" : row?.AppointeeData?.IsTrustPassbook ?? false ? "Yes " : "No",
                EPFOPassBookStatus = row?.AppointeeData?.IsUanAvailable == false && string.IsNullOrEmpty(row?.AppointeeData?.UANNumber) ? "No" : row?.AppointeeData?.IsPassbookFetch ?? false ? "Yes " : "NA",
                CreatedDate = row?.CreatedDate
            }).ToList();
            _response.AddRange(_processPfViewdata);
            //}


            var res = _response?.OrderByDescending(y => y?.DateOfJoining)?.ToList();
            return res;
        }


        public async Task<List<AppointeePfStatusDataFilterReportResponse>> AppointeePfDetailsFileterReport(AppointeePfDataFilterReportRequest reqObj)
        {

            List<AppointeePfStatusDataFilterReportResponse> responseList = new();

            try
            {

                bool? isTrusPassbook = null;
                bool? EpfoPassbook = null;
                bool? manualPassbook = reqObj.IsManual switch
                {
                    (int)CommonEnum.StatusType.Yes => true,
                    (int)CommonEnum.StatusType.No => false,

                    _ => null,
                };
                switch (reqObj.PfType)
                {
                    case (int)CommonEnum.PfType.TrustPassbook:
                        isTrusPassbook = true;
                        break;

                    case (int)CommonEnum.PfType.EpfoPassBook:
                        EpfoPassbook = true;
                        break;

                    case (int)CommonEnum.PfType.Na:
                        isTrusPassbook = false;
                        EpfoPassbook = false;
                        break;
                    case (int)CommonEnum.PfType.EPFnTrus:
                        isTrusPassbook = true;
                        EpfoPassbook = true;
                        break;

                    default:
                        isTrusPassbook = null;
                        EpfoPassbook = null;
                        break;
                }


                var filterRequest = new PfDataFilterReportRequest
                {
                    IsManual = manualPassbook,
                    IsTrustPassbook = isTrusPassbook,
                    IsEpfoPassbook = EpfoPassbook,
                    IsPensionGapIdentified = reqObj.PensionStatus,
                    FromDate = reqObj.FromDate,
                    ToDate = reqObj.ToDate,

                };

                // Fetch the processed data based on the filter
                var processedDataList = await _workFlowDetailsContext.GetAppointeePfdetailsAsync(filterRequest);

                // Filter and transform the processed data into the required response format
                var filteredData = processedDataList?
                    .DistinctBy(x => x.AppointeeId)
                    .OrderByDescending(x => x.ProcessedId)
                    .Select(row => new AppointeePfStatusDataFilterReportResponse
                    {
                        candidateId = row?.CandidateId,
                        AppointeeName = row?.AppointeeName,
                        AppointeeId = row?.AppointeeId,
                        EmailId = row?.AppointeeEmailId,
                        MobileNo = row?.MobileNo,
                        DateOfJoining = row?.DateOfJoining,
                        PensionStatus = row?.PensionGapIdentified == null ? "NA" : row?.PensionGapIdentified == true ? "Yes" : "No",
                        EPFOPassBookStatus = !string.IsNullOrEmpty(row?.Uan) ? "Epfo" : "No UAN",
                        TrustPassBookStatus = row?.IsTrustPassbook == null ? string.Empty : (row?.IsTrustPassbook == true ? "Trust" : string.Empty),
                        IsManual = row?.IsManualPassbook == null ? "NA" : (row?.IsManualPassbook == true ? "Manual" : "Auto"),
                        UAN = string.IsNullOrEmpty(row?.Uan) ? "NA" : CommonUtility.DecryptString(key, row?.Uan),
                        AadharNumber = string.IsNullOrEmpty(row?.AadhaarNumberView) ? "NA" : row?.AadhaarNumberView,
                        IsUanAadharLink="Yes",
                        CreatedDate = row?.CreatedDate
                    })
                    .ToList();

                if (filteredData != null)
                {
                    responseList.AddRange(filteredData);
                }

                return responseList.OrderByDescending(y => y?.DateOfJoining).ToList();
            }
            catch (Exception ex)
            {

                return new List<AppointeePfStatusDataFilterReportResponse>();
            }


        }
        public async Task<List<AppointeePfDataExcelRespopnse>> GetAppointeePfDataExcelReport(List<AppointeePfStatusDataFilterReportResponse> appointeeStatusList)
        {
            var excelDataList = appointeeStatusList.Select(x => new AppointeePfDataExcelRespopnse
            {
                CandidateId = x.candidateId,
                AppointeeName = x.AppointeeName,
                AppointeeEmailId = x.EmailId,
                MobileNo = x.MobileNo,
                DateOfJoining = x.DateOfJoining?.ToShortDateString(),
                Status = x.EPFOPassBookStatus,
                IsTrustPassbook = string.IsNullOrEmpty(x.TrustPassBookStatus) ? "NA" : x.TrustPassBookStatus,
                Uan = x.UAN,
                AadhaarNumberView = x.AadharNumber,
                IsManualPassbook = x.IsManual,
                PensionGapIdentified = x.PensionStatus
            }).ToList();

            return excelDataList;
        }
      
        public async Task<List<ManualVerificationExcelDataResponse>> GetAppointeeManualVerificationExcelReport(List<ManualVerificationProcessDetailsResponse> processDetails)

        {
            var excelDataList = processDetails.Select(x => new ManualVerificationExcelDataResponse
            {
                id = x.id,
                candidateId = x.candidateId,
                appointeeId = x.appointeeId,
                appointeeName = x.appointeeName,
                appointeeEmailId = x.appointeeEmailId,
                mobileNo = x.mobileNo,
                dateOfJoining = x.dateOfJoining?.ToShortDateString(),
                isDocSubmitted = x.isDocSubmitted,
                isNoIsuueinVerification = x.isNoIsuueinVerification,
                Status = x.status
            }).ToList();

            return await Task.FromResult(excelDataList);
        }

                
    }
}
