using Tick.Core.Contract;
using Tick.Core.DTO.Request;
using Tick.Core.DTO.Response;
using Tick.Core.Extension;
using Tick.Domain.Settings;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Tick.Core.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly IAPIImplementation _apiCall;
        private readonly AdminOptions _adminOptions;

        public NotificationService(IAPIImplementation apiCall,
            IOptions<AdminOptions> adminOptions)
        {
            _apiCall = apiCall;
            _adminOptions = adminOptions.Value;
        }

        /**
         * GENERAL COMMENTS ON THIS SERVICE
         * 
        **/
        public async Task<SendMailResponse> SendPasswordResetToken(string userName, string url, string firstName, string email)
        {
            var emailRequest = new SendMailRequest()
            {
                from = _adminOptions.BroadcastEmail,
                to = email,
                subject = "Tick Password Reset Token",
                mailMessage = EmailTemplates.GetPasswordResetEmail(userName, firstName, url)
            };

            return await _apiCall.SendMail(emailRequest);
        }
    }
}
