using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using System.Threading.Tasks;

namespace Prechart.Service.Core.Authorization
{
    public class ServiceAccessAuthorizationHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var pendingRequirements = context.PendingRequirements.ToList();
            var claims = context.User.Claims;

            if ((claims.IsService() && context.Resource is HttpContext httpcontext && httpcontext.GetEndpoint() is RouteEndpoint routeEndpoint && claims.HasServiceRight(routeEndpoint.RoutePattern.RawText))
                ||
                (claims.IsUser() && claims.HasRole("Administrator")))
            {
                foreach (var pendingRequirement in pendingRequirements)
                {
                    context.Succeed(pendingRequirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
