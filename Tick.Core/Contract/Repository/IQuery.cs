using Tick.Persistence;
using System.Linq;

namespace Tick.Core.Contract.Repository
{
    public interface IQuery<TOut>
    {
        IQueryable<TOut> Run(ApplicationDbContext dbContext);
    }
}
