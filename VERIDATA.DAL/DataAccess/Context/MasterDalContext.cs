using Microsoft.EntityFrameworkCore;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.DAL.DBContext;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Admin;
using VERIDATA.Model.Table.Master;

namespace VERIDATA.DAL.DataAccess.Context
{
    public class MasterDalContext : IMasterDalContext
    {
        private readonly DbContextDalDB _dbContextClass;

        public MasterDalContext(DbContextDalDB dbContextClass)
        {
            _dbContextClass = dbContextClass;
        }

        public async Task<GeneralSetup> GetGeneralSetupData()
        {
            GeneralSetup? generalSetup = await _dbContextClass.GeneralSetup.FirstOrDefaultAsync(m => m.ActiveStatus.Equals(true)) ?? new GeneralSetup();
            return generalSetup;
        }

        public async Task<List<MenuMaster>> GetMasterMenuData()
        {
            List<MenuMaster> menuDataList = new();
            menuDataList = await _dbContextClass.MenuMaster.Where(m => m.ActiveStatus == true).ToListAsync();
            return menuDataList;
        }

        public async Task<List<WorkflowApprovalStatusMaster>> GetAllApprovalStateMaster()
        {
            List<WorkflowApprovalStatusMaster> approvalStateList = new();

            approvalStateList = await _dbContextClass.WorkflowApprovalStatusMaster.Where(x => x.ActiveStatus == true).ToListAsync();
            return approvalStateList;
        }

        public async Task<List<EscalationLevelMasterDataResponse>> GetEscalationLevelMasterData()
        {
            List<EscalationLevelMasterDataResponse> data = new();
            IQueryable<EscalationLevelMasterDataResponse> getlevelquerydata = from a in _dbContextClass.EscalationLevelMaster
                                                                              join b in _dbContextClass.EscalationLevelEmailMapping
                                                                                on a.LevelId equals b.LevelId
                                                                              where a.ActiveStatus == true && b.ActiveStatus == true
                                                                              select new EscalationLevelMasterDataResponse
                                                                              {
                                                                                  LevelId = a.LevelId,
                                                                                  LevelCode = a.LevelCode,
                                                                                  LevelName = a.LevelName,
                                                                                  SetupAlias = a.SetupAlias,
                                                                                  NoOfDays = a.NoOfDays ?? 0,
                                                                                  Emailaddress = b.Email
                                                                              };

            data = await getlevelquerydata.ToListAsync().ConfigureAwait(false);

            return data;
        }

        public async Task<List<DropDownDetailsResponse>> getCountryDataAsync()
        {
            List<NationilityMaster> _countrymaster = await _dbContextClass.NationilityMaster.Where(m => m.ActiveStatus.Equals(true)).ToListAsync();
            List<DropDownDetailsResponse> _countryList = _countrymaster.Select(x => new DropDownDetailsResponse
            {
                Id = x.NationilityId,
                Code = x.NationName,
                Value = x.NationName
            }).ToList();
            return _countryList;
        }

        public async Task<List<DropDownDetailsResponse>> getNationilityDataAsync()
        {
            List<NationilityMaster> _nationilitymaster = await _dbContextClass.NationilityMaster.Where(m => m.ActiveStatus.Equals(true)).ToListAsync();
            List<DropDownDetailsResponse>? _nationalityList = _nationilitymaster.Select(x => new DropDownDetailsResponse
            {
                Id = x.NationilityId,
                Code = x.NationilityName,
                Value = x.NationilityName
            }).ToList();

            return _nationalityList;
        }

        public async Task<List<DropDownDetailsResponse>> getGenderDataAsync()
        {
            List<GenderMaster> _gendermaster = await _dbContextClass.GenderMaster.Where(m => m.ActiveStatus.Equals(true)).ToListAsync();
            List<DropDownDetailsResponse> _genderList = _gendermaster.Select(x => new DropDownDetailsResponse
            {
                Id = x.GenderId,
                Code = x.GenderCode,
                Value = x.GenderName
            }).ToList();
            return _genderList;
        }

        public async Task<List<DropDownDetailsResponse>> getDisabilityDataAsync()
        {
            List<DisabilityMaster> _disabilitymaster = await _dbContextClass.DisabilityMaster.Where(m => m.ActiveStatus.Equals(true)).ToListAsync();
            List<DropDownDetailsResponse> _disabilityList = _disabilitymaster.Select(x => new DropDownDetailsResponse
            {
                Id = x.DisabilityId,
                Code = x.DisabilityCode,
                Value = x.DisabilityName
            }).ToList();
            return _disabilityList;
        }

