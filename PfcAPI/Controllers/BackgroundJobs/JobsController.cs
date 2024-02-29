using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VERIDATA.BLL.Services;

namespace PfcAPI.Controllers.BackgroundJobs
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        //private readonly IEmailSender _emailSender;
        private readonly IWorkerService _workerService;

        public JobsController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        ////[AllowAnonymous]
        ////[HttpPost("schedule-job")]
        ////public ActionResult ScheduleJob()
        ////{

        ////    // Schedule the background job to run immediately
        ////    //RecurringJob.AddOrUpdate(() => _workerService.DoSomething(), Cron.Minutely);

        ////    return Ok("Job scheduled successfully!");
        ////}
        [AllowAnonymous]
        [HttpPost("schedule-job-api-counter")]
        public ActionResult ScheduleJobCounter()
        {
            try
            {
                // Schedule the background job to run immediately
                RecurringJob.AddOrUpdate("api-counter", () => _workerService.ApiCountMailAsync(), Cron.Daily);
                return Ok("Job scheduled successfully!");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("schedule-job-total-appointee-counter")]
        public ActionResult ScheduleAppointeeCounter()
        {
            try
            {
                // Schedule the background job to run immediately
                RecurringJob.AddOrUpdate("appointee-counter", () => _workerService.ApponteeCountMailAsync(), Cron.Daily);
                return Ok("Job scheduled successfully!");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("schedule-job-critical-appointee")]
        public ActionResult ScheduleCriticalAppointee()
        {
            RecurringJob.AddOrUpdate("critical-appointee", () => _workerService.CriticalAppointeeMail(), Cron.Daily);
            return Ok("Job scheduled successfully!");
        }

        [AllowAnonymous]
        [HttpPost("schedule-job-case-escalation")]
        public ActionResult ScheduleUnderProcessAppointee()
        {
            RecurringJob.AddOrUpdate("case-escalation-appointee", () => _workerService.CaseBasedEscalation(), Cron.Weekly);
            return Ok("Job scheduled successfully!");
        }


    }
}
