using Hangfire.Dashboard;

namespace TemplateTest.Infrastructure.Filters
{
    public class HangfireAuthFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}
