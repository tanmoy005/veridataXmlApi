using Microsoft.EntityFrameworkCore;
using PfcAPI.Infrastucture.DBContext;
using PfcAPI.Infrastucture.Interfaces;
using PfcAPI.Infrastucture.Notification.Provider;
using PfcAPI.Infrastucture.utility;
using PfcAPI.Model.Configuration;
using PfcAPI.Model.DataAccess;
using PfcAPI.Model.Maintainance;
using PfcAPI.Model.Master;
using PfcAPI.Model.MenuRoleUser;
using PfcAPI.Model.RequestModel;
using PfcAPI.Model.ResponseModel;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static PfcAPI.Infrastucture.CommonEnum;

namespace PfcAPI.Infrastucture.Context
{
    public class UserContext : IuserContext
    {
        private readonly DbContextDB _context;
        private readonly ConfigurationSetup _configSetup;
        private readonly IEmailSender _emailSender;
        //private readonly IWorkFlowContext _workFlowContext;

        public UserContext(DbContextDB context, ConfigurationSetup configSetup, IEmailSender emailSender)
        {
            _context = context;
            _configSetup = configSetup;
            _emailSender = emailSender;
            //_workFlowContext = workFlowContext;
        }
        public async Task<List<AppointeeBasicInfo>> ValidateExistingUser(List<AppointeeBasicInfo> _appointeeList)
        {
            var exstinguserList = new List<AppointeeBasicInfo>();
            var DuplicateCheckUsersList = new List<AppointeeBasicInfo>();

            //var _appointeeList = userList?.Where(x => (x.appointeeId != null))?.ToList();
            if (_appointeeList.Any())
            {
                DuplicateCheckUsersList = _appointeeList;
                // var _exstingappointeeId = await _context.UserMaster.Where(x => _appointeeList.Contains(x.RefAppointeeId)).Select(y => y.RefAppointeeId).ToListAsync();
                //duplicate checking in user data
                var _userList = await _context.UserMaster.Where(x => x.ActiveStatus.Value.Equals(true) && !string.IsNullOrEmpty(x.CandidateId)).Select(y => y.CandidateId).ToListAsync();

                exstinguserList = _appointeeList.Where(y => _userList.Contains(y.CandidateID)).ToList();

                if (exstinguserList?.Count() > 0)
                {
                    DuplicateCheckUsersList = _appointeeList.Except(exstinguserList).ToList();

                }
                //duplicate checking in non process data
                var _nonProcessUserList = await _context.UnProcessedFileData.Where(x => x.ActiveStatus.Value.Equals(true)).Select(y => y.CandidateId).ToListAsync();
                var exstingNonProcessUserList = DuplicateCheckUsersList.Where(y => _nonProcessUserList.Contains(y.CandidateID)).ToList();
                if (exstingNonProcessUserList?.Count() > 0)
                {
                    exstinguserList?.AddRange(exstingNonProcessUserList);
                    DuplicateCheckUsersList = DuplicateCheckUsersList.Except(exstingNonProcessUserList).ToList();
                }
                //duplicate checking in raw data
                var _rawUserList = await _context.RawFileData.Where(x => x.ActiveStatus.Value.Equals(true)).Select(y => y.CandidateId).ToListAsync();
                var exstingRawUserList = DuplicateCheckUsersList.Where(y => _rawUserList.Contains(y.CandidateID)).ToList();
                if (exstingRawUserList?.Count() > 0)
                {
                    exstinguserList?.AddRange(exstingRawUserList);
                    DuplicateCheckUsersList = DuplicateCheckUsersList.Except(exstingRawUserList).ToList();
                }

                var _exstinguserList = exstinguserList?.Distinct()?.ToList();
                //exstinguserList = _exstinguserList?.ToList();

                //duplicate checking in varified data

                var querydata = from p in _context.ProcessedFileData
                                join a in _context.AppointeeDetails
                                    on p.AppointeeId equals a.AppointeeId
                                where p.ActiveStatus == true & a.IsProcessed == true
                                select new { a.CandidateId };
                var varifiedAppointeelist = await querydata.ToListAsync().ConfigureAwait(false);
                var varifiedCandidateIdlist = varifiedAppointeelist.Select(x => x.CandidateId).ToList();

                var exstingVarifiedUserList = DuplicateCheckUsersList.Where(y => varifiedCandidateIdlist.Contains(y.CandidateID)).ToList();

                if (exstingVarifiedUserList?.Count() > 0)
                {
                    exstinguserList?.AddRange(exstingVarifiedUserList);
                    //DuplicateCheckUsersList = DuplicateCheckUsersList.Except(exstingVarifiedUserList).ToList();
                }
            }
            return exstinguserList;
        }
        public async Task<List<UpdatedAppointeeBasicInfo>> UpdateExistingUser(List<UpdatedAppointeeBasicInfo> _appointeeList, int UserId)
        {

            var UpdateUsersList = new List<UpdatedAppointeeBasicInfo>();

            //var _appointeeList = userList?.Where(x => (x.appointeeId != null))?.ToList();
            if (_appointeeList.Any())
            {
                UpdateUsersList = _appointeeList;
                //updating in raw data
                var _rawUserList = await _context.RawFileData.Where(x => x.ActiveStatus == true).Select(y => y.CandidateId).ToListAsync();
                var exstingRawUserList = _appointeeList.Where(y => _rawUserList.Contains(y.CandidateID)).ToList();
                if (exstingRawUserList?.Count() > 0)
                {
                    UpdateUsersList = UpdateUsersList?.Except(exstingRawUserList)?.ToList();
                    await UpdateCandidateDataStatusWise(exstingRawUserList, CandidateUpdateTableType.Raw, UserId);
                }
                var _nonProcessUserList = await _context.UnProcessedFileData.Where(x => x.ActiveStatus == true).Select(y => y.CandidateId).ToListAsync();
                var exstingNonProcessUserList = UpdateUsersList?.Where(y => _nonProcessUserList.Contains(y?.CandidateID))?.ToList();
                //var exstingNonProcessUserList = _nonProcessUserList?.SelectMany(x => UpdateUsersList?.Where(y => y?.CandidateID == x.CandidateId)).ToList();
                if (exstingNonProcessUserList?.Count() > 0)
                {
                    UpdateUsersList = UpdateUsersList?.Except(exstingNonProcessUserList)?.ToList();
                    await UpdateCandidateDataStatusWise(exstingNonProcessUserList, CandidateUpdateTableType.linknotsend, UserId);
                }

                //updating in user data

                var querydata = from p in _context.UserMaster
                                join a in _context.WorkFlowDetails
                                    on p.RefAppointeeId equals a.AppointeeId
                                where p.ActiveStatus == true & a.AppvlStatusId == 1
                                select new { p.CandidateId };
                var UnderProcessAppointeelist = await querydata.ToListAsync().ConfigureAwait(false);
                var ActiveCandidateIdList = UnderProcessAppointeelist.Select(x => x.CandidateId).ToList();

                //var _userList = await _context.UserMaster.Where(x => x.ActiveStatus.Value.Equals(true)).ToListAsync();

                var underProcessUserList = UpdateUsersList?.Where(y => ActiveCandidateIdList.Contains(y.CandidateID))?.ToList();
                //var underProcessUserList = UnderProcessAppointeelist?.SelectMany(x => UpdateUsersList?.Where(y => y?.CandidateID == x.CandidateId)).ToList();

                if (underProcessUserList?.Count() > 0)
                {
                    UpdateUsersList = UpdateUsersList?.Except(underProcessUserList)?.ToList();
                    await UpdateCandidateDataStatusWise(underProcessUserList, CandidateUpdateTableType.underProcess, UserId);

                }
                var updatedList = _appointeeList.Except(UpdateUsersList)?.ToList();
                List<AppointeeUpdateLog> updateLogList = new List<AppointeeUpdateLog>();
                foreach (var updatedobj in updatedList)
                {
                    if (!string.IsNullOrEmpty(updatedobj.AppointeeName))
                    {
                        updateLogList.Add(new AppointeeUpdateLog { CandidateId = updatedobj.CandidateID, UpdateType = "Name", UpdateValue = updatedobj.AppointeeName, CreatedBy = UserId, CreatedOn = DateTime.Now });
                    }
                    if (!string.IsNullOrEmpty(updatedobj.DateOfJoining))
                    {
                        updateLogList.Add(new AppointeeUpdateLog { CandidateId = updatedobj.CandidateID, UpdateType = "DateOfJoining", UpdateValue = updatedobj.DateOfJoining, CreatedBy = UserId, CreatedOn = DateTime.Now });
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
                if (updateLogList?.Count() > 0)
                {
                    _context.AppointeeUpdateLog.AddRange(updateLogList);
                    await _context.SaveChangesAsync();
                }

            }
            return UpdateUsersList;
        }
        private async Task UpdateCandidateDataStatusWise(List<UpdatedAppointeeBasicInfo> _appointeeList, string type, int UserId)
        {
            var candidateIdList = _appointeeList.Select(x => x.CandidateID)?.ToList();
            if (type == CandidateUpdateTableType.Raw && candidateIdList?.Count > 0)
            {
                var rawData = await _context.RawFileData.Where(x => x.ActiveStatus.Value.Equals(true) && candidateIdList.Contains(x.CandidateId))?.ToListAsync();
                foreach (var obj in rawData)
                {
                    var currRawdata = _appointeeList.FirstOrDefault(x => x.CandidateID == obj.CandidateId);
                    obj.DateOfJoining = !string.IsNullOrEmpty(currRawdata.DateOfJoining) ? Convert.ToDateTime(currRawdata.DateOfJoining) : obj.DateOfJoining;
                    obj.AppointeeName = (!string.IsNullOrEmpty(currRawdata.AppointeeName) && (obj.AppointeeName?.Trim() != currRawdata.AppointeeName)) ? currRawdata.AppointeeName : obj.AppointeeName;
                    //obj.AppointeeEmailId = (!string.IsNullOrEmpty(currRawdata.AppointeeEmailId) && (obj.AppointeeEmailId?.Trim() != currRawdata.AppointeeEmailId)) ? currRawdata.AppointeeEmailId : obj.AppointeeEmailId?.Trim();
                    //obj.CompanyName = (!string.IsNullOrEmpty(currRawdata.CompanyName) && (obj.CompanyName?.Trim() != currRawdata.CompanyName)) ? currRawdata.CompanyName : obj.CompanyName;
                    //obj.MobileNo = (!string.IsNullOrEmpty(currRawdata.MobileNo) && (obj.MobileNo?.Trim() != currRawdata.MobileNo)) ? currRawdata.MobileNo : obj.MobileNo;
                    obj.UpdatedBy = UserId;
                    obj.UpdatedOn = DateTime.Now;
                }

            }
            if (type == CandidateUpdateTableType.linknotsend && candidateIdList?.Count > 0)
            {
                var NonProcessData = await _context.UnProcessedFileData.Where(x => x.ActiveStatus.Value.Equals(true) && candidateIdList.Contains(x.CandidateId))?.ToListAsync();
                foreach (var obj in NonProcessData)
                {
                    var currRawdata = _appointeeList.FirstOrDefault(x => x.CandidateID == obj.CandidateId);
                    obj.DateOfJoining = !string.IsNullOrEmpty(currRawdata.DateOfJoining) ? Convert.ToDateTime(currRawdata.DateOfJoining) : obj.DateOfJoining;
                    obj.AppointeeName = (!string.IsNullOrEmpty(currRawdata.AppointeeName) && (obj.AppointeeName?.Trim() != currRawdata.AppointeeName)) ? currRawdata.AppointeeName : obj.AppointeeName;
                    //obj.AppointeeEmailId = (!string.IsNullOrEmpty(currRawdata.AppointeeEmailId) && (obj.AppointeeEmailId?.Trim() != currRawdata.AppointeeEmailId)) ? currRawdata.AppointeeEmailId : obj.AppointeeEmailId?.Trim();
                    //obj.CompanyName = (!string.IsNullOrEmpty(currRawdata.CompanyName) && (obj.CompanyName?.Trim() != currRawdata.CompanyName)) ? currRawdata.CompanyName : obj.CompanyName;
                    //obj.MobileNo = (!string.IsNullOrEmpty(currRawdata.MobileNo) && (obj.MobileNo?.Trim() != currRawdata.MobileNo)) ? currRawdata.MobileNo : obj.MobileNo;
                    obj.UpdatedBy = UserId;
                    obj.UpdatedOn = DateTime.Now;
                }

            }
            if (type == CandidateUpdateTableType.underProcess && candidateIdList?.Count > 0)
            {
                var UnderProcessData = await _context.UnderProcessFileData.Where(x => x.ActiveStatus.Value.Equals(true) && candidateIdList.Contains(x.CandidateId))?.ToListAsync();
                foreach (var obj in UnderProcessData)
                {
                    var currRawdata = _appointeeList.FirstOrDefault(x => x.CandidateID == obj.CandidateId);
                    obj.DateOfJoining = !string.IsNullOrEmpty(currRawdata.DateOfJoining) ? Convert.ToDateTime(currRawdata.DateOfJoining) : obj.DateOfJoining;
                    obj.AppointeeName = (!string.IsNullOrEmpty(currRawdata.AppointeeName) && (obj.AppointeeName?.Trim() != currRawdata.AppointeeName)) ? currRawdata.AppointeeName : obj.AppointeeName;
                    //obj.AppointeeEmailId = (!string.IsNullOrEmpty(currRawdata.AppointeeEmailId) && (obj.AppointeeEmailId?.Trim() != currRawdata.AppointeeEmailId)) ? currRawdata.AppointeeEmailId : obj.AppointeeEmailId?.Trim();
                    //obj.CompanyName = (!string.IsNullOrEmpty(currRawdata.CompanyName) && (obj.CompanyName?.Trim() != currRawdata.CompanyName)) ? currRawdata.CompanyName : obj.CompanyName;
                    //obj.MobileNo = (!string.IsNullOrEmpty(currRawdata.MobileNo) && (obj.MobileNo?.Trim() != currRawdata.MobileNo)) ? currRawdata.MobileNo : obj.MobileNo;
                    obj.UpdatedBy = UserId;
                    obj.UpdatedOn = DateTime.Now;
                }

                var AppointeeDetailsData = await _context.AppointeeDetails.Where(x => x.ActiveStatus.Value.Equals(true) && candidateIdList.Contains(x.CandidateId))?.ToListAsync();
                foreach (var obj in AppointeeDetailsData)
                {
                    var currRawdata = _appointeeList.FirstOrDefault(x => x.CandidateID == obj.CandidateId);

                    obj.DateOfJoining = !string.IsNullOrEmpty(currRawdata.DateOfJoining) ? Convert.ToDateTime(currRawdata.DateOfJoining) : obj.DateOfJoining;
                    obj.AppointeeName = (!string.IsNullOrEmpty(currRawdata.AppointeeName) && (obj.AppointeeName?.Trim() != currRawdata.AppointeeName)) ? currRawdata.AppointeeName : obj.AppointeeName;
                    if ((!string.IsNullOrEmpty(currRawdata.AppointeeName) && (obj.AppointeeName?.Trim() != currRawdata.AppointeeName)))
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
                var UnderProcessUserData = await _context.UserMaster.Where(x => x.ActiveStatus.Value.Equals(true) && candidateIdList.Contains(x.CandidateId))?.ToListAsync();
                foreach (var obj in UnderProcessUserData)
                {
                    var currRawdata = _appointeeList.FirstOrDefault(x => x.CandidateID == obj.CandidateId);
                    obj.UserName = (!string.IsNullOrEmpty(currRawdata.AppointeeName) && (obj.UserName?.Trim() != currRawdata.AppointeeName)) ? currRawdata.AppointeeName : obj.UserName;

                    //obj.EmailId = (!string.IsNullOrEmpty(currRawdata.AppointeeEmailId) && (obj.EmailId?.Trim() != currRawdata.AppointeeEmailId)) ? currRawdata.AppointeeEmailId : obj.EmailId?.Trim();
                    //obj.ContactNo = (!string.IsNullOrEmpty(currRawdata.MobileNo) && (obj.ContactNo?.Trim() != currRawdata.MobileNo)) ? currRawdata.MobileNo : obj.ContactNo;
                    obj.UpdatedBy = UserId;
                    obj.UpdatedOn = DateTime.Now;

                }
                //begin Appointee User Create
                var userList = new List<CreateUserDetailsRequest>();
                //var _userList = await _context.UserMaster.Where(x => x.ActiveStatus.Value.Equals(true) && candidateIdList.Contains(x.CandidateId)).ToListAsync();

                var querydata = from u in _context.UserMaster
                                join a in _context.UserAuthentication
                                    on u.UserId equals a.UserId
                                where u.ActiveStatus == true & candidateIdList.Contains(u.CandidateId)
                                & a.ActiveStatus == true
                                select new { u, a.UserPwdTxt };
                var _userList = await querydata.ToListAsync().ConfigureAwait(false);

                //var _userAuthList = await _context.UserAuthentication.Where(x => x.ActiveStatus.Value.Equals(true) && _userIdList.Contains(x.UserId)).ToListAsync();


                foreach (var (obj, user) in from obj in _userList
                                            let user = new CreateUserDetailsRequest()
                                            select (obj, user))
                {
                    user.UserName = obj.u?.UserName;
                    user.UserCode = obj.u?.UserCode;
                    user.Password = obj.UserPwdTxt;
                    user.EmailId = obj.u?.EmailId;
                    user.ContactNo = obj.u?.ContactNo;
                    user.CandidateId = obj.u?.CandidateId;
                    user.CompanyId = 1;
                    user.UserTypeId = (int)UserType.Appoientee;
                    user.RoleId = 0;
                    user.RefAppointeeId = obj.u?.RefAppointeeId;
                    user.UserId = UserId;
                    userList.Add(user);
                }

                await _emailSender.SendAppointeeLoginMail(userList, MailType.CandidateUpdate);
            }
            await _context.SaveChangesAsync();
        }
        public async Task createNewUserwithRole(List<CreateUserDetailsRequest> userList, int userId)
        {
            var UserAuthList = new List<UserAuthentication>();
            var UserRoleMappingList = new List<RoleUserMapping>();
            var _appointeeList = userList?.Where(x => (x.RefAppointeeId != null)).Select(y => y.RefAppointeeId).ToList();
            if (_appointeeList.Any())
            {
                var _exstingappointeeId = await _context.UserMaster.Where(x => _appointeeList.Contains(x.RefAppointeeId)).Select(y => y.RefAppointeeId).ToListAsync();
                userList = userList.Where(x => !_exstingappointeeId.Contains(x.RefAppointeeId)).ToList();
            }

            foreach (var (obj, user) in from obj in userList
                                        let user = new CreateUserDetailsRequest()
                                        select (obj, user))
            {
                var Users = new UserMaster
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
                _context.UserMaster.Add(Users);
                await _context.SaveChangesAsync();

                var UsersAuth = new UserAuthentication
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

                var UserRoleMappingData = new RoleUserMapping
                {
                    RoleId = obj.RoleId,
                    UserId = Users.UserId,
                    ActiveStatus = true,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now
                };
                UserRoleMappingList.Add(UserRoleMappingData);
            }
            _context.UserAuthentication.AddRange(UserAuthList);
            _context.RoleUserMapping.AddRange(UserRoleMappingList);
            await _context.SaveChangesAsync();
        }
        public async Task<List<UserDetails>> getAllAdminUser()
        {
            var querydata = from um in _context.UserMaster
                            join ur in _context.RoleUserMapping
                            on um.UserId equals ur.UserId
                            join r in _context.RoleMaster
                             on ur.RoleId equals r.RoleId
                            join ua in _context.UserAuthentication
                            on ur.UserId equals ua.UserId
                            where um.ActiveStatus == true & r.ActiveStatus == true
                            & ur.ActiveStatus == true
                            & r.IsCompanyAdmin == true & ua.ActiveStatus == true
                            select new UserDetails
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

            var userlist = await querydata.ToListAsync().ConfigureAwait(false);

            return userlist;
        }
        public async Task<UserDetails> getAdminUserDetails(int userId)
        {
            var querydata = from um in _context.UserMaster
                            join ur in _context.RoleUserMapping
                            on um.UserId equals ur.UserId
                            join r in _context.RoleMaster
                             on ur.RoleId equals r.RoleId
                            join ua in _context.UserAuthentication
                            on ur.UserId equals ua.UserId
                            where um.ActiveStatus == true & r.ActiveStatus == true
                            & ua.ActiveStatus == true & r.IsCompanyAdmin == true
                            & um.UserId == userId
                            select new UserDetails
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

            var userlist = await querydata.FirstOrDefaultAsync().ConfigureAwait(false);

            return userlist;
        }
        public async Task editAdminUser(AdminUserUpdateRequest userDetails)
        {
            var UserRoleMappingList = new List<RoleUserMapping>();
            var _userDetails = await _context.UserMaster.Where(x => x.UserId.Equals(userDetails.Id) && x.ActiveStatus == true).FirstOrDefaultAsync();
            var _userAuthDetails = await _context.UserAuthentication.Where(x => x.UserId.Equals(userDetails.Id) && x.ActiveStatus == true).FirstOrDefaultAsync();
            var _userRoleDetails = await _context.RoleUserMapping.Where(x => x.UserId.Equals(userDetails.Id) && x.ActiveStatus == true).FirstOrDefaultAsync();

            _userDetails.UserCode = userDetails.UserCode?.Trim();
            _userDetails.UserName = userDetails.UserName?.Trim();
            _userDetails.EmailId = userDetails.EmailId?.Trim();
            _userDetails.ContactNo = userDetails.Phone;
            _userDetails.RoleId = userDetails.RoleId ?? 0;
            _userDetails.UserTypeId = userDetails.UserTypeId;
            _userDetails.UpdatedBy = userDetails.UserId;
            _userDetails.UpdatedOn = DateTime.Now;

            if (_userAuthDetails.UserPwdTxt != userDetails.Password?.Trim())
            {
                _userAuthDetails.UserPwdTxt = userDetails.Password?.Trim();
                _userAuthDetails.UserPwd = CommonUtility.hashPassword(userDetails.Password?.Trim());
            }
            if (_userRoleDetails.RoleId != userDetails.RoleId && userDetails.RoleId != null)
            {
                _userRoleDetails.ActiveStatus = false;

                var UserRoleMappingData = new RoleUserMapping
                {
                    RoleId = userDetails.RoleId ?? 0,
                    UserId = userDetails.UserId,
                    ActiveStatus = true,
                    CreatedBy = userDetails.UserId,
                    CreatedOn = DateTime.Now
                };
                UserRoleMappingList.Add(UserRoleMappingData);
                _context.RoleUserMapping.AddRange(UserRoleMappingList);
            }
            await _context.SaveChangesAsync();
        }
        public async Task addRoleByRoleAlias(string RoleType, List<CreateUserDetailsRequest> userList, int userId)
        {
            //var _appointeeList = UserList?.Where(x => (x.RefAppointeeId != null)).Select(y => y.RefAppointeeId).ToList();
            //var UserListId = await _context.UserMaster.Where(x => _appointeeList.Contains(x.RefAppointeeId)).Select(y => y.UserId).ToListAsync();
            //var _roledata = await _context.RoleMaster.FirstOrDefaultAsync(x => x.ActiveStatus == true & x.RolesAlias.Equals(RoleType.Trim()));
            //foreach (var (obj, roleUser) in from obj in UserListId
            //                                let roleUser = new RoleUserMapping()
            //                                select (obj, roleUser))
            //{
            //    var UserRoleMappingData = new RoleUserMapping
            //    {
            //        RoleId = _roledata.RoleId,
            //        UserId = obj,
            //        ActiveStatus = true,
            //        CreatedBy = userId,
            //        CreatedOn = DateTime.Now
            //    };
            //    _context.RoleUserMapping.Add(UserRoleMappingData);
            //}

            //await _context.SaveChangesAsync();
        }
        public async Task createRole(CreateUsereRoleRequest roleDetails)
        {
            var UserRoleData = new RoleMaster
            {
                RoleName = roleDetails.RoleName,
                RolesAlias = roleDetails.RoleAlias,
                RoleDesc = roleDetails.RoleDesc,
                ActiveStatus = true,
                CreatedBy = roleDetails.UserId,
                CreatedOn = DateTime.Now
            };
            _context.RoleMaster.Add(UserRoleData);

            await _context.SaveChangesAsync();
        }
        public async Task<List<RoleDetails>> getUserRoleList()
        {
            var RoleList = new List<RoleDetails>();
            var _roleList = await _context.RoleMaster.Where(x => x.ActiveStatus == true).ToListAsync();
            RoleList = _roleList.Select(x => new RoleDetails
            {
                RoleId = x.RoleId,
                RoleName = x.RoleName,
                RoleDescription = x.RoleDesc,
                ActiveStatus = x.ActiveStatus
            }).ToList();
            return RoleList;
        }
        public async Task<RoleDetails> getRoleDetailsByRoleAlias(string roleAlias)
        {
            var RoleDetails = new RoleDetails();
            var _roledata = await _context.RoleMaster.FirstOrDefaultAsync(x => x.ActiveStatus == true & x.RolesAlias.Equals(roleAlias.Trim()));
            RoleDetails.RoleId = _roledata.RoleId;
            RoleDetails.RoleName = _roledata.RoleName;
            RoleDetails.RoleDescription = _roledata.RoleDesc;
            RoleDetails.RoleAlias = _roledata.RolesAlias;
            return RoleDetails;
        }
        public Task<RoleDetails> getRoleDetails(int RoleId)
        {
            throw new NotImplementedException();
        }
        public async Task<CompanyDetails> getCompanyDetails(int companyId)
        {
            var CompanyDetails = new CompanyDetails();
            var _companydetails = await _context.CompanyDetails.FirstOrDefaultAsync(x => x.ActiveStatus == true & x.Id.Equals(companyId));

            CompanyDetails.Id = _companydetails.Id;
            CompanyDetails.CompanyName = _companydetails.CompanyName;
            CompanyDetails.CompanyAddress = _companydetails.CompanyAddress;
            CompanyDetails.City = _companydetails.City;
            CompanyDetails.ActiveStatus = _companydetails.ActiveStatus;

            return CompanyDetails;
        }
        public async Task<List<CompanyDetails>> getCompanyList()
        {
            var CompanyList = new List<CompanyDetails>();
            var _companyList = await _context.CompanyDetails.Where(x => x.ActiveStatus == true).ToListAsync();
            CompanyList = _companyList.Select(x => new CompanyDetails
            {
                Id = x.Id,
                CompanyName = x.CompanyName,
                CompanyAddress = x.CompanyAddress,
                City = x.City,
                ActiveStatus = x.ActiveStatus
            }).ToList();

            return CompanyList;
        }
        public async Task<List<DropDownDetails>> getDropDownData(string type)
        {
            var _dataList = new List<DropDownDetails>();

            switch (type)
            {
                case CommonEnum.MasterDataType.COUNTRY:
                    _dataList = await getcountrydataAsync();
                    break;

                case CommonEnum.MasterDataType.NATIONALITY:
                    _dataList = await getnationilitydataAsync();
                    break;

                case CommonEnum.MasterDataType.GENDER:
                    _dataList = await getgenderdataAsync();
                    break;
                case CommonEnum.MasterDataType.MARATIALSTAT:
                    _dataList = await getmaratialstatdataAsync();
                    break;
                case CommonEnum.MasterDataType.DISABILITY:
                    _dataList = await getdisabilitydataAsync();
                    break;
                case CommonEnum.MasterDataType.FILETYPE:
                    _dataList = await getfiletypetatdataAsync();
                    break;
                case CommonEnum.MasterDataType.QUALIFICATION:
                    _dataList = await getqualificationdataAsync();
                    break;
                case CommonEnum.MasterDataType.ROLE:
                    _dataList = await getuserRoleAsync();
                    break;
                default:
                    return _dataList;

            }

            return _dataList;
        }

        private async Task<List<DropDownDetails>> getuserRoleAsync()
        {

            //var _rolemaster = await _context.RoleMaster.Where(m => m.ActiveStatus.Equals(true) && m.RolesAlias != RoleTypeAlias.Appointee).ToListAsync();
            var _rolemaster = await _context.RoleMaster.Where(m => m.ActiveStatus.Equals(true) && m.IsCompanyAdmin == true).ToListAsync();
            var __rolemasterList = _rolemaster?.Select(x => new DropDownDetails
            {
                Id = x.RoleId,
                Code = x.RolesAlias,
                Value = x.RoleName
            }).ToList();

            return __rolemasterList;
        }
        private async Task<List<DropDownDetails>> getnationilitydataAsync()
        {

            var _nationilitymaster = await _context.NationilityMaster.Where(m => m.ActiveStatus.Equals(true)).ToListAsync();
            var _nationalityList = _nationilitymaster?.Select(x => new DropDownDetails
            {
                Id = x.NationilityId,
                Code = x.NationilityName,
                Value = x.NationilityName
            }).ToList();

            return _nationalityList;
        }
        private async Task<List<DropDownDetails>> getcountrydataAsync()
        {
            var _countrymaster = await _context.NationilityMaster.Where(m => m.ActiveStatus.Equals(true)).ToListAsync();
            var _countryList = _countrymaster.Select(x => new DropDownDetails
            {
                Id = x.NationilityId,
                Code = x.NationName,
                Value = x.NationName
            }).ToList();
            return _countryList;
        }
        private async Task<List<DropDownDetails>> getgenderdataAsync()
        {
            var _gendermaster = await _context.GenderMaster.Where(m => m.ActiveStatus.Equals(true)).ToListAsync();
            var _genderList = _gendermaster.Select(x => new DropDownDetails
            {
                Id = x.GenderId,
                Code = x.GenderCode,
                Value = x.GenderName
            }).ToList();
            return _genderList;
        }
        private async Task<List<DropDownDetails>> getdisabilitydataAsync()
        {
            var _disabilitymaster = await _context.DisabilityMaster.Where(m => m.ActiveStatus.Equals(true)).ToListAsync();
            var _disabilityList = _disabilitymaster.Select(x => new DropDownDetails
            {
                Id = x.DisabilityId,
                Code = x.DisabilityCode,
                Value = x.DisabilityName
            }).ToList();
            return _disabilityList;
        }
        private async Task<List<DropDownDetails>> getmaratialstatdataAsync()
        {
            var _maratialstatmaster = await _context.MaratialStatusMaster.Where(m => m.ActiveStatus.Equals(true)).ToListAsync();
            var _maratialstatList = _maratialstatmaster.Select(x => new DropDownDetails
            {
                Id = x.MStatusId,
                Code = x.MStatusCode,
                Value = x.MStatusName
            }).ToList();
            return _maratialstatList;
        }
        private async Task<List<DropDownDetails>> getfiletypetatdataAsync()
        {
            var _masterdata = await _context.UploadTypeMaster.Where(m => m.ActiveStatus.Equals(true)).ToListAsync();
            var _dataList = _masterdata.Select(x => new DropDownDetails
            {
                Id = x.UploadTypeId,
                Code = x.UploadTypeCode,
                Value = x.UploadTypeName
            }).ToList();
            return _dataList;
        }
        private async Task<List<DropDownDetails>> getqualificationdataAsync()
        {
            var _masterdata = await _context.QualificationMaster.Where(m => m.ActiveStatus.Equals(true)).ToListAsync();
            var _dataList = _masterdata.Select(x => new DropDownDetails
            {
                Id = x.QualificationId,
                Code = x.QualificationCode,
                Value = x.QualificationName
            }).ToList();
            return _dataList;
        }
        public async Task<UserDetails> getUserDetailsAsyncbyId(int uid)
        {
            var userDetails = new UserDetails();
            var userDetailsQuery = from u in _context.UserMaster
                                   join a in _context.UserAuthentication
                                                       on u.UserId equals a.UserId
                                   where u.ActiveStatus == true & a.UserId == uid & a.ActiveStatus == true
                                   select new { u, a.UserPwdTxt, a.UserProfilePwd };
            var users = await userDetailsQuery.FirstOrDefaultAsync().ConfigureAwait(false);
            //var users = await _context.UserMaster.FirstOrDefaultAsync(m => m.UserId.Equals(uid) & m.ActiveStatus == true);
            var usersRole = await _context.RoleUserMapping.FirstOrDefaultAsync(m => m.UserId.Equals(uid) & m.ActiveStatus == true);
            var _roledata = await _context.RoleMaster.FirstOrDefaultAsync(x => x.ActiveStatus == true & x.RoleId.Equals(usersRole.RoleId));
            var _userDetails = await _context.AppointeeDetails.FirstOrDefaultAsync(x => x.ActiveStatus == true & x.AppointeeId == users.u.RefAppointeeId);

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
            // userDetails.CurrentStatus = users.CurrStatus ?? false;
            userDetails.IsProcessed = _userDetails?.IsProcessed ?? false;
            userDetails.IsSubmit = _userDetails?.IsSubmit ?? false;
            userDetails.IsSetProfilePassword = !string.IsNullOrEmpty(users?.UserProfilePwd);
            userDetails.CompanyId = 1;

            return userDetails;
        }
        //public async Task postUserAuthDetailsAsyncbyId(int uid, string? IpaAdress, string? browserName)
        public async Task postUserAuthDetailsAsyncbyId(UserAuthDetailsRequest req)
        {
            var userDetails = new UserDetails();
            //var users = await _context.UserMaster.FirstOrDefaultAsync(m => m.UserId.Equals(req.UserId));
            var usersauthdata = await _context.UserAuthenticationHist.Where(m => m.UserId.Equals(req.UserId) && m.ActiveStatus == true).ToListAsync();
            usersauthdata?.ForEach(x => x.ActiveStatus = false);

            var _userAuthHis = new UserAuthenticationHist
            {
                UserId = req.UserId,
                ClientId = req.ClientId,
                EntryTime = DateTime.Now,
                IPAddress = req.IpaAdress,
                BrowserName = req.browserName,
                TokenNo = req.Token,
                Otp = req.Otp,
                ActiveStatus = true,
                CreatedBy = req.UserId,
                CreatedOn = DateTime.Now
                //IsChecked = null,
            };
            _context.UserAuthenticationHist.Add(_userAuthHis);
            await _context.SaveChangesAsync();
        }
        public async Task<UserDetails> getUserDetailsbyAppointeeId(int appnteid)
        {
            var userDetails = new UserDetails();
            var users = await _context.UserMaster.FirstOrDefaultAsync(m => m.RefAppointeeId.Equals(appnteid) & m.ActiveStatus == true);
            var UnderProcessedUsersdetails = await _context.UnderProcessFileData.FirstOrDefaultAsync(m => m.AppointeeId.Equals(appnteid) & m.ActiveStatus == true);
            if (users != null)
            {
                userDetails.UserId = users?.UserId ?? 0;
                userDetails.UserName = users?.UserName;
                userDetails.EmailId = users?.EmailId;
                userDetails.Phone = users?.ContactNo;
                userDetails.RoleId = users?.RoleId;
                userDetails.UserCode = users?.UserCode;
                userDetails.UserTypeId = users.UserTypeId;
                userDetails.AppointeeId = users.RefAppointeeId;
                userDetails.CompanyName = UnderProcessedUsersdetails?.CompanyName ?? string.Empty;
                userDetails.CompanyId = 1;
            }
            return userDetails;
        }
        //public async Task<List<UserDetails>> getUserListAsync()
        //{
        //    var userList = new List<UserDetails>();

        //    var users = await _context.UserMaster.Where(x => x.ActiveStatus == true).ToListAsync();
        //    userList = users.Select(x => new UserDetails
        //    {
        //        UserId = x.UserId,
        //        UserName = x?.UserName,
        //        EmailId = x?.EmailId,
        //        Phone = x.ContactNo,
        //        RoleId = x.RoleId,
        //        UserCode = x.UserCode,
        //        UserTypeId = x.UserTypeId,
        //    }).ToList();

        //    return userList;
        //}
        public bool getUserProfileDetails(string uname)
        {
            throw new NotImplementedException();
        }
        public async Task removeUserDetails(int uid, int userId)
        {
            var applicationUsers = await _context.UserMaster.FindAsync(uid);
            applicationUsers.ActiveStatus = false;
            applicationUsers.UpdatedOn = DateTime.Now;
            applicationUsers.UpdatedBy = userId;
            await _context.SaveChangesAsync();
            //IsUserRemoved = true;

            //return IsUserRemoved;
        }
        public async Task<bool> validateUserByCode(string? userCode)
        {
            var IsValidUser = false;

            var dbusers = await _context.UserMaster.FirstOrDefaultAsync(m => m.UserCode.Equals(userCode));
            if (dbusers != null)
                IsValidUser = true;
            return IsValidUser;
        }
        public async Task<bool> validateUserById(int? id)
        {
            var IsValidUser = false;

            var dbusers = await _context.UserMaster.FirstOrDefaultAsync(m => m.UserId.Equals(id));
            if (dbusers != null)
                IsValidUser = true;
            return IsValidUser;
        }
        public async Task<int> validateUserByOtp(string? clientId, string? otp, int userType)
        {
            int _userId = 0;
            var dbusers = await _context.UserAuthenticationHist.FirstOrDefaultAsync(m => m.ClientId == clientId & m.ActiveStatus == true);

            _userId = (userType == (Int32)UserType.Appoientee) ? dbusers.Otp == otp ? dbusers.UserId : 0 : dbusers?.UserId ?? -1;
            return _userId;
        }
        public async Task<ValidateUserResponse> validateUserSignInAsync(UserSignInRequest user)
        {
            ValidateUserResponse res = new ValidateUserResponse();
            string _password = CommonUtility.hashPassword(password: user.Password);
            var dbuserStatusId = -1;
            var dbuserTypeId = -1;
            var GeneralSetup = await _context.GeneralSetup.FirstOrDefaultAsync(m => m.ActiveStatus.Equals(true));

            var dbusers = await _context.UserMaster.FirstOrDefaultAsync(m => m.UserCode.Equals(user.UserCode));

            if (dbusers != null)
            {
                dbuserTypeId = dbusers.UserTypeId;
                dbuserStatusId = (!(dbusers.ActiveStatus ?? false)) ? -2 : !(dbusers.CurrStatus ?? false) ? -4 : dbuserStatusId;
                if (dbuserStatusId == -1)
                {
                    var _authenticatedUserId = await _context.UserAuthentication.FirstOrDefaultAsync(m => m.UserId.Equals(dbusers.UserId) & m.UserPwd.Equals(_password) & m.ActiveStatus == true);

                    dbuserStatusId = _authenticatedUserId == null ? 0 : dbusers.UserId;
                    if (dbusers.RefAppointeeId != null)
                    {
                        var _userDetails = await _context.UnderProcessFileData.FirstOrDefaultAsync(m => m.AppointeeId.Equals(dbusers.RefAppointeeId) & m.ActiveStatus.Equals(true));
                        if (_userDetails.DateOfJoining < DateTime.Now.AddDays(GeneralSetup?.GracePeriod ?? 0))
                        {
                            dbuserStatusId = -3;
                        }
                    }
                }
                //else
                //{
                //    dbUserId = -2;
                //}
            }

            //res.userId = dbusers?.UserId ?? 0;
            res.clientId = string.Concat(user.UserCode, "_", CommonUtility.RandomString(8));
            res.dbUserType = dbuserTypeId;
            res.userStatus = dbuserStatusId;
            res.userMailId = dbusers?.EmailId;
            res.userName = dbusers?.UserName;
            res.userId = dbusers?.UserId ?? 0;
            return res;
        }
        public async Task<bool> validateProfilePasswowrdAsync(ValidateProfilePasswordRequest req)
        {
            ValidateUserResponse res = new ValidateUserResponse();
            string _password = CommonUtility.hashPassword(password: req.ProfilePassword);

            var _authenticatedPassword = await _context.UserAuthentication.FirstOrDefaultAsync(m => m.UserId.Equals(req.UserId) & m.ActiveStatus == true);
            bool Isvalidate = (_authenticatedPassword?.UserProfilePwd == _password);
            return Isvalidate;
        }
        public async Task EditUserProfile(EditUserProfileRequest req)
        {
            ValidateUserResponse res = new ValidateUserResponse();
            string _password = CommonUtility.hashPassword(password: req.ProfilePassword);

            var _UserauthDetails = await _context.UserAuthentication.FirstOrDefaultAsync(m => m.UserId.Equals(req.UserId) & m.ActiveStatus == true);
            if (_UserauthDetails != null)
            {
                bool Isvalidate = (_UserauthDetails?.UserProfilePwd == _password);
                if (!Isvalidate)
                {
                    _UserauthDetails.ActiveStatus = false;
                    var UsersAuth = new UserAuthentication
                    {
                        UserId = _UserauthDetails.UserId,
                        UserPwd = _UserauthDetails.UserPwd,
                        UserPwdTxt = _UserauthDetails.UserPwdTxt,
                        UserProfilePwd = CommonUtility.hashPassword(_password),
                        IsDefaultPass = CommonEnum.CheckType.yes,
                        ActiveStatus = true,
                        CreatedBy = req.UserId,
                        CreatedOn = DateTime.Now
                    };
                    _context.UserAuthentication.Add(UsersAuth);
                    await _context.SaveChangesAsync();
                }
            }
        }
        public async Task<WidgetProgressData> GetTotalProgressWidgetData()
        {
            var widgetData = new WidgetProgressData();
            var totalAppointee = 0;
            var totalUnderProcess = 0;
            var totalProcess = 0;
            var totalReject = 0;
            var totalNonProcess = 0;

            var _getapprovalStatus = await _context.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToArrayAsync();
            var ReprocessState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Reprocess?.Trim());
            var CloseState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ProcessClose?.Trim());
            var RejectState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Rejected?.Trim());
            var ApproveState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Approved?.Trim());

