using VERIDATA.Model.DataAccess;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Response.api.Karza;
using VERIDATA.Model.Response.api.Signzy;

namespace VERIDATA.BLL.apiContext.signzy

{
    public interface IsignzyApiContext
    {
        public Task<PanDetails> GetPanDetails(string panNo, int userId);
        public Task<GetCandidateUanDetails> GetUanFromMobilenPan(string panNo, string mobileNo, int userId);
        public Task<PassportDetails> GetPassportDetails(AppointeePassportValidateRequest reqObj);
        public Task<UanGenerateOtpDetails> GenerateUANOTP(string UanNumber, string PhoneNumber, int userId);
        public Task<UanSubmitOtpDetails> SubmitUanOTP(string clientId, string otp, int userId);
        public Task<PfPassbookDetails> GetPassbook(string clientId, int userId);
        public Task<GetEmployemntDetailsResponse> GetEmploymentHistoryByUan(string Uan, int userId);
        public Task<List<EpsContributionCheckResult>> CheckEpsContributionConsistency(SignzyUanPassbookDetails uanPassbookDetails);
    }
}
