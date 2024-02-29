using VERIDATA.Model.DataAccess.Response;

namespace VERIDATA.BLL.apiContext.Common
{
    public interface IUitityContext
    {
        public Task<HttpResponseMessage> HttpPostApi(ApiConfigResponse apiConfig, StringContent content, int userId);
    }
}
