using System.Net;
using System.Security.Claims;
using VERIDATA.BLL.Authentication;
using VERIDATA.BLL.Interfaces;
using VERIDATA.BLL.Notification.Provider;
using VERIDATA.BLL.utility;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.Model.Configuration;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.ExchangeModels;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Admin;
using VERIDATA.Model.Table.Public;
using static VERIDATA.BLL.utility.CommonEnum;

namespace VERIDATA.BLL.Context
{
    public class UserContext : IUserContext
    {
        private readonly IUserDalContext _userDalContext;
        private readonly IAppointeeDalContext _appointeeDalContext;
        private readonly IMasterDalContext _masterDalContext;
        private readonly IActivityDalContext _activityDalContext;
        private readonly ITokenAuth _tokenAuth;
        private readonly IEmailSender _emailSender;
        private readonly IWorkFlowDalContext _workflowDalContext;
        private readonly ApiConfiguration _configSetup;

        public UserContext(IEmailSender emailSender, IUserDalContext userDalContext, IAppointeeDalContext appointeeDalContext, IMasterDalContext masterDalContext,
            IActivityDalContext activityDalContext, IWorkFlowDalContext workflowDalContext, ITokenAuth tokenAuth, ApiConfiguration apiConfiguration)
        {

            _emailSender = emailSender;
            _userDalContext = userDalContext;
            _appointeeDalContext = appointeeDalContext;
            _masterDalContext = masterDalContext;
            _activityDalContext = activityDalContext;
            _tokenAuth = tokenAuth;
            _workflowDalContext = workflowDalContext;
            _configSetup = apiConfiguration;
        }
        public async Task<List<AppointeeBasicInfo>?> validateExistingUser(List<AppointeeBasicInfo> _appointeeList)
        {
            List<AppointeeBasicInfo>? exstinguserList = new();
            List<AppointeeBasicInfo> duplicateCheckUsersList = new();
            List<AppointeeBasicInfo> exstingNonProcessUserList = new();

            if (_appointeeList.Any())
            {
                duplicateCheckUsersList = _appointeeList;
                //duplicate checking in user data
                List<string?> candidateIdList = new();
                candidateIdList = await _userDalContext.GetAllCandidateId(CandidateIdType.All);
                if (candidateIdList.Any())
                {
                    exstinguserList = _appointeeList.Where(y => candidateIdList.Contains(y.CandidateID)).ToList();
                }
                if (exstinguserList?.Count() > 0)
                {
                    duplicateCheckUsersList = _appointeeList.Except(exstinguserList).ToList();

                }
                //duplicate checking in non process data

                List<string?> _nonProcessUserList = await _userDalContext.GetAllCandidateId(CandidateIdType.UnProcess);
                if (_nonProcessUserList.Any())
                {
                    exstingNonProcessUserList = duplicateCheckUsersList.Where(y => _nonProcessUserList.Contains(y.CandidateID)).ToList();
                }
                if (exstingNonProcessUserList?.Count() > 0)
                {
                    exstinguserList?.AddRange(exstingNonProcessUserList);
                    duplicateCheckUsersList = duplicateCheckUsersList.Except(exstingNonProcessUserList).ToList();
                }
                //duplicate checking in raw data
                List<string?> _rawUserList = await _userDalContext.GetAllCandidateId(CandidateIdType.Raw);
                if (_rawUserList.Any())
                {
                    List<AppointeeBasicInfo> exstingRawUserList = duplicateCheckUsersList.Where(y => _rawUserList.Contains(y.CandidateID)).ToList();
                    if (exstingRawUserList?.Count() > 0)
                    {
                        exstinguserList?.AddRange(exstingRawUserList);
                        duplicateCheckUsersList = duplicateCheckUsersList.Except(exstingRawUserList).ToList();
                    }
                }

                //duplicate checking in varified data

                List<string?> varifiedCandidateIdlist = await _userDalContext.GetAllCandidateId(CandidateIdType.Processed);
                if (varifiedCandidateIdlist.Any())
                {
                    List<AppointeeBasicInfo> exstingVarifiedUserList = duplicateCheckUsersList.Where(y => varifiedCandidateIdlist.Contains(y.CandidateID)).ToList();

                    if (exstingVarifiedUserList?.Count() > 0)
                    {
                        exstinguserList?.AddRange(exstingVarifiedUserList);
                    }
                }
            }
            return exstinguserList?.Distinct()?.ToList();
        }
        public async Task<List<UpdatedAppointeeBasicInfo>> updateExistingUser(List<UpdatedAppointeeBasicInfo> _appointeeList, int userId)
        {

            List<UpdatedAppointeeBasicInfo>? UpdateUsersList = new();

            if (_appointeeList.Any())
            {
                UpdateUsersList = _appointeeList;
                //updating in raw data
                List<string?> _rawUserList = await _userDalContext.GetAllCandidateId(CandidateIdType.Raw);
                if (_rawUserList.Any())
                {
                    List<UpdatedAppointeeBasicInfo> exstingRawUserList = _appointeeList.Where(y => _rawUserList.Contains(y.CandidateID)).ToList();
                    if (exstingRawUserList?.Count() > 0)
                    {
                        UpdateUsersList = UpdateUsersList?.Except(exstingRawUserList)?.ToList();
                        await updateCandidateDataStatusWise(exstingRawUserList, CandidateUpdateTableType.Raw, userId);
                    }
                }
                List<string?> _nonProcessUserList = await _userDalContext.GetAllCandidateId(CandidateIdType.UnProcess);
                if (_nonProcessUserList.Any())
                {
                    List<UpdatedAppointeeBasicInfo>? exstingNonProcessUserList = UpdateUsersList?.Where(y => _nonProcessUserList.Contains(y?.CandidateID))?.ToList();
                    if (exstingNonProcessUserList?.Count() > 0)
                    {
                        UpdateUsersList = UpdateUsersList?.Except(exstingNonProcessUserList)?.ToList();
                        await updateCandidateDataStatusWise(exstingNonProcessUserList, CandidateUpdateTableType.linknotsend, userId);
                    }
                }

                //updating in user data
                List<string?> activeCandidateIdList = await _userDalContext.GetAllCandidateId(CandidateIdType.UnderProcess);
                if (activeCandidateIdList.Any())
                {
                    List<UpdatedAppointeeBasicInfo>? underProcessUserList = UpdateUsersList?.Where(y => activeCandidateIdList.Contains(y.CandidateID))?.ToList();
                    if (underProcessUserList?.Count() > 0)
                    {
                        UpdateUsersList = UpdateUsersList?.Except(underProcessUserList)?.ToList();
                        await updateCandidateDataStatusWise(underProcessUserList, CandidateUpdateTableType.underProcess, userId);

                    }

                }
                List<UpdatedAppointeeBasicInfo>? updatedList = _appointeeList.Except(UpdateUsersList)?.ToList();
                await _userDalContext.PostAppointeeUpdateLog(updatedList, userId);

            }
            return UpdateUsersList;
        }
        private async Task updateCandidateDataStatusWise(List<UpdatedAppointeeBasicInfo> _appointeeList, string type, int userId)
        {
            List<string>? candidateIdList = _appointeeList?.Select(x => x.CandidateID).ToList();
            if (type == CandidateUpdateTableType.Raw && candidateIdList?.Count > 0)
            {
                await _userDalContext.UpdateRawCandidateData(_appointeeList, candidateIdList, userId);

            }
            if (type == CandidateUpdateTableType.linknotsend && candidateIdList?.Count > 0)
            {

                await _userDalContext.UpdateNonProcessCandidateData(_appointeeList, candidateIdList, userId);

            }
            if (type == CandidateUpdateTableType.underProcess && candidateIdList?.Count > 0)
            {
                await _userDalContext.UpdateUnderProcessCandidateData(_appointeeList, candidateIdList, userId);

            }
        }
        public async Task<List<UserDetailsResponse>> getAllAdminUser()
        {
            List<UserDetailsResponse> userList = await _userDalContext.getAllAdminUser();
            return userList;
        }
        public async Task<bool> validateUserByCode(string? userCode)
        {
            bool isValidUser = false;

            UserMaster? dbusers = await _userDalContext.getUserByUserCode(userCode);
            if (dbusers != null)
            {
                isValidUser = true;
            }

            return isValidUser;
        }
        public async Task<bool> validateUserById(int id)
        {
            bool isValidUser = false;

            UserMaster? dbusers = await _userDalContext.getUserByUserId(id);
            if (dbusers != null)
            {
                isValidUser = true;
            }

            return isValidUser;
        }
        public async Task<UserDetailsResponse> getUserDetailsAsyncbyId(int id)
        {
            _ = new UserDetailsResponse();
            UserDetailsResponse response = await _userDalContext.GetUserDetailsAsyncbyId(id);
            return response;
        }
        public async Task<bool> createNewUserwithRole(CreateUserDetailsRequest userdetails)
        {
            bool ValidateUser = await validateUserByCode(userdetails.UserCode);
            bool isCreateUser;
            if (!ValidateUser)
            {
                List<CreateUserDetailsRequest> userList = new()
                {
                    userdetails
                };
                await _userDalContext.createNewUserwithRole(userList, userdetails.UserId);
                isCreateUser = true;
            }
            else
            {
                isCreateUser = false;
            }
            return isCreateUser;
        }
        public async Task editAdminUser(AdminUserUpdateRequest userDetails)
        {
            await _userDalContext.updateAdminUser(userDetails);
        }
        public async Task<bool> removeUserDetails(int uid, int userId)
        {
            bool validateUser = await validateUserById(uid);

            if (validateUser)
            {
                return false;
            }
            await _userDalContext.deleteUserDetails(uid, userId);
            return true;
        }
        public async Task<ValidateUserDetails> validateUserSign(UserSignInRequest user)
        {
            string _otp = string.Empty;
            ValidateUserDetails validateUserResponse = await validateUserSignDataAsync(user);
            int _dbUserId = validateUserResponse.userStatus;
            validateUserResponse.otp = _otp;
            if (_dbUserId <= 0)
            {
                string _errormsg = _dbUserId switch
                {
                    0 => "User ID Password mismatch",
                    -1 => "User ID invalid",
                    -2 => "User ID expired",
                    -3 => "You have crossed your joining date with grace pireod",
                    -4 => "Your application has been cancelled by your Recruitment partner / Admin",
                    _ => string.Empty,
                };
                validateUserResponse.StatusCode = HttpStatusCode.NotFound;
                validateUserResponse.UserMessage = _errormsg;

                return validateUserResponse;
            }
            if (validateUserResponse.dbUserType == (int)UserType.Appoientee)
            {
                //generate otp ///
                //send mail//
                _otp = await candidateSigninGenerateOtp(validateUserResponse);
                if (string.IsNullOrEmpty(_otp))
                {
                    validateUserResponse.StatusCode = HttpStatusCode.NotFound;
                    validateUserResponse.UserMessage = "Email Address not available";
                    return validateUserResponse;
                }
                validateUserResponse.otp = _otp;

            }
            validateUserResponse.StatusCode = HttpStatusCode.OK;
            return validateUserResponse;
        }
        public async Task<ValidateUserSignInResponse> postUserAuthdetails(ValidateUserDetails req)
        {
            _ = new ValidateUserSignInResponse();
            UserAuthDetailsRequest authreq = new() { ClientId = req.clientId, UserId = req.userId, Otp = req.otp };
            await _userDalContext.postUserAuthDetailsAsyncbyId(authreq);
            ValidateUserSignInResponse res = new()
            {
                clientId = req.clientId,
                dbUserType = req.dbUserType,
                OTP = req.otp
            };
            return res;
        }
        public async Task postUsersignOut(int userId)
        {
            await _userDalContext.postUserSignOutDetailsAsyncbyId(userId);
        }
        private async Task<ValidateUserDetails> validateUserSignDataAsync(UserSignInRequest user)
        {
            ValidateUserDetails res = new();
            _ = CommonUtility.hashPassword(password: user.Password);
            int dbuserStatusId = -1;
            int dbuserTypeId = -1;
            GeneralSetup GeneralSetup = await _masterDalContext.GetGeneralSetupData();

            UserMaster? dbusers = await _userDalContext.getUserByUserCode(user.UserCode);

            if (dbusers != null)
            {
                dbuserTypeId = dbusers.UserTypeId;
                dbuserStatusId = (!(dbusers.ActiveStatus ?? false)) ? -2 : !(dbusers.CurrStatus ?? false) ? -4 : dbuserStatusId;
                if (dbuserStatusId == -1)
                {
                    UserAuthentication? _authenticatedUserId = await _userDalContext.getAuthUserDetailsByPassword(dbusers.UserId, user.Password);

                    dbuserStatusId = _authenticatedUserId == null ? 0 : dbusers.UserId;
                    if (dbusers.RefAppointeeId != null)
                    {
                        UnderProcessFileData _userDetails = await _appointeeDalContext.GetUnderProcessAppinteeDetailsById(dbusers.RefAppointeeId ?? 0);
                        if (_userDetails.DateOfJoining?.AddDays(GeneralSetup?.GracePeriod ?? 0) < DateTime.Now)
                        {
                            dbuserStatusId = -3;
                        }
                    }
                }
            }

            res.clientId = string.Concat(user.UserCode, "_", CommonUtility.RandomString(8));
            res.dbUserType = dbuserTypeId;
            res.userStatus = dbuserStatusId;
            res.userMailId = dbusers?.EmailId;
            res.userName = dbusers?.UserName;
            res.userId = dbusers?.UserId ?? 0;
            return res;
        }
        private async Task<string> candidateSigninGenerateOtp(ValidateUserDetails user)
        {
            string otp = string.Empty;
            if (!string.IsNullOrEmpty(user.userMailId))
            {
                otp = CommonUtility.RandomNumber(6);
                MailDetails mailDetails = new();
                MailBodyParseDataDetails bodyDetails = new()
                {
                    Name = user.userName,
                    Otp = otp,
                };
                mailDetails.MailType = MailType.SendOTP;
                mailDetails.ParseData = bodyDetails;
                await _emailSender.SendAppointeeMail(user.userMailId, mailDetails);

            }
            return otp;
        }
        public async Task<bool> validateProfilePasswowrdAsync(ValidateProfilePasswordRequest req)
        {
            //string dycriptPassword = CommonUtility.DecryptString(_configSetup.EncriptKey, req.ProfilePassword);
            string _password = CommonUtility.hashPassword(password: req.ProfilePassword);

            UserAuthentication _authenticatedPassword = await _userDalContext.getAuthUserDetailsById(req.UserId);
            bool Isvalidate = _authenticatedPassword?.UserProfilePwd == _password;
            return Isvalidate;
        }
        public async Task<int> validateUserByOtp(string? clientId, string? otp, int userType)
        {
            UserAuthenticationHist? dbusers = await _userDalContext.getAuthHistUserDetailsByClientId(clientId);

            int _userId = userType == (int)UserType.Appoientee ? dbusers?.Otp == otp ? dbusers.UserId : 0 : dbusers?.UserId ?? -1;
            return _userId;
        }
        public async Task<AuthenticatedUserResponse> getValidatedSigninUserDetails(int userId)
        {
            AuthenticatedUserResponse authUsertDetails = new()
            {
                UserDetails = new UserDetailsResponse(),
                TokenDetails = new TokenDetailsResponse(),
            };
            TokenDetailsResponse tokenDetails = new();
            UserDetailsResponse? userDetails = await getUserDetailsAsyncbyId(userId);

            if (userDetails?.UserId != null)
            {
                string token = _tokenAuth.createToken(userDetails.UserCode, userDetails.RoleName, userId);
                string refreshToken = _tokenAuth.GenerateRefreshToken();

                tokenDetails.Token = token;
                tokenDetails.RefreshToken = refreshToken;
                await _userDalContext.postUserTokenDetailsAsyncbyId(userId, refreshToken);
                if (userDetails?.AppointeeId != null)
                {
                    await _activityDalContext.PostActivityDetails(userDetails?.AppointeeId ?? 0, userId, ActivityLog.LOGIN);
                }
            }
            authUsertDetails.UserDetails = userDetails;
            authUsertDetails.TokenDetails = tokenDetails;
            return authUsertDetails;
        }
        public async Task<TokenDetailsResponse> getRefreshToken(RefreshTokenRequest reqObj)
        {
            TokenDetailsResponse response = new();
            string accessToken = reqObj.Token;
            string refreshToken = reqObj.RefreshToken;
            response.IsValid = false;
            var principal = _tokenAuth.GetPrincipalFromExpiredToken(accessToken);
            //var username = principal.Identity.Name; //this is mapped to the Name claim by default
            if (principal != null)
            {
                IEnumerable<Claim> claims = principal.Claims;
                var userIdClaim = claims?.Where(x => x.Type == "userId")?.FirstOrDefault()?.Value;
                if (!string.IsNullOrEmpty(userIdClaim))
                {
                    int userId = Convert.ToInt32(userIdClaim);

                    UserAuthenticationHist? user = await _userDalContext.getAuthHistUserDetailsById(userId);
                    if (!(user == null || user.TokenNo != refreshToken) || user.RefreshTokenExpiryTime <= DateTime.Now)
                    {

                        var newAccessToken = _tokenAuth.GenerateAccessToken(principal.Claims);
                        var newRefreshToken = _tokenAuth.GenerateRefreshToken();

                        await _userDalContext.postUserTokenDetailsAsyncbyId(userId, newRefreshToken);
                        response.Token = newAccessToken;
                        response.RefreshToken = newRefreshToken;
                        response.IsValid = true;
                    }
                }
            }

            return response;
        }

