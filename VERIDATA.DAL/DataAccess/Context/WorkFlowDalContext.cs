using Microsoft.EntityFrameworkCore;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.DAL.DBContext;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Admin;
using VERIDATA.Model.Table.Config;
using VERIDATA.Model.Table.Master;
using VERIDATA.Model.Table.Public;
using static VERIDATA.DAL.utility.CommonEnum;

namespace VERIDATA.DAL.DataAccess.Context
{
    public class WorkFlowDalContext : IWorkFlowDalContext
    {
        private readonly DbContextDalDB _dbContextClass;
        private readonly IActivityDalContext _dbContextDalActivity;
        public WorkFlowDalContext(DbContextDalDB dbContextClass, IActivityDalContext dbContextDalActivity)
        {
            _dbContextClass = dbContextClass;
            _dbContextDalActivity = dbContextDalActivity;
        }
        public async Task<WorkflowApprovalStatusMaster?> GetApprovalState(string approvalStatus)
        {
            WorkflowApprovalStatusMaster? approvalState = new();

            approvalState = await _dbContextClass.WorkflowApprovalStatusMaster.FirstOrDefaultAsync(x => x.AppvlStatusCode.Equals(approvalStatus) && x.ActiveStatus == true);
            return approvalState;
        }
        public async Task<WorkFlowDetails?> GetCurrentApprovalStateByAppointeeId(int appointeeId)
        {
            WorkFlowDetails? getcurrentState = new();
            getcurrentState = await _dbContextClass.WorkFlowDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId) && x.ActiveStatus == true);
            return getcurrentState;
        }
        public async Task<int> GetWorkFlowStateIdByAlias(string StateAlias)
        {
            WorkFlowStateMaster? getData = await _dbContextClass.WorkFlowStateMaster.FirstOrDefaultAsync(x => x.StateAlias.Equals(StateAlias) && x.ActiveStatus == true);

            return getData?.StateId ?? 0;
        }
        public async Task<List<ProcessedDataDetailsResponse>> GetProcessedAppointeeDetailsAsync(ProcessedFilterRequest filter)
        {
            WorkflowApprovalStatusMaster _processClosed = await GetApprovalState(WorkFlowType.ProcessClose);
            WorkflowApprovalStatusMaster _filteredStatus = new();
            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime? _ToDate = filter.ToDate != null ? filter.ToDate?.AddDays(1) : null;
            DateTime? startDate = filter.FromDate;
            if (!string.IsNullOrEmpty(filter.ProcessStatus))
            {
                _filteredStatus = await GetApprovalState(filter.ProcessStatus);
            }
            if (!(filter.IsFiltered ?? false))
            {
                int _filterDays = filter.NoOfDays ?? 0;

                startDate = _filterDays > 0 ? _currDate.AddDays(-_filterDays) : null;
            }
            IQueryable<ProcessedDataDetailsResponse> querydata = from p in _dbContextClass.ProcessedFileData
                                                                 join u in _dbContextClass.UnderProcessFileData
                                                                    on p.AppointeeId equals u.AppointeeId
                                                                 join w in _dbContextClass.WorkFlowDetails
                                                                         on p.AppointeeId equals w.AppointeeId
                                                                 join x in _dbContextClass.AppointeeDetails
                                                                         on p.AppointeeId equals x.AppointeeId into grouping
                                                                 from a in grouping.DefaultIfEmpty()
                                                                 where p.ActiveStatus == true
&& (startDate == null || p.CreatedOn >= startDate)
&& (filter.ToDate == null || p.CreatedOn < _ToDate)
&& (filter.IsPfRequired == null || a.IsPFverificationReq == filter.IsPfRequired)
&& (string.IsNullOrEmpty(filter.ProcessStatus) || w.AppvlStatusId == _filteredStatus.AppvlStatusId)
&& (string.IsNullOrEmpty(filter.AppointeeName) || u.AppointeeName.Contains(filter.AppointeeName))
&& (string.IsNullOrEmpty(filter.CandidateId) || u.CandidateId.Contains(filter.CandidateId))
                                                                 select new ProcessedDataDetailsResponse
                                                                 {

                                                                     AppointeeData = a,
                                                                     CompanyId = u.CompanyId,
                                                                     CandidateId = u.CandidateId,
                                                                     AppointeeName = u.AppointeeName,
                                                                     AppointeeId = u.AppointeeId,
                                                                     AppointeeEmailId = u.AppointeeEmailId,
                                                                     MobileNo = u.MobileNo,
                                                                     DateOfJoining = u.DateOfJoining,
                                                                     EpfWages = u.EPFWages,
                                                                     ProcessedId = p.ProcessedId,
                                                                     StateAlias = w.StateAlias,
                                                                 };

            List<ProcessedDataDetailsResponse> appointeelist = await querydata.ToListAsync().ConfigureAwait(false);
            return appointeelist;
        }
        public async Task<List<RejectedDataDetailsResponse>> GetRejectedAppointeeDetailsAsync(FilterRequest filter)
        {
            DateTime? _ToDate = filter.ToDate != null ? filter.ToDate?.AddDays(1) : null;

            IQueryable<RejectedDataDetailsResponse> querydata = from r in _dbContextClass.RejectedFileData
                                                                join u in _dbContextClass.UnderProcessFileData
                                                                    on r.AppointeeId equals u.AppointeeId
                                                                join rp in _dbContextClass.AppointeeReasonMappingData
                                                                    on u.AppointeeId equals rp.AppointeeId
                                                                join rm in _dbContextClass.ReasonMaser
                                                                    on rp.ReasonId equals rm.ReasonId
                                                                join x in _dbContextClass.AppointeeDetails
                                                                    on r.AppointeeId equals x.AppointeeId into grouping
                                                                from a in grouping.DefaultIfEmpty()
                                                                where r.ActiveStatus == true && rm.ActiveStatus == true && rp.ActiveStatus == true
&& (string.IsNullOrEmpty(filter.AppointeeName) || u.AppointeeName.Contains(filter.AppointeeName))
&& (string.IsNullOrEmpty(filter.CandidateId) || u.CandidateId.Contains(filter.CandidateId))
&& (filter.FromDate == null || r.CreatedOn >= filter.FromDate)
&& (filter.ToDate == null || r.CreatedOn < _ToDate)
                                                                select new RejectedDataDetailsResponse
                                                                {
                                                                    CompanyId = u.CompanyId,
                                                                    CandidateId = u.CandidateId,
                                                                    AppointeeId = u.AppointeeId,
                                                                    AppointeeName = u.AppointeeName,
                                                                    DateOfBirth = a.DateOfBirth,
                                                                    MobileNo = u.MobileNo,
                                                                    AppointeeEmailId = u.AppointeeEmailId,
                                                                    DateOfJoining = u.DateOfJoining,
                                                                    Nationality = a.Nationality,
                                                                    Qualification = a.Qualification,
                                                                    UANNumber = a.UANNumber,
                                                                    EpfWages = u.EPFWages,
                                                                    Gender = a.Gender,
                                                                    MaratialStatus = a.MaratialStatus,
                                                                    MemberName = a.MemberName,
                                                                    MemberRelation = a.MemberRelation,
                                                                    IsHandicap = a.IsHandicap,
                                                                    HandicapeType = a.HandicapeType,
                                                                    IsInternationalWorker = a.IsInternationalWorker,
                                                                    PassportNo = a.PassportNo,
                                                                    PassportValidFrom = a.PassportValidFrom,
                                                                    PassportValidTill = a.PassportValidTill,
                                                                    OriginCountry = a.OriginCountry,
                                                                    PANName = a.PANName,
                                                                    PANNumber = a.PANNumber,
                                                                    AadhaarName = a.AadhaarName,
                                                                    AadhaarNumberView = a.AadhaarNumberView,
                                                                    AadhaarNumber = a.AadhaarNumberView,
                                                                    //AadhaarNumber = a.AadhaarNumber,
                                                                    Remarks = rp.Remarks,
                                                                    RejectedId = r.RejectedId,
                                                                };

            List<RejectedDataDetailsResponse> rejectedAppointeeList = await querydata.ToListAsync().ConfigureAwait(false);
            return rejectedAppointeeList;
        }
        public async Task<List<UnderProcessQueryDataResponse>> GetUnderProcessDataAsync(AppointeeSeacrhFilterRequest reqObj)
        {
            string CurrDate = DateTime.Now.ToShortDateString();
            DateTime _CurrDate = Convert.ToDateTime(CurrDate);
            DateTime? _ToDate = reqObj.ToDate != null ? reqObj.ToDate?.AddDays(1) : null;
            List<WorkflowApprovalStatusMaster> _getapprovalStatus = await _dbContextClass.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToListAsync();
            WorkflowApprovalStatusMaster? ReprocessState = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Reprocess?.Trim());
            WorkflowApprovalStatusMaster? CloseState = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ProcessClose?.Trim());
            WorkflowApprovalStatusMaster? RejectState = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Rejected?.Trim());
            WorkflowApprovalStatusMaster? ApproveState = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Approved?.Trim());


            IQueryable<UnderProcessQueryDataResponse> querydata = from b in _dbContextClass.UnderProcessFileData
                                                                  join w in _dbContextClass.WorkFlowDetails
                                                                  on b.AppointeeId equals w.AppointeeId
                                                                  join p in _dbContextClass.AppointeeDetails
                                                                  on b.AppointeeId equals p.AppointeeId into grouping
                                                                  from p in grouping.Where(x => x.ProcessStatus.GetValueOrDefault() != ReprocessState.AppvlStatusId).DefaultIfEmpty()
                                                                  join c in _dbContextClass.AppointeeConsentMapping
                                                                  on b.AppointeeId equals c.AppointeeId into consentgrouping
                                                                  from c in consentgrouping.Where(x => x.ActiveStatus == true).DefaultIfEmpty()
                                                                  where (!(w.AppvlStatusId == CloseState.AppvlStatusId || w.AppvlStatusId == ApproveState.AppvlStatusId || w.AppvlStatusId == RejectState.AppvlStatusId))
                                                                  && (p.IsProcessed.Equals(false) || p.IsProcessed == null)
                                                                  && (string.IsNullOrEmpty(reqObj.AppointeeName) || b.AppointeeName.Contains(reqObj.AppointeeName))
                                                                  && (string.IsNullOrEmpty(reqObj.CandidateId) || b.CandidateId.Contains(reqObj.CandidateId))
                                                                  && (reqObj.FromDate == null || b.CreatedOn >= reqObj.FromDate) && (reqObj.ToDate == null || b.CreatedOn < _ToDate)
                                                                  && b.ActiveStatus == true
                                                                  orderby p.IsSubmit
                                                                  select new UnderProcessQueryDataResponse
                                                                  {
                                                                      UnderProcess = b,
                                                                      AppointeeDetails = p,
                                                                      AppvlStatusId = w.AppvlStatusId,
                                                                      AppointeeId = b.AppointeeId,
                                                                      ConsentStatusId=c.ConsentStatus,
                                                                      IsJoiningDateLapsed = b.DateOfJoining < _CurrDate
                                                                  };

            List<UnderProcessQueryDataResponse> list = await querydata.ToListAsync().ConfigureAwait(false);

            return list;
        }
        public async Task<List<UnderProcessQueryDataResponse>> GetUnderProcessDataByDOJAsync(DateTime? startDate, DateTime? endDate, DateTime? FromDate, DateTime? ToDate)
        {
            List<WorkflowApprovalStatusMaster> _getapprovalStatus = await _dbContextClass.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToListAsync();
            WorkflowApprovalStatusMaster? ReprocessState = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Reprocess?.Trim());
            WorkflowApprovalStatusMaster? CloseState = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ProcessClose?.Trim());
            WorkflowApprovalStatusMaster? RejectState = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Rejected?.Trim());
            WorkflowApprovalStatusMaster? ApproveState = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Approved?.Trim());


            IQueryable<UnderProcessQueryDataResponse> querydata = from b in _dbContextClass.UnderProcessFileData
                                                                  join w in _dbContextClass.WorkFlowDetails
                                                                  on b.AppointeeId equals w.AppointeeId
                                                                  join p in _dbContextClass.AppointeeDetails
                                                                  on b.AppointeeId equals p.AppointeeId into grouping
                                                                  from p in grouping.Where(x => x.ProcessStatus.GetValueOrDefault() != ReprocessState.AppvlStatusId).DefaultIfEmpty()
                                                                  where (!(w.AppvlStatusId == CloseState.AppvlStatusId || w.AppvlStatusId == ApproveState.AppvlStatusId || w.AppvlStatusId == RejectState.AppvlStatusId))
                                                                    && (p.IsProcessed.Equals(false) || p.IsProcessed == null)
                                                                    && (FromDate == null || b.CreatedOn >= FromDate) && (ToDate == null || b.CreatedOn < ToDate)
                                                                    && (startDate == null || b.DateOfJoining >= startDate)
                                                                    && (endDate == null || b.DateOfJoining <= endDate)
                                                                    && b.ActiveStatus == true
                                                                  orderby p.IsSubmit
                                                                  select new UnderProcessQueryDataResponse
                                                                  {
                                                                      UnderProcess = b,
                                                                      AppointeeDetails = p,
                                                                      AppvlStatusId = w.AppvlStatusId,
                                                                      AppointeeId = b.AppointeeId,
                                                                      IsJoiningDateLapsed = b.DateOfJoining < startDate
                                                                  };

            List<UnderProcessQueryDataResponse> list = await querydata.ToListAsync().ConfigureAwait(false);

            return list;
        }
        public async Task<List<int?>?> GetTotalOfferAppointeeList(int FilterDays)
        {
            int filterdaysrange = FilterDays;
            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime? startDate = filterdaysrange > 0 ? _currDate.AddDays(-filterdaysrange) : null;


            var querydata = from w in _dbContextClass.WorkFlowDetailsHist
                            join u in _dbContextClass.UnderProcessFileData
                            on w.AppointeeId equals u.AppointeeId
                            where w.StateId.Equals(1) && w.AppvlStatusId.Equals(1) && u.ActiveStatus.Equals(true) && (startDate == null || w.CreatedOn >= startDate)
                            select new { w };
            var data = await querydata.ToListAsync().ConfigureAwait(false);
            List<WorkFlowDetailsHist> workfdata = data.Select(x => x.w).ToList();
            List<int?>? appinteeList = workfdata?.DistinctBy(x => x.AppointeeId).Select(x => x.AppointeeId).ToList();
            return appinteeList;
        }
        public async Task<int> PostUploadedXSLfileAsync(string? fileName, string? filepath, int? companyid)
        {
            UploadedXSLfile _postobj = new()
            {
                CompanyId = companyid,
                FileName = fileName,
                FilePath = filepath,
                ActiveStatus = true,
                CreatedBy = 1,
                CreatedOn = DateTime.Now
            };

            _ = _dbContextClass.UploadedXSLfile.Add(_postobj);
            _ = await _dbContextClass.SaveChangesAsync();

            int fileId = _postobj.FileId;
            return fileId;
        }
        public async Task PostRawfiledataAsync(RawdataSubmitRequest data)
        {
            int? _userId = data?.UserId;
            int? _companyId = data?.CompanyId;
            int? _fileId = data?.FileId;

            List<RawFileData>? _rawfileDatalist = data?.ApnteFileData?.Select(x => new RawFileData
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
            await _dbContextClass.RawFileData.AddRangeAsync(_rawfileDatalist);

            List<RawFileHistoryData>? _rawfileHistDatalist = data?.ApnteFileData?.Select(x => new RawFileHistoryData
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

            UploadAppointeeCounter _totalrawfileData = new()
            {
                Count = _rawfileDatalist?.Count() ?? 0,
                CreatedBy = _userId,
                CreatedOn = DateTime.Now,
                FileId = _fileId ?? 0
            };
            _ = await _dbContextClass.UploadAppointeeCounter.AddAsync(_totalrawfileData);

            _ = await _dbContextClass.SaveChangesAsync();

        }
        public async Task<List<RawFileData>> GetRawfiledataByIdAsync(int? fileId, int companyId)
        {
            List<RawFileData> _companydetails = new();
            _companydetails = fileId == 0
                ? await _dbContextClass.RawFileData.Where(x => x.ActiveStatus == true).ToListAsync()
                : await _dbContextClass.RawFileData.Where(x => x.ActiveStatus == true && x.FileId.Equals(fileId)).ToListAsync();
            return _companydetails;
        }
        public async Task<List<UnProcessedFileData>> GetUnProcessDataAsync(AppointeeSeacrhFilterRequest reqObj)
        {
            List<UnProcessedFileData> nonProcessData = new();
            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));

            DateTime? startDate = (reqObj.IsFiltered ?? false) && reqObj.NoOfDays > 0 ? _currDate.AddDays(-reqObj.NoOfDays ?? 0) : null;
            DateTime? _ToDate = reqObj.ToDate != null ? reqObj.ToDate?.AddDays(1) : null;

            nonProcessData = await _dbContextClass.UnProcessedFileData.Where(x => x.ActiveStatus == true
            && (startDate == null || x.CreatedOn >= startDate)
            && (string.IsNullOrEmpty(reqObj.AppointeeName) || x.AppointeeName.Contains(reqObj.AppointeeName))
            && (string.IsNullOrEmpty(reqObj.CandidateId) || x.CandidateId.Contains(reqObj.CandidateId))
            && (reqObj.FromDate == null || x.CreatedOn >= reqObj.FromDate)
            && (reqObj.ToDate == null || x.CreatedOn < _ToDate)
                           ).ToListAsync();
            return nonProcessData;
        }
        public async Task<List<RawFileData>> GetRawfiledataAsync()
        {
            List<RawFileData> rawDataList = new();
            rawDataList = await _dbContextClass.RawFileData.Where(m => m.ActiveStatus == true).ToListAsync();
            return rawDataList;
        }

        public async Task<List<RejectedFileData>> GetRejectedDataAsync(DateTime? startDate)
        {
            List<RejectedFileData> rejectedData = await _dbContextClass.RejectedFileData.Where(m => (startDate == null || m.CreatedOn >= startDate) && m.ActiveStatus == true).ToListAsync();

            return rejectedData;
        }
        public async Task<List<UnProcessedFileData>> GetCriticalUnProcessDataAsync(DateTime? startDate, DateTime? endDate, DateTime? FromDate, DateTime? ToDate)
        {
            List<UnProcessedFileData> unProcessedData = await _dbContextClass.UnProcessedFileData.Where(m => m.DateOfJoining <= endDate && m.DateOfJoining >= startDate && m.ActiveStatus == true
                && (FromDate == null || m.CreatedOn >= FromDate) && (ToDate == null || m.CreatedOn < ToDate)).ToListAsync();
            return unProcessedData;
        }
        public async Task<List<WorkFlowDetailsHist>> GetTotalDataAsync(DateTime? startDate)
        {
            List<WorkFlowDetailsHist> totalData = new();

            var querydata = from w in _dbContextClass.WorkFlowDetailsHist
                            join u in _dbContextClass.UnderProcessFileData
                            on w.AppointeeId equals u.AppointeeId
                            where w.StateId.Equals(1) && w.AppvlStatusId.Equals(1) && u.ActiveStatus.Equals(true) && (startDate == null || w.CreatedOn >= startDate)
                            select new { w };
            var data = await querydata.ToListAsync().ConfigureAwait(false);
            totalData = data.Select(x => x.w).ToList();
            return totalData;
        }
        public async Task<List<RawFileDataDetailsResponse>> GetRawfiledetailsByTypeId(List<RawDataRequest> rawDataList, int? userId, int type)
        {
            List<RawFileDataDetailsResponse> _rawData = new();

            if (type == 1)//type 1 for checked
            {
                List<int>? CheckedRawIdList = rawDataList?.Where(x => x.isChecked == true)?.Select(y => y.id).ToList();
                if (CheckedRawIdList?.Count > 0)
                {
                    List<RawFileDataDetailsResponse> _CheckedRawData = await _dbContextClass.RawFileData.Where(x => x.ActiveStatus == true && CheckedRawIdList.Contains(x.RawFileId)).Select(x => new RawFileDataDetailsResponse
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
                List<int>? UnCheckedRawIdList = rawDataList?.Where(x => x.isChecked == false)?.Select(y => y.id).ToList();
                if (UnCheckedRawIdList?.Count > 0)
                {
                    List<RawFileDataDetailsResponse> _UnCheckedRawData = await _dbContextClass.RawFileData.Where(x => x.ActiveStatus == true && UnCheckedRawIdList.Contains(x.RawFileId)).Select(x => new RawFileDataDetailsResponse
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

        public async Task<List<RawFileDataDetailsResponse>> GetNonProcessedDetailsByTypeId(List<RawDataRequest> rawDataList, int? userId)
        {
            List<RawFileDataDetailsResponse> _rawData = new();
            List<int>? CheckedRawIdList = rawDataList?.Where(x => x.isChecked == true)?.Select(y => y.id).ToList();
            if (CheckedRawIdList?.Count > 0)
            {
                List<RawFileDataDetailsResponse> _CheckedRawData = await _dbContextClass.UnProcessedFileData.Where(x => x.ActiveStatus == true && CheckedRawIdList.Contains(x.UnProcessedId)).Select(x => new RawFileDataDetailsResponse
                {
                    companyId = x.CompanyId,
                    id = x.UnProcessedId,
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
            return _rawData;
        }
        public async Task<List<UnderProcessFileData>> PostUnderProcessDataAsync(List<RawFileDataDetailsResponse> underprocessdata, int userId)
        {
            List<UnderProcessFileData> _underProcessList = new();

            foreach ((RawFileDataDetailsResponse obj, AppointeeMaster user) in from obj in underprocessdata
                                                                               let user = new AppointeeMaster()
                                                                               select (obj, user))
            {
                AppointeeMaster appointee = new()
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
                _ = _dbContextClass.AppointeeMaster.Add(appointee);
                _ = await _dbContextClass.SaveChangesAsync();

                UnderProcessFileData _underProcessdata = new()
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
            _ = await _dbContextClass.SaveChangesAsync();
            return _underProcessList;
        }
        public async Task PostNonProcessDataAsync(List<RawFileDataDetailsResponse> unprocessdata, int userId)
        {
            List<UnProcessedFileData> _unProcessdata = unprocessdata.Select(row => new UnProcessedFileData
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
            _ = await _dbContextClass.SaveChangesAsync();
        }
        public async Task RemoveRawDataAsync(List<int> RemoveRawId)
        {
            List<RawFileData> getRawData = await _dbContextClass.RawFileData.Where(x => RemoveRawId.Contains(x.RawFileId)).ToListAsync();

            if (getRawData != null)
            {
                _dbContextClass.RawFileData.RemoveRange(getRawData);
                _ = await _dbContextClass.SaveChangesAsync();
            }
        }
        public async Task RemoveUnprocessedDataAsync(List<int> RemoveRawId)
        {
            List<UnProcessedFileData> getRawData = await _dbContextClass.UnProcessedFileData.Where(x => RemoveRawId.Contains(x.UnProcessedId)).ToListAsync();

            if (getRawData != null)
            {
                _dbContextClass.UnProcessedFileData.RemoveRange(getRawData);
                _ = await _dbContextClass.SaveChangesAsync();
            }
        }
        public async Task AppointeeWorkflowIniAsync(List<int?> appointeeList, int workflowState, int userId)
        {
            WorkflowApprovalStatusMaster approvalState = await GetApprovalState(WorkFlowType.ProcessIni);

            if (appointeeList?.Count() > 0)
            {
                List<WorkFlowDetails> _workFlow_det = await _dbContextClass.WorkFlowDetails.Where(x => appointeeList.Contains(x.AppointeeId) && x.ActiveStatus == true).ToListAsync();
                if (_workFlow_det?.Count > 0)
                {
                    _dbContextClass.WorkFlowDetails.RemoveRange(_workFlow_det);
                    _ = await _dbContextClass.SaveChangesAsync();
                }
                List<WorkFlowDetails> _genarateWorkflowdata = appointeeList.Select(row => new WorkFlowDetails
                {
                    AppointeeId = row,
                    StateId = workflowState,
                    AppvlStatusId = approvalState?.AppvlStatusId ?? 0,
                    Remarks = string.Empty,
                    ReprocessCount = 0,
                    ActionTakenAt = DateTime.Now,
                    ActiveStatus = true,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    StateAlias = WorkFlowType.ProcessIni
                    //IsChecked = null,
                }).ToList();

                List<WorkFlowDetailsHist>? _workFlow_det_his = appointeeList?.Select(x => new WorkFlowDetailsHist
                {
                    AppointeeId = x,
                    StateId = workflowState,
                    AppvlStatusId = approvalState?.AppvlStatusId ?? 0,
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

                _ = await _dbContextClass.SaveChangesAsync();
            }

        }
        public async Task UpdateAppointeeDojByAdmin(CompanySaveAppointeeDetailsRequest appointeeDetails)
        {
            List<AppointeeUpdateLog> updateLogList = new();
            //using var transaction = _dbContextClass.Database.BeginTransaction();
            //try
            //{
            int appointeeId = appointeeDetails.AppointeeId;
            if (appointeeDetails.DateOfJoining != null)
            {
                if (appointeeId != 0)
                {

                    UnderProcessFileData? _appntundrprocessdata = await _dbContextClass.UnderProcessFileData.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId)) ?? null;
                    if (_appntundrprocessdata != null)
                    {
                        updateLogList.Add(new AppointeeUpdateLog { CandidateId = _appntundrprocessdata.CandidateId, UpdateType = "DateOfJoining", UpdateValue = appointeeDetails.DateOfJoining?.ToString(), CreatedBy = appointeeDetails.UserId, CreatedOn = DateTime.Now });

                        //_appntundrprocessdata.EPFWages = appointeeDetails?.EPFWages ?? _appntundrprocessdata.EPFWages;
                        _appntundrprocessdata.DateOfJoining = appointeeDetails?.DateOfJoining;
                        _appntundrprocessdata.UpdatedBy = appointeeDetails?.UserId;
                        _appntundrprocessdata.UpdatedOn = DateTime.Now;

                    }
                    AppointeeDetails? _appnteDetailsData = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId)) ?? null;
                    if (_appnteDetailsData != null)
                    {
                        _appnteDetailsData.DateOfJoining = appointeeDetails?.DateOfJoining;
                        _appnteDetailsData.UpdatedBy = appointeeDetails?.UserId;
                        _appnteDetailsData.UpdatedOn = DateTime.Now;

                    }
                }
                else
                {
                    UnProcessedFileData? _appntNonprocessdata = await _dbContextClass.UnProcessedFileData.FirstOrDefaultAsync(x => x.CandidateId == appointeeDetails.CandidateId) ?? null;
                    if (_appntNonprocessdata != null)
                    {
                        updateLogList.Add(new AppointeeUpdateLog { CandidateId = _appntNonprocessdata.CandidateId, UpdateType = "DateOfJoining", UpdateValue = appointeeDetails.DateOfJoining?.ToString(), CreatedBy = appointeeDetails.UserId, CreatedOn = DateTime.Now });
                        //  _appntundrprocessdata.EPFWages = AppointeeDetails?.EPFWages ?? _appntundrprocessdata.EPFWages;
                        _appntNonprocessdata.DateOfJoining = appointeeDetails?.DateOfJoining;
                        _appntNonprocessdata.UpdatedBy = appointeeDetails?.UserId;
                        _appntNonprocessdata.UpdatedOn = DateTime.Now;

                    }
                }
                if (updateLogList?.Count() > 0)
                {
                    _dbContextClass.AppointeeUpdateLog.AddRange(updateLogList);

                }
                _ = await _dbContextClass.SaveChangesAsync();
            }
            //    transaction.Commit();

            //}
            //catch (Exception)
            //{
            //    throw;

            //}
        }

        public async Task PostAppointeeSaveDetailsAsync(AppointeeSaveDetailsRequest AppointeeDetails)
        {
            //using var transaction = _dbContextClass.Database.BeginTransaction();
            //try
            //{
            await PostAppointeePersonalDetailsAsync(AppointeeDetails);

            string _activityStatus = AppointeeDetails.IsSubmit ? ActivityLog.DATASAVED : ActivityLog.DATADRAFT;
            await _dbContextDalActivity.PostActivityDetails(AppointeeDetails?.AppointeeId ?? 0, AppointeeDetails.UserId, _activityStatus);
            //    transaction.Commit();
            //}
            //catch (Exception)
            //{
            //    throw;

            //}
        }
        private async Task PostAppointeePersonalDetailsAsync(AppointeeSaveDetailsRequest AppointeeDetails)
        {
            if (AppointeeDetails != null)
            {
                int appointeeId = AppointeeDetails.AppointeeId;
                bool _isPassportAvailable = AppointeeDetails?.IsPassportAvailable?.ToString()?.ToUpper() == CheckType.yes;
                bool _isInternationalWrkr = AppointeeDetails?.IsInternationalWorker?.ToString()?.ToUpper() == CheckType.yes;
                bool _isHandicap = AppointeeDetails?.IsHandicap?.ToString()?.ToUpper() == CheckType.yes;
                AppointeeDetails? _appointeedetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId) && x.IsProcessed.Equals(false)) ?? null;
                UnderProcessFileData? _appntundrprocessdata = await _dbContextClass.UnderProcessFileData.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId)) ?? null;
                if (_appointeedetails == null)
                {

                    AppointeeDetails data = new()
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
                        PassportNo = AppointeeDetails.PassportNo,
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

                    _ = _dbContextClass.AppointeeDetails.Add(data);

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
                    _appointeedetails.PassportNo = AppointeeDetails.PassportNo;
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
                _ = await _dbContextClass.SaveChangesAsync();

            }
        }

        public async Task AppointeeWorkflowUpdateAsync(WorkFlowDataRequest WorkFlowRequest)
        {
            WorkflowApprovalStatusMaster? approvalState = new();
            int _reprocessCount = 0;
            List<WorkflowApprovalStatusMaster> getapprovalStatus = await _dbContextClass.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToListAsync();
            if (!string.IsNullOrEmpty(WorkFlowRequest.approvalStatus))
            {
                approvalState = getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowRequest.approvalStatus?.Trim());

                if (WorkFlowRequest.approvalStatus is WorkFlowType.Approved or WorkFlowType.ForcedApproved or
                 WorkFlowType.Rejected or WorkFlowType.ProcessClose)
                {
                    await AppointeeWorkflowProcessdAsync(WorkFlowRequest, approvalState.AppvlStatusId);

                }

                if (WorkFlowRequest.approvalStatus == WorkFlowType.Reprocess)
                {
                    await AppointeeWorkflowReProcessdAsync(WorkFlowRequest.appointeeId, approvalState.AppvlStatusId, WorkFlowRequest.userId);
                }
            }

            WorkFlowDetails? _workFlow_det = await _dbContextClass.WorkFlowDetails.FirstOrDefaultAsync(x => x.AppointeeId == WorkFlowRequest.appointeeId && x.ActiveStatus == true);
            if (_workFlow_det != null)
            {
                _reprocessCount = _workFlow_det?.ReprocessCount ?? 0;
                if (string.IsNullOrEmpty(WorkFlowRequest.approvalStatus))
                {
                    approvalState = getapprovalStatus.Find(x => x.AppvlStatusId == _workFlow_det.AppvlStatusId);
                }

                if (WorkFlowRequest.approvalStatus == WorkFlowType.Reprocess)
                {
                    _reprocessCount = _workFlow_det?.ReprocessCount ?? 0 + 1;
                }

                // await _dbContextClass.SaveChangesAsync();
            }

            await workflowdataUpdate(WorkFlowRequest, approvalState, _reprocessCount, _workFlow_det);
        }
        private async Task AppointeeWorkflowReProcessdAsync(int appointeeId, int StatusId, int userId)
        {
            UserMaster? _userdata = await _dbContextClass.UserMaster.FirstOrDefaultAsync(x => x.RefAppointeeId == appointeeId);
            AppointeeDetails? _appointeedetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId)) ?? null;
            if (_appointeedetails != null)
            {
                _appointeedetails.IsSubmit = false;
                _appointeedetails.ProcessStatus = StatusId;
                _appointeedetails.SaveStep = 0;
                _appointeedetails.IsPasssportVarified = null;
                _appointeedetails.IsAadhaarVarified = null;
                _appointeedetails.IsUanVarified = null;

            }
            _userdata.UpdatedOn = DateTime.Now;
            _userdata.UpdatedBy = userId;
            _ = await _dbContextClass.SaveChangesAsync();
        }
        private async Task AppointeeWorkflowProcessdAsync(WorkFlowDataRequest workFlowRequest, int StatusId)
        {
            UserMaster? _userdata = await _dbContextClass.UserMaster.FirstOrDefaultAsync(x => x.RefAppointeeId == workFlowRequest.appointeeId);

            AppointeeDetails? _appointeedetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(workFlowRequest.appointeeId) && x.IsProcessed.Equals(false)) ?? null;
            if (_appointeedetails != null)
            {
                _appointeedetails.IsProcessed = true;
                _appointeedetails.ProcessStatus = StatusId;
            }
            if (workFlowRequest.approvalStatus is WorkFlowType.Approved or WorkFlowType.ForcedApproved)
            {
                ProcessedFileData _processeddata = new()
                {
                    AppointeeId = workFlowRequest.appointeeId,
                    ActiveStatus = true,
                    DataUploaded = false,
                    CreatedBy = workFlowRequest.userId,
                    CreatedOn = DateTime.Now,
                };
                _ = _dbContextClass.ProcessedFileData.Add(_processeddata);

            }

            if (workFlowRequest.approvalStatus == WorkFlowType.Rejected)
            {
                RejectedFileData _rejecteddata = new()
                {
                    AppointeeId = workFlowRequest.appointeeId,
                    RejectReason = workFlowRequest.Remarks,
                    RejectState = (int)RejectState.ApprovalReject,
                    ActiveStatus = true,
                    CreatedBy = workFlowRequest.userId,
                    CreatedOn = DateTime.Now,
                };
                _ = _dbContextClass.RejectedFileData.Add(_rejecteddata);
            }
            _userdata.ActiveStatus = workFlowRequest.approvalStatus == WorkFlowType.ProcessClose ? false : _userdata.ActiveStatus;
            _userdata.CurrStatus = workFlowRequest.approvalStatus == WorkFlowType.Rejected ? false : _userdata.CurrStatus;
            _userdata.UpdatedOn = DateTime.Now;
            _userdata.UpdatedBy = workFlowRequest.userId;
            _ = await _dbContextClass.SaveChangesAsync();
        }
        public async Task workflowdataUpdate(WorkFlowDataRequest WorkFlowRequest, WorkflowApprovalStatusMaster? approvalState, int _reprocessCount, WorkFlowDetails? _workFlow_det)
        {
            WorkFlowDetails _genarateWorkflowdata = new()
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

            WorkFlowDetailsHist _workFlow_det_his = new()
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

            _ = await _dbContextClass.SaveChangesAsync();
        }
        public async Task<List<GlobalSearchAppointeeData>> GetUnderProcessAppointeeSearch(string Name)
        {
            List<GlobalSearchAppointeeData> appointeeList = new();
            WorkflowApprovalStatusMaster closeState = await GetApprovalState(WorkFlowType.ProcessClose?.Trim());

            IQueryable<GlobalSearchAppointeeData> querydata = from b in _dbContextClass.UnderProcessFileData.Where(m => m.AppointeeName.ToLower().Contains(Name.ToLower()) && m.ActiveStatus == true)
                                                              join w in _dbContextClass.WorkFlowDetails
                                                              on b.AppointeeId equals w.AppointeeId
                                                              where (w.AppvlStatusId != closeState.AppvlStatusId)
                                                              && b.ActiveStatus == true
                                                              orderby b.AppointeeId
                                                              select new GlobalSearchAppointeeData
                                                              { AppointeeId = b.AppointeeId ?? 0, AppointeeName = b.AppointeeName, CandidateId = b.CandidateId, DateOfJoining = b.DateOfJoining, AppvlStatusId = w.AppvlStatusId };

            appointeeList = await querydata.ToListAsync().ConfigureAwait(false);
            return appointeeList;
        }
        public async Task<List<GlobalSearchAppointeeData>> GetAppointeeSearchDetails(string name, string type)
        {
            List<GlobalSearchAppointeeData> appointeeList = new();
            if (type == "LinkNotSend")
            {
                appointeeList = await _dbContextClass.UnProcessedFileData.Where(m => m.AppointeeName.ToLower().Contains(name.ToLower()) && m.ActiveStatus == true)
                    .Select(x => new GlobalSearchAppointeeData
                    {
                        AppointeeId = x.UnProcessedId,
                        AppointeeName = x.AppointeeName,
                        CandidateId = x.CandidateId,
                        DateOfJoining = x.DateOfJoining,
                    }).ToListAsync();
            }
            if (type == "Raw")
            {
                appointeeList = await _dbContextClass.RawFileData.Where(m => m.AppointeeName.ToLower().Contains(name.ToLower()) && m.ActiveStatus == true)
                    .Select(x => new GlobalSearchAppointeeData
                    {
                        AppointeeId = x.RawFileId,
                        AppointeeName = x.AppointeeName,
                        CandidateId = x.CandidateId,
                        DateOfJoining = x.DateOfJoining,
                    }).ToListAsync();
            }
            return appointeeList;
        }
    }
}
