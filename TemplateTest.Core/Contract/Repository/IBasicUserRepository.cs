using TemplateTest.Domain.Entities;
using TemplateTest.Domain.Enum;
using System.Threading;
using System.Threading.Tasks;

namespace TemplateTest.Core.Contract.Repository
{
    public interface IBasicUserRepository : IRepository<BasicUser>
    {
        Task<BasicUser> GetBasicUserByApiKey(string apiKey, CancellationToken cancellationToken, BasicAuthStatus? status = null);
    }
}
