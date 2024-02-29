using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PfcAPI.Infrastucture.DBContext;
using PfcAPI.Infrastucture.Interfaces;
using PfcAPI.Infrastucture.utility;
using PfcAPI.Model.Appointee;
using PfcAPI.Model.Configuration;
using PfcAPI.Model.DataAccess;
using PfcAPI.Model.RequestModel;
using PfcAPI.Model.ResponseModel;
using System.Net;
using VERIDATA.Model.Configuration;
using static PfcAPI.Infrastucture.CommonEnum;

namespace PfcAPI.Infrastucture.Context
{
    public class Reporting : IReporting
    {
        private readonly DbContextDB _dbContextClass;
        private readonly ApiConfiguration _aadhaarConfig;
        private readonly string key;
        public Reporting(DbContextDB dbContextClass, ApiConfiguration aadhaarConfig)
        {
            _dbContextClass = dbContextClass;
            _aadhaarConfig = aadhaarConfig;
            key = aadhaarConfig.EncriptKey;
        }
        public async Task<List<ProcessedDataReportDetails>> GetApporvedAppointeeDetails(ProcessedFilterRequest filter)
        {
            var querydata = from p in _dbContextClass.ProcessedFileData
                            join a in _dbContextClass.AppointeeDetails
                                on p.AppointeeId equals a.AppointeeId
                            where p.ActiveStatus == true //& a.IsProcessed == true
                            //& p.DataUploaded == false
                            & (filter.FromDate == null || p.CreatedOn >= filter.FromDate)
                            & (filter.ToDate == null || p.CreatedOn <= filter.ToDate)
                            & (filter.IsPfRequired == null || a.IsPFverificationReq == filter.IsPfRequired)
                            select new { a }.a;
            var appointeelist = await querydata.ToListAsync().ConfigureAwait(false);

            List<ProcessedDataReportDetails> processeddata = await GetProcessedViewData(appointeelist);

            return processeddata;
        }

        public async Task<List<ProcessedDataReportDetails>> GetApporvedMISAppointeeDetails(FilterRequest filter)
        {
            var pAppntedata = from p in _dbContextClass.ProcessedFileData
                              join a in _dbContextClass.AppointeeDetails
                                  on p.AppointeeId equals a.AppointeeId
                              where p.ActiveStatus == true & a.IsProcessed == true
                              & p.DataUploaded == false & a.IsPFverificationReq != true
                              & (filter.FromDate == null || p.CreatedOn >= filter.FromDate)
                              & (filter.ToDate == null || p.CreatedOn <= filter.ToDate)
                              select new { a }.a;
            var appointeelist = await pAppntedata.ToListAsync();

            List<ProcessedDataReportDetails> processeddata = await GetProcessedViewData(appointeelist);

            return processeddata;
        }

