using Microsoft.EntityFrameworkCore;
using PfcAPI.Model.DataAccess;
using PfcAPI.Model.ResponseModel;
using System.Data;
using System.Net;
using VERIDATA.BLL.Interfaces;
using VERIDATA.BLL.Notification.Provider;
using VERIDATA.BLL.utility;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Response;
using static PfcAPI.Infrastucture.CommonEnum;

namespace PfcAPI.Services
{
    public class WorkerService : IWorkerService
    {
        //private const int generalDelay = 1 * 10 * 1000; // 10 seconds
        private readonly ILogger<WorkerService> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IFileContext _fileService;
        private readonly IWorkFlowContext _workflowcontext;
        private readonly ISetupConfigarationContext _setupConfigarationContext;
        private readonly DbContextDB _context;
        public WorkerService(ILogger<WorkerService> logger, IEmailSender emailSender, IFileContext fileService, IWorkFlowContext workflowcontext, ISetupConfigarationContext setupConfigarationContext, DbContextDB dbContextClass)
        {
            _logger = logger;
            _context = dbContextClass;
            _emailSender = emailSender;
            _fileService = fileService;
            _workflowcontext = workflowcontext;
            _setupConfigarationContext = setupConfigarationContext;
        }

        //public void DoSomething()
        //{
        //    var name = CommonUtility.GenarateUserName("ap", 1);
        //    Console.WriteLine("Welcome to Shopping World!");
        //    var testdata = CommonUtility.CustomEncryptString("AAECAwQFBgcICQoLDA0ODw==", "123456789012");
        //    var data = CommonUtility.MaskedString("");
        //    var userList = new List<UserDetails>();
        //    var user = new UserDetails
        //    {
        //        UserName = "TestTanmoy",
        //        UserCode = CommonUtility.GenarateUserName("TestTanmoy", 1),
        //        Password = CommonUtility.RandomString(8),
        //        EmailId = "pfcsrver005@gmail.com",
        //        Phone = "",
        //        CompanyId = 1,
        //        UserTypeId = (int)UserType.Appoientee,
        //        //user.roleId = obj.companyId; //ToDO
        //        AppointeeId = 1
        //    };
        //    userList.Add(user);
        //    Task.Run(async () => await _emailSender.SendAppointeeLoginMail(userList)).GetAwaiter().GetResult();
        //    // Your background job logic here
        //    // This method will be executed asynchronously by Hangfire.
        //}
        public async Task apiCountMailAsync()
        {
            DateTime _currDate = DateTime.Today;
            // Your background job logic here

            var totalApiList = await _context.ApiCounter.Where(m => m.CreatedOn > _currDate).ToListAsync();

            var TotalApiCount = totalApiList?.Where(x => x?.Type == "Request")?.GroupBy(x => x.ApiName)?.Select(y => new ApiCountJobResponse
            {
                ApiName = y.Key?.ToLower(),
                TotalApiCount = y?.ToList()?.Count() ?? 0,
                TotalSuccessApiCount = 0
            })?.ToList();

            var TotalResponse = totalApiList?.Where(x => x?.Type == "Response")?.ToList();

            var TotalSuccessApiCount = TotalResponse?.Where(x => x?.Status == (Int32)HttpStatusCode.OK)?.GroupBy(x => x.ApiName)?.Select(y => new ApiCountJobResponse
            {
                ApiName = y?.Key,
                TotalSuccessApiCount = y?.ToList()?.Count() ?? 0,
            })?.ToList();

            var TotalUnproceesbleApiCount = TotalResponse?.Where(x => x?.Status == (Int32)HttpStatusCode.UnprocessableEntity)?.GroupBy(x => x.ApiName)?.Select(y => new ApiCountJobResponse
            {
                ApiName = y?.Key,
                TotalUnprocessableEntityCount = y?.ToList()?.Count() ?? 0,
                //TotalApiCount = TotalApiCount?.Where(x => x.ApiName?.ToLower() == y?.Key?.ToLower())?.ToList()?.Count() ?? 0
            })?.ToList();

            var TotalFaliureApiCount = TotalResponse?.Where(x => !(x?.Status == (int)HttpStatusCode.UnprocessableEntity || x?.Status == (int)HttpStatusCode.OK))?.GroupBy(x => x.ApiName)?.Select(y => new ApiCountJobResponse
            {
                ApiName = y?.Key,
                TotalFailureCount = y?.ToList()?.Count() ?? 0,
            })?.ToList();

            foreach (var obj in TotalApiCount)
            {
                obj.Date = _currDate.ToShortDateString();
                obj.TotalSuccessApiCount = TotalSuccessApiCount?.Where(x => x.ApiName?.ToLower() == obj?.ApiName?.ToLower())?.FirstOrDefault()?.TotalSuccessApiCount ?? 0;
                obj.TotalUnprocessableEntityCount = TotalUnproceesbleApiCount?.Where(x => x.ApiName?.ToLower() == obj?.ApiName?.ToLower())?.FirstOrDefault()?.TotalUnprocessableEntityCount ?? 0;
                obj.TotalFailureCount = TotalFaliureApiCount?.Where(x => x.ApiName?.ToLower() == obj?.ApiName?.ToLower())?.FirstOrDefault()?.TotalFailureCount ?? 0;
            }
            // var apilist = querydata.ToList();
            if (TotalApiCount?.Count() > 0)
            {
                var _exportdt = CommonUtility.ToDataTable<ApiCountJobResponse>(TotalApiCount);
                var filedata = Task.Run(async () => await _fileService.GenerateDataTableTofile(_exportdt, "Report", ValidationType.ApiCount)).GetAwaiter().GetResult();
                var attachtData = new List<Filedata>() { filedata };
                Task.Run(async () => await _emailSender.SendMailWithAttachtment("Tanmoy", "pfcsrver005@gmail.com", attachtData, ValidationType.ApiCount)).GetAwaiter().GetResult();

            }

            // This method will be executed asynchronously by Hangfire.
        }
        public async Task ApponteeCountMailAsync()
        {
            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            var appointeeCount = await _context.UploadAppointeeCounter?.Where(x => x.CreatedOn >= _currDate)?.ToListAsync();
            var totalAppointee = appointeeCount.Sum(x => x.Count);
            var dataList = new List<CountJobResponse>();
            var data = new CountJobResponse
            {
                Name = $"Total number of appointee added in {_currDate}",
                Value = totalAppointee.ToString(),
            };
            dataList.Add(data);
            var _exportdt = CommonUtility.ToDataTable<CountJobResponse>(dataList);
            var filedata = Task.Run(async () => await _fileService.GenerateDataTableTofile(_exportdt, "Report", ValidationType.AppointeeCount)).GetAwaiter().GetResult();
            var attachtData = new List<Filedata>() { filedata };
            Task.Run(async () => await _emailSender.SendMailWithAttachtment("Tanmoy", "pfcsrver005@gmail.com", attachtData, ValidationType.AppointeeCount)).GetAwaiter().GetResult();

        }
        public async Task criticalAppointeeMail()
        {
            var escalationSetupData = Task.Run(async () => await _setupConfigarationContext.gettSetupData()).GetAwaiter().GetResult();
            await criticalAppointeeCaseWise(EscalationCase.DOJ1Week, escalationSetupData);
            await criticalAppointeeCaseWise(EscalationCase.DOJ2Week, escalationSetupData);
        }
        private async Task criticalAppointeeCaseWise(string caseCode, GeneralSetupDetailsResponse escalationSetupData)
        {
            //var escalationSetupData = Task.Run(async () => await _setupConfigarationContext.gettSetupData()).GetAwaiter().GetResult();
            var criticalDays = 0;
            var criticality = ValidationType.Critical1week;
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
            var _escalationlevelData = escalationSetupData?.EmailEscalationLevelDetails?.Where(x => x.LevelCode == levelType)?.FirstOrDefault();
            var _CriticalStatus = GetCaseOption(escalationSetupData, levelType, caseCode);
            if (_CriticalStatus?.SetupCaseOption ?? false)
            {
                var EscalatedEmailAddress = string.IsNullOrEmpty(_CriticalStatus.CaseEmailAddress) ? _escalationlevelData?.Emailaddress.FirstOrDefault() : _CriticalStatus.CaseEmailAddress;
                var datalist = await GetCriticalAppointeeList(criticalDays);
                if (datalist.Count > 0)
                {
                    Dictionary<string, List<AppointeeJobDetails>>? _userList = new Dictionary<string, List<AppointeeJobDetails>>();
                    _userList = generateCaseBasedData(levelType, _userList, datalist, EscalatedEmailAddress, type);
                    //var _exportdt = CommonUtility.ToDataTable<CriticalAppointeeJobResponse>(datalist);
                    //var filePath = Task.Run(async () => await _fileService.SaveDataTabletofile(_exportdt, "report", criticality)).GetAwaiter().GetResult();

                    //escalationMailSend(EscalationLevel.Level1, caseCode, filePath, criticality);
                    //escalationMailSend(EscalationLevel.Level2, caseCode, filePath, criticality);
                }
            }


        }
        //private void escalationMailSend(string level, string caseCode, string filePath, ValidationType type)
        //{
        //    var escalationSetupData = Task.Run(async () => await _setupConfigarationContext.gettSetupData()).GetAwaiter().GetResult();
        //    var _escalationlevelData = escalationSetupData.EmailEscalationLevelDetails.FirstOrDefault(x => x.LevelCode.Equals(level));
        //    var _escalationSetUpData = escalationSetupData.EmailEscalationSetupDetails.FirstOrDefault(x => x.LevelCode.Equals(level));
        //    var _escalationCaseData = _escalationSetUpData.SetupCaseDetails?.FirstOrDefault(y => y.SetupCaseCode == caseCode);

