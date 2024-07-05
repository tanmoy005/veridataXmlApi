
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Response;

namespace VERIDATA.BLL.Interfaces
{
    public interface ICandidateContext
    {
        public Task<AppointeeDetailsResponse> GetAppointeeDetailsAsync(int appointeeId);
        public Task<CandidateValidateResponse> UpdateCandidateValidateData(CandidateValidateUpdatedDataRequest validationReq);
        public Task<List<GetRemarksResponse>> GetRemarks(int appointeeId);
        public Task<string?> GetRemarksRemedy(int remarksId);
        public Task<List<AppointeeActivityDetailsResponse>> GetActivityDetails(int appointeeId);
        public Task<AppointeePassbookDetailsViewResponse> GetPassbookDetailsByAppointeeId(int appointeeId);
        public Task<AppointeeEmployementDetailsViewResponse> GetGetEmployementDetailsByAppointeeId(int appointeeId);


    }
}
