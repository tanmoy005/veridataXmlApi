using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using VERIDATA.BLL.Interfaces;
using VERIDATA.BLL.utility;
using VERIDATA.Model.Base;
using VERIDATA.Model.Configuration;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;

namespace PfcAPI.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ErrorResponse _ErrorResponse = new();
        private readonly ApiConfiguration _aadhaarConfig;
        private readonly string key;
        public UsersController(IUserContext userContext, ApiConfiguration aadhaarConfig)
        {
            _userContext = userContext;
            _aadhaarConfig = aadhaarConfig;
            key = aadhaarConfig?.EncriptKey ?? string.Empty;
        }


        ////[Authorize(Roles = $"{RoleTypeAlias.SuperAdmin},{RoleTypeAlias.CompanyAdmin},{RoleTypeAlias.GeneralAdmin}")]
        [Authorize]
        [HttpGet("GetAdminUserList")]
        public ActionResult GetAdminUserList()
        {
            try
            {
                List<UserDetailsResponse> userList = new();
                userList = Task.Run(_userContext.getAllAdminUser).GetAwaiter().GetResult();
                return Ok(new BaseResponse<UserDetailsResponse>(HttpStatusCode.OK, userList));
            }
            catch (Exception)
            {
                throw;

            }
        }

        [Authorize]
        [HttpPost("CreateUser")]
        public ActionResult CreateUser(CreateUserDetailsRequest _userdetails)
        {
            try
            {
                bool isCreateUser = Task.Run(async () => await _userContext.createNewUserwithRole(_userdetails)).GetAwaiter().GetResult();
                if (isCreateUser)
                {
                    return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
                }
                else
                {
                    _ErrorResponse.ErrorCode = 500;
                    _ErrorResponse.UserMessage = "User Already Exsist,please try another name";
                    _ErrorResponse.InternalMessage = "User Already Exsist";
                    return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest, _ErrorResponse));
                }
            }
            catch (Exception)
            {
                throw;

            }
        }
        [Authorize]
        [HttpGet("GetUserByUserId")]
        public ActionResult GetUserByUserId(int userId)
        {
            try
            {
                UserDetailsResponse userDetails = Task.Run(async () => await _userContext.getUserDetailsAsyncbyId(userId)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<UserDetailsResponse>(HttpStatusCode.OK, userDetails));

            }
            catch (Exception)
            {
                throw;

            }
        }

        [Authorize]
        [HttpPost("UpdateAdminUser")]
        public ActionResult EditAdminUser(AdminUserUpdateRequest userDetails)
        {
            try
            {
                Task.Run(async () => await _userContext.editAdminUser(userDetails)).GetAwaiter().GetResult();
                return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
            }
            catch (Exception)
            {
                throw;

            }
        }
        [Authorize]
        [HttpPost("RemoveAdminUser")]
        public IActionResult RemoveAdminUsers(int id, int userId)
        {
            try
            {
                bool removeUser = Task.Run(async () => await _userContext.removeUserDetails(id, userId)).GetAwaiter().GetResult();

                return removeUser == false
                    ? Ok(new BaseResponse<string>(HttpStatusCode.NotFound, "success"))
                    : (IActionResult)Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
            }
            catch (Exception)
            {
                throw;

            }
        }

        [Authorize]
        [HttpPost("ValidateUserCode")]
        public ActionResult ValidateUserCode(string userCode)
        {

            try
            {
                if (string.IsNullOrEmpty(userCode))
                {
                    return Ok(new BaseResponse<string>(HttpStatusCode.BadRequest, "Error"));
                }
                else
                {
                    bool ValidateUser = Task.Run(async () => await _userContext.validateUserByCode(userCode)).GetAwaiter().GetResult();
                    if (!ValidateUser)
                    {
                        return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));
                    }
                    else
                    {
                        _ErrorResponse.ErrorCode = (int)HttpStatusCode.Ambiguous;
                        _ErrorResponse.UserMessage = "User Already Exsist,please try another name";
                        _ErrorResponse.InternalMessage = "User Already Exsist";
                        return Ok(new BaseResponse<ErrorResponse>(HttpStatusCode.Ambiguous, _ErrorResponse));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize]
        //[HttpPost("CreateRole")]
        //public ActionResult CreateRole(CreateUsereRoleRequest _roleDetails)
        //{
        //    try
        //    {
        //        Task.Run(async () => await _userContext.createRole(_roleDetails)).GetAwaiter().GetResult();

        //        return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));

        //    }
        //    catch (Exception)
        //    {
        //        throw;

        //    }
        //}

        [Authorize]
        [HttpPost("AppointeeConsentUpdate")]
        public ActionResult SubmitConsent(AppointeeConsentSubmitRequest consentRequest)
        {

            try
            {
                if (consentRequest == null)
                {
                    return Ok(new BaseResponse<string>(HttpStatusCode.BadRequest, "Error"));
                }
                else
                {
                    Task.Run(async () => await _userContext.updateAppointeeConsent(consentRequest)).GetAwaiter().GetResult();

                    return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("AppointeePrerequisiteUpdate")]
        public ActionResult SubmitPrerequisiteStatus(AppointeeConsentSubmitRequest consentRequest)
        {

            try
            {
                if (consentRequest == null)
                {
                    return Ok(new BaseResponse<string>(HttpStatusCode.BadRequest, "Error"));
                }
                else
                {
                    Task.Run(async () => await _userContext.updateAppointeePrerequisite(consentRequest)).GetAwaiter().GetResult();

                    return Ok(new BaseResponse<string>(HttpStatusCode.OK, "success"));

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("EncriptData")]
        public ActionResult EncriptData(string data)
        {
            try
            {
                string encriptData = CommonUtility.CustomEncryptString(key, data);

                return Ok(new BaseResponse<string>(HttpStatusCode.OK, encriptData));

            }
            catch (Exception)
            {
                throw;

            }
        }
        [AllowAnonymous]
        [HttpPost("DecriptData")]
        public ActionResult DecriptData(string data)
        {
            try
            {
                string decriptData = CommonUtility.DecryptString(key, data);

                return Ok(new BaseResponse<string>(HttpStatusCode.OK, decriptData));

            }
            catch (Exception)
            {
                throw;

            }
        }

    }
}
