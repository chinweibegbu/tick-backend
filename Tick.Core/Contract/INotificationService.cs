using Tick.Core.DTO.Response;
using System.Threading.Tasks;

namespace Tick.Core.Contract
{
    public interface INotificationService
    {
        Task<SendMailResponse> SendPasswordResetToken(string userName, string url, string firstName, string email);
    }
}
