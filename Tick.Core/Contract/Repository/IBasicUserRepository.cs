using Tick.Domain.Entities;
using Tick.Domain.Enum;
using System.Threading;
using System.Threading.Tasks;

namespace Tick.Core.Contract.Repository
{
    public interface IBasicUserRepository : IRepository<BasicUser>
    {
        Task<BasicUser> GetBasicUserByApiKey(string apiKey, CancellationToken cancellationToken, BasicAuthStatus? status = null);
    }
}
