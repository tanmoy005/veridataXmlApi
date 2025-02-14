using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Response.api.Signzy;

namespace VERIDATA.BLL.Services
{
    public interface IApiConfigService
    {
        public List<ApiConfigResponse> GetApiConfigDetails();

        public Task<ApiConfigResponse> GetApiConfigData(string apiType, string apiName, string provider);

        public Task<string> StoreCallBacKPassbookData(SignzyUanPassbookDetails data);

        public Task<SignzyUanPassbookDetails> CheckForCallBacKPassbookData(string keyData);
    }
}