using PfcAPI.Model.RequestModel;
using PfcAPI.Model.ResponseModel;

namespace PfcAPI.Infrastucture.Interfaces
{
    public interface IActivityContext
    {
        /// <summary>
        /// save excel file details in to db
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filepath"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public Task<List<AppointeeActivityDetailsResponse>> GetActivityDetails(int appointeeId);

        public Task PostActivityDetails(int appointeeId, int userId, string? activityCode);
        public Task PostApiActivity(ApiCountLogRequest req);
    }
}
