using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;

namespace VERIDATA.BLL.Interfaces
{
    public interface IReportingContext
    {
        public Task<List<ProcessedDataReportDetailsResponse>> GetApporvedAppointeeDetails(ProcessedFilterRequest filter);
        public Task<List<RejectedDataReportDetailsResponse>> GetRejectedAppointeeDetails(FilterRequest filter);
        public Task<List<PfUserListResponse>> GetPfUserCreateAppointeeDetails(PfUserListRequest filter);
        public Task<List<PfCreateAppointeeDetailsResponse>> DownloadedPfUserCreateAppointeeDetails(PfUserListRequest filter);
        public List<UnderProcessedDataReportDetails> GetUnderProcessDetails(List<UnderProcessDetailsResponse> reqList);
        public Task<ApiCountReportResponse> ApiCountReport(DateTime? FromDate, DateTime? ToDate);
        public Task<AppointeeCountDateWiseDetails> AppointeeCountReport(AppointeeCountReportSearchRequest reqObj);// DateTime? FromDate, DateTime? ToDate);
        public Task<List<AppointeeAgingDataReportDetails>> AppointeeDetailsAgingReport(GetAgingReportRequest reqObj);
        public Task<List<AppointeeNationalityDataReportDetails>> AppointeeNationalityDetailsReport(GetNationalityReportRequest reqObj);//DateTime? FromDate, DateTime? ToDate)


    }
}
