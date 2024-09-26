using VERIDATA.Model.DataAccess;
using VERIDATA.Model.Request;

namespace VERIDATA.BLL.apiContext.karza
{
    public interface IkarzaApiContext
    {
        public Task<PanDetails> GetPanDetails(string panNo, int userId);
        public Task<GetCandidateUanDetails> GetUanFromPan(string panNo, string MobileNo, int userId);
        public Task<GetCandidateUanDetails> GetUanFromMobile(string mobileNo, int userId);
        public Task<PassportDetails> GetPassportDetails(AppointeePassportValidateRequest reqObj);
        public Task<AadharGenerateOTPDetails> GenerateAadharOTP(string aadharNumber, int userId);
        public Task<AadharSubmitOtpDetails> SubmitAadharOTP(string clientId, string aadharNumber, string otp, int userId);
        public Task<UanGenerateOtpDetails> GenerateUANOTP(string UanNumber, int userId);
        public Task<PfPassbookDetails> GetPassbookBySubmitUanOTP(string clientId, string otp, int userId);
    }
}
