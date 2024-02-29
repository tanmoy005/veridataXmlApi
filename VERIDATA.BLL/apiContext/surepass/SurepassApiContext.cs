using Newtonsoft.Json;
using System.Net;
using System.Text;
using VERIDATA.BLL.apiContext.Common;
using VERIDATA.BLL.Services;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.Request;
using VERIDATA.Model.Request.api.surepass;
using VERIDATA.Model.Response.api.surepass;
using static VERIDATA.BLL.apiContext.Common.ApiRoute;
using static VERIDATA.BLL.utility.CommonEnum;

namespace VERIDATA.BLL.apiContext.surepass
{
    public class SurepassApiContext : IsurepassApiContext
    {
        private readonly IUitityContext _apicontext;

        private readonly IApiConfigService _apiConfigContext;

        public SurepassApiContext(IUitityContext context, IApiConfigService apiConfigContext)
        {
            _apicontext = context;
            _apiConfigContext = apiConfigContext;
        }

        public async Task<AadharGenerateOTPDetails> GenerateAadharOTP(string aadharNumber, int userId)
        {
            AadharGenerateOTPDetails Response = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.Adhaar, ApiSubTYpeName.AadharGenerateOTP, ApiProviderType.SurePass);
            Surepass_AadhaarGenerateOTPRequest request = new()
            {
                id_number = aadharNumber,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);

            if (_apiResponse.IsSuccessStatusCode)
            {
                string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
                Surepass_AadhaarGenerateOtpResponse OTPResponse = JsonConvert.DeserializeObject<Surepass_AadhaarGenerateOtpResponse>(apiResponse);
                Response.StatusCode = _apiResponse.StatusCode;
                Response.if_number = OTPResponse?.data?.if_number ?? false;
                Response.otp_sent = OTPResponse?.data?.otp_sent ?? false;
                Response.client_id = OTPResponse?.data?.client_id ?? string.Empty;
                Response.valid_aadhaar = OTPResponse?.data?.valid_aadhaar ?? false;
            }
            else
            {
                Response.StatusCode = _apiResponse.StatusCode;
                Response.ReasonPhrase = _apiResponse.StatusCode == HttpStatusCode.UnprocessableEntity ? "Invalid  Aadhaar Number or Combination of Inputs" : _apiResponse?.ReasonPhrase?.ToString();
            }
            return Response;
        }

        public async Task<AadharSubmitOtpDetails> SubmitAadharOTP(string clientId, string otp, int userId)
        {
            AadharSubmitOtpDetails Response = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.Adhaar, ApiSubTYpeName.AadharVerifyOTP, ApiProviderType.SurePass);
            Surepass_AadhaarSubmitOtpRequest request = new()
            {
                client_id = clientId,
                otp = otp
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);

