using TemplateTest.Core.DTO.Response;
using System.Threading.Tasks;

namespace TemplateTest.Core.Contract
{
    public interface INotificationService
    {
        Task<SendMailResponse> SendPasswordResetToken(string userName, string url, string firstName, string email);
    }
}