        private async Task<List<ProcessedDataReportDetails>> GetProcessedViewData(List<AppointeeDetails> appointeelist)
        {
            var othRsnCatgry = await _dbContextClass.ReasonMaser.FirstOrDefaultAsync(x => x.ReasonType.Equals(RemarksType.Others));
            var key = _aadhaarConfig.EncriptKey;
            var processeddata = appointeelist
                   .Join(_dbContextClass.MaratialStatusMaster,
                       a => a.MaratialStatus,
                       mts => mts.MStatusCode,
                       (a, mts) => new
                       {
                           a.AppointeeId,
                           a.CandidateId,
                           a.AppointeeName,
                           a.DateOfBirth,
                           a.MobileNo,
                           a.AppointeeEmailId,
                           a.DateOfJoining,
                           a.Nationality,
                           a.Qualification,
                           a.UANNumber,
                           a.EPFWages,
                           a.Gender,
                           a.MaratialStatus,
                           mts.MStatusName,
                           a.MemberName,
                           a.MemberRelation,
                           a.IsHandicap,
                           a.HandicapeType,
                           a.IsInternationalWorker,
                           a.PassportNo,
                           a.PassportValidFrom,
                           a.PassportValidTill,
                           a.OriginCountry,
                           a.PANName,
                           a.PANNumber,
                           a.AadhaarName,
                           a.AadhaarNumberView,
                           a.AadhaarNumber
                       })
                   .Join(_dbContextClass.QualificationMaster,
                       o => o.Qualification,
                       qua => qua.QualificationCode,
                       (o, qua) => new
                       {
                           o.AppointeeId,
                           o.CandidateId,
                           o.AppointeeName,
                           o.DateOfBirth,
                           o.MobileNo,
                           o.AppointeeEmailId,
                           o.DateOfJoining,
                           o.Nationality,
                           o.Qualification,
                           qua.QualificationName,
                           o.UANNumber,
                           o.EPFWages,
                           o.Gender,
                           o.MaratialStatus,
                           o.MStatusName,
                           o.MemberName,
                           o.MemberRelation,
                           o.IsHandicap,
                           o.HandicapeType,
                           o.IsInternationalWorker,
                           o.PassportNo,
                           o.PassportValidFrom,
                           o.PassportValidTill,
                           o.OriginCountry,
                           o.PANName,
                           o.PANNumber,
                           o.AadhaarName,
                           o.AadhaarNumberView,
                           o.AadhaarNumber

                       })
                   .Join(_dbContextClass.GenderMaster,
                       o => o.Gender,
                       g => g.GenderCode,
                       (o, g) => new
                       {
                           o.AppointeeId,
                           o.CandidateId,
                           o.AppointeeName,
                           o.DateOfBirth,
                           o.MobileNo,
                           o.AppointeeEmailId,
                           o.DateOfJoining,
                           o.Nationality,
                           o.Qualification,
                           o.QualificationName,
                           o.UANNumber,
                           o.EPFWages,
                           o.Gender,
                           g.GenderName,
                           o.MaratialStatus,
                           o.MStatusName,
                           o.MemberName,
                           o.MemberRelation,
                           o.IsHandicap,
                           o.HandicapeType,
                           o.IsInternationalWorker,
                           o.PassportNo,
                           o.PassportValidFrom,
                           o.PassportValidTill,
                           o.OriginCountry,
                           o.PANName,
                           o.PANNumber,
                           o.AadhaarName,
                           o.AadhaarNumberView,
                           o.AadhaarNumber

                       }).GroupJoin(
          _dbContextClass.AppointeeReasonMappingData,
          ap => ap.AppointeeId,
          rm => rm.AppointeeId,
          (x, y) => new { a = x, r = y })
           .SelectMany(
           x => x.r.DefaultIfEmpty(),
            (o, r) => new
            {
                o.a.AppointeeId,
                o.a.CandidateId,
                o.a.AppointeeName,
                o.a.DateOfBirth,
                o.a.MobileNo,
                o.a.AppointeeEmailId,
                o.a.DateOfJoining,
                o.a.Nationality,
                o.a.Qualification,
                o.a.QualificationName,
                o.a.UANNumber,
                o.a.EPFWages,
                o.a.Gender,
                o.a.GenderName,
                o.a.MaratialStatus,
                o.a.MStatusName,
                o.a.MemberName,
                o.a.MemberRelation,
                o.a.IsHandicap,
                o.a.HandicapeType,
                o.a.IsInternationalWorker,
                o.a.PassportNo,
                o.a.PassportValidFrom,
                o.a.PassportValidTill,
                o.a.OriginCountry,
                o.a.PANName,
                o.a.PANNumber,
                o.a.AadhaarName,
                o.a.AadhaarNumberView,
                o.a.AadhaarNumber,
                Remarks = r?.Remarks,
                ReasonId = r?.ReasonId
            }).GroupBy(x => x.AppointeeId).Select(r => new ProcessedDataReportDetails
            {

                CandidateId = r.FirstOrDefault()?.CandidateId,
                AppointeeName = r.FirstOrDefault()?.AppointeeName,
                DateOfBirth = r.FirstOrDefault()?.DateOfBirth?.ToShortDateString(),
                MobileNo = r.FirstOrDefault()?.MobileNo,
                EmailId = r.FirstOrDefault()?.AppointeeEmailId,
                DateOfJoining = r.FirstOrDefault()?.DateOfJoining?.ToShortDateString(),
                Nationality = r.FirstOrDefault()?.Nationality,
                //Qualification = r.Qualification,
                QualificationName = r.FirstOrDefault()?.QualificationName,
                UANNumber = string.IsNullOrEmpty(r.FirstOrDefault()?.UANNumber) ? "NA" : CommonUtility.DecryptString(key, r.FirstOrDefault()?.UANNumber),
                //EPFWages = r.EPFWages,
                //Gender = r.Gender,
                GenderName = r.FirstOrDefault()?.GenderName,
                //MaratialStatus = r.MaratialStatus,
                MaratialStatusName = r.FirstOrDefault()?.MStatusName,
                MemberName = r.FirstOrDefault()?.MemberName,
                //MemberRelation = r.MemberRelation,
                MemberRelationName = r.FirstOrDefault()?.MemberRelation == "F" ? "Father" : r.FirstOrDefault()?.MemberRelation == "H" ? "Husband" : string.Empty,
                IsHandicap = string.IsNullOrEmpty(r.FirstOrDefault()?.IsHandicap) ? "NA" : r.FirstOrDefault()?.IsHandicap?.ToUpper() == "N" ? "No" : "Yes",
                HandicapeType = string.IsNullOrEmpty(r.FirstOrDefault()?.HandicapeType) ? "NA" : r.FirstOrDefault()?.HandicapeType,
                IsInternationalWorker = string.IsNullOrEmpty(r.FirstOrDefault()?.IsInternationalWorker) ? "NA" : r.FirstOrDefault()?.IsInternationalWorker?.ToUpper() == "N" ? "No" : "Yes",
                PassportNo = string.IsNullOrEmpty(r.FirstOrDefault()?.PassportNo) ? "NA" : CommonUtility.DecryptString(key, r.FirstOrDefault()?.PassportNo),
                PassportValidFrom = r.FirstOrDefault()?.PassportValidFrom == null ? "NA" : r.FirstOrDefault()?.PassportValidFrom?.ToShortDateString(),
                PassportValidTill = r.FirstOrDefault()?.PassportValidTill == null ? "NA" : r.FirstOrDefault()?.PassportValidTill?.ToShortDateString(),
                OriginCountry = string.IsNullOrEmpty(r.FirstOrDefault()?.OriginCountry) ? "NA" : r.FirstOrDefault()?.OriginCountry,
                PANName = r.FirstOrDefault()?.PANName,
                PANNumber = string.IsNullOrEmpty(r.FirstOrDefault()?.PANNumber) ? null : CommonUtility.DecryptString(key, r.FirstOrDefault()?.PANNumber),
                AadhaarName = r.FirstOrDefault()?.AadhaarName,
                AadhaarNumber = string.IsNullOrEmpty(r.FirstOrDefault()?.AadhaarNumber) ? "NA" : CommonUtility.DecryptString(key, r.FirstOrDefault()?.AadhaarNumber),
                Remarks = r?.Where(x => x?.ReasonId == othRsnCatgry.ReasonId)?.Select(y => y.Remarks)?
                       .Aggregate("", (current, s) => current + (s + ","))
            }).ToList();

            return processeddata;
        }
        public async Task<List<RejectedDataReportDetails>> GetRejectedAppointeeDetails(FilterRequest filter)
        {
            var key = _aadhaarConfig.EncriptKey;
            var pAppntedata = from r in _dbContextClass.RejectedFileData
                              join u in _dbContextClass.UnderProcessFileData
                                  on r.AppointeeId equals u.AppointeeId
                              join rp in _dbContextClass.AppointeeReasonMappingData
                              on r.AppointeeId equals rp.AppointeeId
                              join rm in _dbContextClass.ReasonMaser
                              on rp.ReasonId equals rm.ReasonId
                              join x in _dbContextClass.AppointeeDetails
                                on r.AppointeeId equals x.AppointeeId into grouping
                              from a in grouping.DefaultIfEmpty()
                              where r.ActiveStatus == true & rm.ActiveStatus == true
                              & (filter.FromDate == null || r.CreatedOn >= filter.FromDate)
                              & (filter.ToDate == null || r.CreatedOn <= filter.ToDate)
                              select new
                              {
                                  u.CandidateId,
                                  u.AppointeeId,
                                  u.AppointeeName,
                                  a.DateOfBirth,
                                  u.MobileNo,
                                  u.AppointeeEmailId,
                                  u.DateOfJoining,
                                  a.Nationality,
                                  a.Qualification,
                                  a.UANNumber,
                                  u.EPFWages,
                                  a.Gender,
                                  a.MaratialStatus,
                                  a.MemberName,
                                  a.MemberRelation,
                                  a.IsHandicap,
                                  a.HandicapeType,
                                  a.IsInternationalWorker,
                                  a.PassportNo,
                                  a.PassportValidFrom,
                                  a.PassportValidTill,
                                  a.OriginCountry,
                                  a.PANName,
                                  a.PANNumber,
                                  a.AadhaarName,
                                  a.AadhaarNumberView,
                                  a.AadhaarNumber,
                                  rp.Remarks
                              };

            var appointeelist = await pAppntedata.ToListAsync();
            // appointeelist.Where(x => x.)
            var RejectedAppointeeList = appointeelist.Select(r => new RejectedAppointeeDetails
            {
                CandidateId = r.CandidateId,
                AppointeeId = r.AppointeeId,
                AppointeeName = r.AppointeeName,
                DateOfBirth = r.DateOfBirth,
                MobileNo = r.MobileNo,
                EmailId = r.AppointeeEmailId,
                DateOfJoining = r.DateOfJoining,
                Nationality = r.Nationality,
                UANNumber = r.UANNumber,
                EPFWages = r.EPFWages ?? 0,
                PANNumber = r.PANNumber,
                //AadhaarName = r.AadhaarName,
                AadhaarNumber = r.AadhaarNumber,
                Remarks = r.Remarks
            }).ToList(); ;

            List<RejectedDataReportDetails> processeddata = GetRejectedViewData(RejectedAppointeeList);

            return processeddata;
        }

