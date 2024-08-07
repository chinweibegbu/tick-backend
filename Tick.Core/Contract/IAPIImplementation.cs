using Tick.Core.DTO.Request;
using Tick.Core.DTO.Response;
using System.Threading.Tasks;

namespace Tick.Core.Contract
{
    public interface IAPIImplementation
    {
        Task<SendMailResponse> SendMail(SendMailRequest request);
    }
}
