﻿using Newtonsoft.Json;
using System.Net;
using System.Text;
using VERIDATA.BLL.apiContext.Common;
using VERIDATA.BLL.Services;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.Request;
using VERIDATA.Model.Request.api.Karza;
using VERIDATA.Model.Response;
using VERIDATA.Model.Response.api.Karza;
using static VERIDATA.DAL.utility.CommonEnum;

namespace VERIDATA.BLL.apiContext.karza
{
    public class KarzaApiContext : IkarzaApiContext
    {
        private readonly IUitityContext _apicontext;
        private readonly IApiConfigService _apiConfigContext;

        public KarzaApiContext(IUitityContext context, IApiConfigService apiConfigContext)
        {
            _apicontext = context;
            _apiConfigContext = apiConfigContext;
        }
        public async Task<PanDetails> GetPanDetails(string panNo, int userId)
        {

            PanDetails res = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.Pan, ApiSubTYpeName.Pan, ApiProviderType.Karza);
            Karza_GetPanDetailsRequest request = new()
            {
                pan = panNo,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);

            if (_apiResponse.IsSuccessStatusCode)
            {
                string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
                Karza_PanDetailsResponse PanResponse = JsonConvert.DeserializeObject<Karza_PanDetailsResponse>(apiResponse);
                if (PanResponse.statusCode == (int)KarzaStatusCode.Invalid || PanResponse.statusCode == (int)KarzaStatusCode.NotFound)
                {
                    res.StatusCode = HttpStatusCode.BadRequest;
                    res.ReasonPhrase = "Invalid PAN Number or PAN number not found";
                }
                else
                {
                    PanInfoResult? personalInfo = PanResponse?.result;
                    res.StatusCode = _apiResponse.StatusCode;
                    res.PanNumber = personalInfo?.pan?.Trim();
                    res.Name = personalInfo?.name?.Trim();
                    res.MobileNumber = personalInfo?.mobileNo?.Trim();
                    res.DateOfBirth = personalInfo?.dob?.Trim();
                }
            }
            else
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString();
            }