        public async Task editUserProfile(EditUserProfileRequest req)
        {
            await _userDalContext.editUserProfile(req);

        }

        public async Task<List<MenuNodeResponse>> GetMenuData(int userid)
        {
            List<MenuNodeResponse> data = await GetMenuLeafNodeList(userid);
            List<MenuNodeResponse> _data = data.DistinctBy(x => x.Id).OrderBy(x => x.Id).ToList();
            List<MenuNodeResponse>? mainMenuLst = _data.Where(x => x.Pid == 0 && x.Level == 1)?.ToList();


            return mainMenuLst;
        }
        private async Task<List<MenuNodeResponse>> GetMenuLeafNodeList(int userid)
        {
            RoleDetailsResponse userRole = await _userDalContext.GetUserRole(userid);
            List<MenuNodeDetails> menudata = await _userDalContext.GetMenuLeafNodeList(userRole.RoleId);

            List<MenuNodeResponse> data = new();
            menudata?.ToList().ForEach(x =>
            {
                List<MenuNodeDetails> _OptActions = menudata.Where(ch => (ch.MenuId == x.MenuId) && (ch.ActionId > 0)).DistinctBy(x => x.ActionId).ToList();
                List<MenuNodeResponse>? _curActions = _OptActions?.Select((xa) => new MenuNodeResponse
                {
                    OprnActionId = xa?.ActionId ?? 0,
                    OprnActionAlias = xa?.ActionAlias ?? string.Empty,
                    OprnActionName = xa?.ActionName ?? string.Empty
                })?.ToList();
                bool _isQuickMenu = x.MenuLevel == 1 && string.IsNullOrEmpty(x.ActionUrl);
                data.Add(new MenuNodeResponse()
                {
                    IsMenu = x.MenuLevel == 1,
                    IsQuickMenu = _isQuickMenu,
                    Id = x.MenuId,
                    Pid = x.ParentMenuId,
                    Level = x.MenuLevel,
                    ActionUrl = x?.ActionUrl ?? string.Empty,
                    DisplayName = x?.MenuTitle ?? string.Empty,
                    IconClass = x?.IconClass ?? string.Empty,
                    NodeTitle = x?.MenuDesc ?? string.Empty,
                    SeqNo = x?.SeqNo,
                    OprnActionId = x?.ActionId ?? 0,
                    OprnActionAlias = x?.ActionAlias ?? string.Empty,
                    OprnActionName = x?.ActionName ?? string.Empty,
                    OptActions = !_isQuickMenu ? _curActions : null,
                    Children = null,
                });
            });

            data.ForEach(i =>
            {
                i.Children = data.Where(ch => ch.Pid == i.Id).DistinctBy(x => x.Id).ToList();
            });
            //data = data.Where(x => x.Level == 1).ToList();
            return data;
        }
        private List<MenuNodeResponse> GenerateNestedMenuNodes(List<MenuNodeResponse> menus)
        {
            List<MenuNodeResponse>? uniqueMenus = menus.Where(x => x.IsMenu).DistinctBy(x => x.Id)?.ToList();
            List<MenuNodeResponse> result = new();

            uniqueMenus.ForEachWithIndex((x, i) =>
            {
                if (x.Children != null && x.Children.Any(y => y.IsMenu))
                {
                    x.Children = GenerateNestedMenuNodes(x.Children);
                }

                result.Add(x);
            });

            return result;
        }
        private List<MenuNodeResponse> findChild(List<MenuNodeResponse> lst, int chldlevel, int pid)
        {
            List<MenuNodeResponse>? chldLst = new();
            lst?.ForEach(x =>
            {
                if (x.Pid == pid && x.Level == chldlevel)
                {
                    x.Children = findChild(lst, x.Level + 1, x.Id);
                    chldLst.Add(x);

                }
            });
            chldLst = chldLst.Any() ? chldLst : null;
            return chldLst;
        }

