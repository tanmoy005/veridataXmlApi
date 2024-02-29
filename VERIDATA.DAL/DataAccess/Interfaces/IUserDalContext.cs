using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Admin;

namespace VERIDATA.DAL.DataAccess.Interfaces
{
    public interface IUserDalContext
    {
        public Task<UserDetailsResponse> GetUserDetailsAsyncbyId(int uid);
        public Task<List<string?>> GetAllCandidateId(string type);
        public Task PostAppointeeUpdateLog(List<UpdatedAppointeeBasicInfo> updatedList, int userId);
        public Task UpdateRawCandidateData(List<UpdatedAppointeeBasicInfo> _appointeeList, List<string> candidateIdList, int UserId);
        public Task UpdateUnderProcessCandidateData(List<UpdatedAppointeeBasicInfo> _appointeeList, List<string> candidateIdList, int UserId);
        public Task UpdateNonProcessCandidateData(List<UpdatedAppointeeBasicInfo> _appointeeList, List<string> candidateIdList, int UserId);
        public Task<List<UserDetailsResponse>> getAllAdminUser();
        public Task<UserMaster?> getUserByUserCode(string? userCode);
        public Task<UserMaster?> getUserByUserId(int userId);
        public Task<UserAuthentication?> getAuthUserDetailsByPassword(int userId, string password);
        public Task<UserAuthentication> getAuthUserDetailsById(int userId);
        public Task<UserAuthenticationHist?> getAuthHistUserDetailsByClientId(string? clientId);
        public Task<UserAuthenticationHist?> getAuthHistUserDetailsById(int? userId);
        public Task createNewUserwithRole(List<CreateUserDetailsRequest> userList, int userId);
        public Task updateAdminUser(AdminUserUpdateRequest userDetails);
        public Task deleteUserDetails(int uid, int userId);
        public Task postUserAuthDetailsAsyncbyId(UserAuthDetailsRequest req);
        public Task postUserTokenDetailsAsyncbyId(int userId, string token);
        public Task postUserSignOutDetailsAsyncbyId(int userId);
        public Task editUserProfile(EditUserProfileRequest req);
        public Task<RoleDetailsResponse> GetUserRole(int userid);
        public Task<List<MenuNodeDetails>> GetMenuLeafNodeList(int roleId);
    }
}
