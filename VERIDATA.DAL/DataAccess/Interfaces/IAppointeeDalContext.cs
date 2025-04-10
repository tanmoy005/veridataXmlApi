﻿using VERIDATA.Model.DataAccess;
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

        public Task UpdateAppointeeAahdaarImage(int appointeeId, string candidateId, int userId, string imageBase64Data);

        public Task UpdateAppointeeUanNumber(int appointeeId, string uanNumber);

        public Task<string> UpdateRemarksByType(int AppointeeId, List<ReasonRemarks> Reasons, string Type, int UserId);

        public Task<string> UpdateRemarksByType(int AppointeeId, List<ReasonRemarks> Reasons, string Type, int UserId, string subType);

        public Task UpdateRemarksStatusByType(int AppointeeId, string Type, string subType, int UserId);

        public Task Uploadfiles(List<AppointeeUploadDetails> uploadDetails);

        public Task RemovePrevfiles(List<AppointeeUploadDetails> prevDocDetails, int userId);

        public Task UpdateAppointeeSubmit(int AppointeeId, bool IsSubmit, bool? IsManualPassbookUploaded);

        public Task UpdateAppointeeTrustnUanAvailibility(int AppointeeId, bool? TrustPassbookAvailable, bool? IsUanAvailable, bool? IsFinalSubmit);

        public Task UpdateAppointeeHandicapDetails(int AppointeeId, string? IsHandicap, string? HandicapType);

        public Task<List<GetRemarksResponse>> GetRemarks(int appointeeId);

        public Task<string?> GetRemarksRemedy(int remarksId);

        public Task<string?> GetRemarksRemedyByCode(string ReasonType, string remarksCode);

        public Task<UploadTypeMaster> getFileTypeDataByAliasAsync(string? fileTypeAlias);

        public Task<AppointeeConsentMapping> getAppointeeContestAsync(int? appointeeId);

        public Task postAppointeeContestAsync(AppointeeConsentSubmitRequest req);

        public Task PostOfflineKycStatus(OfflineAadharVarifyStatusUpdateRequest reqObj);

        public Task PostMailTransDetails(mailTransactionRequest reqObj);

        public Task<List<mailTransactionResponse>> GetMailTransDetails(int appointeeId, int userId);

        public Task<AppointeeEmployementDetails> PostEmployementDetails(EmployementHistoryDetails reqObj);

        public Task<UserCredetialDetailsResponse> GetUserCredentialInfo(int RefAppointeeId);

        public Task<AppointeeEmployementDetails> GetEmployementDetails(int appointeeId, string type);

        public Task<List<AppointeeUploadDetails>> GetAppinteeUploadDetails(int appointeeId, string uploadTypeCode);

        public Task<AppointeeUploadDetails> GetAppinteeUploadDetailsById(int appointeeId, int? uploadFileId);

        public Task<AppointeeDetails> UpdateAppinteePensionById(AppointeeApprovePensionRequest reqObj);

        public Task<AppointeeDetails> VefifyAppinteeFathersNameManualById(int appointeeId, bool? isValid, string type, int userId);

        public Task<AppointeeDetails> VefifyAppinteePfDetailsManualById(AppointeePfVerificationRequest reqObj);
        public Task UpdateAppinteeDocAvailibilityById(DoctypeAvailibilityUpdateRequest reqObj);
    }
}