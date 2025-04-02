using System.Net;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using VERIDATA.BLL.apiContext.karza;
using VERIDATA.BLL.apiContext.signzy;
using VERIDATA.BLL.apiContext.surepass;
using VERIDATA.BLL.Interfaces;
using VERIDATA.BLL.utility;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.Model.Configuration;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Response.api.Karza;
using VERIDATA.Model.Response.api.Signzy;
using VERIDATA.Model.Response.api.surepass;
using VERIDATA.Model.Table.Public;
using static VERIDATA.DAL.utility.CommonEnum;

namespace VERIDATA.BLL.Context
{
    public class VerifyDataContext : IVerifyDataContext
    {
        private readonly ApiConfiguration _apiConfigContext;
        private readonly IMasterDalContext _masterContext;
        private readonly IActivityDalContext _activityContext;
        private readonly IFileContext _fileService;
        private readonly IkarzaApiContext _karzaApiContext;
        private readonly IsurepassApiContext _surepassApiContext;
        private readonly IsignzyApiContext _signzyApiContext;
        private readonly ICandidateContext _candidateContext;

        public VerifyDataContext(ApiConfiguration apiConfigContext, IMasterDalContext masterContext, IActivityDalContext activityContext, IFileContext fileService,
            IsurepassApiContext surepassApiContext, IkarzaApiContext karzaApiContext, ICandidateContext candidateContext, IsignzyApiContext signzyApiContext)
        {
            _apiConfigContext = apiConfigContext;
            _masterContext = masterContext;
            _activityContext = activityContext;
            _karzaApiContext = karzaApiContext;
            _surepassApiContext = surepassApiContext;
            _signzyApiContext = signzyApiContext;
            _candidateContext = candidateContext;
            _fileService = fileService;
        }

        public string GenarateErrorMsg(int statusCode, string reasonCode, string type)
        {
            string msg = statusCode == (int)HttpStatusCode.InternalServerError
                ? $"{"The"} {type} {"server is currently busy. Please try again later."}"
                : $"{reasonCode}. {"Please try again later."}";
            return msg;
        }

