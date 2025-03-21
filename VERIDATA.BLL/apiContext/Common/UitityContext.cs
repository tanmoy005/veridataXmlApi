﻿using System.Net;
using System.Net.Http.Headers;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.Model.Configuration;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using static VERIDATA.DAL.utility.CommonEnum;

namespace VERIDATA.BLL.apiContext.Common
{
    public class UitityContext : IUitityContext
    {
        private readonly ApiConfiguration _apiConfig;
        private readonly IActivityDalContext _activityContext;
        private readonly IMasterDalContext _masterContext;

        public UitityContext(ApiConfiguration apiConfig, IActivityDalContext activityContext, IMasterDalContext masterContext)
        {
            _apiConfig = apiConfig;
            _activityContext = activityContext;
            _masterContext = masterContext;
        }

        public async Task<HttpResponseMessage> HttpPostApi(ApiConfigResponse apiConfig, StringContent content, int userId)
        {
            HttpResponseMessage responsse = new();
            if (apiConfig == null)
            {
                return responsse;
            }
            HttpClient client = new();
            //var apiConfig = await _masterContext.GetApiConfigData(apiType);
            //string _url = apiConfig?.apiUrl;
            string _url = $"{apiConfig.apiBaseUrl}{apiConfig.apiUrl}";
            HttpRequestMessage request = new(HttpMethod.Post, _url);

            ApiCountLogRequest ApiCountLogReq = new ApiCountLogRequest()
            {
                Provider = apiConfig.apiProvider,
                ApiName = apiConfig.apiName,
                Url = apiConfig.apiUrl,
                Type = "Request",
                UserId = userId,
                Payload = string.Empty,
            };
            Task.Run(async () => await _activityContext.PostApiActivity(ApiCountLogReq)).GetAwaiter().GetResult();

            if (apiConfig.apiProvider?.ToLower() == ApiProviderType.Karza)
            {
                request.Headers.Add(_apiConfig.ApiKey, _apiConfig.ApiKeyValue);
            }

            if (apiConfig.apiProvider?.ToLower() == ApiProviderType.SurePass)
            {
                ////Token Auth
                AuthenticationHeaderValue Authorization = new("Bearer", _apiConfig.ApiToken);
                request.Headers.Authorization = Authorization;
                ////Token Auth
            }
            if (apiConfig.apiProvider?.ToLower() == ApiProviderType.Signzy)
            {
                ////Token Auth
                AuthenticationHeaderValue Authorization = new(_apiConfig.AuthTokenSignzy);
                request.Headers.Authorization = Authorization;
                ////Token Auth
            }
            request.Content = content;
            responsse = await client.SendAsync(request);

            if (responsse.StatusCode != HttpStatusCode.OK)
            {
                ApiCountLogReq.Provider = apiConfig.apiProvider;
                ApiCountLogReq.ApiName = apiConfig.apiName;
                ApiCountLogReq.Type = "Response";
                ApiCountLogReq.Status = (Int32)responsse.StatusCode;
                ApiCountLogReq.Payload = _apiConfig.ApiDataLog ?? false ? await responsse.Content.ReadAsStringAsync() : string.Empty;
                Task.Run(async () => await _activityContext.PostApiActivity(ApiCountLogReq)).GetAwaiter().GetResult();
            }
            else
            {
                ApiCountLogReq.Provider = apiConfig.apiProvider;
                ApiCountLogReq.ApiName = apiConfig.apiName;
                ApiCountLogReq.Type = "Response";
                ApiCountLogReq.Status = (Int32)responsse.StatusCode;
                ApiCountLogReq.Payload = string.Empty;
                Task.Run(async () => await _activityContext.PostApiActivity(ApiCountLogReq)).GetAwaiter().GetResult();
            }
            return responsse;
        }
    }
}