using Tick.Core.Contract;
using Tick.Core.DTO.Request;
using Tick.Core.DTO.Response;
using Tick.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tick.Core.Implementation
{
    public class APIImplementation : IAPIImplementation
    {
        private readonly IClientFactory _clientFactory;
        private readonly ILogger<APIImplementation> _logger;
        private readonly ExternalApiOptions _externalApiOptions;
        private readonly MockOptions _mockOptions;

        public APIImplementation(IClientFactory clientFactory,
            ILogger<APIImplementation> logger,
            IOptions<ExternalApiOptions> externalApiOptions,
            IOptions<MockOptions> mockOptions)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _externalApiOptions = externalApiOptions.Value;
            _mockOptions = mockOptions.Value;
        }

        public async Task<SendMailResponse> SendMail(SendMailRequest request)
        {
            var headers = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Authorization", _externalApiOptions.MailAuthorizationKey),
            };

            SendMailResponse response = await _clientFactory.PostDataAsync<SendMailResponse, SendMailRequest>(_externalApiOptions.SendMail, request, headers);

            if (!response.succeeded)
            {
                _logger.LogError($"An error occured while calling {nameof(SendMail)} for {request.to}.");
                _logger.LogError(JsonConvert.SerializeObject(response));
            }
            return response;
        }
    }
}
