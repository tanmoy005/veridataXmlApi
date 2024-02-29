using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PfcAPI.Infrastucture.DBContext;
using PfcAPI.Infrastucture.Interfaces;
using PfcAPI.Infrastucture.Notification.Provider;
using PfcAPI.Infrastucture.utility;
using PfcAPI.Model.Appointee;
using PfcAPI.Model.Configuration;
using PfcAPI.Model.DataAccess;
using PfcAPI.Model.DataTransfer.UAN.Response;
using PfcAPI.Model.ExchangeModels;
using PfcAPI.Model.Maintainance;
using PfcAPI.Model.Master;
using PfcAPI.Model.RequestModel;
using PfcAPI.Model.ResponseModel;
using VERIDATA.Model.Configuration;
using VERIDATA.Model.Public;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using static PfcAPI.Infrastucture.CommonEnum;

namespace PfcAPI.Infrastucture.Context
{
    public class WorkFlowContext : IWorkFlowContext
    {
        private readonly DbContextDB _dbContextClass;
        private readonly IFileContext _fileContext;
        private readonly IEmailSender _emailSender;
        private readonly ApiConfiguration _aadhaarConfig;
        private readonly ConfigurationSetup _configSetup;
        private readonly string key;
        public WorkFlowContext(DbContextDB dbContextClass, IFileContext fileContext, IEmailSender emailSender, ApiConfiguration aadhaarConfig, ConfigurationSetup configSetup)
        {
            _dbContextClass = dbContextClass;
            _fileContext = fileContext;
            _emailSender = emailSender;
            _aadhaarConfig = aadhaarConfig;
            _configSetup = configSetup;
            key = aadhaarConfig.EncriptKey;

        }

