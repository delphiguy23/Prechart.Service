using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Prechart.Service.Core.TestBase
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        public Queue<string> Requests { get; } = new Queue<string>();
        public Queue<object> Responses { get; } = new Queue<object>();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            Requests.Enqueue(JsonConvert.SerializeObject(request, new JsonSerializerSettings { Converters = new List<JsonConverter> { new HttpContentJsonConverter() } }));
            var response = Responses.Dequeue();
            if (response is Exception)
            {
                throw response as Exception;
            }

            return Task.FromResult(response as HttpResponseMessage);
        }

        public void VerifyJsonObjectPost(Uri uri, object jsonObject, string authorizationHeader)
        {
            using (var httpRequest = new HttpRequestMessage
            {
                RequestUri = uri,
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(jsonObject), Encoding.UTF8, "application/json"),
            })
            {
                httpRequest.Headers.Add("Authorization", authorizationHeader);
                Verify(httpRequest);
            }
        }

        public void Verify(FakeHttpRequestMessage request)
        {
            using (var httpRequest = new HttpRequestMessage
            {
                RequestUri = request.RequestUri,
                Method = request.Method,
                Content = request.Content,
            })
            {
                foreach (var header in request.Headers)
                {
                    httpRequest.Headers.Add(header.Key, header.Value);
                }

                Verify(httpRequest);
            }
        }

        public void Verify(HttpRequestMessage request)
        {
            var call = Requests.Dequeue();
            call.Should().Be(JsonConvert.SerializeObject(request, new JsonSerializerSettings { Converters = new List<JsonConverter> { new HttpContentJsonConverter() } }));
        }
    }

    public class FakeHttpRequestMessage
    {
        public Uri RequestUri { get; set; }
        public HttpMethod Method { get; set; }
        public HttpContent Content { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    }
}
