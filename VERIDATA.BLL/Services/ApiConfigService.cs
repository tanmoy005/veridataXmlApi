using Microsoft.Extensions.Caching.Memory;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.Model.DataAccess.Response;

namespace VERIDATA.BLL.Services
{
    public class ApiConfigService : IApiConfigService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IMasterDalContext _masterContext;
        public string cacheKey = "apiConfig";

        public ApiConfigService(IMemoryCache memoryCache, IMasterDalContext masterContext)
        {
            _memoryCache = memoryCache;
            _masterContext = masterContext;
        }

        public List<ApiConfigResponse> GetApiConfigDetails()
        {
            List<ApiConfigResponse> apiConfigResponse;

            if (!_memoryCache.TryGetValue(cacheKey, out apiConfigResponse))
            {
                apiConfigResponse = GetValuesFromDbAsync().Result;

                _memoryCache.Set(cacheKey, apiConfigResponse,
                    new MemoryCacheEntryOptions());
                //.SetAbsoluteExpiration(TimeSpan.FromSeconds(5)));
            }
            return apiConfigResponse;
        }

        private async Task<List<ApiConfigResponse>> GetValuesFromDbAsync()
        {
            //var apiProvider = _masterContext.GetApiProviderData(ApiType.Pan);
            var apiProvider = await _masterContext.GetApiConfigDataAll();

            List<ApiConfigResponse> apiConfig = new List<ApiConfigResponse>();
            apiConfig.AddRange(apiProvider);

            Task<List<ApiConfigResponse>> ApiConfigList = Task<List<ApiConfigResponse>>.Factory.StartNew(() =>
            {
                return apiConfig;
            });

            return apiConfig;

        }
        public async Task<ApiConfigResponse> GetApiConfigData(string apiType, string apiName, string provider)
        {
            ApiConfigResponse response = new();
            string? _apiType = apiType?.ToUpper()?.Trim();

            if (!string.IsNullOrEmpty(_apiType) && !string.IsNullOrEmpty(apiName))
            {
                var _data = GetApiConfigDetails();

                var responseData = _data.Where(x => x.apiProvider.ToLower() == provider.Trim().ToLower()
                                           && x.typeCode.Trim().ToLower() == apiType.Trim().ToLower() && x.apiName.Trim().ToLower() == apiName.Trim().ToLower()).ToList();



                response = responseData?.FirstOrDefault();
            }
            return response;
        }
    }
}
