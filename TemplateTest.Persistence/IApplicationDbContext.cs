using TemplateTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace TemplateTest.Persistence
{
    public interface IApplicationDbContext
    {
        public DbSet<BasicUser> BasicUser { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DatabaseFacade Database { get; }
        Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task ExecuteTransactionalAsync(Func<Task> action, CancellationToken cancellationToken = default);
        Task<T> ExecuteTransactionalAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);
    }
}