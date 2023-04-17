using Hangfire.Dashboard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Prechart.Service.Core.Configuration;

namespace Prechart.Service.Core.Authorization
{
    public class BearerAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly GeneralConfiguration _generalConfiguration;
        private readonly ITokenHelper _tokenHelper;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public BearerAuthorizationFilter(IOptions<GeneralConfiguration> generalConfiguration, ITokenHelper tokenHelper, IWebHostEnvironment hostingEnvironment)
        {
            _generalConfiguration = generalConfiguration.Value;
            _tokenHelper = tokenHelper;
            _hostingEnvironment = hostingEnvironment;
        }

        public bool Authorize(DashboardContext context)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                return true;
            }

            var httpContext = context.GetHttpContext();

            if (httpContext.Request.Cookies.TryGetValue("Bearer", out var token))
            {
                if (_tokenHelper.TryParseToken(token, out var claims))
                {
                    return claims.HasRole("administrator");
                }

                return false;
            }

            return httpContext.User.Identity.IsAuthenticated;
        }
    }
}
