
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Public;

namespace VERIDATA.BLL.Interfaces
{
    public interface ICandidateContext
    {
        public Task<AppointeeDetailsResponse> GetAppointeeDetailsAsync(int appointeeId);
        public Task<CandidateValidateResponse> UpdateCandidateValidateData(CandidateValidateUpdatedDataRequest validationReq);
        public Task<List<GetRemarksResponse>> GetRemarks(int appointeeId);
        public Task<string?> GetRemarksRemedy(GetRemarksRemedyRequest reqObj);
        public Task<List<AppointeeActivityDetailsResponse>> GetActivityDetails(int appointeeId);
        public Task<AppointeePassbookDetailsViewResponse> GetPassbookDetailsByAppointeeId(int appointeeId);
        public Task<AppointeeEmployementDetailsViewResponse> GetEmployementDetailsByAppointeeId(int appointeeId, int userId);
        public Task UpdateCandidateUANData(int appointeeId, string UanNumber);
        public Task PostAppointeefileUploadAsync(AppointeeFileDetailsRequest AppointeeFileDetails);
        public Task PostAppointeeTrusUanDetailsAsync(AppointeeUpdatePfUanDetailsRequest AppointeeTrustDetails);
        public Task PostAppointeeHandicapDetailsAsync(AppointeeHadicapFileDetailsRequest AppointeeHandicapDetails);
        public Task<AppointeeEmployementDetails> PostAppointeeEmployementDetailsAsync(EmployementHistoryDetails reqObj);
        public Task<AppointeeDetails> PostAppointeepensionAsync(AppointeeApprovePensionRequest reqObj);
        public Task<AppointeeDetails> VerifyAppointeeManualAsync(AppointeeApproveVerificationRequest reqObj);
        public Task<AppointeeFileViewDetailResponse> GetNotVeriedfileView(AppointeeNotVerifiedFileViewRequest reqObj);
       



    }
}
