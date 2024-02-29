using PfcAPI.Model.Appointee;
using PfcAPI.Model.DataAccess;
using PfcAPI.Model.Master;
using PfcAPI.Model.RequestModel;
using PfcAPI.Model.ResponseModel;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;

namespace PfcAPI.Infrastucture.Interfaces
{
    public interface IWorkFlowContext
    {
        public Task<List<UnderProcessFileData>> PostUnderProcessDataAsync(List<RawFileDataModel> underprocessdata, int userId);
        public Task PostNonProcessDataAsync(List<RawFileDataModel> unprocessdata, int userId);
        public Task<List<UnderProcessDetailsResponse>> GetUnderProcessDataAsync(AppointeeSeacrhFilterRequest reqObj);
        public Task<List<UnderProcessDetailsResponse>> GetExpiredProcessDataAsync(AppointeeSeacrhFilterRequest reqObj);
        public Task<List<UnderProcessDetailsResponse>> GetReProcessDataAsync(int? companyId);
        public Task<List<RawFileDataModel>> GetNonProcessDataAsync(AppointeeSeacrhFilterRequest reqObj);
        public Task<List<ProcessDataResponse>> GetProcessDataAsync(ProcessedFilterRequest filter);
        public Task<List<ProcessDataResponse>> GetProcessEPFODataAsync(FilterRequest filter);
        public Task<List<ProcessDataResponse>> GetProcessMISDataAsync(FilterRequest filter);
        public Task<List<RejectedDataResponse>> GetRejectedDataAsync(FilterRequest filter);
        public Task RemoveRawDataAsync(List<int> RemoveRawId);
        public Task<List<RawFileDataModel>> GetRawfiledataById(List<RawDataRequest> RawDataList, int? userId, int type);
        public Task RemoveUnprocessedDataAsync(List<int> RemoveRawId);
        public Task<int> GetWorkFlowStateId(string StateAlias);
        public Task<int> GetWorkFlow_Approval_StatusId(string StateAlias);
        public Task<string?> Appointee_Workflow_CurrentState(int appointeeId);
        public Task Appointee_Workflow_Ini_Async(List<int?> appointeeList, int workflowState, int userId);
        public Task Appointee_Workflow_Update_Async(WorkFlowDataRequest WorkFlowRequest);
        public Task CompanyAdminSaveAppointeeDetailsAsync(CompanySaveAppointeeDetailsRequest AppointeeDetails);
        public Task PostAppointeeSaveDetailsAsync(AppointeeSaveDetailsRequest AppointeeDetails);
        public Task PostAppointeeFileDetailsAsync(AppointeeFileDetailsRequest AppointeeFileDetails);
        public Task<AppointeeDetails> UpdateAadharDetails(AadhaarValidateUpdateRequest AppointeeAadharDetails);
        public Task PostRawfiledataAsync(RawdataSubmitRequest data);
        public Task<AppointeeDetailsResponse> GetAppointeeDetailsAsync(int appointeeId);
        public Task<List<RawFileData>> GetRawfiledataAsync(int? fileId, int companyId);
        public Task<CandidateValidateResponse> UpdateIsValidAadhaarORUan(AadhaarValidateUpdateRequest validationReq);
        public Task UpdateIsPfVarification(int AppointeeId, bool isRequired);
        public Task<List<GetRemarksResponse?>> GetRemarks(int appointeeId);
        public Task<string?> GetRemarksRemedy(int RemarksId);
        public Task ModifyAadhaarValidatedField(ModifyAadhaarValidatedFieldRequest Request);
        public Task<string> UpdateRemarksByType(int AppointeeId, List<ReasonRemarks?> Reasons, string Type, int UserId);
        public Task<WorkflowApprovalStatusMaster> GetApprovalState(string approvalStatus);
        public Task<List<CriticalAppointeeResponse>> GetCriticalAppointeeList(CriticalFilterData reqObj);
        public Task<List<GetAppointeeGlobalSearchResponse>> GetAppointeeSearchGlobal(string Name);
        public Task<List<DropDownDetails>> GetAllReportFilterStatus();
        public Task<GetPassbookDetailsResponse> GetPassbookDetails(int appointeeId);

    }
}
