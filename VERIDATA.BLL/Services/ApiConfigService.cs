using Microsoft.Extensions.Caching.Memory;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Response.api.Signzy;

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

        public async Task<string> StoreCallBacKPassbookData(SignzyUanPassbookDetails data)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(10)) // Expire after 10 minutes if not accessed
            .SetAbsoluteExpiration(TimeSpan.FromHours(1)); // Expire after 1 hour regardless of access

            _memoryCache.Set(data.EmployeeDetails.MemberName, data, cacheEntryOptions);

            return data.EmployeeDetails.MemberName;
        }

        public async Task<SignzyUanPassbookDetails> CheckForCallBacKPassbookData(string keyData)
        {
            var maxRetries = 10;
            var retryDelay = 2000; // 2 seconds delay between checks

            for (int i = 0; i < maxRetries; i++)
            {
                // Step 1: Check if callback data is available in cache
                var response = GetCallbackPassbookResponse(keyData);
                if (response != null)
                {
                    // Step 2: Return response when found
                    return response;
                }

                // Step 3: If not found, wait before retrying
                Thread.Sleep(retryDelay); // wait before retrying
            }

            return null;
        }

        private SignzyUanPassbookDetails? GetCallbackPassbookResponse(string keyData)
        {
            // Try to get the callback data from the cache
            _memoryCache.TryGetValue(keyData, out SignzyUanPassbookDetails? response);
            return response; // return null if not found
        }
    }
}