        public async Task<List<DropDownDetailsResponse>> getMaratialStatusDataAsync()
        {
            List<MaritalStatusMaster> _maratialstatmaster = await _dbContextClass.MaratialStatusMaster.Where(m => m.ActiveStatus.Equals(true)).ToListAsync();
            List<DropDownDetailsResponse> _maratialstatList = _maratialstatmaster.Select(x => new DropDownDetailsResponse
            {
                Id = x.MStatusId,
                Code = x.MStatusCode,
                Value = x.MStatusName
            }).ToList();
            return _maratialstatList;
        }

        public async Task<List<DropDownDetailsResponse>> getFileTypeDataAsync()
        {
            List<UploadTypeMaster> _masterdata = await _dbContextClass.UploadTypeMaster.Where(m => m.ActiveStatus.Equals(true)).ToListAsync();
            List<DropDownDetailsResponse> _dataList = _masterdata.Select(x => new DropDownDetailsResponse
            {
                Id = x.UploadTypeId,
                Code = x.UploadTypeCode,
                Value = x.UploadTypeName
            }).ToList();
            return _dataList;
        }

        public async Task<List<DropDownDetailsResponse>> getQualificationDataAsync()
        {
            List<QualificationMaster> _masterdata = await _dbContextClass.QualificationMaster.Where(m => m.ActiveStatus.Equals(true)).ToListAsync();
            List<DropDownDetailsResponse> _dataList = _masterdata.Select(x => new DropDownDetailsResponse
            {
                Id = x.QualificationId,
                Code = x.QualificationCode,
                Value = x.QualificationName
            }).ToList();
            return _dataList;
        }

        public async Task<List<DropDownDetailsResponse>> getUserRoleAsync()
        {
            List<RoleMaster> _rolemaster = await _dbContextClass.RoleMaster.Where(m => m.ActiveStatus.Equals(true) && m.IsCompanyAdmin == true).ToListAsync();
            List<DropDownDetailsResponse>? __rolemasterList = _rolemaster?.Select(x => new DropDownDetailsResponse
            {
                Id = x.RoleId,
                Code = x.RolesAlias,
                Value = x.RoleName
            }).ToList();

            return __rolemasterList ?? new List<DropDownDetailsResponse>();
        }

        public async Task<RoleDetailsResponse> getRoleDetailsByRoleAlias(string roleAlias)
        {
            RoleDetailsResponse RoleDetails = new();
            RoleMaster? _roledata = await _dbContextClass.RoleMaster.FirstOrDefaultAsync(x => x.ActiveStatus == true && x.RolesAlias.Equals(roleAlias.Trim()));
            RoleDetails.RoleId = _roledata.RoleId;
            RoleDetails.RoleName = _roledata.RoleName;
            RoleDetails.RoleDescription = _roledata.RoleDesc;
            RoleDetails.RoleAlias = _roledata.RolesAlias;
            return RoleDetails;
        }

        public async Task<List<EscalationLevelCaseDetails>> GetEscalationCaseMasterDetails()
        {
            List<EscalationLevelCaseDetails> levelCaseDataRes = new();

            IQueryable<EscalationLevelCaseDetails> getcasesetupquerydata = from a in _dbContextClass.EmailEscalationSetupMapping
                                                                           join b in _dbContextClass.EmailEscalationCaseMaster
                                                                               on a.CaseId equals b.CaseId
                                                                           join c in _dbContextClass.EscalationLevelMaster
                                                                                on a.LevelId equals c.LevelId
                                                                           where a.ActiveStatus == true && b.ActiveStatus == true && c.ActiveStatus == true
                                                                           select new EscalationLevelCaseDetails
                                                                           {
                                                                               CaseId = a.CaseId,
                                                                               SetupDesc = b.SetupDesc,
                                                                               SetupCode = b.SetupCode,
                                                                               SetupStatus = a.SetupStatus,
                                                                               SetupAlias = b.SetupAlias,
                                                                               LevelId = c.LevelId,
                                                                               LevelCode = c.LevelCode,
                                                                               EmailId = a.EmailId
                                                                           };

            levelCaseDataRes = await getcasesetupquerydata.ToListAsync().ConfigureAwait(false);
            return levelCaseDataRes;
        }

