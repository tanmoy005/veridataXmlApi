using Microsoft.EntityFrameworkCore;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.DAL.DBContext;
using VERIDATA.DAL.utility;
using VERIDATA.Model.Configuration;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Config;
using VERIDATA.Model.Table.Master;
using VERIDATA.Model.Table.Public;
using static VERIDATA.DAL.utility.CommonEnum;

namespace VERIDATA.DAL.DataAccess.Context
{
    public class ReportingDalContext : IReportingDalContext
    {
        private readonly DbContextDalDB _dbContextClass;
        private readonly ApiConfiguration _config;
        public ReportingDalContext(DbContextDalDB dbContextClass, ApiConfiguration config)
        {
            _dbContextClass = dbContextClass;
            _config = config;
        }
        public async Task<List<ProcessedDataReportDetailsResponse>> GetProcessedAppointeeReportDetailsAsync(List<ProcessedDataDetailsResponse> AppointeeList)
        {
            ReasonMaser? othRsnCatgry = await _dbContextClass.ReasonMaser.FirstOrDefaultAsync(x => x.ReasonType.Equals(RemarksType.Others));

            string? key = _config.EncriptKey;
            var querydata = from p in AppointeeList
                            join x in _dbContextClass.MaratialStatusMaster
                                    on p.AppointeeData.MaratialStatus equals x.MStatusCode into MGrouping
                            from m in MGrouping.DefaultIfEmpty()
                            join y in _dbContextClass.QualificationMaster
                                    on p.AppointeeData.Qualification equals y.QualificationCode into QGrouping
                            from q in QGrouping.DefaultIfEmpty()
                            join y in _dbContextClass.GenderMaster
                                    on p.AppointeeData.Gender equals y.GenderCode into GGrouping
                            from g in GGrouping.DefaultIfEmpty()
                            join z in _dbContextClass.DisabilityMaster
                                   on p.AppointeeData.HandicapeType equals z.DisabilityCode into DGrouping
                            from h in DGrouping.DefaultIfEmpty()

                            select new { p, m.MStatusName, q.QualificationName, g.GenderName, h?.DisabilityName };

            var appointeelist = querydata.ToList();
            var remarksquerydata = from a in appointeelist
                                   join z in _dbContextClass.AppointeeReasonMappingData
                                 on a.p.AppointeeId equals z.AppointeeId into RGrouping
                                   from r in RGrouping.DefaultIfEmpty()
                                       //where r.ReasonId == othRsnCatgry.ReasonId
                                   select new
                                   {
                                       data = a?.p,
                                       AppointeeId = a?.p?.AppointeeId,
                                       MStatusName = a?.MStatusName,
                                       QualificationName = a?.QualificationName,
                                       GenderName = a?.GenderName,
                                       HandiCapName = a?.DisabilityName,
                                       Remarks = r?.Remarks,
                                       ReasonId = r?.ReasonId
                                   };
            List<ProcessedDataReportDetailsResponse> Response = remarksquerydata.ToList().GroupBy(x => x.AppointeeId).Select(r => new ProcessedDataReportDetailsResponse
            {

                CandidateId = r.FirstOrDefault()?.data?.CandidateId,
                AppointeeName = r.FirstOrDefault()?.data?.AppointeeName,
                DateOfBirth = r.FirstOrDefault()?.data?.AppointeeData?.DateOfBirth?.ToShortDateString(),
                MobileNo = r.FirstOrDefault()?.data?.MobileNo,
                EmailId = r.FirstOrDefault()?.data?.AppointeeEmailId,
                DateOfJoining = r.FirstOrDefault()?.data?.DateOfJoining?.ToShortDateString(),
                Nationality = r.FirstOrDefault()?.data?.AppointeeData?.Nationality,
                QualificationName = r.FirstOrDefault()?.QualificationName,
                UANNumber = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.UANNumber) ? "NA" : CommonUtility.DecryptString(key, r.FirstOrDefault()?.data?.AppointeeData?.UANNumber),
                PensionAvailable = r.FirstOrDefault()?.data?.AppointeeData?.IsPensionApplicable == null ? "NA" : r.FirstOrDefault()?.data?.AppointeeData?.IsPensionApplicable ?? false ? "Yes" : "No",
                GenderName = r.FirstOrDefault()?.GenderName,
                MaratialStatusName = r.FirstOrDefault()?.MStatusName,
                MemberName = r.FirstOrDefault()?.data?.AppointeeData?.MemberName,
                MemberRelationName = r.FirstOrDefault()?.data?.AppointeeData?.MemberRelation == "F" ? "Father" : r.FirstOrDefault()?.data?.AppointeeData?.MemberRelation == "H" ? "Husband" : string.Empty,
                IsHandicap = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.IsHandicap) ? "NA" : r.FirstOrDefault()?.data?.AppointeeData?.IsHandicap?.ToUpper() == "N" ? "No" : "Yes",
                HandicapeType = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.HandicapeType) ? "NA" : r.FirstOrDefault()?.HandiCapName,
                //HandicapeName = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.HandicapeName) ? "NA" : r.FirstOrDefault()?.data?.AppointeeData?.HandicapeName,
                IsInternationalWorker = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.IsInternationalWorker) ? "NA" : r.FirstOrDefault()?.data?.AppointeeData?.IsInternationalWorker?.ToUpper() == "N" ? "No" : "Yes",
                PassportNo = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.PassportNo) ? "NA" : CommonUtility.DecryptString(key, r.FirstOrDefault()?.data?.AppointeeData?.PassportNo),
                PassportValidFrom = r.FirstOrDefault()?.data?.AppointeeData?.PassportValidFrom == null ? "NA" : r.FirstOrDefault()?.data?.AppointeeData?.PassportValidFrom?.ToShortDateString(),
                PassportValidTill = r.FirstOrDefault()?.data?.AppointeeData?.PassportValidTill == null ? "NA" : r.FirstOrDefault()?.data?.AppointeeData?.PassportValidTill?.ToShortDateString(),
                OriginCountry = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.OriginCountry) ? "NA" : r.FirstOrDefault()?.data?.AppointeeData?.OriginCountry,
                PANName = r.FirstOrDefault()?.data?.AppointeeData?.PANName,
                PANNumber = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.PANNumber) ? null : CommonUtility.DecryptString(key, r.FirstOrDefault()?.data?.AppointeeData?.PANNumber),
                AadhaarName = r.FirstOrDefault()?.data?.AppointeeData?.AadhaarName,
                //AadhaarNumber = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.AadhaarNumberView) ? "NA" : r.FirstOrDefault()?.data?.AppointeeData?.AadhaarNumberView,
                AadhaarNumber = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.AadhaarNumberView) ? "NA" : CommonUtility.DecryptString(key, r.FirstOrDefault()?.data?.AppointeeData?.AadhaarNumber)?? r.FirstOrDefault()?.data?.AppointeeData?.AadhaarNumberView,
                Remarks = r?.Where(x => x.ReasonId == othRsnCatgry.ReasonId)?.Select(y => y.Remarks)?.Aggregate("", (current, s) => current + s + ",")
            }).ToList();

            return Response;
        }

        public async Task<List<PfCreationProcessedReportResponse>> GetPfCreationProcessedReportDetailsAsync(PfUserListRequest filter)
        {
            List<PfCreationProcessedReportResponse> Response = new();

            IQueryable<PfCreationProcessedReportResponse> querydata = from p in _dbContextClass.ProcessedFileData
                                                                      join a in _dbContextClass.AppointeeDetails
                                                                          on p.AppointeeId equals a.AppointeeId
                                                                      where p.ActiveStatus == true && a.IsProcessed == true
                                                                        && (filter.FromDate == null || p.CreatedOn >= filter.FromDate)
                                                                        && (filter.ToDate == null || p.CreatedOn <= filter.ToDate)
                                                                        && (filter.IsDownloaded == null || p.DataUploaded == filter.IsDownloaded)
                                                                      select new PfCreationProcessedReportResponse { AppointeeData = a, ProcessData = p };
            Response = await querydata.ToListAsync().ConfigureAwait(false);

            return Response;
        }

        public async Task UpdateDownloadedProcessData(List<ProcessedFileData> data)
        {
            if (data.Count > 0)
            {
                foreach (ProcessedFileData obj in data)
                {
                    obj.UpdatedOn = DateTime.Now;
                    obj.DataUploaded = true;
                }

                _ = await _dbContextClass.SaveChangesAsync();
            }
        }

        public async Task<List<ApiCounter>> GetTotalApiList(DateTime? FromDate, DateTime? ToDate)
        {
            List<ApiCounter> totalApiList = await _dbContextClass.ApiCounter.Where(m => (FromDate == null || m.CreatedOn >= FromDate) && (ToDate == null || m.CreatedOn <= ToDate)).ToListAsync();
            return totalApiList;

        }
        public async Task<List<NonProcessCandidateReportDataResponse>> GetNonProcessCandidateReport(AppointeeCountReportSearchRequest reqObj)
        {

            IQueryable<NonProcessCandidateReportDataResponse> nonProcessQueryData = from ap in _dbContextClass.UploadAppointeeCounter
 .Where(m => (reqObj.FromDate == null || m.CreatedOn >= reqObj.FromDate)
             && (reqObj.ToDate == null || m.CreatedOn <= reqObj.ToDate))
                                                                                    join a in _dbContextClass.UnProcessedFileData
                                                                                    on ap.FileId equals a.FileId
                                                                                    join c in _dbContextClass.CompanyDetails
                                                                                    on a.CompanyId equals c.Id
                                                                                    where
                                                                                    // Modify this line to use Contains for multiple EntityIds
                                                                                    (reqObj.EntityId == null || !reqObj.EntityId.Any() || reqObj.EntityId.Contains(a.CompanyId)) &&
                                                                                    (string.IsNullOrEmpty(reqObj.AppointeeName) || a.AppointeeName.Contains(reqObj.AppointeeName))
                                                                                    select new NonProcessCandidateReportDataResponse
                                                                                    {
                                                                                        AppointeeName = a.AppointeeName,
                                                                                        AppointeeEmail = a.AppointeeEmailId,
                                                                                        CandidateId = a.CandidateId,
                                                                                        DateOfJoining = a.DateOfJoining,
                                                                                        CompanyId = c.Id,
                                                                                        CompanyName = c.CompanyName,
                                                                                        CreatedOn = ap.CreatedOn,
                                                                                    };

            return await nonProcessQueryData.ToListAsync();
        }

        public async Task<List<NationalityQueryDataResponse>> GetCandidateNationalityReport(GetNationalityReportRequest reqObj)
        {
            string CurrDate = DateTime.Now.ToShortDateString();
            DateTime _CurrDate = Convert.ToDateTime(CurrDate);
            DateTime? _ToDate = reqObj.ToDate != null ? reqObj.ToDate?.AddDays(1) : null;

            // Fetch approval status
            List<WorkflowApprovalStatusMaster> _getapprovalStatus = await _dbContextClass.WorkflowApprovalStatusMaster
                .Where(x => x.ActiveStatus == true)
                .ToListAsync();

            // Get required states
            WorkflowApprovalStatusMaster? ReprocessState = _getapprovalStatus
                .FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Reprocess?.Trim());
            WorkflowApprovalStatusMaster? CloseState = _getapprovalStatus
                .FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ProcessClose?.Trim());
            WorkflowApprovalStatusMaster? RejectState = _getapprovalStatus
                .FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Rejected?.Trim());
            WorkflowApprovalStatusMaster? ApproveState = _getapprovalStatus
                .FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Approved?.Trim());

            if (ReprocessState == null || CloseState == null || RejectState == null || ApproveState == null)
            {
                throw new Exception("One or more workflow states are missing.");
            }

            // Query to fetch nationality data
            IQueryable<NationalityQueryDataResponse> querydata = from p in _dbContextClass.AppointeeDetails
                                                                 join u in _dbContextClass.UnderProcessFileData on p.AppointeeId equals u.AppointeeId
                                                                 join w in _dbContextClass.WorkFlowDetails on u.AppointeeId equals w.AppointeeId
                                                                 where w.AppvlStatusId != CloseState.AppvlStatusId
                                                                 && w.AppvlStatusId != ApproveState.AppvlStatusId
                                                                 && w.AppvlStatusId != RejectState.AppvlStatusId
                                                                 && (p.IsProcessed == false || p.IsProcessed == null)
                                                                 && (reqObj.FromDate == null || u.CreatedOn >= reqObj.FromDate)
                                                                 && (reqObj.ToDate == null || u.CreatedOn < _ToDate)
                                                                 && !string.IsNullOrEmpty(p.Nationality)
                                                                 && u.ActiveStatus == true
                                                                 orderby p.IsSubmit
                                                                 select new NationalityQueryDataResponse
                                                                 {
                                                                     AppointeeDetails = p,
                                                                     AppvlStatusId = w.AppvlStatusId,
                                                                     AppointeeId = u.AppointeeId,
                                                                     IsJoiningDateLapsed = u.DateOfJoining < _CurrDate
                                                                 };

            List<NationalityQueryDataResponse> list = await querydata.ToListAsync().ConfigureAwait(false);

            if (list == null || list.Count == 0)
            {
                throw new Exception("No records found based on the query parameters.");
            }

            return list;

           
        }
        public async Task<List<UnderProcessCandidateReportDataResponse>> GetUnderProcessCandidateReport(AppointeeCountReportSearchRequest reqObj, string? _statusCode, bool? _intSubmitCode, int? _intSubStatusCode)
        {


            IQueryable<UnderProcessCandidateReportDataResponse> underProcessQueryData = from ap in _dbContextClass.UploadAppointeeCounter
                                                                                        .Where(m => (reqObj.FromDate == null || m.CreatedOn >= reqObj.FromDate)
                                                                                        && (reqObj.ToDate == null || m.CreatedOn <= reqObj.ToDate))
                                                                                        join a in _dbContextClass.UnderProcessFileData
                                                                                        on ap.FileId equals a.FileId
                                                                                        join c in _dbContextClass.CompanyDetails
                                                                                        on a.CompanyId equals c.Id
                                                                                        join w in _dbContextClass.WorkFlowDetails
                                                                                        on a.AppointeeId equals w.AppointeeId
                                                                                        join wm in _dbContextClass.WorkflowApprovalStatusMaster
                                                                                        on w.AppvlStatusId equals wm.AppvlStatusId
                                                                                        join p in _dbContextClass.AppointeeDetails
                                                                                        on a.AppointeeId equals p.AppointeeId into grouping
                                                                                        from p in grouping.DefaultIfEmpty()
                                                                                        where
                                                                                        (string.IsNullOrEmpty(reqObj.AppointeeName) || a.AppointeeName.ToUpper().Contains(reqObj.AppointeeName)) &&
                                                                                        (string.IsNullOrEmpty(_statusCode) || wm.AppvlStatusCode == _statusCode) &&
                                                                                        (reqObj.EntityId == null || !reqObj.EntityId.Any() || reqObj.EntityId.Contains(a.CompanyId)) && // Modify this line
                                                                                        (_intSubmitCode == null || (p.IsSubmit == _intSubmitCode)) &&
                                                                                        (_intSubStatusCode == null || (_intSubStatusCode == 1 && p.SaveStep == _intSubStatusCode && p.IsSubmit != true)
                                                                                        || (_intSubStatusCode == 0 && p.SaveStep != 1 && p.IsSubmit != true))
                                                                                        select new UnderProcessCandidateReportDataResponse
                                                                                        {
                                                                                            AppointeeName = a.AppointeeName,
                                                                                            AppointeeEmail = a.AppointeeEmailId,
                                                                                            CandidateId = a.CandidateId,
                                                                                            CompanyId = c.Id,
                                                                                            CompanyName = c.CompanyName,
                                                                                            DateOfJoining = a.DateOfJoining,
                                                                                            CreatedOn = ap.CreatedOn,
                                                                                            AppvlStatusId = w.AppvlStatusId,
                                                                                            ActionTakenAt = w.ActionTakenAt,
                                                                                            AppvlStatusDesc = wm.AppvlStatusDesc,
                                                                                            AppvlStatusCode = wm.AppvlStatusCode,
                                                                                            UpdatedOn = p.UpdatedOn,
                                                                                            SaveStep = p.SaveStep,
                                                                                            IsSubmit = p.IsSubmit
                                                                                        };

            return await underProcessQueryData.ToListAsync();

        }
    }
}
