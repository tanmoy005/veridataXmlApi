using VERIDATA.Model.Request;
using VERIDATA.Model.Response;

namespace VERIDATA.BLL.Interfaces
{
    public interface IWorkFlowContext
    {

        public Task<List<ProcessDataResponse>> GetProcessDataAsync(ProcessedFilterRequest filter);
        public Task<List<RejectedDataResponse>> GetRejectedDataAsync(FilterRequest filter);
        public Task<List<UnderProcessDetailsResponse>> GetUnderProcessDataAsync(AppointeeSeacrhFilterRequest reqObj);
        public Task<List<ManualVerificationProcessDetailsResponse>> GetManualVeificationProcessData(ManualVeificationProcessDataRequest reqObj);
        public Task<List<UnderProcessDetailsResponse>> GetExpiredProcessDataAsync(AppointeeSeacrhFilterRequest reqObj);
        public Task<List<CriticalAppointeeResponse>> GetCriticalAppointeeList(CriticalFilterDataRequest reqObj);
        public Task<List<RawFileDataDetailsResponse>> GetNonProcessDataAsync(AppointeeSeacrhFilterRequest reqObj);
        public Task<List<RawFileDataDetailsResponse>> getRawfileData(int? fileId, int companyId);
        public Task<List<UnderProcessDetailsResponse>> ProcessRawData(RawDataProcessRequest rawdfileata);
        public Task UpdateAppointeeDojByAdmin(CompanySaveAppointeeDetailsRequest AppointeeDetails);
        public Task PostAppointeeSaveDetailsAsync(AppointeeSaveDetailsRequest AppointeeDetails);
        public Task PostAppointeeFileDetailsAsync(AppointeeFileDetailsRequest AppointeeFileDetails);
        public Task<List<GetAppointeeGlobalSearchResponse>> GetAppointeeSearchGlobal(string Name);
        public Task<List<DropDownDetailsResponse>> GetAllReportFilterStatus();
        public Task<string?> AppointeeWorkflowCurrentState(int appointeeId);
        public Task PostAppointeeApprove(AppointeeApproverRequest request);
        public Task PostAppointeeRejected(AppointeeApproverRequest request);
        public Task PostAppointeeClose(AppointeeApproverRequest request);
        public Task PostRemainderMail(int appointeeId, int UserId);
        public Task<VarificationStatusResponse> ValidateRemainderMail(int appointeeId, int UserId);
        public Task PostMailResend(int appointeeId, int UserId);
        public Task<List<FileCategoryResponse>> getFileType(int appointeeId);
        public Task<bool> VerifyAppointeeManualAsync(AppointeeApproveVerificationRequest reqObj);

        public Task<List<FileCategoryResponse>> GetNotVeriedfileView(int appointeeId);
        public Task PostAppointeeFileReuploadDetailsAsync(AppointeeReUploadFilesAfterSubmitRequest AppointeeFileDetailsReupload);
    }
}