        private List<RejectedDataReportDetails> GetRejectedViewData(List<RejectedAppointeeDetails> rejectedAppointeeList)
        {
            var response = new List<RejectedDataReportDetails>();
            var GroupData = rejectedAppointeeList.GroupBy(x => x.AppointeeId).ToList();
            foreach (var item in GroupData.Select((value, index) => new { Value = value, Index = index }))
            {
                var x = item.Value;
                var ResonDetails = x.Select(y => y.Remarks).ToList();
                string Remarks = ResonDetails.Count > 0 ? string.Join(", ", ResonDetails) : "NA";
                var _data = x.Select(r =>

                    new RejectedDataReportDetails
                    {
                        CandidateId = r.CandidateId,
                        AppointeeName = r.AppointeeName,
                        DateOfBirth = string.IsNullOrEmpty(r.DateOfBirth?.ToShortDateString()) ? "NA" : r.DateOfBirth?.ToShortDateString(),
                        MobileNo = r.MobileNo,
                        EmailId = r.EmailId,
                        DateOfJoining = r.DateOfJoining?.ToShortDateString(),
                        Nationality = string.IsNullOrEmpty(r.Nationality) ? "NA" : r.Nationality,
                        UANNumber = string.IsNullOrEmpty(r.UANNumber) ? "NA" : CommonUtility.DecryptString(key, r.UANNumber),
                        PANNumber = string.IsNullOrEmpty(r.PANNumber) ? "NA" : CommonUtility.DecryptString(key, r.PANNumber),
                        AadhaarNumber = string.IsNullOrEmpty(r.AadhaarNumber) ? "NA" : CommonUtility.DecryptString(key, r.AadhaarNumber),
                        Remarks = Remarks

                    }).FirstOrDefault();
                response.Add(_data);
            }

            return response;
        }