        private async Task<List<int?>> GetTotalOfferAppointeeList(int FilterDays)
        {
            var filterdaysrange = FilterDays;
            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime? startDate = filterdaysrange > 0 ? _currDate.AddDays(-filterdaysrange) : null;


            var querydata = from w in _dbContextClass.WorkFlowDetailsHist
                            join u in _dbContextClass.UnderProcessFileData
                            on w.AppointeeId equals u.AppointeeId
                            where w.StateId.Equals(1) & w.AppvlStatusId.Equals(1) & u.ActiveStatus.Equals(true) & (startDate == null || w.CreatedOn >= startDate)
                            select new { w };
            var data = await querydata.ToListAsync().ConfigureAwait(false);
            var workfdata = data.Select(x => x.w).ToList();
            var appinteeList = workfdata?.DistinctBy(x => x.AppointeeId).Select(x => x.AppointeeId).ToList();
            return appinteeList;
        }
        //public async Task<List<UnderProcessDataModel>> GetUnderProcessDataAsync(int? companyId, bool IsFiltered, int NoOfDays, string FilterType)
        public async Task<List<UnderProcessDetailsResponse>> GetUnderProcessDataAsync(AppointeeSeacrhFilterRequest reqObj)
        {
            var _underProcessViewdata = new List<UnderProcessDetailsResponse>();
            var _underProcessdata = new List<UnderProcessDetailsResponse>();
            string CurrDate = DateTime.Now.ToShortDateString();
            DateTime _CurrDate = Convert.ToDateTime(CurrDate);
            DateTime? _ToDate = reqObj.ToDate != null ? reqObj.ToDate?.AddDays(1) : null;
            // var _underProcessData = await _dbContextClass.UnderProcessFileData.Where(x => x.CompanyId.Equals(companyId)).ToListAsync();
            var _getapprovalStatus = await _dbContextClass.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToArrayAsync();
            var ReprocessState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Reprocess?.Trim());
            var CloseState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ProcessClose?.Trim());
            var RejectState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Rejected?.Trim());
            var ApproveState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Approved?.Trim());
            int? FilterCode = reqObj.FilterType == "UNDPRCS" ? 1 : reqObj.FilterType == "NORES" ? 0 : null;
            //var _appointeeData = await _dbContextClass.AppointeeDetails.Where(x => x.CompanyId.Equals(companyId)).ToListAsync();

            var querydata = from b in _dbContextClass.UnderProcessFileData
                            join w in _dbContextClass.WorkFlowDetails
                            on b.AppointeeId equals w.AppointeeId
                            join p in _dbContextClass.AppointeeDetails
                            on b.AppointeeId equals p.AppointeeId into grouping
                            from p in grouping.Where(x => x.ProcessStatus.GetValueOrDefault() != (ReprocessState.AppvlStatusId)).DefaultIfEmpty()
                            where (!(w.AppvlStatusId == CloseState.AppvlStatusId || w.AppvlStatusId == ApproveState.AppvlStatusId || w.AppvlStatusId == RejectState.AppvlStatusId))
                            //  & w.AppvlStatusId != ReprocessState.AppvlStatusId
                            & (p.IsProcessed.Equals(false) || p.IsProcessed == null)
                            & (string.IsNullOrEmpty(reqObj.AppointeeName) || b.AppointeeName.Contains(reqObj.AppointeeName))
                            & (string.IsNullOrEmpty(reqObj.CandidateId) || b.CandidateId.Contains(reqObj.CandidateId))
                            & (reqObj.FromDate == null || b.CreatedOn >= reqObj.FromDate)
                            & (reqObj.ToDate == null || b.CreatedOn < _ToDate)
                            & b.DateOfJoining > _CurrDate & b.ActiveStatus == true
                            orderby p.IsSubmit
                            select new { b, p, w.AppvlStatusId };

            var list = await querydata.ToListAsync().ConfigureAwait(false);
            if (!reqObj.IsFiltered)
            {
                _underProcessdata = list.Select(row => new UnderProcessDetailsResponse
                {
                    id = row.b.UnderProcessId,
                    fileId = row.b.FileId,
                    companyId = row.b.CompanyId,
                    candidateId = row.p?.CandidateId ?? row.b.CandidateId,
                    appointeeName = row.p?.AppointeeName ?? row.b.AppointeeName,
                    appointeeId = row.b.AppointeeId,
                    appointeeEmailId = row.p?.AppointeeEmailId ?? row.b.AppointeeEmailId,
                    mobileNo = row.b.MobileNo,
                    isPFverificationReq = row.b.IsPFverificationReq,
                    epfWages = row.p?.EPFWages ?? row.b.EPFWages,
                    dateOfOffer = row.b.DateOfOffer,
                    dateOfJoining = row.p?.DateOfJoining ?? row.b.DateOfJoining,
                    isDocSubmitted = row.p?.IsSubmit ?? false,
                    isReprocess = (row.AppvlStatusId == (ReprocessState.AppvlStatusId)),
                    //isNoIsuueinVerification = row.p?.IsAadhaarVarified != null ? (row.p?.IsAadhaarVarified == true && row.p?.IsUanVarified != null ? row.p?.IsUanVarified : row.p?.IsAadhaarVarified) : row.p?.IsAadhaarVarified,
                    isNoIsuueinVerification = !(row.p?.IsAadhaarVarified == false || row.p?.IsUanVarified == false || row.p?.IsPanVarified == false || row.p?.IsPasssportVarified == false),
                    Status = row.p?.IsSubmit ?? false ? "Submitted" : row.p?.SaveStep == 1 ? "Ongoing" : "No Response",
                    StatusCode = row.p?.IsSubmit ?? false ? 2 : row.p?.SaveStep ?? 0,
                    CreatedDate = row.b?.CreatedOn
                }).OrderByDescending(x => x.isDocSubmitted).ThenBy(y => y.dateOfJoining).ToList();


            }
            else
            {
                var AppointeeList = await GetTotalOfferAppointeeList(reqObj.NoOfDays);

                _underProcessdata = list?.DistinctBy(x => x.b.AppointeeId).Where(x => AppointeeList.Contains(x.b.AppointeeId)).Select(row => new UnderProcessDetailsResponse
                {
                    id = row.b.UnderProcessId,
                    fileId = row.b.FileId,
                    companyId = row.b.CompanyId,
                    candidateId = row.p?.CandidateId ?? row.b.CandidateId,
                    appointeeName = row.p?.AppointeeName ?? row.b.AppointeeName,
                    appointeeId = row.b.AppointeeId,
                    appointeeEmailId = row.p?.AppointeeEmailId ?? row.b.AppointeeEmailId,
                    mobileNo = row.b.MobileNo,
                    isPFverificationReq = row.b.IsPFverificationReq,
                    epfWages = row.p?.EPFWages ?? row.b.EPFWages,
                    dateOfOffer = row.b.DateOfOffer,
                    dateOfJoining = row.p?.DateOfJoining ?? row.b.DateOfJoining,
                    isDocSubmitted = row.p?.IsSubmit ?? false,
                    isReprocess = (row.AppvlStatusId == (ReprocessState.AppvlStatusId)),
                    //isNoIsuueinVerification = row.p?.IsAadhaarVarified != null ? (row.p?.IsAadhaarVarified == true && row.p?.IsUanVarified != null ? row.p?.IsUanVarified : row.p?.IsAadhaarVarified) : row.p?.IsAadhaarVarified,
                    isNoIsuueinVerification = !(row.p?.IsAadhaarVarified == false || row.p?.IsUanVarified == false || row.p?.IsPanVarified == false || row.p?.IsPasssportVarified == false),
                    Status = row.p?.IsSubmit ?? false ? "Submitted" : row.p?.SaveStep == 1 ? "Ongoing" : "No Response",
                    StatusCode = row.p?.IsSubmit ?? false ? 2 : row.p?.SaveStep ?? 0,
                    CreatedDate = row.b?.CreatedOn
                }).OrderByDescending(x => x.isDocSubmitted).ThenBy(y => y.dateOfJoining).ToList();


            }

            _underProcessViewdata = (FilterCode != null) ? (FilterCode == 0) ? _underProcessdata.Where(x => x.StatusCode == FilterCode).ToList()
                     : _underProcessdata.Where(x => x.StatusCode != 0).ToList() : _underProcessdata;

            _underProcessViewdata = (reqObj.StatusCode == null || reqObj.StatusCode?.ToUpper() == "ALL") ? _underProcessViewdata
                     : _underProcessdata.Where(x => x.StatusCode == Convert.ToInt32(reqObj.StatusCode ?? "0"))?.ToList();


            return _underProcessViewdata;
        }
        public async Task<List<UnderProcessDetailsResponse>> GetExpiredProcessDataAsync(AppointeeSeacrhFilterRequest reqObj)
        {
            var _expiredProcessViewdata = new List<UnderProcessDetailsResponse>();
            var _expiredProcessData = new List<UnderProcessDetailsResponse>();
            string CurrDate = DateTime.Now.ToShortDateString();
            DateTime _CurrDate = Convert.ToDateTime(CurrDate);
            DateTime? _ToDate = reqObj.ToDate != null ? reqObj.ToDate?.AddDays(1) : null;
            // var _underProcessData = await _dbContextClass.UnderProcessFileData.Where(x => x.CompanyId.Equals(companyId)).ToListAsync();
            var _getapprovalStatus = await _dbContextClass.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToArrayAsync();
            var ReprocessState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Reprocess?.Trim());
            var CloseState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ProcessClose?.Trim());
            var RejectState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Rejected?.Trim());
            var ApproveState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Approved?.Trim());
            //var _appointeeData = await _dbContextClass.AppointeeDetails.Where(x => x.CompanyId.Equals(companyId)).ToListAsync();

            var querydata = from b in _dbContextClass.UnderProcessFileData
                            join w in _dbContextClass.WorkFlowDetails
                            on b.AppointeeId equals w.AppointeeId
                            join p in _dbContextClass.AppointeeDetails
                            on b.AppointeeId equals p.AppointeeId into grouping
                            from p in grouping.Where(x => !x.ProcessStatus.GetValueOrDefault().Equals(ReprocessState.AppvlStatusId)).DefaultIfEmpty()
                            where (!(w.AppvlStatusId == CloseState.AppvlStatusId || w.AppvlStatusId == ApproveState.AppvlStatusId || w.AppvlStatusId == RejectState.AppvlStatusId))
                            & (p.IsProcessed.Equals(false) || p.IsProcessed == null)
                            & (string.IsNullOrEmpty(reqObj.AppointeeName) || b.AppointeeName.Contains(reqObj.AppointeeName))
                            & (string.IsNullOrEmpty(reqObj.CandidateId) || b.CandidateId.Contains(reqObj.CandidateId))
                            & (reqObj.FromDate == null || b.CreatedOn >= reqObj.FromDate)
                            & (reqObj.ToDate == null || b.CreatedOn < _ToDate)
                            & b.DateOfJoining < _CurrDate & b.ActiveStatus == true
                            orderby p.IsSubmit
                            select new { b, p };

            var list = await querydata.ToListAsync().ConfigureAwait(false);
            if (!reqObj.IsFiltered)
            {

                _expiredProcessViewdata = list.Select(row => new UnderProcessDetailsResponse
                {
                    id = row.b.UnderProcessId,
                    fileId = row.b.FileId,
                    companyId = row.b.CompanyId,
                    candidateId = row.p?.CandidateId ?? row.b.CandidateId,
                    appointeeName = row.p?.AppointeeName ?? row.b.AppointeeName,
                    appointeeId = row.b.AppointeeId,
                    appointeeEmailId = row.p?.AppointeeEmailId ?? row.b.AppointeeEmailId,
                    mobileNo = row.b.MobileNo,
                    isPFverificationReq = row.b.IsPFverificationReq,
                    epfWages = row.p?.EPFWages ?? row.b.EPFWages,
                    dateOfOffer = row.b.DateOfOffer,
                    dateOfJoining = row.p?.DateOfJoining ?? row.b.DateOfJoining,
                    isDocSubmitted = row.p?.IsSubmit ?? false,
                    isReprocess = false,
                    Status = row.p?.IsSubmit ?? false ? "Submitted" : row.p?.SaveStep == 1 ? "Ongoing" : "No Response",
                    StatusCode = row.p?.IsSubmit ?? false ? 2 : row.p?.SaveStep ?? 0,
                    CreatedDate = row.b?.CreatedOn
                }).OrderByDescending(x => x.isDocSubmitted).ThenBy(y => y.dateOfJoining).ToList();
            }
            else
            {
                var AppointeeList = await GetTotalOfferAppointeeList(reqObj.NoOfDays);
                _expiredProcessViewdata = list?.DistinctBy(x => x.b.AppointeeId).Where(x => AppointeeList.Contains(x.b.AppointeeId)).Select(row => new UnderProcessDetailsResponse
                {
                    id = row.b.UnderProcessId,
                    fileId = row.b.FileId,
                    companyId = row.b.CompanyId,
                    candidateId = row.p?.CandidateId ?? row.b.CandidateId,
                    appointeeName = row.p?.AppointeeName ?? row.b.AppointeeName,
                    appointeeId = row.b.AppointeeId,
                    appointeeEmailId = row.p?.AppointeeEmailId ?? row.b.AppointeeEmailId,
                    mobileNo = row.b.MobileNo,
                    isPFverificationReq = row.b.IsPFverificationReq,
                    epfWages = row.p?.EPFWages ?? row.b.EPFWages,
                    dateOfOffer = row.b.DateOfOffer,
                    dateOfJoining = row.p?.DateOfJoining ?? row.b.DateOfJoining,
                    isDocSubmitted = row.p?.IsSubmit ?? false,
                    isReprocess = false,
                    Status = row.p?.IsSubmit ?? false ? "Submitted" : row.p?.SaveStep == 1 ? "Ongoing" : "No Response",
                    StatusCode = row.p?.IsSubmit ?? false ? 2 : row.p?.SaveStep ?? 0,
                    CreatedDate = row.b?.CreatedOn
                }).OrderByDescending(x => x.isDocSubmitted).ThenBy(y => y.dateOfJoining).ToList();
            }
            _expiredProcessData = (reqObj.StatusCode == null || reqObj.StatusCode?.ToUpper() == "ALL") ? _expiredProcessViewdata :
                _expiredProcessViewdata?.Where(x => x.StatusCode == Convert.ToInt32(reqObj.StatusCode ?? "0"))?.ToList();
            return _expiredProcessData;
        }
        public async Task<List<UnderProcessDetailsResponse>> GetReProcessDataAsync(int? companyId)
        {

            string CurrDate = DateTime.Now.ToShortDateString();
            DateTime _CurrDate = Convert.ToDateTime(CurrDate);
            // var _underProcessData = await _dbContextClass.UnderProcessFileData.Where(x => x.CompanyId.Equals(companyId)).ToListAsync();
            var approvalState = await GetApprovalState(WorkFlowType.Reprocess?.Trim());
            //var _appointeeData = await _dbContextClass.AppointeeDetails.Where(x => x.CompanyId.Equals(companyId)).ToListAsync();

            var querydata = from b in _dbContextClass.UnderProcessFileData
                            join p in _dbContextClass.AppointeeDetails
                            on b.AppointeeId equals p.AppointeeId into grouping
                            from p in grouping.DefaultIfEmpty()
                            where p.ProcessStatus.GetValueOrDefault().Equals(approvalState.AppvlStatusId) & (p.IsProcessed.Equals(false) || p.IsProcessed == null)
                              & b.DateOfJoining > _CurrDate & b.ActiveStatus == true
                            orderby p.IsSubmit
                            select new { b, p };

            var list = await querydata.ToListAsync().ConfigureAwait(false);

            var _underProcessViewdata = list.Select(row => new UnderProcessDetailsResponse
            {
                id = row.b.UnderProcessId,
                fileId = row.b.FileId,
                companyId = row.b.CompanyId,
                candidateId = row.p?.CandidateId ?? row.b.CandidateId,
                appointeeName = row.p?.AppointeeName ?? row.b.AppointeeName,
                appointeeId = row.b.AppointeeId,
                appointeeEmailId = row.p?.AppointeeEmailId ?? row.b.AppointeeEmailId,
                mobileNo = row.p?.MobileNo ?? row.b.MobileNo,
                isPFverificationReq = row.b.IsPFverificationReq,
                epfWages = row.p?.EPFWages ?? row.b.EPFWages,
                dateOfOffer = row.b.DateOfOffer,
                dateOfJoining = row.p?.DateOfJoining ?? row.b.DateOfJoining,
                isDocSubmitted = row.p?.IsSubmit ?? false,
                isReprocess = true,
                CreatedDate = row.p?.CreatedOn
            }).OrderByDescending(x => x.isDocSubmitted).ToList();


            return _underProcessViewdata;
        }
        public async Task<List<RawFileDataModel>> GetNonProcessDataAsync(AppointeeSeacrhFilterRequest reqObj)
        {
            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime? startDate = reqObj.IsFiltered && reqObj.NoOfDays > 0 ? _currDate.AddDays(-reqObj.NoOfDays) : null;
            DateTime? _ToDate = reqObj.ToDate != null ? reqObj.ToDate?.AddDays(1) : null;
            var _unProcessData = await _dbContextClass.UnProcessedFileData.Where(x => x.CompanyId.Equals(reqObj.CompanyId) & x.ActiveStatus == true &
            (startDate == null || x.CreatedOn >= startDate)
            & (string.IsNullOrEmpty(reqObj.AppointeeName) || x.AppointeeName.Contains(reqObj.AppointeeName))
            & (string.IsNullOrEmpty(reqObj.CandidateId) || x.CandidateId.Contains(reqObj.CandidateId))
            & (reqObj.FromDate == null || x.CreatedOn >= reqObj.FromDate)
            & (reqObj.ToDate == null || x.CreatedOn < _ToDate)
            ).ToListAsync();

            var _unProcessViewdata = _unProcessData.Select(row => new RawFileDataModel
            {
                id = row.UnProcessedId,
                fileId = row.FileId,
                companyId = row.CompanyId,
                CandidateId = row.CandidateId,
                appointeeName = row.AppointeeName,
                appointeeEmailId = row.AppointeeEmailId,
                mobileNo = row.MobileNo,
                isPFverificationReq = row.IsPFverificationReq,
                epfWages = row.EPFWages,
                dateOfOffer = row.DateOfOffer,
                dateOfJoining = row.DateOfJoining,
                isChecked = null,
            }).ToList();

            return _unProcessViewdata;
        }
        public async Task<List<ProcessDataResponse>> GetProcessDataAsync(ProcessedFilterRequest filter)
        {
            var _processViewdata = new List<ProcessDataResponse>();
            var _processClosed = await GetApprovalState(WorkFlowType.ProcessClose);
            WorkflowApprovalStatusMaster _filteredStatus = new WorkflowApprovalStatusMaster();
            DateTime? _ToDate = filter.ToDate != null ? filter.ToDate?.AddDays(1) : null;
            if (!string.IsNullOrEmpty(filter.ProcessStatus))
            {
                _filteredStatus = await GetApprovalState(filter.ProcessStatus);
            }
            if (!(filter.IsFiltered ?? false))
            {
                var querydata = from p in _dbContextClass.ProcessedFileData
                                join u in _dbContextClass.UnderProcessFileData
                                   on p.AppointeeId equals u.AppointeeId
                                join w in _dbContextClass.WorkFlowDetails
                                        on p.AppointeeId equals w.AppointeeId
                                join x in _dbContextClass.AppointeeDetails
                                        on p.AppointeeId equals x.AppointeeId into grouping
                                from a in grouping.DefaultIfEmpty()
                                where p.ActiveStatus == true// & a.ActiveStatus == true & a.IsProcessed == true
                                & a.ProcessStatus != _processClosed.AppvlStatusId
                                & (filter.FromDate == null || p.CreatedOn >= filter.FromDate)
                                & (filter.ToDate == null || p.CreatedOn < _ToDate)
                                & (filter.IsPfRequired == null || a.IsPFverificationReq == filter.IsPfRequired)
                                & (string.IsNullOrEmpty(filter.ProcessStatus) || w.AppvlStatusId == _filteredStatus.AppvlStatusId)
                                & (string.IsNullOrEmpty(filter.AppointeeName) || u.AppointeeName.Contains(filter.AppointeeName))
                                & (string.IsNullOrEmpty(filter.CandidateId) || u.CandidateId.Contains(filter.CandidateId))
                                select new
                                {
                                    p.ProcessedId,
                                    p.AppointeeId,
                                    a.AadhaarNumberView,
                                    a.PANNumber,
                                    a.UANNumber,
                                    a.IsPensionApplicable,
                                    a.IsTrustPassbook,
                                    w.StateAlias,
                                    u
                                };

                var list = await querydata.ToListAsync().ConfigureAwait(false);

                _processViewdata = list?.DistinctBy(x => x.AppointeeId)?.OrderByDescending(x => x.ProcessedId)?.Select(row => new ProcessDataResponse
                {
                    id = row.ProcessedId,
                    companyId = row.u.CompanyId,
                    candidateId = row.u.CandidateId,
                    appointeeName = row.u.AppointeeName,
                    appointeeId = row.AppointeeId,
                    appointeeEmailId = row.u.AppointeeEmailId,
                    mobileNo = row.u.MobileNo,
                    adhaarNo = string.IsNullOrEmpty(row.AadhaarNumberView) ? "NA" : row.AadhaarNumberView,
                    panNo = string.IsNullOrEmpty(row.PANNumber) ? "NA" : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row.PANNumber)),
                    dateOfJoining = row.u.DateOfJoining,
                    epfWages = row.u.EPFWages,
                    uanNo = string.IsNullOrEmpty(row.UANNumber) ? "NA" : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row.UANNumber)),
                    status = row.StateAlias == WorkFlowType.ForcedApproved ? "Manual Override" : "Verified",
                    isPensionApplicable = row.IsPensionApplicable == null ? "NA" : row.IsPensionApplicable ?? false ? "Yes" : "No",
                    isTrustPFApplicable = row.IsTrustPassbook ?? false,
                }).ToList();
            }
            else
            {
                var _filterDays = filter.NoOfDays ?? 0;
                var AppointeeList = await GetTotalOfferAppointeeList(_filterDays);
                var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
                var startDate = _filterDays > 0 ? _currDate.AddDays(-(_filterDays)) : DateTime.MinValue;
                var querydata = from p in _dbContextClass.ProcessedFileData
                                join u in _dbContextClass.UnderProcessFileData
                                    on p.AppointeeId equals u.AppointeeId
                                join w in _dbContextClass.WorkFlowDetails
                                on p.AppointeeId equals w.AppointeeId
                                join x in _dbContextClass.AppointeeDetails
                                      on p.AppointeeId equals x.AppointeeId into grouping
                                from a in grouping.DefaultIfEmpty()
                                where p.ActiveStatus == true //& a.ActiveStatus == true & a.IsProcessed == true
                                & (filter.FromDate == null || p.CreatedOn >= filter.FromDate)
                                & (filter.ToDate == null || p.CreatedOn < _ToDate)
                                & (filter.IsPfRequired == null || a.IsPFverificationReq == filter.IsPfRequired)
                                & (string.IsNullOrEmpty(filter.ProcessStatus) || w.AppvlStatusId == _filteredStatus.AppvlStatusId)
                                & (string.IsNullOrEmpty(filter.AppointeeName) || u.AppointeeName.Contains(filter.AppointeeName))
                                & (string.IsNullOrEmpty(filter.CandidateId) || u.CandidateId.Contains(filter.CandidateId))
                                & p.CreatedOn >= startDate
                                select new
                                {
                                    p.ProcessedId,
                                    p.AppointeeId,
                                    a.AadhaarNumberView,
                                    a.PANNumber,
                                    a.UANNumber,
                                    a.IsPensionApplicable,
                                    a.IsTrustPassbook,
                                    w.StateAlias,
                                    u
                                }; 

                var list = await querydata.ToListAsync().ConfigureAwait(false);

                _processViewdata = list?.Where(x => AppointeeList.Contains(x.AppointeeId))?.DistinctBy(x => x.AppointeeId)?.OrderByDescending(x => x.ProcessedId)?.Select(row => new ProcessDataResponse
                {
                    id = row.ProcessedId,
                    companyId = row.u.CompanyId,
                    candidateId = row.u.CandidateId,
                    appointeeName = row.u.AppointeeName,
                    appointeeId = row.AppointeeId,
                    appointeeEmailId = row.u.AppointeeEmailId,
                    mobileNo = row.u.MobileNo,
                    adhaarNo = string.IsNullOrEmpty(row.AadhaarNumberView) ? "NA" : row.AadhaarNumberView,
                    panNo = string.IsNullOrEmpty(row.PANNumber) ? "NA" : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row.PANNumber)),
                    dateOfJoining = row.u.DateOfJoining,
                    epfWages = row.u.EPFWages,
                    uanNo = string.IsNullOrEmpty(row.UANNumber) ? "NA" : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row.UANNumber)),
                    status = row.StateAlias == WorkFlowType.ForcedApproved ? "Manual Override" : "Verified",
                    isPensionApplicable = row.IsPensionApplicable == null ? "NA" : row.IsPensionApplicable ?? false ? "Yes" : "No",
                    isTrustPFApplicable = row.IsTrustPassbook ?? false,
                }).ToList();
            }

            return _processViewdata;
        }
        public async Task<List<ProcessDataResponse>> GetProcessEPFODataAsync(FilterRequest filter)
        {
            DateTime? _ToDate = filter.ToDate != null ? filter.ToDate?.AddDays(1) : null;
            var approvalStatus = await GetApprovalState(WorkFlowType.ProcessClose);
            var querydata = from p in _dbContextClass.ProcessedFileData
                            join a in _dbContextClass.AppointeeDetails
                                on p.AppointeeId equals a.AppointeeId
                            where p.ActiveStatus == true & a.ActiveStatus == true & a.IsProcessed == true
                            & a.IsPFverificationReq == true & a.ProcessStatus != approvalStatus.AppvlStatusId
                            & (filter.FromDate == null || p.CreatedOn >= filter.FromDate)
                            & (filter.ToDate == null || p.CreatedOn < _ToDate)
                            select new { p.ProcessedId, a };

            var list = await querydata.ToListAsync().ConfigureAwait(false);
            var _processViewdata = list.OrderByDescending(x => x.ProcessedId).Select(row => new ProcessDataResponse
            {
                id = row.ProcessedId,
                companyId = row.a.CompanyId,
                candidateId = row.a.CandidateId,
                appointeeName = row.a.AppointeeName,
                appointeeId = row.a.AppointeeId,
                appointeeEmailId = row.a.AppointeeEmailId,
                mobileNo = row.a.MobileNo,
                adhaarNo = row.a.AadhaarNumberView,
                panNo = string.IsNullOrEmpty(row.a.PANNumber) ? null : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row.a.PANNumber)),
                dateOfJoining = row.a.DateOfJoining,
                epfWages = row.a.EPFWages,
            }).ToList();


            return _processViewdata;
        }
        public async Task<List<ProcessDataResponse>> GetProcessMISDataAsync(FilterRequest filter)
        {
            DateTime? _ToDate = filter.ToDate != null ? filter.ToDate?.AddDays(1) : null;
            var approvalStatus = await GetApprovalState(WorkFlowType.ProcessClose);
            var querydata = from p in _dbContextClass.ProcessedFileData
                            join a in _dbContextClass.AppointeeDetails
                                on p.AppointeeId equals a.AppointeeId
                            where p.ActiveStatus == true & a.ActiveStatus == true & a.IsProcessed == true
                            & a.IsPFverificationReq != true & a.ProcessStatus != approvalStatus.AppvlStatusId
                            & (filter.FromDate == null || p.CreatedOn >= filter.FromDate)
                            & (filter.ToDate == null || p.CreatedOn < _ToDate)
                            & (string.IsNullOrEmpty(filter.AppointeeName) || a.AppointeeName.Contains(filter.AppointeeName))
                            select new { p.ProcessedId, a };

            var list = await querydata.ToListAsync().ConfigureAwait(false);
            var _processViewdata = list.OrderByDescending(x => x.ProcessedId).Select(row => new ProcessDataResponse
            {
                id = row.ProcessedId,
                companyId = row.a.CompanyId,
                candidateId = row.a.CandidateId,
                appointeeName = row.a.AppointeeName,
                appointeeId = row.a.AppointeeId,
                appointeeEmailId = row.a.AppointeeEmailId,
                mobileNo = row.a.MobileNo,
                adhaarNo = row.a.AadhaarNumberView,
                panNo = string.IsNullOrEmpty(row.a.PANNumber) ? null : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row.a.PANNumber)),
                dateOfJoining = row.a.DateOfJoining,
                epfWages = row.a.EPFWages,
            }).ToList();


            return _processViewdata;
        }
        public async Task<List<RejectedDataResponse>> GetRejectedDataAsync(FilterRequest filter)
        {

            DateTime? _ToDate = filter.ToDate != null ? filter.ToDate?.AddDays(1) : null;
            var pAppntedata = from r in _dbContextClass.RejectedFileData
                              join u in _dbContextClass.UnderProcessFileData
                                  on r.AppointeeId equals u.AppointeeId
                              join rp in _dbContextClass.AppointeeReasonMappingData
                                  on u.AppointeeId equals rp.AppointeeId
                              join rm in _dbContextClass.ReasonMaser
                                  on rp.ReasonId equals rm.ReasonId
                              join a in _dbContextClass.AppointeeDetails
                                  on r.AppointeeId equals a.AppointeeId into grouping
                              from x in grouping.DefaultIfEmpty()

                              where r.ActiveStatus == true & rm.ActiveStatus == true
                              //& a.ActiveStatus == true 
                              & rm.ActiveStatus == true && rp.ActiveStatus == true
                              // & a.ProcessStatus != approvalStatus.AppvlStatusId
                              & (string.IsNullOrEmpty(filter.AppointeeName) || u.AppointeeName.Contains(filter.AppointeeName))
                              & (string.IsNullOrEmpty(filter.CandidateId) || u.CandidateId.Contains(filter.CandidateId))
                              & (filter.FromDate == null || r.CreatedOn >= filter.FromDate)
                              & (filter.ToDate == null || r.CreatedOn < _ToDate)
                              select new
                              {
                                  r.RejectedId,
                                  u.CompanyId,
                                  u.AppointeeId,
                                  u.CandidateId,
                                  u.AppointeeName,
                                  u.MobileNo,
                                  u.AppointeeEmailId,
                                  u.DateOfJoining,
                                  x.PANNumber,
                                  x.AadhaarNumberView,
                                  rp.Remarks
                              };

            var rejectedAppointeeList = await pAppntedata.ToListAsync().ConfigureAwait(false);

            var response = new List<RejectedDataResponse>();
            var GroupData = rejectedAppointeeList.GroupBy(x => x.AppointeeId).ToList();
            foreach (var item in GroupData.Select((value, index) => new { Value = value, Index = index }))
            {
                var x = item.Value;
                var ResonDetails = x.Select(y => y.Remarks).ToList();
                var Remarks = string.Join(",", ResonDetails);
                var _data = x.Select(row =>

                    new RejectedDataResponse
                    {
                        id = row.RejectedId,
                        companyId = row.CompanyId,
                        candidateId = row.CandidateId,
                        appointeeName = row.AppointeeName,
                        appointeeId = row.AppointeeId,
                        appointeeEmailId = row.AppointeeEmailId,
                        mobileNo = row.MobileNo,
                        adhaarNo = string.IsNullOrEmpty(row.AadhaarNumberView) ? "NA" : row.AadhaarNumberView,
                        panNo = string.IsNullOrEmpty(row.PANNumber) ? "NA" : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row.PANNumber)),
                        dateOfJoining = row.DateOfJoining,
                        RejectReason = Remarks,
                        RejectFrom = "",
                    }).FirstOrDefault();
                response.Add(_data);
            }

            return response;
        }
        public async Task<List<UnderProcessFileData>> PostUnderProcessDataAsync(List<RawFileDataModel> underprocessdata, int userId)
        {
            var _underProcessList = new List<UnderProcessFileData>();

            foreach (var (obj, user) in from obj in underprocessdata
                                        let user = new AppointeeMaster()
                                        select (obj, user))
            {
                var appointee = new AppointeeMaster
                {
                    CandidateId = obj.CandidateId,
                    AppointeeName = obj.appointeeName,
                    AppointeeEmailId = obj.appointeeEmailId,
                    MobileNo = obj.mobileNo,
                    FileId = obj.fileId,
                    ActiveStatus = true,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now
                };
                _dbContextClass.AppointeeMaster.Add(appointee);
                await _dbContextClass.SaveChangesAsync();

                var _underProcessdata = new UnderProcessFileData
                {
                    FileId = obj.fileId,
                    CompanyId = obj.companyId,
                    CompanyName = obj.companyName,
                    AppointeeId = appointee.AppointeeId,
                    CandidateId = obj.CandidateId,
                    AppointeeName = obj.appointeeName,
                    AppointeeEmailId = obj.appointeeEmailId,
                    MobileNo = obj.mobileNo,
                    IsPFverificationReq = obj.isPFverificationReq,
                    EPFWages = obj.epfWages,
                    DateOfOffer = obj.dateOfOffer,
                    DateOfJoining = obj.dateOfJoining,
                    IsProcessed = false,
                    ActiveStatus = true,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    lvl1Email = obj.lvl1Email,
                    lvl2Email = obj.lvl2Email,
                    lvl3Email = obj.lvl3Email,
                    //IsChecked = null,
                };
                _underProcessList.Add(_underProcessdata);
            }

            _dbContextClass.UnderProcessFileData.AddRange(_underProcessList);
            await _dbContextClass.SaveChangesAsync();
            return _underProcessList;
        }
        public async Task PostNonProcessDataAsync(List<RawFileDataModel> unprocessdata, int userId)
        {
            var _unProcessdata = unprocessdata.Select(row => new UnProcessedFileData
            {
                FileId = row.fileId,
                CompanyId = row.companyId,
                CandidateId = row.CandidateId,
                CompanyName = row.companyName,
                AppointeeName = row.appointeeName,
                AppointeeEmailId = row.appointeeEmailId,
                MobileNo = row.mobileNo,
                IsPFverificationReq = row.isPFverificationReq,
                EPFWages = row.epfWages,
                DateOfOffer = row.dateOfOffer,
                DateOfJoining = row.dateOfJoining,
                ActiveStatus = true,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                lvl1Email = row.lvl1Email,
                lvl2Email = row.lvl2Email,
                lvl3Email = row.lvl3Email,
                //IsChecked = null,
            }).ToList();

            _dbContextClass.UnProcessedFileData.AddRange(_unProcessdata);
            await _dbContextClass.SaveChangesAsync();
        }
        public async Task Appointee_Workflow_Ini_Async(List<int?> appointeeList, int workflowState, int userId)
        {
            var approvalStateId = await GetWorkFlow_Approval_StatusId(WorkFlowType.ProcessIni);

            if (appointeeList?.Count() > 0)
            {
                var _workFlow_det = await _dbContextClass.WorkFlowDetails.Where(x => appointeeList.Contains(x.AppointeeId) & x.ActiveStatus == true).ToListAsync();
                if (_workFlow_det?.Count > 0)
                {
                    _dbContextClass.WorkFlowDetails.RemoveRange(_workFlow_det);
                    await _dbContextClass.SaveChangesAsync();
                }
                var _genarateWorkflowdata = appointeeList.Select(row => new WorkFlowDetails
                {
                    AppointeeId = row,
                    StateId = workflowState,
                    AppvlStatusId = approvalStateId,
                    Remarks = string.Empty,
                    ReprocessCount = 0,
                    ActionTakenAt = DateTime.Now,
                    ActiveStatus = true,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    StateAlias = WorkFlowType.ProcessIni
                    //IsChecked = null,
                }).ToList();

                var _workFlow_det_his = appointeeList?.Select(x => new WorkFlowDetailsHist
                {
                    AppointeeId = x,
                    StateId = workflowState,
                    AppvlStatusId = approvalStateId,
                    Remarks = string.Empty,
                    ReprocessCount = 0,
                    ActionTakenAt = DateTime.Now,
                    ActiveStatus = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    StateAlias = WorkFlowType.ProcessIni
                }).ToList();

                _dbContextClass.WorkFlowDetails.AddRange(_genarateWorkflowdata);
                _dbContextClass.WorkFlowDetailsHist.AddRange(_workFlow_det_his);

                await _dbContextClass.SaveChangesAsync();
            }
        }
        public async Task<WorkflowApprovalStatusMaster> GetApprovalState(string approvalStatus)
        {
            var approvalState = new WorkflowApprovalStatusMaster();
            var getapprovalStatus = await _dbContextClass.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToArrayAsync();
            if (!string.IsNullOrEmpty(approvalStatus))
            {
                approvalState = getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == approvalStatus?.Trim());
            }
            return approvalState;
        }
        public async Task Appointee_Workflow_Update_Async(WorkFlowDataRequest WorkFlowRequest)
        {
            var approvalState = new WorkflowApprovalStatusMaster();
            var _reprocessCount = 0;
            var getapprovalStatus = await _dbContextClass.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToArrayAsync();
            var _userdata = await _dbContextClass.UserMaster.FirstOrDefaultAsync(x => x.RefAppointeeId == WorkFlowRequest.appointeeId);
            if (!string.IsNullOrEmpty(WorkFlowRequest.approvalStatus))
            {
                approvalState = getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowRequest.approvalStatus?.Trim());

                if ((WorkFlowRequest.approvalStatus == WorkFlowType.Approved) || (WorkFlowRequest.approvalStatus == WorkFlowType.ForcedApproved) ||
                 (WorkFlowRequest.approvalStatus == WorkFlowType.Rejected) || WorkFlowRequest.approvalStatus == WorkFlowType.ProcessClose)
                {
                    await Appointee_Workflow_Processd_Async(WorkFlowRequest, approvalState.AppvlStatusId);

                    _userdata.ActiveStatus = WorkFlowRequest.approvalStatus == WorkFlowType.ProcessClose ? false : _userdata.ActiveStatus;
                    _userdata.CurrStatus = WorkFlowRequest.approvalStatus == WorkFlowType.Rejected ? false : _userdata.CurrStatus;
                    _userdata.UpdatedOn = DateTime.Now;
                    _userdata.UpdatedBy = WorkFlowRequest.userId;
                }

                if (WorkFlowRequest.approvalStatus == WorkFlowType.Reprocess)
                {
                    //_reprocessCount = 1;
                    await Appointee_Workflow_ReProcessd_Async(WorkFlowRequest.appointeeId, approvalState.AppvlStatusId);

                    //_userdata.CurrStatus = true;
                    _userdata.UpdatedOn = DateTime.Now;
                    _userdata.UpdatedBy = WorkFlowRequest.userId;
                }
            }

            var _workFlow_det = await _dbContextClass.WorkFlowDetails.FirstOrDefaultAsync(x => x.AppointeeId == WorkFlowRequest.appointeeId & x.ActiveStatus == true);
            if (_workFlow_det != null)
            {
                _reprocessCount = _workFlow_det?.ReprocessCount ?? 0;
                if (string.IsNullOrEmpty(WorkFlowRequest.approvalStatus))
                {
                    approvalState = getapprovalStatus.FirstOrDefault(x => x.AppvlStatusId == _workFlow_det.AppvlStatusId);
                }

                if (WorkFlowRequest.approvalStatus == WorkFlowType.Reprocess)
                {
                    _reprocessCount = _workFlow_det?.ReprocessCount ?? 0 + 1;
                }

                // await _dbContextClass.SaveChangesAsync();
            }

            await workflowdataUpdate(WorkFlowRequest, approvalState, _reprocessCount, _workFlow_det);
        }
        private async Task workflowdataUpdate(WorkFlowDataRequest WorkFlowRequest, WorkflowApprovalStatusMaster? approvalState, int _reprocessCount, WorkFlowDetails? _workFlow_det)
        {
            var _genarateWorkflowdata = new WorkFlowDetails
            {
                AppointeeId = WorkFlowRequest.appointeeId,
                StateId = WorkFlowRequest.workflowState ?? 0,
                AppvlStatusId = approvalState.AppvlStatusId,
                Remarks = WorkFlowRequest.Remarks,
                ReprocessCount = _reprocessCount,
                StateAlias = WorkFlowRequest.approvalStatus,
                ActionTakenAt = DateTime.Now,
                ActiveStatus = true,
                CreatedBy = WorkFlowRequest.userId,
                CreatedOn = DateTime.Now
            };

            var _workFlow_det_his = new WorkFlowDetailsHist
            {
                AppointeeId = WorkFlowRequest.appointeeId,
                StateId = WorkFlowRequest.workflowState ?? 0,
                AppvlStatusId = approvalState.AppvlStatusId,
                Remarks = WorkFlowRequest.Remarks,
                ReprocessCount = _reprocessCount,
                StateAlias = WorkFlowRequest.approvalStatus,
                ActiveStatus = false,
                ActionTakenAt = DateTime.Now,
                CreatedBy = WorkFlowRequest.userId,
                CreatedOn = DateTime.Now,
            };
            _dbContextClass.WorkFlowDetails.RemoveRange(_workFlow_det);
            _dbContextClass.WorkFlowDetails.AddRange(_genarateWorkflowdata);
            _dbContextClass.WorkFlowDetailsHist.AddRange(_workFlow_det_his);

            await _dbContextClass.SaveChangesAsync();
        }
        private async Task Appointee_Workflow_ReProcessd_Async(int appointeeId, int StatusId)
        {

            var _appointeedetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId)) ?? null;
            if (_appointeedetails != null)
            {
                _appointeedetails.IsSubmit = false;
                _appointeedetails.ProcessStatus = StatusId;
                _appointeedetails.SaveStep = 0;
                _appointeedetails.IsPasssportVarified = null;
                _appointeedetails.IsAadhaarVarified = null;
                _appointeedetails.IsUanVarified = null;
                await _dbContextClass.SaveChangesAsync();
            }
        }
        private async Task Appointee_Workflow_Processd_Async(WorkFlowDataRequest workFlowRequest, int StatusId)
        {

            var _appointeedetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(workFlowRequest.appointeeId) & x.IsProcessed.Equals(false)) ?? null;
            if (_appointeedetails != null)
            {
                _appointeedetails.IsProcessed = true;
                _appointeedetails.ProcessStatus = StatusId;
            }
            if ((workFlowRequest.approvalStatus == WorkFlowType.Approved) || (workFlowRequest.approvalStatus == WorkFlowType.ForcedApproved))
            {
                var _processeddata = new ProcessedFileData
                {
                    AppointeeId = workFlowRequest.appointeeId,
                    ActiveStatus = true,
                    DataUploaded = false,
                    CreatedBy = workFlowRequest.userId,
                    CreatedOn = DateTime.Now,
                };
                _dbContextClass.ProcessedFileData.Add(_processeddata);

            }

            if (workFlowRequest.approvalStatus == WorkFlowType.Rejected)
            {
                var _rejecteddata = new RejectedFileData
                {
                    AppointeeId = workFlowRequest.appointeeId,
                    RejectReason = workFlowRequest.Remarks,
                    RejectState = (Int32)RejectState.ApprovalReject,
                    ActiveStatus = true,
                    CreatedBy = workFlowRequest.userId,
                    CreatedOn = DateTime.Now,
                };
                _dbContextClass.RejectedFileData.Add(_rejecteddata);
            }

            await _dbContextClass.SaveChangesAsync();
        }
        public async Task RemoveRawDataAsync(List<int> RemoveRawId)
        {
            var getRawData = await _dbContextClass.RawFileData.Where(x => RemoveRawId.Contains(x.RawFileId)).ToListAsync();

            if (getRawData != null)
            {
                _dbContextClass.RawFileData.RemoveRange(getRawData);
                await _dbContextClass.SaveChangesAsync();
            }
        }
        public async Task RemoveUnprocessedDataAsync(List<int> RemoveRawId)
        {
            var getRawData = await _dbContextClass.UnProcessedFileData.Where(x => RemoveRawId.Contains(x.UnProcessedId)).ToListAsync();

            if (getRawData != null)
            {
                _dbContextClass.UnProcessedFileData.RemoveRange(getRawData);
                await _dbContextClass.SaveChangesAsync();
            }
        }
        public async Task<int> GetWorkFlowStateId(string StateAlias)
        {
            var getData = await _dbContextClass.WorkFlowStateMaster.FirstOrDefaultAsync(x => x.StateAlias.Equals(StateAlias) & x.ActiveStatus == true);

            return getData?.StateId ?? 0;
        }
        public async Task<string?> Appointee_Workflow_CurrentState(int appointeeId)
        {
            var getcurrentState = await _dbContextClass.WorkFlowDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId) & x.ActiveStatus == true);
            var getApprovalStatus = await _dbContextClass.WorkflowApprovalStatusMaster.FirstOrDefaultAsync(x => x.AppvlStatusId.Equals(getcurrentState.AppvlStatusId) & x.ActiveStatus == true);

            return getApprovalStatus?.AppvlStatusCode;
        }
        public async Task<int> GetWorkFlow_Approval_StatusId(string StateAlias)
        {
            var getRawData = await _dbContextClass.WorkflowApprovalStatusMaster.FirstOrDefaultAsync(x => x.AppvlStatusCode.Equals(StateAlias) & x.ActiveStatus == true);

            return getRawData?.AppvlStatusId ?? 0;
        }
        public async Task PostAppointeeSaveDetailsAsync(AppointeeSaveDetailsRequest AppointeeDetails)
        {
            if (AppointeeDetails != null)
            {
                var appointeeId = AppointeeDetails.AppointeeId;
                var _isPassportAvailable = AppointeeDetails?.IsPassportAvailable?.ToString()?.ToUpper() == CheckType.yes ? true : false;
                var _isInternationalWrkr = AppointeeDetails?.IsInternationalWorker?.ToString()?.ToUpper() == CheckType.yes ? true : false;
                var _isHandicap = AppointeeDetails?.IsHandicap?.ToString()?.ToUpper() == CheckType.yes ? true : false;
                var _appointeedetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId) & x.IsProcessed.Equals(false)) ?? null;
                var _appntundrprocessdata = await _dbContextClass.UnderProcessFileData.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId)) ?? null;
                if (_appointeedetails == null)
                {

                    var data = new AppointeeDetails
                    {
                        AppointeeId = appointeeId,
                        CandidateId = AppointeeDetails?.CandidateId,
                        AppointeeName = AppointeeDetails?.AppointeeName,
                        AppointeeEmailId = AppointeeDetails?.AppointeeEmailId,
                        //PANNumber = AppointeeDetails?.PANNumber,
                        //PANName = AppointeeDetails?.PANName,
                        CompanyId = AppointeeDetails.CompanyId,
                        CompanyName = AppointeeDetails.CompanyName,
                        DateOfBirth = AppointeeDetails.DateOfBirth,
                        DateOfJoining = AppointeeDetails.DateOfJoining,
                        Gender = AppointeeDetails.Gender,
                        EPFWages = AppointeeDetails.EPFWages,
                        IsHandicap = _isHandicap ? AppointeeDetails.IsHandicap?.ToString() : CheckType.no,
                        HandicapeType = _isHandicap ? AppointeeDetails.HandicapeType?.ToString() : string.Empty,
                        IsPassportAvailable = _isPassportAvailable ? AppointeeDetails.IsPassportAvailable?.ToString()?.ToUpper() : CheckType.no,
                        IsInternationalWorker = _isInternationalWrkr ? AppointeeDetails.IsInternationalWorker?.ToString()?.ToUpper() : CheckType.no,
                        OriginCountry = _isPassportAvailable ? AppointeeDetails.OriginCountry : string.Empty,
                        PassportNo = _isPassportAvailable ? string.IsNullOrEmpty(AppointeeDetails.PassportNo) ? null : CommonUtility.CustomEncryptString(key, AppointeeDetails.PassportNo) : null,
                        PassportValidFrom = _isPassportAvailable ? AppointeeDetails.PassportValidFrom : null,
                        PassportValidTill = _isPassportAvailable ? AppointeeDetails.PassportValidTill : null,
                        Qualification = AppointeeDetails.Qualification?.ToString(),
                        MaratialStatus = AppointeeDetails.MaratialStatus?.ToString(),
                        MemberName = AppointeeDetails.MemberName,
                        MemberRelation = AppointeeDetails.MemberRelation?.ToString(),
                        MobileNo = AppointeeDetails.MobileNo,
                        Nationality = AppointeeDetails.Nationality,
                        IsPFverificationReq = AppointeeDetails?.IsPFverificationReq,
                        IsAadhaarVarified = AppointeeDetails?.IsAadhaarVarified,
                        IsUanVarified = AppointeeDetails?.IsUanVarified,
                        SaveStep = AppointeeDetails.IsSubmit ? 1 : 0,
                        lvl1Email = _appntundrprocessdata.lvl1Email,
                        lvl2Email = _appntundrprocessdata.lvl2Email,
                        lvl3Email = _appntundrprocessdata.lvl3Email,
                        IsSave = true,
                        ActiveStatus = true,
                        IsProcessed = false,
                        IsSubmit = false,
                        CreatedBy = AppointeeDetails.UserId,
                        CreatedOn = DateTime.Now
                    };

                    _dbContextClass.AppointeeDetails.Add(data);

                }
                else
                {
                    _appointeedetails.CandidateId = AppointeeDetails.CandidateId;
                    _appointeedetails.AppointeeName = AppointeeDetails.AppointeeName ?? string.Empty;
                    _appointeedetails.AppointeeEmailId = AppointeeDetails.AppointeeEmailId;
                    //_appointeedetails.PANNumber = AppointeeDetails.PANNumber;
                    //_appointeedetails.PANName = AppointeeDetails.PANName;
                    _appointeedetails.CompanyId = AppointeeDetails.CompanyId;
                    //_appointeedetails.CompanyName = AppointeeDetails.CompanyName;
                    _appointeedetails.DateOfBirth = AppointeeDetails.DateOfBirth;
                    _appointeedetails.DateOfJoining = AppointeeDetails.DateOfJoining;
                    _appointeedetails.Gender = AppointeeDetails.Gender;
                    _appointeedetails.EPFWages = AppointeeDetails.EPFWages;
                    _appointeedetails.IsHandicap = _isHandicap ? AppointeeDetails.IsHandicap?.ToString() : CheckType.no;
                    _appointeedetails.HandicapeType = _isHandicap ? AppointeeDetails.HandicapeType?.ToString() : string.Empty;
                    _appointeedetails.IsPassportAvailable = _isPassportAvailable ? AppointeeDetails.IsPassportAvailable?.ToString()?.ToUpper() : CheckType.no;
                    _appointeedetails.IsInternationalWorker = _isInternationalWrkr ? AppointeeDetails.IsInternationalWorker?.ToString()?.ToUpper() : CheckType.no;
                    _appointeedetails.OriginCountry = _isPassportAvailable ? AppointeeDetails.OriginCountry : string.Empty;
                    _appointeedetails.PassportNo = _isPassportAvailable ? string.IsNullOrEmpty(AppointeeDetails.PassportNo) ? null : CommonUtility.CustomEncryptString(key, AppointeeDetails.PassportNo) : null;
                    _appointeedetails.PassportValidFrom = _isPassportAvailable ? AppointeeDetails.PassportValidFrom : null;
                    _appointeedetails.PassportValidTill = _isPassportAvailable ? AppointeeDetails.PassportValidTill : null;
                    _appointeedetails.Qualification = AppointeeDetails.Qualification?.ToString();
                    _appointeedetails.MaratialStatus = AppointeeDetails.MaratialStatus?.ToString();
                    _appointeedetails.MemberName = AppointeeDetails.MemberName;
                    _appointeedetails.MemberRelation = AppointeeDetails.MemberRelation?.ToString();
                    _appointeedetails.MobileNo = AppointeeDetails.MobileNo;
                    _appointeedetails.Nationality = AppointeeDetails.Nationality;
                    _appointeedetails.IsPFverificationReq = AppointeeDetails.IsPFverificationReq ?? false;
                    _appointeedetails.SaveStep = AppointeeDetails.IsSubmit ? 1 : 0;
                    _appointeedetails.IsSave = true;
                    _appointeedetails.ActiveStatus = true;
                    _appointeedetails.IsProcessed = false;
                    _appointeedetails.IsSubmit = false;
                    _appointeedetails.UpdatedBy = AppointeeDetails.UserId;
                    _appointeedetails.UpdatedOn = DateTime.Now;
                }
                await _dbContextClass.SaveChangesAsync();
            }
        }
        public async Task CompanyAdminSaveAppointeeDetailsAsync(CompanySaveAppointeeDetailsRequest AppointeeDetails)
        {
            if (AppointeeDetails != null)
            {
                //if (AppointeeDetails?.Type == "UP")
                //{
                var appointeeId = AppointeeDetails.AppointeeId;
                if (appointeeId != 0)
                {
                    await saveUnderprocessAppointeedetailsByCompany(AppointeeDetails, appointeeId);
                    var _appointeedetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId) & x.IsProcessed.Equals(false)) ?? null;
                    if (_appointeedetails != null)
                    {
                        _appointeedetails.DateOfJoining = AppointeeDetails?.DateOfJoining ?? _appointeedetails.DateOfJoining;
                        //_appointeedetails.EPFWages = AppointeeDetails?.EPFWages ?? _appointeedetails.EPFWages;
                        _appointeedetails.UpdatedBy = AppointeeDetails?.UserId;
                        _appointeedetails.UpdatedOn = DateTime.Now;
                        await _dbContextClass.SaveChangesAsync();
                    }
                }
                else
                {
                    await saveNonprocessAppointeedetailsByCompany(AppointeeDetails, appointeeId);
                }
                await AppointeeDOJUpdateLog(AppointeeDetails);
            }
        }
        private async Task saveUnderprocessAppointeedetailsByCompany(CompanySaveAppointeeDetailsRequest AppointeeDetails, int appointeeId)
        {
            var _appntundrprocessdata = await _dbContextClass.UnderProcessFileData.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId)) ?? null;
            if (_appntundrprocessdata != null)
            {
                var _AppointeeUpdateHis = new AppointeeDetailsUpdateActivity
                {
                    AppointeeId = appointeeId,
                    Type = UpdateType.DOJ,
                    Value = _appntundrprocessdata.DateOfJoining?.ToShortDateString(),
                    ActiveStatus = true,
                    CreatedBy = AppointeeDetails?.UserId,
                    CreatedOn = DateTime.Now
                };
                _dbContextClass.AppointeeDetailsUpdateActivity.Add(_AppointeeUpdateHis);

                //_appntundrprocessdata.EPFWages = AppointeeDetails?.EPFWages ?? _appntundrprocessdata.EPFWages;
                _appntundrprocessdata.DateOfJoining = AppointeeDetails?.DateOfJoining ?? _appntundrprocessdata.DateOfJoining;
                _appntundrprocessdata.UpdatedBy = AppointeeDetails?.UserId;
                _appntundrprocessdata.UpdatedOn = DateTime.Now;

                await _dbContextClass.SaveChangesAsync();
            }
        }
        private async Task AppointeeDOJUpdateLog(CompanySaveAppointeeDetailsRequest AppointeeDetails)
        {
            var _AppointeeUpdateLog = new AppointeeUpdateLog
            {
                AppointeeId = AppointeeDetails.AppointeeId,
                CandidateId = AppointeeDetails.CandidateId,
                UpdateType = "DateOfJoining",
                UpdateValue = AppointeeDetails?.DateOfJoining?.ToShortDateString(),
                CreatedBy = AppointeeDetails.UserId,
                CreatedOn = DateTime.Now
            };
            _dbContextClass.AppointeeUpdateLog.Add(_AppointeeUpdateLog);
            await _dbContextClass.SaveChangesAsync();
        }
        private async Task saveNonprocessAppointeedetailsByCompany(CompanySaveAppointeeDetailsRequest AppointeeDetails, int Id)
        {
            var _appntNonprocessdata = await _dbContextClass.UnProcessedFileData.FirstOrDefaultAsync(x => x.UnProcessedId.Equals(Id)) ?? null;
            if (_appntNonprocessdata != null)
            {
                //  _appntundrprocessdata.EPFWages = AppointeeDetails?.EPFWages ?? _appntundrprocessdata.EPFWages;
                _appntNonprocessdata.DateOfJoining = AppointeeDetails?.DateOfJoining ?? _appntNonprocessdata.DateOfJoining;
                _appntNonprocessdata.UpdatedBy = AppointeeDetails?.UserId;
                _appntNonprocessdata.UpdatedOn = DateTime.Now;

                await _dbContextClass.SaveChangesAsync();
            }
        }
        public async Task PostAppointeeFileDetailsAsync(AppointeeFileDetailsRequest AppointeeFileDetails)
        {
            var key = _aadhaarConfig.EncriptKey;
            var appointeeDtlId = AppointeeFileDetails.AppointeeDetailsId;
            var appointeeId = AppointeeFileDetails.AppointeeId;
            var _appointeedetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId) & x.IsProcessed.Equals(false)) ?? null;
            var _isSubmit = _appointeedetails?.IsSubmit ?? false;
            if (_appointeedetails != null && _isSubmit != true)
            {
                await _fileContext.postappointeeUploadedFiles(AppointeeFileDetails);
                _appointeedetails.IsSubmit = AppointeeFileDetails.IsSubmit ?? false;
            }
            if (AppointeeFileDetails.IsSubmit ?? false && _isSubmit != true)
            {

                if ((_appointeedetails?.IsUanVarified ?? false) && (_appointeedetails.IsAadhaarVarified ?? false) && (_appointeedetails.IsPanVarified ?? false))
                {
                    _appointeedetails.IsProcessed = true;
                    await DataUploadAndApproved(_appointeedetails, AppointeeFileDetails?.UserId ?? 0, true);//isapprove set true

                }
            }
            await _dbContextClass.SaveChangesAsync();
        }
        public async Task<AppointeeDetails> UpdateAadharDetails(AadhaarValidateUpdateRequest AppointeeAadharDetails)
        {

            var appointeeId = AppointeeAadharDetails.AppointeeId;

            var _appointeedetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId) & x.IsProcessed.Equals(false)) ?? null;

            if (_appointeedetails != null)
            {
                _appointeedetails.AadhaarName = string.IsNullOrEmpty(AppointeeAadharDetails?.aadharData?.AadhaarName) ? null : AppointeeAadharDetails?.aadharData?.AadhaarName;
                _appointeedetails.AadhaarNumber = string.IsNullOrEmpty(AppointeeAadharDetails?.aadharData?.AadhaarNumber) ? null : CommonUtility.CustomEncryptString(key, AppointeeAadharDetails?.aadharData?.AadhaarNumber);
                _appointeedetails.AadhaarNumberView = string.IsNullOrEmpty(AppointeeAadharDetails?.aadharData?.AadhaarNumber) ? null : CommonUtility.MaskedString(AppointeeAadharDetails?.aadharData?.AadhaarNumber);

                await _dbContextClass.SaveChangesAsync();
            }
            return _appointeedetails;
        }
        public async Task<AppointeeDetailsResponse> GetAppointeeDetailsAsync(int appointeeId)
        {
            var key = _aadhaarConfig.EncriptKey;
            var _FileDataList = new List<GetFileDataModel>();
            var _appointeedetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId)) ?? null;
            var data = new AppointeeDetailsResponse();
            if (_appointeedetails == null)
            {
                var _appntundrprocessdata = await _dbContextClass.UnderProcessFileData.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId)) ?? null;
                if (_appntundrprocessdata != null)
                {
                    data.AppointeeDetailsId = _appointeedetails?.AppointeeDetailsId ?? 0;
                    data.AppointeeId = _appntundrprocessdata?.AppointeeId;
                    data.CandidateId = _appntundrprocessdata?.CandidateId;
                    data.CompanyName = _appntundrprocessdata?.CompanyName;
                    data.AppointeeName = _appntundrprocessdata?.AppointeeName;
                    data.AppointeeEmailId = _appntundrprocessdata?.AppointeeEmailId;
                    data.CompanyId = _appntundrprocessdata?.CompanyId;
                    data.DateOfJoining = _appntundrprocessdata?.DateOfJoining;
                    data.EPFWages = _appntundrprocessdata?.EPFWages ?? 0;
                    data.MobileNo = _appntundrprocessdata?.MobileNo;
                    data.IsPFverificationReq = _appntundrprocessdata?.IsPFverificationReq;
                    //data.IsPFverificationReq = _appntundrprocessdata?.IsPFverificationReq;
                    data.SaveStep = 0;
                    data.IsPassportValid = null;
                    data.isPanVarified = null;
                    data.IsUanVarified = null;
                    data.IsAadhaarVarified = null;
                    data.IsProcessed = false;
                    data.IsSubmit = false;
                    data.FileUploaded = _FileDataList;
                }
            }
            else
            {
                // var createdDate = _appointeedetails?.CreatedOn.Value.ToShortDateString() ?? "TEST";
                data.AppointeeDetailsId = _appointeedetails?.AppointeeDetailsId ?? 0;
                data.AppointeeId = _appointeedetails?.AppointeeId ?? appointeeId;
                data.CandidateId = _appointeedetails?.CandidateId;
                data.CompanyName = _appointeedetails?.CompanyName;
                data.AppointeeName = _appointeedetails?.AppointeeName;
                data.AppointeeEmailId = _appointeedetails?.AppointeeEmailId;
                data.AadhaarName = string.IsNullOrEmpty(_appointeedetails?.AadhaarName) ? null : _appointeedetails.AadhaarName;
                data.AadhaarNumber = string.IsNullOrEmpty(_appointeedetails?.AadhaarNumber) ? null : CommonUtility.DecryptString(key, _appointeedetails.AadhaarNumber);
                data.AadhaarNumberView = string.IsNullOrEmpty(_appointeedetails?.AadhaarNumber) ? null : _appointeedetails.AadhaarNumberView;
                data.NameFromAadhaar = _appointeedetails?.NameFromAadhaar;
                data.DobFromAadhaar = _appointeedetails?.DobFromAadhaar;
                data.PANNumber = string.IsNullOrEmpty(_appointeedetails?.PANNumber) ? null : CommonUtility.MaskedString(CommonUtility.DecryptString(key, _appointeedetails?.PANNumber));
                data.PANName = _appointeedetails?.PANName;
                data.FathersNameFromPan = _appointeedetails?.FathersNameFromPan;
                data.CompanyId = _appointeedetails?.CompanyId;
                data.DateOfBirth = _appointeedetails?.DateOfBirth;
                data.UANNumber = string.IsNullOrEmpty(_appointeedetails?.UANNumber) ? null : CommonUtility.MaskedString(CommonUtility.DecryptString(key, _appointeedetails?.UANNumber));
                data.DateOfJoining = _appointeedetails?.DateOfJoining;
                data.Gender = _appointeedetails?.Gender;
                data.EPFWages = _appointeedetails?.EPFWages ?? 0;
                data.IsHandicap = _appointeedetails?.IsHandicap?.ToUpper();
                data.HandicapeType = _appointeedetails?.HandicapeType?.ToUpper();
                data.isPassportAvailable = _appointeedetails?.IsPassportAvailable?.ToUpper();
                data.IsInternationalWorker = _appointeedetails?.IsInternationalWorker?.ToUpper();
                data.OriginCountry = _appointeedetails?.OriginCountry;
                data.PassportNo = string.IsNullOrEmpty(_appointeedetails?.PassportNo) ? null : CommonUtility.MaskedString(CommonUtility.DecryptString(key, _appointeedetails?.PassportNo));
                data.PassportFileNo = _appointeedetails?.PassportFileNo;
                data.PassportValidFrom = _appointeedetails?.PassportValidFrom;
                data.PassportValidTill = _appointeedetails?.PassportValidTill;
                data.Qualification = _appointeedetails?.Qualification;
                data.MaratialStatus = _appointeedetails?.MaratialStatus;
                data.MemberName = _appointeedetails?.MemberName;
                data.MemberRelation = _appointeedetails?.MemberRelation;
                data.MobileNo = _appointeedetails?.MobileNo;
                data.Nationality = _appointeedetails?.Nationality;
                data.IsPassportValid = _appointeedetails?.IsPasssportVarified;
                data.isPanVarified = _appointeedetails?.IsPanVarified;
                data.IsPensionApplicable = _appointeedetails?.IsPensionApplicable;
                data.IsPFverificationReq = _appointeedetails?.IsPFverificationReq;
                data.IsAadhaarVarified = _appointeedetails?.IsAadhaarVarified;
                data.IsUanVarified = _appointeedetails?.IsUanVarified;
                data.IsTrustPassbook = _appointeedetails?.IsTrustPassbook;
                data.IsProcessed = _appointeedetails?.IsProcessed;
                data.SaveStep = _appointeedetails?.SaveStep ?? 0;
                data.IsSubmit = _appointeedetails?.IsSubmit ?? false;
                data.UserId = _appointeedetails?.CreatedBy ?? 0;
                //GetFileDataModel
                var _paddedName = _appointeedetails?.AppointeeName?.Length > 4 ? _appointeedetails.AppointeeName?.Substring(0, 3) : _appointeedetails?.AppointeeName?.PadRight(3, '0');
                var candidateFileName = $"{_appointeedetails?.CandidateId}_{_paddedName}";
                await getFiledetailsByAppointeeId(appointeeId, candidateFileName, _FileDataList);
                data.FileUploaded = _FileDataList;

            }
            return data;
        }
        private async Task getFiledetailsByAppointeeId(int appointeeId, string candidateFileName, List<GetFileDataModel> _FileDataList)
        {
            var _UploadDetails = await _dbContextClass.AppointeeUploadDetails.Where(x => x.AppointeeId.Equals(appointeeId) & x.ActiveStatus == true).ToListAsync();

            if (_UploadDetails.Any())
            {
                foreach (var (obj, doc) in from obj in _UploadDetails
                                           let doc = new GetFileDataModel()
                                           select (obj, doc))
                {
                    var _FileData = await _fileContext.GetFileDataAsync(obj.UploadPath);
                    doc.FileData = _FileData;
                    string _fileName = $"{candidateFileName}_{obj?.UploadTypeCode}";
                    doc.FileName = _fileName;
                    doc.UploadTypeId = obj?.UploadTypeId ?? 0;
                    doc.mimeType = obj?.MimeType ?? string.Empty;
                    doc.UploadTypeAlias = obj?.UploadTypeCode ?? string.Empty;
                    _FileDataList.Add(doc);

                }
            }
        }
        public async Task<List<RawFileData>> GetRawfiledataAsync(int? fileId, int companyId)
        {
            var _companydetails = new List<RawFileData>();
            if (fileId == 0)
            {
                _companydetails = await _dbContextClass.RawFileData.Where(x => x.ActiveStatus == true & x.CompanyId.Equals(companyId)).ToListAsync();

            }
            else
            {
                _companydetails = await _dbContextClass.RawFileData.Where(x => x.ActiveStatus == true & x.CompanyId.Equals(companyId) & x.FileId.Equals(fileId)).ToListAsync();
            }
            return _companydetails;
        }
        public async Task<List<RawFileDataModel>> GetRawfiledataById(List<RawDataRequest> rawDataList, int? userId, int type)
        {
            var _rawData = new List<RawFileDataModel>();

            if (type == 1)//type 1 for checked
            {
                var CheckedRawIdList = rawDataList?.Where(x => x.isChecked == true)?.Select(y => y.id).ToList();
                if (CheckedRawIdList?.Count > 0)
                {
                    var _CheckedRawData = await _dbContextClass.RawFileData.Where(x => x.ActiveStatus == true && CheckedRawIdList.Contains(x.RawFileId)).Select(x => new RawFileDataModel
                    {
                        companyId = x.CompanyId,
                        id = x.RawFileId,
                        fileId = x.FileId,
                        companyName = x.CompanyName,
                        CandidateId = x.CandidateId,
                        appointeeName = x.AppointeeName,
                        appointeeEmailId = x.AppointeeEmailId,
                        mobileNo = x.MobileNo,
                        dateOfJoining = x.DateOfJoining,
                        isPFverificationReq = x.IsPFverificationReq,
                        dateOfOffer = x.DateOfOffer,
                        epfWages = x.EPFWages,
                        lvl1Email = x.lvl1Email,
                        lvl2Email = x.lvl2Email,
                        lvl3Email = x.lvl3Email,
                        isChecked = true,
                        userId = userId
                    }).ToListAsync();
                    _rawData.AddRange(_CheckedRawData);
                }
            }
            if (type == 2)//type 2 for UnChecked
            {
                var UnCheckedRawIdList = rawDataList?.Where(x => x.isChecked == false)?.Select(y => y.id).ToList();
                if (UnCheckedRawIdList?.Count > 0)
                {
                    var _UnCheckedRawData = await _dbContextClass.RawFileData.Where(x => x.ActiveStatus == true && UnCheckedRawIdList.Contains(x.RawFileId)).Select(x => new RawFileDataModel
                    {
                        companyId = x.CompanyId,
                        id = x.RawFileId,
                        fileId = x.FileId,
                        companyName = x.CompanyName,
                        CandidateId = x.CandidateId,
                        appointeeName = x.AppointeeName,
                        appointeeEmailId = x.AppointeeEmailId,
                        mobileNo = x.MobileNo,
                        dateOfJoining = x.DateOfJoining,
                        isPFverificationReq = x.IsPFverificationReq,
                        dateOfOffer = x.DateOfOffer,
                        epfWages = x.EPFWages,
                        lvl1Email = x.lvl1Email,
                        lvl2Email = x.lvl2Email,
                        lvl3Email = x.lvl3Email,
                        isChecked = false,
                        userId = userId
                    }).ToListAsync();
                    _rawData.AddRange(_UnCheckedRawData);
                }
            }
            return _rawData;
        }
        public async Task PostRawfiledataAsync(RawdataSubmitRequest data)
        {
            var _userId = data?.UserId;
            var _companyId = data?.CompanyId;
            var _fileId = data?.FileId;

            var _rawfileDatalist = data?.ApnteFileData?.Select(x => new RawFileData
            {
                CandidateId = x.CandidateID,
                CompanyName = x.CompanyName,
                AppointeeName = x.AppointeeName,
                AppointeeEmailId = x.AppointeeEmailId,
                //EPFWages = x.EPFWages,
                MobileNo = x.MobileNo,
                IsPFverificationReq = x.IsFresher?.ToUpper() == "NO",
                //DateOfOffer = null,
                DateOfJoining = Convert.ToDateTime(x.DateOfJoining),
                lvl1Email = x.lvl1Email,
                lvl2Email = x.lvl2Email,
                lvl3Email = x.lvl3Email,
                CreatedBy = _userId,
                ActiveStatus = true,
                CreatedOn = DateTime.Now,
                CompanyId = _companyId ?? 0,
                FileId = _fileId ?? 0
            }).ToList();
            await _dbContextClass.RawFileData.AddRangeAsync(_rawfileDatalist);

            var _rawfileHistDatalist = data?.ApnteFileData?.Select(x => new RawFileHistoryData
            {
                CandidateId = x.CandidateID,
                CompanyName = x.CompanyName,
                AppointeeName = x.AppointeeName,
                AppointeeEmailId = x.AppointeeEmailId,
                MobileNo = x.MobileNo,
                IsPFverificationReq = x.IsFresher?.ToUpper() == "NO",
                DateOfJoining = Convert.ToDateTime(x.DateOfJoining),
                lvl1Email = x.lvl1Email,
                lvl2Email = x.lvl2Email,
                lvl3Email = x.lvl3Email,
                CreatedBy = _userId,
                ActiveStatus = true,
                CreatedOn = DateTime.Now,
                CompanyId = _companyId ?? 0,
                FileId = _fileId ?? 0
            }).ToList();
            await _dbContextClass.RawFileHistoryData.AddRangeAsync(_rawfileHistDatalist);

            var _totalrawfileData = new UploadAppointeeCounter
            {
                Count = _rawfileDatalist?.Count() ?? 0,
                CreatedBy = _userId,
                CreatedOn = DateTime.Now,
                FileId = _fileId ?? 0
            };
            await _dbContextClass.UploadAppointeeCounter.AddAsync(_totalrawfileData);

            await _dbContextClass.SaveChangesAsync();
        }
        public async Task<CandidateValidateResponse> UpdateIsValidAadhaarORUan(AadhaarValidateUpdateRequest validationReq)
        {
            var key = _aadhaarConfig.EncriptKey;
            var Response = new CandidateValidateResponse();
            string Remarks = string.Empty;
            var _appointeedetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(validationReq.AppointeeId)) ?? null;

            if (_appointeedetails != null)
            {
                _appointeedetails.IsPensionApplicable = validationReq?.IsPensionApplicable;
                if (validationReq.Type == RemarksType.Adhaar)
                {
                    _appointeedetails.IsAadhaarVarified = validationReq.Status;
                    _appointeedetails.AadhaarName = validationReq?.aadharData?.AadhaarName ?? string.Empty;
                    _appointeedetails.AadhaarNumber = CommonUtility.CustomEncryptString(key, validationReq?.aadharData?.AadhaarNumber) ?? string.Empty;
                    _appointeedetails.NameFromAadhaar = validationReq?.aadharData?.NameFromAadhaar;
                    _appointeedetails.GenderFromAadhaar = validationReq?.aadharData?.GenderFromAadhaar;
                    _appointeedetails.DobFromAadhaar = validationReq?.aadharData?.DobFromAadhaar;

                }
                if (validationReq.Type == RemarksType.UAN)
                {
                    _appointeedetails.IsUanVarified = validationReq.Status;
                    _appointeedetails.UANNumber = !string.IsNullOrEmpty(validationReq?.UanNumber) ? CommonUtility.CustomEncryptString(key, validationReq?.UanNumber) : null;
                    // await _dbContextClass.SaveChangesAsync();
                }
                if (validationReq.Type == RemarksType.Passport)
                {
                    _appointeedetails.IsPasssportVarified = validationReq.Status;
                    _appointeedetails.PassportFileNo = validationReq.PassportFileNo;
                    // _appointeedetails.UANNumber = validationReq.Status ? validationReq?.UanNumber : null;
                    // await _dbContextClass.SaveChangesAsync();
                }
                if (validationReq.Type == RemarksType.Pan)
                {
                    _appointeedetails.IsPanVarified = validationReq.Status;
                    _appointeedetails.PANNumber = !string.IsNullOrEmpty(validationReq?.panData?.PanNumber) ? CommonUtility.CustomEncryptString(key, validationReq?.panData?.PanNumber) : null;
                    _appointeedetails.PANName = validationReq?.panData?.PanName;
                    _appointeedetails.FathersNameFromPan = validationReq?.panData?.PanFatherName;
                    // _appointeedetails.UANNumber = validationReq.Status ? validationReq?.UanNumber : null;
                    // await _dbContextClass.SaveChangesAsync();
                }
                


            }
            if (validationReq?.Reasons?.Count > 0)
            {
                Remarks = await UpdateRemarksByType(validationReq.AppointeeId, validationReq.Reasons, validationReq?.Type ?? "", validationReq?.UserId ?? 0);
                if (!validationReq?.Status ?? false)
                {
                    string mailtype = GetMailType(validationReq.Type);
                    if (!string.IsNullOrEmpty(validationReq?.EmailId))
                    {
                        MailDetails mailDetails = new MailDetails();
                        MailBodyParseDataDetails bodyDetails = new MailBodyParseDataDetails
                        {
                            Name = validationReq?.UserName,
                            Reason = Remarks,
                        };
                        mailDetails.MailType = mailtype;
                        mailDetails.ParseData = bodyDetails;
                        Task.Run(async () => await _emailSender.SendAppointeeMail(validationReq?.EmailId, mailDetails)).GetAwaiter().GetResult();
                    }
                }
            }
            await _dbContextClass.SaveChangesAsync();

            Response.IsValid = validationReq.Status ?? false;
            Response.Remarks = Remarks;

            return Response;
        }
        private async Task UpdateUserStatus(int AppointeeId, int UserId)
        {
            var _userdata = await _dbContextClass.UserMaster.FirstOrDefaultAsync(x => x.RefAppointeeId == AppointeeId & x.ActiveStatus == true);
            if (_userdata != null)
            {
                //_userdata.CurrStatus = false;
                _userdata.UpdatedOn = DateTime.Now;
                _userdata.UpdatedBy = UserId;

            }
        }
        private async Task DataUploadAndApproved(AppointeeDetails? appointeedetails, int userId, bool IsApproved)
        {
            var UploadDetailsId = await GetWorkFlowStateId(WorkFlowType.UploadDetails);

            var _WorkFlowDataRequest = new WorkFlowDataRequest
            {
                appointeeId = appointeedetails?.AppointeeId ?? 0,
                workflowState = UploadDetailsId,
                approvalStatus = string.Empty,
                Remarks = string.Empty,
                userId = userId

            };
            await Appointee_Workflow_Update_Async(_WorkFlowDataRequest);
            if (IsApproved)
            {
                var _stateId = await GetWorkFlowStateId(WorkFlowType.DataVarified);

                var _WorkFlowApproveDataRequest = new WorkFlowDataRequest
                {
                    appointeeId = appointeedetails?.AppointeeId ?? 0,
                    workflowState = _stateId,
                    approvalStatus = WorkFlowType.Approved,
                    Remarks = string.Empty,
                    userId = userId
                };
                await Appointee_Workflow_Update_Async(_WorkFlowApproveDataRequest);
            }
        }
        private static string GetMailType(string Type)
        {
            string mailtype = string.Empty;
            if (!string.IsNullOrEmpty(Type))
            {
                switch (Type)
                {
                    case RemarksType.Adhaar:
                        mailtype = MailType.AdhrValidation;
                        break;
                    case RemarksType.UAN:
                        mailtype = MailType.UANValidation;
                        break;
                    case RemarksType.Passport:
                        mailtype = MailType.Passport;
                        break;
                    case RemarksType.Pan:
                        mailtype = MailType.Pan;
                        break;
                    case RemarksType.Others:
                        mailtype = MailType.Others;
                        break;
                    default:
                        mailtype = string.Empty;
                        break;

                }
            }

            return mailtype;
        }
        public async Task<string> UpdateRemarksByType(int AppointeeId, List<ReasonRemarks?> Reasons, string Type, int UserId)
        {
            string AllRemarks = string.Empty;
            // var ReasonCodeList = Reasons?.Select(x => x.ReasonCode)?.ToList();

            var AllResonDetails = await _dbContextClass.ReasonMaser.Where(x => x.ReasonType.Equals(Type) && x.ActiveStatus == true).ToListAsync();
            var AllPrevReason = await _dbContextClass.AppointeeReasonMappingData.Where(x => x.AppointeeId.Equals(AppointeeId) && x.ActiveStatus == true).ToListAsync();
            var reasonListquery = from rm in AllResonDetails
                                  join r in Reasons
                                  on rm.ReasonCode equals r.ReasonCode
                                  select new { rm.ReasonName, rm.ReasonCode, rm.ReasonId, r.Inputdata, r.Fetcheddata, r.Remarks };
            var ResonDetails = reasonListquery.ToList();
            // var ResonDetails = AllResonDetails.Where(x => ReasonCodeList.Contains(x.ReasonCode)).ToList();
            if (ResonDetails?.Count > 0)
            {

                //var CurrReasonIdList = ResonDetails.Select(x => x.ReasonId).ToList();
                var AllReasonIdList = AllResonDetails.Select(x => x.ReasonId).ToList();
                var PrevReason = AllPrevReason.Where(x => AllReasonIdList.Contains(x.ReasonId)).ToList();
                var _resaonList = ResonDetails?.Select(x => new AppointeeReasonMappingData
                {
                    AppointeeId = AppointeeId,
                    ReasonId = x.ReasonId,
                    Remarks = x.ReasonCode != ReasonCode.OTHER ? CommonUtility.ParseMessage(x.ReasonName, new { x.Inputdata, x.Fetcheddata }) : x.Remarks,
                    CreatedBy = UserId,
                    ActiveStatus = true,
                    CreatedOn = DateTime.Now,
                }).ToList();
                await _dbContextClass.AppointeeReasonMappingData.AddRangeAsync(_resaonList);

                if (PrevReason.Count > 0)
                {
                    PrevReason.ForEach(x => x.ActiveStatus = false);
                    //await _dbContextClass.AppointeeReasonMappingData.AddRangeAsync(_resaonList);
                }
                if (_resaonList.Count > 0)
                {
                    AllRemarks = string.Join(", ", _resaonList.Select(x => x.Remarks).ToList());
                }
                //await _dbContextClass.SaveChangesAsync();
            }
            else
            {
                if (AllPrevReason.Count > 0)
                {
                    var allreasonIdByType = AllResonDetails.Select(x => x.ReasonId).ToList();
                    AllPrevReason.Where(x => allreasonIdByType.Contains(x.ReasonId)).ToList().ForEach(x => x.ActiveStatus = false);
                    //await _dbContextClass.AppointeeReasonMappingData.AddRangeAsync(_resaonList);
                }
            }

            return AllRemarks;
        }
        public async Task UpdateIsPfVarification(int AppointeeId, bool isRequired)
        {
            var Response = new AadhaarValidateUpdateResponse();
            string Remarks = string.Empty;
            var appointeedetail = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(AppointeeId)) ?? null;
            if (appointeedetail != null)
            {
                appointeedetail.IsPFverificationReq = isRequired;
                await _dbContextClass.SaveChangesAsync();

            }
        }
        public async Task<List<GetRemarksResponse?>> GetRemarks(int appointeeId)
        {
            var querydata = from r in _dbContextClass.ReasonMaser
                            join a in _dbContextClass.AppointeeReasonMappingData
                                on r.ReasonId equals a.ReasonId
                            where r.ActiveStatus == true & a.AppointeeId == appointeeId & a.ActiveStatus == true
                            select new { a.Remarks, r.ReasonId, r.ReasonCode, r.ReasonCategory };

            var list = await querydata.ToListAsync().ConfigureAwait(false);
            var remarks = list.Select(x => new GetRemarksResponse
            {
                RemarksId = x.ReasonId,
                RemarksCode = x.ReasonCode,
                RemarksCategory = x.ReasonCategory,
                Remarks = x.Remarks
            }).ToList();

            return remarks;
        }
        public async Task<string?> GetRemarksRemedy(int RemarksId)
        {
            string remedyhtml = await _dbContextClass.ReasonMaser?.Where(x => x.ReasonId == RemarksId)?.Select(y => y.ReasonRemedy).FirstOrDefaultAsync();
            return remedyhtml;
        }
        public async Task ModifyAadhaarValidatedField(ModifyAadhaarValidatedFieldRequest Request)
        {
            if (Request?.AppointeeId != null)
            {
                var appointeeId = Request.AppointeeId;

                var _appointeedetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId) & x.IsProcessed.Equals(false)) ?? null;

                if (_appointeedetails != null)
                {
                    _appointeedetails.AppointeeName = string.IsNullOrEmpty(Request.Name) ? _appointeedetails.AppointeeName : Request.Name;
                    _appointeedetails.DateOfBirth = Request.DateOfBirth ?? _appointeedetails.DateOfBirth;
                    _appointeedetails.Gender = string.IsNullOrEmpty(Request.Gender) ? _appointeedetails.Gender : Request.Gender;
                    _appointeedetails.MemberName = string.IsNullOrEmpty(Request.FathersName) ? _appointeedetails.MemberName : Request.FathersName;
                    //_appointeedetails.UpdatedBy = AppointeeDetails.UserId;
                    //_appointeedetails.UpdatedOn = DateTime.Now;
                }
                await _dbContextClass.SaveChangesAsync();
            }
        }
        private async Task UpdateAadhaar(AadhaarValidateUpdateRequest validationReq)
        {
            var key = _aadhaarConfig.EncriptKey;
            var Response = new AadhaarValidateUpdateResponse();
            string Remarks = string.Empty;
            var appointeedetail = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(validationReq.AppointeeId)) ?? null;
            //var appointeedetail = Task.Run(async () => await _workflowcontext.GetAppointeeDetailsAsync(appointeeId)).GetAwaiter().GetResult();
            if (appointeedetail != null)
            {
                if (validationReq.Type == RemarksType.Adhaar)
                {
                    appointeedetail.AadhaarName = validationReq?.aadharData?.AadhaarName ?? null;
                    appointeedetail.AadhaarNumber = string.IsNullOrEmpty(validationReq?.aadharData?.AadhaarNumber) ? null : CommonUtility.CustomEncryptString(key, validationReq?.aadharData?.AadhaarNumber);

                }

            }
        }
        public async Task<List<CriticalAppointeeResponse>> GetCriticalAppointeeList(CriticalFilterData reqObj)
        {
            //  var filterdaysrange = _configSetup.CriticalDaysLimit;
            var generalsetupData = await _dbContextClass.GeneralSetup.Where(x => x.ActiveStatus == true).ToListAsync();
            var filterdaysrange = generalsetupData.FirstOrDefault()?.CriticalNoOfDays ?? 0;
            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            var maxDate = _currDate.AddDays(filterdaysrange);
            var actionRequiredListdata = new List<CriticalAppointeeResponse>();
            var _getapprovalStatus = await _dbContextClass.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToArrayAsync();
            var ApprovedState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Approved?.Trim());
            var ForcedApprovedState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ForcedApproved?.Trim());
            var CloseState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ProcessClose?.Trim());
            var RejectedState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Rejected?.Trim());

            var querydata = from b in _dbContextClass.UnderProcessFileData
                            join w in _dbContextClass.WorkFlowDetails
                            on b.AppointeeId equals w.AppointeeId
                            join p in _dbContextClass.AppointeeDetails
                            on b.AppointeeId equals p.AppointeeId into grouping
                            from p in grouping //.Where(x => !x.ProcessStatus.GetValueOrDefault().Equals(ReprocessState.AppvlStatusId)).DefaultIfEmpty()
                            where w.AppvlStatusId != CloseState.AppvlStatusId
                            & w.AppvlStatusId != RejectedState.AppvlStatusId
                            & w.AppvlStatusId != ApprovedState.AppvlStatusId
                            & w.AppvlStatusId != ForcedApprovedState.AppvlStatusId
                            & b.DateOfJoining <= maxDate & b.DateOfJoining >= _currDate & b.ActiveStatus == true & p.ActiveStatus == true
                            & (reqObj.FromDate == null || b.CreatedOn >= reqObj.FromDate)
                            & (reqObj.ToDate == null || b.CreatedOn <= reqObj.ToDate)
                            orderby p.IsSubmit
                            select new { b, p, w.AppvlStatusId };

            var UnderProcessData = await querydata.OrderByDescending(x => x.p.DateOfJoining).ToListAsync().ConfigureAwait(false);
            //  var UnderProcessData = await _dbContextClass.UnderProcessFileData.Where(m => m.DateOfJoining <= maxDate && m.ActiveStatus == true).ToListAsync();
            var NonProcessData = await _dbContextClass.UnProcessedFileData.Where(m => m.DateOfJoining <= maxDate && m.DateOfJoining >= _currDate
            && m.ActiveStatus == true && (reqObj.FromDate == null || m.CreatedOn >= reqObj.FromDate) && (reqObj.ToDate == null || m.CreatedOn <= reqObj.ToDate)).ToListAsync();
            var _underProcessViewdata = UnderProcessData.Select(row => new CriticalAppointeeResponse
            {

                id = row.b.UnderProcessId,
                fileId = row.b.FileId,
                companyId = row.b.CompanyId,
                candidateId = row.p?.CandidateId ?? row.b.CandidateId,
                appointeeName = row.p?.AppointeeName ?? row.b.AppointeeName,
                appointeeId = row.b.AppointeeId,
                appointeeEmailId = row.p?.AppointeeEmailId ?? row.b.AppointeeEmailId,
                mobileNo = row.b.MobileNo,
                dateOfJoining = row.p?.DateOfJoining ?? row.b.DateOfJoining,
                Status = row.p?.IsSubmit ?? false ? "Ongoing" : row.p?.SaveStep == 1 ? "Ongoing" : "No Response",
                StatusCode = row.p?.IsSubmit ?? false ? 2 : row.p?.SaveStep == 1 ? 2 : 1,
                DaysToJoin = Convert.ToInt32(((row.p?.DateOfJoining ?? row.b.DateOfJoining) - DateTime.Now)?.TotalDays ?? 0),
                CreatedDate = row.b?.CreatedOn
            })?.ToList();
            var _unProcessViewdata = NonProcessData.Select(row => new CriticalAppointeeResponse
            {
                id = row.UnProcessedId,
                fileId = row.FileId,
                companyId = row.CompanyId,
                candidateId = row.CandidateId,
                appointeeName = row.AppointeeName,
                appointeeId = 0,
                appointeeEmailId = row.AppointeeEmailId,
                mobileNo = row.MobileNo,
                dateOfJoining = row.DateOfJoining,
                Status = "Mail Not sent",
                StatusCode = 0,
                DaysToJoin = Convert.ToInt32((row.DateOfJoining - DateTime.Now)?.TotalDays ?? 0),
                CreatedDate = row.CreatedOn
            })?.ToList();

            if (_underProcessViewdata?.Count() > 0)
            {
                actionRequiredListdata.AddRange(_underProcessViewdata);
            }
            if (_unProcessViewdata?.Count() > 0)
            {
                actionRequiredListdata.AddRange(_unProcessViewdata);
            }
            //criticaldata = UnderProcessData?.Count ?? 0 + NonProcessData?.Count ?? 0;
            return actionRequiredListdata;
        }
        public async Task<string> GetAppointeeActivityList(int appointeId)
        {
            var GetUser = await _dbContextClass.UserMaster.Where(x => x.RefAppointeeId == appointeId).FirstOrDefaultAsync();
            var GetLogindata = await _dbContextClass.UserAuthenticationHist.Where(m => m.UserId.Equals(GetUser.UserId) && m.ActiveStatus == true).ToListAsync();
            return "";
        }
        public async Task<List<GetAppointeeGlobalSearchResponse>> GetAppointeeSearchGlobal(string Name)
        {
            var searchedDataRes = new List<GetAppointeeGlobalSearchResponse>();
            var generalsetupData = await _dbContextClass.GeneralSetup.Where(x => x.ActiveStatus == true).ToListAsync();
            var filterdaysrange = generalsetupData.FirstOrDefault()?.CriticalNoOfDays ?? 0;
            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            var maxDate = _currDate.AddDays(filterdaysrange);
            var _getapprovalStatus = await _dbContextClass.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToArrayAsync();
            var closeState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ProcessClose?.Trim());
            var verifiedState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Approved?.Trim());
            var forcedVerifiedState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ForcedApproved?.Trim());
            var rejectedState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Rejected?.Trim());

            var menuDataList = await _dbContextClass.MenuMaster.Where(m => m.ActiveStatus == true).ToListAsync();
            var verifiedMenu = menuDataList.FirstOrDefault(x => x.MenuAlias == MenuCode.VERIFIED);
            var rejectedMenu = menuDataList.FirstOrDefault(x => x.MenuAlias == MenuCode.REJECTED);
            var processingMenu = menuDataList.FirstOrDefault(x => x.MenuAlias == MenuCode.PROCESSING);
            var lapsedMenu = menuDataList.FirstOrDefault(x => x.MenuAlias == MenuCode.EXPIRED);
            //var criticalMenu = menuDataList.FirstOrDefault(x => x.MenuAlias == MenuCode.CRITICAL);
            var linkntsendMenu = menuDataList.FirstOrDefault(x => x.MenuAlias == MenuCode.LINKNOTSENT);
            var uploadedDataMenu = menuDataList.FirstOrDefault(x => x.MenuAlias == MenuCode.UPLOADEDDATA);


            //var appointeeData = await _dbContextClass.AppointeeMaster.Where(m => m.AppointeeName.Contains(Name) && m.ActiveStatus == true).ToListAsync();

            var querydata = from b in _dbContextClass.UnderProcessFileData.Where(m => m.AppointeeName.ToLower().Contains(Name.ToLower()) && m.ActiveStatus == true)
                            join w in _dbContextClass.WorkFlowDetails
                            on b.AppointeeId equals w.AppointeeId
                            where (w.AppvlStatusId != closeState.AppvlStatusId)
                            & b.ActiveStatus == true
                            orderby b.AppointeeId
                            select new { b.AppointeeId, b.AppointeeName, b.CandidateId, b.DateOfJoining, w.AppvlStatusId };

            var appointeelist = await querydata.ToListAsync().ConfigureAwait(false);



            var _allProcessedData = appointeelist.DistinctBy(x => x.AppointeeId).Where(x => x.AppvlStatusId == verifiedState.AppvlStatusId || x.AppvlStatusId == rejectedState.AppvlStatusId || x.AppvlStatusId == forcedVerifiedState.AppvlStatusId)
                .Select(obj => new GetAppointeeGlobalSearchResponse
                {
                    AppointeeName = obj.AppointeeName,
                    CandidateId = obj.CandidateId,
                    AppointeePath = (obj.AppvlStatusId == verifiedState.AppvlStatusId || obj.AppvlStatusId == forcedVerifiedState.AppvlStatusId) ? verifiedMenu.menu_action : obj.AppvlStatusId == rejectedState.AppvlStatusId ? rejectedMenu.menu_action : "",
                    PathName = (obj.AppvlStatusId == verifiedState.AppvlStatusId || obj.AppvlStatusId == forcedVerifiedState.AppvlStatusId) ? verifiedMenu.MenuTitle : obj.AppvlStatusId == rejectedState.AppvlStatusId ? rejectedMenu.MenuTitle : "",

                }).ToList();

            if (_allProcessedData.Count > 0)
            {
                searchedDataRes.AddRange(_allProcessedData);
            }

            var _allUnderProcessedData = appointeelist.DistinctBy(x => x.AppointeeId).Where(x => !(x.AppvlStatusId == verifiedState.AppvlStatusId || x.AppvlStatusId == rejectedState.AppvlStatusId || x.AppvlStatusId == forcedVerifiedState.AppvlStatusId))
                .Select(obj => new GetAppointeeGlobalSearchResponse
                {
                    AppointeeName = obj.AppointeeName,
                    CandidateId = obj.CandidateId,
                    AppointeePath = (obj.DateOfJoining < _currDate) ? lapsedMenu.menu_action : processingMenu.menu_action,
                    PathName = (obj.DateOfJoining < _currDate) ? lapsedMenu.MenuTitle : processingMenu.MenuTitle,
                }).ToList();
            if (_allUnderProcessedData.Count > 0)
            {
                searchedDataRes.AddRange(_allUnderProcessedData);
            }
            //var _allProcessedData = appointeelist.Where(x => x.IsProcessed == true).ToList();

            var linknotsendData = await _dbContextClass.UnProcessedFileData.Where(m => m.AppointeeName.ToLower().Contains(Name.ToLower()) && m.ActiveStatus == true).ToListAsync();

            foreach (var obj in linknotsendData)
            {
                var res = new GetAppointeeGlobalSearchResponse();
                res.AppointeeName = obj.AppointeeName;
                res.CandidateId = obj.CandidateId;
                res.AppointeePath = linkntsendMenu.menu_action;
                res.PathName = linkntsendMenu.MenuTitle;
                searchedDataRes.Add(res);
            }
            var uploadedData = await _dbContextClass.RawFileData.Where(m => m.AppointeeName.ToLower().Contains(Name.ToLower()) && m.ActiveStatus == true).ToListAsync();

            foreach (var obj in uploadedData)
            {
                var res = new GetAppointeeGlobalSearchResponse();
                res.AppointeeName = obj.AppointeeName;
                res.CandidateId = obj.CandidateId;
                res.AppointeePath = uploadedDataMenu.menu_action;
                res.PathName = uploadedDataMenu.MenuTitle;
                searchedDataRes.Add(res);
            }

            return searchedDataRes;
        }

        public async Task<List<DropDownDetails>> GetAllReportFilterStatus()
        {
            List<DropDownDetails> dataList = new List<DropDownDetails>();
            var _getapprovalStatus = await _dbContextClass.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToArrayAsync();
            var processIni = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode.Equals(WorkFlowType.ProcessIni));
            var resultSet = _getapprovalStatus?.Where(y => y.AppvlStatusCode != WorkFlowType.ProcessIni).Select(x => new DropDownDetails
            {
                Id = x.AppvlStatusId,
                Code = x.AppvlStatusCode,
                Value = x.AppvlStatusDesc
            })?.ToList();
            dataList.AddRange(resultSet);
            DropDownDetails nores = new DropDownDetails()
            {
                Id = processIni.AppvlStatusId,
                Code = ReportFilterStatus.ProcessIniNoResponse,
                Value = $"{processIni.AppvlStatusDesc} {"( No Response ) "}",
            };
            dataList.Add(nores);

            DropDownDetails ongoing = new DropDownDetails()
            {
                Id = processIni.AppvlStatusId,
                Code = ReportFilterStatus.ProcessIniOnGoing,
                Value = $"{processIni.AppvlStatusDesc} {"( Ongoing ) "}",
            };
            dataList.Add(ongoing);
            DropDownDetails submitted = new DropDownDetails()
            {
                Id = processIni.AppvlStatusId,
                Code = ReportFilterStatus.ProcessIniSubmit,
                Value = $"{processIni.AppvlStatusDesc} {"( Submitted ) "}",
            };
            dataList.Add(submitted);

            DropDownDetails linkNtSent = new DropDownDetails()
            {
                Id = processIni.AppvlStatusId,
                Code = ReportFilterStatus.LinkNotSent,
                Value = "Link Not Sent",
            };
            dataList.Add(linkNtSent);
            return dataList;
        }

        public async Task<GetPassbookDetailsResponse> GetPassbookDetails(int appointeeId)
        {
            var passbookDetails = new GetPassbookDetailsResponse();
            var _appointeedetails = await _dbContextClass.AppointeeDetails.Where(x => x.AppointeeId.Equals(appointeeId) & x.ActiveStatus == true).FirstOrDefaultAsync();
            string filePath = string.Empty;
            Surepass_GetUanPassbookResponse PassBookResponse = new Surepass_GetUanPassbookResponse();

            var _DocList = await _dbContextClass.AppointeeUploadDetails.Where(x => x.AppointeeId.Equals(appointeeId) & x.ActiveStatus == true & x.UploadTypeCode == FileTypealias.PFPassbook).FirstOrDefaultAsync();
            if (_DocList != null)
            {
                string path = _DocList.UploadPath;
                if (File.Exists(path))
                {
                    // Read entire text file content in one string
                    string _passbookdata = File.ReadAllText(path);
                    PassBookResponse = JsonConvert.DeserializeObject<Surepass_GetUanPassbookResponse>(_passbookdata);
                    var PassBookResponseData = PassBookResponse?.data;
                    if (PassBookResponseData != null)
                    {
                        var _companyDetailsList = new List<PfCompanyDetails>();
                        passbookDetails.clientId = PassBookResponseData.client_id;
                        passbookDetails.fullName = PassBookResponseData.full_name;
                        passbookDetails.fatherName = PassBookResponseData.father_name;
                        passbookDetails.pfUan = PassBookResponseData.pf_uan;
                        passbookDetails.dob = PassBookResponseData.dob;

                        foreach (var obj in PassBookResponseData.companies)
                        {
                            var _copmnyData = obj.Value;
                            var _companyDetails = new PfCompanyDetails();
                            _companyDetails.passbook = new List<CompanyPassbookDetails>();
                            _companyDetails.companyName = _copmnyData?.company_name;
                            _companyDetails.establishmentId = _copmnyData?.establishment_id;
                            _companyDetails.memberId = _copmnyData?.passbook.LastOrDefault()?.member_id;
                            _companyDetails.LastTransactionApprovedOn = _copmnyData?.passbook.LastOrDefault()?.approved_on;
                            _companyDetails.LastTransactionMonth = CommonUtility.getMonthName(Convert.ToInt32(_copmnyData?.passbook.LastOrDefault()?.month));
                            _companyDetails.LastTransactionYear = _copmnyData?.passbook.LastOrDefault()?.year;
                            var passbookDetailsList = _copmnyData?.passbook?.Select((x, index) => new CompanyPassbookDetails
                            {
                                id = index + 1,
                                approvedOn = x.approved_on,
                                description = x.description,
                                month = CommonUtility.getMonthName(Convert.ToInt32(x.month)),
                                year = x.year
                            })?.ToList();
                            _companyDetails.passbook = passbookDetailsList;
                            _companyDetailsList.Add(_companyDetails);
                        }
                        passbookDetails.companies = _companyDetailsList;
                    }
                }
            }
            return passbookDetails;
            //  return filePath;
        }
    }
}