            var _rawdata = await _context.RawFileData.Where(m => m.ActiveStatus == true).Select(x => x.AppointeeEmailId).ToListAsync();
            //var _underProcess = await _context.WorkFlowDetails.Where(m => m.StateId == 1 || m.StateId == 2 & m.ActiveStatus == true).Select(x => x.AppointeeId).ToListAsync();
            var _nonProcess = await _context.UnProcessedFileData.Where(m => m.ActiveStatus == true).Select(x => x.CandidateId).ToListAsync();

            var _totalReject = await _context.RejectedFileData.Where(m => m.ActiveStatus == true).Select(x => x.AppointeeId).ToListAsync();

            var underprocessquerydata = from b in _context.UnderProcessFileData
                                        join w in _context.WorkFlowDetails
                                        on b.AppointeeId equals w.AppointeeId
                                        join p in _context.AppointeeDetails
                                        on b.AppointeeId equals p.AppointeeId into grouping
                                        from p in grouping.Where(x => !x.ProcessStatus.GetValueOrDefault().Equals(ReprocessState.AppvlStatusId)).DefaultIfEmpty()
                                        where (!(w.AppvlStatusId == CloseState.AppvlStatusId || w.AppvlStatusId == ApproveState.AppvlStatusId || w.AppvlStatusId == RejectState.AppvlStatusId))
                                        & (p.IsProcessed.Equals(false) || p.IsProcessed == null)
                                        //& w.AppvlStatusId!= ReprocessState.AppvlStatusId
                                        //// & b.DateOfJoining > CurrDate 
                                        & b.ActiveStatus == true
                                        //& w.CreatedOn >= startDate
                                        orderby p.IsSubmit
                                        select new { b.AppointeeId, b, p, w.AppvlStatusId };

