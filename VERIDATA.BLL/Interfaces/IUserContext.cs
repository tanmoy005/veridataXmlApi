

using VERIDATA.Model.DataAccess;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;

namespace VERIDATA.BLL.Interfaces
{
    public interface IUserContext
    {
        public Task<ValidateUserDetails> validateUserSign(UserSignInRequest user);
        public Task<ValidateUserDetails> validateUserChangePassword(ChangePasswordGenerateOTPRequest user);
        public Task<ValidateUserSignInResponse> postUserAuthdetails(ValidateUserDetails req);
        public Task updateUserAuthdetails(int userId);
        public Task postUsersignOut(int userId);
        public Task postUserPasswordChange(SetNewPasswordRequest req);
        public Task<int> validateUserByOtp(string? clientId, string? otp, int userType);
        public Task<int> getUserByEmail(string? email);
        public Task<AuthenticatedUserResponse> getValidatedSigninUserDetails(int userId);
        public Task<TokenDetailsResponse> getRefreshToken(RefreshTokenRequest reqObj);
        public Task<bool> validateProfilePasswowrdAsync(ValidateProfilePasswordRequest req);
        public Task editUserProfile(EditUserProfileRequest req);
        public Task<List<AppointeeBasicInfo>> validateExistingUser(List<AppointeeBasicInfo> userList);
        public Task<List<UpdatedAppointeeBasicInfo>> updateExistingUser(List<UpdatedAppointeeBasicInfo> _appointeeList, int UserId);
        public Task<UserDetailsResponse> getUserDetailsAsyncbyId(int id);
        public Task<List<UserDetailsResponse>> getAllAdminUser();
        public Task<bool> validateUserByCode(string? userCode);
        public Task<bool> createNewUserwithRole(CreateUserDetailsRequest userdetails);
        public Task editAdminUser(AdminUserUpdateRequest userDetails);
        public Task<bool> removeUserDetails(int uid, int userId);
        public Task<List<MenuNodeResponse>> GetMenuData(int userid);
        public Task<List<DropDownDetailsResponse>> getDropDownData(string type);
        public Task<List<DashboardWidgetResponse>> GetWidgetData(int? filterDays, bool isfilterd);
        public Task<CriticalAppointeeWidgetResponse> GetCriticalData();
        public Task<WidgetProgressDataResponse> GetTotalProgressWidgetData();
        public Task<List<AppointeeStatusWizResponse>> GetAppointeeStatusWidgetData(string code);
        public Task updateAppointeeConsent(AppointeeConsentSubmitRequest req);
        public Task updateAppointeePrerequisite (AppointeeConsentSubmitRequest req);
    }
}
