using VERIDATA.Model.Request;
using VERIDATA.Model.Response;

namespace VERIDATA.BLL.Interfaces
{
    public interface ISetupConfigarationContext
    {
        public Task PostSetupData(GeneralSetupSubmitRequest setupRequest);

        public Task<GeneralSetupDetailsResponse> gettSetupData();

        public Task<List<FaqDetailsResponse>> GetFaqData();
    }
}