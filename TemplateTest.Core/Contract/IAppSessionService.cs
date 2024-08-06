using System.Threading.Tasks;

namespace TemplateTest.Core.Contract
{
    public interface IAppSessionService
    {
        Task<bool> CreateSession(string sessionId, string username);
        void DeleteSession(string username);
        Task<bool> ValidateSession(string sessionId, string username);
    }
}
