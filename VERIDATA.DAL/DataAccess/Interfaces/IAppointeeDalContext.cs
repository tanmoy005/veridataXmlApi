using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Master;
using VERIDATA.Model.Table.Public;

namespace VERIDATA.DAL.DataAccess.Interfaces
{
    public interface IAppointeeDalContext
    {
        public Task<AppointeeDetails> GetAppinteeDetailsById(int appointeeId);
        public Task<UnderProcessFileData> GetUnderProcessAppinteeDetailsById(int appointeeId);
        public Task<List<AppointeeUploadDetails>> GetAppinteeUploadDetails(int appointeeId);
        public Task<List<ReasonMaser>> GetAllRemarksByType(string Type);
        public Task UpdateAppointeeVerifiedData(CandidateValidateUpdatedDataRequest validationReq);
        public Task<string> UpdateRemarksByType(int AppointeeId, List<ReasonRemarks> Reasons, string Type, int UserId);
        public Task UpdateRemarksStatusByType(int AppointeeId, string Type, int UserId);
        public Task uploadFilesNUpdatePrevfiles(AppointeeUploadDetails uploadDetails, AppointeeUploadDetails prevDocDetails, int userId);
        public Task UpdateAppointeeSubmit(int AppointeeId, bool TrustPassbookAvailable, bool IsSubmit);
        public Task<List<GetRemarksResponse>> GetRemarks(int appointeeId);
        public Task<string?> GetRemarksRemedy(int remarksId);
        public Task<UploadTypeMaster> getFileTypeDataByAliasAsync(string? fileTypeAlias);
        public Task<AppointeeConsentMapping> getAppointeeContestAsync(int? appointeeId);
        public Task postAppointeeContestAsync(AppointeeConsentSubmitRequest req);
        public Task PostOfflineKycStatus(OfflineAadharVarifyStatusUpdateRequest reqObj);

    }
}
