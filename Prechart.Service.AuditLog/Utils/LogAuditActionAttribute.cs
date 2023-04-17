using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Globals.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Prechart.Service.AuditLog.Utils;

public class LogAuditActionAttribute : ActionFilterAttribute
{
    public string User { get; set; }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (next is not null) { await next(); }

        if (context.Controller is ControllerBase controllerBase)
        {
            var endpoint = context.HttpContext.RequestServices.GetService<IPublishEndpoint>();

            if (endpoint is not null)
            {

                var controllerData = context.RouteData.Values.Keys.Select(valuesKey => new Dictionary<string, object> { { valuesKey, context.RouteData.Values[valuesKey] ?? null } }).ToList();
                controllerData.Add(new Dictionary<string, object> { { "RequestBody", Regex.Unescape(Regex.Unescape(StringHelper.RemoveSpecialCharacters(context.HttpContext.Request.BodyToString())) ?? string.Empty) ?? string.Empty } });
                controllerData.Add(new Dictionary<string, object> { { "LocalIp", $"{context.HttpContext.Connection.LocalIpAddress} : {context.HttpContext.Connection.LocalPort}" } });
                controllerData.Add(new Dictionary<string, object> { { "RemoteIp", $"{context.HttpContext.Connection.RemoteIpAddress} : {context.HttpContext.Connection.RemotePort}" } });

                await endpoint.Publish<IControllerLogs>(new ControllerLogs
                {
                    User = context.HttpContext.User.Identity?.Name ?? string.Empty,
                    ActionOn = DateTime.Now,
                    Data = controllerData,
                });
            }
        }
    }
}
