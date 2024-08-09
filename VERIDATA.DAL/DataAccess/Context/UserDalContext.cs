using Microsoft.EntityFrameworkCore;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.DAL.DBContext;
using VERIDATA.DAL.utility;
using VERIDATA.Model.Configuration;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Admin;
using VERIDATA.Model.Table.Config;
using VERIDATA.Model.Table.Master;
using VERIDATA.Model.Table.Public;
using static VERIDATA.DAL.utility.CommonEnum;

namespace VERIDATA.DAL.DataAccess.Context
{
    public class UserDalContext : IUserDalContext
    {
        private readonly DbContextDalDB _dbContextClass;
        private readonly TokenConfiguration _tokenConfig;
        private readonly ApiConfiguration _apiConfig;
        private readonly IMasterDalContext _dbContextMaster;
        public UserDalContext(DbContextDalDB dbContextClass, TokenConfiguration tokenConfig, IMasterDalContext dbContextMaster, ApiConfiguration apiConfig)
        {
            _dbContextClass = dbContextClass;
            _tokenConfig = tokenConfig;
            _dbContextMaster = dbContextMaster;
            _apiConfig = apiConfig;
        }

        public async Task<List<string?>> GetAllCandidateId(string type)
        {
            List<string?> candidateIdList = new();
            if (type == CandidateIdType.All)
            {
                candidateIdList = await _dbContextClass.UserMaster.Where(x => x.ActiveStatus.Value.Equals(true) && !string.IsNullOrEmpty(x.CandidateId)).Select(y => y.CandidateId).ToListAsync();
            }
            if (type == CandidateIdType.UnProcess)
            {
                candidateIdList = await _dbContextClass.UnProcessedFileData.Where(x => x.ActiveStatus.Value.Equals(true)).Select(y => y.CandidateId).ToListAsync();
            }
            if (type == CandidateIdType.Raw)
            {
                candidateIdList = await _dbContextClass.RawFileData.Where(x => x.ActiveStatus.Value.Equals(true)).Select(y => y.CandidateId).ToListAsync();
            }
            if (type == CandidateIdType.Processed)
            {
                var querydata = from p in _dbContextClass.ProcessedFileData
                                join a in _dbContextClass.AppointeeDetails
                                    on p.AppointeeId equals a.AppointeeId
                                where p.ActiveStatus == true && a.IsProcessed == true
                                select new { a.CandidateId };
                var varifiedAppointeelist = await querydata.ToListAsync().ConfigureAwait(false);
                candidateIdList = varifiedAppointeelist.Select(x => x.CandidateId).ToList();
            }
            if (type == CandidateIdType.UnderProcess)
            {
                var querydata = from p in _dbContextClass.UserMaster
                                join a in _dbContextClass.WorkFlowDetails
                                    on p.RefAppointeeId equals a.AppointeeId
                                where p.ActiveStatus == true && a.AppvlStatusId == 1
                                select new { p.CandidateId };
                var UnderProcessAppointeelist = await querydata.ToListAsync().ConfigureAwait(false);
                candidateIdList = UnderProcessAppointeelist.Select(x => x.CandidateId).ToList();
            }
            return candidateIdList;
        }
        public async Task<UserDetailsResponse> GetUserDetailsAsyncbyId(int uid)
        {

            List<WorkflowApprovalStatusMaster> _getapprovalStatus = await _dbContextMaster.GetAllApprovalStateMaster();
            WorkflowApprovalStatusMaster? closeState = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ProcessClose?.Trim());
            WorkflowApprovalStatusMaster? verifiedState = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Approved?.Trim());
            WorkflowApprovalStatusMaster? forcedVerifiedState = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ForcedApproved?.Trim());
            WorkflowApprovalStatusMaster? rejectedState = _getapprovalStatus.Find(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Rejected?.Trim());

            UserDetailsResponse userDetails = new();
            var userDetailsQuery = from u in _dbContextClass.UserMaster
                                   join a in _dbContextClass.UserAuthentication
                                                       on u.UserId equals a.UserId
                                   where u.ActiveStatus == true && a.UserId == uid && a.ActiveStatus == true
                                   select new { u, a.UserPwdTxt, a.UserProfilePwd, a.IsDefaultPass, a.PasswordSetDate };
            var users = await userDetailsQuery.FirstOrDefaultAsync().ConfigureAwait(false);
            //var users = await _dbContextClass.UserMaster.FirstOrDefaultAsync(m => m.UserId.Equals(uid) & m.ActiveStatus == true);
            RoleUserMapping? usersRole = await _dbContextClass.RoleUserMapping.FirstOrDefaultAsync(m => m.UserId.Equals(uid) && m.ActiveStatus == true);
            RoleMaster? _roledata = await _dbContextClass.RoleMaster.FirstOrDefaultAsync(x => x.ActiveStatus == true && x.RoleId.Equals(usersRole.RoleId));
            AppointeeDetails? _userDetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.ActiveStatus == true && x.AppointeeId == users.u.RefAppointeeId);
            List<AppointeeConsentMapping>? consentStatusList = await _dbContextClass.AppointeeConsentMapping.Where(x => x.AppointeeId == users.u.RefAppointeeId).ToListAsync();
            var consentStatus = consentStatusList?.FirstOrDefault(x => x.ActiveStatus == true);
            bool? IsPrerequisiteDataAvailable = null;
            var IsConsentProcessed = false;
            if (consentStatusList.Any())
            {
                IsPrerequisiteDataAvailable = consentStatusList?.Any(x => x.ConsentStatus == (int)ConsentStatus.PrerequisiteYes) ?? false;
                var IsConsentAvailable = consentStatusList?.Where(x => x.ConsentStatus == (int)ConsentStatus.Agree || x.ConsentId == (int)ConsentStatus.Revoked)?.ToList();
                IsConsentProcessed = IsConsentAvailable?.Count() > 0;
            }
            var _userStatusDetails = await _dbContextClass.WorkFlowDetails.FirstOrDefaultAsync(x => x.ActiveStatus == true && x.AppointeeId == users.u.RefAppointeeId);
            string status = "No Response";

            if (_userStatusDetails != null)
            {
                switch (true)
                {
                    case bool _ when _userStatusDetails.AppvlStatusId == verifiedState.AppvlStatusId || _userStatusDetails.AppvlStatusId == forcedVerifiedState.AppvlStatusId:
                        status = "Approved";
                        break;
                    case bool _ when _userStatusDetails.AppvlStatusId == rejectedState.AppvlStatusId:
                        status = "Rejected";
                        break;
                    case bool _ when _userDetails?.IsSubmit ?? false:
                        status = "Submitted";
                        break;
                    case bool _ when _userDetails?.SaveStep == 1:
                        status = "Ongoing";
                        break;
                }
            }

            userDetails.UserId = users?.u?.UserId ?? 0;
            userDetails.UserName = users?.u?.UserName;
            userDetails.Password = users?.UserPwdTxt;
            userDetails.EmailId = users?.u?.EmailId;
            userDetails.Phone = users?.u?.ContactNo;
            userDetails.RoleId = _roledata?.RoleId;
            userDetails.RoleName = _roledata?.RolesAlias;
            userDetails.UserCode = users?.u?.UserCode;
            userDetails.UserTypeId = users?.u?.UserTypeId ?? 0;
            userDetails.AppointeeId = users?.u?.RefAppointeeId;
            userDetails.IsProcessed = _userDetails?.IsProcessed ?? false;
            userDetails.ConsentStatus = consentStatus?.ConsentStatus ?? 0;
            userDetails.IsConsentProcessed = IsConsentProcessed;
            userDetails.IsPrerequisiteDataAvailable = IsPrerequisiteDataAvailable;
            userDetails.IsSubmit = _userDetails?.IsSubmit ?? false;
            userDetails.IsSetProfilePassword = !string.IsNullOrEmpty(users?.UserProfilePwd);
            userDetails.IsDefaultPassword = !string.IsNullOrEmpty(users?.IsDefaultPass) && users?.IsDefaultPass == "Y";
            userDetails.IsPasswordExpire = users?.PasswordSetDate == null && users?.IsDefaultPass == "Y" ? false : users?.PasswordSetDate.GetValueOrDefault().AddDays(_apiConfig.PasswordExpiryDays) <= DateTime.Now;
            userDetails.CompanyId = 1;
            userDetails.Status = status;

            return userDetails;
        }
        public async Task PostAppointeeUpdateLog(List<UpdatedAppointeeBasicInfo> updatedList, int userId)
        {
            List<AppointeeUpdateLog> updateLogList = new();
            foreach (UpdatedAppointeeBasicInfo updatedobj in updatedList)
            {
                if (!string.IsNullOrEmpty(updatedobj.AppointeeName))
                {
                    updateLogList.Add(new AppointeeUpdateLog { CandidateId = updatedobj.CandidateID, UpdateType = "Name", UpdateValue = updatedobj.AppointeeName, CreatedBy = userId, CreatedOn = DateTime.Now });
                }
                if (!string.IsNullOrEmpty(updatedobj.DateOfJoining))
                {
                    updateLogList.Add(new AppointeeUpdateLog { CandidateId = updatedobj.CandidateID, UpdateType = "DateOfJoining", UpdateValue = updatedobj.DateOfJoining, CreatedBy = userId, CreatedOn = DateTime.Now });
                }

                //if (!string.IsNullOrEmpty(updatedobj.AppointeeEmailId))
                //{
                //    updateLogList.Add(new AppointeeUpdateLog { CandidateId = updatedobj.CandidateID, UpdateType = "Email", UpdateValue = updatedobj.AppointeeEmailId, CreatedBy = UserId, CreatedOn = DateTime.Now });
                //}
                //if (!string.IsNullOrEmpty(updatedobj.CompanyName))
                //{
                //    updateLogList.Add(new AppointeeUpdateLog { CandidateId = updatedobj.CandidateID, UpdateType = "Company", UpdateValue = updatedobj.CompanyName, CreatedBy = UserId, CreatedOn = DateTime.Now });
                //}
                //if (!string.IsNullOrEmpty(updatedobj.MobileNo))
                //{
                //    updateLogList.Add(new AppointeeUpdateLog { CandidateId = updatedobj.CandidateID, UpdateType = "Mobile", UpdateValue = updatedobj.MobileNo, CreatedBy = UserId, CreatedOn = DateTime.Now });
                //}

            }
            if (updateLogList.Count > 0)
            {
                _dbContextClass.AppointeeUpdateLog.AddRange(updateLogList);
                _ = await _dbContextClass.SaveChangesAsync();
            }
        }
        public async Task UpdateRawCandidateData(List<UpdatedAppointeeBasicInfo> _appointeeList, List<string> candidateIdList, int UserId)
        {
            List<RawFileData> rawData = await _dbContextClass.RawFileData.Where(x => x.ActiveStatus.Value.Equals(true) && candidateIdList.Contains(x.CandidateId)).ToListAsync();
            foreach (RawFileData? obj in rawData)
            {
                UpdatedAppointeeBasicInfo? currRawdata = _appointeeList.Find(x => x.CandidateID == obj.CandidateId);
                obj.DateOfJoining = !string.IsNullOrEmpty(currRawdata.DateOfJoining) ? Convert.ToDateTime(currRawdata.DateOfJoining) : obj.DateOfJoining;
                obj.AppointeeName = (!string.IsNullOrEmpty(currRawdata.AppointeeName) && (obj.AppointeeName?.Trim() != currRawdata.AppointeeName)) ? currRawdata.AppointeeName : obj.AppointeeName;
                //obj.AppointeeEmailId = (!string.IsNullOrEmpty(currRawdata.AppointeeEmailId) && (obj.AppointeeEmailId?.Trim() != currRawdata.AppointeeEmailId)) ? currRawdata.AppointeeEmailId : obj.AppointeeEmailId?.Trim();
                //obj.CompanyName = (!string.IsNullOrEmpty(currRawdata.CompanyName) && (obj.CompanyName?.Trim() != currRawdata.CompanyName)) ? currRawdata.CompanyName : obj.CompanyName;
                //obj.MobileNo = (!string.IsNullOrEmpty(currRawdata.MobileNo) && (obj.MobileNo?.Trim() != currRawdata.MobileNo)) ? currRawdata.MobileNo : obj.MobileNo;
                obj.UpdatedBy = UserId;
                obj.UpdatedOn = DateTime.Now;
            }
            _ = await _dbContextClass.SaveChangesAsync();
        }
        public async Task UpdateUnderProcessCandidateData(List<UpdatedAppointeeBasicInfo> _appointeeList, List<string> candidateIdList, int UserId)
        {
            List<UnderProcessFileData> UnderProcessData = await _dbContextClass.UnderProcessFileData.Where(x => x.ActiveStatus.Value.Equals(true) && candidateIdList.Contains(x.CandidateId))?.ToListAsync();
            foreach (UnderProcessFileData? obj in UnderProcessData)
            {
                UpdatedAppointeeBasicInfo? currRawdata = _appointeeList.Find(x => x.CandidateID == obj.CandidateId);
                obj.DateOfJoining = !string.IsNullOrEmpty(currRawdata.DateOfJoining) ? Convert.ToDateTime(currRawdata.DateOfJoining) : obj.DateOfJoining;
                obj.AppointeeName = (!string.IsNullOrEmpty(currRawdata.AppointeeName) && (obj.AppointeeName?.Trim() != currRawdata.AppointeeName)) ? currRawdata.AppointeeName : obj.AppointeeName;
                //obj.AppointeeEmailId = (!string.IsNullOrEmpty(currRawdata.AppointeeEmailId) && (obj.AppointeeEmailId?.Trim() != currRawdata.AppointeeEmailId)) ? currRawdata.AppointeeEmailId : obj.AppointeeEmailId?.Trim();
                //obj.CompanyName = (!string.IsNullOrEmpty(currRawdata.CompanyName) && (obj.CompanyName?.Trim() != currRawdata.CompanyName)) ? currRawdata.CompanyName : obj.CompanyName;
                //obj.MobileNo = (!string.IsNullOrEmpty(currRawdata.MobileNo) && (obj.MobileNo?.Trim() != currRawdata.MobileNo)) ? currRawdata.MobileNo : obj.MobileNo;
                obj.UpdatedBy = UserId;
                obj.UpdatedOn = DateTime.Now;
            }

            List<AppointeeDetails> AppointeeDetailsData = await _dbContextClass.AppointeeDetails.Where(x => x.ActiveStatus.Value.Equals(true) && candidateIdList.Contains(x.CandidateId))?.ToListAsync();
            foreach (AppointeeDetails? obj in AppointeeDetailsData)
            {
                UpdatedAppointeeBasicInfo? currRawdata = _appointeeList.Find(x => x.CandidateID == obj.CandidateId);

                obj.DateOfJoining = !string.IsNullOrEmpty(currRawdata.DateOfJoining) ? Convert.ToDateTime(currRawdata.DateOfJoining) : obj.DateOfJoining;
                obj.AppointeeName = (!string.IsNullOrEmpty(currRawdata.AppointeeName) && (obj.AppointeeName?.Trim() != currRawdata.AppointeeName)) ? currRawdata.AppointeeName : obj.AppointeeName;
                if (!string.IsNullOrEmpty(currRawdata.AppointeeName) && (obj.AppointeeName?.Trim() != currRawdata.AppointeeName))
                {
                    if (obj.IsPanVarified ?? false)
                    {
                        obj.IsPanVarified = null;
                    }
                    if (obj.IsAadhaarVarified ?? false)
                    {
                        obj.IsAadhaarVarified = null;
                    }
                    if (obj.IsUanVarified ?? false)
                    {
                        obj.IsUanVarified = null;
                    }
                }

                //obj.AppointeeEmailId = (!string.IsNullOrEmpty(currRawdata.AppointeeEmailId) && (obj.AppointeeEmailId?.Trim() != currRawdata.AppointeeEmailId)) ? currRawdata.AppointeeEmailId : obj.AppointeeEmailId?.Trim();
                //obj.CompanyName = (!string.IsNullOrEmpty(currRawdata.CompanyName) && (obj.CompanyName?.Trim() != currRawdata.CompanyName)) ? currRawdata.CompanyName : obj.CompanyName;
                //obj.MobileNo = (!string.IsNullOrEmpty(currRawdata.MobileNo) && (obj.MobileNo?.Trim() != currRawdata.MobileNo)) ? currRawdata.MobileNo : obj.MobileNo;
                obj.UpdatedBy = UserId;
                obj.UpdatedOn = DateTime.Now;
            }
            List<UserMaster> UnderProcessUserData = await _dbContextClass.UserMaster.Where(x => x.ActiveStatus.Value.Equals(true) && candidateIdList.Contains(x.CandidateId))?.ToListAsync();
            foreach (UserMaster? obj in UnderProcessUserData)
            {
                UpdatedAppointeeBasicInfo? currRawdata = _appointeeList.Find(x => x.CandidateID == obj.CandidateId);
                obj.UserName = (!string.IsNullOrEmpty(currRawdata.AppointeeName) && (obj.UserName?.Trim() != currRawdata.AppointeeName)) ? currRawdata.AppointeeName : obj.UserName;

                //obj.EmailId = (!string.IsNullOrEmpty(currRawdata.AppointeeEmailId) && (obj.EmailId?.Trim() != currRawdata.AppointeeEmailId)) ? currRawdata.AppointeeEmailId : obj.EmailId?.Trim();
                //obj.ContactNo = (!string.IsNullOrEmpty(currRawdata.MobileNo) && (obj.ContactNo?.Trim() != currRawdata.MobileNo)) ? currRawdata.MobileNo : obj.ContactNo;
                obj.UpdatedBy = UserId;
                obj.UpdatedOn = DateTime.Now;

            }

            _ = await _dbContextClass.SaveChangesAsync();
        }
        public async Task UpdateNonProcessCandidateData(List<UpdatedAppointeeBasicInfo> _appointeeList, List<string> candidateIdList, int UserId)
        {
            List<UnProcessedFileData> NonProcessData = await _dbContextClass.UnProcessedFileData.Where(x => x.ActiveStatus.Value.Equals(true) && candidateIdList.Contains(x.CandidateId))?.ToListAsync();
            foreach (UnProcessedFileData obj in NonProcessData)
            {
                UpdatedAppointeeBasicInfo currRawdata = _appointeeList.Find(x => x.CandidateID == obj.CandidateId);
                obj.DateOfJoining = !string.IsNullOrEmpty(currRawdata.DateOfJoining) ? Convert.ToDateTime(currRawdata.DateOfJoining) : obj.DateOfJoining;
                obj.AppointeeName = (!string.IsNullOrEmpty(currRawdata.AppointeeName) && (obj.AppointeeName?.Trim() != currRawdata.AppointeeName)) ? currRawdata.AppointeeName : obj.AppointeeName;
                //obj.AppointeeEmailId = (!string.IsNullOrEmpty(currRawdata.AppointeeEmailId) && (obj.AppointeeEmailId?.Trim() != currRawdata.AppointeeEmailId)) ? currRawdata.AppointeeEmailId : obj.AppointeeEmailId?.Trim();
                //obj.CompanyName = (!string.IsNullOrEmpty(currRawdata.CompanyName) && (obj.CompanyName?.Trim() != currRawdata.CompanyName)) ? currRawdata.CompanyName : obj.CompanyName;
                //obj.MobileNo = (!string.IsNullOrEmpty(currRawdata.MobileNo) && (obj.MobileNo?.Trim() != currRawdata.MobileNo)) ? currRawdata.MobileNo : obj.MobileNo;
                obj.UpdatedBy = UserId;
                obj.UpdatedOn = DateTime.Now;
            }
            _ = await _dbContextClass.SaveChangesAsync();
        }
        public async Task<List<UserDetailsResponse>> getAllAdminUser()
        {
            IQueryable<UserDetailsResponse> querydata = from um in _dbContextClass.UserMaster
                                                        join ur in _dbContextClass.RoleUserMapping
                                                        on um.UserId equals ur.UserId
                                                        join r in _dbContextClass.RoleMaster
                                                         on ur.RoleId equals r.RoleId
                                                        join ua in _dbContextClass.UserAuthentication
                                                        on ur.UserId equals ua.UserId
                                                        where um.ActiveStatus == true && r.ActiveStatus == true
                                                        && ur.ActiveStatus == true
                                                        && r.IsCompanyAdmin == true && ua.ActiveStatus == true
                                                        select new UserDetailsResponse
                                                        {
                                                            UserId = um.UserId,
                                                            UserCode = um.UserCode,
                                                            UserName = um.UserName,
                                                            Password = ua.UserPwdTxt,
                                                            EmailId = um.EmailId,
                                                            Phone = um.ContactNo,
                                                            RoleId = r.RoleId,
                                                            RoleName = r.RoleName,
                                                            UserTypeId = um.UserTypeId,
                                                            CompanyId = 1,
                                                        };

            List<UserDetailsResponse> userlist = await querydata.ToListAsync().ConfigureAwait(false);

            return userlist;
        }
        public async Task<UserMaster?> getUserByUserCode(string? userCode)
        {
            UserMaster? dbusers = await _dbContextClass.UserMaster.FirstOrDefaultAsync(m => m.UserCode == userCode);

            return dbusers;
        }
        public async Task<UserMaster?> getUserByUserId(int userId)
        {
            UserMaster? dbusers = await _dbContextClass.UserMaster.FirstOrDefaultAsync(m => m.UserId.Equals(userId));

            return dbusers;
        }
        public async Task<UserAuthentication?> getAuthUserDetailsByPassword(int userId, string password)
        {
            string _password = CommonUtility.hashPassword(password: password);

            UserAuthentication? authDbUser = await _dbContextClass.UserAuthentication.FirstOrDefaultAsync(m => m.UserId.Equals(userId) && m.UserPwd == _password && m.ActiveStatus == true);

            return authDbUser;
        }
        public async Task<UserAuthentication> getAuthUserDetailsById(int userId)
        {
            UserAuthentication? authDbUser = await _dbContextClass.UserAuthentication.FirstOrDefaultAsync(m => m.UserId.Equals(userId) && m.ActiveStatus == true) ?? new UserAuthentication();

            return authDbUser;
        }
        public async Task<UserAuthenticationHist?> getAuthHistUserDetailsById(int? userId)
        {
            UserAuthenticationHist? authDbUser = await _dbContextClass.UserAuthenticationHist.OrderByDescending(x => x.AuthoHistId).FirstOrDefaultAsync(m => m.UserId == userId && m.ActiveStatus == true);

            return authDbUser;
        }
        public async Task<UserAuthenticationHist?> getAuthHistUserDetailsByClientId(string? clientId)
        {
            UserAuthenticationHist? authDbUser = await _dbContextClass.UserAuthenticationHist.OrderByDescending(x => x.AuthoHistId).FirstOrDefaultAsync(m => m.ClientId == clientId && m.ActiveStatus == true);

            return authDbUser;
        }
        public async Task createNewUserwithRole(List<CreateUserDetailsRequest> userList, int userId)
        {
            List<UserAuthentication> UserAuthList = new();
            List<RoleUserMapping> UserRoleMappingList = new();
            List<int?>? _appointeeList = userList?.Where(x => x.RefAppointeeId != null).Select(y => y.RefAppointeeId).ToList();
            if (_appointeeList.Any())
            {
                List<int?> _exstingappointeeId = await _dbContextClass.UserMaster.Where(x => _appointeeList.Contains(x.RefAppointeeId)).Select(y => y.RefAppointeeId).ToListAsync();
                userList = userList.Where(x => !_exstingappointeeId.Contains(x.RefAppointeeId)).ToList();
            }

            foreach ((CreateUserDetailsRequest obj, CreateUserDetailsRequest user) in from obj in userList
                                                                                      let user = new CreateUserDetailsRequest()
                                                                                      select (obj, user))
            {
                UserMaster Users = new()
                {
                    UserCode = obj.UserCode?.Trim(),
                    CandidateId = obj.CandidateId,
                    UserName = obj.UserName,
                    EmailId = obj.EmailId,
                    ContactNo = obj.ContactNo,
                    RoleId = obj.RoleId,
                    UserTypeId = obj.UserTypeId,
                    RefAppointeeId = obj.RefAppointeeId,
                    ActiveStatus = true,
                    CurrStatus = true,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now
                };
                _ = _dbContextClass.UserMaster.Add(Users);
                _ = await _dbContextClass.SaveChangesAsync();

                UserAuthentication UsersAuth = new()
                {
                    UserId = Users.UserId,
                    UserPwd = CommonUtility.hashPassword(obj.Password),
                    UserPwdTxt = obj.Password,
                    IsDefaultPass = CommonEnum.CheckType.yes,
                    ActiveStatus = true,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now
                };
                UserAuthList.Add(UsersAuth);

                RoleUserMapping UserRoleMappingData = new()
                {
                    RoleId = obj.RoleId,
                    UserId = Users.UserId,
                    ActiveStatus = true,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now
                };
                UserRoleMappingList.Add(UserRoleMappingData);
            }
            _dbContextClass.UserAuthentication.AddRange(UserAuthList);
            _dbContextClass.RoleUserMapping.AddRange(UserRoleMappingList);
            _ = await _dbContextClass.SaveChangesAsync();
        }
        public async Task updateAdminUser(AdminUserUpdateRequest userDetails)
        {
            List<RoleUserMapping> UserRoleMappingList = new();
            UserMaster? _userDetails = await _dbContextClass.UserMaster.Where(x => x.UserId.Equals(userDetails.Id) && x.ActiveStatus == true).FirstOrDefaultAsync();
            UserAuthentication? _userAuthDetails = await _dbContextClass.UserAuthentication.Where(x => x.UserId.Equals(userDetails.Id) && x.ActiveStatus == true).FirstOrDefaultAsync();
            RoleUserMapping? _userRoleDetails = await _dbContextClass.RoleUserMapping.Where(x => x.UserId.Equals(userDetails.Id) && x.ActiveStatus == true).FirstOrDefaultAsync();
            if (_userDetails != null)
            {
                _userDetails.UserCode = userDetails.UserCode?.Trim();
                _userDetails.UserName = userDetails.UserName?.Trim();
                _userDetails.EmailId = userDetails.EmailId?.Trim();
                _userDetails.ContactNo = userDetails.Phone;
                _userDetails.RoleId = userDetails.RoleId ?? 0;
                _userDetails.UserTypeId = userDetails.UserTypeId;
                _userDetails.UpdatedBy = userDetails.UserId;
                _userDetails.UpdatedOn = DateTime.Now;
            }
            if (_userAuthDetails != null && _userAuthDetails.UserPwdTxt != userDetails.Password?.Trim())
            {
                _userAuthDetails.UserPwdTxt = userDetails.Password?.Trim();
                _userAuthDetails.UserPwd = CommonUtility.hashPassword(userDetails.Password?.Trim());
            }
            if (_userRoleDetails != null && _userRoleDetails.RoleId != userDetails.RoleId && userDetails.RoleId != null)
            {
                _userRoleDetails.ActiveStatus = false;

                RoleUserMapping UserRoleMappingData = new()
                {
                    RoleId = userDetails.RoleId ?? 0,
                    UserId = userDetails.UserId,
                    ActiveStatus = true,
                    CreatedBy = userDetails.UserId,
                    CreatedOn = DateTime.Now
                };
                UserRoleMappingList.Add(UserRoleMappingData);
                _dbContextClass.RoleUserMapping.AddRange(UserRoleMappingList);
            }
            _ = await _dbContextClass.SaveChangesAsync();
        }
        public async Task deleteUserDetails(int uid, int userId)
        {
            UserMaster? applicationUsers = await _dbContextClass.UserMaster.FindAsync(uid);
            if (applicationUsers != null)
            {
                applicationUsers.ActiveStatus = false;
                applicationUsers.UpdatedOn = DateTime.Now;
                applicationUsers.UpdatedBy = userId;
                _ = await _dbContextClass.SaveChangesAsync();
            }
        }
        public async Task postUserAuthDetailsAsyncbyId(UserAuthDetailsRequest req)
        {
            List<UserAuthenticationHist> usersauthdata = await _dbContextClass.UserAuthenticationHist.Where(m => m.UserId.Equals(req.UserId) && m.ActiveStatus == true).ToListAsync();
            var userAuthDetails = usersauthdata?.LastOrDefault();
            usersauthdata.Where(x => x.CreatedOn < DateTime.Now.AddMinutes(-(_apiConfig.ProfileLockDuration)))?.ToList()?.ForEach(x => x.ActiveStatus = false);

            //usersauthdata?.ForEach(x => x.ActiveStatus = false);
            var timeOutTime = DateTime.Now.AddMinutes(_tokenConfig?.Timeout ?? 0);
            UserAuthenticationHist _userAuthHis = new()
            {
                UserId = req.UserId,
                ClientId = req.ClientId,
                EntryTime = DateTime.Now,
                IPAddress = req.IpaAdress,
                BrowserName = req.browserName,
                TokenNo = req.Token,
                RefreshTokenExpiryTime = timeOutTime.AddMinutes(-1),
                Otp = req.Otp,
                OtpExpiryTime = string.IsNullOrEmpty(req.Otp) ? null : userAuthDetails?.Otp == req.Otp ? userAuthDetails?.OtpExpiryTime : DateTime.Now.AddMinutes(_apiConfig.OtpExpiryDuration),
                ActiveStatus = true,
                CreatedBy = req.UserId,
                CreatedOn = DateTime.Now
                //IsChecked = null,
            };
            _ = _dbContextClass.UserAuthenticationHist.Add(_userAuthHis);
            _ = await _dbContextClass.SaveChangesAsync();
        }
        public async Task updateUserAuthDetailsAsyncbyId(int UserId)
        {
            List<UserAuthenticationHist> usersauthdata = await _dbContextClass.UserAuthenticationHist.Where(m => m.UserId.Equals(UserId) && m.ActiveStatus == true).ToListAsync();
            usersauthdata?.ForEach(x => x.ActiveStatus = false);
            _ = await _dbContextClass.SaveChangesAsync();
        }
        public async Task<List<UserAuthenticationHist>> getUserOtpTryDetailsAsyncbyId(int UserId)
        {
            List<UserAuthenticationHist> usersauthdata = await _dbContextClass.UserAuthenticationHist.Where(m => m.UserId.Equals(UserId) && m.ActiveStatus == true).ToListAsync();
            return usersauthdata;
        }
        public async Task postUserTokenDetailsAsyncbyId(int userId, string token)
        {
            var timeOutTime = DateTime.Now.AddMinutes(_tokenConfig?.Timeout ?? 0);
            UserAuthenticationHist? usersauthdata = await _dbContextClass.UserAuthenticationHist.FirstOrDefaultAsync(m => m.UserId.Equals(userId) && m.ActiveStatus == true);
            if (usersauthdata != null)
            {
                usersauthdata.TokenNo = token;
                usersauthdata.RefreshTokenExpiryTime = timeOutTime.AddMinutes(-1);
                _ = await _dbContextClass.SaveChangesAsync();
            }
        }
        public async Task postUserSignOutDetailsAsyncbyId(int userId)
        {
            List<UserAuthenticationHist> usersauthdata = await _dbContextClass.UserAuthenticationHist.Where(m => m.UserId.Equals(userId) && m.ActiveStatus == true).ToListAsync();

            if (usersauthdata?.Count > 0)
            {
                usersauthdata?.ForEach(x => x.ActiveStatus = false);
                _ = await _dbContextClass.SaveChangesAsync();
            }
        }
        public async Task postUserPasswordChangeAsyncbyId(int userId, string password)
        {
            UserAuthentication usersauthdata = await _dbContextClass.UserAuthentication.FirstOrDefaultAsync(m => m.UserId.Equals(userId) && m.ActiveStatus == true);

            if (usersauthdata?.UserAuthoId != null)
            {
                usersauthdata.IsDefaultPass = "N";
                usersauthdata.UserPwd = CommonUtility.hashPassword(password);
                usersauthdata.UserPwdTxt = password;
                usersauthdata.PasswordSetDate = DateTime.Now;
                usersauthdata.UpdatedOn = DateTime.Now;
                usersauthdata.UpdatedBy = userId;

                await _dbContextClass.SaveChangesAsync();
            }
        }
        public async Task editUserProfile(EditUserProfileRequest req)
        {
            string _password = CommonUtility.hashPassword(password: req.ProfilePassword);

            UserAuthentication? _authenticatedUserDatails = await getAuthUserDetailsById(req.UserId);
            if (_authenticatedUserDatails != null)
            {
                bool Isvalidate = _authenticatedUserDatails?.UserProfilePwd == _password;
                if (!Isvalidate)
                {
                    _authenticatedUserDatails.ActiveStatus = false;
                    UserAuthentication UsersAuth = new()
                    {
                        UserId = _authenticatedUserDatails.UserId,
                        UserPwd = _authenticatedUserDatails.UserPwd,
                        UserPwdTxt = _authenticatedUserDatails.UserPwdTxt,
                        UserProfilePwd = CommonUtility.hashPassword(_password),
                        IsDefaultPass = CommonEnum.CheckType.yes,
                        ActiveStatus = true,
                        CreatedBy = req.UserId,
                        CreatedOn = DateTime.Now
                    };
                    _ = _dbContextClass.UserAuthentication.Add(UsersAuth);
                    _ = await _dbContextClass.SaveChangesAsync();
                }
            }
        }
        public async Task<RoleDetailsResponse> GetUserRole(int userid)
        {

            RoleDetailsResponse? roleDetails = new();
            IQueryable<RoleDetailsResponse> querydata = from p in _dbContextClass.RoleUserMapping
                                                        join a in _dbContextClass.RoleMaster
                                                            on p.RoleId equals a.RoleId
                                                        where p.ActiveStatus == true && a.ActiveStatus == true && p.UserId == userid
                                                        select new RoleDetailsResponse
                                                        {
                                                            RoleId = p.RoleId,
                                                            RoleName = a.RoleName,
                                                            RoleDescription = a.RoleDesc,
                                                            RoleAlias = a.RolesAlias,
                                                        };

            roleDetails = await querydata.FirstOrDefaultAsync().ConfigureAwait(false);

            return roleDetails ?? new RoleDetailsResponse();
        }
        public async Task<List<MenuNodeDetails>> GetMenuLeafNodeList(int roleId)
        {
            var _menudata = from r in _dbContextClass.MenuRoleMapping
                            join m in _dbContextClass.MenuMaster
                                on r.MenuId equals m.MenuId
                            where r.RoleId == roleId && m.ActiveStatus == true && r.ActiveStatus == true
                            orderby m.SeqNo ascending
                            select new
                            {
                                m.MenuId,
                                m.ParentMenuId,
                                m.MenuTitle,
                                m.MenuDesc,
                                m.menu_level,
                                m.menu_icon_url,
                                m.menu_action,
                                m.SeqNo,
                                r.ActionId
                            };

            List<MenuNodeDetails> menudata = await _menudata
                    .Join(_dbContextClass.ActionMaster.Where(x => x.ActiveStatus == true),
                        a => a.ActionId,
                        m => m.ActionId,
                        (m, a) => new MenuNodeDetails
                        {
                            MenuId = m.MenuId,
                            ParentMenuId = m.ParentMenuId,
                            MenuTitle = m.MenuTitle,
                            MenuDesc = m.MenuDesc,
                            MenuLevel = m.menu_level,
                            IconClass = m.menu_icon_url,
                            ActionUrl = m.menu_action,
                            SeqNo = m.SeqNo,
                            ActionName = a.ActionName,
                            ActionAlias = a.ActionAlias,
                            ActionId = a.ActionId
                        })?.ToListAsync();

            return menudata;
        }


    }
}