            if (_apiResponse.IsSuccessStatusCode)
            {
                string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
                Surepass_AadhaarSubmitOtpResponse OTPResponse = JsonConvert.DeserializeObject<Surepass_AadhaarSubmitOtpResponse>(apiResponse);

                string[]? appointeeCareOfDetails = OTPResponse?.data?.care_of?.Split(":");
                string? appointeeCareOf = appointeeCareOfDetails?.Count() > 1 ? appointeeCareOfDetails?.LastOrDefault()?.ToUpper()?.Trim() : OTPResponse?.data?.care_of?.ToUpper();

                Response.StatusCode = _apiResponse.StatusCode;
                Response.Name = OTPResponse?.data?.full_name?.Trim();
                Response.Gender = OTPResponse?.data?.gender?.Trim();
                Response.Dob = OTPResponse?.data?.dob?.Trim();
                Response.AadharNumber = OTPResponse?.data?.aadhaar_number?.Trim();
                Response.CareOf = appointeeCareOf;
            }
            else
            {
                Response.StatusCode = _apiResponse.StatusCode;
                Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString();
            }
            return Response;
        }

        public async Task<PanDetails> GetPanDetails(string panNo, int userId)
        {
            PanDetails res = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.Pan, ApiSubTYpeName.Pan, ApiProviderType.SurePass);
            Surepass_GetPanDetailsRequest request = new()
            {
                id_number = panNo,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);

            if (_apiResponse.IsSuccessStatusCode)
            {
                string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
                Surepass_PanDetailsResponse PanResponse = JsonConvert.DeserializeObject<Surepass_PanDetailsResponse>(apiResponse);
                res.StatusCode = _apiResponse.StatusCode;
                res.Name = PanResponse?.data?.full_name?.Trim();
                res.PanNumber = PanResponse?.data?.pan_number?.Trim();
                res.DateOfBirth = PanResponse?.data?.dob?.Trim();
                res.MobileNumber = PanResponse?.data?.phone_number?.Trim();
            }
            else
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString();
            }

            return res;
        }

        public async Task<PassportDetails> GetPassportDetails(AppointeePassportValidateRequest reqObj)
        {
            PassportDetails res = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.Passport, ApiSubTYpeName.Passport, ApiProviderType.SurePass);
            Surepass_GetPassportRequest request = new()
            {
                id_number = reqObj.passportFileNo,
                dob = reqObj.dateOfBirth.ToString("yyyy-MM-dd")
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, reqObj.userId);

            if (_apiResponse.IsSuccessStatusCode)
            {
                string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
                Surepass_GetPassportResponse? PassportResponse = JsonConvert.DeserializeObject<Surepass_GetPassportResponse>(apiResponse);
                res.StatusCode = _apiResponse.StatusCode;
                res.Name = PassportResponse?.data?.full_name?.Trim();
                res.PassportNumber = PassportResponse?.data?.passport_number?.Trim();
                res.DateOfBirth = PassportResponse?.data?.dob?.Trim();
                res.FileNumber = PassportResponse?.data?.file_number?.Trim();

            }
            else
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString();
            }

            return res;
        }

        public async Task<GetCandidateUanDetails> GetUanFromAadhar(string aadharNumber, int userId)
        {
            GetCandidateUanDetails res = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.UAN, ApiSubTYpeName.FindUan, ApiProviderType.SurePass);
            Surepass_GetUanNumberRequest request = new()
            {
                aadhaar_number = aadharNumber
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);
            string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
            Surepass_GetUanNumberResponse? UanResponse = JsonConvert.DeserializeObject<Surepass_GetUanNumberResponse>(apiResponse);

            if (_apiResponse.IsSuccessStatusCode)
            {
                _ = _apiResponse.EnsureSuccessStatusCode();
                res.StatusCode = _apiResponse.StatusCode;
                if (string.IsNullOrEmpty(UanResponse?.data?.pf_uan))
                {
                    res.IsUanAvailable = false;
                    res.ReasonPhrase = "Uan not available.";
                }
                else
                {
                    res.IsUanAvailable = true;
                    res.UanNumber = UanResponse?.data?.pf_uan;
                }

            }
            else
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.ReasonPhrase = _apiResponse.StatusCode == HttpStatusCode.UnprocessableEntity ? UanResponse?.message?.ToString() : _apiResponse?.ReasonPhrase?.ToString();
            }
            return res;
        }

        public async Task<UanGenerateOtpDetails> GenerateUANOTP(string UanNumber, int userId)
        {
            UanGenerateOtpDetails Response = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.EPFO, ApiSubTYpeName.UANGenerateOTP, ApiProviderType.SurePass);
            Surepass_UanValidateRequest request = new()
            {
                id_number = UanNumber,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);

            if (_apiResponse.IsSuccessStatusCode)
            {
                string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
                Surepass_UanGenerateOtpResponse OTPResponse = JsonConvert.DeserializeObject<Surepass_UanGenerateOtpResponse>(apiResponse);
                Response.StatusCode = _apiResponse.StatusCode;
                Response.IsAsync = OTPResponse?.data?.is_async ?? false;
                Response.OtpSent = OTPResponse?.data?.otp_sent ?? false;
                Response.ClientId = OTPResponse?.data?.client_id ?? string.Empty;
                Response.MaskedMobileNumber = OTPResponse?.data?.masked_mobile_number;
            }
            else
            {
                Response.StatusCode = _apiResponse.StatusCode;
                Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString();
            }
            return Response;
        }

        public async Task<UanSubmitOtpDetails> SubmitUanOTP(string clientId, string otp, int userId)
        {
            UanSubmitOtpDetails Response = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.EPFO, ApiSubTYpeName.UANSubmitOTP, ApiProviderType.SurePass);
            Surepass_UanSubmitOtpResponse OTPResponse = new();
            Surepass_UanSubmitOtpRequest request = new()
            {
                client_id = clientId,
                otp = otp
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);

            if (_apiResponse.IsSuccessStatusCode)
            {
                string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
                OTPResponse = JsonConvert.DeserializeObject<Surepass_UanSubmitOtpResponse>(apiResponse);

                Response.StatusCode = _apiResponse.StatusCode;
                Response.ClientId = OTPResponse?.data?.client_id?.Trim();

            }
            else
            {
                Response.StatusCode = _apiResponse.StatusCode;
                Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString();
            }
            Response.OtpValidated = OTPResponse?.data?.otp_validated;
            return Response;
        }

        public async Task<PfPassbookDetails> GetPassbookDetails(string clientId, int userId)
        {
            PfPassbookDetails res = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.EPFO, ApiSubTYpeName.UanPassbook, ApiProviderType.SurePass);
            Surepass_GetUanPassbookRequest request = new()
            {
                client_id = clientId,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);

            if (_apiResponse.IsSuccessStatusCode)
            {
                string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
                Surepass_GetUanPassbookResponse PassbookResponse = JsonConvert.DeserializeObject<Surepass_GetUanPassbookResponse>(apiResponse);
                res.StatusCode = _apiResponse.StatusCode;
                res.Passbkdata = PassbookResponse?.data;


            }
            else
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString();
            }

            return res;
        }
    }
}
