
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Config;
using VERIDATA.Model.Table.Public;

namespace VERIDATA.DAL.DataAccess.Interfaces
{
    public interface IReportingDalContext
    {
        public Task<List<ProcessedDataReportDetailsResponse>> GetProcessedAppointeeReportDetailsAsync(List<ProcessedDataDetailsResponse> AppointeeList);
        public Task<List<PfCreationProcessedReportResponse>> GetPfCreationProcessedReportDetailsAsync(PfUserListRequest filter);
        public Task UpdateDownloadedProcessData(List<ProcessedFileData> data);
        public Task<List<ApiCounter>> GetTotalApiList(DateTime? FromDate, DateTime? ToDate);
        public Task<List<NonProcessCandidateReportDataResponse>> GetNonProcessCandidateReport(AppointeeCountReportSearchRequest reqObj);
        public Task<List<NationalityQueryDataResponse>> GetCandidateNationalityReport(GetNationalityReportRequest reqObj);
        public Task<List<UnderProcessCandidateReportDataResponse>> GetUnderProcessCandidateReport(AppointeeCountReportSearchRequest reqObj, string? _statusCode, bool? _intSubmitCode, int? _intSubStatusCode);

    }
}
