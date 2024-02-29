using PfcAPI.Model.DataAccess;
using PfcAPI.Model.RequestModel;
using PfcAPI.Model.ResponseModel;

namespace PfcAPI.Infrastucture.Interfaces
{
    public interface IuserContext
    {
        public Task<ValidateUserResponse> validateUserSignInAsync(UserSignInRequest user);
        public Task<bool> validateUserByCode(string? userCode);
        public Task<bool> validateUserById(int? id);
        public Task<int> validateUserByOtp(string? clientId, string? otp, int userType);
        public bool getUserProfileDetails(string uname);
        public Task<bool> validateProfilePasswowrdAsync(ValidateProfilePasswordRequest req);
        public Task EditUserProfile(EditUserProfileRequest req);
        public Task<UserDetails> getUserDetailsAsyncbyId(int uid);
        public Task<List<UserDetails>> getAllAdminUser();
        public Task editAdminUser(AdminUserUpdateRequest userDetails);
        public Task<UserDetails> getAdminUserDetails(int userId);
        //public Task postUserAuthDetailsAsyncbyId(int uid, string? IpaAdress, string? browserName);
        public Task postUserAuthDetailsAsyncbyId(UserAuthDetailsRequest req);
        public Task<UserDetails> getUserDetailsbyAppointeeId(int appnteid);
        public Task removeUserDetails(int uid, int userId);
        //public Task<List<UserDetails>> getUserListAsync();
        public Task<List<AppointeeBasicInfo>> ValidateExistingUser(List<AppointeeBasicInfo> userList);
        public Task<List<UpdatedAppointeeBasicInfo>> UpdateExistingUser(List<UpdatedAppointeeBasicInfo> _appointeeList, int UserId);
        public Task createNewUserwithRole(List<CreateUserDetailsRequest> userList, int userId);
        public Task addRoleByRoleAlias(string RoleType, List<CreateUserDetailsRequest> userListId, int userId);
        public Task createRole(CreateUsereRoleRequest roleDetails);
        //public bool addNewCompanyUser(string uname);
        public Task<CompanyDetails> getCompanyDetails(int companyId);
        public Task<List<CompanyDetails>> getCompanyList();
        public Task<RoleDetails> getRoleDetailsByRoleAlias(string roleAlias);
        public Task<List<RoleDetails>> getUserRoleList();
        public Task<RoleDetails> getRoleDetails(int RoleId);
        public Task<List<DropDownDetails>> getDropDownData(string type);
        public Task<WidgetProgressData> GetTotalProgressWidgetData();
        public Task<WidgetProgressData> GetTodayProgressWidgetData();
        public Task<List<DashboardWidgetResponse>> GetWidgetData(int? filterDays, bool isfilterd);
        public Task<CriticalAppointeeWidgetResponse> GetCriticalData();
        public Task<UnderProcessWidgetData> GetUndrPrcessProgressWidgetData();
        public Task<int> GetProgressStatusWidgetData();
        public Task<List<AppointeeStatusWizResponse>> GetAppointeeStatusWidgetData(string code);

    }
}
