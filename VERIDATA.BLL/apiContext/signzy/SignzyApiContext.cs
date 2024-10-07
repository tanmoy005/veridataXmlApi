using Newtonsoft.Json;
using System.Net;
using System.Text;
using VERIDATA.BLL.apiContext.Common;
using VERIDATA.BLL.Services;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.Request;
using VERIDATA.Model.Request.api.Signzy;
using VERIDATA.Model.Response;
using VERIDATA.Model.Response.api.Karza;
using VERIDATA.Model.Response.api.Signzy;
using static VERIDATA.BLL.utility.CommonEnum;

namespace VERIDATA.BLL.apiContext.signzy
{
    public class SignzyApiContext : IsignzyApiContext
    {
        private readonly IUitityContext _apicontext;
        private readonly IApiConfigService _apiConfigContext;

        public SignzyApiContext(IUitityContext context, IApiConfigService apiConfigContext)
        {
            _apicontext = context;
            _apiConfigContext = apiConfigContext;
        }
        public async Task<PanDetails> GetPanDetails(string panNo, int userId)
        {

            PanDetails res = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.Pan, ApiSubTYpeName.Pan, ApiProviderType.Signzy);
            Signzy_GetPanDetailsRequest request = new()
            {
                panNumber = panNo,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);
            string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
            Signzy_PanDetailsResponse PanResponse = JsonConvert.DeserializeObject<Signzy_PanDetailsResponse>(apiResponse);

