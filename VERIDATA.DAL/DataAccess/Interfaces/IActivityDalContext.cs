using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Config;
using VERIDATA.Model.Table.Public;

namespace VERIDATA.DAL.DataAccess.Interfaces
{
    public interface IActivityDalContext
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

        public Task<List<ApiCounter>> GetTotalApiCountByDate(DateTime startDate);

        public Task<List<UploadAppointeeCounter>> GetTotalAppointeeCountByDate(DateTime startDate);
    }
}