        public async Task<List<DropDownDetailsResponse>> getDropDownData(string type)
        {
            List<DropDownDetailsResponse> _dataList = new();

            switch (type)
            {
                case CommonEnum.MasterDataType.COUNTRY:
                    _dataList = await _masterDalContext.getCountryDataAsync();
                    break;

                case CommonEnum.MasterDataType.NATIONALITY:
                    _dataList = await _masterDalContext.getNationilityDataAsync();
                    break;

                case CommonEnum.MasterDataType.GENDER:
                    _dataList = await _masterDalContext.getGenderDataAsync();
                    break;
                case CommonEnum.MasterDataType.MARATIALSTAT:
                    _dataList = await _masterDalContext.getMaratialStatusDataAsync();
                    break;
                case CommonEnum.MasterDataType.DISABILITY:
                    _dataList = await _masterDalContext.getDisabilityDataAsync();
                    break;
                case CommonEnum.MasterDataType.FILETYPE:
                    _dataList = await _masterDalContext.getFileTypeDataAsync();
                    break;
                case CommonEnum.MasterDataType.QUALIFICATION:
                    _dataList = await _masterDalContext.getQualificationDataAsync();
                    break;
                case CommonEnum.MasterDataType.ROLE:
                    _dataList = await _masterDalContext.getUserRoleAsync();
                    break;
                default:
                    return _dataList;

            }

            return _dataList;
        }