        public async Task<List<CaseSetupDetails>> GetCaseDetails()
        {
            List<CaseSetupDetails>? caseListDetails = new();

            caseListDetails = await _dbContextClass.EmailEscalationCaseMaster?.Where(m => m.ActiveStatus == true)?.Select(x => new CaseSetupDetails
            {
                SetupCaseId = x.CaseId,
                SetupCaseDesc = x.SetupDesc,
                SetupAlias = x.SetupAlias,
            })?.OrderBy(x => x.SetupCaseId)?.ToListAsync();

            return caseListDetails;
        }

        public async Task PostSetupData(GeneralSetupSubmitRequest setupRequest)
        {
            //using var transaction = _dbContextClass.Database.BeginTransaction();
            //try
            //{
            List<EmailEscalationLevel> _EscalatedEmailSetup = setupRequest.EmailEscalationLevel;
            List<EmailEscalationSetup> _EmailEscalationSetup = setupRequest.EmailEscalationSetup;
            List<EscalationLevelEmailMapping> newMailsetup = new();
            if (_EscalatedEmailSetup != null)
            {
                List<EscalationLevelMaster> levelMasterData = await _dbContextClass.EscalationLevelMaster?.Where(x => x.ActiveStatus == true)?.ToListAsync();
                List<EscalationLevelEmailMapping> levelEmailMappingData = await _dbContextClass.EscalationLevelEmailMapping?.Where(x => x.ActiveStatus == true)?.ToListAsync();
                _EscalatedEmailSetup.ForEach(x =>
                {
                    _ = (levelMasterData?.Where(y => y.LevelId.Equals(x.LevelId)).FirstOrDefault(a =>
                        {
                            if (a.NoOfDays != x.NoOfDays)
                            {
                                a.NoOfDays = x.NoOfDays;
                                a.UpdatedBy = setupRequest.UserId;
                                a.UpdatedOn = DateTime.Now;
                            }
                            return true;
                        }));

                    //Emailaddresss  update
                    if (levelEmailMappingData.Count > 0)
                    {
                        levelEmailMappingData.Where(y => y.LevelId.Equals(x.LevelId)).ToList()?.ForEach(y =>
                        {
                            if (!x.Emailaddress.Contains(y.Email))
                            {
                                y.ActiveStatus = false;
                                y.UpdatedBy = setupRequest.UserId;
                                y.UpdatedOn = DateTime.Now;

                                List<string> _currNewMail = x.Emailaddress.Where(m => m != y.Email).ToList();
                                List<EscalationLevelEmailMapping> currMailsetup = _currNewMail.Select(a => new EscalationLevelEmailMapping
                                {
                                    Email = a,
                                    LevelId = x.LevelId,
                                    CreatedBy = setupRequest.UserId,
                                    CreatedOn = DateTime.Now,
                                    ActiveStatus = true,
                                }).ToList();

                                if (currMailsetup.Count > 0)
                                {
                                    newMailsetup.AddRange(currMailsetup);
                                }
                            }
                        });
                    }
                });

                //case setup  update
                List<EmailEscalationSetupMapping> EmailEscalationList = new();
                List<EmailEscalationSetupMapping> levelCaseSetupData = await _dbContextClass.EmailEscalationSetupMapping?.Where(x => x.ActiveStatus == true)?.ToListAsync();
                _EmailEscalationSetup.ForEach(x =>
                {
                    _ = levelCaseSetupData.Where(y => y.LevelId.Equals(x.LevelId) && y.CaseId.Equals(x.CaseId)).FirstOrDefault(a =>
                    {
                        if (a != null && (a.SetupStatus != x.CaseOption || a?.EmailId?.ToUpper()?.Trim() != x?.CaseEmail?.ToUpper()?.Trim()))
                        {
                            a.ActiveStatus = false;
                            a.UpdatedBy = setupRequest.UserId;
                            a.UpdatedOn = DateTime.Now;

                            EmailEscalationSetupMapping currMailsetup = new()
                            {
                                CaseId = x.CaseId,
                                LevelId = x.LevelId,
                                SetupStatus = x.CaseOption,
                                EmailId = x.CaseEmail,
                                CreatedBy = setupRequest.UserId,
                                CreatedOn = DateTime.Now,
                                ActiveStatus = true,
                            };
                            EmailEscalationList.Add(currMailsetup);
                        }
                        return true;
                    });
                });

                GeneralSetup? generalsetupData = await _dbContextClass.GeneralSetup?.Where(x => x.ActiveStatus == true)?.FirstOrDefaultAsync();

                if (generalsetupData.CriticalNoOfDays != null || generalsetupData.GracePeriod != null)
                {
                    if (generalsetupData?.CriticalNoOfDays != setupRequest.CriticalDays)
                    {
                        generalsetupData.CriticalNoOfDays = setupRequest.CriticalDays;
                    }
                    if (generalsetupData?.GracePeriod != setupRequest.GracePeriod)
                    {
                        generalsetupData.GracePeriod = setupRequest.GracePeriod;
                    }
                    if (generalsetupData?.OverLapDays != setupRequest.OverlapDays)
                    {
                        generalsetupData.OverLapDays = setupRequest.OverlapDays;
                    }
                    generalsetupData.UpdatedBy = setupRequest.UserId;
                    generalsetupData.UpdatedOn = DateTime.Now;
                }

                if (newMailsetup.Count > 0)
                {
                    _dbContextClass.EscalationLevelEmailMapping.AddRange(newMailsetup);
                }
                if (EmailEscalationList.Count > 0)
                {
                    _dbContextClass.EmailEscalationSetupMapping.AddRange(EmailEscalationList);
                }
                _ = await _dbContextClass.SaveChangesAsync();
            }
            //    transaction.Commit();
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public async Task<List<ApiConfigResponse>> GetApiConfigDataAll()
        {
            List<ApiConfigResponse> response = new();
            var getApiSetupQueryData = from a in _dbContextClass.ApiTypeMaster
                                       join b in _dbContextClass.ApiTypeMapping
                                           on a.Id equals b.TypeId
                                       where a.ActiveStatus == true && b.ActiveStatus == true
                                       select new ApiConfigResponse
                                       {
                                           apiName = b.ApiName,
                                           apiBaseUrl = b.BaseUrl,
                                           apiUrl = b.Url,
                                           apiProvider = a.Provider,
                                           typeCode = a.TypeCode,
                                           activeStatus = b.ActiveStatus,
                                       };
            var responseData = await getApiSetupQueryData.ToListAsync().ConfigureAwait(false);

            response = responseData;

            return responseData;
        }

        public async Task<string> GetApiProviderData(string? apiType)
        {
            var response = await GetApiProviderDataPriorityBase(apiType, 1);
            return response;
        }

        public async Task<string> GetApiProviderData(string? apiType, int prioriy)
        {
            string response = string.Empty;
            string? _apiType = apiType?.ToLower()?.Trim();

            if (!string.IsNullOrEmpty(_apiType))
            {
                var responseData = await _dbContextClass.ApiTypeMaster?.FirstOrDefaultAsync(x => x.ActiveStatus == true && x.TypeCode.ToLower().Trim() == _apiType);
                response = responseData.Provider;
            }
            return response;
        }

        public async Task<string> GetApiProviderDataPriorityBase(string? apiType, int? apiPriority) //mGhosh new method
        {
            string response = string.Empty;
            string? _apiType = apiType?.ToLower()?.Trim();

            if (!string.IsNullOrEmpty(_apiType) && apiPriority.HasValue)
            {
                var responseData = await _dbContextClass.ApiTypeMaster?.FirstOrDefaultAsync(x => x.ActiveStatus == true && x.TypeCode.ToLower().Trim() == _apiType
                && x.apiPriotity == (apiPriority ?? 1)
                );
                //response = responseData.Provider;

                if (responseData != null)
                {
                    response = responseData.Provider;
                }
            }
            return response;
        }

        public async Task<List<FaqDetailsResponse>> GetAllFaqMaster()
        {
            var responseData = await _dbContextClass.FaqMaster?.Where(x => x.ActiveStatus == true)?.
                Select(y => new FaqDetailsResponse
                {
                    FaqId = y.FaqId,
                    FaqName = y.FaqName,
                    FaqDescription = y.FaqDesc,
                    Contenttype = y.textType.Trim()
                })?.ToListAsync();
            //response = responseData.Provider;
            return responseData;
        }

        public async Task<List<CompanyEntityDetailsResponse>> GetAllCompanyEntityMaster()
        {
            var responseData = await _dbContextClass.CompanyDetails?.Where(x => x.ActiveStatus == true)?.
                Select(y => new CompanyEntityDetailsResponse
                {
                    CompanyId = y.Id,
                    CompanyName = y.CompanyName,
                    CompanyCode = y.CompanyAlias,
                    CompanyCity = y.City,
                })?.ToListAsync();
            //response = responseData.Provider;
            return responseData;
        }
    }
}