            return res;
        }

        public async Task<GetCandidateUanDetails> GetUanFromMobile(string mobileNo, int userId)
        {

            GetCandidateUanDetails res = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.UAN, ApiSubTYpeName.FindUan, ApiProviderType.Karza);
            Karza_GetUanDetailsByMobileRequest request = new()
            {
                mobile = mobileNo,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);

            bool isIanactiveUan = false;
            if (_apiResponse.IsSuccessStatusCode)
            {
                List<Employer> activeUanList = new();
                string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
                Karza_GetUanDetailsByMobileResponse MobileToUanResponse = JsonConvert.DeserializeObject<Karza_GetUanDetailsByMobileResponse>(apiResponse);
                res.StatusCode = _apiResponse.StatusCode;
                string? uan = string.Empty;
                bool multiActiveUanData = false;
                List<string>? uanList = MobileToUanResponse?.result?.Uan?.ToList();
                if (MobileToUanResponse?.result != null)
                {
                    if (uanList != null && uanList.Count > 0)
                    {
                        uan = uanList?.FirstOrDefault();
                        //foreach (Uan? obj in uanList)
                        //{
                        //    string? uanNo = obj?.uan?.Trim();
                        //    List<Employer>? activeUan = obj?.employer?.Where(x => x.isRecent == true)?.DistinctBy(x => x.memberId)?.ToList();
                        //    if (activeUan != null)
                        //    {
                        //        activeUanList?.AddRange(activeUan);
                        //    }
                        //    if (activeUanList?.Count == 1)
                        //    {
                        //        uan = uanNo;
                        //    }
                        //    else
                        //    {
                        //        multiActiveUanData = true;
                        //    }
                        //    if (multiActiveUanData)
                        //    {
                        //        res.StatusCode = _apiResponse.StatusCode;
                        //        res.ReasonPhrase = "Multi Active Uan";
                        //    }
                        //}
                    }


                    res.StatusCode = _apiResponse.StatusCode;
                    res.IsUanAvailable = !string.IsNullOrEmpty(uan);
                    res.IsInactiveUan = isIanactiveUan;
                    res.UanNumber = uan;
                }
                else
                {
                    res.StatusCode = HttpStatusCode.InternalServerError;
                    res.ReasonPhrase = "EPFO server is currently busy!Please try again later";
                }
            }
            else
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString();
            }

            return res;
        }
        public async Task<GetCandidateUanDetails> GetUanFromPan(string panNo, string MobileNo, int userId)
        {
            GetCandidateUanDetails res = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.UAN, ApiSubTYpeName.FindUan, ApiProviderType.Karza);
            Karza_GetUanDetailsByPanRequest request = new()
            {
                pan = panNo,
                mobile = MobileNo,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);
            bool isIanactiveUan = false;
            string? uanName = string.Empty;
            if (_apiResponse.IsSuccessStatusCode)
            {
                List<Employer> activeUanList = new();
                string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
                Karza_GetUanDetailsByPanResponse PanToUanResponse = JsonConvert.DeserializeObject<Karza_GetUanDetailsByPanResponse>(apiResponse);
                //if (PanToUanResponse.statusCode == ((int)KarzaStatusCode.Invalid).ToString() || PanToUanResponse.statusCode == (((int)KarzaStatusCode.NotFound).ToString()))
                //{
                //    res.StatusCode = HttpStatusCode.BadRequest;
                //    res.ReasonPhrase = "Invalid Mobile Number or Combination of Inputs";
                //}
                //else
                //{
                res.StatusCode = _apiResponse.StatusCode;
                string? uan = string.Empty;
                bool multiActiveUanData = false;
                List<Uan>? uanList = PanToUanResponse?.result?.uan?.ToList();

                if (PanToUanResponse?.result?.personalInfo != null && PanToUanResponse?.result?.summary != null)
                {
                    if (uanList != null && uanList.Count > 0)
                    {
                        foreach (Uan? obj in uanList)
                        {
                            string? uanNo = obj?.uan?.Trim();
                            List<Employer>? activeUan = obj?.employer?.Where(x => x.isRecent == true)?.DistinctBy(x => x.memberId)?.ToList();
                            if (activeUan != null)
                            {
                                activeUanList?.AddRange(activeUan);
                            }
                            if (activeUanList?.Count == 1)
                            {
                                uan = uanNo;
                            }
                            else
                            {
                                multiActiveUanData = true;
                            }
                            if (multiActiveUanData)
                            {
                                res.StatusCode = _apiResponse.StatusCode;
                                res.ReasonPhrase = "Multi Active Uan";
                            }
                        }
                    }
                    NameLookup? uanNameSummeryInfo = PanToUanResponse?.result?.summary?.nameLookup;
                    uanName = uanNameSummeryInfo?.matchName;

                    if (uanNameSummeryInfo != null)
                    {
                        isIanactiveUan = !(uanNameSummeryInfo?.isUnique ?? false) && string.IsNullOrEmpty(uanNameSummeryInfo?.matchName);
                    }
                    else if (PanToUanResponse?.result?.personalInfo == null)
                    {
                        isIanactiveUan = true;
                    }
                    res.StatusCode = _apiResponse.StatusCode;
                    res.UanName = uanName;
                    res.IsUanAvailable = !string.IsNullOrEmpty(uan);
                    res.IsInactiveUan = isIanactiveUan;
                    res.UanNumber = uan;
                }
                else
                {
                    if (Convert.ToInt32(PanToUanResponse?.statusCode) == (int)KarzaStatusCode.NotFound)
                    {
                        res.IsUanAvailable = false;
                        res.StatusCode = _apiResponse.StatusCode;
                        //res.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString();
                    }
                    else
                    {
                        res.StatusCode = HttpStatusCode.InternalServerError;
                        res.ReasonPhrase = "EPFO server is currently busy!Please try again later";
                    }
                }
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
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.Passport, ApiSubTYpeName.Passport, ApiProviderType.Karza);
            Karza_GetPassportRequest request = new()
            {
                fileNo = reqObj.passportFileNo,
                dob = reqObj.dateOfBirth.ToString("dd/MM/yyyy")
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, reqObj.userId);

            if (_apiResponse.IsSuccessStatusCode)
            {
                string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
                Karza_GetPassportResponse? PassportResponse = JsonConvert.DeserializeObject<Karza_GetPassportResponse>(apiResponse);
                if (PassportResponse.statusCode == (int)KarzaStatusCode.Invalid || PassportResponse.statusCode == (int)KarzaStatusCode.NotFound)
                {
                    res.StatusCode = HttpStatusCode.BadRequest;
                    res.ReasonPhrase = "Invalid Passport File Number or Combination of Inputs";
                }
                else
                {
                    PassportData? passportData = PassportResponse?.result;

                    Name? _name = passportData?.name;
                    res.StatusCode = _apiResponse.StatusCode;
                    res.Name = $"{_name?.nameFromPassport?.Trim()} {_name?.surnameFromPassport?.Trim()}";
                    res.PassportNumber = passportData?.passportNumber?.passportNumberFromSource?.Trim();
                    res.DateOfBirth = reqObj.dateOfBirth.ToString("yyyy-MM-dd");
                    res.FileNumber = reqObj.passportFileNo;
                }

            }
            else
            {
                res.StatusCode = _apiResponse.StatusCode;
                res.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString();
            }

            return res;
        }
        //public async Task<AadharGenerateOTPDetails> GenerateAadharOTP(string aadharNumber, int userId)
        //{
        //    AadharGenerateOTPDetails Response = new();
        //    var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.Adhaar, ApiSubTYpeName.AadharGenerateOTP, ApiProviderType.Karza);
        //    Karza_AadhaarGenerateOTPRequest request = new()
        //    {
        //        aadhaarNo = aadharNumber,
        //    };
        //    StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        //    HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);

        //    if (_apiResponse.IsSuccessStatusCode)
        //    {
        //        string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
        //        Karza_AadhaarGenerateOtpResponse OTPResponse = JsonConvert.DeserializeObject<Karza_AadhaarGenerateOtpResponse>(apiResponse);
        //        AadharGenerateOtp? data = OTPResponse?.result;
        //        if ((string.IsNullOrEmpty(data?.message) && OTPResponse?.statusCode == (int)KarzaStatusCode.Invalid) || (OTPResponse?.statusCode == (int)KarzaStatusCode.MaxTry))
        //        {
        //            string msg = (OTPResponse.statusCode == (int)KarzaStatusCode.MaxTry) ? "Max retries exceeded " : "Invalid  Aadhaar Number or Combination of Inputs";
        //            Response.StatusCode = HttpStatusCode.UnprocessableEntity;
        //            Response.ReasonPhrase = msg;

        //        }
        //        else
        //        {
        //            Response.StatusCode = _apiResponse.StatusCode;
        //            Response.if_number = OTPResponse.statusCode != (int)KarzaStatusCode.Invalid;// OTPResponse?.data?.if_number ?? false;
        //            Response.otp_sent = OTPResponse.statusCode == (int)KarzaStatusCode.Sent;//OTPResponse?.data?.otp_sent ?? false;
        //            Response.client_id = OTPResponse?.requestId ?? string.Empty;
        //            Response.valid_aadhaar = true;//OTPResponse?.data?.valid_aadhaar ?? false;
        //        }
        //    }
        //    else
        //    {
        //        Response.StatusCode = _apiResponse.StatusCode;
        //        Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString();
        //    }
        //    return Response;
        //}
        //public async Task<AadharSubmitOtpDetails> SubmitAadharOTP(string clientId, string aadharNumber, string otp, int userId)
        //{
        //    AadharSubmitOtpDetails Response = new();
        //    var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.Adhaar, ApiSubTYpeName.AadharVerifyOTP, ApiProviderType.Karza);
        //    Karza_AadhaarSubmitOtpRequest request = new()
        //    {
        //        accessKey = clientId,
        //        aadhaarNo = aadharNumber,
        //        otp = otp
        //    };
        //    StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        //    HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);

        //    if (_apiResponse.IsSuccessStatusCode)
        //    {
        //        string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
        //        Karza_AadhaarSubmitOtpResponse OTPResponse = JsonConvert.DeserializeObject<Karza_AadhaarSubmitOtpResponse>(apiResponse);

        //        //var appointeeCareOfDetails = OTPResponse?.data?.care_of?.Split(":");
        //        //var appointeeCareOf = appointeeCareOfDetails?.Count() > 1 ? appointeeCareOfDetails?.LastOrDefault()?.ToUpper()?.Trim() : OTPResponse?.data?.care_of?.ToUpper();
        //        AadharResult? data = OTPResponse?.result;

        //        if ((OTPResponse?.statusCode ?? 0) == 101)
        //        {
        //            if (data?.dataFromAadhaar != null)
        //            {
        //                DataFromAadhaar? resData = data?.dataFromAadhaar;
        //                Response.StatusCode = _apiResponse.StatusCode;
        //                Response.Name = resData?.name?.Trim();
        //                Response.Gender = resData?.gender?.Trim();
        //                Response.Dob = resData?.dob?.Trim();
        //                Response.AadharNumber = aadharNumber.Trim();
        //                Response.CareOf = resData?.fatherName;
        //            }
        //            else
        //            {
        //                Response.StatusCode = HttpStatusCode.UnprocessableEntity;
        //                Response.ReasonPhrase = "No records found for the given Aadhar";
        //            }
        //        }
        //        else
        //        {
        //            string msg = (OTPResponse.statusCode == (int)KarzaStatusCode.MaxTry) ? "Max retries exceeded " :
        //                (OTPResponse.statusCode == (int)KarzaStatusCode.NotFound) ? "No records found for the given Aadhar" : string.Empty;
        //            Response.StatusCode = HttpStatusCode.UnprocessableEntity;
        //            Response.ReasonPhrase = OTPResponse.statusCode == (int)KarzaStatusCode.Invalid ? "Invalid  OTP " : msg;
        //        }
        //    }
        //    else
        //    {
        //        Response.StatusCode = _apiResponse.StatusCode;
        //        Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString();
        //    }
        //    return Response;
        //}
        public async Task<UanGenerateOtpDetails> GenerateUANOTP(string UanNumber, int userId)
        {
            UanGenerateOtpDetails Response = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.EPFO, ApiSubTYpeName.UANGenerateOTP, ApiProviderType.Karza);
            Karza_UanGenerateOtpRequest request = new()
            {
                uan = UanNumber,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);

            if (_apiResponse.IsSuccessStatusCode)
            {
                string apiResponse = await _apiResponse.Content.ReadAsStringAsync();

                Karza_UanGenerateOtpResponse OTPResponse = JsonConvert.DeserializeObject<Karza_UanGenerateOtpResponse>(apiResponse);
                UanGenerateOtp? data = OTPResponse?.result;
                if (data != null)
                {
                    int _statusCode = Convert.ToInt32(OTPResponse?.statusCode);
                    if ((string.IsNullOrEmpty(data?.message) && _statusCode == (int)KarzaStatusCode.Invalid) || (_statusCode == (int)KarzaStatusCode.MaxTry))
                    {
                        string msg = (_statusCode == (int)KarzaStatusCode.MaxTry) ? "Max retries exceeded " : "Invalid UAN Number or Combination of Inputs";
                        Response.StatusCode = HttpStatusCode.BadRequest;
                        Response.ReasonPhrase = msg;

                    }
                    else if (_statusCode == (int)KarzaStatusCode.NotFound)
                    {
                        Response.StatusCode = HttpStatusCode.NotFound;
                        Response.ReasonPhrase = "Uan is inactive / Mobile Number not linked with  Uan Number  ";
                    }
                    else
                    {
                        Response.StatusCode = _apiResponse.StatusCode;
                        Response.IsAsync = true;//data?.is_async ?? false;
                        Response.OtpSent = _statusCode == (int)KarzaStatusCode.Sent;// OTPResponse?.data?.otp_sent ?? false;
                        Response.ClientId = OTPResponse?.request_id ?? string.Empty;
                        Response.MaskedMobileNumber = string.Empty;// OTPResponse?.data?.masked_mobile_number;
                    }
                }
                else
                {
                    Response.StatusCode = HttpStatusCode.BadRequest;
                    Response.ReasonPhrase = "Invalid  Uan Number ";
                }
            }
            else
            {
                Response.StatusCode = _apiResponse.StatusCode;
                Response.ReasonPhrase = "Site Not Reachable. Please try again after some time - OR - Opt for manual passbook upload.";
                //Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString();
            }
            return Response;
        }
        public async Task<PfPassbookDetails> GetPassbookBySubmitUanOTP(string clientId, string otp, int userId)
        {
            PfPassbookDetails Response = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.EPFO, ApiSubTYpeName.UANSubmitOTP, ApiProviderType.Karza);
            Karza_UanSubmitOtpRequest request = new()
            {
                request_id = clientId,
                otp = otp
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);

            if (_apiResponse.IsSuccessStatusCode)
            {
                string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
                Karza_UanSubmitOtpResponse OTPResponse = JsonConvert.DeserializeObject<Karza_UanSubmitOtpResponse>(apiResponse);
                if (OTPResponse.statusCode == (int)KarzaStatusCode.Invalid || OTPResponse.statusCode == (int)KarzaStatusCode.NotFound)
                {
                    Response.StatusCode = HttpStatusCode.BadRequest;
                    Response.ReasonPhrase = "Invalid OTP File Number or Combination of Inputs";
                    return Response;
                }

                Response.StatusCode = _apiResponse.StatusCode;
                Response.KarzaPassbkdata = UpdateEpsContribution(OTPResponse?.result ?? new UanPassbookDetails());
            }
            else
            {
                Response.StatusCode = _apiResponse.StatusCode;
                Response.ReasonPhrase = "Site Not Reachable. Please try again after some time - OR - Opt for manual passbook upload.";
                //Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString();
            }

            return Response;

        }

        public async Task<GetEmployemntDetailsResponse> GetEmployementDetais(string uan, int userId)
        {
            GetEmployemntDetailsResponse Response = new();
            var apiConfig = await _apiConfigContext.GetApiConfigData(ApiType.EPFOUAN, ApiSubTYpeName.UanValidation, ApiProviderType.Karza);
            Karza_GetEmployementDetailsByUanRequest request = new()
            {
                uan = uan,
            };
            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            HttpResponseMessage _apiResponse = await _apicontext.HttpPostApi(apiConfig, content, userId);

            if (_apiResponse.IsSuccessStatusCode)
            {
                string apiResponse = await _apiResponse.Content.ReadAsStringAsync();
                Response.StatusCode = _apiResponse.StatusCode;
                Response.EmployementData = apiResponse;
            }
            else
            {
                Response.StatusCode = _apiResponse.StatusCode;
                Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString();
            }

            return Response;
        }

        public UanPassbookDetails UpdateEpsContribution(UanPassbookDetails uanPassbookDetails)
        {
            if (uanPassbookDetails?.est_details == null)
            {
                return uanPassbookDetails; // Return the object as-is if there are no establishment details
            }

            foreach (var estDetail in uanPassbookDetails.est_details)
            {
                if (estDetail?.passbook == null)
                {
                    continue; // Skip if passbook entries are null
                }

                foreach (var entry in estDetail.passbook)
                {
                    if (entry?.db_cr_flag == "C")
                    {
                        // Parse "cr_pen_bal" as an integer
                        int crPenBal = int.TryParse(entry.cr_pen_bal, out int result) ? result : 0;

                        // Update "cr_pen_bal" to "1" if it is greater than zero
                        if (crPenBal > 0)
                        {
                            entry.cr_pen_bal = "1";
                        }
                    }
                }
            }

            return uanPassbookDetails; // Return the modified object
        }

        public async Task<List<EpsContributionCheckResult>> CheckEpsContributionConsistencyForkarza(UanPassbookDetails uanPassbookDetails)
        {
            var result = new List<EpsContributionCheckResult>();
            DateTime? lastCompanyEndDate = null;

            if (uanPassbookDetails?.est_details == null)
            {
                return result; // Return an empty list if there are no establishment details
            }
            var sortedEstDetails = uanPassbookDetails.est_details.OrderBy(estDetail => DateTime.TryParse(estDetail.doj_epf, out var dojDate) ? dojDate : DateTime.MinValue).ToList();
            foreach (var estDetail in sortedEstDetails)
            {
                string companyName = estDetail?.est_name;
                var passbookEntries = estDetail?.passbook;

                if (passbookEntries != null && passbookEntries.Count > 0)
                {
                    // Sort passbook entries by "approved_on" date in ascending order with null check
                    var sortedEntries = passbookEntries
                        .OrderBy(entry => DateTime.TryParse(entry.approved_on, out var date) ? date : DateTime.MinValue)
                        .ToList();

                    bool hasEpsContribution = false;
                    bool gapDetected = false;
                    DateTime? startDate = null;

                    foreach (var entry in sortedEntries)
                    {
                        if (entry?.db_cr_flag == "C" && (entry?.particular.ToLower().Contains("cont.") ?? false))
                        {
                            int crPenBal = int.TryParse(entry.cr_pen_bal, out var balance) ? balance : 0;

                            if (crPenBal >= 1)
                            {
                                if (startDate == null && DateTime.TryParse(entry.approved_on, out var approvedDate))
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

                    // Check if this company has consistent EPS contribution from the start date
                    if (hasEpsContribution && lastCompanyEndDate.HasValue && startDate < lastCompanyEndDate)
                    {
                        gapDetected = true;
                    }

                    // Record the response for this company
                    result.Add(new EpsContributionCheckResult
                    {
                        Company = companyName,
                        StartDate = startDate?.ToString("dd/MM/yyyy"),
                        EpsGapfind = gapDetected,
                        HasEpsContribution= hasEpsContribution
                    });

                    // Update last company's end date for comparison with the next company, with null check
                    if (sortedEntries.LastOrDefault()?.approved_on != null &&
                        DateTime.TryParse(sortedEntries.Last().approved_on, out var lastApprovedDate))
                    {
                        lastCompanyEndDate = lastApprovedDate;
                    }
                }
            }

            return result;
        }

    }
}