        public async Task<List<DashboardWidgetResponse>> GetWidgetData(int? filterDays, bool isfilterd)
        {
            List<DashboardWidgetResponse> Wizdata = new();
            int filterDaysrange = filterDays ?? 0;

            WidgetTotalOfferDetails _totaloffer = await GetTotalOffer(filterDaysrange);
            List<int?>? AppointeeListData = _totaloffer.AppointeeList?.Select(x => x.AppointeeId)?.ToList();
            DashboardWidgetResponse _varifieddata = await GetVerifiedData(filterDaysrange, isfilterd, AppointeeListData);
            List<UnderProcessDetailsResponse> _underProcessedAppointeeListData = await GetUnderProcessedAppointeeList(filterDaysrange, AppointeeListData);
            DashboardWidgetResponse _noResponsedata = GetNoResponsedData(filterDaysrange, _underProcessedAppointeeListData);
            DashboardWidgetResponse _underProcessdata = GetUnderProcessedData(filterDaysrange, _underProcessedAppointeeListData);
            DashboardWidgetResponse _lapsedData = GetLapsedData(filterDaysrange, _underProcessedAppointeeListData);
            DashboardWidgetResponse _lonkNotSentdata = await GetLinkNotSentData(filterDaysrange);
            Wizdata.Add(_totaloffer.WizResData);
            Wizdata.Add(_varifieddata);
            Wizdata.Add(_noResponsedata);
            Wizdata.Add(_underProcessdata);
            Wizdata.Add(_lapsedData);
            Wizdata.Add(_lonkNotSentdata);
            return Wizdata;
        }
        private async Task<WidgetTotalOfferDetails> GetTotalOffer(int filterDays)
        {
            WidgetTotalOfferDetails Response = new();
            DashboardWidgetResponse wizResponse = new();
            int filterDaysrange = filterDays;
            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime? startDate = filterDays > 0 ? _currDate.AddDays(-filterDaysrange) : null;
            DashboardCardWidgetResponse TotalOffer = new()
            {
                WidgetTypeName = "Total Offers in this period",
                WidgetFilterDays = filterDays
            };

            List<WorkFlowDetailsHist> workfdata = await _workflowDalContext.GetTotalDataAsync(startDate);
            List<WorkFlowDetailsHist>? appinteeList = workfdata?.DistinctBy(x => x.AppointeeId).ToList();
            var groupdateValue = appinteeList?.GroupBy(x => Convert.ToDateTime(x.CreatedOn?.ToString("dd/MM/yyyy")))?.Select(x => new
            {
                Value = x.Count(),
                Date = x.Key
            })?.ToList();
            AppointeeSeacrhFilterRequest reqObj = new() { IsFiltered = true, NoOfDays = filterDays };
            List<UnProcessedFileData> unProcesseddata = await _workflowDalContext.GetUnProcessDataAsync(reqObj);

            var groupunprocessdateValue = unProcesseddata?.GroupBy(x => Convert.ToDateTime(x.CreatedOn?.ToString("dd/MM/yyyy")))?.Select(x => new
            {
                Value = x.Count(),
                Date = x.Key
            })?.ToList();
            List<int> ChartData = new();
            if (startDate != null)
            {
                for (int i = 0; i < filterDaysrange; i++)
                {
                    DateTime? tempcurDate = startDate?.AddDays(i);
                    var _currgropdate = groupdateValue.Where(x => x.Date.Equals(tempcurDate));
                    var _currunprocessgropdate = groupunprocessdateValue.Where(x => x.Date.Equals(tempcurDate));
                    int chartvalLinksent = _currgropdate?.FirstOrDefault()?.Value ?? 0;
                    int chartvalLinkNotSent = _currunprocessgropdate?.FirstOrDefault()?.Value ?? 0;
                    int chartval = chartvalLinksent + chartvalLinkNotSent;
                    ChartData.Add(chartval);
                }
            }
            TotalOffer.WidgetTypeValue = (appinteeList?.Count() ?? 0) + (unProcesseddata?.Count() ?? 0);
            TotalOffer.WidgetChartValue = ChartData;
            wizResponse.WidgetTypeCode = "TOTLOFFR";
            wizResponse.WidgetValue = TotalOffer;
            Response.WizResData = wizResponse;
            Response.AppointeeList = appinteeList;
            return Response;
        }
        private async Task<DashboardWidgetResponse> GetVerifiedData(int filterDays, bool isfilterd, List<int?> appointeeList)
        {
            DashboardWidgetResponse Response = new();

            int filterDaysrange = filterDays;
            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime? startDate = isfilterd && filterDays > 0 ? _currDate.AddDays(-filterDaysrange) : null;
            DashboardCardWidgetResponse VerifiedData = new()
            {
                WidgetTypeName = "Verified",

                WidgetFilterDays = filterDays
            };

            ProcessedFilterRequest processedFilterRequest = new() { IsFiltered = true, FromDate = startDate };

            List<ProcessedDataDetailsResponse> list = await _workflowDalContext.GetProcessedAppointeeDetailsAsync(processedFilterRequest);

            List<AppointeeDetails>? data = list.Where(x => appointeeList.Contains(x.AppointeeId)).DistinctBy(x => x.AppointeeId)?.Select(x => x.AppointeeData).ToList();


            VerifiedData.WidgetTypeValue = data?.Count ?? 0;

            var groupdateValue = data?.GroupBy(x => Convert.ToDateTime(x.CreatedOn?.ToString("dd/MM/yyyy")))?.Select(x => new
            {
                Value = x.Count(),
                Date = x.Key
            })?.ToList();

            List<int> ChartData = new();
            if (startDate != null)
            {
                for (int i = 0; i < filterDaysrange; i++)
                {
                    DateTime? tempcurDate = startDate?.AddDays(i);
                    var _currgropdate = groupdateValue.Where(x => x.Date.Equals(tempcurDate));
                    int chartval = _currgropdate.Any() ? _currgropdate.FirstOrDefault().Value : 0;
                    ChartData.Add(chartval);
                }
            }
            VerifiedData.WidgetChartValue = ChartData;

            Response.WidgetTypeCode = "VIRFD";
            Response.WidgetValue = VerifiedData;
            return Response;
        }
        private async Task<List<UnderProcessDetailsResponse>> GetUnderProcessedAppointeeList(int filterDays, List<int?> appointeeList)
        {
            int filterDaysrange = filterDays;
            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime CurrDate = Convert.ToDateTime(_currDate);
            DateTime? startDate = filterDays > 0 ? _currDate.AddDays(-filterDaysrange) : null;

            AppointeeSeacrhFilterRequest reqObj = new() { FromDate = startDate };
            List<UnderProcessQueryDataResponse> underProcessAppointeeList = await _workflowDalContext.GetUnderProcessDataAsync(reqObj);


            List<UnderProcessDetailsResponse>? _underProcessViewdata = underProcessAppointeeList?.DistinctBy(x => x.AppointeeId).Where(x => appointeeList.Contains(x.AppointeeId)).Select(row => new UnderProcessDetailsResponse
            {
                id = row.UnderProcess.UnderProcessId,
                fileId = row.UnderProcess.FileId,
                companyId = row.UnderProcess.CompanyId,
                appointeeName = row.AppointeeDetails?.AppointeeName ?? row.UnderProcess.AppointeeName,
                appointeeId = row.UnderProcess.AppointeeId,
                appointeeEmailId = row.AppointeeDetails?.AppointeeEmailId ?? row.UnderProcess.AppointeeEmailId,
                mobileNo = row.UnderProcess.MobileNo,
                isPFverificationReq = row.UnderProcess.IsPFverificationReq,
                epfWages = row.AppointeeDetails?.EPFWages ?? row.UnderProcess.EPFWages,
                dateOfOffer = row.UnderProcess.DateOfOffer,
                dateOfJoining = row.AppointeeDetails?.DateOfJoining ?? row.UnderProcess.DateOfJoining,
                isDocSubmitted = row.AppointeeDetails?.IsSubmit ?? false,
                isReprocess = false,
                Status = row.AppointeeDetails?.IsSubmit ?? false ? "Ongoing" : row.AppointeeDetails?.SaveStep == 1 ? "Ongoing" : "No Response",
                StatusCode = row.AppointeeDetails?.IsSubmit ?? false ? 2 : row.AppointeeDetails?.SaveStep ?? 0,
                ConsentStatusCode = row.ConsentStatusId ?? 0,
                CreatedDate = row.UnderProcess?.CreatedOn
            }).OrderByDescending(x => x.isDocSubmitted).ThenBy(y => y.dateOfJoining).ToList();


            return _underProcessViewdata;
        }
        private DashboardWidgetResponse GetNoResponsedData(int filterDays, List<UnderProcessDetailsResponse> appointeeList)
        {
            DashboardWidgetResponse Response = new();
            DashboardCardWidgetResponse UnderProcessedData = new();
            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime? startDate = filterDays > 0 ? _currDate.AddDays(-filterDays) : null;
            UnderProcessedData.WidgetTypeName = "No Response";

            UnderProcessedData.WidgetFilterDays = filterDays;

            List<UnderProcessDetailsResponse>? data = appointeeList.Where(x => x.StatusCode == 0 && x.dateOfJoining > _currDate).ToList();

            UnderProcessedData.WidgetTypeValue = data?.Count() ?? 0;

            var groupdateValue = data.GroupBy(x => Convert.ToDateTime(x.CreatedDate?.ToString("dd/MM/yyyy")))?.Select(x => new
            {
                Value = x.Count(),
                Date = x.Key
            })?.ToList();

            List<int> ChartData = new();
            if (startDate != null)
            {
                for (int i = 0; i < filterDays; i++)
                {
                    DateTime? tempcurDate = startDate?.AddDays(i);
                    var _currgropdate = groupdateValue.Where(x => x.Date.Equals(tempcurDate));
                    int chartval = _currgropdate.Any() ? _currgropdate.FirstOrDefault().Value : 0;
                    ChartData.Add(chartval);
                }
            }
            UnderProcessedData.WidgetChartValue = ChartData;
            Response.WidgetTypeCode = "NORES";
            Response.WidgetValue = UnderProcessedData;
            return Response;
        }
        private DashboardWidgetResponse GetUnderProcessedData(int filterDays, List<UnderProcessDetailsResponse> appointeeList)
        {
            DashboardWidgetResponse Response = new();
            int filterDaysrange = filterDays;
            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime? startDate = filterDays > 0 ? _currDate.AddDays(-filterDaysrange) : null;
            DashboardCardWidgetResponse UnderProcessedData = new()
            {
                WidgetTypeName = "Processing",

                WidgetFilterDays = filterDays
            };

            List<UnderProcessDetailsResponse>? data = appointeeList.Where(m => m.StatusCode != 0 && m.dateOfJoining >= _currDate).ToList();

            UnderProcessedData.WidgetTypeValue = data?.Count() ?? 0;

            var groupdateValue = data.GroupBy(x => Convert.ToDateTime(x.CreatedDate?.ToString("dd/MM/yyyy")))?.Select(x => new
            {
                Value = x.Count(),
                Date = x.Key
            })?.ToList();

            List<int> ChartData = new();
            if (startDate != null)
            {
                for (int i = 0; i < filterDaysrange; i++)
                {
                    DateTime? tempcurDate = startDate?.AddDays(i);
                    var _currgropdate = groupdateValue?.Where(x => x.Date.Equals(tempcurDate));
                    int chartval = _currgropdate.Any() ? _currgropdate.FirstOrDefault().Value : 0;
                    ChartData.Add(chartval);
                }
            }
            UnderProcessedData.WidgetChartValue = ChartData;
            Response.WidgetTypeCode = "UNDPRCS";
            Response.WidgetValue = UnderProcessedData;
            return Response;
        }
        private static DashboardWidgetResponse GetLapsedData(int filterDays, List<UnderProcessDetailsResponse> appointeeList)
        {
            DashboardWidgetResponse Response = new();
            DashboardCardWidgetResponse UnderProcessedData = new();
            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime? startDate = filterDays > 0 ? _currDate.AddDays(-filterDays) : null;
            UnderProcessedData.WidgetTypeName = "Lapsed";

            UnderProcessedData.WidgetFilterDays = filterDays;

            List<UnderProcessDetailsResponse>? data = appointeeList.Where(x => x.dateOfJoining < _currDate).ToList();

            UnderProcessedData.WidgetTypeValue = data?.Count() ?? 0;

            var groupdateValue = data?.GroupBy(x => Convert.ToDateTime(x.CreatedDate?.ToString("dd/MM/yyyy")))?.Select(x => new
            {
                Value = x.Count(),
                Date = x.Key
            })?.ToList();

            List<int> ChartData = new();
            if (startDate != null)
            {
                for (int i = 0; i < filterDays; i++)
                {
                    DateTime? tempcurDate = startDate?.AddDays(i);
                    var _currgropdate = groupdateValue.Where(x => x.Date.Equals(tempcurDate));
                    int chartval = _currgropdate.Any() ? _currgropdate.FirstOrDefault().Value : 0;
                    ChartData.Add(chartval);
                }
            }
            UnderProcessedData.WidgetChartValue = ChartData;
            Response.WidgetTypeCode = "LAPSED";
            Response.WidgetValue = UnderProcessedData;
            return Response;
        }
        private async Task<DashboardWidgetResponse> GetLinkNotSentData(int filterDays)
        {
            DashboardWidgetResponse Response = new();
            int filterDaysrange = filterDays;
            DateTime _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime? startDate = filterDays > 0 ? _currDate.AddDays(-filterDays) : null;
            DashboardCardWidgetResponse LinkNotSentData = new()
            {
                WidgetTypeName = "Link Not Sent",
                WidgetFilterDays = filterDays
            };

            AppointeeSeacrhFilterRequest reqObj = new() { IsFiltered = true, NoOfDays = filterDays };

            List<UnProcessedFileData>? data = await _workflowDalContext.GetUnProcessDataAsync(reqObj);

            LinkNotSentData.WidgetTypeValue = data?.Count() ?? 0;

            var groupdateValue = data.GroupBy(x => Convert.ToDateTime(x.CreatedOn?.ToString("dd/MM/yyyy")))?.Select(x => new
            {
                Value = x.Count(),
                Date = x.Key
            })?.ToList();


            List<int> ChartData = new();
            if (startDate != null)
            {
                for (int i = 0; i < filterDaysrange; i++)
                {
                    DateTime? tempcurDate = startDate?.AddDays(i);
                    var _currgropdate = groupdateValue.Where(x => x.Date.Equals(tempcurDate));
                    int chartval = _currgropdate.Any() ? _currgropdate.FirstOrDefault().Value : 0;
                    ChartData.Add(chartval);
                }
            }
            LinkNotSentData.WidgetChartValue = ChartData;
            Response.WidgetTypeCode = "NTVRFD";
            Response.WidgetValue = LinkNotSentData;
            return Response;
        }
        public async Task<CriticalAppointeeWidgetResponse> GetCriticalData()
        {
            CriticalAppointeeWidgetResponse returnObj = new();
            int criticaldata = 0;
            string currDate = DateTime.Now.ToShortDateString();
            DateTime _currDate = Convert.ToDateTime(currDate);

            GeneralSetup generalsetupData = await _masterDalContext.GetGeneralSetupData();
            int filterDaysrange = generalsetupData?.CriticalNoOfDays ?? 0;
            DateTime maxDate = _currDate.AddDays(filterDaysrange);

            List<UnderProcessQueryDataResponse> criticalUnderProcessList = await _workflowDalContext.GetUnderProcessDataByDOJAsync(_currDate, maxDate, null, null);
            List<UnderProcessQueryDataResponse>? underProcessData = criticalUnderProcessList.OrderByDescending(x => x.UnderProcess.DateOfJoining).ToList();

            List<UnProcessedFileData>? nonProcessData = await _workflowDalContext.GetCriticalUnProcessDataAsync(_currDate, maxDate, null, null);
            criticaldata = (underProcessData?.Count ?? 0) + (nonProcessData?.Count ?? 0);
            returnObj.TotalCriticalAppointee = criticaldata;
            returnObj.UnderProcessCriticalAppointee = underProcessData?.Count ?? 0;
            returnObj.NonProcessCriticalAppointee = nonProcessData?.Count ?? 0;
            returnObj.CriticalDaysNo = filterDaysrange;
            return returnObj;
        }
        public async Task<WidgetProgressDataResponse> GetTotalProgressWidgetData()
        {
            WidgetProgressDataResponse widgetData = new();
            int totalAppointee = 0;
            int totalUnderProcess = 0;
            int totalProcess = 0;
            int totalReject = 0;
            int totalNonProcess = 0;

            List<RawFileData> rawdata = await _workflowDalContext.GetRawfiledataAsync();
            List<string?>? _rawdata = rawdata?.Select(x => x.CandidateId)?.Distinct()?.ToList();

            AppointeeSeacrhFilterRequest req = new();

            List<UnProcessedFileData> nonProcess = await _workflowDalContext.GetUnProcessDataAsync(req);

            List<string?>? _nonProcess = nonProcess?.DistinctBy(x => x.CandidateId)?.Select(x => x.CandidateId)?.ToList();

            List<RejectedFileData> rejectedData = await _workflowDalContext.GetRejectedDataAsync(null);

            List<int?>? _totalReject = rejectedData?.Select(x => x.AppointeeId).ToList();
            AppointeeSeacrhFilterRequest reqObj = new();
            List<UnderProcessQueryDataResponse> underProcessDataList = await _workflowDalContext.GetUnderProcessDataAsync(reqObj);

            List<UnderProcessFileData?>? _underProcess = underProcessDataList?.DistinctBy(x => x.AppointeeId)?.Select(x => x.UnderProcess)?.ToList();
            ProcessedFilterRequest processReqObj = new();
            List<ProcessedDataDetailsResponse> _totalProcessData = await _workflowDalContext.GetProcessedAppointeeDetailsAsync(processReqObj);

            List<int?>? _totalProcess = _totalProcessData?.DistinctBy(x => x.AppointeeId)?.Select(x => x.AppointeeId)?.ToList();

            totalUnderProcess = _underProcess?.Distinct()?.ToList()?.Count() ?? 0;
            totalNonProcess = _nonProcess?.Distinct()?.ToList()?.Count() ?? 0;
            totalProcess = _totalProcess?.Distinct()?.ToList()?.Count() ?? 0;
            totalReject = _totalReject?.Distinct()?.ToList()?.Count() ?? 0;
            int totalrawdata = _rawdata?.Distinct()?.ToList()?.Count() ?? 0;
            totalAppointee = totalUnderProcess + totalNonProcess + totalProcess + totalReject;

            widgetData.TotalProcess = totalProcess;
            widgetData.TotalReject = totalReject;
            widgetData.TotalUnderProcess = totalUnderProcess;
            widgetData.TotalNonProcess = totalNonProcess;
            widgetData.TotalAppointee = totalAppointee;
            return widgetData;
        }

