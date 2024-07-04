using Microsoft.EntityFrameworkCore;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.DAL.DBContext;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Activity;
using VERIDATA.Model.Table.Config;
using VERIDATA.Model.Table.Public;

namespace VERIDATA.DAL.DataAccess.Context
{

    public class ActivityDalContext : IActivityDalContext
    {
        private readonly DbContextDalDB _dbContextClass;

        public ActivityDalContext(DbContextDalDB dbContextClass)
        {
            _dbContextClass = dbContextClass;
        }

        public async Task<List<AppointeeActivityDetailsResponse>> GetActivityDetails(int appointeeId)
        {
            var activityQuery = from ac in _dbContextClass.ActivityMaster
                                join at in _dbContextClass.ActivityTransaction
                                    on ac.ActivityId equals at.ActivityId
                                where ac.ActiveStatus == true && at.ActiveStatus == true
                                && at.AppointeeId == appointeeId
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

            List<AppointeeActivityDetailsResponse>? _activityViewdata = activityList?.Select(row => new AppointeeActivityDetailsResponse
            {
                id = row.ActivityTransId,
                ActivityType = row.ActivityType,
                ActivityName = row.ActivityName,
                ActivityInfo = row.ActivityInfo,
                Color = row.ActivityColor,
                CreatedOn = row.CreatedOn

            }).ToList();

            return _activityViewdata ?? new List<AppointeeActivityDetailsResponse>();
        }

        public async Task PostActivityDetails(int appointeeId, int userId, string? activityCode)
        {
            ActivityMaster? getCurrentActivity = await _dbContextClass.ActivityMaster.Where(x => x.ActivityCode == activityCode && x.ActiveStatus == true).FirstOrDefaultAsync();
            if (getCurrentActivity != null)
            {
                ActivityTransaction activtydata = new()
                {
                    AppointeeId = appointeeId,
                    ActivityId = getCurrentActivity.ActivityId,
                    ActiveStatus = true,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now
                };

                _ = await _dbContextClass.ActivityTransaction.AddAsync(activtydata);
                _ = await _dbContextClass.SaveChangesAsync();

            }
        }
        public async Task PostApiActivity(ApiCountLogRequest req)
        {
            List<string>? ApiNameData = !string.IsNullOrEmpty(req.Url) ? req.Url?.Split("/")?.ToList() : null;
            List<string>? getApiTypeName = ApiNameData?.Count >= 2 ? ApiNameData?.TakeLast(2)?.ToList() : null;
            string? getApiName = getApiTypeName != null ? string.Join(" - ", getApiTypeName?.ToList()) : ApiNameData?.LastOrDefault();
            if (!string.IsNullOrEmpty(getApiName))
            {
                ApiCounter apiActivty = new()
                {
                    ApiName = getApiName,
                    ProviderName=req?.Provider,
                    Url = req.Url,
                    Type = req.Type,
                    Status = req.Status,
                    Payload = req.Payload,
                    CreatedBy = req.UserId,
                    CreatedOn = DateTime.Now
                };

                _ = await _dbContextClass.ApiCounter.AddAsync(apiActivty);
                _ = await _dbContextClass.SaveChangesAsync();

            }
        }

        public async Task<List<ApiCounter>> GetTotalApiCountByDate(DateTime startDate)
        {
            List<ApiCounter> totalApiList = await _dbContextClass.ApiCounter.Where(m => m.CreatedOn > startDate).ToListAsync();
            return totalApiList ?? new List<ApiCounter>();
        }

        public async Task<List<UploadAppointeeCounter>> GetTotalAppointeeCountByDate(DateTime startDate)
        {
            List<UploadAppointeeCounter> totalAppointeeList = await _dbContextClass.UploadAppointeeCounter.Where(x => x.CreatedOn >= startDate).ToListAsync();
            return totalAppointeeList ?? new List<UploadAppointeeCounter>();
        }
    }
}
