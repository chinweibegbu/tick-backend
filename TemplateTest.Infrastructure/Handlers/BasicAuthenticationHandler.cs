using TemplateTest.Core.Contract;
using TemplateTest.Core.DTO.Response;
using TemplateTest.Core.Exceptions;
using TemplateTest.Domain.Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace TemplateTest.Infrastructure.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _accountService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserService accountService)
            : base(options, logger, encoder, clock)
        {
            _accountService = accountService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                throw new ApiException("Authorization key is missing.");
            }

            // Get authorization key
            var authorizationHeader = Request.Headers["Authorization"].ToString();

            BasicAuthResponse basicUser = await _accountService.BasicAuthenticateAsync(authorizationHeader);

            if (basicUser == null || basicUser.Status != BasicAuthStatus.Active)
            {
                return AuthenticateResult.Fail("Invalid Authorization Key.");
            }

            var claims = new[] {
                new Claim("apiKey", basicUser.ApiKey),
                new Claim("name", basicUser.Name),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(identity));

            return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
        }
    }
}