using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.BLL.Authentication
{
    public interface IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context);
    }
}