            if (_apiResponse.IsSuccessStatusCode)
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.PanNumber = PanResponse?.Number?.Trim();
                res.Name = PanResponse?.Name?.Trim();
                res.MobileNumber = PanResponse?.MobileNumber?.Trim();
                res.DateOfBirth = PanResponse?.DateOfBirth?.Trim();
            }
            else
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.ReasonPhrase = PanResponse?.Error?.Message?.ToString();
            }

            return res;
        }
        public async Task<GetCandidateUanDetails> GetUanFromMobilenPan(string panNo, string mobileNo, int userId)
        {
            GetCandidateUanDetails res = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.UAN, ApiSubTYpeName.FindUan, ApiProviderType.Signzy);
            Signzy_GetUanDetailsByPanRequest request = new()
            {
                panNumber = panNo,
                mobileNumber = mobileNo,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);
            string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
            Signzy_GetUanDetailsByPanResponse PanToUanResponse = JsonConvert.DeserializeObject<Signzy_GetUanDetailsByPanResponse>(apiResponse);

            bool isIanactiveUan = false;
            if (_apiResponse.IsSuccessStatusCode)
            {
                List<Employer> activeUanList = new();
                res.StatusCode = _apiResponse.StatusCode;
                string? uan = string.Empty;
                bool multiActiveUanData = false;
                List<string>? uanList = PanToUanResponse?.Result?.Result?.Uan?.ToList();
                if (uanList != null && uanList.Count > 0)
                {
                    uan = PanToUanResponse?.Result?.Result?.Summary?.MatchingUan;

                }
                res.StatusCode = _apiResponse.StatusCode;
                res.IsUanAvailable = !string.IsNullOrEmpty(uan);
                res.IsInactiveUan = isIanactiveUan;
                res.UanNumber = uan;
            }
            else
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.ReasonPhrase = PanToUanResponse?.Error?.Message?.ToString() ?? PanToUanResponse?.Message;
            }

            return res;
        }
        public async Task<PassportDetails> GetPassportDetails(AppointeePassportValidateRequest reqObj)
        {
            PassportDetails res = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.Passport, ApiSubTYpeName.Passport, ApiProviderType.Signzy);
            Signzy_GetPassportRequest request = new()
            {
                fileNumber = reqObj.passportFileNo,
                dob = reqObj.dateOfBirth.ToString("dd/MM/yyyy")
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, reqObj.userId);

            string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
            Signzy_GetPassportResponse? PassportResponse = JsonConvert.DeserializeObject<Signzy_GetPassportResponse>(apiResponse);
            if (_apiResponse.IsSuccessStatusCode)
            {
                PassportResult? passportData = PassportResponse?.Result;
                res.StatusCode = _apiResponse.StatusCode;
                res.Name = passportData?.Name?.Trim();
                res.PassportNumber = string.Empty;
                res.DateOfBirth = (Convert.ToDateTime(passportData.Dob)).ToString("yyy-MM-dd");
                res.FileNumber = passportData.FileNumber;

            }
            else
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.ReasonPhrase = PassportResponse?.Error?.Message?.ToString();
            }

            return res;
        }
        public async Task<UanGenerateOtpDetails> GenerateUANOTP(string UanNumber, string PhoneNumber, int userId)
        {
            UanGenerateOtpDetails Response = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.EPFO, ApiSubTYpeName.UANGenerateOTP, ApiProviderType.Signzy);
            Signzy_UanGenerateOtpRequest request = new()
            {
                phoneNumber = PhoneNumber,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);
            string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
            Signzy_UanGenerateOtpResponse OTPResponse = JsonConvert.DeserializeObject<Signzy_UanGenerateOtpResponse>(apiResponse);

            if (_apiResponse.IsSuccessStatusCode)
            {
                var data = OTPResponse;
                if (data != null)
                {
                    if ((string.IsNullOrEmpty(data?.Message) && OTPResponse?.ResultCode == (int)SignzyStatusCode.Succed))
                    {
                        string msg = "Invalid Mobile Number or Combination of Inputs";
                        Response.StatusCode = HttpStatusCode.BadRequest;
                        Response.ReasonPhrase = msg;

                    }
                    //else if (OTPResponse.statusCode == (int)KarzaStatusCode.NotFound)
                    //{
                    //    Response.StatusCode = HttpStatusCode.NotFound;
                    //    Response.ReasonPhrase = "Uan is inactive / Mobile Number not linked with  Uan Number  ";
                    //}
                    else
                    {
                        Response.StatusCode = _apiResponse.StatusCode;
                        Response.IsAsync = true;//data?.is_async ?? false;
                        Response.OtpSent = OTPResponse.ResultCode == (int)SignzyStatusCode.Succed;// OTPResponse?.data?.otp_sent ?? false;
                        Response.ClientId = OTPResponse?.TxnId ?? string.Empty;
                        Response.MaskedMobileNumber = string.Empty;// OTPResponse?.data?.masked_mobile_number;
                    }
                }
                else
                {
                    Response.StatusCode = HttpStatusCode.BadRequest;
                    Response.ReasonPhrase = "Invalid  Mobile Number ";
                }
            }
            else
            {

                Response.StatusCode = _apiResponse.StatusCode;
                Response.ReasonPhrase = OTPResponse?.Error?.Message?.ToString() ?? OTPResponse?.Message;
            }
            return Response;
        }
        public async Task<UanSubmitOtpDetails> SubmitUanOTP(string clientId, string otp, int userId)
        {
            UanSubmitOtpDetails Response = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.EPFO, ApiSubTYpeName.UANSubmitOTP, ApiProviderType.Signzy);
            Signzy_UanSubmitOtpRequest request = new()
            {
                txnId = clientId,
                otp = otp
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);
            string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
            Signzy_UanSubmitOtpResponse OTPResponse = JsonConvert.DeserializeObject<Signzy_UanSubmitOtpResponse>(apiResponse);
            if (_apiResponse.IsSuccessStatusCode)
            {

                Response.StatusCode = _apiResponse.StatusCode;
                Response.ClientId = OTPResponse?.TxnId;
                Response.OtpValidated = true;
            }
            else
            {
                Response.StatusCode = _apiResponse.StatusCode;
                Response.ReasonPhrase = OTPResponse?.Error?.Message?.ToString();
                Response.OtpValidated = false;
            }

            return Response;

        }
        public async Task<PfPassbookDetails> GetPassbook(string clientId, int userId)
        {
            PfPassbookDetails Response = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.EPFO, ApiSubTYpeName.UanPassbook, ApiProviderType.Signzy);
            Signzy_UanPassbookFetchRequest request = new()
            {
                TxnId = clientId,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);
            string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
            Signzy_UanPassbookFetchResponse OTPResponse = JsonConvert.DeserializeObject<Signzy_UanPassbookFetchResponse>(apiResponse);

            if (_apiResponse.IsSuccessStatusCode)
            {
                Response.StatusCode = _apiResponse.StatusCode;
                Response.SignzyPassbkdata = OTPResponse.Data;
            }
            else
            {
                Response.StatusCode = _apiResponse.StatusCode;
                Response.ReasonPhrase = OTPResponse?.Error?.Message?.ToString();
            }

            return Response;

        }
        public async Task<GetEmployemntDetailsResponse> GetEmploymentHistoryByUan(string Uan, int userId)
        {
            GetEmployemntDetailsResponse res = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.EPFOUAN, ApiSubTYpeName.UanValidation, ApiProviderType.Signzy);
            Signzy_GetEmployementDetailsByUanRequest request = new()
            {
                uan = Uan,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);
            string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
            Signzy_GetEmployementDetailsByUanResponse employementUanResponse = JsonConvert.DeserializeObject<Signzy_GetEmployementDetailsByUanResponse>(apiResponse);

            res.StatusCode = _apiResponse.StatusCode;
            if (!_apiResponse.IsSuccessStatusCode)
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.EmployementData = apiResponse;
            }
            else
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.ReasonPhrase = employementUanResponse?.Error?.Message?.ToString() ?? employementUanResponse?.Message;
            }

            return res;
        }
    }
}