        //public Task<List<ProcessDataResponse>> GetPfUserCreateAppointeeDetails(ProcessedFilterRequest filter)
        public async Task<List<ProcessDataResponse>> GetPfUserCreateAppointeeDetails(DownloadPfUserListRequest filter)
        {
            var querydata = from p in _dbContextClass.ProcessedFileData
                            join a in _dbContextClass.AppointeeDetails
                                on p.AppointeeId equals a.AppointeeId
                            where p.ActiveStatus == true & a.IsProcessed == true
                            //& p.DataUploaded == false
                            & (filter.FromDate == null || p.CreatedOn >= filter.FromDate)
                            & (filter.ToDate == null || p.CreatedOn <= filter.ToDate)
                            & (filter.IsDownloaded == null || p.DataUploaded == filter.IsDownloaded)
                            select new { a, p.DataUploaded };
            var appointeelist = await querydata.ToListAsync().ConfigureAwait(false);

            var _appointeeViewdata = appointeelist?.DistinctBy(x => x.a.AppointeeId)?.OrderBy(x => x.a.DateOfJoining)?.Select(row => new ProcessDataResponse
            {
                id = row.a.AppointeeDetailsId,
                companyId = row.a.CompanyId,
                candidateId = row.a.CandidateId,
                appointeeName = row.a.AppointeeName,
                appointeeId = row.a.AppointeeId,
                appointeeEmailId = row.a.AppointeeEmailId,
                mobileNo = row.a.MobileNo,
                adhaarNo = row.a.AadhaarNumberView,
                panNo = string.IsNullOrEmpty(row.a.PANNumber) ? null : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row.a.PANNumber)),
                //panNo = string.IsNullOrEmpty(row.a.PANNumber) ? null : CommonUtility.DecryptString(key, row.a.PANNumber),
                dateOfJoining = row.a.DateOfJoining,
                epfWages = row.a.EPFWages,
                uanNo = string.IsNullOrEmpty(row.a.UANNumber) ? null : CommonUtility.MaskedString(CommonUtility.DecryptString(key, row.a.UANNumber)),
                //uanNo = string.IsNullOrEmpty(row.a.UANNumber) ? null : CommonUtility.DecryptString(key, row.a.UANNumber),
                status = row.DataUploaded ?? false ? "Downloaded" : string.Empty,
                isPensionApplicable = row.a.IsPensionApplicable == null ? string.Empty : row.a.IsPensionApplicable ?? false ? "Yes" : "No",
            }).ToList();