            var list = await underprocessquerydata.ToListAsync().ConfigureAwait(false);

            var _underProcess = list?.DistinctBy(x => x.AppointeeId)?.Select(x => x.b)?.ToList();

            var _totalProcessquery = from b in _context.ProcessedFileData
                                     join w in _context.AppointeeDetails
                                     on b.AppointeeId equals w.AppointeeId
                                     where w.ActiveStatus == true && b.ActiveStatus == true
                                     select new { b.AppointeeId, b };
            var processlist = await _totalProcessquery.ToListAsync().ConfigureAwait(false);
            var _totalProcess = processlist?.DistinctBy(x => x.AppointeeId)?.Select(x => x.b)?.ToList();


            //var _underProcess= Underprocessdata.Count
            totalUnderProcess = _underProcess?.Distinct()?.ToList()?.Count() ?? 0;
            totalNonProcess = _nonProcess?.Distinct()?.ToList()?.Count() ?? 0;
            totalProcess = _totalProcess?.Distinct()?.ToList()?.Count() ?? 0;
            totalReject = _totalReject?.Distinct()?.ToList()?.Count() ?? 0;
            var totalrawdata = _rawdata?.Distinct()?.ToList()?.Count() ?? 0;
            totalAppointee = totalUnderProcess + totalNonProcess + totalProcess + totalReject;

