using Tick.Domain.Entities;
using Tick.Domain.Entities.Base;
using Tick.Persistence.Seeds;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tick.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<Ticker>, IApplicationDbContext
    {
#nullable enable
        private IDbContextTransaction? _currentTransaction;
#nullable disable
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<BasicUser> BasicUser { get; set; }
        public DbSet<Ticker> Ticker { get; set; }
        public DbSet<Tick.Domain.Entities.Task> Task { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var configRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            options.UseSqlServer(
                configRoot["ConnectionStrings:DBConnectionString"],
                x => x.MigrationsHistoryTable("MIGRATION_HISTORY", "API_TEMPLATE"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("API_TEMPLATE");

            modelBuilder.Entity<BasicUser>(entity =>
            {
                entity.ToTable(name: "BASIC_USER");

                entity.HasIndex(t => t.ApiKey)
                    .IsUnique();
            });

            modelBuilder.Entity<Ticker>(entity =>
            {
                entity.ToTable(name: "TICKER");
                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");
                entity.Property(x => x.AccessFailedCount)
                    .HasColumnName("ACCESS_FAILED_COUNT");
                entity.Property(x => x.ConcurrencyStamp)
                    .HasColumnName("CONCURRENCY_STAMP");
                entity.Property(x => x.CreatedAt)
                    .HasColumnName("CREATED_AT");
                entity.Property(x => x.Email)
                    .HasColumnName("EMAIL");
                entity.Property(x => x.EmailConfirmed)
                    .HasColumnName("EMAIL_CONFIRMED");
                entity.Property(x => x.FirstName)
                    .HasColumnName("FIRST_NAME");
                entity.Property(x => x.IsActive)
                    .HasColumnName("IS_ACTIVE");
                entity.Property(x => x.IsLoggedIn)
                    .HasColumnName("IS_LOGGED_IN");
                entity.Property(x => x.LastLoginTime)
                    .HasColumnName("LAST_LOGIN_TIME");
                entity.Property(x => x.LastName)
                    .HasColumnName("LAST_NAME");
                entity.Property(x => x.LockoutEnabled)
                    .HasColumnName("LOCKOUT_ENABLED");
                entity.Property(x => x.LockoutEnd)
                    .HasColumnName("LOCKOUT_END");
                entity.Property(x => x.DefaultRole)
                    .HasColumnName("DEFAULT_ROLE");
                entity.Property(x => x.NormalizedEmail)
                    .HasColumnName("NORMALIZED_EMAIL");
                entity.Property(x => x.NormalizedUserName)
                    .HasColumnName("NORMALIZED_USER_NAME");
                entity.Property(x => x.PasswordHash)
                    .HasColumnName("PASSWORD_HASH");
                entity.Property(x => x.PhoneNumber)
                    .HasColumnName("PHONE_NUMBER");
                entity.Property(x => x.PhoneNumberConfirmed)
                    .HasColumnName("PHONE_NUMBER_CONFIRMED");
                entity.Property(x => x.SecurityStamp)
                    .HasColumnName("SECURITY_STAMP");
                entity.Property(x => x.TwoFactorEnabled)
                    .HasColumnName("TWO_FACTOR_ENABLED");
                entity.Property(x => x.UserName)
                    .HasColumnName("USER_NAME");
            }).Model.SetMaxIdentifierLength(30);
            
            modelBuilder.Entity<Tick.Domain.Entities.Task>(entity =>
            {
                entity.ToTable(name: "TASK");

                // Ticker-Task DB relationship
                entity.HasOne(x => x.Ticker)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.TickerId);

            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "ROLE");
                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");
                entity.Property(x => x.ConcurrencyStamp)
                    .HasColumnName("CONCURRENCY_STAMP");
                entity.Property(x => x.Name)
                    .HasColumnName("NAME");
                entity.Property(x => x.NormalizedName)
                    .HasColumnName("NORMALIZED_NAME");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("USER_ROLES");
                entity.Property(x => x.RoleId)
                    .HasColumnName("ROLE_ID");
                entity.Property(x => x.UserId)
                    .HasColumnName("USER_ID");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("USER_CLAIMS");
                entity.Property(x => x.ClaimType)
                    .HasColumnName("CLAIM_TYPE");
                entity.Property(x => x.ClaimValue)
                    .HasColumnName("CLAIM_VALUE");
                entity.Property(x => x.Id)
                    .HasColumnName("ID");
                entity.Property(x => x.UserId)
                    .HasColumnName("USER_ID");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("ROLE_CLAIMS");
                entity.Property(x => x.ClaimType)
                    .HasColumnName("CLAIM_TYPE");
                entity.Property(x => x.ClaimValue)
                    .HasColumnName("CLAIM_VALUE");
                entity.Property(x => x.Id)
                    .HasColumnName("ID");
                entity.Property(x => x.RoleId)
                    .HasColumnName("ROLE_ID");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("USER_LOGINS");
                entity.Property(x => x.LoginProvider)
                    .HasColumnName("LOGIN_PROVIDER");
                entity.Property(x => x.ProviderDisplayName)
                    .HasColumnName("PROVIDER_DISPLAY_NAME");
                entity.Property(x => x.ProviderKey)
                    .HasColumnName("PROVIDER_KEY");
                entity.Property(x => x.UserId)
                    .HasColumnName("USER_ID");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("USER_TOKENS");
                entity.Property(x => x.LoginProvider)
                    .HasColumnName("LOGIN_PROVIDER");
                entity.Property(x => x.Name)
                    .HasColumnName("NAME");
                entity.Property(x => x.Value)
                    .HasColumnName("VALUE");
                entity.Property(x => x.UserId)
                    .HasColumnName("USER_ID");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Seed();
        }

        public async override System.Threading.Tasks.Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamp();
            AddAuditMeta();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task BeginTransactionAsync(
            IsolationLevel isolationLevel,
            CancellationToken cancellationToken = default
        )
        {
            _currentTransaction ??= await Database.BeginTransactionAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await SaveChangesAsync(cancellationToken);
                await _currentTransaction?.CommitAsync(cancellationToken)!;
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }

        public async System.Threading.Tasks.Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _currentTransaction?.RollbackAsync(cancellationToken)!;
            }
            finally
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }

        public System.Threading.Tasks.Task RetryOnExceptionAsync(Func<System.Threading.Tasks.Task> operation)
        {
            return Database.CreateExecutionStrategy().ExecuteAsync(operation);
        }

        public Task<TResult> RetryOnExceptionAsync<TResult>(Func<Task<TResult>> operation)
        {
            return Database.CreateExecutionStrategy().ExecuteAsync(operation);
        }

        public System.Threading.Tasks.Task ExecuteTransactionalAsync(Func<System.Threading.Tasks.Task> action, CancellationToken cancellationToken = default)
        {
            var strategy = Database.CreateExecutionStrategy();
            return strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    await action();

                    await transaction.CommitAsync(cancellationToken);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }

        public System.Threading.Tasks.Task<T> ExecuteTransactionalAsync<T>(Func<System.Threading.Tasks.Task<T>> action, CancellationToken cancellationToken = default)
        {
            var strategy = Database.CreateExecutionStrategy();
            return strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    var result = await action();

                    await transaction.CommitAsync(cancellationToken);

                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }

        private void AddTimestamp()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is EntityBase && x.State == EntityState.Added);

            foreach (var entity in entities)
            {
                var now = DateTime.Now; // current datetime

                ((EntityBase)entity.Entity).CreatedAt = now;
            }
        }

        private void AddAuditMeta()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is AuditableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value ?? "internal";
                var now = DateTime.Now; // current datetime

                if (entity.State == EntityState.Added)
                {
                    ((AuditableEntity)entity.Entity).CreatedBy = userId;
                }
                if (entity.State == EntityState.Modified)
                {
                    ((AuditableEntity)entity.Entity).UpdatedAt = now;
                    ((AuditableEntity)entity.Entity).UpdatedBy = userId;
                }
            }
        }
    }
}