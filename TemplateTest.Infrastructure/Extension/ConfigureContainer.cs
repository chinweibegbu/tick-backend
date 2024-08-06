using TemplateTest.Domain.Settings;
using TemplateTest.Infrastructure.Filters;
using TemplateTest.Infrastructure.Middleware;
using TemplateTest.Persistence;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TemplateTest.Infrastructure.Extension
{
    public static class ConfigureContainer
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }

        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/OpenAPISpecification/swagger.json", "TemplateTest Admin API");
                setupAction.RoutePrefix = "OpenAPI";
            });
        }

        public static void ConfigureCors(this IApplicationBuilder app, IConfiguration configuration)
        {
            AdminOptions adminOptions = new AdminOptions();
            configuration.Bind("AdminOptions", adminOptions);

            app.UseCors(options =>
                 options.WithOrigins(adminOptions.AllowedHosts)
                 .AllowAnyHeader()
                 .WithExposedHeaders("Content-Disposition")
                 .AllowAnyMethod());
        }

        public async static void ConfigureDbContext(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
            }
        }

        public static void ConfigureHangfire(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                DashboardTitle = "TemplateTest Admin - Hangfire",
                Authorization = new[] { new HangfireAuthFilter() },
                IgnoreAntiforgeryToken = true
            });
        }
    }
}
