﻿using Microsoft.EntityFrameworkCore;
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

                            select new { p, m.MStatusName, q.QualificationName, g.GenderName };

            var appointeelist = querydata.ToList();
            var remarksquerydata = from a in appointeelist
                                   join z in _dbContextClass.AppointeeReasonMappingData
                                 on a.p.AppointeeId equals z.AppointeeId into RGrouping
                                   from r in RGrouping.DefaultIfEmpty()
                                       //where r.ReasonId == othRsnCatgry.ReasonId
                                   select new
                                   {
                                       data = a.p,
                                       a.p.AppointeeId,
                                       a.MStatusName,
                                       a.QualificationName,
                                       a.GenderName,
                                       r.Remarks,
                                       r.ReasonId
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
                GenderName = r.FirstOrDefault()?.GenderName,
                MaratialStatusName = r.FirstOrDefault()?.MStatusName,
                MemberName = r.FirstOrDefault()?.data?.AppointeeData?.MemberName,
                MemberRelationName = r.FirstOrDefault()?.data?.AppointeeData?.MemberRelation == "F" ? "Father" : r.FirstOrDefault()?.data?.AppointeeData?.MemberRelation == "H" ? "Husband" : string.Empty,
                IsHandicap = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.IsHandicap) ? "NA" : r.FirstOrDefault()?.data?.AppointeeData?.IsHandicap?.ToUpper() == "N" ? "No" : "Yes",
                HandicapeType = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.HandicapeType) ? "NA" : r.FirstOrDefault()?.data?.AppointeeData?.HandicapeType,
                IsInternationalWorker = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.IsInternationalWorker) ? "NA" : r.FirstOrDefault()?.data?.AppointeeData?.IsInternationalWorker?.ToUpper() == "N" ? "No" : "Yes",
                PassportNo = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.PassportNo) ? "NA" : CommonUtility.DecryptString(key, r.FirstOrDefault()?.data?.AppointeeData?.PassportNo),
                PassportValidFrom = r.FirstOrDefault()?.data?.AppointeeData?.PassportValidFrom == null ? "NA" : r.FirstOrDefault()?.data?.AppointeeData?.PassportValidFrom?.ToShortDateString(),
                PassportValidTill = r.FirstOrDefault()?.data?.AppointeeData?.PassportValidTill == null ? "NA" : r.FirstOrDefault()?.data?.AppointeeData?.PassportValidTill?.ToShortDateString(),
                OriginCountry = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.OriginCountry) ? "NA" : r.FirstOrDefault()?.data?.AppointeeData?.OriginCountry,
                PANName = r.FirstOrDefault()?.data?.AppointeeData?.PANName,
                PANNumber = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.PANNumber) ? null : CommonUtility.DecryptString(key, r.FirstOrDefault()?.data?.AppointeeData?.PANNumber),
                AadhaarName = r.FirstOrDefault()?.data?.AppointeeData?.AadhaarName,
                AadhaarNumber = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.AadhaarNumberView) ? "NA" : r.FirstOrDefault()?.data?.AppointeeData?.AadhaarNumberView,
                //AadhaarNumber = string.IsNullOrEmpty(r.FirstOrDefault()?.data?.AppointeeData?.AadhaarNumber) ? "NA" : CommonUtility.DecryptString(key, r.FirstOrDefault()?.data?.AppointeeData?.AadhaarNumber),
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
                                                                                    where string.IsNullOrEmpty(reqObj.AppointeeName) ||
                                                                                    a.AppointeeName.Contains(reqObj.AppointeeName)
                                                                                    select new NonProcessCandidateReportDataResponse
                                                                                    {
                                                                                        AppointeeName = a.AppointeeName,
                                                                                        AppointeeEmail = a.AppointeeEmailId,
                                                                                        CandidateId = a.CandidateId,
                                                                                        DateOfJoining = a.DateOfJoining,
                                                                                        CreatedOn = ap.CreatedOn,
                                                                                    };

            List<NonProcessCandidateReportDataResponse> nonProcessAppointeeList = await nonProcessQueryData.ToListAsync().ConfigureAwait(false);
            return nonProcessAppointeeList;
        }
        public async Task<List<UnderProcessCandidateReportDataResponse>> GetUnderProcessCandidateReport(AppointeeCountReportSearchRequest reqObj, string? _statusCode, bool? _intSubmitCode, int? _intSubStatusCode)
        {
            IQueryable<UnderProcessCandidateReportDataResponse> underProcessQueryData = from ap in _dbContextClass.UploadAppointeeCounter
                                       .Where(m => (reqObj.FromDate == null || m.CreatedOn >= reqObj.FromDate)
&& (reqObj.ToDate == null || m.CreatedOn <= reqObj.ToDate))
                                                                                        join a in _dbContextClass.UnderProcessFileData
                                                                                        on ap.FileId equals a.FileId
                                                                                        join w in _dbContextClass.WorkFlowDetails
                                                                                        on a.AppointeeId equals w.AppointeeId
                                                                                        join wm in _dbContextClass.WorkflowApprovalStatusMaster
                                                                                        on w.AppvlStatusId equals wm.AppvlStatusId
                                                                                        join p in _dbContextClass.AppointeeDetails
                                                                                    on a.AppointeeId equals p.AppointeeId into grouping
                                                                                        from p in grouping.DefaultIfEmpty()
                                                                                        where (string.IsNullOrEmpty(reqObj.AppointeeName) ||
                                                                                        a.AppointeeName.ToUpper().Contains(reqObj.AppointeeName))
&& (string.IsNullOrEmpty(_statusCode) || wm.AppvlStatusCode == _statusCode)
&& (_intSubmitCode == null || (p.IsSubmit == _intSubmitCode))
&& (_intSubStatusCode == null || (_intSubStatusCode == 1 && p.SaveStep == _intSubStatusCode && p.IsSubmit != true)
                                                                                        || (_intSubStatusCode == 0 && p.SaveStep != 1 && p.IsSubmit != true))
                                                                                        select new UnderProcessCandidateReportDataResponse
                                                                                        {
                                                                                            AppointeeName = a.AppointeeName,
                                                                                            AppointeeEmail = a.AppointeeEmailId,
                                                                                            CandidateId = a.CandidateId,
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
            List<UnderProcessCandidateReportDataResponse> underProcessAppointeeList = await underProcessQueryData.ToListAsync().ConfigureAwait(false);

            return underProcessAppointeeList;
        }
    }
}
