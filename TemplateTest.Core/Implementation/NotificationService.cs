using TemplateTest.Core.Contract;
using TemplateTest.Core.DTO.Request;
using TemplateTest.Core.DTO.Response;
using TemplateTest.Core.Extension;
using TemplateTest.Domain.Settings;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace TemplateTest.Core.Implementation
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
                subject = "TemplateTest Admin Password Reset Token",
                mailMessage = EmailTemplates.GetPasswordResetEmail(userName, firstName, url)
            };

            return await _apiCall.SendMail(emailRequest);
        }
    }
}
