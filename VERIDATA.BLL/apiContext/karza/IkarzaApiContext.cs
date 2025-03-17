using VERIDATA.Model.DataAccess;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Response.api.Karza;

namespace VERIDATA.BLL.apiContext.karza
{
    public interface IkarzaApiContext
    {
        public Task<PanDetails> GetPanDetails(string panNo, int userId);

        public Task<GetCandidateUanDetails> GetUanFromPan(string panNo, string MobileNo, int userId);

        public Task<GetCandidateUanDetails> GetUanFromMobile(string mobileNo, int userId);

        public Task<PassportDetails> GetPassportDetails(AppointeePassportValidateRequest reqObj);

        public Task<UanGenerateOtpDetails> GenerateUANOTP(string UanNumber, int userId);

        public Task<PfPassbookDetails> GetPassbookBySubmitUanOTP(string clientId, string otp, int userId);

        public Task<GetEmployemntDetailsResponse> GetEmployementDetais(string uan, int userId);

        public Task<EpsContributionCheckResult> CheckEpsContributionConsistencyForkarza(UanPassbookDetails uanPassbookDetails);

        public Task<GetAadharMobileLinkDetails> GetMobileAadharLinkStatus(string aadharNo, string mobileNo, int userId);

        public Task<AadharGenerateOTPDetails> GenerateAadharOTP(string aadharNumber, int userId);

        public Task<AadharSubmitOtpDetails> SubmitAadharOTP(string clientId, string aadharNumber, string otp, string shareCode, int userId);

        public Task<BankDetails> GetBackAccountDetails(string accountNumber, string ifsc, int userId);

        public Task<FirDetails> GetFirDetails(string name, DateTime? dob, string contactNo, int userId);

        public Task<DrivingLicenseDetails> GetDrivingLicenseDetails(string? number, DateTime dob, int userId);
    }
}