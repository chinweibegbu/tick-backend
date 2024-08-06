using TemplateTest.Core.DTO.Request;
using TemplateTest.Core.DTO.Response;
using TemplateTest.Domain.Common;
using TemplateTest.Domain.QueryParameters;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TemplateTest.Core.Contract
{
    public interface IUserService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, CancellationToken cancellationToken);
        Task<BasicAuthResponse> BasicAuthenticateAsync(string apiKey, CancellationToken cancellationToken = default);
        Task<Response<string>> LogoutAsync();
        Task<PagedResponse<List<UserResponse>>> GetUsersAsync(UserQueryParameters queryParameters, CancellationToken cancellationToken);
        Task<Response<UserResponse>> GetUserById(string id, CancellationToken cancellationToken);
        Task<Response<string>> AddUserAsync(AddUserRequest request, CancellationToken cancellationToken);
        Task<Response<string>> EditUserAsync(EditUserRequest request, CancellationToken cancellationToken);
        Task<Response<string>> DeleteUserAsync(DeleteUserRequest request, CancellationToken cancellationToken);
        Task<Response<string>> ResetUserAsync(ResetUserRequest request);
        Task<Response<string>> PasswordReset(PasswordResetRequest request);
        Task<Response<string>> ResetUserLockoutAsync(ResetUserRequest request);
    }
}
