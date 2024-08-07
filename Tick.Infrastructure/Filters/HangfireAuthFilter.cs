using Hangfire.Dashboard;

namespace Tick.Infrastructure.Filters
{
    public class HangfireAuthFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}
