using Newtonsoft.Json;
using System.Net;
using System.Xml;
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
using static VERIDATA.BLL.utility.CommonEnum;

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
                ? $"{type} {"server is currently busy! Please try again later"}"
                : $"{reasonCode}, {"Please try again later."}";
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

            if (appointeedetail.AppointeeDetailsId != null && request != null)
            {
                DateTime _inptdob = Convert.ToDateTime(panDOB);

                if (panName?.ToUpper() == panFullName?.Trim()?.ToUpper() &&
                    appointeedetail?.DateOfBirth == _inptdob)
                //&& (string.IsNullOrEmpty(phoneNo?.ToUpper()) || appointeedetail?.MobileNo?.ToUpper() == phoneNo?.ToUpper()))
                {
                    IsValid = true;
                    string maskedPhoneNumber = CommonUtility.MaskedString(phoneNo);
                    if (string.IsNullOrEmpty(phoneNo?.ToUpper()))
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.PANMOBILENOTAVAIL, Inputdata = appointeedetail?.MobileNo, Fetcheddata = maskedPhoneNumber });
                    }
                    if (appointeedetail?.AppointeeName?.Trim()?.ToUpper() != panFullName?.ToUpper())
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.UPLOADEDNAME, Inputdata = appointeedetail?.AppointeeName, Fetcheddata = panFullName });
                    }
                    if (appointeedetail?.MobileNo != phoneNo && !string.IsNullOrEmpty(phoneNo?.ToUpper()))
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.PANMOBILE, Inputdata = appointeedetail?.MobileNo, Fetcheddata = maskedPhoneNumber });
                    }
                }
                else
                {
                    if (appointeedetail?.AppointeeName?.Trim()?.ToUpper() != panFullName?.Trim()?.ToUpper())
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.UPLOADEDNAME, Inputdata = appointeedetail?.AppointeeName, Fetcheddata = panFullName });
                    }

                    //if (panName?.Trim()?.ToUpper() != panFullName?.Trim()?.ToUpper())
                    //{
                    //    ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.NAME, Inputdata = panName, Fetcheddata = panFullName });
                    //}

                    if (appointeedetail?.DateOfBirth != _inptdob)
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
            string passportName = request?.Name?.Trim();
            string? passportNo = request?.PassportNumber?.Trim();
            string? passportDOB = request?.DateOfBirth;
            //string? passportValidFrom = request?.ValidFrom;
            //string? passportValidTill = request?.ValidTill;

            if (appointeedetail.AppointeeDetailsId != null && request != null && !string.IsNullOrEmpty(passportName) && !string.IsNullOrEmpty(passportDOB))
            {
                DateTime _inptdob = Convert.ToDateTime(passportDOB);
                if (appointeedetail?.DateOfBirth == _inptdob && appointeedetail?.AppointeeName?.ToUpper() == passportName?.ToUpper())
                {
                    IsValid = true;
                }
                else
                {
                    if (appointeedetail?.DateOfBirth != _inptdob)
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.PASSPRTDOB, Inputdata = appointeedetail?.DateOfBirth?.ToShortDateString(), Fetcheddata = passportDOB });
                    }
                    if (appointeedetail?.AppointeeName?.ToUpper() != passportName?.ToUpper())
                    {
                        ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.PASSPRTNAME, Inputdata = appointeedetail?.AppointeeName, Fetcheddata = passportName });
                    }
                    //if (appointeedetail?.PassportValidFrom  != _inptValidFrom)
                    //{
                    //    ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.PASSPRTNAME, Inputdata = appointeedetail?.PassportValidFrom?.ToShortDateString(), Fetcheddata = passportValidFrom });
                    //}
                    //if (appointeedetail?.PassportValidTill  != _inptValidTill)
                    //{
                    //    ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.PASSPRTNAME, Inputdata = appointeedetail?.PassportValidTill?.ToShortDateString(), Fetcheddata = passportValidTill });
                    //}
                    //if (string.IsNullOrEmpty(passportNo))
                    //{
                    //    ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.INVDPASSPRT, Inputdata = appointeedetail?.PassportNo, Fetcheddata = passportDOB });

                    //}
                    //else
                    //    if (appointeedetail?.PassportNo != passportNo)
                    //{
                    //    ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.PASSPORTNO, Inputdata = appointeedetail?.PassportNo, Fetcheddata = passportDOB });
                    //}

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
                response.Remarks = "No data fetch from passport, please try again later";

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
                Response = await GetPanMobileToUan(reqObj);
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
                // await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, ActivityLog.PASPRTVERIFIFAILED);
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
                    //var VerifyResponse = await VerifyUanName(_apiResponse, reqObj.appointeeId, reqObj.userId);
                    //Response.IsVarified = ;
                    //Response.Remarks = string.IsNullOrEmpty(Response.Remarks) ? VerifyResponse.Remarks : $"{Response.Remarks}, {VerifyResponse.Remarks}";
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
                    Response.UserMessage = "Inactive UAN, Please activate your UAN from epfo site and try again.";
                    Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString() ?? string.Empty;
                }
                string activitystate = _apiResponse.IsUanAvailable ?? false ? ActivityLog.UANFETCH : ActivityLog.NOUAN;
                await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, activitystate);
            }
            else
            {
                // await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, ActivityLog.PASPRTVERIFIFAILED);
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

            if (_apiResponse.StatusCode == HttpStatusCode.OK)
            {
                Response.IsUanAvailable = _apiResponse.IsUanAvailable ?? false;
                Response.UanNumber = _apiResponse.UanNumber;
                Response.Remarks = _apiResponse.IsUanAvailable ?? false ? string.Empty : _apiResponse.ReasonPhrase;
                List<ReasonRemarks> ReasonList = new();

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
                        Reasons = ReasonList,
                        uanData = _uanDetails,
                        Type = RemarksType.UAN

                    };
                    _ = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);
                }
                if (_apiResponse.IsInactiveUan ?? false)
                {
                    ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.INACTIVE, Inputdata = string.Empty, Fetcheddata = string.Empty });
                    Response.StatusCode = HttpStatusCode.UnprocessableEntity;
                    Response.UserMessage = "Inactive UAN, Please activate your UAN from epfo site and try again.";
                    Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString() ?? string.Empty;
                }
                string activitystate = _apiResponse.IsUanAvailable ?? false ? ActivityLog.UANFETCH : ActivityLog.NOUAN;
                await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, activitystate);
            }
            else
            {
                // await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, ActivityLog.PASPRTVERIFIFAILED);
                Response.IsUanAvailable = false;
                Response.UserMessage = GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase, "EPFO");
            }
            return Response;
        }
        private async Task<GetUanResponse> GetMobilePanToUan(GetUanNumberDetailsRequest reqObj)
        {
            GetUanResponse Response = new();
            GetCandidateUanDetails _apiResponse = await _signzyApiContext.GetUanFromMobilenPan(reqObj.panNumber, reqObj.mobileNumber, reqObj.userId);
            Response.StatusCode = _apiResponse.StatusCode;
            if (_apiResponse.StatusCode == HttpStatusCode.OK || _apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                Response.StatusCode = HttpStatusCode.OK;
                Response.IsUanAvailable = _apiResponse.IsUanAvailable ?? false;
                Response.UanNumber = _apiResponse.UanNumber;
                Response.Remarks = !(_apiResponse.IsUanAvailable ?? false) ? "UAN Not available" : string.Empty;
                List<ReasonRemarks> ReasonList = new();

                if (_apiResponse.IsUanAvailable ?? false)
                {
                    UanData _uanDetails = new UanData
                    {
                        UanNumber = Response.UanNumber,
                    };
                    CandidateValidateUpdatedDataRequest candidateUpdatedDataReq = new()
                    {
                        AppointeeId = reqObj.appointeeId,
                        EmailId = string.Empty,
                        UserId = reqObj.userId,
                        UserName = string.Empty,
                        Reasons = ReasonList,
                        uanData = _uanDetails,
                        Type = RemarksType.UAN

                    };
                    _ = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);
                }
                if (_apiResponse.IsInactiveUan ?? false)
                {
                    ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.INACTIVE, Inputdata = string.Empty, Fetcheddata = string.Empty });
                    Response.StatusCode = HttpStatusCode.UnprocessableEntity;
                    Response.UserMessage = "Inactive UAN, Please activate your UAN from epfo site and try again.";
                    Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString() ?? string.Empty;
                }
                string activitystate = _apiResponse.IsUanAvailable ?? false ? ActivityLog.UANFETCH : ActivityLog.NOUAN;
                await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, activitystate);
            }
            else
            {
                var msg = _apiResponse.StatusCode switch
                {
                    HttpStatusCode.BadRequest => "Invalid mobile or PAN number.Please try again later.",
                    HttpStatusCode.Conflict => "server is currently busy! Please try again later",
                    _ => string.Empty
                };
                // await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, ActivityLog.PASPRTVERIFIFAILED);
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
                AadhaarName = reqObj?.aaddharName,
                AadhaarNumber = reqObj?.aaddharNumber,
                AadhaarNumberView = reqObj?.aaddharNumber,
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
            //if (apiProvider?.ToLower() == ApiProviderType.Karza)
            //{
            //    _apiResponse = await _karzaApiContext.GenerateAadharOTP(reqObj.aaddharNumber, reqObj.userId);
            //}
            if (apiProvider?.ToLower() == ApiProviderType.SurePass)
            {
                _apiResponse = await _surepassApiContext.GenerateAadharOTP(reqObj.aaddharNumber, reqObj.userId);
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
                    AadharSubmitOtpDetails _adhaardata = new() { AadharNumber = reqObj?.aaddharNumber };
                    VarifyReq.AadharDetails = _adhaardata;
                    VarifyReq.isValidAdhar = false;
                    VarifyReq.AppointeeId = reqObj.appointeeId;
                    VarifyReq.AppointeeAadhaarName = reqObj.aaddharName;
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
                AppointeeDetailsResponse appointeedetail = await _candidateContext.GetAppointeeDetailsAsync(reqObj.appointeeId);
                string? aadharNo = appointeedetail.AadhaarNumber;
                //_apiResponse = await _karzaApiContext.SubmitAadharOTP(reqObj.client_id, aadharNo, reqObj.otp, reqObj.userId);
            }
            if (apiProvider?.ToLower() == ApiProviderType.SurePass)
            {
                _apiResponse = await _surepassApiContext.SubmitAadharOTP(reqObj.client_id, reqObj.otp, reqObj.userId);
            }


            Response.StatusCode = _apiResponse.StatusCode;
            if (_apiResponse.StatusCode != HttpStatusCode.OK)
            {
                bool IsUnprocessableEntity = (int)_apiResponse.StatusCode == (int)HttpStatusCode.UnprocessableEntity;
                Response.UserMessage = IsUnprocessableEntity ? "Invalid OTP, Please retry" : GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase?.ToString(), "UIDAI (Aadhar)");
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

                    XmlNode personalInfoNode = personNode.SelectSingleNode("Poi");
                    XmlNode personalAddresNode = personNode.SelectSingleNode("Poa");
                    string referenceId = root.SelectSingleNode("@referenceId").Value;
                    string nameNode = personalInfoNode.SelectSingleNode("@name").Value;
                    string genderNode = personalInfoNode.SelectSingleNode("@gender").Value;
                    string dobNode = personalInfoNode.SelectSingleNode("@dob").Value;
                    string careofNode = personalAddresNode.SelectSingleNode("@careof").Value;
                    var lastFourDigit = new string(referenceId.Where(char.IsDigit).Take(4).ToArray());
                    response.Name = nameNode;
                    response.Dob = dobNode;
                    response.CareOf = careofNode;
                    response.Gender = genderNode;
                    response.AadharNumber = $"{"XXXXXXXX"}{lastFourDigit}";

                }
            }
            return response;
        }
        public async Task<CandidateValidateResponse> VerifyAadharData(AadharValidationRequest reqObj)
        {
            bool IsValid = false;
            _ = new CandidateValidateResponse();
            AadharDetailsData _aadharData = new();
            List<ReasonRemarks> ReasonList = new();
            _ = new CandidateValidateUpdatedDataRequest();
            AppointeeDetailsResponse? appointeedetail = await _candidateContext.GetAppointeeDetailsAsync(reqObj.AppointeeId);
            if (reqObj.isValidAdhar)
            {
                string? aadharName = reqObj?.AppointeeAadhaarName?.Trim();
                string? appointeeAadhaarFullName = reqObj?.AadharDetails?.Name;
                string? appointeeAadhaarGender = reqObj?.AadharDetails?.Gender;
                string? appointeeAadhaarDOB = reqObj?.AadharDetails?.Dob;
                string? appointeeCareOf = reqObj?.AadharDetails?.CareOf;

                _aadharData.AadhaarName = aadharName;
                _aadharData.AadhaarNumber = reqObj?.AadharDetails?.AadharNumber;
                _aadharData.NameFromAadhaar = appointeeAadhaarFullName;
                _aadharData.GenderFromAadhaar = appointeeAadhaarGender;
                _aadharData.DobFromAadhaar = appointeeAadhaarDOB;

                if (appointeedetail.AppointeeDetailsId != null && reqObj.AadharDetails != null)
                {
                    DateTime _inptdob = Convert.ToDateTime(appointeeAadhaarDOB);
                    bool hasCoName = !string.IsNullOrEmpty(appointeeCareOf);
                    _ = !hasCoName || appointeedetail?.MemberName?.ToUpper() == appointeeCareOf;

                    if (aadharName?.ToUpper() == appointeeAadhaarFullName?.ToUpper() &&
                        appointeedetail?.Gender?.ToUpper() == appointeeAadhaarGender?.ToUpper() && appointeedetail?.DateOfBirth == _inptdob)//&& validateCareOfName)

                    {

                        if (appointeedetail.AppointeeName?.Trim()?.ToUpper() != appointeeAadhaarFullName?.Trim()?.ToUpper())
                        {
                            ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.UPLOADEDNAME, Inputdata = appointeedetail.AppointeeName, Fetcheddata = appointeeAadhaarFullName });
                        }

                        IsValid = true;
                    }
                    else
                    {

                        //if (aadharName?.ToUpper() != appointeeAadhaarFullName?.ToUpper())
                        //{
                        //    ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.NAME, Inputdata = aadharName, Fetcheddata = appointeeAadhaarFullName });
                        //}
                        if (appointeedetail?.Gender?.ToUpper() != appointeeAadhaarGender?.ToUpper())
                        {
                            ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.GENDER, Inputdata = appointeedetail?.Gender, Fetcheddata = appointeeAadhaarGender });

                        }
                        if (appointeedetail?.DateOfBirth != _inptdob)
                        {
                            ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.DOB, Inputdata = appointeedetail?.DateOfBirth?.ToShortDateString(), Fetcheddata = appointeeAadhaarDOB });
                        }
                        if (appointeedetail.AppointeeName?.Trim()?.ToUpper() != appointeeAadhaarFullName?.Trim()?.ToUpper())
                        {
                            ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.UPLOADEDNAME, Inputdata = appointeedetail.AppointeeName, Fetcheddata = appointeeAadhaarFullName });
                        }
                        //if (validateCareOfName)
                        //{
                        //    ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.CAREOFNAME, Inputdata = appointeedetail?.MemberName?.ToString(), Fetcheddata = appointeeCareOf });
                        //}

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
                //PanNumber = panNumber,
            };

            CandidateValidateResponse response = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);


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
            //CandidateValidateUpdatedDataRequest candidateUpdatedDataReq = new()
            //{
            //    AppointeeId = reqObj.appointeeId,
            //    EmailId = string.Empty,
            //    UserId = reqObj.userId,
            //    UserName = string.Empty,
            //    Reasons = ReasonList,
            //    uanData = _uanDetails,
            //    Type = RemarksType.UAN

            //};
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
                    "Inactive UAN, Please activate your UAN from epfo site and try again." : GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase?.ToString(), "EPFO");
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
                        GetPassbookDetails.UserMessage = "OTP not validated, Please retry";
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
                        GetPassbookDetails.UserMessage = "OTP not validated, Please retry";
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

                response.UserMessage = isInvalidOtp ? "Invalid OTP, Please retry"
                                                    : GenarateErrorMsg((int)apiResponse.StatusCode, apiResponse.ReasonPhrase, "EPFO");
                response.ReasonPhrase = apiResponse.ReasonPhrase ?? string.Empty;
                response.OtpValidated = apiResponse.OtpValidated;
            }
            else
            {
                response.StatusCode = apiResponse?.StatusCode ?? HttpStatusCode.NotAcceptable;
                response.UserMessage = "Somethin went wrong, Please retry";
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
            List<EpsContributionCheckResult>? epsContributionDetails = new();
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

                //if (_passBookData?.overall_pf_balance != null)
                //{
                //    int _pensionshare = _passBookData?.overall_pf_balance?.pension_balance ?? 0;
                //    if (_pensionshare > 0)
                //    {
                //        isPensionApplicable = Convert.ToInt32(_pensionshare) > 0;
                //        _passBookData.overall_pf_balance.pension_balance = 1;
                //    }
                //}
                passbookJsonData = JsonConvert.SerializeObject(_passBookData);

            }
            if (apiProvider?.ToLower() == ApiProviderType.Signzy)
            {
                _apiResponse = await _signzyApiContext.GetPassbook(reqObj.OtpDetails.ClientId, reqObj.UserId);
                SignzyUanPassbookDetails? _passBookData = _apiResponse?.SignzyPassbkdata;
                if (_passBookData?.Companies.Count > 0)
                {
                    int _pensionshare = _passBookData?.Companies?.FirstOrDefault().Value?.PensionTotal ?? 0;
                    if (_pensionshare > 0)
                    {
                        isPensionApplicable = Convert.ToInt32(_pensionshare) > 0;
                        _passBookData.Companies.FirstOrDefault().Value.PensionTotal = 1;
                    }
                }
                passbookJsonData = JsonConvert.SerializeObject(_passBookData);

            }
            Response.StatusCode = _apiResponse.StatusCode;
            if (_apiResponse.StatusCode != HttpStatusCode.OK)
            {
                await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, ActivityLog.UANVERIFIFAILED);
                Response.UserMessage = (int)_apiResponse.StatusCode == (int)HttpStatusCode.UnprocessableEntity ? "Invalid Uan, Please retry" : GenarateErrorMsg((int)_apiResponse.StatusCode, _apiResponse?.ReasonPhrase?.ToString(), "EPFO");
                Response.ReasonPhrase = _apiResponse?.ReasonPhrase?.ToString() ?? string.Empty;
            }
            else
            {
                if (_apiResponse.Passbkdata == null && _apiResponse.KarzaPassbkdata == null && _apiResponse.SignzyPassbkdata == null)
                {
                    await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, ActivityLog.UANVERIFIFAILED);
                    Response.StatusCode = HttpStatusCode.UnprocessableEntity;
                    Response.UserMessage = "Site Not Reachable. Please try again after some time - OR - Opt for manual passbook upload.";
                }
                else
                {

                    //await PostPfPassbookData(reqObj, passbookJsonData, apiProvider?.ToLower());
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
                        Response.IsPensionApplicable = HasEpsContribution(epsContributionDetails);
                        Response.EpsContributionDetails = epsContributionDetails;
                        //Response.PfUan = _passBookData.pf_uan;
                    }
                    if (apiProvider?.ToLower() == ApiProviderType.Signzy)
                    {
                        SignzyUanPassbookDetails _passBookData = _apiResponse.SignzyPassbkdata;
                        Response.FathersName = _passBookData?.FatherName ?? string.Empty;
                        Response.Name = _passBookData?.FullName;
                        Response.DateOfBirth = _passBookData?.Dob.ToShortDateString() ?? string.Empty;
                        Response.IsPensionApplicable = isPensionApplicable;
                        Response.PfUan = _passBookData.Uan;
                    }

                    if (string.IsNullOrEmpty(Response.Name) || string.IsNullOrEmpty(Response.DateOfBirth))
                    {
                        await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, ActivityLog.UANVERIFIFAILED);
                        Response.StatusCode = HttpStatusCode.UnprocessableEntity;
                        Response.UserMessage = "Site Not Reachable. Please try again after some time - OR - Opt for manual passbook upload.";
                    }
                }
            }


            return Response;
        }

        private async Task PostPfPassbookData(GetPassbookDetailsRequest reqObj, string apiResponse, string apiProvider)
        {

            AppointeeDataSaveInFilesRequset uploadreq = new()
            {
                AppointeeId = reqObj.AppointeeId,
                AppointeeCode = reqObj?.AppointeeCode ?? "DEFAULT",
                FileTypeAlias = FileTypealias.PFPassbook,
                FileSubType = apiProvider,
                FileUploaded = apiResponse,
                UserId = reqObj.UserId,
                mimetype = "json"
            };

            await _fileService.postAppointeePassbookUpload(uploadreq);
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
            string? UanFatherName = reqObj?.PassbookDetails?.FathersName;
            string? UanDob = reqObj?.PassbookDetails?.DateOfBirth;
            bool _IsPensionApplicable = reqObj?.PassbookDetails?.IsPensionApplicable ?? false;
            bool _IsPensionGapIdentified = HasEpsGap(reqObj?.PassbookDetails?.EpsContributionDetails);
            bool _isFatherNameValidate = false;
            if (!reqObj.IsUanActive)
            {
                ReasonList.Add(new ReasonRemarks() { ReasonCode = ReasonCode.INACTIVE, Inputdata = string.Empty, Fetcheddata = string.Empty });
            }
            else if (appointeedetail.AppointeeDetailsId != null && reqObj?.PassbookDetails != null)
            {

                string? AppointeeFullName = string.IsNullOrEmpty(appointeedetail?.NameFromAadhaar) ? appointeedetail?.AppointeeName?.Trim() : appointeedetail?.NameFromAadhaar?.Trim();
                //_ = Convert.ToDateTime(string.IsNullOrEmpty(appointeedetail?.DobFromAadhaar) ? appointeedetail?.DateOfBirth?.ToString("dd/MM/yyyy") : appointeedetail?.DobFromAadhaar);

                string? fathersName = appointeedetail.MemberName;

                DateTime _inptdob = Convert.ToDateTime(UanDob);


                _isFatherNameValidate = !string.IsNullOrEmpty(fathersName) && fathersName.ToUpper() == UanFatherName?.ToUpper();
                if (AppointeeFullName?.ToUpper() == UanFullName?.ToUpper()
                    && appointeedetail?.DateOfBirth == _inptdob && _isFatherNameValidate && !_IsPensionGapIdentified)
                {
                    IsValid = true;
                }
                else
                {

                    if (AppointeeFullName?.ToUpper() != UanFullName?.ToUpper())
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
                IsFNameVarified= _isFatherNameValidate,
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
            };

            CandidateValidateResponse response = await _candidateContext.UpdateCandidateValidateData(candidateUpdatedDataReq);

            if (IsValid)
            {
                await _activityContext.PostActivityDetails(reqObj.AppointeeId, reqObj.UserId, ActivityLog.APNTVERIFICATIONCOMPLETE);
            }
            return response;
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
                    response = await GetPanMobileToUan(reqObj);

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

        public bool HasEpsGap(List<EpsContributionCheckResult> epsResults)
        {
            return epsResults?.Any(result => result.EpsGapfind) ?? false;
        }
        public bool HasEpsContribution(List<EpsContributionCheckResult> epsResults)
        {
            return epsResults?.Any(result => result.HasEpsContribution) ?? false;
        }

    }
}
