using TemplateTest.Core.Contract.Repository;
using TemplateTest.Core.Repository.Base;
using TemplateTest.Domain.Entities;
using TemplateTest.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TemplateTest.Core.Repository
{
    public class BasicUserRepository : Repository<BasicUser>, IBasicUserRepository
    {
        public BasicUserRepository(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory, (context) => context.Set<BasicUser>())
        {
        }

        public async Task<BasicUser> GetBasicUserByApiKey(string apiKey, CancellationToken cancellationToken, BasicAuthStatus? status = null)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = GetDatabaseContext(scope);
                var _entities = _getDbSet(dbContext);

                var query = _entities
                    .Where(x => x.ApiKey == apiKey)
                    .AsQueryable();

                if (status.HasValue)
                {
                    query = query.Where(x => x.Status == status.Value);
                }

                var response = await query
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellationToken);

                return response;
            }
        }
    }
}
