
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.Request;

namespace VERIDATA.BLL.apiContext.surepass
{
    public interface IsurepassApiContext
    {
        public Task<AadharGenerateOTPDetails> GenerateAadharOTP(string aadharNumber, int userId);
        public Task<AadharSubmitOtpDetails> SubmitAadharOTP(string clientId, string otp, int userId);
        public Task<PanDetails> GetPanDetails(string panNo, int userId);
        public Task<PassportDetails> GetPassportDetails(AppointeePassportValidateRequest reqObj);
        public Task<GetCandidateUanDetails> GetUanFromAadhar(string aadharNumber, int userId);
        public Task<UanGenerateOtpDetails> GenerateUANOTP(string UanNumber, int userId);
        public Task<UanSubmitOtpDetails> SubmitUanOTP(string clientId, string otp, int userId);
        public Task<PfPassbookDetails> GetPassbookDetails(string clientId, int userId);

    }
}
