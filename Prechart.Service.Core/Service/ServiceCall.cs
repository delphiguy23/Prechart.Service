using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Prechart.Service.Core.Authorization;
using Prechart.Service.Core.Configuration;

namespace Prechart.Service.Core.Service
{
    public class ServiceCall : IServiceCall
    {
        private readonly GeneralConfiguration _generalConfiguration;
        private readonly ILogger<ServiceCall> _logger;
        private readonly HttpClient _httpClient;
        private readonly ITokenHelper _tokenHelper;

        public ServiceCall(
            IOptions<GeneralConfiguration> generalConfiguration,
            HttpClient httpClient,
            ILogger<ServiceCall> logger,
            ITokenHelper tokenhelper)
        {
            _generalConfiguration = generalConfiguration.Value;
            _logger = logger;
            _httpClient = httpClient;
            _tokenHelper = tokenhelper;
        }

        public async Task<IResult<HttpContent>> Send(ServiceCallRequest request)
        {
            _logger.LogInformation("Send {@request}", request);

            try
            {
                using (var req = CreateHttpRequest(request))
                {
                    var process = await _httpClient.SendAsync(req);
                    if (!process.IsSuccessStatusCode)
                    {
                        _logger.LogError("Invalid server response: {@responseCode} {@response}", process.StatusCode, await process.Content.ReadAsStringAsync());
                        return "Invalid response".Failure<HttpContent>();
                    }

                    return process.Content.Some();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Send));
                return "Exception".Failure<HttpContent>();
            }
        }

        private HttpRequestMessage CreateHttpRequest(ServiceCallRequest request)
        {
            Uri serviceUri = !string.IsNullOrEmpty(request.ServiceName) ?
                new Uri($"{_generalConfiguration.HostName}/service/{request.ServiceName}/{request.Path}")
                : new Uri($"{_generalConfiguration.HostName}/{request.Path}");
            var requestMessage = new HttpRequestMessage
            {
                RequestUri = serviceUri,
                Method = request.Method,
                Content = request.Content,
            };

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenHelper.GenerateToken(request.Claims, 60));
            return requestMessage;
        }
    }

    public class ServiceCallRequest
    {
        public string ServiceName { get; set; }
        public string Path { get; set; }
        public HttpContent Content { get; set; }
        public HttpMethod Method { get; set; }
        public IList<Claim> Claims { get; set; }
    }
}
