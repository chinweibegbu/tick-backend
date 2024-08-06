using TemplateTest.Infrastructure.Extension;
using Serilog;

namespace TemplateTest
{
    public class Startup
    {
        private readonly IConfigurationRoot configRoot;

        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            Configuration = configuration;

            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            configRoot = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransientServices();

            services.AddScopedServices();

            services.AddSingletonServices();

            services.AddDatabaseContext(Configuration, configRoot);

            services.AddRepositoryServices();

            services.AddJwtIdentityService(Configuration);

            services.AddBasicIdentityService();

            services.AddCustomAutoMapper();

            services.AddInMemoryCache();

            services.AddSwaggerOpenAPI();

            services.AddCustomOptions();

            services.AddCustomControllers();

            services.AddRequestRateLimiter(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log)
        {
            if (env.IsDevelopment())
            {
                app.ConfigureDbContext();

                app.ConfigureSwagger();
            }

            app.ConfigureCors(Configuration);

            app.ConfigureCustomExceptionMiddleware();

            log.AddSerilog();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
