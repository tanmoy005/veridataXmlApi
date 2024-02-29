using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.BLL.Authentication
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string[] _roles;

        public HangfireAuthorizationFilter(params string[] roles)
        {
            _roles = roles;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = ((AspNetCoreDashboardContext)context).HttpContext;

            //Your authorization logic goes here.

            return true; //I'am returning true for simplicity
        }
    }
}
