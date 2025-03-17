using VERIDATA.Model.DataAccess;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
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

        public Task<EpsContributionCheckResult> CheckEpsContributionConsistency(SignzyUanPassbookDetails uanPassbookDetails);

        public Task<BankDetails> GetBankDetails(string? accountNo, string ifscCode, string name, string mobile, int userId);

        public Task<GetFirStatusDetails> GetFirStatusDetails(string name, string fatherName, int userId);

        public Task<FirDetails> GetFirDetails(string? searchId, int userId);

        public Task<DrivingLicenseDetails> GetDrivingLicenseDetails(string? number, DateTime dob, int userId);
    }
}