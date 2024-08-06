using TemplateTest.Persistence;
using System.Linq;

namespace TemplateTest.Core.Contract.Repository
{
    public interface IQuery<TOut>
    {
        IQueryable<TOut> Run(ApplicationDbContext dbContext);
    }
}
