using Microsoft.EntityFrameworkCore;
using PfcAPI.Infrastucture.DBContext;
using PfcAPI.Infrastucture.Interfaces;
using PfcAPI.Model.ActivityLog;
using PfcAPI.Model.Maintainance;
using PfcAPI.Model.RequestModel;
using PfcAPI.Model.ResponseModel;

namespace PfcAPI.Infrastucture.Context
{

    public class ActivityContext : IActivityContext
    {
        private readonly DbContextDB _dbContextClass;

        public ActivityContext(DbContextDB dbContextClass)
        {
            _dbContextClass = dbContextClass;
        }

        public async Task<List<AppointeeActivityDetailsResponse>> GetActivityDetails(int appointeeId)
        {
            var activityQuery = from ac in _dbContextClass.ActivityMaster
                                join at in _dbContextClass.ActivityTransaction
                                    on ac.ActivityId equals at.ActivityId
                                where ac.ActiveStatus == true & at.ActiveStatus == true
                                & at.AppointeeId == appointeeId
                                orderby at.CreatedOn
                                select new
                                {
                                    at.ActivityTransId,
                                    ac.ActivityType,
                                    ac.ActivityName,
                                    ac.ActivityInfo,
                                    ac.ActivityColor,
                                    at.CreatedOn,
                                };



            var activityList = await activityQuery.ToListAsync().ConfigureAwait(false);

            var _activityViewdata = activityList?.Select(row => new AppointeeActivityDetailsResponse
            {
                id = row.ActivityTransId,
                ActivityType = row.ActivityType,
                ActivityName = row.ActivityName,
                ActivityInfo = row.ActivityInfo,
                Color = row.ActivityColor,
                CreatedOn = row.CreatedOn

            })?.ToList();
            return _activityViewdata;
        }

        public async Task PostActivityDetails(int appointeeId, int userId, string? activityCode)
        {
            var getCurrentActivity = await _dbContextClass.ActivityMaster.Where(x => x.ActivityCode == activityCode && x.ActiveStatus == true).FirstOrDefaultAsync();
            if (getCurrentActivity != null)
            {
                var activtydata = new ActivityTransaction();
                activtydata.AppointeeId = appointeeId;
                activtydata.ActivityId = getCurrentActivity.ActivityId;
                activtydata.ActiveStatus = true;
                activtydata.CreatedBy = userId;
                activtydata.CreatedOn = DateTime.Now;

                await _dbContextClass.ActivityTransaction.AddAsync(activtydata);
                await _dbContextClass.SaveChangesAsync();

            }
        }
        public async Task PostApiActivity(ApiCountLogRequest req)
        {
            var ApiNameData = !string.IsNullOrEmpty(req.Url) ? req.Url?.Split("/")?.ToList() : null;
            var getApiTypeName = ApiNameData?.Count >= 2 ? ApiNameData?.TakeLast(2)?.ToList() : null;
            var getApiName = getApiTypeName != null ? string.Join(" - ", getApiTypeName?.ToList()) : ApiNameData?.LastOrDefault();
            if (!string.IsNullOrEmpty(getApiName))
            {
                var apiActivty = new ApiCounter();
                apiActivty.ApiName = getApiName;
                apiActivty.Url = req.Url;
                apiActivty.Type = req.Type;
                apiActivty.Status = req.Status;
                apiActivty.Payload = req.Payload;
                apiActivty.CreatedBy = req.UserId;
                apiActivty.CreatedOn = DateTime.Now;

                await _dbContextClass.ApiCounter.AddAsync(apiActivty);
                await _dbContextClass.SaveChangesAsync();

            }
        }
    }
}
