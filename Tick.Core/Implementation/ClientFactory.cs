using Tick.Core.Contract;
using Tick.Core.Extension;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Tick.Core.Implementation
{
    public class ClientFactory : IClientFactory
    {
        private readonly ILogger<ClientFactory> _logger;

        public ClientFactory(ILogger<ClientFactory> logger)
        {
            _logger = logger;
        }

        public async Task<SampleResponse> PostDataAsync<SampleResponse, SampleRequest>(string endPoint, SampleRequest dto)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(handler))
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Stopwatch timer = new Stopwatch();
                timer.Start();

                HttpResponseMessage httpResponse = await client.PostAsync(endPoint, content);
                timer.Stop();
                _logger.LogWarning($"HTTP Response Code for {endPoint}:: {httpResponse.StatusCode} Response Time:: {timer.ElapsedMilliseconds}ms");

                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogWarning($"JSON Response for {endPoint}:: {jsonString}");
                var data = JsonConvert.DeserializeObject<SampleResponse>(jsonString);

                return data;
            }
        }

        public async Task<SampleResponse> PostDataAsync<SampleResponse>(string endPoint, ICollection<KeyValuePair<string, string>> headers)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(handler))
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(String.Empty), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }

                Stopwatch timer = new Stopwatch();
                timer.Start();

                HttpResponseMessage httpResponse = await client.PostAsync(endPoint, content);
                timer.Stop();
                _logger.LogWarning($"HTTP Response Code for {endPoint}:: {httpResponse.StatusCode} Response Time:: {timer.ElapsedMilliseconds}ms");

                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogWarning($"JSON Response for {endPoint}:: {jsonString}");
                var data = JsonConvert.DeserializeObject<SampleResponse>(jsonString);

                return data;
            }
        }

        public async Task<SampleResponse> PostDataAsync<SampleResponse, SampleRequest>(string endPoint, SampleRequest dto, ICollection<KeyValuePair<string, string>> headers)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(handler))
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }

                Stopwatch timer = new Stopwatch();
                timer.Start();

                HttpResponseMessage httpResponse = await client.PostAsync(endPoint, content);
                timer.Stop();
                _logger.LogWarning($"HTTP Response Code for {endPoint}:: {httpResponse.StatusCode} Response Time:: {timer.ElapsedMilliseconds}ms");

                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogWarning($"JSON Response for {endPoint}:: {jsonString}");
                var data = JsonConvert.DeserializeObject<SampleResponse>(jsonString);

                return data;
            }
        }

        public async Task<SampleResponse> GetDataAsync<SampleResponse>(string endPoint, ICollection<KeyValuePair<string, string>> headers = null)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }

                Stopwatch timer = new Stopwatch();
                timer.Start();

                HttpResponseMessage httpResponse = await client.GetAsync(endPoint);
                timer.Stop();
                _logger.LogWarning($"HTTP Response Code for {endPoint}:: {httpResponse.StatusCode} Response Time:: {timer.ElapsedMilliseconds}ms");

                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogWarning($"JSON Response for {endPoint}:: {jsonString}");
                var data = JsonConvert.DeserializeObject<SampleResponse>(jsonString);

                return data;
            }
        }

        public async Task<SampleResponse> GetDataAsync<SampleResponse, SampleRequest>(string endPoint, SampleRequest request)
        {
            IDictionary<string, string> param = request.ToDictionary();

            Uri url = new Uri(QueryHelpers.AddQueryString(endPoint, param));

            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Stopwatch timer = new Stopwatch();
                timer.Start();

                HttpResponseMessage httpResponse = await client.GetAsync(url);
                timer.Stop();
                _logger.LogWarning($"HTTP Response Code for {url}:: {httpResponse.StatusCode} Response Time:: {timer.ElapsedMilliseconds}ms");

                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogWarning($"JSON Response for {url}:: {jsonString}");
                var data = JsonConvert.DeserializeObject<SampleResponse>(jsonString);

                return data;
            }
        }

        public async Task<SampleResponse> GetDataAsync<SampleResponse>(string endPoint)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Stopwatch timer = new Stopwatch();
                timer.Start();

                HttpResponseMessage httpResponse = await client.GetAsync(endPoint);
                timer.Stop();
                _logger.LogWarning($"HTTP Response Code for {endPoint}:: {httpResponse.StatusCode} Response Time:: {timer.ElapsedMilliseconds}ms");

                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogWarning($"JSON Response for {endPoint}:: {jsonString}");
                var data = JsonConvert.DeserializeObject<SampleResponse>(jsonString);

                return data;
            }
        }
    }
}
