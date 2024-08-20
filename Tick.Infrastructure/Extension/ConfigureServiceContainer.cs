using Tick.Core.Cache;
using Tick.Core.Contract;
using Tick.Core.Contract.Repository;
using Tick.Core.Implementation;
using Tick.Core.Repository;
using Tick.Core.Storage;
using Tick.Domain.Entities;
using Tick.Domain.Settings;
using Tick.Infrastructure.Configs;
using Tick.Infrastructure.Handlers;
using Tick.Persistence;
using AspNetCoreRateLimit;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using IClientFactory = Tick.Core.Contract.IClientFactory;

namespace Tick.Infrastructure.Extension
{
    public static class ConfigureServiceContainer
    {
        public static void AddDatabaseContext(this IServiceCollection serviceCollection,
             IConfiguration configuration, IConfigurationRoot configRoot)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration
                    .GetConnectionString("DBConnectionString") ?? configRoot["ConnectionStrings:DBConnectionString"]
                , b =>
                {
                    b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                });
                //options.AddInterceptors(serviceCollection.BuildServiceProvider().GetRequiredService<DBLongQueryLogger>());
            });
        }
        public static void AddTransientServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAPIImplementation, APIImplementation>();
            serviceCollection.AddTransient<IClientFactory, ClientFactory>();
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<ITaskService, TaskService>();
            serviceCollection.AddTransient<IAppSessionService, AppSessionService>();
            serviceCollection.AddTransient<INotificationService, NotificationService>();
        }

        public static void AddSingletonServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IStorageService, AzureBlobStorageService>();
        }

        public static void AddScopedServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            serviceCollection.AddScoped<IUserService, UserService>();
        }

        public static void AddRepositoryServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<IBasicUserRepository, BasicUserRepository>();
            serviceCollection.AddSingleton<ITaskRepository, TaskRepository>();
        }

        public static void AddJwtIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<Ticker, IdentityRole>(opt =>
            {
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5000);
                opt.Lockout.MaxFailedAccessAttempts = 5;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            var code = HttpStatusCode.Unauthorized;
                            context.Response.StatusCode = (int)code;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Domain.Common.Response<string>("You are not Authorized", (int)code));
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            var code = HttpStatusCode.Forbidden;
                            context.Response.StatusCode = (int)code;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Domain.Common.Response<string>("You are not authorized to access this resource", (int)code));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
        }

        public static void AddBasicIdentityService(this IServiceCollection services)
        {
            services
                .AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("BasicAuthentication", new AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser().Build());
            });
        }

        public static void AddCustomOptions(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions<ExternalApiOptions>().BindConfiguration("ExternalApiOptions");
            serviceCollection.AddOptions<MockOptions>().BindConfiguration("MockOptions");
            serviceCollection.AddOptions<JWTSettings>().BindConfiguration("JWTSettings");
            serviceCollection.AddOptions<SendGridSettings>().BindConfiguration("SendGridSettings");
            serviceCollection.AddOptions<AdminOptions>().BindConfiguration("AdminOptions");
            serviceCollection.AddOptions<AzureBlobOptions>().BindConfiguration("AzureBlobOptions");
        }

        public static void AddCustomAutoMapper(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(MappingProfileConfiguration));
        }

        public static void AddValidation(this IServiceCollection serviceCollection)
        {
            serviceCollection.Configure<ApiBehaviorOptions>(opt => { opt.SuppressModelStateInvalidFilter = false; });
        }

        public static void AddSwaggerOpenAPI(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(setupAction =>
            {

                setupAction.SwaggerDoc(
                    "OpenAPISpecification",
                    new OpenApiInfo()
                    {
                        Title = "Tick WebAPI",
                        Version = "1",
                        Description = "API Details for Tick",
                        Contact = new OpenApiContact()
                        {
                            Email = "okafori@theTicknetwork.com",
                            Name = "The Tick Network",
                            Url = new Uri(" https://theTicknetwork.com")
                        },
                        License = new OpenApiLicense()
                        {
                            Name = "UNLICENSED"
                        }
                    });

                setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = $"Input your Bearer token in this format - Bearer token to access this API",
                });
                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        }, new List<string>()
                    },
                });
            });
        }

        public static void AddCustomHangfire(this IServiceCollection serviceCollection, IConfiguration config, IConfigurationRoot configRoot)
        {
            var hangfireDbContext = serviceCollection.BuildServiceProvider().GetService<IApplicationDbContext>();

            serviceCollection.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(config.GetConnectionString("DBConnectionString") ?? configRoot["ConnectionStrings:DBConnectionString"], new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.FromSeconds(5),
                    SchemaName = "PORTAL_HANGFIRE",
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true,
                }));

            serviceCollection.AddHangfireServer(options =>
            {
                options.ServerName = string.Format("{0}.{1}", Environment.MachineName, Guid.NewGuid().ToString());
                options.WorkerCount = 1;
                options.Queues = new[] {
                      "default",
                };
            });
        }

        public static void AddCustomControllers(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddEndpointsApiExplorer();

            serviceCollection.AddControllersWithViews()
                .AddNewtonsoftJson(ops =>
                {
                    ops.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                    ops.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                });
            serviceCollection.AddRazorPages();

            serviceCollection.Configure<ApiBehaviorOptions>(apiBehaviorOptions =>
                apiBehaviorOptions.InvalidModelStateResponseFactory = actionContext =>
                {
                    var logger = actionContext.HttpContext.RequestServices.GetRequiredService<ILogger<BadRequestObjectResult>>();
                    IEnumerable<string> errorList = actionContext.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                    logger.LogError("Bad Request");
                    logger.LogError(string.Join(",", errorList));
                    return new BadRequestObjectResult(new Domain.Common.Response<IEnumerable<string>>("Tick Validation Error", 400, errorList));
                });
        }

        public static void AddHTTPPolicies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var policyConfigs = new HttpClientPolicyOptions();
            configuration.Bind("HttpClientPolicies", policyConfigs);

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(policyConfigs.RetryTimeoutInSeconds));

            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(r => r.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(policyConfigs.RetryCount, _ => TimeSpan.FromMilliseconds(policyConfigs.RetryDelayInMs));

            var circuitBreakerPolicy = HttpPolicyExtensions
               .HandleTransientHttpError()
               .CircuitBreakerAsync(policyConfigs.MaxAttemptBeforeBreak, TimeSpan.FromSeconds(policyConfigs.BreakDurationInSeconds));

            var noOpPolicy = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();

            HttpClientHandler handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            //Register a Typed Instance of HttpClientFactory for a Protected Resource
            //More info see: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.0

            serviceCollection.AddHttpClient<IClientFactory, ClientFactory>()
                .ConfigurePrimaryHttpMessageHandler(_ =>
                {
                    var handler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                    };
                    return handler;
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(policyConfigs.HandlerTimeoutInMinutes))
                .AddPolicyHandler(request => request.Method == HttpMethod.Get ? retryPolicy : noOpPolicy)
                .AddPolicyHandler(timeoutPolicy)
                .AddPolicyHandler(circuitBreakerPolicy);
        }

        public static IServiceCollection AddInMemoryCache(this IServiceCollection services)
        {
            services.AddMemoryCache();

            if (services.Contains(ServiceDescriptor.Transient<ICacheService, InMemoryCacheService>()))
            {
                return services;
            }

            services.AddTransient<ICacheService, InMemoryCacheService>();
            return services;
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration config,
            Action<RedisCacheOptions> setupAction = null)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (services.Contains(ServiceDescriptor.Transient<ICacheService, DistributedCacheService>()))
            {
                return services;
            }

            var redisOptions = new RedisCacheOptions();
            var redisSection = config.GetSection("Redis");

            redisSection.Bind(redisOptions);
            services.Configure<RedisCacheOptions>(redisSection);
            setupAction?.Invoke(redisOptions);

            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = config[redisOptions.Prefix];
                options.ConfigurationOptions = GetRedisConfigurationOptions(redisOptions);
            });

            services.AddTransient<ICacheService, DistributedCacheService>();

            return services;
        }

        private static ConfigurationOptions GetRedisConfigurationOptions(RedisCacheOptions redisOptions)
        {
            var configurationOptions = new ConfigurationOptions
            {
                ConnectTimeout = redisOptions.ConnectTimeout,
                SyncTimeout = redisOptions.SyncTimeout,
                ConnectRetry = redisOptions.ConnectRetry,
                AbortOnConnectFail = redisOptions.AbortOnConnectFail,
                ReconnectRetryPolicy = new ExponentialRetry(redisOptions.DeltaBackoffMiliseconds),
                KeepAlive = 5,
                Ssl = redisOptions.Ssl
            };

            if (!string.IsNullOrWhiteSpace(redisOptions.Password))
            {
                configurationOptions.Password = redisOptions.Password;
            }

            var endpoints = redisOptions.Url.Split(',');
            foreach (var endpoint in endpoints)
            {
                configurationOptions.EndPoints.Add(endpoint);
            }

            return configurationOptions;
        }

        public static void AddRequestRateLimiter(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            // needed to load configuration from appsettings.json
            serviceCollection.AddOptions();
            // needed to store rate limit counters and ip rules
            serviceCollection.AddMemoryCache();

            //load general configuration from appsettings.json
            serviceCollection.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

            // inject counter and rules stores
            serviceCollection.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            serviceCollection.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            // configuration (resolvers, counter key builders)
            serviceCollection.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }
    }
}
