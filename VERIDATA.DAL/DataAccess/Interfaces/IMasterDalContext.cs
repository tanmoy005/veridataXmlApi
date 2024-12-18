using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Admin;
using VERIDATA.Model.Table.Master;

namespace VERIDATA.DAL.DataAccess.Interfaces
{
    public interface IMasterDalContext
    {
        public Task<GeneralSetup> GetGeneralSetupData();

        public Task<List<MenuMaster>> GetMasterMenuData();

        public Task<List<WorkflowApprovalStatusMaster>> GetAllApprovalStateMaster();

        public Task<List<EscalationLevelMasterDataResponse>> GetEscalationLevelMasterData();

        public Task<List<DropDownDetailsResponse>> getCountryDataAsync();

        public Task<List<DropDownDetailsResponse>> getNationilityDataAsync();

        public Task<List<DropDownDetailsResponse>> getGenderDataAsync();

        public Task<List<DropDownDetailsResponse>> getDisabilityDataAsync();

        public Task<List<DropDownDetailsResponse>> getMaratialStatusDataAsync();

        public Task<List<DropDownDetailsResponse>> getFileTypeDataAsync();

        public Task<List<DropDownDetailsResponse>> getQualificationDataAsync();

        public Task<List<DropDownDetailsResponse>> getUserRoleAsync();

        public Task<RoleDetailsResponse> getRoleDetailsByRoleAlias(string roleAlias);

        public Task<List<EscalationLevelCaseDetails>> GetEscalationCaseMasterDetails();

        public Task<List<CaseSetupDetails>> GetCaseDetails();

        public Task PostSetupData(GeneralSetupSubmitRequest setupRequest);

        //public Task<ApiConfigResponse> GetApiConfigData(string apiType, string apiName, string provider);
        public Task<List<ApiConfigResponse>> GetApiConfigDataAll();

        public Task<string> GetApiProviderData(string? apiType);

        public Task<List<FaqDetailsResponse>> GetAllFaqMaster();

        public Task<List<CompanyEntityDetailsResponse>> GetAllCompanyEntityMaster();

        public Task<string> GetApiProviderDataPriorityBase(string? apiType, int? apiPriority); // mGhosh new method
    }
}