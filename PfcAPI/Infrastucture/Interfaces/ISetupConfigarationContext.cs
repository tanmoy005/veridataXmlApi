using PfcAPI.Model.RequestModel;
using PfcAPI.Model.ResponseModel;

namespace PfcAPI.Infrastucture.Interfaces
{
    public interface ISetupConfigarationContext
    {
        public Task PostSetupData(GeneralSetupSubmitRequest setupRequest);

        public Task<GeneralSetupDetailsResponse> gettSetupData();


    }
}
