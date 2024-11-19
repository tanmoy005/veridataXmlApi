using VERIDATA.Model.DataAccess;
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
        public Task<List<AppointeeDataFilterReportDetails>> AppointeeDetailsReport(AppointeeDataFilterReportRequest reqObj);
        public Task<List<AppointeeDataPfReportResponse>> AppointeePfDetailsReport(AppointeeDataFilterReportRequest reqObj);
        public Task<List<AppointeePfStatusDataFilterReportResponse>> AppointeePfDetailsFileterReport(AppointeePfDataFilterReportRequest reqObj);
        public Task<List<AppointeePfDataExcelRespopnse>> GetAppointeePfDataExcelReport(List<AppointeePfStatusDataFilterReportResponse> reqObj);
        public Task<List<ManualVerificationExcelDataResponse>> GetAppointeeManualVerificationExcelReport(List<ManualVerificationProcessDetailsResponse> reqObj);

    }
}
