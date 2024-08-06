using TemplateTest.Domain.Entities.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TemplateTest.Core.Contract.Repository
{
    public interface IRepository<T> where T : IEntityBase
    {
        Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        Task<T> SaveAsync(T entity, CancellationToken cancellationToken = default);

        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}
