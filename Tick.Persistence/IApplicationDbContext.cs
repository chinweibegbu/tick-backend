using Tick.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.ConstrainedExecution;

namespace Tick.Persistence
{
    public interface IApplicationDbContext
    {
        public DbSet<BasicUser> BasicUser { get; set; }
        public DbSet<Ticker> Ticker { get; set; }
        public DbSet<Tick.Domain.Entities.Task> Task { get; set; }


        System.Threading.Tasks.Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DatabaseFacade Database { get; }
        System.Threading.Tasks.Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
        System.Threading.Tasks.Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        System.Threading.Tasks.Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        System.Threading.Tasks.Task ExecuteTransactionalAsync(Func<System.Threading.Tasks.Task> action, CancellationToken cancellationToken = default);
        System.Threading.Tasks.Task<T> ExecuteTransactionalAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);
    }
}