            //List<PfCreateAppointeeDetailsResponse> processeddata = GetPfCreateProcessedViewData(appointeelist);

            return _appointeeViewdata;
        }
        private async Task<List<AppointeeDetails>> GetAppointeeList(DownloadPfUserListRequest filter)
        {
            var querydata = from p in _dbContextClass.ProcessedFileData
                            join a in _dbContextClass.AppointeeDetails
                                on p.AppointeeId equals a.AppointeeId
                            where p.ActiveStatus == true & a.IsProcessed == true
                            //& p.DataUploaded == false
                            & (filter.FromDate == null || p.CreatedOn >= filter.FromDate)
                            & (filter.ToDate == null || p.CreatedOn <= filter.ToDate)
                            & (filter.IsDownloaded == null || p.DataUploaded == filter.IsDownloaded)
                            select new { a, p.DataUploaded }.a;
            var appointeelist = await querydata.ToListAsync().ConfigureAwait(false);

            return appointeelist;
        }

        public async Task<List<PfCreateAppointeeDetailsResponse>> DownloadedPfUserCreateAppointeeDetails(DownloadPfUserListRequest filter)
        {
            var key = _aadhaarConfig.EncriptKey;
            var querydata = from p in _dbContextClass.ProcessedFileData
                            join a in _dbContextClass.AppointeeDetails
                                on p.AppointeeId equals a.AppointeeId
                            where p.ActiveStatus == true & a.IsProcessed == true
                            //& p.DataUploaded == false
                            & (filter.FromDate == null || p.CreatedOn >= filter.FromDate)
                            & (filter.ToDate == null || p.CreatedOn <= filter.ToDate)
                            & (filter.IsDownloaded == null || p.DataUploaded == filter.IsDownloaded)
                            select new { a, p };
            var appointeelist = await querydata.ToListAsync().ConfigureAwait(false);

            var processedList = appointeelist?.Select(x => x.p).ToList();
            foreach (var obj in processedList)
            {
                obj.UpdatedOn = DateTime.Now;
                obj.DataUploaded = true;
            }

            await _dbContextClass.SaveChangesAsync();


            var processeddata = appointeelist?.DistinctBy(x => x.a.AppointeeId)?
                .OrderBy(x => x.a.DateOfJoining)?.Select(r => new PfCreateAppointeeDetailsResponse
                {
                    AppointeeName = r.a.AppointeeName,
                    DateOfBirth = r.a.DateOfBirth?.ToShortDateString(),
                    MobileNo = r.a.MobileNo,
                    EmailId = r.a.AppointeeEmailId,
                    DateOfJoining = r.a.DateOfJoining?.ToShortDateString(),
                    Nationality = r.a.Nationality,
                    Qualification = r.a.Qualification,
                    UANNumber = string.IsNullOrEmpty(r.a.UANNumber) ? null : CommonUtility.DecryptString(key, r.a.UANNumber),
                    EPFWages = r.a.EPFWages,
                    Gender = r.a.Gender,
                    MaratialStatus = r.a.MaratialStatus,
                    MemberName = r.a.MemberName,
                    MemberRelation = r.a.MemberRelation,
                    IsHandicap = r.a.IsHandicap,
                    HandicapeType = r.a.HandicapeType,
                    IsInternationalWorker = r.a.IsInternationalWorker,
                    PassportNo = string.IsNullOrEmpty(r.a.PassportNo) ? null : CommonUtility.DecryptString(key, r.a.PassportNo),
                    PassportValidFrom = r.a.PassportValidFrom?.ToShortDateString(),
                    PassportValidTill = r.a.PassportValidTill?.ToShortDateString(),
                    OriginCountry = r.a.OriginCountry,
                    PANName = r.a.PANName,
                    PANNumber = string.IsNullOrEmpty(r.a.PANNumber) ? "NA" : CommonUtility.DecryptString(key, r.a.PANNumber),
                    AadhaarName = r.a.AadhaarName,
                    AadhaarNumber = string.IsNullOrEmpty(r.a.AadhaarNumber) ? "NA" : CommonUtility.DecryptString(key, r.a.AadhaarNumber),
                }).ToList();


            return processeddata;
        }
        public async Task<List<ApiCountJobResponse>> ApiCountReport(DateTime? FromDate, DateTime? ToDate)
        {
            //DateTime _currDate = DateTime.Today;
            // Your background job logic here

            List<ApiCountJobResponse> ReportRes = new List<ApiCountJobResponse>();
            var totalApiList = await _dbContextClass.ApiCounter.Where(m =>
              (FromDate == null || m.CreatedOn >= FromDate)
            & (ToDate == null || m.CreatedOn <= ToDate)).ToListAsync();

            var DateWiseTotalRequestApiList = totalApiList?.Where(x => x?.Type == "Request")?.GroupBy(x => x.CreatedOn?.ToShortDateString())?.ToList();
            var TotalResponseApiList = totalApiList?.Where(x => x?.Type == "Response")?.ToList();


            foreach (var TotalApiPerDate in DateWiseTotalRequestApiList)
            {
                string _currDate = TotalApiPerDate?.Key?.ToString();
                var _currTotalData = TotalApiPerDate?.ToList();

                var TotalApiCount = _currTotalData?.Where(x => x?.Type == "Request")?.GroupBy(x => x.ApiName)?.Select(y => new ApiCountJobResponse
                {
                    ApiName = y.Key?.ToLower(),
                    TotalApiCount = y?.ToList()?.Count() ?? 0,
                })?.ToList();

                var DateWiseTotalResponseApiList = TotalResponseApiList?.Where(x => x?.CreatedOn?.ToShortDateString() == _currDate)?.ToList();

                var TotalSuccessApiCount = DateWiseTotalResponseApiList?.Where(x => x?.Status == (Int32)HttpStatusCode.OK)?.GroupBy(x => x.ApiName)?.Select(y => new ApiCountJobResponse
                {
                    ApiName = y?.Key,
                    TotalSuccessApiCount = y?.ToList()?.Count() ?? 0,
                })?.ToList();

                var TotalUnproceesbleApiCount = DateWiseTotalResponseApiList?.Where(x => x?.Status == (Int32)HttpStatusCode.UnprocessableEntity)?.GroupBy(x => x.ApiName)?.Select(y => new ApiCountJobResponse
                {
                    ApiName = y?.Key,
                    TotalUnprocessableEntityCount = y?.ToList()?.Count() ?? 0,
                    //TotalApiCount = TotalApiCount?.Where(x => x.ApiName?.ToLower() == y?.Key?.ToLower())?.ToList()?.Count() ?? 0
                })?.ToList();

                var TotalFaliureApiCount = DateWiseTotalResponseApiList?.Where(x => !(x?.Status == (int)HttpStatusCode.UnprocessableEntity || x?.Status == (int)HttpStatusCode.OK))?.GroupBy(x => x.ApiName)?.Select(y => new ApiCountJobResponse
                {
                    ApiName = y?.Key,
                    TotalFailureCount = y?.ToList()?.Count() ?? 0,
                })?.ToList();
                //var count = 0;
                foreach (var obj in TotalApiCount)
                {

                    obj.Date = _currDate;
                    obj.TotalSuccessApiCount = TotalSuccessApiCount?.Where(x => x.ApiName?.ToLower() == obj?.ApiName?.ToLower())?.FirstOrDefault()?.TotalSuccessApiCount ?? 0;
                    obj.TotalUnprocessableEntityCount = TotalUnproceesbleApiCount?.Where(x => x.ApiName?.ToLower() == obj?.ApiName?.ToLower())?.FirstOrDefault()?.TotalUnprocessableEntityCount ?? 0;
                    obj.TotalFailureCount = TotalFaliureApiCount?.Where(x => x.ApiName?.ToLower() == obj?.ApiName?.ToLower())?.FirstOrDefault()?.TotalFailureCount ?? 0;
                    //count++;
                }
                ReportRes.AddRange(TotalApiCount);

            }
            return ReportRes;
            // This method will be executed asynchronously by Hangfire.
        }

        public async Task<AppointeeCountDateWiseDetails> AppointeeCountReport(AppointeeCountReportSearchRequest reqObj)//DateTime? FromDate, DateTime? ToDate)
        {
            AppointeeCountDateWiseDetails _response = new AppointeeCountDateWiseDetails();
            List<AppointeeCountDetails>? _apntDetailsList = new List<AppointeeCountDetails>();
            List<AppointeeTotalCount> _appointeeTotalCountList = new List<AppointeeTotalCount>();
            List<AppointeeCountDateWise> _appointeeCountDateWises = new List<AppointeeCountDateWise>();
            List<AppointeeCountDateWise> _appointeeNonProcessDateWise = new List<AppointeeCountDateWise>();
            if (reqObj.StatusCode == ReportFilterStatus.LinkNotSent || string.IsNullOrEmpty(reqObj.StatusCode))
            {



                var nonProcessQueryData = from ap in _dbContextClass.UploadAppointeeCounter
                                          .Where(m => (reqObj.FromDate == null || m.CreatedOn >= reqObj.FromDate)
                                          & (reqObj.ToDate == null || m.CreatedOn <= reqObj.ToDate))
                                          join a in _dbContextClass.UnProcessedFileData
                                          on ap.FileId equals a.FileId
                                          where (string.IsNullOrEmpty(reqObj.AppointeeName) ||
                                          a.AppointeeName.Contains(reqObj.AppointeeName))
                                          select new
                                          {
                                              a.AppointeeName,
                                              a.AppointeeEmailId,
                                              a.CandidateId,
                                              a.DateOfJoining,
                                              ap.CreatedOn,
                                          };

                var nonProcessAppointeeList = await nonProcessQueryData.ToListAsync().ConfigureAwait(false);

                var _noProcessAppointeeCountdateWise = nonProcessAppointeeList.GroupBy(x => x.CreatedOn?.ToString("dd-MM-yyyy"))?.ToList();

                foreach (var (_currdata, _totalCount, _currDate, _currDateWiseCount, _currAppointeeCount) in from obj in _noProcessAppointeeCountdateWise
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
                        EmailId = x?.AppointeeEmailId,
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
                    _appointeeNonProcessDateWise.Add(_currDateWiseCount);
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

                var underProcessQueryData = from ap in _dbContextClass.UploadAppointeeCounter
                                        .Where(m => (reqObj.FromDate == null || m.CreatedOn >= reqObj.FromDate)
                                        & (reqObj.ToDate == null || m.CreatedOn <= reqObj.ToDate))
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
                                            & (string.IsNullOrEmpty(_statusCode) || wm.AppvlStatusCode == _statusCode)
                                            & (_intSubmitCode == null || (p.IsSubmit == _intSubmitCode))
                                            & (_intSubStatusCode == null || (_intSubStatusCode == 1 & p.SaveStep == _intSubStatusCode & p.IsSubmit != true)
                                            || (_intSubStatusCode == 0 & p.SaveStep != 1 & p.IsSubmit != true))
                                            select new
                                            {
                                                a.AppointeeName,
                                                a.AppointeeEmailId,
                                                a.CandidateId,
                                                a.DateOfJoining,
                                                ap.CreatedOn,
                                                w.AppvlStatusId,
                                                w.ActionTakenAt,
                                                wm.AppvlStatusDesc,
                                                wm.AppvlStatusCode,
                                                p.UpdatedOn,
                                                p.SaveStep,
                                                p.IsSubmit
                                            };
                var underProcessAppointeeList = await underProcessQueryData.ToListAsync().ConfigureAwait(false);

                var _appointeeCountdateWise = underProcessAppointeeList.GroupBy(x => x.CreatedOn?.ToString("dd-MM-yyyy"))?.ToList();
                foreach (var (_currdata, _totalCount, _currDate, _currDateWiseCount, _nonProcessData, _currAppointeeCount) in from obj in _appointeeCountdateWise
                                                                                                                              let _currdata = obj?.ToList()
                                                                                                                              let _totalCount = _currdata?.Count ?? 0
                                                                                                                              let _currDate = obj?.Key
                                                                                                                              let _nonProcessData = _appointeeNonProcessDateWise?.Where(x => x?.appointeeTotalCount?.Date == _currDate).FirstOrDefault()
                                                                                                                              let _currDateWiseCount = new AppointeeCountDateWise()
                                                                                                                              let _currAppointeeCount = new AppointeeTotalCount()
                                                                                                                              select (_currdata, _totalCount, _currDate, _currDateWiseCount, _nonProcessData, _currAppointeeCount))
                {

                    //var _date = _currdata;

                    List<AppointeeCountDetails>? _apntDetails = _currdata?.Select(x => new AppointeeCountDetails
                    {
                        AppointeeName = x?.AppointeeName,
                        CandidateId = x?.CandidateId,
                        EmailId = x?.AppointeeEmailId,
                        ActionTaken = (x?.AppvlStatusCode != WorkFlowType.ProcessIni?.Trim() && x?.SaveStep == 1) ? x?.UpdatedOn?.ToString("dd-MM-yyyy") ?? x?.ActionTakenAt?.ToString("dd-MM-yyyy")
                                     : x?.ActionTakenAt?.ToString("dd-MM-yyyy"),
                        AppointeeStatus = x?.AppvlStatusCode == WorkFlowType.ProcessIni?.Trim() ?
                                     x?.AppvlStatusDesc + "(" + (x?.IsSubmit ?? false ? "Submitted" : x?.SaveStep == 1 ? "Ongoing" : "No Response") + ")"
                                     : x?.AppvlStatusDesc,
                        Date = _currDate,
                    })?.ToList();

                    if (_nonProcessData != null)
                    {
                        _apntDetails?.AddRange(_nonProcessData.AppointeeCountDetails);
                    }
                    _currAppointeeCount.TotalAppointeeCount = (_nonProcessData != null) ? (_nonProcessData?.appointeeTotalCount?.TotalAppointeeCount ?? 0) + _totalCount : _totalCount;
                    _currAppointeeCount.TotalLinkNotSentCount = (_nonProcessData != null) ? _nonProcessData?.appointeeTotalCount?.TotalLinkNotSentCount ?? 0 : 0;
                    _currAppointeeCount.TotalLinkSentCount = _totalCount;
                    _currAppointeeCount.Date = _currDate;
                    _currDateWiseCount.appointeeTotalCount = _currAppointeeCount;
                    _currDateWiseCount.AppointeeCountDetails = _apntDetails;
                    _apntDetailsList.AddRange(_apntDetails);
                    _appointeeCountDateWises.Add(_currDateWiseCount);
                    _appointeeTotalCountList.Add(_currAppointeeCount);
                }
            }
            _response.AppointeeCountDateWise = _appointeeCountDateWises;
            _response.AppointeeCountDetails = _apntDetailsList;
            _response.AppointeeTotalCount = _appointeeTotalCountList;

            return _response;
        }
    }
}
