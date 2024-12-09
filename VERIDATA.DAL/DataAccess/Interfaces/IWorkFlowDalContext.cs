
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Master;
using VERIDATA.Model.Table.Public;

namespace VERIDATA.DAL.DataAccess.Interfaces
{
    public interface IWorkFlowDalContext
    {
        public Task<WorkflowApprovalStatusMaster?> GetApprovalState(string approvalStatus);
        public Task<int> GetWorkFlowStateIdByAlias(string StateAlias);
        public Task<WorkFlowDetails?> GetCurrentApprovalStateByAppointeeId(int appointeeId);
        public Task<List<ProcessedDataDetailsResponse>> GetProcessedAppointeeDetailsAsync(ProcessedFilterRequest filter);
        public Task<List<RejectedDataDetailsResponse>> GetRejectedAppointeeDetailsAsync(FilterRequest filter);
        public Task<List<UnderProcessQueryDataResponse>> GetUnderProcessDataAsync(AppointeeSeacrhFilterRequest reqObj);
        public Task<List<UnderProcessQueryDataResponse>> GetUnderProcessDataByDOJAsync(DateTime? startDate, DateTime? endDate, DateTime? FromDate, DateTime? ToDate);
        public Task<List<int?>?> GetTotalOfferAppointeeList(int FilterDays);
        public Task<int> PostUploadedXSLfileAsync(string? fileName, string? filepath, int? companyid);
        public Task PostRawfiledataAsync(RawdataSubmitRequest data);
        public Task<List<RawFileData>> GetRawfiledataByIdAsync(int? fileId, int companyId);
        public Task<List<RawFileData>> GetRawfiledataAsync();
        //public Task<List<UnProcessedFileData>> GetUnProcessDataAsync(DateTime? startDate);
        public Task<List<UnProcessedFileData>> GetUnProcessDataAsync(AppointeeSeacrhFilterRequest reqObj);
        public Task<List<RejectedFileData>> GetRejectedDataAsync(DateTime? startDate);
        public Task<List<UnProcessedFileData>> GetCriticalUnProcessDataAsync(DateTime? startDate, DateTime? endDate, DateTime? FromDate, DateTime? ToDate);
        public Task<List<WorkFlowDetailsHist>> GetTotalDataAsync(DateTime? startDate);
        public Task<List<RawFileDataDetailsResponse>> GetRawfiledetailsByTypeId(List<RawDataRequest> rawDataList, int? userId, int type);
        public Task<List<RawFileDataDetailsResponse>> GetNonProcessedDetailsByTypeId(List<RawDataRequest> rawDataList, int? userId);
        public Task<List<UnderProcessFileData>> PostUnderProcessDataAsync(List<RawFileDataDetailsResponse> underprocessdata, int userId);
        public Task PostNonProcessDataAsync(List<RawFileDataDetailsResponse> unprocessdata, int userId);
        public Task RemoveRawDataAsync(List<int> RemoveRawId);
        public Task RemoveUnprocessedDataAsync(List<int> RemoveRawId);
        public Task AppointeeWorkflowIniAsync(List<int?> appointeeList, int workflowState, int userId);
        public Task UpdateAppointeeDojByAdmin(CompanySaveAppointeeDetailsRequest appointeeDetails);
        public Task PostAppointeeSaveDetailsAsync(AppointeeSaveDetailsRequest AppointeeDetails);
        public Task AppointeeWorkflowUpdateAsync(WorkFlowDataRequest WorkFlowRequest);
        public Task workflowdataUpdate(WorkFlowDataRequest WorkFlowRequest, WorkflowApprovalStatusMaster? approvalState, int _reprocessCount, WorkFlowDetails? _workFlow_det);
        public Task<List<GlobalSearchAppointeeData>> GetUnderProcessAppointeeSearch(string Name);
        public Task<List<GlobalSearchAppointeeData>> GetAppointeeSearchDetails(string name, string type);
        public Task<List<UnderProcessWithActionQueryDataResponse>> GetUnderProcessDataWithActionAsync(AppointeeSeacrhFilterRequest reqObj);
        public Task<List<PfStatusDataFilterQueryResponse>> GetAppointeePfdetailsAsync(PfDataFilterReportRequest reqObj);
        public Task<List<FileCategoryResponse>> getFileTypeCode(int appointeeId);
        Task<bool> CheckVerifyDetails(int appointeeId);
        public Task<List<ManualVerificationProcessQueryDataResponse>> GetManualVerificationProcessDataAsync(ManualVeificationProcessDataRequest reqObj);
        public Task UpdateReuploadFathersName(AppointeeReUploadFilesAfterSubmitRequest reqObj);

    }
}
