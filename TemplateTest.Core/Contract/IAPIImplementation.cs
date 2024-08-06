using TemplateTest.Core.DTO.Request;
using TemplateTest.Core.DTO.Response;
using System.Threading.Tasks;

namespace TemplateTest.Core.Contract
{
    public interface IAPIImplementation
    {
        Task<SendMailResponse> SendMail(SendMailRequest request);
    }
}
