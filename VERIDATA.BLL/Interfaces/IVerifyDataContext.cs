using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;

namespace VERIDATA.BLL.Interfaces
{
    public interface IVerifyDataContext
    {
        public string GenarateErrorMsg(int statusCode, string reasonCode, string type);
        public Task<AppointeePanValidateResponse> PanDetailsValidation(AppointeePanValidateRequest reqObj);
        public Task<AppointeePassportValidateResponse> PassportDetailsValidation(AppointeePassportValidateRequest reqObj);
        public Task<GetUanResponse> GetUanNumber(GetUanNumberDetailsRequest reqObj);
        public Task<AadharGenerateOTPDetails> GeneratetAadharOTP(AppointeeAadhaarValidateRequest reqObj);
        public Task<AadharSubmitOtpDetails> SubmitAadharOTP(AppointeeAadhaarSubmitOtpRequest reqObj);
        public Task<AadharSubmitOtpDetails> GetAadharDetailsFromXml(string? xmlData);
        public Task<CandidateValidateResponse> VerifyAadharData(AadharValidationRequest reqObj);
        public Task<UanGenerateOtpDetails> GeneratetUANOTP(UanGenerateOtpRequest reqObj);
        //public Task<UanSubmitOtpDetails> SubmitUanOTP(AppointeeUANSubmitOtpRequest reqObj);
        public Task<GetPassbookDetailsResponse> GetPassbookBySubmitOTP(AppointeeUANSubmitOtpRequest reqObj);
        public Task<GetPassbookDetailsResponse> GetPfPassbookData(GetPassbookDetailsRequest reqObj);
        public Task<CandidateValidateResponse> VerifyUanData(UanValidationRequest reqObj);
        public Task PostActivity(int appointeeId, int userId, string activityCode);
        //public Task<EmployementHistoryDetailsRespons> GetEmployementHistoryDetails(GetEmployemntDetailsRequest reqObj);
        public Task<GetUanResponse> GetUanNumberPriorityBase(GetUanNumberDetailsRequest reqObj);   // mGhosh
        //public Task<CandidateValidateResponse> VerifyUanDataPriorityBase(UanValidationRequest reqObj);  // mGhosh
    }
}