        private async Task<CandidateValidateResponse> VarifyPanData(PanDetails request, int appointeeId, string panName)
        {
            bool IsValid = false;
            _ = new CandidateValidateResponse();
            List<ReasonRemarks> ReasonList = new();
            CandidateValidateUpdatedDataRequest candidateUpdatedDataReq = new();
            AppointeeDetailsResponse? appointeedetail = await _candidateContext.GetAppointeeDetailsAsync(appointeeId);

            string? panFullName = request?.Name?.Trim();
            string? panNumber = request?.PanNumber?.Trim();
            string? panDOB = request?.DateOfBirth?.Trim();
            string? phoneNo = request?.MobileNumber?.Trim();
            string normalizedPanName = CommonUtility.NormalizeWhitespace(panName?.Trim());
            string normalizedPanFullName = CommonUtility.NormalizeWhitespace(panFullName?.Trim());

            if (appointeedetail.AppointeeDetailsId != null && request != null)
            {
                DateTime _inptdob = new();
                bool dOBVarified = false;
                if (string.IsNullOrEmpty(panDOB))
                {
                    dOBVarified = true;
                }
                else
                {
                    _inptdob = Convert.ToDateTime(panDOB);
                }
                if (string.Equals(normalizedPanName, normalizedPanFullName, StringComparison.OrdinalIgnoreCase) &&
                    (appointeedetail?.DateOfBirth == _inptdob || dOBVarified))
                {
                    IsValid = true;
                    string maskedPhoneNumber = CommonUtility.MaskedString(phoneNo);
                    if (appointeedetail?.MobileNo != phoneNo && !string.IsNullOrEmpty(phoneNo?.ToUpper()))
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.MOBILENTMATCH, Inputdata = appointeedetail?.MobileNo, Fetcheddata = maskedPhoneNumber });
                    }
                }
                else
                {
                    if (!string.Equals(normalizedPanName, normalizedPanFullName, StringComparison.OrdinalIgnoreCase))
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.UPLOADEDNAME, Inputdata = appointeedetail?.AppointeeName, Fetcheddata = panFullName });
                    }
                    if (appointeedetail?.DateOfBirth != _inptdob && dOBVarified == false)
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.PANDOB, Inputdata = appointeedetail?.DateOfBirth?.ToShortDateString(), Fetcheddata = _inptdob.ToShortDateString() });
                    }
                }
                PanData _panData = new()
                {
                    PanFatherName = string.Empty,
                    PanName = panName,
                    PanNumber = panNumber,
                };
                candidateUpdatedDataReq = new CandidateValidateUpdatedDataRequest()
                {
                    AppointeeId = appointeeId,
                    EmailId = appointeedetail?.AppointeeEmailId ?? string.Empty,
                    Status = IsValid,
                    Reasons = ReasonList,
                    UserId = appointeedetail.UserId,
                    UserName = appointeedetail.AppointeeName,
                    Type = RemarksType.Pan,
                    panData = _panData,
                    HasData = true,
                    step = 3
                    //PanNumber = panNumber,
                };
            }
            CandidateValidateResponse response = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);

            return response;
        }

        public async Task<AppointeePanValidateResponse> PanDetailsValidation(AppointeePanValidateRequest reqObj)
        {
            PanDetails _apiResponse = new();
            AppointeePanValidateResponse Response = new();
            int priority = 1;  // Start with highest priority
            bool isSuccess = false;

            await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, ActivityLog.PANVERIFICATIONSTART);

            // Attempt provider calls based on priority
            while (!isSuccess)
            {
                var apiProvider = await _masterContext.GetApiProviderDataPriorityBase(ApiType.Pan, priority);

                if (string.IsNullOrEmpty(apiProvider))
                {
                    break;  // Exit loop if no more providers
                }

                if (apiProvider?.ToLower() == ApiProviderType.Karza)
                {
                    _apiResponse = await _karzaApiContext.GetPanDetails(reqObj.panNummber, reqObj.userId);
                }
                else if (apiProvider?.ToLower() == ApiProviderType.SurePass)
                {
                    _apiResponse = await _surepassApiContext.GetPanDetails(reqObj.panNummber, reqObj.userId);
                }
                else if (apiProvider?.ToLower() == ApiProviderType.Signzy)
                {
                    _apiResponse = await _signzyApiContext.GetPanDetails(reqObj.panNummber, reqObj.userId);
                }

                // Check if the call was successful
                if (_apiResponse.StatusCode == HttpStatusCode.OK)
                {
                    isSuccess = true; // Mark success and exit the loop
                }
                if (_apiResponse.StatusCode == HttpStatusCode.ServiceUnavailable || _apiResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    priority++;  // Increase priority to try the next provider
                }
                else
                {
                    break;
                }
            }

            // Post-process response once successful or log failure
            if (isSuccess)
            {
                CandidateValidateResponse verifyResponse = await VarifyPanData(_apiResponse, reqObj.appointeeId, reqObj.panName);
                Response.StatusCode = _apiResponse.StatusCode;
                Response.IsValid = verifyResponse.IsValid;
                Response.Remarks = verifyResponse.Remarks;
                Response.IsUanFetchCall = false;
                string activityState = Response?.IsValid ?? false ? ActivityLog.PANVERIFICATIONCMPLTE : ActivityLog.PANDATAVERIFICATIONFAILED;
                await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, activityState);
            }
            else
            {
                await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, ActivityLog.PANVERIFIFAILED);
                Response.IsValid = false;
                Response.Remarks = GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase, "PAN");
            }

            return Response;
        }

        public async Task<AppointeePassportValidateResponse> PassportDetailsValidation(AppointeePassportValidateRequest reqObj)
        {
            PassportDetails _apiResponse = new();
            AppointeePassportValidateResponse Response = new();
            var apiProvider = await _masterContext.GetApiProviderData(ApiType.Passport);
            await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, ActivityLog.PASPRTVERIFICATIONSTART);
            if (apiProvider?.ToLower() == ApiProviderType.Karza)
            {
                _apiResponse = await _karzaApiContext.GetPassportDetails(reqObj);
            }
            if (apiProvider?.ToLower() == ApiProviderType.SurePass)
            {
                _apiResponse = await _surepassApiContext.GetPassportDetails(reqObj);
            }
            if (apiProvider?.ToLower() == ApiProviderType.Signzy)
            {
                _apiResponse = await _signzyApiContext.GetPassportDetails(reqObj);
                if (_apiResponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    _apiResponse.ReasonPhrase = "Invalid Passport information";
                }
            }
            Response.StatusCode = _apiResponse.StatusCode;
            if (_apiResponse.StatusCode == HttpStatusCode.OK)
            {
                var _VerifyResponse = await VarifyPassportData(_apiResponse, reqObj.appointeeId);

                Response.IsValid = _VerifyResponse.IsValid;
                Response.Remarks = _VerifyResponse.Remarks;

                string activitystate = Response?.IsValid ?? false ? ActivityLog.PASPRTVERIFICATIONCMPLTE : ActivityLog.PASPRTDATAVERIFICATIONFAILED;
                await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, activitystate);
            }
            else
            {
                await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, ActivityLog.PASPRTVERIFIFAILED);
                Response.IsValid = false;
                Response.Remarks = GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase, "PASSPORT");
            }

            return Response;
        }

        private async Task<CandidateValidateResponse> VarifyPassportData(PassportDetails request, int appointeeId)
        {
            bool IsValid = false;
            var response = new CandidateValidateResponse();
            List<ReasonRemarks> ReasonList = new();
            CandidateValidateUpdatedDataRequest candidateUpdatedDataReq = new();
            AppointeeDetailsResponse? appointeedetail = await _candidateContext.GetAppointeeDetailsAsync(appointeeId);
            string passportName = CommonUtility.NormalizeWhitespace(request?.Name?.Trim());
            string? passportNo = request?.PassportNumber?.Trim();
            string? passportDOB = request?.DateOfBirth;
            string? appointeeName = CommonUtility.NormalizeWhitespace(appointeedetail?.AppointeeName?.ToUpper());
            if (appointeedetail.AppointeeDetailsId != null && request != null && !string.IsNullOrEmpty(passportName) && !string.IsNullOrEmpty(passportDOB))
            {
                DateTime _inptdob = Convert.ToDateTime(passportDOB);
                if (appointeedetail?.DateOfBirth == _inptdob && string.Equals(appointeeName, passportName, StringComparison.OrdinalIgnoreCase))
                {
                    IsValid = true;
                }
                else
                {
                    if (appointeedetail?.DateOfBirth != _inptdob)
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.PASSPRTDOB, Inputdata = appointeedetail?.DateOfBirth?.ToShortDateString(), Fetcheddata = passportDOB });
                    }
                    if (!string.Equals(appointeeName, passportName, StringComparison.OrdinalIgnoreCase))
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.PASSPRTNAME, Inputdata = appointeedetail?.AppointeeName, Fetcheddata = passportName });
                    }
                }

                candidateUpdatedDataReq = new CandidateValidateUpdatedDataRequest()
                {
                    AppointeeId = appointeeId,
                    EmailId = appointeedetail?.AppointeeEmailId,
                    Status = IsValid,
                    Reasons = ReasonList,
                    UserId = appointeedetail.UserId,
                    UserName = appointeedetail.AppointeeName,
                    PassportFileNo = request.FileNumber,
                    Type = RemarksType.Passport,
                };
                response = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);
            }
            else
            {
                response.IsValid = false;
                response.Remarks = "No data could be fetched from the passport. Please try again later.";
            }

            return response;
        }

        public async Task<GetUanResponse> GetUanNumber(GetUanNumberDetailsRequest reqObj)
        {
            GetUanResponse Response = new();
            var apiProvider = await _masterContext.GetApiProviderData(ApiType.UAN);
            if (apiProvider?.ToLower() == ApiProviderType.SurePass)
            {
                Response = await GetAadharToUan(reqObj);
            }
            if (apiProvider?.ToLower() == ApiProviderType.Karza)
            {
                Response = await GetMobileToUan(reqObj);
            }
            if (apiProvider?.ToLower() == ApiProviderType.Signzy)
            {
                Response = await GetMobilePanToUan(reqObj);
            }
            return Response;
        }

        private async Task<GetUanResponse> GetAadharToUan(GetUanNumberDetailsRequest reqObj)
        {
            GetUanResponse Response = new();

            _ = new GetCandidateUanDetails();
            GetCandidateUanDetails _apiResponse = await _surepassApiContext.GetUanFromAadhar(reqObj.aaddharNumber, reqObj.userId);
            if (_apiResponse.StatusCode == HttpStatusCode.OK)
            {
                Response.StatusCode = _apiResponse.StatusCode;
                Response.IsUanAvailable = _apiResponse.IsUanAvailable ?? false;
                Response.UanNumber = _apiResponse.UanNumber;
                Response.Remarks = _apiResponse.IsUanAvailable ?? false ? string.Empty : _apiResponse.ReasonPhrase;
                Response.isAadharUanVerified = _apiResponse.isUanLinkVerified ?? false;
                if (_apiResponse.IsUanAvailable ?? false)
                {
                    UanData _uanDetails = new UanData
                    {
                        UanNumber = _apiResponse.UanNumber,
                    };
                    CandidateValidateUpdatedDataRequest candidateUpdatedDataReq = new()
                    {
                        AppointeeId = reqObj.appointeeId,
                        EmailId = string.Empty,
                        UserId = reqObj.userId,
                        UserName = string.Empty,
                        uanData = _uanDetails,
                        Type = RemarksType.UAN
                    };
                    _ = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);
                }
                string activitystate = _apiResponse.IsUanAvailable ?? false ? ActivityLog.UANFETCH : ActivityLog.NOUAN;
                await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, activitystate);
            }
            else
            {
                Response.IsUanAvailable = false;
                Response.Remarks = GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase, "EPFO");
            }
            return Response;
        }

        private async Task<GetUanResponse> GetPanMobileToUan(GetUanNumberDetailsRequest reqObj)
        {
            GetUanResponse Response = new();

            _ = new GetCandidateUanDetails();
            GetCandidateUanDetails _apiResponse = await _karzaApiContext.GetUanFromPan(reqObj.panNumber, reqObj.mobileNumber, reqObj.userId);
            Response.StatusCode = _apiResponse.StatusCode;

            if (_apiResponse.StatusCode == HttpStatusCode.OK)
            {
                Response.IsUanAvailable = _apiResponse.IsUanAvailable ?? false;
                Response.UanNumber = _apiResponse.UanNumber;
                Response.Remarks = _apiResponse.IsUanAvailable ?? false ? string.Empty : _apiResponse.ReasonPhrase;
                List<ReasonRemarks> ReasonList = new();
                UanData _uanDetails = new UanData
                {
                    IsPassbookFetch = false,
                    UanNumber = _apiResponse.UanNumber,
                };
                CandidateValidateUpdatedDataRequest candidateUpdatedDataReq = new()
                {
                    AppointeeId = reqObj.appointeeId,
                    EmailId = string.Empty,
                    UserId = reqObj.userId,
                    UserName = string.Empty,
                    Reasons = ReasonList,
                    uanData = _uanDetails,
                    Type = RemarksType.UAN,
                };
                if (_apiResponse.IsUanAvailable ?? false)
                {
                    candidateUpdatedDataReq.Status = false;
                    _ = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);
                }
                else
                if (_apiResponse.IsUanAvailable == false && string.IsNullOrEmpty(_apiResponse.UanNumber))
                {
                    candidateUpdatedDataReq.Status = true;
                    _ = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);
                }
                if (_apiResponse.IsInactiveUan ?? false)
                {
                    ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.INACTIVE, Inputdata = string.Empty, Fetcheddata = string.Empty });
                    Response.StatusCode = HttpStatusCode.UnprocessableEntity;
                    Response.UserMessage = "Your UAN is inactive. Please activate it on the EPFO portal before proceeding. Visit EPFO UAN Activation for assistance.";
                    Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString() ?? string.Empty;
                }
                string activitystate = _apiResponse.IsUanAvailable ?? false ? ActivityLog.UANFETCH : ActivityLog.NOUAN;
                await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, activitystate);
            }
            else
            {
                Response.IsUanAvailable = false;
                Response.UserMessage = GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase, "EPFO");
            }
            return Response;
        }

        private async Task<GetUanResponse> GetMobileToUan(GetUanNumberDetailsRequest reqObj)
        {
            GetUanResponse Response = new();

            _ = new GetCandidateUanDetails();
            GetCandidateUanDetails _apiResponse = await _karzaApiContext.GetUanFromMobile(reqObj.mobileNumber, reqObj.userId);
            Response.StatusCode = _apiResponse.StatusCode;
            AppointeeDetailsResponse? appointeedetail = await _candidateContext.GetAppointeeDetailsAsync(reqObj.appointeeId);

            if (_apiResponse.StatusCode == HttpStatusCode.OK)
            {
                Response.IsUanAvailable = _apiResponse.IsUanAvailable ?? false;
                Response.UanNumber = _apiResponse.UanNumber;
                Response.Remarks = _apiResponse.IsUanAvailable ?? false ? string.Empty : _apiResponse.ReasonPhrase;
                List<ReasonRemarks> ReasonList = new();
                UanData _uanDetails = new UanData
                {
                    UanNumber = _apiResponse.UanNumber,
                };
                CandidateValidateUpdatedDataRequest candidateUpdatedDataReq = new()
                {
                    AppointeeId = reqObj.appointeeId,
                    EmailId = string.Empty,
                    UserId = reqObj.userId,
                    UserName = string.Empty,
                    Reasons = ReasonList,
                    uanData = _uanDetails,
                    Type = RemarksType.UAN,
                    step = 5
                };
                if (_apiResponse.IsUanAvailable ?? false)
                {
                    _uanDetails.IsUanFromMobile = true;
                    _uanDetails.AadharUanLinkYN = true;

                    _ = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);
                }
                else
                if (_apiResponse.IsUanAvailable == false && appointeedetail.IsUanAvailable == false && string.IsNullOrEmpty(_apiResponse.UanNumber))
                {
                    candidateUpdatedDataReq.Status = true;
                    _ = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);
                }
                if (_apiResponse.IsInactiveUan ?? false)
                {
                    ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.INACTIVE, Inputdata = string.Empty, Fetcheddata = string.Empty });
                    Response.StatusCode = HttpStatusCode.UnprocessableEntity;
                    Response.UserMessage = "Your UAN is inactive. Please activate it on the EPFO portal before proceeding. Visit EPFO UAN Activation for assistance.";
                    Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString() ?? string.Empty;
                }
                string activitystate = _apiResponse.IsUanAvailable ?? false ? ActivityLog.UANFETCH : ActivityLog.NOUAN;
                await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, activitystate);
            }
            else
            {
                Response.IsUanAvailable = false;
                Response.UserMessage = GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase, "EPFO");
            }
            return Response;
        }

        private async Task<GetUanResponse> GetMobilePanToUan(GetUanNumberDetailsRequest reqObj)
        {
            GetUanResponse Response = new();
            GetCandidateUanDetails _apiResponse = await _signzyApiContext.GetUanFromMobilenPan(reqObj.panNumber, reqObj.mobileNumber, reqObj.userId);
            AppointeeDetailsResponse? appointeedetail = await _candidateContext.GetAppointeeDetailsAsync(reqObj.appointeeId);

            Response.StatusCode = _apiResponse.StatusCode;
            if (_apiResponse.StatusCode == HttpStatusCode.OK || _apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                Response.StatusCode = HttpStatusCode.OK;
                Response.IsUanAvailable = _apiResponse.IsUanAvailable ?? false;
                Response.UanNumber = _apiResponse.UanNumber;
                Response.Remarks = !(_apiResponse.IsUanAvailable ?? false) ? "UAN Not available" : string.Empty;
                List<ReasonRemarks> ReasonList = new();
                UanData _uanDetails = new UanData
                {
                    UanNumber = _apiResponse.UanNumber,
                };
                CandidateValidateUpdatedDataRequest candidateUpdatedDataReq = new()
                {
                    AppointeeId = reqObj.appointeeId,
                    EmailId = string.Empty,
                    UserId = reqObj.userId,
                    UserName = string.Empty,
                    Reasons = ReasonList,
                    uanData = _uanDetails,
                    Type = RemarksType.UAN,
                    step = 5
                };
                if (_apiResponse.IsUanAvailable ?? false)
                {
                    _uanDetails.IsUanFromMobile = true;
                    _uanDetails.AadharUanLinkYN = true;

                    _ = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);
                }
                else
                if (_apiResponse.IsUanAvailable == false && appointeedetail.IsUanAvailable == false && string.IsNullOrEmpty(_apiResponse.UanNumber))
                {
                    candidateUpdatedDataReq.Status = true;
                    _ = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);
                }
                if (_apiResponse.IsInactiveUan ?? false)
                {
                    ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.INACTIVE, Inputdata = string.Empty, Fetcheddata = string.Empty });
                    Response.StatusCode = HttpStatusCode.UnprocessableEntity;
                    Response.UserMessage = "Your UAN is inactive. Please activate it on the EPFO portal before proceeding. Visit EPFO UAN Activation for assistance.";
                    Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString() ?? string.Empty;
                }
                string activitystate = _apiResponse.IsUanAvailable ?? false ? ActivityLog.UANFETCH : ActivityLog.NOUAN;
                await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, activitystate);
            }
            else
            {
                var msg = _apiResponse.StatusCode switch
                {
                    HttpStatusCode.BadRequest => "The mobile number entered is invalid. Please check and try again later.",
                    HttpStatusCode.Conflict => "server is currently busy. Please try again later.",
                    _ => string.Empty
                };
                Response.IsUanAvailable = false;
                Response.UserMessage = string.IsNullOrEmpty(msg) ? GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase, "EPFO") : msg;
            }
            return Response;
        }

        public async Task<AadharGenerateOTPDetails> GeneratetAadharOTP(AppointeeAadhaarValidateRequest reqObj)
        {
            AadharGenerateOTPDetails Response = new();
            AadharGenerateOTPDetails _apiResponse = new();

            var apiProvider = await _masterContext.GetApiProviderData(ApiType.Adhaar);

            AadharDetailsData _AaddhaarDetails = new()
            {
                AadhaarName = reqObj?.aadharName,
                AadhaarNumber = reqObj?.aadharNumber,
                AadhaarNumberView = reqObj?.aadharNumber,
            };

            CandidateValidateUpdatedDataRequest candidateUpdatedDataReq = new()
            {
                AppointeeId = reqObj.appointeeId,
                EmailId = string.Empty,
                UserId = reqObj.userId,
                UserName = string.Empty,
                Type = RemarksType.Adhaar,
                aadharData = _AaddhaarDetails,
            };
            _ = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);

            if (apiProvider?.ToLower() == ApiProviderType.SurePass)
            {
                _apiResponse = await _surepassApiContext.GenerateAadharOTP(reqObj.aadharNumber, reqObj.userId);
            }
            if (apiProvider?.ToLower() == ApiProviderType.Karza)
            {
                _apiResponse = await _karzaApiContext.GenerateAadharOTP(reqObj.aadharNumber, reqObj.userId);
            }
            Response.StatusCode = _apiResponse.StatusCode;
            if (_apiResponse.StatusCode == HttpStatusCode.OK)
            {
                return _apiResponse;
            }
            else
            {
                if ((int)_apiResponse.StatusCode == (int)HttpStatusCode.UnprocessableEntity)
                {
                    await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, ActivityLog.ADHINVALID);

                    AadharValidationRequest VarifyReq = new();
                    AadharSubmitOtpDetails _adhaardata = new() { AadharNumber = reqObj?.aadharNumber };
                    VarifyReq.AadharDetails = _adhaardata;
                    VarifyReq.isValidAdhar = false;
                    VarifyReq.AppointeeId = reqObj.appointeeId;
                    VarifyReq.AppointeeAadhaarName = reqObj.aadharName;
                    _ = await VerifyAadharData(VarifyReq);
                }
                else
                {
                    await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, ActivityLog.ADHVERIFIFAILED);
                }
                Response.UserMessage = GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase?.ToString(), "UIDAI (Aadhar)");
                Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString() ?? string.Empty;
            }

            return Response;
        }

        public async Task<AadharSubmitOtpDetails> SubmitAadharOTP(AppointeeAadhaarSubmitOtpRequest reqObj)
        {
            AadharSubmitOtpDetails Response = new();
            AadharSubmitOtpDetails _apiResponse = new();
            var apiProvider = await _masterContext.GetApiProviderData(ApiType.Adhaar);
            if (apiProvider?.ToLower() == ApiProviderType.Karza)
            {
                //AppointeeDetailsResponse appointeedetail = await _candidateContext.GetAppointeeDetailsAsync(reqObj.appointeeId);
                _apiResponse = await _karzaApiContext.SubmitAadharOTP(reqObj.client_id, reqObj.aadharNumber, reqObj.otp, reqObj.shareCode, reqObj.userId);

                //string? aadharNo = appointeedetail.AadhaarNumber;
            }
            if (apiProvider?.ToLower() == ApiProviderType.SurePass)
            {
                _apiResponse = await _surepassApiContext.SubmitAadharOTP(reqObj.client_id, reqObj.otp, reqObj.userId);
            }

            Response.StatusCode = _apiResponse.StatusCode;
            if (_apiResponse.StatusCode != HttpStatusCode.OK)
            {
                bool IsUnprocessableEntity = (int)_apiResponse.StatusCode == (int)HttpStatusCode.UnprocessableEntity;
                Response.UserMessage = IsUnprocessableEntity ? "Invalid OTP. Please enter the correct OTP and try again." : GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase?.ToString(), "UIDAI (Aadhar)");
                Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString() ?? string.Empty;
            }
            else
            {
                Response = _apiResponse;
            }
            return Response;
        }

        public async Task<AadharSubmitOtpDetails> GetAadharDetailsFromXml(string? xmlData)
        {
            AadharSubmitOtpDetails response = new();
            if (xmlData != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlData);

                // Navigate and extract data
                XmlNode root = xmlDoc.DocumentElement;
                XmlNode personNode = root.SelectSingleNode("UidData");
                if (personNode != null)
                {
                    XmlNode personNodeImage = personNode.SelectSingleNode("Pht");
                    XmlNode personalInfoNode = personNode.SelectSingleNode("Poi");
                    XmlNode personalAddresNode = personNode.SelectSingleNode("Poa");
                    string referenceId = root.SelectSingleNode("@referenceId").Value;
                    string nameNode = personalInfoNode.SelectSingleNode("@name").Value;
                    string genderNode = personalInfoNode.SelectSingleNode("@gender").Value;
                    string dobNode = personalInfoNode.SelectSingleNode("@dob").Value;
                    string hashMobileNo = personalInfoNode.SelectSingleNode("@m").Value;
                    string careofNode = personalAddresNode.SelectSingleNode("@careof").Value;
                    string profileImage = personNodeImage.InnerText;
                    var lastFourDigit = new string(referenceId.Where(char.IsDigit).Take(4).ToArray());
                    response.Name = nameNode;
                    response.Dob = dobNode;
                    response.CareOf = careofNode;
                    response.Gender = genderNode;
                    response.AadharNumber = $"{"XXXXXXXX"}{lastFourDigit}";
                    response.MobileNumberHash = hashMobileNo?.Trim();
                    response.AadharImage = profileImage;
                }
            }
            return response;
        }

        public async Task<CandidateValidateResponse> VerifyAadharData(AadharValidationRequest reqObj)
        {
            bool IsValid = false;
            CandidateValidateResponse response = new();
            AadharDetailsData _aadharData = new();
            List<ReasonRemarks> ReasonList = new();
            _ = new CandidateValidateUpdatedDataRequest();
            AppointeeDetailsResponse? appointeedetail = await _candidateContext.GetAppointeeDetailsAsync(reqObj.AppointeeId);
            if (reqObj.isValidAdhar)
            {
                var lastAdhar4Digit = reqObj.AadharDetails?.AadharNumber?.Trim()?.LastOrDefault().ToString();
                var isMobileValid = string.IsNullOrEmpty(reqObj.AadharDetails?.MobileNumberHash) ? true : CommonUtility.CheckMobileNumber(appointeedetail.MobileNo, reqObj.AadharDetails?.MobileNumberHash, reqObj?.sharePhrase ?? "", lastAdhar4Digit);

                string? aadharName = CommonUtility.NormalizeWhitespace(reqObj?.AppointeeAadhaarName?.Trim());
                string? appointeeAadhaarFullName = CommonUtility.NormalizeWhitespace(reqObj?.AadharDetails?.Name);
                string? appointeeAadhaarGender = reqObj?.AadharDetails?.Gender;
                string? appointeeAadhaarDOB = reqObj?.AadharDetails?.Dob;
                string? appointeeCareOf = reqObj?.AadharDetails?.CareOf;
                string? appointeeAadhaarNumber = reqObj?.AadharDetails?.AadharNumber;
                string? appointeeNname = CommonUtility.NormalizeWhitespace(appointeedetail.AppointeeName?.Trim());

                _aadharData.AadhaarName = aadharName;
                _aadharData.AadhaarNumber = appointeeAadhaarNumber;
                _aadharData.NameFromAadhaar = appointeeAadhaarFullName;
                _aadharData.GenderFromAadhaar = appointeeAadhaarGender;
                _aadharData.DobFromAadhaar = appointeeAadhaarDOB;
                _aadharData.ProfileImageAadhaar = reqObj?.AadharDetails?.AadharImage;

                if (appointeedetail.AppointeeDetailsId != null && reqObj.AadharDetails != null)
                {
                    DateTime _inptdob = Convert.ToDateTime(appointeeAadhaarDOB);
                    bool hasCoName = !string.IsNullOrEmpty(appointeeCareOf);
                    _ = !hasCoName || appointeedetail?.MemberName?.ToUpper() == appointeeCareOf;

                    if (isMobileValid && string.Equals(appointeeNname, appointeeAadhaarFullName, StringComparison.OrdinalIgnoreCase) &&
                        appointeedetail?.Gender?.ToUpper() == appointeeAadhaarGender?.ToUpper() && appointeedetail?.DateOfBirth == _inptdob)//&& validateCareOfName)

                    {
                        IsValid = true;
                    }
                    else
                    {
                        if (!isMobileValid)
                        {
                            ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.MOBILENTMATCH, Inputdata = appointeedetail?.MobileNo, Fetcheddata = string.Empty });
                        }
                        if (appointeedetail?.Gender?.ToUpper() != appointeeAadhaarGender?.ToUpper())
                        {
                            ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.GENDER, Inputdata = appointeedetail?.Gender, Fetcheddata = appointeeAadhaarGender });
                        }
                        if (appointeedetail?.DateOfBirth != _inptdob)
                        {
                            ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.DOB, Inputdata = appointeedetail?.DateOfBirth?.ToShortDateString(), Fetcheddata = appointeeAadhaarDOB });
                        }
                        if (!string.Equals(appointeeNname, appointeeAadhaarFullName, StringComparison.OrdinalIgnoreCase))
                        {
                            ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.UPLOADEDNAME, Inputdata = appointeeNname, Fetcheddata = appointeeAadhaarFullName });
                        }
                    }

                    string _activityStatus = IsValid ? ActivityLog.ADHVERIFICATIONCMPLTE : ActivityLog.ADHDATAVERIFICATIONFAILED;
                    await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, _activityStatus);
                }
            }
            else
            {
                IsValid = false;
                ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.INVDADHAR, Inputdata = reqObj?.AadharDetails?.AadharNumber, Fetcheddata = string.Empty });
            }

            CandidateValidateUpdatedDataRequest candidateUpdatedDataReq = new()
            {
                AppointeeId = reqObj.AppointeeId,
                EmailId = appointeedetail.AppointeeEmailId,
                Status = IsValid,
                Reasons = ReasonList,
                UserId = appointeedetail.UserId,
                UserName = appointeedetail.AppointeeName,
                Type = RemarksType.Adhaar,
                aadharData = _aadharData,
                step = 2
                //PanNumber = panNumber,
            };

            response = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);
            if (!string.IsNullOrEmpty(_aadharData.ProfileImageAadhaar))
            {
                await _candidateContext.UpdateCandidateImageData(appointeedetail.AppointeeId ?? 0, appointeedetail.CandidateId, appointeedetail.UserId, _aadharData.ProfileImageAadhaar);
            }
            return response;
        }

        public async Task<UanGenerateOtpDetails> GeneratetUANOTP(UanGenerateOtpRequest reqObj)
        {
            UanGenerateOtpDetails Response = new();
            UanGenerateOtpDetails _apiResponse = new();
            List<ReasonRemarks> ReasonList = new();
            UanData _uanDetails = new UanData
            {
                UanNumber = reqObj.UanNumber,
            };
            await _candidateContext.UpdateCandidateUANData(reqObj.appointeeId, reqObj.UanNumber);

            var apiProvider = await _masterContext.GetApiProviderData(ApiType.EPFO);
            await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, ActivityLog.UANVERIFICATIONSTART);
            if (apiProvider?.ToLower() == ApiProviderType.Karza)
            {
                _apiResponse = await _karzaApiContext.GenerateUANOTP(reqObj.UanNumber, reqObj.userId);
                Response.StatusCode = _apiResponse.StatusCode == HttpStatusCode.NotFound ? HttpStatusCode.UnprocessableEntity : _apiResponse.StatusCode;
            }
            if (apiProvider?.ToLower() == ApiProviderType.SurePass)
            {
                _apiResponse = await _surepassApiContext.GenerateUANOTP(reqObj.UanNumber, reqObj.userId);
                Response.StatusCode = _apiResponse.StatusCode;
            }
            if (apiProvider?.ToLower() == ApiProviderType.Signzy)
            {
                _apiResponse = await _signzyApiContext.GenerateUANOTP(reqObj.UanNumber, reqObj.MobileNumber, reqObj.userId);
                Response.StatusCode = _apiResponse.StatusCode;// == HttpStatusCode.NotFound ? HttpStatusCode.UnprocessableEntity : _apiResponse.StatusCode;
            }

            if (_apiResponse.StatusCode == HttpStatusCode.OK)
            {
                return _apiResponse;
            }
            else
            {
                await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, ActivityLog.UANVERIFIFAILED);
                Response.UserMessage = (int)_apiResponse.StatusCode == (int)HttpStatusCode.UnprocessableEntity ?
                    "Your UAN is inactive. Please activate it on the EPFO portal before proceeding. Visit EPFO UAN Activation for assistance." : GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase?.ToString(), "EPFO");
                Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString() ?? string.Empty;
            }

            return Response;
        }

        public async Task<GetPassbookDetailsResponse> GetPassbookBySubmitOTP(AppointeeUANSubmitOtpRequest reqObj)
        {
            GetPassbookDetailsRequest getPassbookReq = new();
            GetPassbookDetailsResponse GetPassbookDetails = new();
            UanSubmitOtpDetails SubmitOtpResponse = new();
            bool isValid = false;
            var apiProvider = await _masterContext.GetApiProviderData(ApiType.EPFO);

            if (apiProvider?.ToLower() == ApiProviderType.SurePass)
            {
                SubmitOtpResponse = await SubmitUanOTP(reqObj, apiProvider);
                if (SubmitOtpResponse.StatusCode != HttpStatusCode.OK)
                {
                    GetPassbookDetails.StatusCode = SubmitOtpResponse.StatusCode;
                    GetPassbookDetails.UserMessage = SubmitOtpResponse?.UserMessage ?? string.Empty;
                    GetPassbookDetails.ReasonPhrase = SubmitOtpResponse?.ReasonPhrase ?? string.Empty;
                }
                else
                {
                    if (!(SubmitOtpResponse?.OtpValidated ?? false))
                    {
                        GetPassbookDetails.StatusCode = HttpStatusCode.NotAcceptable;
                        GetPassbookDetails.UserMessage = "OTP verification failed. Please enter the correct OTP and try again.";
                        GetPassbookDetails.ReasonPhrase = SubmitOtpResponse?.ReasonPhrase?.ToString() ?? string.Empty;
                    }
                    else
                    {
                        isValid = true;
                    }
                }
            }
            if (apiProvider?.ToLower() == ApiProviderType.Karza)
            {
                isValid = true;
                getPassbookReq.Otp = reqObj.otp;
                SubmitOtpResponse.ClientId = reqObj.client_id;
            }
            if (apiProvider?.ToLower() == ApiProviderType.Signzy)
            {
                SubmitOtpResponse = await SubmitUanOTP(reqObj, apiProvider);
                if (SubmitOtpResponse.StatusCode != HttpStatusCode.OK)
                {
                    GetPassbookDetails.StatusCode = SubmitOtpResponse.StatusCode;
                    GetPassbookDetails.UserMessage = SubmitOtpResponse?.UserMessage ?? string.Empty;
                    GetPassbookDetails.ReasonPhrase = SubmitOtpResponse?.ReasonPhrase ?? string.Empty;
                }
                else
                {
                    if (!(SubmitOtpResponse?.OtpValidated ?? false))
                    {
                        GetPassbookDetails.StatusCode = HttpStatusCode.NotAcceptable;
                        GetPassbookDetails.UserMessage = "OTP verification failed. Please enter the correct OTP and try again.";
                        GetPassbookDetails.ReasonPhrase = SubmitOtpResponse?.ReasonPhrase?.ToString() ?? string.Empty;
                    }
                    else
                    {
                        SubmitOtpResponse.ClientId = string.IsNullOrEmpty(SubmitOtpResponse.ClientId) ? reqObj.client_id : SubmitOtpResponse.ClientId;
                        isValid = true;
                    }
                }
            }
            if (isValid)
            {
                getPassbookReq.UserId = reqObj.userId;
                getPassbookReq.AppointeeCode = reqObj.AppointeeCode;
                getPassbookReq.AppointeeId = reqObj.appointeeId;
                getPassbookReq.OtpDetails = SubmitOtpResponse;

                GetPassbookDetails = await GetPfPassbookData(getPassbookReq);

                if (GetPassbookDetails.StatusCode != HttpStatusCode.OK)
                {
                    GetPassbookDetails.StatusCode = GetPassbookDetails.StatusCode;
                    GetPassbookDetails.UserMessage = GetPassbookDetails?.UserMessage ?? string.Empty;
                    GetPassbookDetails.ReasonPhrase = GetPassbookDetails?.ReasonPhrase ?? string.Empty;
                }
            }

            return GetPassbookDetails;
        }

        private async Task<UanSubmitOtpDetails> SubmitUanOTP(AppointeeUANSubmitOtpRequest reqObj, string apiProvider)
        {
            UanSubmitOtpDetails response = new();
            UanSubmitOtpDetails apiResponse = null;

            if (apiProvider?.ToLower() == ApiProviderType.SurePass)
            {
                apiResponse = await _surepassApiContext.SubmitUanOTP(reqObj.client_id, reqObj.otp, reqObj.userId);
            }
            else if (apiProvider?.ToLower() == ApiProviderType.Signzy)
            {
                apiResponse = await _signzyApiContext.SubmitUanOTP(reqObj.client_id, reqObj.otp, reqObj.userId);
            }

            if (apiResponse != null)
            {
                response.StatusCode = apiResponse.StatusCode;

                if (apiResponse.StatusCode == HttpStatusCode.OK)
                {
                    return apiResponse;
                }

                bool isInvalidOtp = (int)apiResponse.StatusCode == (apiProvider?.ToLower() == ApiProviderType.SurePass
                                    ? (int)HttpStatusCode.UnprocessableEntity
                                    : (int)HttpStatusCode.BadRequest);

                response.UserMessage = isInvalidOtp ? "Invalid OTP. Please enter the correct OTP and try again."
                                                    : GenarateErrorMsg((int)apiResponse.StatusCode, apiResponse.ReasonPhrase, "EPFO");
                response.ReasonPhrase = apiResponse.ReasonPhrase ?? string.Empty;
                response.OtpValidated = apiResponse.OtpValidated;
            }
            else
            {
                response.StatusCode = apiResponse?.StatusCode ?? HttpStatusCode.NotAcceptable;
                response.UserMessage = "Somethin went wrong. Please retry";
                response.ReasonPhrase = apiResponse?.ReasonPhrase ?? string.Empty;
                response.OtpValidated = apiResponse?.OtpValidated ?? false;
                response = apiResponse;
            }

            return response;
        }

        public async Task<GetPassbookDetailsResponse> GetPfPassbookData(GetPassbookDetailsRequest reqObj)
        {
            GetPassbookDetailsResponse Response = new();
            PfPassbookDetails _apiResponse = new();
            EpsContributionCheckResult? epsContributionDetails = new();
            List<EpsContributionSummary>? epsContributionSummary = new();
            var apiProvider = await _masterContext.GetApiProviderData(ApiType.EPFO);
            bool isPensionApplicable = false;
            string passbookJsonData = string.Empty;
            if (apiProvider?.ToLower() == ApiProviderType.SurePass)
            {
                _apiResponse = await _surepassApiContext.GetPassbookDetails(reqObj?.OtpDetails?.ClientId, reqObj.UserId);

                passbookJsonData = JsonConvert.SerializeObject(_apiResponse.Passbkdata);
                PassbookData? _passBookData = _apiResponse?.Passbkdata;
                if (_passBookData != null)
                {
                    string? _pensionshare = _passBookData?.companies?.Values.FirstOrDefault()?.passbook.LastOrDefault()?.pension_share;
                    if (!string.IsNullOrEmpty(_pensionshare))
                    {
                        isPensionApplicable = Convert.ToInt32(_pensionshare) > 0;
                    }
                }
            }
            if (apiProvider?.ToLower() == ApiProviderType.Karza)
            {
                _apiResponse = await _karzaApiContext.GetPassbookBySubmitUanOTP(reqObj.OtpDetails.ClientId, reqObj.Otp, reqObj.UserId);
                UanPassbookDetails? _passBookData = _apiResponse?.KarzaPassbkdata;
                epsContributionDetails = await _karzaApiContext.CheckEpsContributionConsistencyForkarza(_passBookData);

                passbookJsonData = JsonConvert.SerializeObject(_passBookData);
            }
            if (apiProvider?.ToLower() == ApiProviderType.Signzy)
            {
                var maxRetries = 10;
                var retryDelay = 2000; // 2 seconds delay between checks
                SignzyUanPassbookDetails? _passBookData = new();
                for (int i = 0; i < maxRetries; i++)
                {
                    // Step 1: Check if callback data is available in cache
                    _apiResponse = await _signzyApiContext.GetPassbook(reqObj.OtpDetails.ClientId, reqObj.UserId);
                    _passBookData = _apiResponse?.SignzyPassbkdata;
                    if (_passBookData != null || _apiResponse.StatusCode != HttpStatusCode.OK)
                    {
                        // Step 2: Return response when found
                        break;
                    }

                    // Step 3: If not found, wait before retrying
                    Thread.Sleep(retryDelay); // wait before retrying
                }
                //_apiResponse = await _signzyApiContext.GetPassbook(reqObj.OtpDetails.ClientId, reqObj.UserId);
                //SignzyUanPassbookDetails? _passBookData = _apiResponse?.SignzyPassbkdata;
                epsContributionDetails = await _signzyApiContext.CheckEpsContributionConsistency(_passBookData);

                // TO DO EPS Contribution
                //if (_passBookData?.EstDetails.Count > 0)
                //{
                //    int _pensionshare = _passBookData?.EstDetails?.FirstOrDefault().PensionTotal ?? 0;
                //    if (_pensionshare > 0)
                //    {
                //        isPensionApplicable = Convert.ToInt32(_pensionshare) > 0;
                //        _passBookData.EstDetails.FirstOrDefault().Value.PensionTotal = 1;
                //    }
                //}
                passbookJsonData = JsonConvert.SerializeObject(_passBookData);
            }
            Response.StatusCode = _apiResponse.StatusCode;
            if (_apiResponse.StatusCode != HttpStatusCode.OK)
            {
                await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, ActivityLog.UANVERIFIFAILED);
                Response.UserMessage = (int)_apiResponse.StatusCode == (int)HttpStatusCode.UnprocessableEntity ? "The UAN entered is invalid. Please check and try again." : GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase?.ToString(), "EPFO");
                Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString() ?? string.Empty;
            }
            else
            {
                if (_apiResponse.Passbkdata == null && _apiResponse.KarzaPassbkdata == null && _apiResponse.SignzyPassbkdata == null)
                {
                    await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, ActivityLog.UANVERIFIFAILED);
                    Response.StatusCode = HttpStatusCode.UnprocessableEntity;
                    Response.UserMessage = "The site is currently unreachable. Please try again after some time or opt for manual passbook upload.";
                }
                else
                {
                    await SavePfPassbookData(reqObj, passbookJsonData, apiProvider?.ToLower());
                    if (apiProvider?.ToLower() == ApiProviderType.SurePass)
                    {
                        PassbookData? _passBookData = _apiResponse.Passbkdata;
                        Response.FathersName = _passBookData?.father_name;
                        Response.Name = _passBookData?.full_name;
                        Response.DateOfBirth = _passBookData.dob;
                        Response.IsPensionApplicable = isPensionApplicable;
                        Response.PfUan = _passBookData?.pf_uan;
                    }
                    if (apiProvider?.ToLower() == ApiProviderType.Karza)
                    {
                        EmployeeDetails _passBookData = _apiResponse.KarzaPassbkdata?.employee_details;
                        Response.FathersName = _passBookData?.father_name ?? string.Empty;
                        Response.Name = _passBookData?.member_name ?? string.Empty;
                        Response.DateOfBirth = _passBookData?.dob ?? string.Empty;
                        Response.IsPensionApplicable = HasEpsContribution(epsContributionDetails?.EpsContributionSummary);
                        Response.EpsContributionDetails = epsContributionDetails.EpsContributionSummary;
                        Response.IsDualEmployement = epsContributionDetails?.HasDualEmplyement ?? false;
                        Response.PfUan = reqObj.UanNumber;
                    }
                    if (apiProvider?.ToLower() == ApiProviderType.Signzy)
                    {
                        SignzyUanPassbookDetails _passBookData = _apiResponse.SignzyPassbkdata;
                        Response.FathersName = _passBookData?.EmployeeDetails?.FatherName ?? string.Empty;
                        Response.Name = _passBookData?.EmployeeDetails?.MemberName;
                        Response.DateOfBirth = _passBookData?.EmployeeDetails?.Dob ?? string.Empty;
                        Response.IsPensionApplicable = HasEpsContribution(epsContributionDetails?.EpsContributionSummary);
                        Response.EpsContributionDetails = epsContributionDetails?.EpsContributionSummary;
                        Response.IsDualEmployement = epsContributionDetails?.HasDualEmplyement ?? false;
                        Response.PfUan = reqObj.UanNumber;
                        //Response.IsCallBack = true;
                    }

                    if (string.IsNullOrEmpty(Response.Name) || string.IsNullOrEmpty(Response.DateOfBirth))
                    {
                        await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, ActivityLog.UANVERIFIFAILED);
                        Response.StatusCode = HttpStatusCode.UnprocessableEntity;
                        Response.UserMessage = "The site is currently unreachable. Please try again after some time or opt for manual passbook upload.";
                    }
                }
            }

            return Response;
        }

        public async Task<GetPassbookDetailsResponse> ValidateBackGetPfPassbookData(SignzyUanPassbookDetails reqObj, int appointeeId, int userId, string uanNumber)
        {
            GetPassbookDetailsResponse Response = new();
            PfPassbookDetails _apiResponse = new();
            EpsContributionCheckResult? epsContributionDetails = new();
            List<EpsContributionSummary>? epsContributionSummary = new();
            SignzyUanPassbookDetails? _passBookData = reqObj;
            epsContributionDetails = await _signzyApiContext.CheckEpsContributionConsistency(_passBookData);

            string passbookJsonData = JsonConvert.SerializeObject(_passBookData);

            Response.StatusCode = _apiResponse.StatusCode;
            if (_apiResponse.StatusCode != HttpStatusCode.OK)
            {
                await _activityContext.PostActivityDetails(appointeeId, userId, ActivityLog.UANVERIFIFAILED);
                Response.UserMessage = (int)_apiResponse.StatusCode == (int)HttpStatusCode.UnprocessableEntity ? "The UAN entered is invalid. Please check and try again." : GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase?.ToString(), "EPFO");
                Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString() ?? string.Empty;
            }
            else
            {
                if (reqObj == null)
                {
                    await _activityContext.PostActivityDetails(appointeeId, userId, ActivityLog.UANVERIFIFAILED);
                    Response.StatusCode = HttpStatusCode.UnprocessableEntity;
                    Response.UserMessage = "The site is currently unreachable. Please try again after some time or opt for manual passbook upload.";
                }
                else
                {
                    GetPassbookDetailsRequest req = new GetPassbookDetailsRequest
                    {
                        AppointeeId = appointeeId,
                        UanNumber = uanNumber,
                        UserId = userId,
                    };
                    await SavePfPassbookData(req, passbookJsonData, ApiProviderType.Signzy?.ToLower());
                    Response.FathersName = _passBookData?.EmployeeDetails?.FatherName ?? string.Empty;
                    Response.Name = _passBookData?.EmployeeDetails?.MemberName;
                    Response.DateOfBirth = _passBookData?.EmployeeDetails?.Dob ?? string.Empty;
                    Response.IsPensionApplicable = HasEpsContribution(epsContributionDetails?.EpsContributionSummary);
                    Response.EpsContributionDetails = epsContributionDetails?.EpsContributionSummary;
                    Response.IsDualEmployement = epsContributionDetails?.HasDualEmplyement ?? false;
                    Response.PfUan = uanNumber;
                }
            }

            return Response;
        }

        private async Task SavePfPassbookData(GetPassbookDetailsRequest reqObj, string apiResponse, string apiProvider)
        {
            EmployementHistoryDetails uploadReq = new()
            {
                AppointeeId = reqObj.AppointeeId,
                Provider = apiProvider,
                SubType = JsonTypeAlias.EmployeePassBook,
                EmpData = apiResponse,
                UserId = reqObj.UserId,
            };
            await _candidateContext.PostAppointeeEmployementDetailsAsync(uploadReq);
        }

        public async Task<CandidateValidateResponse> VerifyUanData(UanValidationRequest reqObj)
        {
            bool IsValid = false;
            _ = new CandidateValidateResponse();
            List<ReasonRemarks> ReasonList = new();
            _ = new CandidateValidateUpdatedDataRequest();
            AppointeeDetailsResponse? appointeedetail = await _candidateContext.GetAppointeeDetailsAsync(reqObj.AppointeeId);
            string? UanFullName = reqObj?.PassbookDetails?.Name;
            string? UanFatherName = CommonUtility.NormalizeWhitespace(reqObj?.PassbookDetails?.FathersName);
            string? UanDob = reqObj?.PassbookDetails?.DateOfBirth;
            bool _IsPensionApplicable = reqObj?.PassbookDetails?.IsPensionApplicable ?? false;
            bool _IsPensionGapIdentified = HasEpsGap(reqObj?.PassbookDetails?.EpsContributionDetails);
            bool _IsDualEmployementIdentified = reqObj?.PassbookDetails?.IsDualEmployement ?? false;
            bool _isFatherNameValidate = false;
            string normalizedUANFullName = CommonUtility.NormalizeWhitespace(UanFullName?.Trim());

            if (!reqObj.IsUanActive)
            {
                ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.INACTIVE, Inputdata = string.Empty, Fetcheddata = string.Empty });
            }
            else if (appointeedetail.AppointeeDetailsId != null && reqObj?.PassbookDetails != null)
            {
                string? AppointeeFullName = string.IsNullOrEmpty(appointeedetail?.NameFromAadhaar) ? appointeedetail?.AppointeeName?.Trim() : appointeedetail?.NameFromAadhaar?.Trim();
                string normalizedApoointeeName = CommonUtility.NormalizeWhitespace(AppointeeFullName?.Trim());
                string? fathersName = CommonUtility.NormalizeWhitespace(appointeedetail.MemberName);
                DateTime _inptdob = Convert.ToDateTime(UanDob);

                _isFatherNameValidate = appointeedetail.IsFnameVarified ?? false ? true : !string.IsNullOrEmpty(fathersName) && (string.Equals(fathersName, UanFatherName, StringComparison.OrdinalIgnoreCase));
                if (string.Equals(normalizedApoointeeName, normalizedUANFullName, StringComparison.OrdinalIgnoreCase)

                    && appointeedetail?.DateOfBirth == _inptdob && _isFatherNameValidate && !_IsPensionGapIdentified)
                {
                    IsValid = true;
                }
                else
                {
                    if (!string.Equals(normalizedApoointeeName, normalizedUANFullName, StringComparison.OrdinalIgnoreCase))
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.NAME, Inputdata = AppointeeFullName, Fetcheddata = UanFullName });
                    }
                    if (!_isFatherNameValidate)
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.CAREOFNAME, Inputdata = fathersName, Fetcheddata = UanFatherName });
                    }
                    if (appointeedetail?.DateOfBirth != _inptdob)
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.DOB, Inputdata = appointeedetail?.DateOfBirth?.ToShortDateString(), Fetcheddata = UanDob });
                    }
                    if (_IsPensionGapIdentified)
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.PENSIONGAPFIND, Inputdata = string.Empty, Fetcheddata = string.Empty });
                    }
                    if (_IsPensionGapIdentified)
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.PENSIONGAPFIND, Inputdata = string.Empty, Fetcheddata = string.Empty });
                    }
                    //if (_IsDualEmployementIdentified)
                    //{
                    //    ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.DUALEMPNT, Inputdata = string.Empty, Fetcheddata = string.Empty });
                    //}
                }

                if (!(appointeedetail.IsPFverificationReq ?? true))
                {
                    ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.ISPFREQUIRED, Inputdata = string.Empty, Fetcheddata = string.Empty });
                }

                string _activityStatus = IsValid ? ActivityLog.UANVERIFICATIONCMPLTE : ActivityLog.UANDATAVERIFICATIONFAILED;
                await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, _activityStatus);
            }
            UanData _uanDetails = new UanData
            {
                IsPassbookFetch = reqObj.IsPassbookFetch,
                IsPensionApplicable = _IsPensionApplicable,
                IsPensionGap = _IsPensionGapIdentified,
                UanNumber = string.IsNullOrEmpty(reqObj?.PassbookDetails?.PfUan) ? appointeedetail?.UANNumber : reqObj?.PassbookDetails?.PfUan,
                IsEmployementVarified = IsValid,
                IsDualEmployementIdentified = _IsDualEmployementIdentified,
            };
            CandidateValidateUpdatedDataRequest candidateUpdatedDataReq = new()
            {
                AppointeeId = reqObj.AppointeeId,
                EmailId = appointeedetail.AppointeeEmailId,
                Status = IsValid,
                Reasons = ReasonList,
                UserId = reqObj.UserId,
                UserName = appointeedetail?.AppointeeName,
                Type = RemarksType.UAN,
                uanData = _uanDetails,
                IsFNameVarified = _isFatherNameValidate,
            };

            CandidateValidateResponse response = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);

            if (IsValid)
            {
                await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, ActivityLog.APNTVERIFICATIONCOMPLETE);
            }
            return response;
        }

        public async Task<CandidateValidateResponse> VerifyUanData1(UanValidationRequest reqObj)
        {
            bool IsValid = false;
            List<ReasonRemarks> ReasonList = new();

            AppointeeDetailsResponse? appointeedetail = await _candidateContext.GetAppointeeDetailsAsync(reqObj.AppointeeId);

            if (!reqObj.IsUanActive)
            {
                ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.INACTIVE, Inputdata = string.Empty, Fetcheddata = string.Empty });
            }
            else if (appointeedetail.AppointeeDetailsId != null && reqObj?.PassbookDetails != null)
            {
                // 1️⃣ Verify Father's Name Separately
                bool _isFatherNameValid = VerifyFatherName(appointeedetail.MemberName, reqObj?.PassbookDetails?.FathersName, ReasonList);

                // 2️⃣ Perform Other UAN Verification
                IsValid = VerifyUanDetails(appointeedetail, reqObj, _isFatherNameValid, ReasonList);

                // 3️⃣ Log Activity
                string _activityStatus = IsValid ? ActivityLog.UANVERIFICATIONCMPLTE : ActivityLog.UANDATAVERIFICATIONFAILED;
                await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, _activityStatus);
            }

            // 4️⃣ Prepare Candidate Validation Data
            CandidateValidateUpdatedDataRequest candidateUpdatedDataReq = PrepareCandidateUpdatedDataRequest(reqObj, appointeedetail, IsValid, ReasonList);

            CandidateValidateResponse response = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);

            if (IsValid)
            {
                await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, ActivityLog.APNTVERIFICATIONCOMPLETE);
            }
            return response;
        }

        // ✅ 1️⃣ Separate Method for Father's Name Verification
        private bool VerifyFatherName(string? appointeeFatherName, string? uanFatherName, List<ReasonRemarks> reasonList)
        {
            bool isValid = !string.IsNullOrEmpty(appointeeFatherName) && appointeeFatherName.ToUpper() == uanFatherName?.ToUpper();
            if (!isValid)
            {
                reasonList.Add(new ReasonRemarks()
                {
                    ReasonCode = ReasonCode.CAREOFNAME,
                    Inputdata = appointeeFatherName,
                    Fetcheddata = uanFatherName
                });
            }
            return isValid;
        }

        // ✅ 2️⃣ Separate Method for UAN Verification
        private bool VerifyUanDetails(AppointeeDetailsResponse appointeeDetails, UanValidationRequest reqObj, bool isFatherNameValid, List<ReasonRemarks> reasonList)
        {
            string? AppointeeFullName = string.IsNullOrEmpty(appointeeDetails?.NameFromAadhaar) ? appointeeDetails?.AppointeeName?.Trim() : appointeeDetails?.NameFromAadhaar?.Trim();
            string? UanFullName = reqObj?.PassbookDetails?.Name;
            string? UanDob = reqObj?.PassbookDetails?.DateOfBirth;
            DateTime _inptdob = Convert.ToDateTime(UanDob);

            bool isPensionGapIdentified = HasEpsGap(reqObj?.PassbookDetails?.EpsContributionDetails);

            if (AppointeeFullName?.ToUpper() == UanFullName?.ToUpper()
                && appointeeDetails?.DateOfBirth == _inptdob
                && isFatherNameValid
                && !isPensionGapIdentified)
            {
                return true; // ✅ UAN Verification Passed
            }

            // ❌ Add Reasons for Failed Verification
            if (AppointeeFullName?.ToUpper() != UanFullName?.ToUpper())
            {
                reasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.NAME, Inputdata = AppointeeFullName, Fetcheddata = UanFullName });
            }
            if (appointeeDetails?.DateOfBirth != _inptdob)
            {
                reasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.DOB, Inputdata = appointeeDetails?.DateOfBirth?.ToShortDateString(), Fetcheddata = UanDob });
            }
            if (isPensionGapIdentified)
            {
                reasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.PENSIONGAPFIND, Inputdata = string.Empty, Fetcheddata = string.Empty });
            }

            return false; // ❌ UAN Verification Failed
        }

        // ✅ 3️⃣ Method to Prepare Candidate Data for Update
        private CandidateValidateUpdatedDataRequest PrepareCandidateUpdatedDataRequest(UanValidationRequest reqObj, AppointeeDetailsResponse appointeeDetails, bool isValid, List<ReasonRemarks> reasonList)
        {
            return new CandidateValidateUpdatedDataRequest()
            {
                AppointeeId = reqObj.AppointeeId,
                EmailId = appointeeDetails?.AppointeeEmailId,
                Status = isValid,
                Reasons = reasonList,
                UserId = reqObj.UserId,
                UserName = appointeeDetails?.AppointeeName,
                Type = RemarksType.UAN,
                uanData = new UanData
                {
                    IsPassbookFetch = reqObj.IsPassbookFetch,
                    IsPensionApplicable = reqObj?.PassbookDetails?.IsPensionApplicable ?? false,
                    IsPensionGap = HasEpsGap(reqObj?.PassbookDetails?.EpsContributionDetails),
                    UanNumber = string.IsNullOrEmpty(reqObj?.PassbookDetails?.PfUan) ? appointeeDetails?.UANNumber : reqObj?.PassbookDetails?.PfUan,
                    IsEmployementVarified = isValid,
                    //IsFNameVarified = reasonList.All(r => r.ReasonCode != ReasonCode.CAREOFNAME),
                    IsDualEmployementIdentified = reqObj?.PassbookDetails?.IsDualEmployement ?? false
                }
            };
        }

        public async Task PostActivity(int appointeeId, int userId, string activityCode)
        {
            await _activityContext.PostActivityDetails(appointeeId, userId, activityCode);
        }

        public async Task<GetUanResponse> GetUanNumberPriorityBase(GetUanNumberDetailsRequest reqObj)
        {
            GetUanResponse response = new();
            int priority = 1;
            bool isSuccess = false;

            // Fetch providers based on priority, starting with the highest priority
            while (!isSuccess)
            {
                var apiProvider = await _masterContext.GetApiProviderDataPriorityBase(ApiType.UAN, priority);

                if (string.IsNullOrEmpty(apiProvider))
                {
                    break; // No more providers available, exit the loop
                }

                //try
                //{
                if (apiProvider?.ToLower() == ApiProviderType.SurePass)
                {
                    response = await GetAadharToUan(reqObj);
                }
                else if (apiProvider?.ToLower() == ApiProviderType.Karza)
                {
                    response = await GetMobileToUan(reqObj);
                }
                else if (apiProvider?.ToLower() == ApiProviderType.Signzy)
                {
                    response = await GetMobilePanToUan(reqObj);
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    isSuccess = true; // Mark success and exit the loop
                }
                if (response.StatusCode == HttpStatusCode.ServiceUnavailable || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    priority++;  // Increase priority to try the next provider
                }
                else
                {
                    break;
                }
            }

            if (!isSuccess)
            {
                throw new Exception("All API providers failed to return a valid response.");
            }

            return response;
        }

        public bool HasEpsGap(List<EpsContributionSummary> epsResults)
        {
            return epsResults?.Any(result => result.EpsGapfind) ?? false;
        }

        public bool HasEpsContribution(List<EpsContributionSummary> epsResults)
        {
            return epsResults?.Any(result => result.HasEpsContribution) ?? false;
        }

        public async Task<GetAadharMobileLinkDetails> GetAadharMobileLinkStatus(string aadharNo, string mobileNo, int userId)
        {
            GetAadharMobileLinkDetails response = new();
            GetAadharMobileLinkDetails _apiResponse = new();
            var apiProvider = await _masterContext.GetApiProviderData(ApiType.Adhaar);
            if (apiProvider?.ToLower() == ApiProviderType.Karza)
            {
                _apiResponse = await _karzaApiContext.GetMobileAadharLinkStatus(aadharNo, mobileNo, userId);
            }

            response.StatusCode = _apiResponse.StatusCode;
            if (_apiResponse.StatusCode != HttpStatusCode.OK)
            {
                response.UserMessage = GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase?.ToString(), "UIDAI (Aadhar)");
                response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString() ?? string.Empty;
            }
            else
            {
                response = _apiResponse;
            }
            return response;
        }

        public async Task<AppointeePassportValidateResponse> PassportDetailsValidationPriorityBase(AppointeePassportValidateRequest reqObj)
        {
            PassportDetails _apiResponse = new();
            AppointeePassportValidateResponse Response = new();
            int priority = 1;  // Start with highest priority
            bool isSuccess = false;

            //var apiProvider = await _masterContext.GetApiProviderData(ApiType.Passport);
            await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, ActivityLog.PASPRTVERIFICATIONSTART);

            while (!isSuccess)
            {
                var apiProvider = await _masterContext.GetApiProviderDataPriorityBase(ApiType.Passport, priority);

                if (string.IsNullOrEmpty(apiProvider))
                {
                    break;  // Exit loop if no more providers
                }
                if (apiProvider?.ToLower() == ApiProviderType.Karza)
                {
                    _apiResponse = await _karzaApiContext.GetPassportDetails(reqObj);
                }
                if (apiProvider?.ToLower() == ApiProviderType.SurePass)
                {
                    _apiResponse = await _surepassApiContext.GetPassportDetails(reqObj);
                }
                if (apiProvider?.ToLower() == ApiProviderType.Signzy)
                {
                    _apiResponse = await _signzyApiContext.GetPassportDetails(reqObj);
                    if (_apiResponse.StatusCode == HttpStatusCode.BadRequest)
                    {
                        _apiResponse.ReasonPhrase = "Invalid Passport information";
                    }
                }

                // Check if the call was successful
                if (_apiResponse.StatusCode == HttpStatusCode.OK)
                {
                    isSuccess = true; // Mark success and exit the loop
                }
                if (_apiResponse.StatusCode == HttpStatusCode.ServiceUnavailable || _apiResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    priority++;  // Increase priority to try the next provider
                }
                else
                {
                    break;
                }
            }

            // Post-process response once successful or log failure
            if (isSuccess)
            {
                CandidateValidateResponse verifyResponse = await VarifyPassportData(_apiResponse, reqObj.appointeeId);
                Response.StatusCode = _apiResponse.StatusCode;
                Response.IsValid = verifyResponse.IsValid;
                Response.Remarks = verifyResponse.Remarks;

                string activityState = Response?.IsValid ?? false ? ActivityLog.PASPRTVERIFICATIONCMPLTE : ActivityLog.PASPRTDATAVERIFICATIONFAILED;
                await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, activityState);
            }
            else
            {
                await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, ActivityLog.PASPRTVERIFIFAILED);
                Response.IsValid = false;
                Response.Remarks = GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase, "PASSPORT");
            }

            return Response;
        }

        public async Task<TResponse> ValidateDetailsPriorityBase<TRequest, TResponse>(TRequest reqObj, string apiType)
            // where TResponse : new()
            where TResponse : BaseApiResponse, new()
            where TRequest : class
        {
            TResponse response = new TResponse();
            int priority = 1;
            bool isSuccess = false;

            switch (apiType.ToUpper())
            {
                case ApiType.Pan:
                    var panRequest = reqObj as AppointeePanValidateRequest;
                    if (panRequest != null)
                        await _activityContext.PostActivityDetails(panRequest.appointeeId, panRequest.userId, ActivityLog.PANVERIFICATIONSTART);
                    break;

                case ApiType.UAN:
                    var uanRequest = reqObj as GetUanNumberDetailsRequest;
                    if (uanRequest != null)
                        await _activityContext.PostActivityDetails(uanRequest.appointeeId, uanRequest.userId, ActivityLog.UANVERIFICATIONSTART);
                    break;

                case ApiType.Passport:
                    var passportRequest = reqObj as AppointeePassportValidateRequest;
                    if (passportRequest != null)
                        await _activityContext.PostActivityDetails(passportRequest.appointeeId, passportRequest.userId, ActivityLog.PASPRTVERIFICATIONSTART);
                    break;
            }
            // Fetch providers and process based on priority
            while (!isSuccess)
            {
                var apiProvider = await _masterContext.GetApiProviderDataPriorityBase(apiType, priority);
                if (string.IsNullOrEmpty(apiProvider))
                {
                    break;
                }
                try
                {
                    response = await ProviderHandler<TRequest, TResponse>(
                        apiProvider.ToLower(),
                        reqObj,
                        apiType
                    );
                    //if (response is BaseApiResponse baseApiResponse &&
                    //    baseApiResponse.StatusCode == HttpStatusCode.OK)
                    //{
                    //    isSuccess = true; // Exit the loop if successful
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error calling provider {apiProvider}: {ex.Message}");
                }

                // Check if the call was successful
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    isSuccess = true;
                }
                if (response.StatusCode == HttpStatusCode.ServiceUnavailable || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    priority++;
                }
                else
                {
                    break;
                }
            }

            // Post-process response once successful or log failure

            switch (apiType.ToUpper())
            {
                case ApiType.Pan:
                    var panRequest = reqObj as AppointeePanValidateRequest;
                    var _panResponse = response as AppointeePanValidateResponse;
                    if (isSuccess)
                    {
                        var _apiPanResponse = _panResponse.PanDetails;

                        CandidateValidateResponse verifyResponse = await VarifyPanData(_apiPanResponse, panRequest.appointeeId, panRequest.panName);
                        _panResponse.StatusCode = _apiPanResponse.StatusCode;
                        _panResponse.IsValid = verifyResponse.IsValid;
                        _panResponse.Remarks = verifyResponse.Remarks;
                        _panResponse.IsUanFetchCall = false;
                        string activityState = _panResponse?.IsValid ?? false ? ActivityLog.PANVERIFICATIONCMPLTE : ActivityLog.PANDATAVERIFICATIONFAILED;
                        await _activityContext.PostActivityDetails(panRequest.appointeeId, panRequest.userId, activityState);
                    }
                    else
                    {
                        await _activityContext.PostActivityDetails(panRequest.appointeeId, panRequest.userId, ActivityLog.PANVERIFIFAILED);
                        _panResponse.IsValid = false;
                        _panResponse.Remarks = GenarateErrorMsg((int)_panResponse.StatusCode, _panResponse?.ReasonPhrase, "PAN");
                    }

                    break;
                //case ApiType.UAN:
                //    var uanRequest = reqObj as GetUanNumberDetailsRequest;
                //    if (uanRequest == null)
                //        await _activityContext.PostActivityDetails(uanRequest.appointeeId, uanRequest.userId, ActivityLog.UANVERIFICATIONSTART);
                //    break;
                case ApiType.Passport:
                    var passportRequest = reqObj as AppointeePassportValidateRequest;
                    var Response = response as AppointeePassportValidateResponse;
                    // Post-process response once successful or log failure
                    if (isSuccess)
                    {
                        var _apiResponse = Response.passportDetails;

                        CandidateValidateResponse verifyResponse = await VarifyPassportData(_apiResponse, passportRequest.appointeeId);
                        Response.StatusCode = _apiResponse.StatusCode;
                        Response.IsValid = verifyResponse.IsValid;
                        Response.Remarks = verifyResponse.Remarks;

                        string activityState = Response?.IsValid ?? false ? ActivityLog.PASPRTVERIFICATIONCMPLTE : ActivityLog.PASPRTDATAVERIFICATIONFAILED;
                        await _activityContext.PostActivityDetails(passportRequest.appointeeId, passportRequest.userId, activityState);
                    }
                    else
                    {
                        await _activityContext.PostActivityDetails(passportRequest.appointeeId, passportRequest.userId, ActivityLog.PASPRTVERIFIFAILED);
                        Response.IsValid = false;
                        Response.Remarks = GenarateErrorMsg((int)Response.StatusCode, Response?.ReasonPhrase, "PASSPORT");
                    }
                    break;
            }
            //  return Response;
            // If no success, consider throwing an exception or handling accordingly
            //if (!isSuccess)
            //{
            //    throw new Exception("All API providers failed to return a valid response.");
            //}
            return response;
        }

        public async Task<TResponse> ProviderHandler<TRequest, TResponse>(
            string provider,
            TRequest request,
            string apiType)
            where TResponse : BaseApiResponse, new()
            where TRequest : class
        {
            switch (apiType.ToUpper())
            {
                case ApiType.Pan:
                    var panRequest = request as AppointeePanValidateRequest;
                    if (panRequest == null)
                        throw new Exception("Invalid request type for PAN API");

                    // Call the appropriate provider method based on the provider
                    if (provider == ApiProviderType.Karza.ToLower())
                    {
                        var panDetails = await _karzaApiContext.GetPanDetails(panRequest.panNummber, panRequest.userId);
                        return MapDetailsToResponse<TResponse>(panDetails);
                    }
                    else if (provider == ApiProviderType.SurePass.ToLower())
                    {
                        var panDetails = await _surepassApiContext.GetPanDetails(panRequest.panNummber, panRequest.userId);
                        return MapDetailsToResponse<TResponse>(panDetails);
                    }
                    else if (provider == ApiProviderType.Signzy.ToLower())
                    {
                        var panDetails = await _signzyApiContext.GetPanDetails(panRequest.panNummber, panRequest.userId);
                        return MapDetailsToResponse<TResponse>(panDetails);
                    }
                    break;
                    throw new Exception($"Unsupported provider or API type: {provider}, {apiType}");
                case ApiType.UAN:
                    var uanRequest = request as GetUanNumberDetailsRequest;
                    if (uanRequest == null)
                        throw new Exception("Invalid request type for PAN API");

                    // Call the appropriate provider method based on the provider
                    if (provider?.ToLower() == ApiProviderType.SurePass)
                    {
                        var res = await GetAadharToUan(uanRequest);
                        return MapDetailsToResponse<TResponse>(res);
                    }
                    else if (provider?.ToLower() == ApiProviderType.Karza)
                    {
                        var res = await GetMobileToUan(uanRequest);
                        return MapDetailsToResponse<TResponse>(res);
                    }
                    else if (provider?.ToLower() == ApiProviderType.Signzy)
                    {
                        var res = await GetMobilePanToUan(uanRequest);
                        return MapDetailsToResponse<TResponse>(res);
                    }
                    break;

                case ApiType.Passport:
                    var passportRequest = request as AppointeePassportValidateRequest;
                    if (string.IsNullOrEmpty(provider))
                    {
                        break;  // Exit loop if no more providers
                    }
                    if (provider?.ToLower() == ApiProviderType.Karza)
                    {
                        var result = await _karzaApiContext.GetPassportDetails(passportRequest);
                        return MapDetailsToResponse<TResponse>(result);
                    }
                    if (provider?.ToLower() == ApiProviderType.SurePass)
                    {
                        var result = await _surepassApiContext.GetPassportDetails(passportRequest);
                        return MapDetailsToResponse<TResponse>(result);
                    }
                    if (provider?.ToLower() == ApiProviderType.Signzy)
                    {
                        var result = await _signzyApiContext.GetPassportDetails(passportRequest);
                        if (result.StatusCode == HttpStatusCode.BadRequest)
                        {
                            result.ReasonPhrase = "Invalid Passport information";
                        }
                        return MapDetailsToResponse<TResponse>(result);
                    }
                    break;
            }

            throw new Exception($"Unsupported provider or API type: {provider}, {apiType}");
        }

        private TResponse MapDetailsToResponse<TResponse>(object details) where TResponse : BaseApiResponse, new()
        {
            var response = new TResponse();

            if (details is PanDetails panDetails)
            {
                response.StatusCode = panDetails.StatusCode;
                response.ReasonPhrase = panDetails.ReasonPhrase;
                if (response is AppointeePanValidateResponse appointeeResponse)
                {
                    appointeeResponse.PanDetails = panDetails; // Set the PanDetails property
                }
            }
            //else if (details is UanDetails uanDetails)
            //{
            //    response.UanNumber = uanDetails.UanNumber;
            //    // Map other properties from uanDetails to uanResponse
            //}
            else if (details is PassportDetails passportDetails)
            {
                response.StatusCode = passportDetails.StatusCode;
                response.ReasonPhrase = passportDetails.ReasonPhrase;
                if (response is AppointeePassportValidateResponse appointeeResponse)
                {
                    appointeeResponse.passportDetails = passportDetails; // Set the PanDetails property
                }
            }
            else
            {
                throw new Exception("Unsupported details type for mapping.");
            }

            //response.StatusCode = HttpStatusCode.OK; // Set status code as needed
            return response;
        }

        public async Task<AppointeeBankValidateResponse> BankDetailsValidation(AppointeeBankValidateRequest reqObj)
        {
            BankDetails _apiResponse = new();
            AppointeeBankValidateResponse Response = new();
            int priority = 1;  // Start with highest priority
            bool isSuccess = false;

            await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, ActivityLog.BANKVERIFICATIONSTART);
            AppointeeDetailsResponse? appointeedetail = await _candidateContext.GetAppointeeDetailsAsync(reqObj.AppointeeId);
            // Attempt provider calls based on priority

            while (!isSuccess)
            {
                var apiProvider = await _masterContext.GetApiProviderDataPriorityBase(ApiType.Bank, priority);

                if (string.IsNullOrEmpty(apiProvider))
                {
                    break;  // Exit loop if no more providers
                }

                if (apiProvider?.ToLower() == ApiProviderType.Karza)
                {
                    _apiResponse = await _karzaApiContext.GetBackAccountDetails(reqObj.AccountNumber, reqObj.Ifsc, reqObj.UserId);
                }
                else if (apiProvider?.ToLower() == ApiProviderType.Signzy)
                {
                    _apiResponse = await _signzyApiContext.GetBankDetails(reqObj.AccountNumber, reqObj.Ifsc, appointeedetail?.AppointeeName, appointeedetail?.MobileNo, reqObj.UserId);
                }

                // Check if the call was successful
                if (_apiResponse.StatusCode == HttpStatusCode.OK)
                {
                    isSuccess = true; // Mark success and exit the loop
                }
                if (_apiResponse.StatusCode == HttpStatusCode.ServiceUnavailable || _apiResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    priority++;  // Increase priority to try the next provider
                }
                else
                {
                    break;
                }
            }
            // Post-process response once successful or log failure
            if (isSuccess)
            {
                CandidateValidateResponse verifyResponse = await VarifyBanKData(_apiResponse, appointeedetail, reqObj.AppointeeId, reqObj.UserId);
                Response.StatusCode = _apiResponse.StatusCode;
                Response.IsValid = verifyResponse.IsValid;
                Response.Remarks = verifyResponse.Remarks;
                string activityState = Response?.IsValid ?? false ? ActivityLog.BANKVERIFICATIONCMPLTE : ActivityLog.BANKDATAVERIFICATIONFAILED;
                await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, activityState);
            }
            else
            {
                await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, ActivityLog.BANKVERIFIFAILED);
                Response.StatusCode = _apiResponse.StatusCode;
                Response.IsValid = false;
                Response.Remarks = GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase, "BANK");
            }

            return Response;
        }

        private async Task<CandidateValidateResponse> VarifyBanKData(BankDetails request, AppointeeDetailsResponse appointeedetail, int appointeeId, int userId)
        {
            bool IsValid = false;
            _ = new CandidateValidateResponse();
            List<ReasonRemarks> ReasonList = new();
            CandidateValidateUpdatedDataRequest candidateUpdatedDataReq = new();
            //AppointeeDetailsResponse? appointeedetail = await _candidateContext.GetAppointeeDetailsAsync(appointeeId);

            string? accountHolderName = request?.AccountHolderName?.Trim();
            string? accNumber = request?.AccountNo?.Trim();
            string? ifscNo = request?.IFSCCode?.Trim();
            string normalizedName = CommonUtility.NormalizeWhitespace(appointeedetail?.AppointeeName?.Trim());

            string TrimmedAccountHolderName = RemoveTitle(accountHolderName);
            accountHolderName = Regex.Replace(TrimmedAccountHolderName, @"\s+", " ");
            string normalizedBankFullName = CommonUtility.NormalizeWhitespace(accountHolderName?.Trim());
            if (appointeedetail.AppointeeDetailsId != null && request != null)
            {
                if (string.Equals(normalizedName, normalizedBankFullName, StringComparison.OrdinalIgnoreCase))
                //&& (appointeedetail?.DateOfBirth == _inptdob || dOBVarified))
                {
                    IsValid = true;
                }
                else
                {
                    if (!string.Equals(normalizedName, normalizedBankFullName, StringComparison.OrdinalIgnoreCase))
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.UPLOADEDNAME, Inputdata = appointeedetail?.AppointeeName, Fetcheddata = normalizedBankFullName });
                    }
                }

                candidateUpdatedDataReq = new CandidateValidateUpdatedDataRequest()
                {
                    AppointeeId = appointeeId,
                    EmailId = appointeedetail?.AppointeeEmailId ?? string.Empty,
                    Status = IsValid,
                    Reasons = ReasonList,
                    UserId = appointeedetail.UserId,
                    UserName = appointeedetail.AppointeeName,
                    Type = RemarksType.Bank,
                    BankDetails = request,
                    step = 3
                    //PanNumber = panNumber,
                };
            }
            CandidateValidateResponse response = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);

            return response;
        }

        private static string RemoveTitle(string name)
        {
            // Remove common prefixes and trim extra spaces
            string cleanedName = Regex.Replace(name, @"^(Mrs\.?|Mr\.?|Ms\.?|Dr\.?|Shri\.?)\s*", "", RegexOptions.IgnoreCase);
            return cleanedName.Trim(); // Ensure no leading/trailing spaces
        }

        public async Task<AppointeeFirDetailsResponse> FIRDetailsValidation(AppointeeFirValidateRequest reqObj)
        {
            FirDetails _apiResponse = new();
            AppointeeFirDetailsResponse Response = new();
            int priority = 1;  // Start with highest priority
            bool isSuccess = false;

            await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, ActivityLog.FIRVERIFICATIONSTART);
            AppointeeDetailsResponse? appointeedetail = await _candidateContext.GetAppointeeDetailsAsync(reqObj.AppointeeId);

            while (!isSuccess)
            {
                // Attempt provider calls based on priority
                var apiProvider = await _masterContext.GetApiProviderDataPriorityBase(ApiType.Police, priority);
                if (apiProvider?.ToLower() == ApiProviderType.Karza)
                {
                    _apiResponse = await _karzaApiContext.GetFirDetails(appointeedetail.AppointeeName, appointeedetail.DateOfBirth, appointeedetail.MobileNo, reqObj.UserId);
                }
                if (apiProvider?.ToLower() == ApiProviderType.Signzy)
                {
                    var fathersName = appointeedetail.MemberRelation?.Trim() == "F" ? appointeedetail.MemberName : string.Empty;
                    var _searchResponse = await _signzyApiContext.GetFirStatusDetails(appointeedetail.AppointeeName, fathersName, reqObj.UserId);
                    if (_searchResponse.StatusCode == HttpStatusCode.OK)
                    {
                        if (!string.IsNullOrEmpty(_searchResponse.SearchId))
                        {
                            _apiResponse = await _signzyApiContext.GetFirDetails(_searchResponse.SearchId, reqObj.UserId);
                        }
                        else
                        {
                            _apiResponse.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        _apiResponse.StatusCode = _searchResponse.StatusCode;
                        _apiResponse.ReasonPhrase = _searchResponse.ReasonPhrase;
                    }
                }

                if (_apiResponse.StatusCode == HttpStatusCode.OK)
                {
                    isSuccess = true; // Mark success and exit the loop
                }
                if (_apiResponse.StatusCode == HttpStatusCode.ServiceUnavailable || _apiResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    priority++;  // Increase priority to try the next provider
                }
                else
                {
                    break;
                }
            }
            if (isSuccess)
            {
                CandidateValidateResponse verifyResponse = await UpdateFirDetails(appointeedetail, _apiResponse, reqObj.UserId);
                Response.StatusCode = _apiResponse.StatusCode;
                Response.IsValid = verifyResponse.IsValid;
                Response.Remarks = verifyResponse.Remarks;
                Response.PoliceFirDetails = _apiResponse.PoliceFirDetails;
                string activityState = Response?.IsValid ?? false ? ActivityLog.FIRVERIFICATIONCMPLTE : ActivityLog.FIRDATAVERIFICATIONFAILED;
                await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, activityState);
            }
            else
            {
                await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, ActivityLog.FIRVERIFIFAILED);
                Response.StatusCode = _apiResponse.StatusCode;
                Response.IsValid = false;
                Response.Remarks = GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase, "FIR");
            }
            return Response;
        }

        private async Task<CandidateValidateResponse> UpdateFirDetails(AppointeeDetailsResponse appntDetails, FirDetails? firDetails, int userId)
        {
            bool IsValid = true;
            List<ReasonRemarks> ReasonList = new();
            CandidateValidateUpdatedDataRequest candidateUpdatedDataReq = new();
            string _firDetails = string.Empty;

            if (appntDetails.AppointeeDetailsId != null && firDetails?.PoliceFirDetails?.Count > 0)
            {
                IsValid = false;
                _firDetails = JsonConvert.SerializeObject(_firDetails, Newtonsoft.Json.Formatting.Indented);

                ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.FIR, Inputdata = appntDetails?.AppointeeName, Fetcheddata = "" });
            }
            candidateUpdatedDataReq = new CandidateValidateUpdatedDataRequest()
            {
                AppointeeId = appntDetails.AppointeeId ?? 0,
                EmailId = appntDetails?.AppointeeEmailId ?? string.Empty,
                Status = IsValid,
                Reasons = ReasonList,
                UserId = appntDetails.UserId,
                UserName = appntDetails.AppointeeName,
                Type = RemarksType.Police,
                FirDetails = _firDetails,
                step = 4
                //PanNumber = panNumber,
            };
            CandidateValidateResponse response = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);

            return response;
        }

        public async Task<AppointeeDLValidateResponse> DrivingLicenseValidation(AppointeeDLValidateRequest reqObj)
        {
            DrivingLicenseDetails _apiResponse = new();
            AppointeeDLValidateResponse Response = new();
            int priority = 1;  // Start with highest priority
            bool isSuccess = false;
            AppointeeDetailsResponse? appointeedetail = await _candidateContext.GetAppointeeDetailsAsync(reqObj.AppointeeId);
            var _inptdob = appointeedetail.DateOfBirth ?? new DateTime();
            await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, ActivityLog.DLVERIFICATIONSTART);
            //AppointeeDetailsResponse? appointeedetail = await _candidateContext.GetAppointeeDetailsAsync(reqObj.AppointeeId);

            while (!isSuccess)
            {
                // Attempt provider calls based on priority
                var apiProvider = await _masterContext.GetApiProviderDataPriorityBase(ApiType.Driving, priority);

                //if (string.IsNullOrEmpty(apiProvider))
                //{
                //    break;  // Exit loop if no more providers
                //}

                if (apiProvider?.ToLower() == ApiProviderType.Karza)
                {
                    _apiResponse = await _karzaApiContext.GetDrivingLicenseDetails(reqObj.DLNumber, _inptdob, reqObj.UserId);
                }
                else
                if (apiProvider?.ToLower() == ApiProviderType.Signzy)
                {
                    _apiResponse = await _signzyApiContext.GetDrivingLicenseDetails(reqObj.DLNumber, _inptdob, reqObj.UserId);
                }

                // Check if the call was successful
                if (_apiResponse.StatusCode == HttpStatusCode.OK)
                {
                    isSuccess = true; // Mark success and exit the loop
                }
                if (_apiResponse.StatusCode == HttpStatusCode.ServiceUnavailable || _apiResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    priority++;  // Increase priority to try the next provider
                }
                else
                {
                    break;
                }
            }
            // Post-process response once successful or log failure
            if (isSuccess)
            {
                CandidateValidateResponse verifyResponse = await VarifyDLData(_apiResponse, appointeedetail, reqObj.AppointeeId, reqObj.UserId);
                Response.StatusCode = _apiResponse.StatusCode;
                Response.IsValid = verifyResponse.IsValid;
                Response.Remarks = verifyResponse.Remarks;
                string activityState = Response?.IsValid ?? false ? ActivityLog.DLVERIFICATIONCMPLTE : ActivityLog.DLDATAVERIFICATIONFAILED;
                await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, activityState);
            }
            else
            {
                await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, ActivityLog.DLVERIFIFAILED);
                Response.StatusCode = _apiResponse.StatusCode;
                Response.IsValid = false;
                Response.Remarks = GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase, "Deiving License");
            }

            return Response;
        }

        private async Task<CandidateValidateResponse> VarifyDLData(DrivingLicenseDetails request, AppointeeDetailsResponse appointeedetail, int appointeeId, int userId)
        {
            bool IsValid = false;
            _ = new CandidateValidateResponse();
            List<ReasonRemarks> ReasonList = new();
            CandidateValidateUpdatedDataRequest candidateUpdatedDataReq = new();

            string? dLName = request?.Name?.Trim();
            string? dLNumberStatus = CommonUtility.NormalizeWhitespace(request?.LicenseStatus?.Trim()?.ToUpperInvariant());
            string? dob = request?.Dob?.Trim();
            string? fatherOrHusbandName = CommonUtility.NormalizeWhitespace(request?.FatherOrHusbandName?.Trim());
            string? dbFatherOrHusbandName = CommonUtility.NormalizeWhitespace(appointeedetail?.MemberName?.Trim());
            string normalizedName = CommonUtility.NormalizeWhitespace(appointeedetail?.AppointeeName?.Trim());
            string normalizedFullName = CommonUtility.NormalizeWhitespace(dLName?.Trim());

            if (appointeedetail.AppointeeDetailsId != null && request != null)
            {
                if (string.Equals(normalizedName, normalizedFullName, StringComparison.OrdinalIgnoreCase))
                //&& string.Equals(dLNumberStatus, "ACTIVE", StringComparison.OrdinalIgnoreCase))
                //&& (appointeedetail?.DateOfBirth == _inptdob || dOBVarified))
                {
                    IsValid = true;
                }
                else
                {
                    if (!string.Equals(normalizedName, normalizedFullName, StringComparison.OrdinalIgnoreCase))
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.UPLOADEDNAME, Inputdata = appointeedetail?.AppointeeName, Fetcheddata = normalizedFullName });
                    }
                    //    if (!string.Equals(dLNumberStatus, "ACTIVE", StringComparison.OrdinalIgnoreCase))
                    //    {
                    //        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.INACTIVE, Inputdata = appointeedetail?.AppointeeName, Fetcheddata = normalizedFullName });
                    //    }
                }
                bool _isFatherNameValidate = !string.IsNullOrEmpty(fatherOrHusbandName) && (string.Equals(fatherOrHusbandName, dbFatherOrHusbandName, StringComparison.OrdinalIgnoreCase));
                if (!_isFatherNameValidate && !string.IsNullOrEmpty(fatherOrHusbandName))
                {
                    ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.CAREOFNAME, Inputdata = dbFatherOrHusbandName, Fetcheddata = fatherOrHusbandName });
                }
                candidateUpdatedDataReq = new CandidateValidateUpdatedDataRequest()
                {
                    AppointeeId = appointeeId,
                    EmailId = appointeedetail?.AppointeeEmailId ?? string.Empty,
                    Status = IsValid,
                    Reasons = ReasonList,
                    UserId = appointeedetail.UserId,
                    UserName = appointeedetail.AppointeeName,
                    Type = RemarksType.DRLNC,
                    DlNumber = request.LicenseStatus,
                    HasData = true,
                    IsFNameVarified = _isFatherNameValidate,
                    step = 2
                    //BankDetails = request,
                    //PanNumber = panNumber,
                };
            }
            CandidateValidateResponse response = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);

            return response;
        }

        public async Task<DigilockerAccessDetails> GeneratetDigilockerUrl(GetDigilockerUrlRequest reqObj)
        {
            DigilockerAccessDetails Response = new();

            var apiProvider = await _masterContext.GetApiProviderData(ApiType.Adhaar);

            if (apiProvider?.ToLower() == ApiProviderType.Signzy)
            {
                Response = await _signzyApiContext.GetDigiLockerUrl(reqObj);
            }

            Response.StatusCode = Response.StatusCode;
            if (Response.StatusCode == HttpStatusCode.OK)
            {
                return Response;
            }
            else
            {
                if ((int)Response.StatusCode == (int)HttpStatusCode.UnprocessableEntity)
                {
                    await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, ActivityLog.ADHINVALID);
                    Response.UserMessage = GenarateErrorMsg((int)Response.StatusCode, Response?.ReasonPhrase?.ToString(), "UIDAI (Aadhar)");
                    Response.ReasonPhrase = Response?.ReasonPhrase?.ToString() ?? string.Empty;
                }

                return Response;
            }
        }

        public async Task<AadharSubmitOtpDetails> GetAadharDetailsFromXmlDigilocker(AppointeeDigilockerAadhaarVarifyRequest reqObj)
        {
            AadharSubmitOtpDetails response = new();
            //DigilockerAadhaarXmlDetails _apiResponse = new();

            var apiProvider = await _masterContext.GetApiProviderData(ApiType.Adhaar);

            if (apiProvider?.ToLower() == ApiProviderType.Signzy)
            {
                response = await _signzyApiContext.GetDigiLockerAadharDetails(reqObj);
            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response;
            }
            else
            {
                if ((int)response.StatusCode == (int)HttpStatusCode.UnprocessableEntity)
                {
                    await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, ActivityLog.ADHINVALID);
                    response.UserMessage = GenarateErrorMsg((int)response.StatusCode, response?.ReasonPhrase?.ToString(), "UIDAI (Aadhar)");
                    response.ReasonPhrase = response?.ReasonPhrase?.ToString() ?? string.Empty;
                }

                return response;
            }

        }
    }
}