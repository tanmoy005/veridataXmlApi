using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VERIDATA.BLL.Interfaces;
using VERIDATA.BLL.Services;
using VERIDATA.Model.Base;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;

namespace PfcAPI.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IUserContext _userContext;
        private readonly ISetupConfigarationContext _setupConfigarationContext;
        private readonly ErrorResponse _ErrorResponse = new();
        private readonly IApiConfigService _apiConfigService;
        private readonly ISetupConfigarationContext _configService;

        public AccountController(IUserContext userContext, ISetupConfigarationContext setupConfigarationContext, IApiConfigService apiConfigService, ISetupConfigarationContext configService)
        {
            _userContext = userContext;
            _setupConfigarationContext = setupConfigarationContext;
            _apiConfigService = apiConfigService;
            _configService = configService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ValidateUserLogIn")]
        public ActionResult ValidateUserLogIn([FromBody] UserSignInRequest user)
        {
            try
            {
                string _Otp = string.Empty;
                UserDetailsResponse userDetails = new();
                ValidateUserDetails validateUserResponse = Task.Run(async () => await _userContext.validateUserSign(user)).GetAwaiter().GetResult();
                if (validateUserResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    _ErrorResponse.ErrorCode = 400;
                    _ErrorResponse.UserMessage = validateUserResponse.UserMessage;
                    _ErrorResponse.InternalMessage = "Bad Request";

                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, _ErrorResponse));
                }

                ValidateUserSignInResponse res = Task.Run(async () => await _userContext.postUserAuthdetails(validateUserResponse)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<ValidateUserSignInResponse>(HttpStatusCode.OK, res));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("UserSignInDetails")]
        public ActionResult UserSignInDetails([FromBody] ValidateUserSignInRequest user)
        {
            try
            {
                AuthenticatedUserResponse userDetails = new();

                int validatedUserId = Task.Run(async () => await _userContext.validateUserByOtp(user.clientId, user.OTP, user.dbUserType)).GetAwaiter().GetResult();
                if (validatedUserId == 0)
                {
                    string _errormsg = "The OTP is invalid. Please try again.";

                    _ErrorResponse.ErrorCode = 400;
                    _ErrorResponse.UserMessage = _errormsg;
                    _ErrorResponse.InternalMessage = "Bad Request";
                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, _ErrorResponse));
                }
                userDetails = Task.Run(async () => await _userContext.getValidatedSigninUserDetails(validatedUserId)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<AuthenticatedUserResponse>(HttpStatusCode.OK, userDetails));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("DefaultPasswordChange")]

        public ActionResult DefaultPasswordChange(SetNewPasswordRequest req)
        {
            try
            {
                Task.Run(async () => await _userContext.postUserPasswordChange(req)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<bool>(HttpStatusCode.OK, true));
            }
            catch (Exception)
            {
                throw;
            }
        }


        [Authorize]
        [HttpPost]
        [Route("ValidateProfilePassword")]

        public ActionResult ValidateProfilePassword(ValidateProfilePasswordRequest req)
        {
            try
            {
                bool ValidateUserResponse = Task.Run(async () => await _userContext.validateProfilePasswowrdAsync(req)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<bool>(HttpStatusCode.OK, ValidateUserResponse));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GenerateRefreshToken")]
        public ActionResult GenerateRefreshToken(RefreshTokenRequest req)
        {
            try
            {
                TokenDetailsResponse RefreshTokenResponse = Task.Run(async () => await _userContext.getRefreshToken(req)).GetAwaiter().GetResult();
                if (!(RefreshTokenResponse.IsValid ?? false))
                {
                    string _errormsg = "Invalid client request, Please try log in again";

                    _ErrorResponse.ErrorCode = 400;
                    _ErrorResponse.UserMessage = _errormsg;
                    _ErrorResponse.InternalMessage = "Bad Request";
                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, _ErrorResponse));
                }
                return Ok(new BaseResponse<TokenDetailsResponse>(HttpStatusCode.OK, RefreshTokenResponse));
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPost]
        [Route("signOut")]
        public ActionResult SignOut(int userId)
        {
            try
            {
                Task.Run(async () => await _userContext.postUsersignOut(userId)).GetAwaiter().GetResult();

                return Ok(new BaseResponse<bool>(HttpStatusCode.OK, true));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("EditUserProfile")]
        public ActionResult EditUserProfile(EditUserProfileRequest req)
        {
            try
            {
                Task.Run(async () => await _userContext.editUserProfile(req)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<string>(HttpStatusCode.OK, "Success"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetMenuListData")]
        public ActionResult GetMenuListData(int userId)
        {
            try
            {
                if (userId == 0)
                {
                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, new ErrorResponse
                    {
                        ErrorCode = 5000,
                        InternalMessage = "Parameter type is not being passed or invalid.",
                        UserMessage = "Parameter type is not being passed or invalid."
                    }));
                }
                List<MenuNodeResponse> _dataList = new();
                _dataList = Task.Run(async () => await _userContext.GetMenuData(userId)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<List<MenuNodeResponse>>(HttpStatusCode.OK, _dataList));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetMastarDropdowndata")]
        public ActionResult GetMastarDropdowndata(string type)
        {
            try
            {
                if (string.IsNullOrEmpty(type))
                {
                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, new ErrorResponse
                    {
                        ErrorCode = 5000,
                        InternalMessage = "Parameter type is not being passed or invalid.",
                        UserMessage = "Parameter type is not being passed or invalid."
                    }));
                }
                List<DropDownDetailsResponse> _dataList = new();
                _dataList = Task.Run(async () => await _userContext.getDropDownData(type)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<DropDownDetailsResponse>(HttpStatusCode.OK, _dataList));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetDashboardWidgetCardData")]
        public ActionResult GetWizData(int? filterDays, bool isfilterd)
        {
            try
            {
                List<DashboardWidgetResponse> _dataList = new();
                _dataList = Task.Run(async () => await _userContext.GetWidgetData(filterDays, isfilterd)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<DashboardWidgetResponse>(HttpStatusCode.OK, _dataList));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetTotalCriticalAppointee")]
        public ActionResult GetCriticalWizData()
        {
            try
            {
                CriticalAppointeeWidgetResponse _criticaldata = new();
                _criticaldata = Task.Run(_userContext.GetCriticalData).GetAwaiter().GetResult();
                return Ok(new BaseResponse<CriticalAppointeeWidgetResponse>(HttpStatusCode.OK, _criticaldata));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetTotalWidgetData")]
        public ActionResult GetTotalWidgetData()
        {
            try
            {
                WidgetProgressDataResponse _dataList = new();
                _dataList = Task.Run(_userContext.GetTotalProgressWidgetData).GetAwaiter().GetResult();
                return Ok(new BaseResponse<WidgetProgressDataResponse>(HttpStatusCode.OK, _dataList));
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpGet]
        [Route("GetAppointeeStatusDetails")]
        public ActionResult GetAppointeeStatusDetails(string code)
        {
            try
            {
                List<AppointeeStatusWizResponse> _dataList = new();
                _dataList = Task.Run(async () => await _userContext.GetAppointeeStatusWidgetData(code)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<AppointeeStatusWizResponse>(HttpStatusCode.OK, _dataList));
            }
            catch (Exception)
            {
                throw;
            }
        }



        [Authorize]
        [HttpGet]
        [Route("GetSetupConfigData")]
        public ActionResult GetSetupConfigData()
        {
            //var _ErrorResponse = new ErrorResponse();
            try
            {
                GeneralSetupDetailsResponse _data = new();
                _data = Task.Run(_setupConfigarationContext.gettSetupData).GetAwaiter().GetResult();
                return Ok(new BaseResponse<GeneralSetupDetailsResponse>(HttpStatusCode.OK, _data));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("PostSetupConfigData")]
        public ActionResult PostSetupConfigData(GeneralSetupSubmitRequest Request)
        {
            if (Request == null)
            {
                return BadRequest();
            }
            try
            {

                Task.Run(async () => await _setupConfigarationContext.PostSetupData(Request)).GetAwaiter().GetResult();



                return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
            }
            catch (Exception)
            {
                throw;
            }
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("GetApiConfigData")]
        public ActionResult GetApiConfigData()
        {
            //var _ErrorResponse = new ErrorResponse();
            try
            {
                //ApiConfigResponse _data = new();
                var _data = _apiConfigService.GetApiConfigDetails();
                return Ok(new BaseResponse<ApiConfigResponse>(HttpStatusCode.OK, _data));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetFaqData")]
        public ActionResult GetFaqData()
        {
            try
            {
                var _data = Task.Run(async () => await _configService.GetFaqData()).GetAwaiter().GetResult();
                return Ok(new BaseResponse<FaqDetailsResponse>(HttpStatusCode.OK, _data));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