        //    if (_escalationCaseData?.SetupCaseOption ?? false)
        //    {
        //        if (_escalationlevelData?.Emailaddress?.Count > 0)
        //        {
        //            Task.Run(async () => await _emailSender.SendMailWithAttachtment("Sir", _escalationlevelData?.Emailaddress.FirstOrDefault(), filePath, type)).GetAwaiter().GetResult();
        //        }
        //    }



        //    // This method will be executed asynchronously by Hangfire.
        //}
        private void caseBasedEscalationMailSend(string level, string emailaddress, string caseCode, Filedata fileData, ValidationType type)
        {
            if (!string.IsNullOrEmpty(emailaddress))
            {
                var attachedFileData = new List<Filedata> { fileData };
                Task.Run(async () => await _emailSender.SendMailWithAttachtment("Sir", emailaddress, attachedFileData, type)).GetAwaiter().GetResult();
            }
        }
        private CaseOptionDetails GetCaseOption(GeneralSetupDetailsResponse escalationSetupData, string level, string caseCode)
        {
            var _escalationlevelData = escalationSetupData.EmailEscalationLevelDetails.FirstOrDefault(x => x.LevelCode.Equals(level));
            var _escalationSetUpData = escalationSetupData.EmailEscalationSetupDetails.FirstOrDefault(x => x.LevelCode.Equals(level));
            var _escalationCaseData = _escalationSetUpData.SetupCaseDetails?.FirstOrDefault(y => y.SetupCaseCode == caseCode);
            return _escalationCaseData;
        }
        private async Task<List<AppointeeJobDetails>> GetCriticalAppointeeList(int criticalDays)
        {
            //int criticaldata = 0;
            //  var filterdaysrange = _configSetup.CriticalDaysLimit;
            //var generalsetupData = await _context.GeneralSetup.Where(x => x.ActiveStatus == true).ToListAsync();
            //var filterdaysrange = generalsetupData.FirstOrDefault()?.CriticalNoOfDays ?? 0;
            var filterdaysrange = criticalDays;
            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            var maxDate = _currDate.AddDays(filterdaysrange);
            var actionRequiredListdata = new List<AppointeeJobDetails>();
            var _getapprovalStatus = await _context.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToListAsync();
            var ApprovedState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Approved?.Trim());
            var CloseState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ProcessClose?.Trim());
            var RejectedState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Rejected?.Trim());

            var querydata = from b in _context.UnderProcessFileData
                            join w in _context.WorkFlowDetails
                            on b.AppointeeId equals w.AppointeeId
                            join p in _context.AppointeeDetails
                            on b.AppointeeId equals p.AppointeeId into grouping
                            from p in grouping //.Where(x => !x.ProcessStatus.GetValueOrDefault().Equals(ReprocessState.AppvlStatusId)).DefaultIfEmpty()
                            where w.AppvlStatusId != CloseState.AppvlStatusId
                            & w.AppvlStatusId != RejectedState.AppvlStatusId
                            & w.AppvlStatusId != ApprovedState.AppvlStatusId
                            & b.DateOfJoining <= maxDate & b.DateOfJoining >= _currDate & b.ActiveStatus == true & p.ActiveStatus == true
                            orderby p.IsSubmit
                            select new { b, p, w.AppvlStatusId };

            var UnderProcessData = await querydata.OrderByDescending(x => x.p.DateOfJoining).ToListAsync().ConfigureAwait(false);
            //  var UnderProcessData = await _dbContextClass.UnderProcessFileData.Where(m => m.DateOfJoining <= maxDate && m.ActiveStatus == true).ToListAsync();
            var NonProcessData = await _context.UnProcessedFileData.Where(m => m.DateOfJoining <= maxDate && m.DateOfJoining >= _currDate && m.ActiveStatus == true).ToListAsync();
            var _underProcessViewdata = UnderProcessData.Select(row => new AppointeeJobDetails
            {


                candidateId = row.p?.CandidateId ?? row.b.CandidateId,
                appointeeName = row.p?.AppointeeName ?? row.b.AppointeeName,
                appointeeEmailId = row.p?.AppointeeEmailId ?? row.b.AppointeeEmailId,
                mobileNo = row.b.MobileNo,
                dateOfJoining = (row.p?.DateOfJoining ?? row.b.DateOfJoining),
                Status = row.p?.IsSubmit ?? false ? "Ongoing" : row.p?.SaveStep == 1 ? "Ongoing" : "No Response",
                DaysToJoin = (Convert.ToInt32(((row.p?.DateOfJoining ?? row.b.DateOfJoining) - DateTime.Now)?.TotalDays ?? 0)).ToString(),
                CreatedDate = row.b?.CreatedOn,
                lvl1Email = row.b?.lvl1Email,
                lvl2Email = row.b?.lvl2Email,
                lvl3Email = row.b?.lvl3Email,
            })?.ToList();
            var _unProcessViewdata = NonProcessData.Select(row => new AppointeeJobDetails
            {
                candidateId = row.CandidateId,
                appointeeName = row.AppointeeName,
                appointeeEmailId = row.AppointeeEmailId,
                mobileNo = row.MobileNo,
                dateOfJoining = row.DateOfJoining,
                Status = "Mail Not sent",
                DaysToJoin = (Convert.ToInt32((row.DateOfJoining - DateTime.Now)?.TotalDays ?? 0)).ToString(),
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
            //criticaldata = UnderProcessData?.Count ?? 0 + NonProcessData?.Count ?? 0;
            return actionRequiredListdata;
        }

        public async Task CaseBasedEscalation()
        {
            var escalationSetupData = Task.Run(async () => await _setupConfigarationContext.gettSetupData()).GetAwaiter().GetResult();
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
            var _escalationlevelData = escalationSetupData.EmailEscalationLevelDetails.Where(x => x.LevelCode == levelType).FirstOrDefault();
            var filterDays = _escalationlevelData.NoOfDays;
            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            var _underProcessData = await GetUnderProcessedAppointeeList(filterDays);
            var EscalatedEmailAddress = string.Empty;
            Dictionary<string, List<AppointeeJobDetails>>? _userList = new Dictionary<string, List<AppointeeJobDetails>>();
            var _noRescaseStatus = GetCaseOption(escalationSetupData, levelType, EscalationCase.NoResponse);
            if (_noRescaseStatus?.SetupCaseOption ?? false)
            {
                var _noResData = _underProcessData.Where(x => x.StatusCode == 0 & x.dateOfJoining > _currDate).ToList();

                EscalatedEmailAddress = string.IsNullOrEmpty(_noRescaseStatus.CaseEmailAddress) ? _escalationlevelData.Emailaddress.FirstOrDefault() : _noRescaseStatus.CaseEmailAddress;
                if (_noResData.Count > 0)
                {
                    _userList = generateCaseBasedData(levelType, _userList, _noResData, EscalatedEmailAddress, ValidationType.NoResponse);

                }
            }
            var _processingcaseStatus = GetCaseOption(escalationSetupData, levelType, EscalationCase.ResponsedNotSubmitted);
            if (_processingcaseStatus?.SetupCaseOption ?? false)
            {
                EscalatedEmailAddress = string.IsNullOrEmpty(_processingcaseStatus.CaseEmailAddress) ? _escalationlevelData.Emailaddress.FirstOrDefault() : _processingcaseStatus.CaseEmailAddress;
                var _processingData = _underProcessData.Where(x => x.StatusCode != 0 & x.dateOfJoining > _currDate).ToList();
                //  var _getProceesingData = GetUnderProcessData(_processingData);

                if (_processingData.Count > 0)
                {
                    _userList = generateCaseBasedData(levelType, _userList, _processingData, EscalatedEmailAddress, ValidationType.Processing);

                }
            }

        }


        private async Task LinkNotSend(GeneralSetupDetailsResponse escalationSetupData, string levelType)
        {
            var _escalationlevelData = escalationSetupData.EmailEscalationLevelDetails.Where(x => x.LevelCode == levelType).FirstOrDefault();
            var filterDays = _escalationlevelData.NoOfDays;
            var _noLinkSendStatus = GetCaseOption(escalationSetupData, levelType, EscalationCase.NoLinkSend);
            if (_noLinkSendStatus?.SetupCaseOption ?? false)
            {
                Dictionary<string, List<AppointeeJobDetails>>? _userList = new Dictionary<string, List<AppointeeJobDetails>>();
                var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
                var startDate = filterDays > 0 ? _currDate.AddDays(-filterDays) : DateTime.MinValue;
                var nolinkdata = await _context.UnProcessedFileData.Where(m => m.CreatedOn >= startDate && m.ActiveStatus == true).ToListAsync();
                var EscalatedEmailAddress = string.IsNullOrEmpty(_noLinkSendStatus.CaseEmailAddress) ? _escalationlevelData.Emailaddress.FirstOrDefault() : _noLinkSendStatus.CaseEmailAddress;
                if (nolinkdata.Count > 0)
                {
                    var _NoLinkViewdata = nolinkdata?.Select(row => new AppointeeJobDetails
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
            foreach (var obj in _userList)
            {
                var mailaddress = obj.Key;
                var _data = obj.Value?.ToList();
                var _exportdt = new DataTable();
                if (type == ValidationType.Critical2week || type == ValidationType.Critical1week)
                {
                    var _getViewData = GetCriticalProcessData(_data);
                    _exportdt = CommonUtility.ToDataTable<CriticalAppointeeJobResponse>(_getViewData);
                }
                else
                {
                    var _getViewData = GetUnderProcessData(_data);
                    _exportdt = CommonUtility.ToDataTable<UnderProcessJobResponse>(_getViewData);
                }


                var fileDetails = Task.Run(async () => await _fileService.GenerateDataTableTofile(_exportdt, "report", type)).GetAwaiter().GetResult();
                caseBasedEscalationMailSend(levelType, mailaddress, EscalationCase.NoResponse, fileDetails, type);
            }

            return _userList;
        }

        private List<UnderProcessJobResponse> GetUnderProcessData(List<AppointeeJobDetails> appointeeList)
        {

            var _underProcessViewdata = appointeeList?.Select(row => new UnderProcessJobResponse
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
        private List<CriticalAppointeeJobResponse> GetCriticalProcessData(List<AppointeeJobDetails> appointeeList)
        {

            var _criticalViewdata = appointeeList.Select(row => new CriticalAppointeeJobResponse
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
        private async Task<List<AppointeeJobDetails>> GetUnderProcessedAppointeeList(int FilterDays)
        {
            var filterdaysrange = FilterDays;
            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            //var startDate = _currDate.AddDays(-filterdaysrange);
            DateTime CurrDate = Convert.ToDateTime(_currDate);
            var startDate = FilterDays > 0 ? _currDate.AddDays(-filterdaysrange) : DateTime.MinValue;
            var _getapprovalStatus = await _context.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToArrayAsync();
            var ReprocessState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Reprocess?.Trim());
            var CloseState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ProcessClose?.Trim());


            var querydata = from b in _context.UnderProcessFileData
                            join w in _context.WorkFlowDetails
                            on b.AppointeeId equals w.AppointeeId
                            join p in _context.AppointeeDetails
                            on b.AppointeeId equals p.AppointeeId into grouping
                            from p in grouping.Where(x => !x.ProcessStatus.GetValueOrDefault().Equals(ReprocessState.AppvlStatusId)).DefaultIfEmpty()
                            where w.AppvlStatusId != CloseState.AppvlStatusId & (p.IsProcessed.Equals(false) || p.IsProcessed == null)
                            & b.ActiveStatus == true & w.CreatedOn >= startDate
                            & b.DateOfJoining >= CurrDate
                            orderby p.IsSubmit
                            select new { b.AppointeeId, b, p, w.AppvlStatusId };

            var list = await querydata.ToListAsync().ConfigureAwait(false);

            var _underProcessViewdata = list?.DistinctBy(x => x.AppointeeId).Select(row => new AppointeeJobDetails
            {
                //id = row.b.UnderProcessId,
                //fileId = row.b.FileId,
                //companyId = row.b.CompanyId,
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
                Status = row.p?.IsSubmit ?? false ? "Ongoing" : row.p?.SaveStep == 1 ? "Ongoing" : "No Response",
                StatusCode = row.p?.IsSubmit ?? false ? 2 : row.p?.SaveStep ?? 0,
                CreatedDate = row.b?.CreatedOn,
                lvl1Email = row.b?.lvl1Email,
                lvl2Email = row.b?.lvl2Email,
                lvl3Email = row.b?.lvl3Email,
            }).OrderByDescending(x => x.isDocSubmitted).ThenBy(y => y.dateOfJoining).ToList();


            return _underProcessViewdata;
        }


    }
}
