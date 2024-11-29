using VERIDATA.BLL.Interfaces;
using VERIDATA.BLL.Notification.Provider;
using VERIDATA.BLL.utility;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.Model.Configuration;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.ExchangeModels;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Admin;
using VERIDATA.Model.Table.Master;
using VERIDATA.Model.Table.Public;
using static VERIDATA.DAL.utility.CommonEnum;

namespace VERIDATA.BLL.Context
{
    public class WorkFlowContext : IWorkFlowContext
    {
        //private readonly DbContextDalDB _dbContextClass;
        private readonly IFileContext _fileContext;
        private readonly IWorkFlowDalContext _dbContextWorkflow;
        private readonly IUserDalContext _dbContextUser;
        private readonly IMasterDalContext _dbContextMaster;
        private readonly IAppointeeDalContext _dbContextCandiate;
        private readonly IActivityDalContext _dbContextActivity;
        private readonly IEmailSender _emailSender;
        private readonly ApiConfiguration _aadhaarConfig;
        private readonly EmailConfiguration _emailConfig;
        private readonly string key;
        public WorkFlowContext(IWorkFlowDalContext dbContextWorkflow, IMasterDalContext dbContextMaster, IUserDalContext dbContextUser, IAppointeeDalContext dbContextCandiate,
            IFileContext fileContext, IEmailSender emailSender, ApiConfiguration aadhaarConfig, EmailConfiguration emailConfig, IActivityDalContext dbContextActivity)
        {
            _dbContextWorkflow = dbContextWorkflow;
            _dbContextMaster = dbContextMaster;
            _dbContextUser = dbContextUser;
            _dbContextCandiate = dbContextCandiate;
            _dbContextActivity = dbContextActivity;
            _fileContext = fileContext;
            _emailSender = emailSender;
            _aadhaarConfig = aadhaarConfig;
            //_configSetup = configSetup;
            key = aadhaarConfig.EncriptKey;
            _emailConfig = emailConfig;
        }
        public async Task<List<ProcessDataResponse>> GetProcessDataAsync(ProcessedFilterRequest filter)
        {
            List<ProcessDataResponse>? _processViewdata = new();

            List<ProcessedDataDetailsResponse> list = await _dbContextWorkflow.GetProcessedAppointeeDetailsAsync(filter);

            if (!(filter.IsFiltered ?? false))
            {
                _processViewdata = list?.DistinctBy(x => x.AppointeeId)?.OrderByDescending(x => x.ProcessedId)?.Select(row => new ProcessDataResponse
                {
                    id = row?.ProcessedId ?? 0,
                    companyId = row?.CompanyId ?? 0,
                    candidateId = row?.CandidateId,
                    appointeeName = row?.AppointeeName,
                    appointeeId = row?.AppointeeId,
                    appointeeEmailId = row?.AppointeeEmailId,
                    mobileNo = row?.MobileNo,
                    adhaarNo = string.IsNullOrEmpty(row?.AppointeeData?.AadhaarNumberView) ? "NA" : row?.AppointeeData?.AadhaarNumberView,
                    panNo = string.IsNullOrEmpty(row?.AppointeeData?.PANNumber) ? "NA" : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row?.AppointeeData?.PANNumber)),
                    dateOfJoining = row?.DateOfJoining,
                    epfWages = row?.AppointeeData?.EPFWages,
                    uanNo = string.IsNullOrEmpty(row?.AppointeeData?.UANNumber) ? "NA" : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row?.AppointeeData?.UANNumber)),
                    status = row?.StateAlias == WorkFlowStatusType.ForcedApproved ? "Manual Override" : "Verified",
                    isPensionApplicable = row?.AppointeeData?.IsPensionApplicable == null ? "NA" : row?.AppointeeData?.IsPensionApplicable ?? false ? "Yes" : "No",
                    isPensionGap = row?.AppointeeData?.IsPensionGap == null ? "NA" : row?.AppointeeData?.IsPensionGap ?? false ? "Yes" : "No",
                    isTrustPFApplicable = row?.AppointeeData?.IsTrustPassbook ?? false,
                    uanLinkWithAadhar = row?.AppointeeData?.IsUanAadharLink == null ? "NA" : row?.AppointeeData?.IsUanAadharLink ?? false ? "Yes" : "No",
                    passbookStatus = row?.AppointeeData?.IsManualPassbook == null && row?.AppointeeData?.IsPassbookFetch == null ? "NA" : row?.AppointeeData?.IsManualPassbook ?? false ? "Manual" : row?.AppointeeData?.IsPassbookFetch ?? false ? "Auto" : "NA",
                    passbookStatusCode = row?.AppointeeData?.IsManualPassbook == null && row?.AppointeeData?.IsPassbookFetch == null ? string.Empty : row?.AppointeeData?.IsManualPassbook ?? false ? "MNL" : row?.AppointeeData?.IsPassbookFetch ?? false ? "AF" : string.Empty,
                    //PassbookVerifiedStatus = row?.AppointeeData?.IsEmployementVarified != null ? (row?.AppointeeData?.IsEmployementVarified ?? false) ? "Yes" : "No" : string.IsNullOrEmpty(row?.AppointeeData?.UANNumber) ? "NA" : "No",
                }).ToList();
            }
            else
            {
                int _filterDays = filter.NoOfDays ?? 0;
                List<int?> AppointeeList = await _dbContextWorkflow.GetTotalOfferAppointeeList(_filterDays);

                _processViewdata = list?.Where(x => AppointeeList.Contains(x.AppointeeId))?.DistinctBy(x => x.AppointeeId)?.OrderByDescending(x => x.ProcessedId)?.Select(row => new ProcessDataResponse
                {
                    id = row?.ProcessedId ?? 0,
                    companyId = row?.CompanyId ?? 0,
                    candidateId = row?.CandidateId,
                    appointeeName = row?.AppointeeName,
                    appointeeId = row?.AppointeeId,
                    appointeeEmailId = row?.AppointeeEmailId,
                    mobileNo = row?.MobileNo,
                    adhaarNo = string.IsNullOrEmpty(row?.AppointeeData?.AadhaarNumberView) ? "NA" : row?.AppointeeData?.AadhaarNumberView,
                    panNo = string.IsNullOrEmpty(row?.AppointeeData?.PANNumber) ? "NA" : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row?.AppointeeData?.PANNumber)),
                    dateOfJoining = row?.DateOfJoining,
                    epfWages = row?.AppointeeData?.EPFWages,
                    uanNo = string.IsNullOrEmpty(row?.AppointeeData?.UANNumber) ? "NA" : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row?.AppointeeData?.UANNumber)),
                    status = row.StateAlias == WorkFlowStatusType.ForcedApproved ? "Manual Override" : "Verified",
                    isPensionApplicable = row?.AppointeeData?.IsPensionApplicable == null ? "NA" : row?.AppointeeData?.IsPensionApplicable ?? false ? "Yes" : "No",
                    isPensionGap = row?.AppointeeData?.IsPensionGap == null ? "NA" : row?.AppointeeData?.IsPensionGap ?? false ? "Yes" : "No",
                    isTrustPFApplicable = row?.AppointeeData?.IsTrustPassbook ?? false,
                    uanLinkWithAadhar = row?.AppointeeData?.IsUanAadharLink == null ? "NA" : row?.AppointeeData?.IsUanAadharLink ?? false ? "Yes" : "No",
                    passbookStatus = row?.AppointeeData?.IsManualPassbook == null && row?.AppointeeData?.IsPassbookFetch == null ? "NA" : row?.AppointeeData?.IsManualPassbook ?? false ? "Manual" : row?.AppointeeData?.IsPassbookFetch ?? false ? "Auto" : "NA",
                    passbookStatusCode = row?.AppointeeData?.IsManualPassbook == null && row?.AppointeeData?.IsPassbookFetch == null ? string.Empty : row?.AppointeeData?.IsManualPassbook ?? false ? "MNL" : row?.AppointeeData?.IsPassbookFetch ?? false ? "AF" : string.Empty,
                    //PassbookVerifiedStatus = row?.AppointeeData?.IsEmployementVarified != null ? (row?.AppointeeData?.IsEmployementVarified ?? false) ? "Yes" : "No" : string.IsNullOrEmpty(row?.AppointeeData?.UANNumber) ? "NA" : "No",

                }).ToList();
            }

            return _processViewdata;
        }
        public async Task<List<RejectedDataResponse>> GetRejectedDataAsync(FilterRequest filter)
        {

            List<RejectedDataDetailsResponse> rejectedAppointeeList = await _dbContextWorkflow.GetRejectedAppointeeDetailsAsync(filter);
            List<RejectedDataResponse> response = new();
            List<IGrouping<int?, RejectedDataDetailsResponse>> GroupData = rejectedAppointeeList.GroupBy(x => x.AppointeeId).ToList();
            foreach (var item in GroupData.Select((value, index) => new { Value = value, Index = index }))
            {
                IGrouping<int?, RejectedDataDetailsResponse> x = item.Value;
                List<string?> ResonDetails = x.Select(y => y.Remarks).ToList();
                string Remarks = string.Join(",", ResonDetails);
                RejectedDataResponse? _data = x.Select(row =>

                    new RejectedDataResponse
                    {
                        id = row?.RejectedId ?? 0,
                        companyId = row?.CompanyId ?? 0,
                        candidateId = row?.CandidateId,
                        appointeeName = row?.AppointeeName,
                        appointeeId = row?.AppointeeId ?? 0,
                        appointeeEmailId = row?.AppointeeEmailId,
                        mobileNo = row?.MobileNo,
                        adhaarNo = string.IsNullOrEmpty(row?.AadhaarNumberView) ? "NA" : row?.AadhaarNumberView,
                        panNo = string.IsNullOrEmpty(row?.PANNumber) ? "NA" : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row?.PANNumber)),
                        dateOfJoining = row?.DateOfJoining,
                        RejectReason = Remarks,
                        RejectFrom = "",
                    }).FirstOrDefault();
                response.Add(_data);
            }

            return response;
        }
        public async Task<List<UnderProcessDetailsResponse>> GetUnderProcessDataAsync(AppointeeSeacrhFilterRequest reqObj)
        {
            List<UnderProcessDetailsResponse>? _underProcessViewdata = new();
            List<UnderProcessDetailsResponse>? _underProcessdata = new();
            int? typeFilterCode = reqObj.FilterType == "UNDPRCS" ? 1 : reqObj.FilterType == "NORES" ? 0 : null;
            List<UnderProcessQueryDataResponse> UnderProcessAppointeeList = await _dbContextWorkflow.GetUnderProcessDataAsync(reqObj);
            if (UnderProcessAppointeeList.Count > 0)
            {
                if (!(reqObj.IsFiltered ?? false))
                {
                    _underProcessdata = UnderProcessAppointeeList?.Where(z => z.IsJoiningDateLapsed.Equals(false))?.Select(row => new UnderProcessDetailsResponse
                    {
                        id = row.UnderProcess.UnderProcessId,
                        fileId = row.UnderProcess?.FileId ?? 0,
                        companyId = row.UnderProcess?.CompanyId ?? 0,
                        candidateId = row.AppointeeDetails?.CandidateId ?? row.UnderProcess?.CandidateId,
                        appointeeName = row.AppointeeDetails?.AppointeeName ?? row.UnderProcess?.AppointeeName,
                        appointeeId = row.UnderProcess?.AppointeeId,
                        appointeeEmailId = row.AppointeeDetails?.AppointeeEmailId ?? row.UnderProcess?.AppointeeEmailId,
                        mobileNo = row.UnderProcess?.MobileNo,
                        isPFverificationReq = row.UnderProcess?.IsPFverificationReq,
                        epfWages = row.AppointeeDetails?.EPFWages ?? row.UnderProcess?.EPFWages,
                        dateOfOffer = row.UnderProcess?.DateOfOffer,
                        dateOfJoining = row.AppointeeDetails?.DateOfJoining ?? row.UnderProcess?.DateOfJoining,
                        isDocSubmitted = row.AppointeeDetails?.IsSubmit ?? false,
                        isReprocess = false,
                        isNoIsuueinVerification = !(row.AppointeeDetails?.IsAadhaarVarified == false || row.AppointeeDetails?.IsUanVarified == false || row.AppointeeDetails?.IsPanVarified == false || row.AppointeeDetails?.IsPasssportVarified == false),
                        status = (row?.AppvlStatusCode == WorkFlowStatusType.ReuploadDocument) ? "Submitted" : (row.AppointeeDetails?.IsSubmit ?? false) ? "Submitted" : (row?.AppvlStatusCode == WorkFlowStatusType.ManualVerification) || (row?.AppvlStatusCode == WorkFlowStatusType.ManualReVerification) || row.AppointeeDetails?.SaveStep == 1 ? "Ongoing" : "No Response",
                        statusCode = row?.AppvlStatusCode == WorkFlowStatusType.ReuploadDocument ? 4 : row.AppointeeDetails?.IsSubmit ?? false ? 2 : row.AppointeeDetails?.SaveStep ?? 0,
                        verificationStatusCode = row?.AppvlStatusCode,
                        consentStatusCode = row.ConsentStatusId ?? 0,
                        passbookStatus = row?.AppointeeDetails?.IsManualPassbook == null && row?.AppointeeDetails?.IsPassbookFetch == null ? "NA" : row?.AppointeeDetails?.IsManualPassbook ?? false ? "Manual" : row?.AppointeeDetails?.IsPassbookFetch ?? false ? "Auto" : "NA",
                        createdDate = row.UnderProcess?.CreatedOn
                    }).OrderByDescending(x => x.isDocSubmitted).ThenBy(y => y.dateOfJoining).ToList();

                }
                else
                {
                    List<int?> AppointeeList = await _dbContextWorkflow.GetTotalOfferAppointeeList(reqObj?.NoOfDays ?? 0);

                    _underProcessdata = UnderProcessAppointeeList?.Where(z => z.IsJoiningDateLapsed.Equals(false))?.DistinctBy(x => x.UnderProcess?.AppointeeId)?.Where(x => AppointeeList.Contains(x.UnderProcess?.AppointeeId))?.Select(row => new UnderProcessDetailsResponse
                    {
                        id = row.UnderProcess.UnderProcessId,
                        fileId = row.UnderProcess?.FileId ?? 0,
                        companyId = row.UnderProcess?.CompanyId ?? 0,
                        candidateId = row.AppointeeDetails?.CandidateId ?? row.UnderProcess?.CandidateId,
                        appointeeName = row.AppointeeDetails?.AppointeeName ?? row.UnderProcess?.AppointeeName,
                        appointeeId = row.UnderProcess?.AppointeeId,
                        appointeeEmailId = row.AppointeeDetails?.AppointeeEmailId ?? row.UnderProcess?.AppointeeEmailId,
                        mobileNo = row.UnderProcess?.MobileNo,
                        isPFverificationReq = row.UnderProcess?.IsPFverificationReq,
                        epfWages = row.AppointeeDetails?.EPFWages ?? row.UnderProcess?.EPFWages,
                        dateOfOffer = row.UnderProcess?.DateOfOffer,
                        dateOfJoining = row.AppointeeDetails?.DateOfJoining ?? row.UnderProcess?.DateOfJoining,
                        isDocSubmitted = row.AppointeeDetails?.IsSubmit ?? false,
                        isReprocess = false,
                        isNoIsuueinVerification = !(row.AppointeeDetails?.IsAadhaarVarified == false || row.AppointeeDetails?.IsUanVarified == false || row.AppointeeDetails?.IsPanVarified == false || row.AppointeeDetails?.IsPasssportVarified == false),
                        //Status = row?.IsReupload ?? false ? "Reupload Requested" : row.AppointeeDetails?.IsSubmit ?? false ? "Submitted" : row.AppointeeDetails?.SaveStep == 1 ? "Ongoing" : "No Response",
                        status = (row?.AppvlStatusCode == WorkFlowStatusType.ReuploadDocument) ? "Submitted" : (row.AppointeeDetails?.IsSubmit ?? false) ? "Submitted" : (row?.AppvlStatusCode == WorkFlowStatusType.ManualVerification) || (row?.AppvlStatusCode == WorkFlowStatusType.ManualReVerification) || row.AppointeeDetails?.SaveStep == 1 ? "Ongoing" : "No Response",
                        statusCode = row?.AppvlStatusCode == WorkFlowStatusType.ReuploadDocument ? 4 : row.AppointeeDetails?.IsSubmit ?? false ? 2 : row.AppointeeDetails?.SaveStep ?? 0,
                        verificationStatusCode = row?.AppvlStatusCode,
                        consentStatusCode = row.ConsentStatusId ?? 0,
                        passbookStatus = row?.AppointeeDetails?.IsManualPassbook == null && row?.AppointeeDetails?.IsPassbookFetch == null ? "NA" : row?.AppointeeDetails?.IsManualPassbook ?? false ? "Manual" : row?.AppointeeDetails?.IsPassbookFetch ?? false ? "Auto" : "NA",
                        //passbookStatus = row?.AppointeeDetails?.IsManualPassbook == null ? "NA" : row?.AppointeeDetails?.IsManualPassbook ?? false ? "Manual" : "AutoFetch",
                        createdDate = row.UnderProcess?.CreatedOn
                    }).OrderByDescending(x => x.isDocSubmitted).ThenBy(y => y.dateOfJoining).ToList();
                }

                _underProcessViewdata = (typeFilterCode != null) ? (typeFilterCode == 0) ? _underProcessdata.Where(x => x.statusCode == typeFilterCode).ToList()
                         : _underProcessdata.Where(x => x.statusCode != 0).ToList() : _underProcessdata;

                _underProcessViewdata = (reqObj.StatusCode == null || reqObj.StatusCode?.ToUpper() == "ALL") ? _underProcessViewdata
                         : _underProcessdata.Where(x => x.statusCode == Convert.ToInt32(reqObj.StatusCode ?? "0"))?.ToList();

                if (reqObj.IssueFilter != null)
                {
                    _underProcessViewdata = reqObj.IssueFilter == true ? _underProcessViewdata?.Where(x => x.isNoIsuueinVerification == false)?.ToList() : _underProcessViewdata?.Where(x => x.isNoIsuueinVerification == true)?.ToList();
                }


            }
            return _underProcessViewdata;
        }
        public async Task<List<UnderProcessDetailsResponse>> GetExpiredProcessDataAsync(AppointeeSeacrhFilterRequest reqObj)
        {
            List<UnderProcessDetailsResponse>? _expiredProcessViewdata = new();
            List<UnderProcessDetailsResponse>? _expiredProcessData = new();
            string CurrDate = DateTime.Now.ToShortDateString();
            List<UnderProcessQueryDataResponse> UnderProcessAppointeeList = await _dbContextWorkflow.GetUnderProcessDataAsync(reqObj);
            List<UnderProcessQueryDataResponse>? list = UnderProcessAppointeeList?.Where(x => x.IsJoiningDateLapsed)?.ToList();
            if (list?.Count > 0)
            {
                if (!(reqObj.IsFiltered ?? false))
                {

                    _expiredProcessViewdata = list.Select(row => new UnderProcessDetailsResponse
                    {
                        id = row.UnderProcess.UnderProcessId,
                        fileId = row.UnderProcess?.FileId ?? 0,
                        companyId = row.UnderProcess?.CompanyId ?? 0,
                        candidateId = row.AppointeeDetails?.CandidateId ?? row.UnderProcess?.CandidateId,
                        appointeeName = row.AppointeeDetails?.AppointeeName ?? row.UnderProcess?.AppointeeName,
                        appointeeId = row.UnderProcess?.AppointeeId,
                        appointeeEmailId = row.AppointeeDetails?.AppointeeEmailId ?? row.UnderProcess?.AppointeeEmailId,
                        mobileNo = row.UnderProcess?.MobileNo,
                        isPFverificationReq = row.UnderProcess?.IsPFverificationReq,
                        epfWages = row.AppointeeDetails?.EPFWages ?? row.UnderProcess?.EPFWages,
                        dateOfOffer = row.UnderProcess?.DateOfOffer,
                        dateOfJoining = row.AppointeeDetails?.DateOfJoining ?? row.UnderProcess?.DateOfJoining,
                        isDocSubmitted = row.AppointeeDetails?.IsSubmit ?? false,
                        isReprocess = false,
                        status = row.AppointeeDetails?.IsSubmit ?? false ? "Submitted" : row.AppointeeDetails?.SaveStep == 1 ? "Ongoing" : "No Response",
                        statusCode = row.AppointeeDetails?.IsSubmit ?? false ? 2 : row.AppointeeDetails?.SaveStep ?? 0,
                        consentStatusCode = row.ConsentStatusId ?? 0,
                        createdDate = row.UnderProcess?.CreatedOn
                    }).OrderByDescending(x => x.isDocSubmitted).ThenBy(y => y.dateOfJoining).ToList();
                }
                else
                {
                    List<int?> AppointeeList = await _dbContextWorkflow.GetTotalOfferAppointeeList(reqObj.NoOfDays ?? 0);
                    _expiredProcessViewdata = list?.DistinctBy(x => x.UnderProcess?.AppointeeId).Where(x => AppointeeList.Contains(x.UnderProcess?.AppointeeId)).Select(row => new UnderProcessDetailsResponse
                    {
                        id = row.UnderProcess.UnderProcessId,
                        fileId = row.UnderProcess.FileId,
                        companyId = row.UnderProcess?.CompanyId ?? 0,
                        candidateId = row.AppointeeDetails?.CandidateId ?? row.UnderProcess?.CandidateId,
                        appointeeName = row.AppointeeDetails?.AppointeeName ?? row.UnderProcess?.AppointeeName,
                        appointeeId = row.UnderProcess?.AppointeeId,
                        appointeeEmailId = row.AppointeeDetails?.AppointeeEmailId ?? row.UnderProcess?.AppointeeEmailId,
                        mobileNo = row.UnderProcess.MobileNo,
                        isPFverificationReq = row.UnderProcess?.IsPFverificationReq,
                        epfWages = row.AppointeeDetails?.EPFWages ?? row.UnderProcess?.EPFWages,
                        dateOfOffer = row.UnderProcess?.DateOfOffer,
                        dateOfJoining = row.AppointeeDetails?.DateOfJoining ?? row.UnderProcess?.DateOfJoining,
                        isDocSubmitted = row.AppointeeDetails?.IsSubmit ?? false,
                        isReprocess = false,
                        status = row.AppointeeDetails?.IsSubmit ?? false ? "Submitted" : row.AppointeeDetails?.SaveStep == 1 ? "Ongoing" : "No Response",
                        statusCode = row.AppointeeDetails?.IsSubmit ?? false ? 2 : row.AppointeeDetails?.SaveStep ?? 0,
                        consentStatusCode = row.ConsentStatusId ?? 0,
                        createdDate = row.UnderProcess?.CreatedOn
                    }).OrderByDescending(x => x.isDocSubmitted).ThenBy(y => y.dateOfJoining).ToList();
                }
                _expiredProcessData = (reqObj.StatusCode == null || reqObj.StatusCode?.ToUpper() == "ALL") ? _expiredProcessViewdata :
                    _expiredProcessViewdata?.Where(x => x.statusCode == Convert.ToInt32(reqObj.StatusCode ?? "0"))?.ToList();
            }
            return _expiredProcessData;
        }
        public async Task<List<CriticalAppointeeResponse>> GetCriticalAppointeeList(CriticalFilterDataRequest reqObj)
        {
            //  var filterdaysrange = _configSetup.CriticalDaysLimit;
            string currDate = DateTime.Now.ToShortDateString();
            DateTime _currDate = Convert.ToDateTime(currDate);
            GeneralSetup generalsetupData = await _dbContextMaster.GetGeneralSetupData();
            int filterDaysrange = generalsetupData?.CriticalNoOfDays ?? 0;
            DateTime maxDate = _currDate.AddDays(filterDaysrange);
            List<CriticalAppointeeResponse> actionRequiredListdata = new();

            List<UnderProcessQueryDataResponse> underProcessData = await _dbContextWorkflow.GetUnderProcessDataByDOJAsync(_currDate, maxDate, reqObj.FromDate, reqObj.ToDate);
            List<UnProcessedFileData> nonProcessData = await _dbContextWorkflow.GetCriticalUnProcessDataAsync(_currDate, maxDate, reqObj.FromDate, reqObj.ToDate);

            List<CriticalAppointeeResponse>? _underProcessViewdata = underProcessData?.Select(row => new CriticalAppointeeResponse
            {

                id = row?.UnderProcess?.UnderProcessId ?? 0,
                fileId = row?.UnderProcess?.FileId ?? 0,
                companyId = row?.UnderProcess?.CompanyId ?? 0,
                candidateId = row?.AppointeeDetails?.CandidateId ?? row?.UnderProcess?.CandidateId,
                appointeeName = row?.AppointeeDetails?.AppointeeName ?? row?.UnderProcess?.AppointeeName,
                appointeeId = row?.UnderProcess?.AppointeeId,
                appointeeEmailId = row?.AppointeeDetails?.AppointeeEmailId ?? row?.UnderProcess?.AppointeeEmailId,
                mobileNo = row?.UnderProcess?.MobileNo,
                dateOfJoining = row?.AppointeeDetails?.DateOfJoining ?? row?.UnderProcess?.DateOfJoining,
                Status = row?.AppointeeDetails?.IsSubmit ?? false ? "Ongoing" : row?.AppointeeDetails?.SaveStep == 1 ? "Ongoing" : "No Response",
                StatusCode = row?.AppointeeDetails?.IsSubmit ?? false ? 2 : row?.AppointeeDetails?.SaveStep == 1 ? 2 : 1,
                ConsentStatusCode = row.ConsentStatusId ?? 0,
                DaysToJoin = Convert.ToInt32(((row?.AppointeeDetails?.DateOfJoining ?? row?.UnderProcess?.DateOfJoining) - DateTime.Now)?.TotalDays ?? 0),
                CreatedDate = row?.UnderProcess?.CreatedOn
            })?.ToList();
            List<CriticalAppointeeResponse>? _unProcessViewdata = nonProcessData?.Select(row => new CriticalAppointeeResponse
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
                ConsentStatusCode = 0,
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
            return actionRequiredListdata;
        }
        public async Task<List<RawFileDataDetailsResponse>> getRawfileData(int? fileId, int companyId)
        {
            List<RawFileData> _rawdataList = await _dbContextWorkflow.GetRawfiledataByIdAsync(fileId, companyId);

            List<RawFileDataDetailsResponse> RawFileData = new();

            foreach ((RawFileData row, RawFileDataDetailsResponse _data) in
            from RawFileData row in _rawdataList
            let _data = new RawFileDataDetailsResponse()
            select (row, _data))
            {
                _data.id = row.RawFileId;
                _data.fileId = row.FileId;
                _data.companyId = row.CompanyId;
                _data.companyName = row.CompanyName;
                _data.CandidateId = row.CandidateId;
                _data.appointeeName = row.AppointeeName;
                _data.appointeeEmailId = row.AppointeeEmailId;
                _data.mobileNo = row.MobileNo;
                _data.isPFverificationReq = row.IsPFverificationReq;
                _data.epfWages = row.EPFWages;
                _data.dateOfOffer = row.DateOfOffer;
                _data.dateOfJoining = row.DateOfJoining;
                _data.isChecked = null;
                _data.lvl1Email = row.lvl1Email;
                _data.lvl2Email = row.lvl2Email;
                _data.lvl3Email = row.lvl3Email;

                RawFileData.Add(_data);
            }

            return RawFileData;
        }
        public async Task<List<RawFileDataDetailsResponse>> GetNonProcessDataAsync(AppointeeSeacrhFilterRequest reqObj)
        {
            List<UnProcessedFileData> _unProcessData = await _dbContextWorkflow.GetUnProcessDataAsync(reqObj);


            List<RawFileDataDetailsResponse> unProcessViewdata = new();

            unProcessViewdata = _unProcessData.Select(row => new RawFileDataDetailsResponse
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

            return unProcessViewdata;
        }
        public async Task<List<UnderProcessDetailsResponse>> ProcessRawData(RawDataProcessRequest rawdfileata)
        {
            List<UnderProcessDetailsResponse> _underProcessViewdata = await MoveDatatoUnderProcessAsync(rawdfileata);
            //end Remove from rawdatalist 
            RoleDetailsResponse _roleDetails = await _dbContextMaster.getRoleDetailsByRoleAlias(RoleTypeAlias.Appointee);
            //begin Appointee User Create
            List<CreateUserDetailsRequest>? userList = new();

            foreach ((UnderProcessDetailsResponse obj, CreateUserDetailsRequest user) in from obj in _underProcessViewdata
                                                                                         let user = new CreateUserDetailsRequest()
                                                                                         select (obj, user))
            {
                user.UserName = obj.appointeeName;
                user.UserCode = CommonUtility.GenarateUserName(obj.appointeeName, obj.fileId);
                user.Password = CommonUtility.RandomString(8);
                user.EmailId = obj.appointeeEmailId;
                user.ContactNo = obj.mobileNo;
                user.CandidateId = obj.candidateId;
                user.CompanyId = obj.companyId;
                user.UserTypeId = (int)UserType.Appoientee;
                user.RoleId = _roleDetails.RoleId;
                user.RefAppointeeId = obj.appointeeId;
                user.UserId = rawdfileata.UserId;
                userList.Add(user);
            }

            if (userList?.Count > 0)
            {
                await _dbContextUser.createNewUserwithRole(userList, rawdfileata.UserId);
                //End Appointee User Create

                //begin WorkFlow mail send insert
                List<int?>? appointeeList = userList?.Select(x => x.RefAppointeeId).ToList();
                int _stateId = await _dbContextWorkflow.GetWorkFlowStateIdByAlias(WorkFlowType.sendMail);

                await _dbContextWorkflow.AppointeeWorkflowIniAsync(appointeeList, _stateId, rawdfileata.UserId);
                bool _IsMailSend = _emailConfig.IsMailSend;
                if (_IsMailSend)
                {
                    await _emailSender.SendAppointeeLoginMail(userList, MailType.CandidateCreate);
                }
                //end WorkFlow mail send insert
            }
            return _underProcessViewdata;
        }
        private async Task<List<UnderProcessDetailsResponse>> MoveDatatoUnderProcessAsync(RawDataProcessRequest rawRequestData)
        {
            List<RawFileDataDetailsResponse> _underProcessData = new();
            List<int>? _removedatalist = rawRequestData.RawDataList?.Select(y => y.id).ToList();

            if (rawRequestData?.IsUnprocessed ?? false)
            {
                _underProcessData = await _dbContextWorkflow.GetNonProcessedDetailsByTypeId(rawRequestData?.RawDataList, rawRequestData?.UserId);

            }
            else
            {
                _underProcessData = await _dbContextWorkflow.GetRawfiledetailsByTypeId(rawRequestData?.RawDataList, rawRequestData?.UserId, 1);
                List<RawFileDataDetailsResponse> _unProcessData = await _dbContextWorkflow.GetRawfiledetailsByTypeId(rawRequestData?.RawDataList, rawRequestData?.UserId, 2);
                if (_unProcessData?.Count > 0)
                {
                    await _dbContextWorkflow.PostNonProcessDataAsync(_unProcessData, rawRequestData.UserId);
                }
            }

            List<UnderProcessFileData> _getunderProcessData = await _dbContextWorkflow.PostUnderProcessDataAsync(_underProcessData, rawRequestData.UserId);

            //Begin Remove from rawdatalist

            if (!(rawRequestData?.IsUnprocessed ?? false))
            {
                await _dbContextWorkflow.RemoveRawDataAsync(_removedatalist);
            }
            else
            {
                await _dbContextWorkflow.RemoveUnprocessedDataAsync(_removedatalist);
            }

            List<UnderProcessDetailsResponse> _underProcessViewdata = _getunderProcessData.Select(row => new UnderProcessDetailsResponse
            {
                id = row.UnderProcessId,
                fileId = row.FileId,
                companyId = row.CompanyId,
                candidateId = row.CandidateId,
                appointeeName = row.AppointeeName,
                appointeeId = row.AppointeeId,
                appointeeEmailId = row.AppointeeEmailId,
                mobileNo = row.MobileNo,
                isPFverificationReq = row.IsPFverificationReq,
                epfWages = row.EPFWages,
                dateOfOffer = row.DateOfOffer,
                dateOfJoining = row.DateOfJoining,
                // isChecked = null,
            }).ToList();

            //end underprocess and unprocess  insert
            return _underProcessViewdata;
        }
        public async Task PostAppointeeSaveDetailsAsync(AppointeeSaveDetailsRequest AppointeeDetails)
        {
            if (AppointeeDetails != null)
            {
                bool _isPassportAvailable = AppointeeDetails?.IsPassportAvailable?.ToString()?.ToUpper() == CheckType.yes;
                AppointeeDetails.PassportNo = _isPassportAvailable ? string.IsNullOrEmpty(AppointeeDetails.PassportNo) ? null : CommonUtility.CustomEncryptString(key, AppointeeDetails.PassportNo) : null;

                await _dbContextWorkflow.PostAppointeeSaveDetailsAsync(AppointeeDetails);
            }
        }
        public async Task UpdateAppointeeDojByAdmin(CompanySaveAppointeeDetailsRequest AppointeeDetails)
        {
            if (AppointeeDetails != null)
            {
                await _dbContextWorkflow.UpdateAppointeeDojByAdmin(AppointeeDetails);
            }
        }
        public async Task PostAppointeeFileDetailsAsync(AppointeeFileDetailsRequest AppointeeFileDetails)
        {
            _ = _aadhaarConfig.EncriptKey;
            int appointeeId = AppointeeFileDetails.AppointeeId;
            AppointeeDetails? _appointeedetails = await _dbContextCandiate.GetAppinteeDetailsById(appointeeId);
            string mailType = string.Empty;
            if (_appointeedetails.IsProcessed != true)
            {
                bool _isSubmit = _appointeedetails?.IsSubmit ?? false;
                if (_appointeedetails != null && !_isSubmit)
                {
                    await _fileContext.postappointeeUploadedFiles(AppointeeFileDetails);
                    var isManual = _appointeedetails?.IsPassbookFetch == true || string.IsNullOrEmpty(_appointeedetails.UANNumber) && AppointeeFileDetails?.IsManualPassbookUploaded == false ? null : AppointeeFileDetails?.IsManualPassbookUploaded;
                    await _dbContextCandiate.UpdateAppointeeSubmit(appointeeId, AppointeeFileDetails.IsSubmit ?? false, isManual);
                    mailType = MailType.Submit;
                    if (isManual ?? false)
                    {
                        WorkFlowDataRequest _WorkFlowDataRequest = new();
                        int _stateId = await _dbContextWorkflow.GetWorkFlowStateIdByAlias(WorkFlowType.UploadDetails);
                        _WorkFlowDataRequest.appointeeId = appointeeId;
                        _WorkFlowDataRequest.remarks = string.Empty;
                        _WorkFlowDataRequest.workflowState = _stateId;
                        _WorkFlowDataRequest.approvalStatus = WorkFlowStatusType.ManualVerification;
                        _WorkFlowDataRequest.userId = AppointeeFileDetails.UserId;

                        await _dbContextWorkflow.AppointeeWorkflowUpdateAsync(_WorkFlowDataRequest);
                    }
                }
                if ((AppointeeFileDetails.IsSubmit ?? false) && !_isSubmit)
                {

                    //if ((_appointeedetails?.IsUanVarified ?? false) && (_appointeedetails.IsAadhaarVarified ?? false) && (_appointeedetails.IsPanVarified ?? false) && (_appointeedetails.IsFNameVarified ?? false))
                    if ((_appointeedetails?.IsUanVarified ?? false) && (_appointeedetails.IsAadhaarVarified ?? false)  && (_appointeedetails.IsFNameVarified ?? false))
                    {
                        mailType = MailType.AutoApprove;
                        await DataUploadAndApproved(_appointeedetails.AppointeeId, AppointeeFileDetails?.UserId ?? 0, true);//isapprove set true

                    }
                }

                await PostMailFileSubmisstionSuccess(_appointeedetails.AppointeeId ?? 0, AppointeeFileDetails?.UserId ?? 0, mailType);

            }

            await _dbContextActivity.PostActivityDetails(AppointeeFileDetails?.AppointeeId ?? 0, AppointeeFileDetails?.UserId ?? 0, ActivityLog.DATASBMT);
        }


        private async Task DataUploadAndApproved(int? appointeeId, int userId, bool IsApproved)
        {
            int UploadDetailsId = await _dbContextWorkflow.GetWorkFlowStateIdByAlias(WorkFlowType.UploadDetails);

            WorkFlowDataRequest workFlowDataRequest = new()
            {
                appointeeId = appointeeId ?? 0,
                workflowState = UploadDetailsId,
                approvalStatus = string.Empty,
                remarks = string.Empty,
                userId = userId

            };
            await _dbContextWorkflow.AppointeeWorkflowUpdateAsync(workFlowDataRequest);
            if (IsApproved)
            {
                int _stateId = await _dbContextWorkflow.GetWorkFlowStateIdByAlias(WorkFlowType.DataVarified);

                WorkFlowDataRequest workFlowApproveDataRequest = new()
                {
                    appointeeId = appointeeId ?? 0,
                    workflowState = _stateId,
                    approvalStatus = WorkFlowStatusType.Approved,
                    remarks = string.Empty,
                    userId = userId
                };
                await _dbContextWorkflow.AppointeeWorkflowUpdateAsync(workFlowApproveDataRequest);
            }
        }
        public async Task<List<GetAppointeeGlobalSearchResponse>> GetAppointeeSearchGlobal(string Name)
        {
            List<GetAppointeeGlobalSearchResponse> searchedDataRes = new();
            GeneralSetup generalsetupData = await _dbContextMaster.GetGeneralSetupData();
            int filterdaysrange = generalsetupData?.CriticalNoOfDays ?? 0;
            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime maxDate = _currDate.AddDays(filterdaysrange);
            List<WorkflowApprovalStatusMaster> _getapprovalStatus = await _dbContextMaster.GetAllApprovalStateMaster();
            WorkflowApprovalStatusMaster? closeState = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowStatusType.ProcessClose?.Trim());
            WorkflowApprovalStatusMaster? verifiedState = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowStatusType.Approved?.Trim());
            WorkflowApprovalStatusMaster? forcedVerifiedState = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowStatusType.ForcedApproved?.Trim());
            WorkflowApprovalStatusMaster? rejectedState = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowStatusType.Rejected?.Trim());
            WorkflowApprovalStatusMaster? manuVerification = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowStatusType.ManualVerification?.Trim());
            WorkflowApprovalStatusMaster? manuReVerification = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowStatusType.ManualReVerification?.Trim());
            WorkflowApprovalStatusMaster? manuReUpload = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowStatusType.ReuploadDocument?.Trim());

            List<MenuMaster> menuDataList = await _dbContextMaster.GetMasterMenuData();
            MenuMaster? verifiedMenu = menuDataList.Find(x => x.MenuAlias == MenuCode.VERIFIED);
            MenuMaster? rejectedMenu = menuDataList.Find(x => x.MenuAlias == MenuCode.REJECTED);
            MenuMaster? processingMenu = menuDataList.Find(x => x.MenuAlias == MenuCode.PROCESSING);
            MenuMaster? lapsedMenu = menuDataList.Find(x => x.MenuAlias == MenuCode.EXPIRED);
            //var criticalMenu = menuDataList.FirstOrDefault(x => x.MenuAlias == MenuCode.CRITICAL);
            MenuMaster? linkntsendMenu = menuDataList.Find(x => x.MenuAlias == MenuCode.LINKNOTSENT);
            MenuMaster? uploadedDataMenu = menuDataList.Find(x => x.MenuAlias == MenuCode.UPLOADEDDATA);
            MenuMaster? manualVeriMenu = menuDataList.Find(x => x.MenuAlias == MenuCode.MANUALVERIFICATION);

            //var appointeeData = await _dbContextClass.AppointeeMaster.Where(m => m.AppointeeName.Contains(Name) && m.ActiveStatus == true).ToListAsync();

            List<GlobalSearchAppointeeData> appointeelist = await _dbContextWorkflow.GetUnderProcessAppointeeSearch(Name?.Trim());

            List<GetAppointeeGlobalSearchResponse> _allProcessedData = appointeelist.DistinctBy(x => x.AppointeeId).Where(x => x.AppvlStatusId == verifiedState.AppvlStatusId || x.AppvlStatusId == rejectedState.AppvlStatusId || x.AppvlStatusId == forcedVerifiedState.AppvlStatusId)
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
            List<GetAppointeeGlobalSearchResponse> _allManualVerificationData = appointeelist.DistinctBy(x => x.AppointeeId).Where(x => (x.AppvlStatusId == manuVerification.AppvlStatusId || x.AppvlStatusId == manuReVerification.AppvlStatusId || x.AppvlStatusId == manuReUpload.AppvlStatusId))
               .Select(obj => new GetAppointeeGlobalSearchResponse
               {

                   AppointeeName = obj.AppointeeName,
                   CandidateId = obj.CandidateId,
                   AppointeePath = manualVeriMenu.menu_action,
                   PathName = manualVeriMenu.MenuTitle,
               }).ToList();
            if (_allManualVerificationData.Count > 0)
            {
                searchedDataRes.AddRange(_allManualVerificationData);
            }
            var manualCandidateList = _allManualVerificationData.Select(y => y.CandidateId).ToList();
            var currentAppointeeList = appointeelist.Where(x => !manualCandidateList.Contains(x.CandidateId))?.ToList();
            List<GetAppointeeGlobalSearchResponse> _allUnderProcessedData = currentAppointeeList.DistinctBy(x => x.AppointeeId).Where(x => !(x.AppvlStatusId == verifiedState.AppvlStatusId || x.AppvlStatusId == rejectedState.AppvlStatusId || x.AppvlStatusId == forcedVerifiedState.AppvlStatusId))
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
            List<GlobalSearchAppointeeData> linknotsendData = await _dbContextWorkflow.GetAppointeeSearchDetails(Name?.Trim(), "LinkNotSend");
            foreach (GlobalSearchAppointeeData obj in linknotsendData)
            {
                GetAppointeeGlobalSearchResponse res = new()
                {
                    AppointeeName = obj.AppointeeName,
                    CandidateId = obj.CandidateId,
                    AppointeePath = linkntsendMenu.menu_action,
                    PathName = linkntsendMenu.MenuTitle
                };
                searchedDataRes.Add(res);
            }

            List<GlobalSearchAppointeeData> uploadedData = await _dbContextWorkflow.GetAppointeeSearchDetails(Name?.Trim(), "Raw");

            foreach (GlobalSearchAppointeeData obj in uploadedData)
            {
                GetAppointeeGlobalSearchResponse res = new()
                {
                    AppointeeName = obj.AppointeeName,
                    CandidateId = obj.CandidateId,
                    AppointeePath = uploadedDataMenu.menu_action,
                    PathName = uploadedDataMenu.MenuTitle
                };
                searchedDataRes.Add(res);
            }

            return searchedDataRes;
        }
        public async Task<List<DropDownDetailsResponse>> GetAllReportFilterStatus()
        {
            List<DropDownDetailsResponse> dataList = new();
            List<WorkflowApprovalStatusMaster> _getapprovalStatus = await _dbContextMaster.GetAllApprovalStateMaster();
            //var _getapprovalStatus = await _dbContextClass.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToArrayAsync();
            WorkflowApprovalStatusMaster? processIni = _getapprovalStatus.Find(x => x.AppvlStatusCode.Equals(WorkFlowStatusType.ProcessIni));
            List<DropDownDetailsResponse>? resultSet = _getapprovalStatus?.Where(y => y.AppvlStatusCode != WorkFlowStatusType.ProcessIni).Select(x => new DropDownDetailsResponse
            {
                Id = x.AppvlStatusId,
                Code = x.AppvlStatusCode,
                Value = x.AppvlStatusDesc
            })?.ToList();
            dataList.AddRange(resultSet);
            DropDownDetailsResponse nores = new()
            {
                Id = processIni.AppvlStatusId,
                Code = ReportFilterStatus.ProcessIniNoResponse,
                Value = $"{processIni.AppvlStatusDesc} {"( No Response ) "}",
            };
            dataList.Add(nores);

            DropDownDetailsResponse ongoing = new()
            {
                Id = processIni.AppvlStatusId,
                Code = ReportFilterStatus.ProcessIniOnGoing,
                Value = $"{processIni.AppvlStatusDesc} {"( Ongoing ) "}",
            };
            dataList.Add(ongoing);
            DropDownDetailsResponse submitted = new()
            {
                Id = processIni.AppvlStatusId,
                Code = ReportFilterStatus.ProcessIniSubmit,
                Value = $"{processIni.AppvlStatusDesc} {"( Submitted ) "}",
            };
            dataList.Add(submitted);

            DropDownDetailsResponse linkNtSent = new()
            {
                Id = processIni.AppvlStatusId,
                Code = ReportFilterStatus.LinkNotSent,
                Value = "Link Not Sent",
            };
            dataList.Add(linkNtSent);
            return dataList;
        }
        public async Task<string?> AppointeeWorkflowCurrentState(int appointeeId)
        {
            WorkFlowDetails? getcurrentState = await _dbContextWorkflow.GetCurrentApprovalStateByAppointeeId(appointeeId);
            List<WorkflowApprovalStatusMaster> approvalStatusList = await _dbContextMaster.GetAllApprovalStateMaster();

            WorkflowApprovalStatusMaster? getApprovalStatus = approvalStatusList.Find(x => x.AppvlStatusId.Equals(getcurrentState?.AppvlStatusId) && x.ActiveStatus == true);

            return getApprovalStatus?.AppvlStatusCode;
        }
        public async Task PostAppointeeApprove(AppointeeApproverRequest request)
        {
            string AllRemarks = string.Empty;
            int _stateId = await _dbContextWorkflow.GetWorkFlowStateIdByAlias(WorkFlowType.DataVarified);
            if (!string.IsNullOrEmpty(request.Remarks))
            {
                List<ReasonRemarks> Remarks = new();
                Remarks.Add(new ReasonRemarks { ReasonCode = ReasonCode.OTHER, Remarks = request.Remarks });
                string _remarks = await _dbContextCandiate.UpdateRemarksByType(request.appointeeId, Remarks, RemarksType.Others, request.userId);
            }
            WorkFlowDataRequest _WorkFlowDataRequest = new()
            {
                appointeeId = request.appointeeId,
                workflowState = _stateId,
                approvalStatus = WorkFlowStatusType.ForcedApproved,
                remarks = request.Remarks,
                userId = request.userId
            };
            await _dbContextWorkflow.AppointeeWorkflowUpdateAsync(_WorkFlowDataRequest);

            List<GetRemarksResponse?> _getRemarksData = await _dbContextCandiate.GetRemarks(request.appointeeId);
            if (_getRemarksData?.Count > 0)
            {
                AllRemarks = string.Join(", ", _getRemarksData.Select(x => x?.Remarks)?.ToList());
            }

            await _emailSender.SendNotificationMailToEmployer(request.appointeeId, AllRemarks, MailType.ForceApprove);

        }
        public async Task PostAppointeeRejected(AppointeeApproverRequest request)
        {
            string remarks = string.Empty;
            if (!string.IsNullOrEmpty(request.Remarks))
            {
                List<ReasonRemarks> ReasonList = new()
                {
                    new ReasonRemarks() { ReasonCode = ReasonCode.OTHER, Remarks = request.Remarks }
                };
                remarks = await _dbContextCandiate.UpdateRemarksByType(request.appointeeId, ReasonList, RemarksType.Others, request?.userId ?? 0);
            }
            WorkFlowDataRequest _WorkFlowDataRequest = new();
            int _stateId = await _dbContextWorkflow.GetWorkFlowStateIdByAlias(WorkFlowType.DataVarified);

            _WorkFlowDataRequest.appointeeId = request.appointeeId;
            _WorkFlowDataRequest.remarks = request.Remarks;
            _WorkFlowDataRequest.workflowState = _stateId;
            _WorkFlowDataRequest.approvalStatus = WorkFlowStatusType.Rejected;
            _WorkFlowDataRequest.userId = request.userId;

            await _dbContextWorkflow.AppointeeWorkflowUpdateAsync(_WorkFlowDataRequest);
            await _emailSender.SendNotificationMailToEmployer(request.appointeeId, remarks, MailType.Reject);
        }
        public async Task PostAppointeeClose(AppointeeApproverRequest request)
        {
            WorkFlowDataRequest _WorkFlowDataRequest = new();
            int _stateId = await _dbContextWorkflow.GetWorkFlowStateIdByAlias(WorkFlowType.DataVarified);

            _WorkFlowDataRequest.appointeeId = request.appointeeId;
            _WorkFlowDataRequest.workflowState = _stateId;
            _WorkFlowDataRequest.approvalStatus = WorkFlowStatusType.ProcessClose;
            _WorkFlowDataRequest.remarks = request.Remarks;
            _WorkFlowDataRequest.userId = request.userId;

            await _dbContextWorkflow.AppointeeWorkflowUpdateAsync(_WorkFlowDataRequest);
        }
        public async Task PostRemainderMail(int appointeeId, int UserId)
        {
            UnderProcessFileData? appointeeDetails = await _dbContextCandiate.GetUnderProcessAppinteeDetailsById(appointeeId);
            if (!string.IsNullOrEmpty(appointeeDetails?.AppointeeEmailId))
            {
                mailTransactionRequest transReq = new();
                MailDetails mailDetails = new();
                MailBodyParseDataDetails bodyDetails = new()
                {
                    Name = appointeeDetails?.AppointeeName,
                    CompanyName = appointeeDetails?.CompanyName,
                    Url = _emailConfig.HostUrl
                };
                mailDetails.MailType = MailType.Remainder;
                mailDetails.ParseData = bodyDetails;
                await _emailSender.SendAppointeeMail(appointeeDetails.AppointeeEmailId, mailDetails);

                transReq.AppointeeId = appointeeId;
                transReq.UserId = UserId;
                transReq.Type = "REMDR";
                await _dbContextCandiate.PostMailTransDetails(transReq);
            }

        }
        public async Task<VarificationStatusResponse> ValidateRemainderMail(int appointeeId, int UserId)
        {
            VarificationStatusResponse response = new();
            response.IsVarified = true;

            List<mailTransactionResponse>? mailDetails = await _dbContextCandiate.GetMailTransDetails(appointeeId, UserId);
            if (mailDetails?.Count != 0)
            {
                var limitTime = DateTime.Now.AddMinutes(-(_emailConfig.ReminderResendLockDuration));
                var MailCount = mailDetails?.Where(x => x.CreatedOn > limitTime)?.ToList();
                //var limitTime = mailDetails.CreatedOn?.AddMinutes(_emailConfig.ReminderResendLockDuration);
                if (MailCount?.Count >= _emailConfig.ReminderAttempt)
                {
                    var lastMailSend = MailCount?.OrderByDescending(x => x.CreatedOn).FirstOrDefault().CreatedOn;
                    var remainTime = (lastMailSend - limitTime).GetValueOrDefault().Minutes;

                    response.Remarks = $"You have send {_emailConfig.ReminderAttempt} mail in last {_emailConfig.ReminderResendLockDuration} min . Please wait {remainTime} minutes before trying again.";
                    response.IsVarified = false;
                }
            }
            return response;
        }

        public async Task PostMailResend(int appointeeId, int UserId)
        {
            UserCredetialDetailsResponse? userCredentialDetails = await _dbContextCandiate.GetUserCredentialInfo(appointeeId);
            if (!string.IsNullOrEmpty(userCredentialDetails?.CandidateId))
            {
                mailTransactionRequest transReq = new();
                MailDetails mailDetails = new();
                MailBodyParseDataDetails bodyDetails = new()
                {
                    Name = userCredentialDetails?.UserName,
                    UserCode = userCredentialDetails?.userCode,
                    Email = userCredentialDetails?.EmailId,
                    Password = userCredentialDetails?.DefaultPassword?.ToUpper() == "Y" ? userCredentialDetails?.Password : string.Empty,
                    Url = _emailConfig.HostUrl
                };
                mailDetails.MailType = MailType.MailResend;
                mailDetails.ParseData = bodyDetails;
                await _emailSender.SendAppointeeMail(userCredentialDetails.EmailId, mailDetails);

                transReq.AppointeeId = appointeeId;
                transReq.UserId = UserId;
                transReq.Type = "RESEND";
                await _dbContextCandiate.PostMailTransDetails(transReq);
            }

        }

        private async Task PostMailFileSubmisstionSuccess(int appointeeId, int UserId, string type)
        {
            var appointeeDetails = await _dbContextCandiate.GetAppinteeDetailsById(appointeeId);
            if (!string.IsNullOrEmpty(appointeeDetails?.AppointeeEmailId))
            {
                MailDetails mailDetails = new();
                MailBodyParseDataDetails bodyDetails = new()
                {
                    Name = appointeeDetails?.AppointeeName,
                    CompanyName = appointeeDetails?.CompanyName,
                    CandidateId = appointeeDetails?.CandidateId,
                    Url = _emailConfig.HostUrl
                };

                // Setting the mail type for success notification
                mailDetails.MailType = type;
                mailDetails.ParseData = bodyDetails;

                // Use existing SendAppointeeMail function
                await _emailSender.SendAppointeeMail(appointeeDetails.AppointeeEmailId, mailDetails);

            }

        }

        public async Task<List<FileCategoryResponse>> getFileType(int appointeeId)
        {
            //  throw new NotImplementedException();

            if (appointeeId <= 0)
            {
                return null;
            }

            return await _dbContextWorkflow.getFileTypeCode(appointeeId);
        }

        public async Task VerifyAppointeeManualAsync(AppointeeApproveVerificationRequest reqObj)
        {
            bool? isDataValid = false;
            var isDataVerificationReq = await VefifyDocValidityManual(reqObj.AppointeeId, reqObj.VerificationSubCategoryList, reqObj.UserId, reqObj.VerificationCategory);
            if (!string.IsNullOrEmpty(reqObj.Remarks))
            {
                List<ReasonRemarks> Remarks = new();
                Remarks.Add(new ReasonRemarks { ReasonCode = ReasonCode.OTHER, Remarks = reqObj.Remarks });
                string _remarks = await _dbContextCandiate.UpdateRemarksByType(reqObj.AppointeeId, Remarks, RemarksType.Others, reqObj.UserId);
            }
            bool? isPensionApplicable = null;
            var updatedAppointeeDetails = new AppointeeDetails();
            //if (isDataVerificationReq)
            //{
            switch (reqObj.VerificationCategory)
            {
                case ManualVerificationType.FathersName:
                    if (isDataVerificationReq)
                    {
                        var verificationQuestion = reqObj.VerificationSubCategoryList?.FirstOrDefault().VerificationQueries;
                        isDataValid = await VefifyFNameValidityManual(reqObj.AppointeeId, verificationQuestion, reqObj.UserId);
                    }
                    updatedAppointeeDetails = await _dbContextCandiate.VefifyAppinteeFathersNameManualById(reqObj.AppointeeId, isDataValid, reqObj.VerificationCategory, reqObj.UserId);
                    break;
                case ManualVerificationType.EpfoPassbook:
                    if (isDataVerificationReq)
                    {
                        var PassbookQuestion = reqObj.VerificationSubCategoryList?.Where(x => x.SubCategory == "EPFPSSBKMNL").FirstOrDefault();
                        isPensionApplicable = PassbookQuestion?.VerificationQueries?.FirstOrDefault(x => x.FieldName == ManualVerificationFieldType.PensionApplicable)?.Value ?? false;
                        isDataValid = await VefifyPassBookValidityManual(reqObj.AppointeeId, PassbookQuestion?.VerificationQueries, isPensionApplicable ?? false, reqObj.UserId);
                    }
                    var pfVerificationReqObj = new AppointeePfVerificationRequest
                    {
                        AppointeeId = reqObj.AppointeeId,
                        IsValid = isDataValid,
                        Type = reqObj.VerificationCategory,
                        IsPensionApplicable = isPensionApplicable,
                        IsPensionGapFind = isDataVerificationReq ? !(isDataValid ?? true) : null,
                        UserId = reqObj.UserId
                    };
                    updatedAppointeeDetails = await _dbContextCandiate.VefifyAppinteePfDetailsManualById(pfVerificationReqObj);

                    break;
            }
            if (isDataVerificationReq)
            {
                //if ((updatedAppointeeDetails?.IsUanVarified ?? false) && (updatedAppointeeDetails.IsAadhaarVarified ?? false) && (updatedAppointeeDetails.IsPanVarified ?? false) && (updatedAppointeeDetails.IsFNameVarified ?? false))
                if ((updatedAppointeeDetails?.IsUanVarified ?? false) && (updatedAppointeeDetails.IsAadhaarVarified ?? false)  && (updatedAppointeeDetails.IsFNameVarified ?? false))
                {
                    await DataUploadAndApproved(updatedAppointeeDetails.AppointeeId, reqObj?.UserId ?? 0, true);//isapprove set true

                    await PostMailFileSubmisstionSuccess(updatedAppointeeDetails.AppointeeId ?? 0, reqObj?.UserId ?? 0, MailType.AutoApprove);
                }

            }
        }
        private async Task<bool> VefifyDocValidityManual(int appointeeId, List<VerificationUpdatesubCategory>? docValidity, int userId, string? VerificationCategory)// TODO
        {
            List<ReasonRemarks> reasonList = new();
            bool isVerificationRequired = true;
            // Dynamically update each field specified in the request
            foreach (var obj in docValidity)
            {
                var inputData = string.Empty;

                inputData = obj.SubCategory == ManualVerificationSubType.TENTHCERT || obj.SubCategory == ManualVerificationSubType.OTHID ? "Father's Name Verification" : obj.SubCategory == ManualVerificationSubType.EpfHistory ? "EPFO Service History" : obj.SubCategory == ManualVerificationSubType.EpfPassbook ? "EPFO Passbook(s)" : string.Empty;
                foreach (var questions in obj?.VerificationQueries)
                {


                    switch (questions.FieldName)
                    {
                        case ManualVerificationFieldType.DocIncomplete:
                            if (!questions.Value)
                            {


                                reasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.INCMPLTDOC, Inputdata = inputData, Fetcheddata = string.Empty });
                                isVerificationRequired = false;
                            }
                            break;
                        case ManualVerificationFieldType.DocInvalid:
                            if (!questions.Value)
                            {
                                reasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.INVDDOC, Inputdata = inputData, Fetcheddata = string.Empty });
                                isVerificationRequired = false;

                            }
                            break;

                    }
                }
                if (!isVerificationRequired)
                {
                    await docReuploadRequested(appointeeId, userId, reasonList);
                    return false;
                }
            }

            //if (!isVerificationRequired)
            //{

            //}

            return isVerificationRequired;

        }

        private async Task docReuploadRequested(int appointeeId, int userId, List<ReasonRemarks> reasonList)
        {
            string remarks = await _dbContextCandiate.UpdateRemarksByType(appointeeId, reasonList, RemarksType.Manual, userId);
            WorkFlowDataRequest _WorkFlowDataRequest = new();
            int _stateId = await _dbContextWorkflow.GetWorkFlowStateIdByAlias(WorkFlowType.UploadDetails);

            _WorkFlowDataRequest.appointeeId = appointeeId;
            _WorkFlowDataRequest.remarks = remarks;
            _WorkFlowDataRequest.workflowState = _stateId;
            _WorkFlowDataRequest.approvalStatus = WorkFlowStatusType.ReuploadDocument;
            _WorkFlowDataRequest.userId = userId;

            await _dbContextWorkflow.AppointeeWorkflowUpdateAsync(_WorkFlowDataRequest);
            await RemarksMailSend(appointeeId, remarks, RemarksType.Manual, userId);
            await _dbContextActivity.PostActivityDetails(appointeeId, userId, ActivityLog.DOCUMENTREUPLOADRQST);
        }

        private async Task<bool> VefifyFNameValidityManual(int appointeeId, List<VerificationUpdate>? DocValidity, int userId)
        {
            List<ReasonRemarks> ReasonList = new();
            bool isDataValid = true;
            string activityType = string.Empty;
            // Dynamically update each field specified in the request
            foreach (var update in DocValidity)
            {
                switch (update.FieldName.ToLower())
                {
                    case ManualVerificationFieldType.FathersName:
                        if (!update.Value)
                        {
                            ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.CAREOFNAME, Inputdata = string.Empty, Fetcheddata = string.Empty });
                            isDataValid = false;

                        }
                        break;

                }
            }
            // Save the changes to the database
            if (!isDataValid)
            {
                string Remarks = await _dbContextCandiate.UpdateRemarksByType(appointeeId, ReasonList, RemarksType.Manual, userId);
                await RemarksMailSend(appointeeId, Remarks, RemarksType.Manual, userId);
                activityType = ActivityLog.MNLFTHRVERIFLD;
                await docReuploadRequested(appointeeId, userId, ReasonList);
            }
            else
            {
                activityType = ActivityLog.MNLFTHRVERIFIED;
            }
            await _dbContextActivity.PostActivityDetails(appointeeId, userId, activityType);
            return isDataValid;
        }

        private async Task<bool> VefifyPassBookValidityManual(int appointeeId, List<VerificationUpdate>? docValidity, bool isPensionApplicable, int userId)
        {
            List<ReasonRemarks> ReasonList = new();
            bool isDataValid = true;
            string activityType = string.Empty;
            foreach (var update in docValidity)
            {
                switch (update.FieldName.ToLower())
                {
                    case ManualVerificationFieldType.PensionGap:
                        if (isPensionApplicable && update.Value)
                        {
                            ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.PENSIONGAPFIND, Inputdata = string.Empty, Fetcheddata = string.Empty });
                            isDataValid = false;
                        }
                        break;

                }
            }
            // Save the changes to the database
            if (!isDataValid)
            {
                string Remarks = await _dbContextCandiate.UpdateRemarksByType(appointeeId, ReasonList, RemarksType.Manual, userId);
                await RemarksMailSend(appointeeId, Remarks, RemarksType.Manual, userId);
                activityType = ActivityLog.MNLUANVERIFLD;
                await docReuploadRequested(appointeeId, userId, ReasonList);
            }
            else
            {
                activityType = ActivityLog.MNLUANERICMPLTE;
            }
            await _dbContextActivity.PostActivityDetails(appointeeId, userId, activityType);
            return isDataValid;


        }
        private async Task RemarksMailSend(int appointeeId, string Remarks, string type, int? userId)
        {
            AppointeeDetails _appointeedetails = await _dbContextCandiate.GetAppinteeDetailsById(appointeeId);

            string mailtype = CommonUtility.GetMailType(type);
            if (!string.IsNullOrEmpty(_appointeedetails?.AppointeeEmailId))
            {
                MailDetails mailDetails = new();
                MailBodyParseDataDetails bodyDetails = new()
                {
                    Name = _appointeedetails.AppointeeName,
                    Reason = Remarks,
                };
                mailDetails.MailType = mailtype;
                mailDetails.ParseData = bodyDetails;
                await _emailSender.SendAppointeeMail(_appointeedetails?.AppointeeEmailId, mailDetails);
            }

        }

        public async Task<List<FileCategoryResponse>> GetNotVeriedfileView(int appointeeId)
        {
            // First, verify the conditions based on `AppointeeDetails` table
            var result = await _dbContextWorkflow.CheckVerifyDetails(appointeeId);

            // If the conditions are not met, return an empty list
            if (!result)
            {
                return new List<FileCategoryResponse>();
            }

            // If conditions are met, proceed to fetch the file data
            var fileData = await _dbContextWorkflow.getFileTypeCode(appointeeId);
            return fileData;
        }

        public async Task<List<ManualVerificationProcessDetailsResponse>> GetManualVeificationProcessData(ManualVeificationProcessDataRequest reqObj)
        {
            List<ManualVerificationProcessQueryDataResponse> UnderProcessAppointeeList = await _dbContextWorkflow.GetManualVerificationProcessDataAsync(reqObj);
            //if (UnderProcessAppointeeList.Count > 0)
            //{

            var _underProcessdata = UnderProcessAppointeeList?.Where(z => z.IsJoiningDateLapsed.Equals(false))?.Select(row => new ManualVerificationProcessDetailsResponse
            {
                id = row.UnderProcess.UnderProcessId,
                companyId = row.UnderProcess?.CompanyId ?? 0,
                candidateId = row.AppointeeDetails?.CandidateId ?? row.UnderProcess?.CandidateId,
                appointeeName = row.AppointeeDetails?.AppointeeName ?? row.UnderProcess?.AppointeeName,
                appointeeId = row.UnderProcess?.AppointeeId,
                appointeeEmailId = row.AppointeeDetails?.AppointeeEmailId ?? row.UnderProcess?.AppointeeEmailId,
                mobileNo = row.UnderProcess?.MobileNo,
                dateOfOffer = row.UnderProcess?.DateOfOffer,
                dateOfJoining = row.AppointeeDetails?.DateOfJoining ?? row.UnderProcess?.DateOfJoining,
                isDocSubmitted = row.AppointeeDetails?.IsSubmit ?? false,
                isNoIsuueinVerification = !(row.AppointeeDetails?.IsAadhaarVarified == false || row.AppointeeDetails?.IsUanVarified == false || row.AppointeeDetails?.IsPanVarified == false || row.AppointeeDetails?.IsPasssportVarified == false),
                verificationAttempted = row?.VerificationAttempted ?? 0,
                createdDate = row?.WorkflowCreatedDate,
                status = row?.Status

            }).OrderByDescending(x => x.createdDate).ThenBy(y => y.dateOfJoining).ToList();

            return _underProcessdata;
        }

        public async Task PostAppointeeFileReuploadDetailsAsync(AppointeeReUploadFilesAfterSubmitRequest AppointeeFileDetailsReupload)
        {
            int appointeeId = AppointeeFileDetailsReupload.AppointeeId;
            AppointeeDetails? _appointeedetails = await _dbContextCandiate.GetAppinteeDetailsById(appointeeId);
            if (_appointeedetails?.IsProcessed != true && _appointeedetails?.IsSubmit == true)
            {
                await _fileContext.postappointeeReUploadedFiles(AppointeeFileDetailsReupload);
                var _stateId = await _dbContextWorkflow.GetWorkFlowStateIdByAlias(WorkFlowType.UploadDetails);
                await _dbContextWorkflow.AppointeeWorkflowUpdateAsync(new WorkFlowDataRequest
                {
                    appointeeId = AppointeeFileDetailsReupload.AppointeeId,
                    remarks = string.Empty,
                    workflowState = _stateId,
                    approvalStatus = WorkFlowStatusType.ManualReVerification,
                    userId = AppointeeFileDetailsReupload.UserId
                });

                await _dbContextActivity.PostActivityDetails(
                    AppointeeFileDetailsReupload?.AppointeeId ?? 0,
                    AppointeeFileDetailsReupload?.UserId ?? 0,
                    ActivityLog.DOCUMENTREUPLED
                );
            }

        }
    }

}