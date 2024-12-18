using System.Data;
using System.Net;
using Microsoft.Extensions.Logging;
using VERIDATA.BLL.Interfaces;
using VERIDATA.BLL.Notification.Provider;
using VERIDATA.BLL.utility;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Config;
using VERIDATA.Model.Table.Public;
using static VERIDATA.DAL.utility.CommonEnum;

namespace VERIDATA.BLL.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IActivityDalContext _activityDalContext;
        private readonly ILogger<WorkerService> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IFileContext _fileService;
        private readonly IWorkFlowDalContext _dbContextWorkflow;
        private readonly ISetupConfigarationContext _setupConfigarationContext;

        public WorkerService(ILogger<WorkerService> logger, IEmailSender emailSender, IActivityDalContext activityDalContext, IFileContext fileService, ISetupConfigarationContext setupConfigarationContext, IWorkFlowDalContext dbContextWorkflow)
        {
            _logger = logger;
            _activityDalContext = activityDalContext;
            _emailSender = emailSender;
            _fileService = fileService;
            _dbContextWorkflow = dbContextWorkflow;
            _setupConfigarationContext = setupConfigarationContext;
        }

        public async Task ApiCountMailAsync()
        {
            DateTime _currDate = DateTime.Today;
            // Your background job logic here

            List<ApiCounter>? totalApiList = await _activityDalContext.GetTotalApiCountByDate(_currDate);

            List<ApiCountJobResponse>? TotalApiCount = totalApiList?.Where(x => x?.Type == "Request")?.GroupBy(x => x.ApiName)?.Select(y => new ApiCountJobResponse
            {
                ApiName = y.Key?.ToLower(),
                TotalApiCount = y?.ToList()?.Count ?? 0,
                TotalSuccessApiCount = 0
            })?.ToList();

            List<ApiCounter>? TotalResponse = totalApiList?.Where(x => x?.Type == "Response")?.ToList();

            List<ApiCountJobResponse>? TotalSuccessApiCount = TotalResponse?.Where(x => x?.Status == (int)HttpStatusCode.OK)?.GroupBy(x => x.ApiName)?.Select(y => new ApiCountJobResponse
            {
                ApiName = y?.Key,
                TotalSuccessApiCount = y?.ToList()?.Count ?? 0,
            })?.ToList();

            List<ApiCountJobResponse>? TotalUnproceesbleApiCount = TotalResponse?.Where(x => x?.Status == (int)HttpStatusCode.UnprocessableEntity)?.GroupBy(x => x.ApiName)?.Select(y => new ApiCountJobResponse
            {
                ApiName = y?.Key,
                TotalUnprocessableEntityCount = y?.ToList()?.Count() ?? 0,
                //TotalApiCount = TotalApiCount?.Where(x => x.ApiName?.ToLower() == y?.Key?.ToLower())?.ToList()?.Count() ?? 0
            })?.ToList();

            List<ApiCountJobResponse>? TotalFaliureApiCount = TotalResponse?.Where(x => x?.Status is not (((int)HttpStatusCode.UnprocessableEntity) or ((int)HttpStatusCode.OK)))?.GroupBy(x => x.ApiName)?.Select(y => new ApiCountJobResponse
            {
                ApiName = y?.Key,
                TotalFailureCount = y?.ToList()?.Count ?? 0,
            })?.ToList();

            foreach (ApiCountJobResponse? obj in TotalApiCount)
            {
                obj.Date = _currDate.ToShortDateString();
                obj.TotalSuccessApiCount = TotalSuccessApiCount?.Where(x => x.ApiName?.ToLower() == obj?.ApiName?.ToLower())?.FirstOrDefault()?.TotalSuccessApiCount ?? 0;
                obj.TotalUnprocessableEntityCount = TotalUnproceesbleApiCount?.Where(x => x.ApiName?.ToLower() == obj?.ApiName?.ToLower())?.FirstOrDefault()?.TotalUnprocessableEntityCount ?? 0;
                obj.TotalFailureCount = TotalFaliureApiCount?.Where(x => x.ApiName?.ToLower() == obj?.ApiName?.ToLower())?.FirstOrDefault()?.TotalFailureCount ?? 0;
            }
            // var apilist = querydata.ToList();
            if (TotalApiCount?.Count() > 0)
            {
                DataTable _exportdt = CommonUtility.ToDataTable(TotalApiCount);
                Filedata filedata = _fileService.GenerateDataTableTofile(_exportdt, "Report", ValidationType.ApiCount);
                List<Filedata> attachtData = new() { filedata };
                await _emailSender.SendMailWithAttachtment("Tanmoy", "pfcsrver005@gmail.com", attachtData, ValidationType.ApiCount);
                //TODO
            }

            // This method will be executed asynchronously by Hangfire.
        }

        public async Task ApponteeCountMailAsync()
        {
            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            List<UploadAppointeeCounter> appointeeCount = await _activityDalContext.GetTotalAppointeeCountByDate(_currDate);
            int totalAppointee = appointeeCount.Sum(x => x.Count);
            List<CountJobResponse> dataList = new();
            CountJobResponse data = new()
            {
                Name = $"Total number of appointee added in {_currDate}",
                Value = totalAppointee.ToString(),
            };
            dataList.Add(data);
            DataTable _exportdt = CommonUtility.ToDataTable(dataList);
            Filedata filedata = _fileService.GenerateDataTableTofile(_exportdt, "Report", ValidationType.AppointeeCount);
            List<Filedata> attachtData = new() { filedata };
            await _emailSender.SendMailWithAttachtment("Tanmoy", "pfcsrver005@gmail.com", attachtData, ValidationType.AppointeeCount);
        }

        public async Task CriticalAppointeeMail()
        {
            GeneralSetupDetailsResponse escalationSetupData = await _setupConfigarationContext.gettSetupData();
            await criticalAppointeeCaseWise(EscalationCase.DOJ1Week, escalationSetupData);
            await criticalAppointeeCaseWise(EscalationCase.DOJ2Week, escalationSetupData);
        }

        private async Task criticalAppointeeCaseWise(string caseCode, GeneralSetupDetailsResponse escalationSetupData)
        {
            ValidationType criticality = ValidationType.Critical1week;
            //var escalationSetupData = Task.Run(async () => await _setupConfigarationContext.gettSetupData()).GetAwaiter().GetResult();
            int criticalDays;
            switch (caseCode)
            {
                case EscalationCase.DOJ1Week:
                    criticalDays = 7;
                    criticality = ValidationType.Critical1week;
                    break;

                case EscalationCase.DOJ2Week:
                    criticalDays = 14;
                    criticality = ValidationType.Critical2week;
                    break;

                default:
                    criticalDays = 7;
                    break;
            }

            await criticalAppointeeLevelWise(caseCode, EscalationLevel.Level1, criticalDays, criticality, escalationSetupData);
            await criticalAppointeeLevelWise(caseCode, EscalationLevel.Level2, criticalDays, criticality, escalationSetupData);
            await criticalAppointeeLevelWise(caseCode, EscalationLevel.Level3, criticalDays, criticality, escalationSetupData);
        }

        private async Task criticalAppointeeLevelWise(string caseCode, string levelType, int criticalDays, ValidationType type, GeneralSetupDetailsResponse escalationSetupData)
        {
            EmailEscalationLevelDetails? _escalationlevelData = escalationSetupData?.EmailEscalationLevelDetails?.Where(x => x.LevelCode == levelType)?.FirstOrDefault();
            CaseOptionDetails _CriticalStatus = GetCaseOption(escalationSetupData, levelType, caseCode);
            if (_CriticalStatus?.SetupCaseOption ?? false)
            {
                string? EscalatedEmailAddress = string.IsNullOrEmpty(_CriticalStatus.CaseEmailAddress) ? _escalationlevelData?.Emailaddress.FirstOrDefault() : _CriticalStatus.CaseEmailAddress;
                List<AppointeeJobDetails> datalist = await GetCriticalAppointeeList(criticalDays);
                if (datalist.Count > 0)
                {
                    Dictionary<string, List<AppointeeJobDetails>>? _userList = new();
                    _userList = generateCaseBasedData(levelType, _userList, datalist, EscalatedEmailAddress, type);
                    //var _exportdt = CommonUtility.ToDataTable<CriticalAppointeeJobResponse>(datalist);
                    //var filePath = Task.Run(async () => await _fileService.SaveDataTabletofile(_exportdt, "report", criticality)).GetAwaiter().GetResult();

                    //escalationMailSend(EscalationLevel.Level1, caseCode, filePath, criticality);
                    //escalationMailSend(EscalationLevel.Level2, caseCode, filePath, criticality);
                }
            }
        }

        private static CaseOptionDetails GetCaseOption(GeneralSetupDetailsResponse escalationSetupData, string level, string caseCode)
        {
            EmailEscalationLevelDetails? _escalationlevelData = escalationSetupData.EmailEscalationLevelDetails.FirstOrDefault(x => x.LevelCode.Equals(level));
            EmailEscalationSetupDetails? _escalationSetUpData = escalationSetupData.EmailEscalationSetupDetails.FirstOrDefault(x => x.LevelCode.Equals(level));
            CaseOptionDetails? _escalationCaseData = _escalationSetUpData.SetupCaseDetails?.FirstOrDefault(y => y.SetupCaseCode == caseCode);
            return _escalationCaseData;
        }

        private async Task<List<AppointeeJobDetails>> GetCriticalAppointeeList(int criticalDays)
        {
            int filterDaysrange = criticalDays;
            string currDate = DateTime.Now.ToShortDateString();
            DateTime _currDate = Convert.ToDateTime(currDate);

            DateTime maxDate = _currDate.AddDays(filterDaysrange);
            List<AppointeeJobDetails> actionRequiredListdata = new();

            List<UnderProcessQueryDataResponse> underProcessData = await _dbContextWorkflow.GetUnderProcessDataByDOJAsync(_currDate, maxDate, null, null);
            List<UnProcessedFileData> nonProcessData = await _dbContextWorkflow.GetCriticalUnProcessDataAsync(_currDate, maxDate, null, null);

            List<AppointeeJobDetails>? _underProcessViewdata = underProcessData?.OrderByDescending(x => x?.UnderProcess?.DateOfJoining)?.Select(row => new AppointeeJobDetails
            {
                candidateId = row?.AppointeeDetails?.CandidateId ?? row?.UnderProcess?.CandidateId,
                appointeeName = row?.AppointeeDetails?.AppointeeName ?? row?.UnderProcess?.AppointeeName,
                appointeeEmailId = row?.AppointeeDetails?.AppointeeEmailId ?? row?.UnderProcess?.AppointeeEmailId,
                mobileNo = row?.UnderProcess?.MobileNo,
                dateOfJoining = row?.AppointeeDetails?.DateOfJoining ?? row?.UnderProcess?.DateOfJoining,
                Status = row?.AppointeeDetails?.IsSubmit ?? false ? "Ongoing" : row?.AppointeeDetails?.SaveStep == 1 ? "Ongoing" : "No Response",
                DaysToJoin = Convert.ToInt32(((row?.AppointeeDetails?.DateOfJoining ?? row?.UnderProcess?.DateOfJoining) - DateTime.Now)?.TotalDays ?? 0).ToString(),
                CreatedDate = row?.UnderProcess?.CreatedOn,
                lvl1Email = row?.UnderProcess?.lvl1Email,
                lvl2Email = row?.UnderProcess?.lvl2Email,
                lvl3Email = row?.UnderProcess?.lvl3Email,
            })?.ToList();
            List<AppointeeJobDetails>? _unProcessViewdata = nonProcessData?.Select(row => new AppointeeJobDetails
            {
                candidateId = row.CandidateId,
                appointeeName = row.AppointeeName,
                appointeeEmailId = row.AppointeeEmailId,
                mobileNo = row.MobileNo,
                dateOfJoining = row.DateOfJoining,
                Status = "Mail Not sent",
                DaysToJoin = Convert.ToInt32((row.DateOfJoining - DateTime.Now)?.TotalDays ?? 0).ToString(),
                CreatedDate = row.CreatedOn,
                lvl1Email = row?.lvl1Email,
                lvl2Email = row?.lvl2Email,
                lvl3Email = row?.lvl3Email,
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

        private Dictionary<string, List<AppointeeJobDetails>> generateCaseBasedData(string levelType, Dictionary<string, List<AppointeeJobDetails>>? _userList,
          List<AppointeeJobDetails>? _appointeeData, string? EscalatedEmailAddress, ValidationType type)
        {
            if (EscalationLevel.Level1 == levelType)
            {
                _appointeeData?.Where(x => string.IsNullOrEmpty(x.lvl1Email))?.ToList()?.ForEach(y => y.lvl1Email = EscalatedEmailAddress);
                _userList = _appointeeData?.GroupBy(o => o.lvl1Email)?.ToDictionary(g => g.Key, g => g.ToList());
            }
            if (EscalationLevel.Level2 == levelType)
            {
                _appointeeData?.Where(x => string.IsNullOrEmpty(x.lvl1Email))?.ToList()?.ForEach(y => y.lvl2Email = EscalatedEmailAddress);
                _userList = _appointeeData?.GroupBy(o => o.lvl2Email)?.ToDictionary(g => g.Key, g => g.ToList());
            }
            if (EscalationLevel.Level3 == levelType)
            {
                _appointeeData?.Where(x => string.IsNullOrEmpty(x.lvl1Email))?.ToList()?.ForEach(y => y.lvl3Email = EscalatedEmailAddress);
                _userList = _appointeeData?.GroupBy(o => o.lvl3Email)?.ToDictionary(g => g.Key, g => g.ToList());
            }
            foreach (KeyValuePair<string, List<AppointeeJobDetails>> obj in _userList)
            {
                string mailaddress = obj.Key;
                List<AppointeeJobDetails>? _data = obj.Value?.ToList();
                DataTable _exportdt = new();
                if (type is ValidationType.Critical2week or ValidationType.Critical1week)
                {
                    List<CriticalAppointeeJobResponse> _getViewData = GetCriticalProcessData(_data);
                    _exportdt = CommonUtility.ToDataTable(_getViewData);
                }
                else
                {
                    List<UnderProcessJobResponse> _getViewData = GetUnderProcessData(_data);
                    _exportdt = CommonUtility.ToDataTable(_getViewData);
                }

                Filedata fileDetails = _fileService.GenerateDataTableTofile(_exportdt, "report", type);
                _ = caseBasedEscalationMailSend(mailaddress, fileDetails, type);
            }

            return _userList;
        }

        private static List<UnderProcessJobResponse> GetUnderProcessData(List<AppointeeJobDetails> appointeeList)
        {
            List<UnderProcessJobResponse>? _underProcessViewdata = appointeeList?.Select(row => new UnderProcessJobResponse
            {
                appointeeName = row?.appointeeName,
                appointeeEmailId = row?.appointeeEmailId,
                mobileNo = row?.mobileNo,
                dateOfOffer = row?.dateOfOffer?.ToString("dd/mm/yyyy"),
                dateOfJoining = row?.dateOfJoining?.ToString("dd/mm/yyyy"),
                Status = row?.Status,
                CreatedDate = row?.CreatedDate?.ToString("dd/mm/yyyy"),
            }).ToList();

            return _underProcessViewdata;
        }

        private static List<CriticalAppointeeJobResponse> GetCriticalProcessData(List<AppointeeJobDetails> appointeeList)
        {
            List<CriticalAppointeeJobResponse>? _criticalViewdata = appointeeList.Select(row => new CriticalAppointeeJobResponse
            {
                candidateId = row.candidateId,
                appointeeName = row.appointeeName,
                appointeeEmailId = row.appointeeEmailId,
                mobileNo = row.mobileNo,
                dateOfJoining = row.dateOfJoining?.ToString("dd/mm/yyyy"),
                Status = row.Status,
                DaysToJoin = row?.DaysToJoin,
                CreatedDate = row.CreatedDate?.ToString("dd/mm/yyyy")
            })?.ToList();

            return _criticalViewdata;
        }

        private async Task caseBasedEscalationMailSend(string emailaddress, Filedata fileData, ValidationType type)
        {
            if (!string.IsNullOrEmpty(emailaddress))
            {
                List<Filedata> attachedFileData = new() { fileData };
                await _emailSender.SendMailWithAttachtment("Sir", emailaddress, attachedFileData, type);
            }
        }

        ////private void escalationMailSend(string level, string caseCode, string filePath, ValidationType type)
        ////{
        ////    var escalationSetupData = Task.Run(async () => await _setupConfigarationContext.gettSetupData()).GetAwaiter().GetResult();
        ////    var _escalationlevelData = escalationSetupData.EmailEscalationLevelDetails.FirstOrDefault(x => x.LevelCode.Equals(level));
        ////    var _escalationSetUpData = escalationSetupData.EmailEscalationSetupDetails.FirstOrDefault(x => x.LevelCode.Equals(level));
        ////    var _escalationCaseData = _escalationSetUpData.SetupCaseDetails?.FirstOrDefault(y => y.SetupCaseCode == caseCode);

        ////    if (_escalationCaseData?.SetupCaseOption ?? false)
        ////    {
        ////        if (_escalationlevelData?.Emailaddress?.Count > 0)
        ////        {
        ////            Task.Run(async () => await _emailSender.SendMailWithAttachtment("Sir", _escalationlevelData?.Emailaddress.FirstOrDefault(), filePath, type)).GetAwaiter().GetResult();
        ////        }
        ////    }

        ////    // This method will be executed asynchronously by Hangfire.
        ////}

        public async Task CaseBasedEscalation()
        {
            GeneralSetupDetailsResponse escalationSetupData = await _setupConfigarationContext.gettSetupData();
            //var lev1Criticality = escalationSetupData.EmailEscalationLevelDetails.Where(x => x.LevelCode == EscalationLevel.Level1).FirstOrDefault();
            //var levl2Criticality = escalationSetupData.EmailEscalationLevelDetails.Where(x => x.LevelCode == EscalationLevel.Level2).FirstOrDefault();
            await UnderProcessListMailsend(escalationSetupData, EscalationLevel.Level1);
            await UnderProcessListMailsend(escalationSetupData, EscalationLevel.Level2);
            await UnderProcessListMailsend(escalationSetupData, EscalationLevel.Level3);
            await LinkNotSend(escalationSetupData, EscalationLevel.Level1);
            await LinkNotSend(escalationSetupData, EscalationLevel.Level2);
            await LinkNotSend(escalationSetupData, EscalationLevel.Level3);
        }

        private async Task UnderProcessListMailsend(GeneralSetupDetailsResponse escalationSetupData, string levelType)
        {
            EmailEscalationLevelDetails? _escalationlevelData = escalationSetupData.EmailEscalationLevelDetails.Where(x => x.LevelCode == levelType).FirstOrDefault();
            int filterDays = _escalationlevelData.NoOfDays;
            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            List<AppointeeJobDetails> _underProcessData = await GetUnderProcessedAppointeeList(filterDays);
            string? EscalatedEmailAddress = string.Empty;
            Dictionary<string, List<AppointeeJobDetails>>? _userList = new();
            CaseOptionDetails _noRescaseStatus = GetCaseOption(escalationSetupData, levelType, EscalationCase.NoResponse);
            if (_noRescaseStatus?.SetupCaseOption ?? false)
            {
                List<AppointeeJobDetails> _noResData = _underProcessData.Where(x => x.StatusCode == 0 && x.dateOfJoining > _currDate).ToList();

                EscalatedEmailAddress = string.IsNullOrEmpty(_noRescaseStatus.CaseEmailAddress) ? _escalationlevelData.Emailaddress.FirstOrDefault() : _noRescaseStatus.CaseEmailAddress;
                if (_noResData.Count > 0)
                {
                    _userList = generateCaseBasedData(levelType, _userList, _noResData, EscalatedEmailAddress, ValidationType.NoResponse);
                }
            }
            CaseOptionDetails _processingcaseStatus = GetCaseOption(escalationSetupData, levelType, EscalationCase.ResponsedNotSubmitted);
            if (_processingcaseStatus?.SetupCaseOption ?? false)
            {
                EscalatedEmailAddress = string.IsNullOrEmpty(_processingcaseStatus.CaseEmailAddress) ? _escalationlevelData.Emailaddress.FirstOrDefault() : _processingcaseStatus.CaseEmailAddress;
                List<AppointeeJobDetails> _processingData = _underProcessData.Where(x => x.StatusCode != 0 && x.dateOfJoining > _currDate).ToList();
                //  var _getProceesingData = GetUnderProcessData(_processingData);

                if (_processingData.Count > 0)
                {
                    _userList = generateCaseBasedData(levelType, _userList, _processingData, EscalatedEmailAddress, ValidationType.Processing);
                }
            }
        }

        private async Task LinkNotSend(GeneralSetupDetailsResponse escalationSetupData, string levelType)
        {
            EmailEscalationLevelDetails? _escalationlevelData = escalationSetupData.EmailEscalationLevelDetails.Where(x => x.LevelCode == levelType).FirstOrDefault();
            int filterDays = _escalationlevelData.NoOfDays;
            CaseOptionDetails _noLinkSendStatus = GetCaseOption(escalationSetupData, levelType, EscalationCase.NoLinkSend);
            if (_noLinkSendStatus?.SetupCaseOption ?? false)
            {
                Dictionary<string, List<AppointeeJobDetails>>? _userList = new();
                DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
                DateTime startDate = filterDays > 0 ? _currDate.AddDays(-filterDays) : DateTime.MinValue;
                AppointeeSeacrhFilterRequest reqObj = new()
                {
                    FromDate = startDate,
                };
                List<UnProcessedFileData> nolinkdata = await _dbContextWorkflow.GetUnProcessDataAsync(reqObj);
                string? EscalatedEmailAddress = string.IsNullOrEmpty(_noLinkSendStatus.CaseEmailAddress) ? _escalationlevelData.Emailaddress.FirstOrDefault() : _noLinkSendStatus.CaseEmailAddress;
                if (nolinkdata.Count > 0)
                {
                    List<AppointeeJobDetails>? _NoLinkViewdata = nolinkdata?.Select(row => new AppointeeJobDetails
                    {
                        appointeeName = row?.AppointeeName,
                        appointeeEmailId = row?.AppointeeEmailId,
                        mobileNo = row?.MobileNo,
                        dateOfOffer = row?.DateOfOffer,
                        dateOfJoining = row?.DateOfJoining,
                        Status = "Link Not Sent",
                        CreatedDate = row?.CreatedOn,
                        candidateId = row?.CandidateId,
                        lvl1Email = row?.lvl1Email,
                        lvl2Email = row?.lvl2Email,
                        lvl3Email = row?.lvl3Email,
                    }).ToList();
                    _userList = generateCaseBasedData(levelType, _userList, _NoLinkViewdata, EscalatedEmailAddress, ValidationType.NoLinkSent);
                }
            }
        }

        private async Task<List<AppointeeJobDetails>> GetUnderProcessedAppointeeList(int FilterDays)
        {
            int filterdaysrange = FilterDays;
            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            //var startDate = _currDate.AddDays(-filterdaysrange);
            DateTime CurrDate = Convert.ToDateTime(_currDate);
            DateTime startDate = FilterDays > 0 ? _currDate.AddDays(-filterdaysrange) : DateTime.MinValue;

            AppointeeSeacrhFilterRequest reqObj = new()
            {
                FromDate = startDate,
            };
            List<UnderProcessQueryDataResponse> underProcessData = await _dbContextWorkflow.GetUnderProcessDataAsync(reqObj);

            List<AppointeeJobDetails>? _underProcessViewdata = underProcessData?.Where(X => X.IsJoiningDateLapsed)?.DistinctBy(x => x.AppointeeId).Select(row => new AppointeeJobDetails
            {
                appointeeName = row?.AppointeeDetails?.AppointeeName ?? row?.UnderProcess?.AppointeeName,
                appointeeId = row?.UnderProcess?.AppointeeId,
                appointeeEmailId = row?.AppointeeDetails?.AppointeeEmailId ?? row?.UnderProcess?.AppointeeEmailId,
                mobileNo = row?.UnderProcess?.MobileNo,
                isPFverificationReq = row?.UnderProcess?.IsPFverificationReq,
                epfWages = row?.AppointeeDetails?.EPFWages ?? row?.UnderProcess?.EPFWages,
                dateOfOffer = row?.UnderProcess?.DateOfOffer,
                dateOfJoining = row?.AppointeeDetails?.DateOfJoining ?? row?.UnderProcess?.DateOfJoining,
                isDocSubmitted = row?.AppointeeDetails?.IsSubmit ?? false,
                isReprocess = false,
                Status = row?.AppointeeDetails?.IsSubmit ?? false ? "Ongoing" : row?.AppointeeDetails?.SaveStep == 1 ? "Ongoing" : "No Response",
                StatusCode = row?.AppointeeDetails?.IsSubmit ?? false ? 2 : row?.AppointeeDetails?.SaveStep ?? 0,
                CreatedDate = row?.UnderProcess?.CreatedOn,
                lvl1Email = row?.UnderProcess?.lvl1Email,
                lvl2Email = row?.UnderProcess?.lvl2Email,
                lvl3Email = row?.UnderProcess?.lvl3Email,
            }).OrderByDescending(x => x.isDocSubmitted).ThenBy(y => y.dateOfJoining).ToList();

            return _underProcessViewdata;
        }
    }
}