            widgetData.TotalProcess = totalProcess;
            widgetData.TotalReject = totalReject;
            widgetData.TotalUnderProcess = totalUnderProcess;
            widgetData.TotalNonProcess = totalNonProcess;
            widgetData.TotalAppointee = totalAppointee;
            return widgetData;
        }
        public async Task<WidgetProgressData> GetTodayProgressWidgetData()
        {
            var widgetData = new WidgetProgressData();
            var totalAppointee = 0;
            var totalUnderProcess = 0;
            var totalProcess = 0;
            var totalReject = 0;
            var totalNonProcess = 0;

            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));

            var _rawdata = await _context.RawFileData.Where(m => m.ActiveStatus == true & m.CreatedOn >= _currDate).Select(x => x.AppointeeEmailId).ToListAsync();
            var _underProcess = await _context.WorkFlowDetails.Where(m => m.StateId == 1 || m.StateId == 2 & m.ActiveStatus == true & m.CreatedOn >= _currDate).Select(x => x.AppointeeId).ToListAsync();
            var _totalProcess = await _context.ProcessedFileData.Where(m => m.ActiveStatus == true & m.CreatedOn >= _currDate).Select(x => x.AppointeeId).ToListAsync();
            var _totalReject = await _context.RejectedFileData.Where(m => m.ActiveStatus == true & m.CreatedOn >= _currDate).Select(x => x.AppointeeId).ToListAsync();
            var _nonProcess = await _context.UnProcessedFileData.Where(m => m.ActiveStatus == true & m.CreatedOn >= _currDate).Select(x => x.AppointeeEmailId).ToListAsync();

            totalUnderProcess = _underProcess?.Distinct()?.ToList()?.Count() ?? 0;
            totalNonProcess = _nonProcess?.Distinct()?.ToList()?.Count() ?? 0;
            totalProcess = _totalProcess?.Distinct()?.ToList()?.Count() ?? 0;
            totalReject = _totalReject?.Distinct()?.ToList()?.Count() ?? 0;
            var totalrawdata = _rawdata?.Distinct()?.ToList()?.Count() ?? 0;
            totalAppointee = totalUnderProcess + totalNonProcess + totalProcess + totalReject;

            widgetData.TotalProcess = totalProcess;
            widgetData.TotalReject = totalReject;
            widgetData.TotalUnderProcess = totalUnderProcess;
            widgetData.TotalNonProcess = totalNonProcess;
            widgetData.TotalAppointee = totalAppointee;
            return widgetData;
        }
        public async Task<UnderProcessWidgetData> GetUndrPrcessProgressWidgetData()
        {
            var widgetData = new UnderProcessWidgetData();
            var totalUnderProcess = 0;
            var totalPendingData = 0;
            var totalReprocess = 0;
            var totalApprovalPending = 0;

            var _underProcessData = await _context.WorkFlowDetails.Where(m => m.StateId == 1 || m.StateId == 2 & m.ActiveStatus == true).ToListAsync();
            var _underProcess = _underProcessData.Select(x => x.AppointeeId).ToList();
            var _reProcessData = _underProcessData.Where(x => x.AppvlStatusId == 3).Select(y => y.AppointeeId).ToList();
            var _mailSendData = _underProcessData.Where(x => x.StateId == 1 & x.AppvlStatusId != 3).Select(y => y.AppointeeId).ToList();
            var _pendingDataSubmit = await _context.AppointeeDetails.Where(m => m.IsProcessed == false & m.IsSubmit == false & _mailSendData.Contains(m.AppointeeId)).Select(x => x.AppointeeId).ToListAsync();
            var _PendingApproval = await _context.AppointeeDetails.Where(m => m.IsProcessed == false & m.IsSubmit == true).Select(x => x.AppointeeId).ToListAsync();

            //totalUnderProcess = _underProcess?.Distinct()?.ToList()?.Count() ?? 0;
            totalUnderProcess = 0;
            // totalPendingData = _pendingDataSubmit?.Distinct()?.ToList()?.Count() ?? 0;
            totalPendingData = _mailSendData?.Distinct()?.ToList()?.Count() ?? 0;
            totalReprocess = _reProcessData?.Distinct()?.ToList()?.Count() ?? 0;
            totalApprovalPending = _PendingApproval?.Distinct()?.ToList()?.Count() ?? 0;


            widgetData.TotalUnderProcess = totalUnderProcess;
            widgetData.TotalReprocess = totalReprocess;
            widgetData.TotalDataPending = totalPendingData;
            widgetData.TotalApprovalPending = totalApprovalPending;

            return widgetData;
        }
        public async Task<int> GetProgressStatusWidgetData()
        {
            var _mailSent = await _context.WorkFlowDetails.Where(m => m.StateId == 1 & m.ActiveStatus == true).ToListAsync();
            var _datasubmitted = await _context.WorkFlowDetails.Where(m => m.StateId == 2 & m.ActiveStatus == true).ToListAsync();
            var _dataVarified = await _context.WorkFlowDetails.Where(m => m.StateId == 3 & m.ActiveStatus == true).ToListAsync();

            var querydata = from p in _context.AppointeeDetails
                            join w in _context.WorkFlowDetails
                            on p.AppointeeId equals w.AppointeeId
                            where w.StateId == 1 & p.SaveStep != 0
                            select new { p };

            var dataSavelist = await querydata.ToListAsync().ConfigureAwait(false);
            return _mailSent?.Count ?? 0;
        }
        public async Task<CriticalAppointeeWidgetResponse> GetCriticalData()
        {
            var returnObj = new CriticalAppointeeWidgetResponse();
            int criticaldata = 0;
            var generalsetupData = await _context.GeneralSetup.Where(x => x.ActiveStatus == true).ToListAsync();
            var filterDaysrange = generalsetupData.FirstOrDefault()?.CriticalNoOfDays ?? 0;
            // var filterDaysrange =  _configSetup?.CriticalDaysLimit ?? 0;
            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            var maxDate = _currDate.AddDays(filterDaysrange);
            var _getapprovalStatus = await _context.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToArrayAsync();
            var ApprovedState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Approved?.Trim());
            var ForcedApprovedState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ForcedApproved?.Trim());
            var CloseState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ProcessClose?.Trim());
            var RejectedState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Rejected?.Trim());

            var criticalunderprocess = from b in _context.UnderProcessFileData
                                       join w in _context.WorkFlowDetails
                                       on b.AppointeeId equals w.AppointeeId
                                       join p in _context.AppointeeDetails
                                       on b.AppointeeId equals p.AppointeeId into grouping
                                       from p in grouping //.Where(x => !x.ProcessStatus.GetValueOrDefault().Equals(ReprocessState.AppvlStatusId)).DefaultIfEmpty()
                                       where w.AppvlStatusId != CloseState.AppvlStatusId
                                       & w.AppvlStatusId != RejectedState.AppvlStatusId
                                       & w.AppvlStatusId != ApprovedState.AppvlStatusId
                                       & w.AppvlStatusId != ForcedApprovedState.AppvlStatusId
                                       & b.DateOfJoining <= maxDate & b.DateOfJoining >= _currDate & b.ActiveStatus == true & p.ActiveStatus == true
                                       orderby p.IsSubmit
                                       select new { b, p, w.AppvlStatusId };

            var UnderProcessData = await criticalunderprocess.OrderByDescending(x => x.b.DateOfJoining).ToListAsync().ConfigureAwait(false);

            // var UnderProcessData = await _context.UnderProcessFileData.Where(m => m.DateOfJoining <= maxDate && m.DateOfJoining >= _currDate && m.ActiveStatus == true).ToListAsync();
            var NonProcessData = await _context.UnProcessedFileData.Where(m => m.DateOfJoining <= maxDate && m.DateOfJoining >= _currDate && m.ActiveStatus == true).ToListAsync();
            criticaldata = (UnderProcessData?.Count ?? 0) + (NonProcessData?.Count ?? 0);
            returnObj.TotalCriticalAppointee = criticaldata;
            returnObj.UnderProcessCriticalAppointee = UnderProcessData?.Count ?? 0;
            returnObj.NonProcessCriticalAppointee = NonProcessData?.Count ?? 0;
            returnObj.CriticalDaysNo = filterDaysrange;
            return returnObj;
        }
        public async Task<List<DashboardWidgetResponse>> GetWidgetData(int? filterDays, bool isfilterd)
        {
            var Wizdata = new List<DashboardWidgetResponse>();
            //var isfilterd = !isfilterd;
            var filterDaysrange = filterDays ?? 0;

            var _totaloffer = await GetTotalOffer(filterDaysrange, isfilterd);
            var AppointeeListData = _totaloffer.AppointeeList?.Select(x => x.AppointeeId)?.ToList();
            var _varifieddata = await GetVerifiedData(filterDaysrange, isfilterd, AppointeeListData);
            var _underProcessedAppointeeListData = await GetUnderProcessedAppointeeList(filterDaysrange, isfilterd, AppointeeListData);
            var _noResponsedata = await GetNoResponsedData(filterDaysrange, isfilterd, _underProcessedAppointeeListData);
            var _underProcessdata = await GetUnderProcessedData(filterDaysrange, isfilterd, _underProcessedAppointeeListData);
            var _lapsedData = await GetLapsedData(filterDaysrange, isfilterd, _underProcessedAppointeeListData);
            var _lonkNotSentdata = await GetLinkNotSentData(filterDaysrange, isfilterd);
            Wizdata.Add(_totaloffer.WizResData);
            Wizdata.Add(_varifieddata);
            Wizdata.Add(_noResponsedata);
            Wizdata.Add(_underProcessdata);
            Wizdata.Add(_lapsedData);
            Wizdata.Add(_lonkNotSentdata);
            return Wizdata;
        }
        private async Task<DashboardWidgetResponse> GetVerifiedData(int filterDays, bool isfilterd, List<int?> appointeeList)
        {
            var Response = new DashboardWidgetResponse();

            var filterDaysrange = filterDays;
            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            //var startDate = _currDate.AddDays(-filterDaysrange);
            DateTime? startDate = isfilterd && filterDays > 0 ? _currDate.AddDays(-filterDaysrange) : null;
            var VerifiedData = new DashboardCardWidgetResponse();
            VerifiedData.WidgetTypeName = "Verified";

            VerifiedData.WidgetFilterDays = filterDays;

            var querydata = from p in _context.ProcessedFileData
                            join u in _context.UnderProcessFileData
                                on p.AppointeeId equals u.AppointeeId
                            join x in _context.AppointeeDetails
                                  on p.AppointeeId equals x.AppointeeId into grouping
                            from a in grouping.DefaultIfEmpty()
                            where p.ActiveStatus == true //& a.ActiveStatus == true & a.IsProcessed == true
                            & (startDate == null || p.CreatedOn >= startDate)
                            select new { p.AppointeeId, p };

            var list = await querydata.ToListAsync().ConfigureAwait(false);
            var data = list.Where(x => appointeeList.Contains(x.AppointeeId)).DistinctBy(x => x.AppointeeId)?.Select(x => x.p).ToList();


            //var data = await _context.ProcessedFileData.Where(m => m.CreatedOn >= startDate && m.ActiveStatus == true && AppointeeList.Contains(m.AppointeeId)).ToListAsync();

            VerifiedData.WidgetTypeValue = data?.Count() ?? 0;

            var groupdateValue = data.GroupBy(x => Convert.ToDateTime(x.CreatedOn?.ToString("dd/MM/yyyy")))?.Select(x => new
            {
                Value = x.Count(),
                Date = (DateTime)x.Key
            })?.ToList();

            var ChartData = new List<int>();
            if (startDate != null)
            {
                for (int i = 0; i < filterDaysrange; i++)
                {
                    var tempcurDate = startDate?.AddDays(i);
                    var _currgropdate = groupdateValue.Where(x => x.Date.Equals(tempcurDate));
                    var chartval = _currgropdate.Any() ? _currgropdate.FirstOrDefault().Value : 0;
                    ChartData.Add(chartval);
                }
            }
            VerifiedData.WidgetChartValue = ChartData;

            Response.WidgetTypeCode = "VIRFD";
            Response.WidgetValue = VerifiedData;
            return Response;
        }
        private async Task<WidgetTotalOfferResponse> GetTotalOffer(int filterDays, bool isfilterd)
        {
            var Response = new WidgetTotalOfferResponse();
            var wizResponse = new DashboardWidgetResponse();
            var filterDaysrange = filterDays;
            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            DateTime? startDate = filterDays > 0 ? _currDate.AddDays(-filterDaysrange) : null;
            //List<DateTime> dates = Enumerable.Range(0, 1 + _currDate.Subtract(startDate).Days).Select(i => startDate.AddDays(i)).ToList();
            var TotalOffer = new DashboardCardWidgetResponse();
            TotalOffer.WidgetTypeName = "Total Offers in this period";
            TotalOffer.WidgetFilterDays = filterDays;

            var querydata = from w in _context.WorkFlowDetailsHist
                            join u in _context.UnderProcessFileData
                            on w.AppointeeId equals u.AppointeeId
                            where w.StateId.Equals(1) & w.AppvlStatusId.Equals(1) & u.ActiveStatus.Equals(true) &
                            (startDate == null || w.CreatedOn >= startDate)
                            select new { w };
            // var data = await _context.WorkFlowDetailsHist.Where(m => m.StateId == 1 & m.AppvlStatusId == 1 & m.CreatedOn >= startDate).ToListAsync();
            var data = await querydata.ToListAsync().ConfigureAwait(false);
            var workfdata = data.Select(x => x.w).ToList();
            var appinteeList = workfdata?.DistinctBy(x => x.AppointeeId).ToList();

            var groupdateValue = appinteeList.GroupBy(x => Convert.ToDateTime(x.CreatedOn?.ToString("dd/MM/yyyy")))?.Select(x => new
            {
                Value = x.Count(),
                Date = (DateTime)x.Key
            })?.ToList();

            var UnProcesseddata = await _context.UnProcessedFileData.Where(m => (startDate == null || m.CreatedOn >= startDate) && m.ActiveStatus == true).ToListAsync();

            var groupunprocessdateValue = UnProcesseddata.GroupBy(x => Convert.ToDateTime(x.CreatedOn?.ToString("dd/MM/yyyy")))?.Select(x => new
            {
                Value = x.Count(),
                Date = (DateTime)x.Key
            })?.ToList();
            var ChartData = new List<int>();
            if (startDate != null)
            {
                for (int i = 0; i < filterDaysrange; i++)
                {
                    var tempcurDate = startDate?.AddDays(i);
                    var _currgropdate = groupdateValue.Where(x => x.Date.Equals(tempcurDate));
                    var _currunprocessgropdate = groupunprocessdateValue.Where(x => x.Date.Equals(tempcurDate));
                    var chartvalLinksent = _currgropdate.Any() ? _currgropdate.FirstOrDefault().Value : 0;
                    var chartvalLinkNotSent = _currunprocessgropdate.Any() ? _currunprocessgropdate.FirstOrDefault().Value : 0;
                    var chartval = (chartvalLinksent + chartvalLinkNotSent);
                    ChartData.Add(chartval);
                }
            }
            TotalOffer.WidgetTypeValue = (appinteeList?.Count() ?? 0) + (UnProcesseddata?.Count() ?? 0);
            TotalOffer.WidgetChartValue = ChartData;
            wizResponse.WidgetTypeCode = "TOTLOFFR";
            wizResponse.WidgetValue = TotalOffer;
            Response.WizResData = wizResponse;
            Response.AppointeeList = appinteeList;
            return Response;
        }
        private async Task<List<UnderProcessDetailsResponse>> GetUnderProcessedAppointeeList(int filterDays, bool isfilterd, List<int?> appointeeList)
        {
            var filterDaysrange = filterDays;
            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            //var startDate = _currDate.AddDays(-filterDaysrange);
            DateTime CurrDate = Convert.ToDateTime(_currDate);
            DateTime? startDate = filterDays > 0 ? _currDate.AddDays(-filterDaysrange) : null;
            var _getapprovalStatus = await _context.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToArrayAsync();
            var ReprocessState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Reprocess?.Trim());
            var CloseState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ProcessClose?.Trim());
            var RejectState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Rejected?.Trim());
            var ApproveState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Approved?.Trim());


            var querydata = from b in _context.UnderProcessFileData
                            join w in _context.WorkFlowDetails
                            on b.AppointeeId equals w.AppointeeId
                            join p in _context.AppointeeDetails
                            on b.AppointeeId equals p.AppointeeId into grouping
                            from p in grouping.Where(x => !x.ProcessStatus.GetValueOrDefault().Equals(ReprocessState.AppvlStatusId)).DefaultIfEmpty()
                            where (!(w.AppvlStatusId == CloseState.AppvlStatusId || w.AppvlStatusId == ApproveState.AppvlStatusId || w.AppvlStatusId == RejectState.AppvlStatusId))
                            //where w.AppvlStatusId != CloseState.AppvlStatusId
                            & (p.IsProcessed.Equals(false) || p.IsProcessed == null)
                            //& w.AppvlStatusId!= ReprocessState.AppvlStatusId
                            //// & b.DateOfJoining > CurrDate 
                            & b.ActiveStatus == true & (startDate == null || w.CreatedOn >= startDate)
                            orderby p.IsSubmit
                            select new { b.AppointeeId, b, p, w.AppvlStatusId };

            var list = await querydata.ToListAsync().ConfigureAwait(false);

            var _underProcessViewdata = list?.DistinctBy(x => x.AppointeeId).Where(x => appointeeList.Contains(x.AppointeeId)).Select(row => new UnderProcessDetailsResponse
            {
                id = row.b.UnderProcessId,
                fileId = row.b.FileId,
                companyId = row.b.CompanyId,
                appointeeName = row.p?.AppointeeName ?? row.b.AppointeeName,
                appointeeId = row.b.AppointeeId,
                appointeeEmailId = row.p?.AppointeeEmailId ?? row.b.AppointeeEmailId,
                mobileNo = row.b.MobileNo,
                isPFverificationReq = row.b.IsPFverificationReq,
                epfWages = row.p?.EPFWages ?? row.b.EPFWages,
                dateOfOffer = row.b.DateOfOffer,
                dateOfJoining = row.p?.DateOfJoining ?? row.b.DateOfJoining,
                isDocSubmitted = row.p?.IsSubmit ?? false,
                isReprocess = false,
                Status = row.p?.IsSubmit ?? false ? "Ongoing" : row.p?.SaveStep == 1 ? "Ongoing" : "No Response",
                StatusCode = row.p?.IsSubmit ?? false ? 2 : row.p?.SaveStep ?? 0,
                CreatedDate = row.b?.CreatedOn
            }).OrderByDescending(x => x.isDocSubmitted).ThenBy(y => y.dateOfJoining).ToList();


            return _underProcessViewdata;
        }
        private async Task<DashboardWidgetResponse> GetUnderProcessedData(int filterDays, bool isfilterd, List<UnderProcessDetailsResponse> appointeeList)
        {
            var Response = new DashboardWidgetResponse();
            var filterDaysrange = filterDays;
            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            //var startDate = _currDate.AddDays(-filterDaysrange);
            DateTime? startDate = filterDays > 0 ? _currDate.AddDays(-filterDaysrange) : null;
            var UnderProcessedData = new DashboardCardWidgetResponse();
            UnderProcessedData.WidgetTypeName = "Processing";

            UnderProcessedData.WidgetFilterDays = filterDays;

            var data = appointeeList.Where(m => m.StatusCode != 0 & m.dateOfJoining >= _currDate).ToList();

            UnderProcessedData.WidgetTypeValue = data?.Count() ?? 0;

            var groupdateValue = data.GroupBy(x => Convert.ToDateTime(x.CreatedDate?.ToString("dd/MM/yyyy")))?.Select(x => new
            {
                Value = x.Count(),
                Date = (DateTime)x.Key
            })?.ToList();

            var ChartData = new List<int>();
            if (startDate != null)
            {
                for (int i = 0; i < filterDaysrange; i++)
                {
                    var tempcurDate = startDate?.AddDays(i);
                    var _currgropdate = groupdateValue.Where(x => x.Date.Equals(tempcurDate));
                    var chartval = _currgropdate.Any() ? _currgropdate.FirstOrDefault().Value : 0;
                    ChartData.Add(chartval);
                }
            }
            UnderProcessedData.WidgetChartValue = ChartData;
            Response.WidgetTypeCode = "UNDPRCS";
            Response.WidgetValue = UnderProcessedData;
            return Response;
        }
        private async Task<DashboardWidgetResponse> GetNoResponsedData(int filterDays, bool isfilterd, List<UnderProcessDetailsResponse> appointeeList)
        {
            var Response = new DashboardWidgetResponse();
            var UnderProcessedData = new DashboardCardWidgetResponse();
            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            //var startDate = _currDate.AddDays(-filterDays);
            DateTime? startDate = filterDays > 0 ? _currDate.AddDays(-filterDays) : null;
            UnderProcessedData.WidgetTypeName = "No Response";

            UnderProcessedData.WidgetFilterDays = filterDays;

            var data = appointeeList.Where(x => x.StatusCode == 0 & x.dateOfJoining > _currDate).ToList();

            UnderProcessedData.WidgetTypeValue = data?.Count() ?? 0;

            var groupdateValue = data.GroupBy(x => Convert.ToDateTime(x.CreatedDate?.ToString("dd/MM/yyyy")))?.Select(x => new
            {
                Value = x.Count(),
                Date = (DateTime)x.Key
            })?.ToList();

            var ChartData = new List<int>();
            if (startDate != null)
            {
                for (int i = 0; i < filterDays; i++)
                {
                    var tempcurDate = startDate?.AddDays(i);
                    var _currgropdate = groupdateValue.Where(x => x.Date.Equals(tempcurDate));
                    var chartval = _currgropdate.Any() ? _currgropdate.FirstOrDefault().Value : 0;
                    ChartData.Add(chartval);
                }
            }
            UnderProcessedData.WidgetChartValue = ChartData;
            Response.WidgetTypeCode = "NORES";
            Response.WidgetValue = UnderProcessedData;
            return Response;
        }
        private async Task<DashboardWidgetResponse> GetLapsedData(int filterDays, bool isfilterd, List<UnderProcessDetailsResponse> appointeeList)
        {
            var Response = new DashboardWidgetResponse();
            var UnderProcessedData = new DashboardCardWidgetResponse();
            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            //var startDate = _currDate.AddDays(-filterDays);
            DateTime? startDate = filterDays > 0 ? _currDate.AddDays(-filterDays) : null;
            UnderProcessedData.WidgetTypeName = "Lapsed";

            UnderProcessedData.WidgetFilterDays = filterDays;

            var data = appointeeList.Where(x => x.dateOfJoining < _currDate).ToList();

            UnderProcessedData.WidgetTypeValue = data?.Count() ?? 0;

            var groupdateValue = data.GroupBy(x => Convert.ToDateTime(x.CreatedDate?.ToString("dd/MM/yyyy")))?.Select(x => new
            {
                Value = x.Count(),
                Date = (DateTime)x.Key
            })?.ToList();

            var ChartData = new List<int>();
            if (startDate != null)
            {
                for (int i = 0; i < filterDays; i++)
                {
                    var tempcurDate = startDate?.AddDays(i);
                    var _currgropdate = groupdateValue.Where(x => x.Date.Equals(tempcurDate));
                    var chartval = _currgropdate.Any() ? _currgropdate.FirstOrDefault().Value : 0;
                    ChartData.Add(chartval);
                }
            }
            UnderProcessedData.WidgetChartValue = ChartData;
            Response.WidgetTypeCode = "LAPSED";
            Response.WidgetValue = UnderProcessedData;
            return Response;
        }
        private async Task<DashboardWidgetResponse> GetLinkNotSentData(int filterDays, bool isfilterd)
        {
            var Response = new DashboardWidgetResponse();
            var filterDaysrange = filterDays;
            var _currDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            //var startDate = _currDate.AddDays(-filterDaysrange);
            DateTime? startDate = filterDays > 0 ? _currDate.AddDays(-filterDays) : null;
            //List<DateTime> dates = Enumerable.Range(0, 1 + _currDate.Subtract(startDate).Days).Select(i => startDate.AddDays(i)).ToList();
            var LinkNotSentData = new DashboardCardWidgetResponse();

            LinkNotSentData.WidgetTypeName = "Link Not Sent";
            LinkNotSentData.WidgetFilterDays = filterDays;

            var data = await _context.UnProcessedFileData.Where(m => (startDate == null || m.CreatedOn >= startDate) && m.ActiveStatus == true).ToListAsync();

            LinkNotSentData.WidgetTypeValue = data?.Count() ?? 0;

            var groupdateValue = data.GroupBy(x => Convert.ToDateTime(x.CreatedOn?.ToString("dd/MM/yyyy")))?.Select(x => new
            {
                Value = x.Count(),
                Date = (DateTime)x.Key
            })?.ToList();


            var ChartData = new List<int>();
            if (startDate != null)
            {
                for (int i = 0; i < filterDaysrange; i++)
                {
                    var tempcurDate = startDate?.AddDays(i);
                    var _currgropdate = groupdateValue.Where(x => x.Date.Equals(tempcurDate));
                    var chartval = _currgropdate.Any() ? _currgropdate.FirstOrDefault().Value : 0;
                    ChartData.Add(chartval);
                }
            }
            LinkNotSentData.WidgetChartValue = ChartData;
            Response.WidgetTypeCode = "NTVRFD";
            Response.WidgetValue = LinkNotSentData;
            return Response;
        }
        public async Task<List<AppointeeStatusWizResponse>> GetAppointeeStatusWidgetData(string code)
        {
            var Wizdata = new List<AppointeeStatusWizResponse>();
            switch (code)
            {
                case FilterCode.UNDERPROCESS:
                    Wizdata = await GeTop5tUnderProcessDataAsync(1);
                    break;
                case FilterCode.LINKNOTSENT:
                    Wizdata = await GetTop5NonProcessDataAsync(1);
                    break;
                case FilterCode.VERIFIED:
                    Wizdata = await GetTop5ProcessDataAsync(1);
                    break;
                case FilterCode.REJECTED:
                    Wizdata = await GetTop5RejectedDataAsync(1);
                    break;
                case FilterCode.LAPSED:
                    Wizdata = await GetTop5LapsedDataAsync(1);
                    break;
                    //default:
                    //    _errormsg = string.Empty;
                    //    break;

            }

            return Wizdata;
        }
        private async Task<List<AppointeeStatusWizResponse>> GeTop5tUnderProcessDataAsync(int? companyId)
        {
            string CurrDate = DateTime.Now.ToShortDateString();
            DateTime _CurrDate = Convert.ToDateTime(CurrDate);
            var _getapprovalStatus = await _context.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToArrayAsync();
            var ReprocessState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Reprocess?.Trim());
            var CloseState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ProcessClose?.Trim());

            var querydata = from b in _context.UnderProcessFileData
                            join w in _context.WorkFlowDetails
                            on b.AppointeeId equals w.AppointeeId
                            join p in _context.AppointeeDetails
                            on b.AppointeeId equals p.AppointeeId into grouping
                            from p in grouping.Where(x => !x.ProcessStatus.GetValueOrDefault().Equals(ReprocessState.AppvlStatusId)).DefaultIfEmpty()
                            where w.AppvlStatusId != CloseState.AppvlStatusId & (p.IsProcessed.Equals(false) || p.IsProcessed == null)
                            & b.DateOfJoining > _CurrDate & b.ActiveStatus == true
                            orderby p.IsSubmit
                            select new { b, p, w.AppvlStatusId };

            var list = await querydata.OrderByDescending(x => x.p.DateOfJoining).ToListAsync().ConfigureAwait(false);
            var distinctdata = list.DistinctBy(x => x.b.AppointeeId).Take(5).ToList();
            var _underProcessViewdata = distinctdata.Select(row => new AppointeeStatusWizResponse
            {
                id = row.b.UnderProcessId,
                fileId = row.b.FileId,
                companyId = row.b.CompanyId,
                appointeeName = row.p?.AppointeeName ?? row.b.AppointeeName,
                appointeeId = row.b.AppointeeId,
                appointeeEmailId = row.p?.AppointeeEmailId ?? row.b.AppointeeEmailId,
                mobileNo = row.b.MobileNo,
                IsReprocess = (row.AppvlStatusId == (ReprocessState.AppvlStatusId)),
                dateOfJoining = row.p?.DateOfJoining ?? row.b.DateOfJoining,
                Status = row.p?.IsSubmit ?? false ? "Ongoing" : row.p?.SaveStep == 1 ? "Ongoing" : "No Response",
                StatusCode = row.p?.IsSubmit ?? false ? 2 : row.p?.SaveStep == 1 ? 2 : 1,
                CreatedDate = row.b?.CreatedOn
            })?.ToList();


            return _underProcessViewdata;
        }
        private async Task<List<AppointeeStatusWizResponse>> GetTop5NonProcessDataAsync(int? companyId)
        {
            var _unProcessData = await _context.UnProcessedFileData.Where(x => x.CompanyId.Equals(companyId) & x.ActiveStatus == true).OrderByDescending(x => x.DateOfJoining).Take(5).ToListAsync();

            var _unProcessViewdata = _unProcessData.Select(row => new AppointeeStatusWizResponse
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
        private async Task<List<AppointeeStatusWizResponse>> GetTop5ProcessDataAsync(int? companyId)
        {
            var approvalStatus = await GetApprovalState(WorkFlowType.ProcessClose);
            var querydata = from p in _context.ProcessedFileData
                            join a in _context.AppointeeDetails
                                on p.AppointeeId equals a.AppointeeId
                            where p.ActiveStatus == true & a.ActiveStatus == true & a.IsProcessed == true
                            & a.ProcessStatus != approvalStatus.AppvlStatusId
                            //& (filter.FromDate == null || p.CreatedOn >= filter.FromDate)
                            //& (filter.ToDate == null || p.CreatedOn <= filter.ToDate)
                            //& (filter.IsPfRequired == null || a.IsPFverificationReq == filter.IsPfRequired)
                            select new { p.ProcessedId, a };

            var list = await querydata.OrderByDescending(x => x.a.DateOfJoining).ToListAsync().ConfigureAwait(false);
            var distinctdata = list.DistinctBy(x => x.a.AppointeeId).Take(5).ToList();
            var _processViewdata = distinctdata?.Select(row => new AppointeeStatusWizResponse
            {
                id = row.ProcessedId,
                companyId = row.a.CompanyId,
                appointeeName = row.a.AppointeeName,
                appointeeId = row.a.AppointeeId,
                appointeeEmailId = row.a.AppointeeEmailId,
                mobileNo = row.a.MobileNo,
                dateOfJoining = row.a.DateOfJoining,
                IsReprocess = false,
                Status = "Verified",
                StatusCode = 3,
            })?.ToList();


            return _processViewdata;
        }
        private async Task<List<AppointeeStatusWizResponse>> GetTop5RejectedDataAsync(int? companyId)
        {

            var pAppntedata = from r in _context.RejectedFileData
                              join a in _context.AppointeeDetails
                                  on r.AppointeeId equals a.AppointeeId
                              join rp in _context.AppointeeReasonMappingData
                              on a.AppointeeId equals rp.AppointeeId
                              join rm in _context.ReasonMaser
                              on rp.ReasonId equals rm.ReasonId
                              where r.ActiveStatus == true & rm.ActiveStatus == true
                              & a.ActiveStatus == true
                              // & a.ProcessStatus != approvalStatus.AppvlStatusId
                              //& (filter.FromDate == null || r.CreatedOn >= filter.FromDate)
                              //& (filter.ToDate == null || r.CreatedOn <= filter.ToDate)
                              select new
                              {
                                  r.RejectedId,
                                  a.CompanyId,
                                  a.AppointeeId,
                                  a.AppointeeName,
                                  a.DateOfBirth,
                                  a.MobileNo,
                                  a.AppointeeEmailId,
                                  a.DateOfJoining,
                                  a.PANNumber,
                                  a.AadhaarNumberView,
                                  rp.Remarks
                              };



            var rejectedAppointeeList = await pAppntedata.OrderByDescending(x => x.DateOfJoining).ToListAsync().ConfigureAwait(false);
            var distinctdata = rejectedAppointeeList.DistinctBy(x => x.AppointeeId).Take(5).ToList();
            var _rejectedViewdata = distinctdata.Select(row => new AppointeeStatusWizResponse
            {
                id = row.RejectedId,
                companyId = row.CompanyId,
                appointeeName = row.AppointeeName,
                appointeeId = row.AppointeeId,
                appointeeEmailId = row.AppointeeEmailId,
                mobileNo = row.MobileNo,
                dateOfJoining = row.DateOfJoining,
                IsReprocess = false,
                Status = "Cancelled",
                StatusCode = 4,
            })?.ToList();

            return _rejectedViewdata;
        }
        private async Task<List<AppointeeStatusWizResponse>> GetTop5LapsedDataAsync(int? companyId)
        {
            string CurrDate = DateTime.Now.ToShortDateString();
            DateTime _CurrDate = Convert.ToDateTime(CurrDate);
            var _getapprovalStatus = await _context.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToArrayAsync();
            var ReprocessState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.Reprocess?.Trim());
            var CloseState = _getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == WorkFlowType.ProcessClose?.Trim());

            var querydata = from b in _context.UnderProcessFileData
                            join w in _context.WorkFlowDetails
                            on b.AppointeeId equals w.AppointeeId
                            join p in _context.AppointeeDetails
                            on b.AppointeeId equals p.AppointeeId into grouping
                            from p in grouping.Where(x => !x.ProcessStatus.GetValueOrDefault().Equals(ReprocessState.AppvlStatusId)).DefaultIfEmpty()
                            where w.AppvlStatusId != CloseState.AppvlStatusId & (p.IsProcessed.Equals(false) || p.IsProcessed == null)
                             //& w.AppvlStatusId!= ReprocessState.AppvlStatusId
                             & b.DateOfJoining < _CurrDate
                            & b.ActiveStatus == true //& w.CreatedOn >= startDate
                            orderby p.IsSubmit
                            select new { b.AppointeeId, b.DateOfJoining, b, p, };

            var list = await querydata.OrderByDescending(x => x.DateOfJoining).ToListAsync().ConfigureAwait(false);
            var LapsedData = list.DistinctBy(y => y.AppointeeId).Take(5).ToList();
            var _lapsedViewdata = LapsedData?.Select(row => new AppointeeStatusWizResponse
            {
                id = row.b.UnderProcessId,
                companyId = row.b.CompanyId,
                appointeeName = row.p?.AppointeeName ?? row.b.AppointeeName,
                appointeeId = row.b.AppointeeId,
                appointeeEmailId = row.p?.AppointeeEmailId ?? row.b.AppointeeEmailId,
                mobileNo = row.b.MobileNo,
                dateOfJoining = row.p?.DateOfJoining ?? row.b.DateOfJoining,
                IsReprocess = false,
                Status = "Lapsed",
                StatusCode = 5,
            })?.ToList();


            return _lapsedViewdata;
        }
        private async Task<WorkflowApprovalStatusMaster> GetApprovalState(string approvalStatus)
        {
            var approvalState = new WorkflowApprovalStatusMaster();
            var getapprovalStatus = await _context.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToArrayAsync();
            if (!string.IsNullOrEmpty(approvalStatus))
            {
                approvalState = getapprovalStatus.FirstOrDefault(x => x.AppvlStatusCode?.Trim() == approvalStatus?.Trim());
            }
            return approvalState;
        }
    }
}
