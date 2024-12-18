using VERIDATA.Model.DataAccess.Response;

namespace VERIDATA.BLL.Services
{
    public interface IApiConfigService
    {
        public List<ApiConfigResponse> GetApiConfigDetails();

        public Task<ApiConfigResponse> GetApiConfigData(string apiType, string apiName, string provider);
    }
}