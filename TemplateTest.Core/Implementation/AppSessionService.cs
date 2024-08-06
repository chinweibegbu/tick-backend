using TemplateTest.Core.Cache;
using TemplateTest.Core.Contract;
using System;
using System.Threading.Tasks;

namespace TemplateTest.Core.Implementation
{
    public class AppSessionService : IAppSessionService
    {
        private readonly ICacheService _cacheService;
        public AppSessionService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<bool> CreateSession(string sessionId, string username)
        {
            var sessionKey = $"session_{username}";
            // If there is an existing session, the command below will overwrite it
            await _cacheService.SetAsync(sessionKey, sessionId, TimeSpan.FromMinutes(2));
            return true;
        }

        public void DeleteSession(string username)
        {
            var sessionKey = $"session_{username}";
            _cacheService.Remove(sessionKey);
        }

        public async Task<bool> ValidateSession(string sessionId, string username)
        {
            var sessionKey = $"session_{username}";
            var existingSessionId = await _cacheService.GetAsync<string>(sessionKey);

            if (sessionId == existingSessionId)
            {
                return true;
            }
            return false;
        }
    }
}
