using System.Net;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using VERIDATA.BLL.apiContext.Common;
using VERIDATA.BLL.Services;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.Request;
using VERIDATA.Model.Request.api.Karza;
using VERIDATA.Model.Request.api.Signzy;
using VERIDATA.Model.Response;
using VERIDATA.Model.Response.api.Karza;
using VERIDATA.Model.Response.api.Signzy;
using static VERIDATA.DAL.utility.CommonEnum;

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
                res.PanNumber = panNo?.Trim();
                res.Name = PanResponse?.Result?.Name?.Trim();
                res.MobileNumber = string.Empty;
                res.DateOfBirth = PanResponse?.Result?.DateOfBirth?.Trim();
            }
            else
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.ReasonPhrase = PanResponse?.Error?.Message?.ToString();
            }

            return res;
        }

        public async Task<GetCandidateUanDetails> GetUanFromMobilenPan(string? panNo, string mobileNo, int userId)
        {
            GetCandidateUanDetails res = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.UAN, ApiSubTYpeName.FindUan, ApiProviderType.Signzy);
            Signzy_GetUanDetailsByPanRequest request = new()
            {
                panNumber = panNo ?? "",
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
            //var callBackBaseUrl = "https://fd20-136-232-69-90.ngrok-free.app";
            //string callBackActionUrl = "/api/AadhaarValidate/callback";
            //var _callBackUrl = $"{callBackBaseUrl}{callBackActionUrl}";
            Signzy_UanGenerateOtpRequest request = new()
            {
                phoneNumber = PhoneNumber,
                //callbackUrl = _callBackUrl,
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
                    Response.ReasonPhrase = "Invalid Mobile Number ";
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
                requestId = clientId,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);
            string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
            //Signzy_UanPassbookFetchResponse OTPResponse = JsonConvert.DeserializeObject<Signzy_UanPassbookFetchResponse>(apiResponse);

            if (_apiResponse.IsSuccessStatusCode)
            {
                SignzyUanPassbookDetails OTPResponse = JsonConvert.DeserializeObject<SignzyUanPassbookDetails>(apiResponse);

                Response.StatusCode = _apiResponse.StatusCode;
                Response.SignzyPassbkdata = OTPResponse;
            }
            else
            {
                Signzy_UanPassbookFetchResponse OTPResponse = JsonConvert.DeserializeObject<Signzy_UanPassbookFetchResponse>(apiResponse);

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
            if (_apiResponse.IsSuccessStatusCode)
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.EmployementData = apiResponse;
            }
            else
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.ReasonPhrase = "The site is currently unreachable. Please try again after some time or opt for manual passbook upload.";
                // res.ReasonPhrase = employementUanResponse?.Error?.Message?.ToString() ?? employementUanResponse?.Message;
            }

            return res;
        }

        public async Task<EpsContributionCheckResult> CheckEpsContributionConsistency(SignzyUanPassbookDetails uanPassbookDetails)
        {
            var result = new EpsContributionCheckResult();
            var epsContributionSummary = new List<EpsContributionSummary>();
            DateTime? lastCompanyEndDate = null;
            bool isDualEmployement = false;

            if (uanPassbookDetails?.EstDetails == null)
            {
                return result; // Return an empty list if there are no establishment details
            }
            var sortedEstDetails = uanPassbookDetails.EstDetails.OrderBy(estDetail => DateTime.TryParse(estDetail.DocEpf, out var dojDate) ? dojDate : DateTime.MinValue).ToList();
            foreach (var estDetail in sortedEstDetails)
            {
                string companyName = estDetail?.EstName;
                var passbookEntries = estDetail?.Passbook;
                string? _epsStartData = estDetail?.DojEpf;
                string? _epsEndData = null;

                if (passbookEntries != null && passbookEntries.Count > 0)
                {
                    // Sort passbook entries by "approved_on" date in ascending order with null check
                    var sortedEntries = passbookEntries
                        .OrderBy(entry => DateTime.TryParse(entry.TrDateMy, out var date) ? date : DateTime.MinValue)
                        .ToList();

                    bool hasEpsContribution = false;
                    bool gapDetected = false;
                    DateTime? startDate = null;

                    foreach (var entry in sortedEntries)
                    {
                        if (entry?.DbCrFlag == "C" && (entry?.Particular.ToLower().Contains("cont.") ?? false))
                        {
                            int crPenBal = int.TryParse(entry.CrPenBal, out var balance) ? balance : 0;
                            _epsEndData = entry?.TrDateMy ?? string.Empty;
                            if (crPenBal >= 1)
                            {
                                if (startDate == null && DateTime.TryParse(entry.TrDateMy, out var approvedDate))
                                {
                                    startDate = approvedDate;
                                }
                                hasEpsContribution = true;
                            }
                            else if (hasEpsContribution)
                            {
                                // If we encounter a 0 after starting contributions, mark as a gap
                                gapDetected = true;
                                break;
                            }
                        }
                    }
                    //    DateTime.TryParse(_epsStartData, out var FirstApprovedDate);
                    // Check if this company has consistent EPS contribution from the start date
                    if (hasEpsContribution && !isDualEmployement && lastCompanyEndDate.HasValue && startDate < lastCompanyEndDate)
                    {
                        isDualEmployement = true;
                    }

                    // Record the response for this company
                    epsContributionSummary.Add(new EpsContributionSummary
                    {
                        Company = companyName,
                        StartDate = startDate?.ToString("dd/MM/yyyy"),
                        EpsGapfind = gapDetected,
                        HasEpsContribution = hasEpsContribution
                    });

                    // Update last company's end date for comparison with the next company, with null check
                    if (_epsEndData != null &&
                        DateTime.TryParse(_epsEndData, out var lastApprovedDate))
                    {
                        lastCompanyEndDate = lastApprovedDate;
                    }
                }
            }
            result.EpsContributionSummary = epsContributionSummary;
            result.HasDualEmplyement = isDualEmployement;
            return result;
        }

        public async Task<BankDetails> GetBankDetails(string? accountNo, string ifscCode, string name, string mobile, int userId)
        {
            BankDetails res = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.Bank, ApiSubTYpeName.AccDetails, ApiProviderType.Signzy);
            Signzy_GetCandidateBankDetailsRequest request = new()
            {
                beneficiaryAccount = accountNo,
                beneficiaryIFSC = ifscCode,
                beneficiaryMobile = mobile,
                beneficiaryName = name,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);
            string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
            Signzy_GetCandidateBankDetailsResponse response = JsonConvert.DeserializeObject<Signzy_GetCandidateBankDetailsResponse>(apiResponse);

            res.StatusCode = _apiResponse.StatusCode;
            if ((int)res.StatusCode == (int)SignzyStatusCode.Invalid || (int)res.StatusCode == (int)SignzyStatusCode.NotFound)
            {
                res.StatusCode = HttpStatusCode.BadRequest;
                res.ReasonPhrase = "No details found";
            }
            else
            {
                var accountResult = response?.Result;
                res.StatusCode = _apiResponse.StatusCode;
                var status = accountResult?.Active?.ToUpper()?.Trim();
                if (status != null && status != "YES")
                {
                    res.StatusCode = HttpStatusCode.BadRequest;
                    res.ReasonPhrase = string.IsNullOrEmpty(accountResult?.Reason) ? "No details found" : accountResult?.Reason;
                }
                res.AccountNo = accountNo;
                res.IFSCCode = ifscCode;
                res.AccountHolderName = name;
            }

            return res;
        }

        public async Task<GetFirStatusDetails> GetFirStatusDetails(string name, string fatherName, int userId)
        {
            GetFirStatusDetails res = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.Police, ApiSubTYpeName.FirSearchId, ApiProviderType.Signzy);
            Signzy_SearchCandidateFirDataRequest request = new()
            {
                name = name,
                fatherName = fatherName,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);
            string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
            Signzy_SearchCandidateFirDataResponse response = JsonConvert.DeserializeObject<Signzy_SearchCandidateFirDataResponse>(apiResponse);

            res.StatusCode = _apiResponse.StatusCode;
            if (_apiResponse.IsSuccessStatusCode || (int)res.StatusCode == (int)SignzyStatusCode.NotFound)
            {
                res.StatusCode = HttpStatusCode.OK;
                res.SearchId = response?.searchId;
            }
            else
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString();
            }
            return res;
        }

        public async Task<FirDetails> GetFirDetails(string? searchId, int userId)
        {
            FirDetails res = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.Police, ApiSubTYpeName.FirDetails, ApiProviderType.Signzy);
            Signzy_GetCandidateFirDetailsRequest request = new()
            {
                searchId = searchId,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);
            string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
            Signzy_CandidateFirDetailsResponse response = JsonConvert.DeserializeObject<Signzy_CandidateFirDetailsResponse>(apiResponse);

            res.StatusCode = _apiResponse.StatusCode;
            if ((int)res.StatusCode == (int)SignzyStatusCode.Invalid || (int)res.StatusCode == (int)SignzyStatusCode.NotFound)
            {
                res.StatusCode = HttpStatusCode.BadRequest;
                res.ReasonPhrase = "No details found";
            }
            else
            {
                var records = response?.cases;
                res.StatusCode = _apiResponse.StatusCode;
                foreach (var record in records)
                {
                    var policeFirDetails = new PoliceFirDetails
                    {
                        FirNumber = record.FIRNumber.ToString(),
                        PoliceStation = record.PoliceStation,
                        FirDate = record.Date,
                    };

                    policeFirDetails.FirActDetails.Add(new FirActAndSection
                    {
                        Acts = record.UnderActs,
                        Sections = record.UnderSections,
                    });

                    res.PoliceFirDetails.Add(policeFirDetails);
                }
            }
            return res;
        }

        public async Task<DrivingLicenseDetails> GetDrivingLicenseDetails(string? number, DateTime dob, int userId)
        {
            DrivingLicenseDetails res = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.Driving, ApiSubTYpeName.DrvLicns, ApiProviderType.Signzy);
            Signzy_GetCandidateDrivingLicenseDetailsRequest request = new()
            {
                number = number,
                dob = dob.ToString("dd/MM/yyyy"),
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);
            string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
            Signzy_GetCandidateDrivingLicenseDetailsResponse response = JsonConvert.DeserializeObject<Signzy_GetCandidateDrivingLicenseDetailsResponse>(apiResponse);

            res.StatusCode = _apiResponse.StatusCode;
            if ((int)res.StatusCode == (int)SignzyStatusCode.Invalid || (int)res.StatusCode == (int)SignzyStatusCode.NotFound)
            {
                res.StatusCode = HttpStatusCode.BadRequest;
                res.ReasonPhrase = "No details found";
            }
            else
            {
                var DLResult = response?.Result;
                res.StatusCode = _apiResponse.StatusCode;
                res.LicenseStatus = DLResult.DlNumber;
                res.Name = DLResult.DetailsOfDrivingLicence?.Name;
                res.FatherOrHusbandName = DLResult.DetailsOfDrivingLicence?.FatherOrHusbandName;
                res.Dob = DLResult.Dob;
            }

            return res;
        }
    }
}