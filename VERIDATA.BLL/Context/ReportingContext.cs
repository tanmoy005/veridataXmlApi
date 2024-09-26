﻿using System.Net;
using VERIDATA.BLL.Interfaces;
using VERIDATA.BLL.utility;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.Model.Configuration;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Config;
using VERIDATA.Model.Table.Public;
using static VERIDATA.BLL.utility.CommonEnum;

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
                    panNo = string.IsNullOrEmpty(row?.AppointeeData?.PANNumber) ? null : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row?.AppointeeData?.PANNumber)),
                    dateOfJoining = row?.AppointeeData?.DateOfJoining,
                    epfWages = row?.AppointeeData?.EPFWages,
                    uanNo = string.IsNullOrEmpty(row?.AppointeeData?.UANNumber) ? null : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row?.AppointeeData?.UANNumber)),
                    status = row?.ProcessData?.DataUploaded ?? false ? "Downloaded" : string.Empty,
                    isPensionApplicable = row?.AppointeeData?.IsPensionApplicable == null ? string.Empty : row?.AppointeeData?.IsPensionApplicable ?? false ? "Yes" : "No",
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
                    CreatedDate = x?.CreatedDate?.ToShortDateString() ?? string.Empty,
                    Status = x?.Status,
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

                List<IGrouping<string?, NonProcessCandidateReportDataResponse>>? _noProcessAppointeeCountdateWise = nonProcessAppointeeList.GroupBy(x => x.CreatedOn?.ToString("dd-MM-yyyy"))?.ToList();

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
                        CompanyName = x?.CompanyName,
                        EmailId = x?.AppointeeEmail,
                        ActionTaken = x?.CreatedOn?.ToString("dd-MM-yyyy"),
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
                            _statusCode = WorkFlowType.ProcessIni?.Trim();
                            _intSubStatusCode = 0;
                            break;
                        case ReportFilterStatus.ProcessIniOnGoing:
                            _statusCode = WorkFlowType.ProcessIni?.Trim();
                            _intSubStatusCode = 1;
                            break;
                        case ReportFilterStatus.ProcessIniSubmit:
                            _statusCode = WorkFlowType.ProcessIni?.Trim();
                            _intSubmitCode = true;
                            break;
                        case ReportFilterStatus.Approved:
                            _statusCode = WorkFlowType.Approved?.Trim();
                            break;
                        case ReportFilterStatus.Rejected:
                            _statusCode = WorkFlowType.Rejected?.Trim();
                            break;
                        case ReportFilterStatus.ForcedApproved:
                            _statusCode = WorkFlowType.ForcedApproved?.Trim();
                            break;
                        default:
                            _statusCode = reqObj.StatusCode;
                            break;

                    }
                }


                List<UnderProcessCandidateReportDataResponse> underProcessAppointeeList = await _reportingDalContext.GetUnderProcessCandidateReport(reqObj, _statusCode, _intSubmitCode, _intSubStatusCode);

                List<IGrouping<string?, UnderProcessCandidateReportDataResponse>>? _appointeeCountdateWise = underProcessAppointeeList.GroupBy(x => x.CreatedOn?.ToString("dd-MM-yyyy"))?.ToList();
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
                        CompanyName = x?.CompanyName,
                        EmailId = x?.AppointeeEmail,
                        ActionTaken = (x?.AppvlStatusCode != WorkFlowType.ProcessIni?.Trim() && x?.SaveStep == 1) ? x?.UpdatedOn?.ToString("dd-MM-yyyy") ?? x?.ActionTakenAt?.ToString("dd-MM-yyyy")
                                      : x?.ActionTakenAt?.ToString("dd-MM-yyyy"),
                        AppointeeStatus = x?.AppvlStatusCode == WorkFlowType.ProcessIni?.Trim() ?
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
                        _currAppointeeCount.TotalAppointeeCount =  _totalCount;
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

        public async Task<List<AppointeeDataFilterReportResponse>> AppointeeDetailsReport(AppointeeDataFilterReportRequest reqObj)//DateTime? FromDate, DateTime? ToDate)
        {

            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime CurrDate = Convert.ToDateTime(_currDate);
            string status = string.Empty;
            List<AppointeeDataFilterReportResponse> _response = new();
            bool _IsAllData = reqObj.StatusCode?.ToLower()?.Trim() == FilterCode.All?.ToLower()?.Trim();
            if (reqObj.StatusCode == FilterCode.VERIFIED || _IsAllData)
            {
                ProcessedFilterRequest filterRequest = new ProcessedFilterRequest()
                {
                    IsFiltered=true,
                    FromDate = reqObj.FromDate,
                    ToDate = reqObj.ToDate,
                    AppointeeName = reqObj.AppointeeName,
                    CandidateId = reqObj.CandidateId,
                };

                List<ProcessedDataDetailsResponse> list = await _workFlowDetailsContext.GetProcessedAppointeeDetailsAsync(filterRequest);
                var _processViewdata = list?.DistinctBy(x => x.AppointeeId)?.OrderByDescending(x => x.ProcessedId)?.Select(row => new AppointeeDataFilterReportResponse
                {
                    candidateId = row?.CandidateId,
                    AppointeeName = row?.AppointeeName,
                    AppointeeId = row?.AppointeeId,
                    EmailId = row?.AppointeeEmailId,
                    MobileNo = row?.MobileNo,
                    DateOfJoining = row?.DateOfJoining,
                    Status = row?.StateAlias == WorkFlowType.ForcedApproved ? "Manual Override" : "Verified",
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
                var _rejectedViewdata = rejectedAppointeeList?.DistinctBy(x => x.AppointeeId)?.OrderByDescending(x => x.RejectedId)?.Select(row => new AppointeeDataFilterReportResponse
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
                List<AppointeeDataFilterReportResponse>? _lapsedViewdata = underProcessData?.Where(X => X.IsJoiningDateLapsed)?.DistinctBy(x => x.AppointeeId).Select(row => new AppointeeDataFilterReportResponse
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

                List<AppointeeDataFilterReportResponse>? _underProcessViewdata = underProcessData?.Where(X => !X.IsJoiningDateLapsed)?.DistinctBy(x => x.AppointeeId).Select(row => new AppointeeDataFilterReportResponse
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
    }
}
