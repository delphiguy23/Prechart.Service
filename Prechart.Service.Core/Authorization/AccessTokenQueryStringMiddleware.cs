using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Prechart.Service.Core.Authorization
{
    public class AccessTokenQueryStringMiddleware
    {
        private readonly RequestDelegate next;

        public AccessTokenQueryStringMiddleware(RequestDelegate next) => this.next = next;

        public async Task Invoke(HttpContext context)
        {
            if (context.GetEndpoint()?.Metadata?.GetMetadata<AllowQueryStringAuthenticationAttribute>() != null)
            {
                var token = context.Request.Query["access_token"];
                if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]) &&
                    !string.IsNullOrEmpty(token))
                {
                    context.Request.Headers["Authorization"] = $"Bearer {token}";
                }
            }

            await next(context);
        }
    }
}
