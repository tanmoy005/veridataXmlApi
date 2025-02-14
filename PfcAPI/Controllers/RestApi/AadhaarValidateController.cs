using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using VERIDATA.BLL.Interfaces;
using VERIDATA.BLL.Services;
using VERIDATA.BLL.utility;
using VERIDATA.DAL.utility;
using VERIDATA.Model.Base;
using VERIDATA.Model.Configuration;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Response.api.Signzy;
using VERIDATA.Model.utility;
using static VERIDATA.DAL.utility.CommonEnum;

namespace PfcAPI.Controllers.RestApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class AadhaarValidateController : ControllerBase
    {
        private readonly ErrorResponse _ErrorResponse = new();
        private readonly ApiConfiguration _apiConfig;
        private readonly IVerifyDataContext _varifyCandidate;
        private readonly IFileContext _fileContext;
        private readonly IApiConfigService _apiConfigService;

        public AadhaarValidateController(IVerifyDataContext varifyCandidate, ApiConfiguration apiConfig, IFileContext fileContext, IApiConfigService apiConfigService)
        {
            _varifyCandidate = varifyCandidate;
            _apiConfig = apiConfig;
            _fileContext = fileContext;
            _apiConfigService = apiConfigService;
        }

        [Authorize(Roles = $"{RoleTypeAlias.Appointee}")]
        //[AllowAnonymous]
        [HttpPost]
        [Route("OfflineKycStatusUpdate")]
        public IActionResult OfflineKycStatusUpdate(OfflineAadharVarifyStatusUpdateRequest reqObj)
        {
            try
            {
                Task.Run(async () => await _fileContext.OfflineKycStatusUpdate(reqObj)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = $"{RoleTypeAlias.Appointee}")]
        //[AllowAnonymous]
        [HttpPost]
        [Route("VerifyAadharViaXml")]
        public IActionResult VerifyAadharViaXml([FromForm] AppointeeAadhaarAadharXmlVarifyRequest reqObj)
        {
            try
            {
                VarificationStatusResponse response = new();
                if (_apiConfig.IsApiCall)
                {
                    var unZipRes = Task.Run(async () => await _fileContext.unzipAdharzipFiles(reqObj)).GetAwaiter().GetResult();

                    if (!unZipRes.IsValid)
                    {
                        _ErrorResponse.ErrorCode = (int)HttpStatusCode.InternalServerError;
                        _ErrorResponse.UserMessage = unZipRes?.Message ?? string.Empty;
                        _ErrorResponse.InternalMessage = unZipRes?.Message ?? string.Empty;
                        return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.InternalServerError, _ErrorResponse));
                    }
                    else
                    {
                        var getAadharDetails = Task.Run(async () => await _varifyCandidate.GetAadharDetailsFromXml(unZipRes.FileContent)).GetAwaiter().GetResult();
                        AadharValidationRequest VarifyReq = new()
                        {
                            AadharDetails = getAadharDetails,
                            isValidAdhar = true,
                            AppointeeId = reqObj.appointeeId,
                            AppointeeAadhaarName = reqObj.aadharName,
                            sharePhrase = reqObj.shareCode
                        };
                        var VerifyAadhar = Task.Run(async () => await _varifyCandidate.VerifyAadharData(VarifyReq)).GetAwaiter().GetResult();

                        response = new()
                        {
                            IsVarified = VerifyAadhar?.IsValid ?? false,
                            Remarks = VerifyAadhar?.Remarks
                        };
                    }
                }
                else
                {
                    _ErrorResponse.ErrorCode = (int)HttpStatusCode.InternalServerError;
                    _ErrorResponse.UserMessage = "The server has been temporarily shut down by the administrator. Please contact the administrator for further assistance.";
                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.InternalServerError, _ErrorResponse));
                }
                return Ok(new BaseResponse<VarificationStatusResponse>(HttpStatusCode.OK, response));
            }
            catch (Exception ex)
            {
                string msg = _varifyCandidate.GenarateErrorMsg((int)HttpStatusCode.InternalServerError, "", "UIDAI (Aadhar)");
                CustomException excp = new(msg, ex);
                throw excp;
            }
        }

        [Authorize(Roles = $"{RoleTypeAlias.Appointee}")]
        //[AllowAnonymous]
        [HttpPost]
        [Route("GetUANDetails")]
        public IActionResult GetUAN(GetUanNumberDetailsRequest reqObj)
        {
            try
            {
                GetUanResponse Response = new();
                if (_apiConfig.IsApiCall)
                {
                    Response = Task.Run(async () => await _varifyCandidate.GetUanNumber(reqObj)).GetAwaiter().GetResult();
                    if (Response.StatusCode != HttpStatusCode.OK)
                    {
                        _ErrorResponse.ErrorCode = (int)Response.StatusCode;
                        _ErrorResponse.UserMessage = Response?.UserMessage ?? string.Empty;
                        _ErrorResponse.InternalMessage = Response?.ReasonPhrase ?? string.Empty;
                        return Ok(new BaseResponse<ErrorResponse>(Response?.StatusCode ?? HttpStatusCode.InternalServerError, _ErrorResponse));
                    }
                }
                else
                {
                    Response.IsUanAvailable = false;
                    Response.Remarks = "The server has been temporarily shut down by the administrator. Please contact the administrator for further assistance.";
                }

                return Ok(new BaseResponse<GetUanResponse>(HttpStatusCode.OK, Response));
            }
            catch (Exception ex)
            {
                string msg = _varifyCandidate.GenarateErrorMsg((int)HttpStatusCode.InternalServerError, "", "UIDAI (Aadhar)");
                CustomException excp = new(msg, ex);
                throw excp;
            }
        }

        [Authorize(Roles = $"{RoleTypeAlias.Appointee}")]
        // [AllowAnonymous]
        [HttpPost]
        [Route("UANGenerateOTP")]
        public IActionResult GenerateUANOTP(UanGenerateOtpRequest reqObj)
        {
            try
            {
                UanGenerateOtpResponse Response = new();
                if (_apiConfig.IsApiCall)
                {
                    UanGenerateOtpDetails? GenarateOtpResponse = Task.Run(async () => await _varifyCandidate.GeneratetUANOTP(reqObj)).GetAwaiter().GetResult();
                    if (GenarateOtpResponse.StatusCode != HttpStatusCode.OK)
                    {
                        if (GenarateOtpResponse.StatusCode == HttpStatusCode.UnprocessableEntity)
                        {
                            UanValidationRequest VarifyReq = new()
                            {
                                PassbookDetails = new GetPassbookDetailsResponse(),
                                IsUanActive = false,
                                IsPassbookFetch = false,
                                AppointeeId = reqObj.appointeeId
                            };

                            CandidateValidateResponse VerifyAadhar = Task.Run(async () => await _varifyCandidate.VerifyUanData(VarifyReq)).GetAwaiter().GetResult();
                        }
                        _ErrorResponse.ErrorCode = (int)GenarateOtpResponse.StatusCode;
                        _ErrorResponse.UserMessage = GenarateOtpResponse?.UserMessage ?? string.Empty;
                        _ErrorResponse.InternalMessage = GenarateOtpResponse?.ReasonPhrase ?? string.Empty;
                        return Ok(new BaseResponse<ErrorResponse>(GenarateOtpResponse.StatusCode, _ErrorResponse));
                    }
                    else
                    {
                        Response.is_async = GenarateOtpResponse?.IsAsync ?? false;
                        Response.otp_sent = GenarateOtpResponse?.OtpSent ?? false;
                        Response.client_id = GenarateOtpResponse?.ClientId ?? string.Empty;
                        Response.masked_mobile_number = GenarateOtpResponse?.MaskedMobileNumber ?? string.Empty;
                    }
                }
                else
                {
                    _ErrorResponse.ErrorCode = (int)HttpStatusCode.InternalServerError;
                    _ErrorResponse.UserMessage = "The server has been temporarily shut down by the administrator. Please contact the administrator for further assistance.";
                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.InternalServerError, _ErrorResponse));
                }

                return Ok(new BaseResponse<UanGenerateOtpResponse>(HttpStatusCode.OK, Response));
            }
            catch (Exception)
            {
                Task.Run(async () => await _varifyCandidate.PostActivity(reqObj.appointeeId, reqObj.userId, ActivityLog.UANVERIFIFAILED)).GetAwaiter().GetResult();
                throw;
            }
        }

        [Authorize(Roles = $"{RoleTypeAlias.Appointee}")]
        //[AllowAnonymous]
        [HttpPost]
        [Route("UANSubmitOTP")]
        public IActionResult SubmitUANOTP(AppointeeUANSubmitOtpRequest reqObj)
        {
            try
            {
                VarificationStatusResponse response = new();
                if (_apiConfig.IsApiCall)
                {
                    GetPassbookDetailsResponse? GenarateOtpPassbookResponse = Task.Run(async () => await _varifyCandidate.GetPassbookBySubmitOTP(reqObj)).GetAwaiter().GetResult();

                    if (GenarateOtpPassbookResponse.StatusCode != HttpStatusCode.OK)
                    {
                        _ErrorResponse.ErrorCode = GenarateOtpPassbookResponse.StatusCode == HttpStatusCode.NotAcceptable ? (int)HttpStatusCode.OK : (int)GenarateOtpPassbookResponse.StatusCode;
                        _ErrorResponse.UserMessage = GenarateOtpPassbookResponse?.UserMessage ?? string.Empty;
                        _ErrorResponse.InternalMessage = GenarateOtpPassbookResponse?.ReasonPhrase ?? string.Empty;
                        return Ok(new BaseResponse<ErrorResponse>(GenarateOtpPassbookResponse.StatusCode, _ErrorResponse));
                    }
                    else
                    {
                        //if (GenarateOtpPassbookResponse?.IsCallBack ?? false)
                        //{
                        //var callBackResponse = Task.Run(async () => await _apiConfigService.CheckForCallBacKPassbookData("VarifyReq")).GetAwaiter().GetResult();
                        //GenarateOtpPassbookResponse = Task.Run(async () => await _varifyCandidate.ValidateBackGetPfPassbookData(callBackResponse, reqObj.appointeeId, reqObj.userId, GenarateOtpPassbookResponse?.PfUan)).GetAwaiter().GetResult();
                        //}
                        UanValidationRequest VarifyReq = new()
                        {
                            PassbookDetails = GenarateOtpPassbookResponse,
                            IsUanActive = true,
                            IsPassbookFetch = true,
                            AppointeeId = reqObj.appointeeId
                        };

                        CandidateValidateResponse VerifyUan = Task.Run(async () => await _varifyCandidate.VerifyUanData(VarifyReq)).GetAwaiter().GetResult();
                        response = new()
                        {
                            IsVarified = VerifyUan.IsValid,
                            Remarks = VerifyUan.Remarks
                        };
                    }
                }
                else
                {
                    _ErrorResponse.ErrorCode = (int)HttpStatusCode.InternalServerError;
                    _ErrorResponse.UserMessage = "The server has been temporarily shut down by the administrator. Please contact the administrator for further assistance.";
                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.InternalServerError, _ErrorResponse));
                }
                return Ok(new BaseResponse<VarificationStatusResponse>(HttpStatusCode.OK, response));
            }
            catch (Exception ex)
            {
                string msg = _varifyCandidate.GenarateErrorMsg((int)HttpStatusCode.InternalServerError, "", "EPFO");
                CustomException excp = new(msg, ex);
                throw excp;
            }
        }

        [Authorize(Roles = $"{RoleTypeAlias.Appointee}")]
        //[AllowAnonymous]
        [HttpPost]
        [Route("VerifyPassportDetails")]
        public IActionResult GetPassportDetails(AppointeePassportValidateRequest reqobj)
        {
            try
            {
                AppointeePassportValidateResponse Response = new();
                if (_apiConfig.IsApiCall)
                {
                    Response = Task.Run(async () => await _varifyCandidate.PassportDetailsValidation(reqobj)).GetAwaiter().GetResult();
                    if (Response.StatusCode != HttpStatusCode.OK)
                    {
                        _ErrorResponse.ErrorCode = Response.StatusCode == HttpStatusCode.NotAcceptable ? (int)HttpStatusCode.OK : (int)Response.StatusCode;
                        _ErrorResponse.UserMessage = Response?.Remarks ?? string.Empty;
                        _ErrorResponse.InternalMessage = Response?.Remarks ?? string.Empty;
                        return Ok(new BaseResponse<ErrorResponse>(Response.StatusCode, _ErrorResponse));
                    }
                }
                else
                {
                    Response.IsValid = false;
                    Response.Remarks = "The server has been temporarily shut down by the administrator. Please contact the administrator for further assistance.";
                }
                return Ok(new BaseResponse<AppointeePassportValidateResponse>(HttpStatusCode.OK, Response));
            }
            catch (Exception)
            {
                Task.Run(async () => await _varifyCandidate.PostActivity(reqobj.appointeeId, reqobj.userId, ActivityLog.PASPRTVERIFIFAILED)).GetAwaiter().GetResult();
                throw;
            }
        }

        //[AllowAnonymous]
        [Authorize(Roles = $"{RoleTypeAlias.Appointee}")]
        [HttpPost]
        [Route("VerifyPanDetails")]
        public IActionResult VerifyPanDetails(AppointeePanValidateRequest reqobj)
        {
            try
            {
                AppointeePanValidateResponse Response = new();
                if (_apiConfig.IsApiCall)
                {
                    Response = Task.Run(async () => await _varifyCandidate.PanDetailsValidation(reqobj)).GetAwaiter().GetResult();
                    if (Response.StatusCode != HttpStatusCode.OK)
                    {
                        _ErrorResponse.ErrorCode = Response.StatusCode == HttpStatusCode.NotAcceptable ? (int)HttpStatusCode.OK : (int)Response.StatusCode;
                        _ErrorResponse.UserMessage = Response?.Remarks ?? string.Empty;
                        _ErrorResponse.InternalMessage = Response?.Remarks ?? string.Empty;
                        return Ok(new BaseResponse<ErrorResponse>(Response.StatusCode, _ErrorResponse));
                    }
                }
                else
                {
                    Response.IsValid = false;
                    Response.Remarks = "The server has been temporarily shut down by the administrator. Please contact the administrator for further assistance.";
                }
                return Ok(new BaseResponse<AppointeePanValidateResponse>(HttpStatusCode.OK, Response));
            }
            catch (Exception)
            {
                Task.Run(async () => await _varifyCandidate.PostActivity(reqobj.appointeeId, reqobj.userId, ActivityLog.PANVERIFIFAILED)).GetAwaiter().GetResult();
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("callback")]
        public async Task<IActionResult> ReceiveCallback([FromBody] SignzyUanPassbookDetails? ResponseData)
        {
            if (ResponseData?.EmployeeDetails == null)
            {
                return BadRequest("Invalid data received.");
            }
            //SignzyUanPassbookDetails passbookResponse = JsonConvert.DeserializeObject<SignzyUanPassbookDetails>(ResponseData);
            // Save the validation result
            await _apiConfigService.StoreCallBacKPassbookData(ResponseData);

            return Ok(new { message = "Validation completed." });
        }

        [Authorize(Roles = $"{RoleTypeAlias.Appointee}")]
        [HttpPost]
        [Route("VerifyPanDetailsPriorityBase")]
        public IActionResult VerifyPanDetailsPriorityBase(AppointeePanValidateRequest reqObj)
        {
            try
            {
                AppointeePanValidateResponse response = new();

                if (_apiConfig.IsApiCall)
                {
                    response = Task.Run(async () =>
                    {
                        return await _varifyCandidate.ValidateDetailsPriorityBase<AppointeePanValidateRequest, AppointeePanValidateResponse>(
                            reqObj,
                            CommonEnum.ApiType.Pan.ToString()
                        );
                    }).GetAwaiter().GetResult();

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        _ErrorResponse.ErrorCode = response.StatusCode == HttpStatusCode.NotAcceptable ?
                            (int)HttpStatusCode.OK :
                            (int)response.StatusCode;

                        _ErrorResponse.UserMessage = response?.Remarks ?? string.Empty;
                        _ErrorResponse.InternalMessage = response?.Remarks ?? string.Empty;

                        return Ok(new BaseResponse<ErrorResponse>(response.StatusCode, _ErrorResponse));
                    }
                }
                else
                {
                    response.IsValid = false;
                    response.Remarks = "The server has been temporarily shut down by the administrator. Please contact the administrator for further assistance.";
                }

                return Ok(new BaseResponse<AppointeePanValidateResponse>(HttpStatusCode.OK, response));
            }
            catch (Exception)
            {
                // Log the failure
                Task.Run(async () =>
                    await _varifyCandidate.PostActivity(reqObj.appointeeId, reqObj.userId, ActivityLog.PANVERIFIFAILED)
                ).GetAwaiter().GetResult();
                throw;
            }
        }

        [Authorize(Roles = $"{RoleTypeAlias.Appointee}")]
        [HttpPost]
        [Route("VerifyUanDetailsPriorityBase")]
        public IActionResult VerifyUanDetails(GetUanNumberDetailsRequest reqObj)
        {
            try
            {
                GetUanResponse response = new();

                if (_apiConfig.IsApiCall)
                {
                    response = Task.Run(async () =>
                    {
                        return await _varifyCandidate.ValidateDetailsPriorityBase<GetUanNumberDetailsRequest, GetUanResponse>(
                            reqObj,
                            CommonEnum.ApiType.UAN.ToString()
                        );
                    }).GetAwaiter().GetResult();

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        _ErrorResponse.ErrorCode = response.StatusCode == HttpStatusCode.NotAcceptable ?
                            (int)HttpStatusCode.OK :
                            (int)response.StatusCode;

                        _ErrorResponse.UserMessage = response?.Remarks ?? string.Empty;
                        _ErrorResponse.InternalMessage = response?.Remarks ?? string.Empty;

                        return Ok(new BaseResponse<ErrorResponse>(response.StatusCode, _ErrorResponse));
                    }
                }
                else
                {
                    response.IsUanAvailable = false;
                    response.Remarks = "The server has been temporarily shut down by the administrator. Please contact the administrator for further assistance.";
                }

                return Ok(new BaseResponse<GetUanResponse>(HttpStatusCode.OK, response));
            }
            catch (Exception ex)
            {
                string msg = _varifyCandidate.GenarateErrorMsg((int)HttpStatusCode.InternalServerError, "", "UIDAI (Aadhar)");
                CustomException excp = new(msg, ex);
                throw excp;
            }
        }

        [Authorize(Roles = $"{RoleTypeAlias.Appointee}")]
        [HttpPost]
        [Route("VerifyPassportDetailsPriorityBase")]
        public IActionResult GetPassportDetailsPriorityBase(AppointeePassportValidateRequest reqObj)
        {
            try
            {
                AppointeePassportValidateResponse response = new();

                if (_apiConfig.IsApiCall)
                {
                    response = Task.Run(async () =>
                    {
                        return await _varifyCandidate.ValidateDetailsPriorityBase<AppointeePassportValidateRequest, AppointeePassportValidateResponse>(
                            reqObj,
                            CommonEnum.ApiType.Passport.ToString()
                        );
                    }).GetAwaiter().GetResult();

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        _ErrorResponse.ErrorCode = response.StatusCode == HttpStatusCode.NotAcceptable ?
                            (int)HttpStatusCode.OK :
                            (int)response.StatusCode;

                        _ErrorResponse.UserMessage = response?.Remarks ?? string.Empty;
                        _ErrorResponse.InternalMessage = response?.Remarks ?? string.Empty;

                        return Ok(new BaseResponse<ErrorResponse>(response.StatusCode, _ErrorResponse));
                    }
                }
                else
                {
                    response.IsValid = false;
                    response.Remarks = "The server has been temporarily shut down by the administrator. Please contact the administrator for further assistance.";
                }

                return Ok(new BaseResponse<AppointeePassportValidateResponse>(HttpStatusCode.OK, response));
            }
            catch (Exception)
            {
                Task.Run(async () => await _varifyCandidate.PostActivity(reqObj.appointeeId, reqObj.userId, ActivityLog.PASPRTVERIFIFAILED)).GetAwaiter().GetResult();
                throw;
            }
        }

        [Authorize(Roles = $"{RoleTypeAlias.Appointee}")]
       // [AllowAnonymous]
        [HttpPost]
        [Route("GenerateOTP")]
        public IActionResult GenerateAadhaarOTP(AppointeeAadhaarValidateRequest reqObj)
        {
            try
            {
                AppointeeAadhaarGenerateOtpResponse Response = new();
                if (_apiConfig.IsApiCall)
                {
                    AadharGenerateOTPDetails GenarateOtpResponse = Task.Run(async () => await _varifyCandidate.GeneratetAadharOTP(reqObj)).GetAwaiter().GetResult();
                    if (GenarateOtpResponse.StatusCode != HttpStatusCode.OK)
                    {
                        _ErrorResponse.ErrorCode = (int)GenarateOtpResponse.StatusCode;
                        _ErrorResponse.UserMessage = GenarateOtpResponse?.UserMessage ?? string.Empty;
                        _ErrorResponse.InternalMessage = GenarateOtpResponse?.ReasonPhrase ?? string.Empty;
                        return Ok(new BaseResponse<ErrorResponse>(GenarateOtpResponse.StatusCode, _ErrorResponse));
                    }
                    else
                    {
                        Response.if_number = GenarateOtpResponse?.if_number ?? false;
                        Response.otp_sent = GenarateOtpResponse?.otp_sent ?? false;
                        Response.client_id = GenarateOtpResponse?.client_id ?? string.Empty;
                        Response.valid_aadhaar = GenarateOtpResponse?.valid_aadhaar ?? false;
                    }
                }
                else
                {
                    _ErrorResponse.ErrorCode = (int)HttpStatusCode.InternalServerError;
                    _ErrorResponse.UserMessage = "server is temporarily shutdown by the admin;please contact with administrator";
                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.InternalServerError, _ErrorResponse));
                }

                return Ok(new BaseResponse<AppointeeAadhaarGenerateOtpResponse>(HttpStatusCode.OK, Response));
            }
            catch (Exception ex)
            {
                string msg = _varifyCandidate.GenarateErrorMsg((int)HttpStatusCode.InternalServerError, "", "UIDAI (Aadhar)");
                Task.Run(async () => await _varifyCandidate.PostActivity(reqObj.appointeeId, reqObj.userId, ActivityLog.ADHVERIFIFAILED)).GetAwaiter().GetResult();
                CustomException excp = new(msg, ex);
                throw excp;
            }
        }

        [Authorize(Roles = $"{RoleTypeAlias.Appointee}")]
        //[AllowAnonymous]
        [HttpPost]
        [Route("SubmitOTP")]
        public IActionResult SubmitAadhaarOTP(AppointeeAadhaarSubmitOtpRequest reqObj)
        {
            try
            {
                VarificationStatusResponse response = new();
                if (_apiConfig.IsApiCall)
                {
                    reqObj.shareCode = CommonUtility.RandomNumber(4);
                    AadharSubmitOtpDetails GenarateOtpResponse = Task.Run(async () => await _varifyCandidate.SubmitAadharOTP(reqObj)).GetAwaiter().GetResult();

                    if (GenarateOtpResponse.StatusCode != HttpStatusCode.OK)
                    {
                        _ErrorResponse.ErrorCode = (int)GenarateOtpResponse.StatusCode;
                        _ErrorResponse.UserMessage = GenarateOtpResponse?.UserMessage ?? string.Empty;
                        _ErrorResponse.InternalMessage = GenarateOtpResponse?.ReasonPhrase ?? string.Empty;
                        return Ok(new BaseResponse<ErrorResponse>(GenarateOtpResponse.StatusCode, _ErrorResponse));
                    }
                    else
                    {
                        AadharValidationRequest VarifyReq = new()
                        {
                            AadharDetails = GenarateOtpResponse,
                            isValidAdhar = true,
                            AppointeeId = reqObj.appointeeId,
                            AppointeeAadhaarName = reqObj.aadharName,
                            sharePhrase = reqObj.shareCode

                        };

                        CandidateValidateResponse? VerifyAadhar = Task.Run(async () => await _varifyCandidate.VerifyAadharData(VarifyReq)).GetAwaiter().GetResult();
                        response = new()
                        {
                            IsVarified = VerifyAadhar?.IsValid ?? false,
                            Remarks = VerifyAadhar?.Remarks
                        };
                    }
                }
                else
                {
                    _ErrorResponse.ErrorCode = (int)HttpStatusCode.InternalServerError;
                    _ErrorResponse.UserMessage = "server is temporarily shutdown by the admin;please contact with administrator";
                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.InternalServerError, _ErrorResponse));
                }
                return Ok(new BaseResponse<VarificationStatusResponse>(HttpStatusCode.OK, response));
            }
            catch (Exception ex)
            {
                string msg = _varifyCandidate.GenarateErrorMsg((int)HttpStatusCode.InternalServerError, "", "UIDAI (Aadhar)");
                CustomException excp = new(msg, ex);
                throw excp;
            }
        }
    }
}