        public async Task<List<AppointeeStatusWizResponse>> GetAppointeeStatusWidgetData(string code)
        {
            List<AppointeeStatusWizResponse> wizdata = new();
            switch (code)
            {
                case FilterCode.UNDERPROCESS:
                    wizdata = await GeTop5tUnderProcessDataAsync(5);
                    break;
                case FilterCode.LINKNOTSENT:
                    wizdata = await GetTop5NonProcessDataAsync(5);
                    break;
                case FilterCode.VERIFIED:
                    wizdata = await GetTop5ProcessDataAsync(5);
                    break;
                case FilterCode.REJECTED:
                    wizdata = await GetTop5RejectedDataAsync(5);
                    break;
                case FilterCode.LAPSED:
                    wizdata = await GetTop5LapsedDataAsync(5);
                    break;

            }

            return wizdata;
        }
        private async Task<List<AppointeeStatusWizResponse>> GeTop5tUnderProcessDataAsync(int numbers)
        {
            string currDate = DateTime.Now.ToShortDateString();
            DateTime _currDate = Convert.ToDateTime(currDate);

            List<UnderProcessQueryDataResponse> underProcessListFilteredByDoj = await _workflowDalContext.GetUnderProcessDataByDOJAsync(_currDate, null, null, null);

            List<UnderProcessQueryDataResponse>? distinctdata = underProcessListFilteredByDoj?.DistinctBy(x => x.AppointeeId)?.Take(numbers)?.ToList();
            List<AppointeeStatusWizResponse>? _underProcessViewdata = distinctdata?.Select(row => new AppointeeStatusWizResponse
            {
                id = row?.UnderProcess?.UnderProcessId ?? 0,
                fileId = row?.UnderProcess?.FileId ?? 0,
                companyId = row.UnderProcess.CompanyId,
                appointeeName = row?.AppointeeDetails?.AppointeeName ?? row?.UnderProcess?.AppointeeName,
                appointeeId = row.UnderProcess.AppointeeId,
                appointeeEmailId = row?.AppointeeDetails?.AppointeeEmailId ?? row?.UnderProcess?.AppointeeEmailId,
                mobileNo = row?.UnderProcess?.MobileNo,
                IsReprocess = false,
                dateOfJoining = row?.AppointeeDetails?.DateOfJoining ?? row?.UnderProcess?.DateOfJoining,
                Status = row?.AppointeeDetails?.IsSubmit ?? false ? "Ongoing" : row?.AppointeeDetails?.SaveStep == 1 ? "Ongoing" : "No Response",
                StatusCode = row?.AppointeeDetails?.IsSubmit ?? false ? 2 : row?.AppointeeDetails?.SaveStep == 1 ? 2 : 1,
                CreatedDate = row?.UnderProcess?.CreatedOn
            })?.ToList();


            return _underProcessViewdata;
        }
        private async Task<List<AppointeeStatusWizResponse>> GetTop5NonProcessDataAsync(int numbers)
        {
            AppointeeSeacrhFilterRequest req = new();
            List<UnProcessedFileData> unProcessData = await _workflowDalContext.GetUnProcessDataAsync(req);
            List<UnProcessedFileData> _unProcessData = unProcessData.OrderByDescending(x => x.DateOfJoining).Take(numbers).ToList();

            List<AppointeeStatusWizResponse>? _unProcessViewdata = _unProcessData.Select(row => new AppointeeStatusWizResponse
            {
                id = row.UnProcessedId,
                fileId = row.FileId,
                companyId = row.CompanyId,
                appointeeName = row.AppointeeName,
                appointeeId = 0,
                appointeeEmailId = row.AppointeeEmailId,
                mobileNo = row.MobileNo,
                IsReprocess = false,
                dateOfJoining = row.DateOfJoining,
                Status = "Mail Not sent",
                StatusCode = 0,
                CreatedDate = row.CreatedOn
            })?.ToList();


            return _unProcessViewdata;
        }
        private async Task<List<AppointeeStatusWizResponse>> GetTop5ProcessDataAsync(int numbers)
        {
            ProcessedFilterRequest processedFilterRequest = new();
            List<ProcessedDataDetailsResponse> listData = await _workflowDalContext.GetProcessedAppointeeDetailsAsync(processedFilterRequest);
            List<ProcessedDataDetailsResponse>? list = listData?.OrderByDescending(x => x.AppointeeData.DateOfJoining)?.DistinctBy(x => x.AppointeeId).Take(numbers).ToList();

            List<AppointeeStatusWizResponse>? _processViewdata = list?.Select(row => new AppointeeStatusWizResponse
            {
                id = row?.ProcessedId ?? 0,
                companyId = row?.CompanyId ?? 0,
                appointeeName = row?.AppointeeName,
                appointeeId = row?.AppointeeId,
                appointeeEmailId = row?.AppointeeEmailId,
                mobileNo = row?.MobileNo,
                dateOfJoining = row?.DateOfJoining,
                IsReprocess = false,
                Status = "Verified",
                StatusCode = 3,
            })?.ToList();


            return _processViewdata;
        }
        private async Task<List<AppointeeStatusWizResponse>> GetTop5RejectedDataAsync(int numbers)
        {
            FilterRequest filterRequest = new();
            List<RejectedDataDetailsResponse> rejectedListData = await _workflowDalContext.GetRejectedAppointeeDetailsAsync(filterRequest);
            List<RejectedDataDetailsResponse>? distinctdata = rejectedListData?.OrderByDescending(x => x.DateOfJoining)?.DistinctBy(x => x.AppointeeId).Take(numbers).ToList();
            List<AppointeeStatusWizResponse>? _rejectedViewdata = distinctdata?.Select(row => new AppointeeStatusWizResponse
            {
                id = row?.RejectedId ?? 0,
                companyId = row?.CompanyId ?? 0,
                appointeeName = row?.AppointeeName,
                appointeeId = row?.AppointeeId,
                appointeeEmailId = row?.AppointeeEmailId,
                mobileNo = row?.MobileNo,
                dateOfJoining = row?.DateOfJoining,
                IsReprocess = false,
                Status = "Cancelled",
                StatusCode = 4,
            })?.ToList();

            return _rejectedViewdata;
        }
        private async Task<List<AppointeeStatusWizResponse>> GetTop5LapsedDataAsync(int numbers)
        {
            string currDate = DateTime.Now.ToShortDateString();
            DateTime _currDate = Convert.ToDateTime(currDate);

            List<UnderProcessQueryDataResponse> lapsedListFilteredByDoj = await _workflowDalContext.GetUnderProcessDataByDOJAsync(null, _currDate, null, null);

            List<UnderProcessQueryDataResponse>? LapsedData = lapsedListFilteredByDoj?.DistinctBy(y => y.AppointeeId)?.OrderByDescending(x => x.UnderProcess?.DateOfJoining)?.Take(numbers).ToList();
            List<AppointeeStatusWizResponse>? _lapsedViewdata = LapsedData?.Select(row => new AppointeeStatusWizResponse
            {
                id = row?.UnderProcess?.UnderProcessId ?? 0,
                companyId = row?.UnderProcess?.CompanyId ?? 0,
                appointeeName = row?.AppointeeDetails?.AppointeeName ?? row.UnderProcess?.AppointeeName,
                appointeeId = row?.UnderProcess?.AppointeeId,
                appointeeEmailId = row?.AppointeeDetails?.AppointeeEmailId ?? row?.UnderProcess?.AppointeeEmailId,
                mobileNo = row?.UnderProcess.MobileNo,
                dateOfJoining = row?.AppointeeDetails?.DateOfJoining ?? row?.UnderProcess?.DateOfJoining,
                IsReprocess = false,
                Status = "Lapsed",
                StatusCode = 5,
            })?.ToList();


            return _lapsedViewdata;
        }
        public async Task updateAppointeeConsent(AppointeeConsentSubmitRequest req)
        {
            await _activityDalContext.PostActivityDetails(req.AppointeeId, req.UserId, req.ConsentStatusCode);
            await _appointeeDalContext.postAppointeeContestAsync(req);

        }
    }
}
