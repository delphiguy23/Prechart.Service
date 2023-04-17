using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Prechart.Service.Core.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Prechart.Service.Core.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerSetup(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "Prechart Platform API", Version = "v1"});
            c.TagActionsBy(t =>
            {
                var name = string.Empty;
                var controllerName = (t.ActionDescriptor as ControllerActionDescriptor)?.ControllerTypeInfo?.FullName;
                if (controllerName != null)
                {
                    name = controllerName.ToLowerInvariant().Replace("Prechart.", string.Empty, StringComparison.InvariantCultureIgnoreCase);
                    var index = name.IndexOf(".controllers", StringComparison.InvariantCultureIgnoreCase);
                    if (index > 0)
                    {
                        name = name.Substring(0, index);
                    }
                }
                else
                {
                    var pathparts = t.RelativePath.Split('/');
                    name = string.Join("/", pathparts.Skip(1).Take(1));
                }

                return new[] {name};
            });
            var security = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                    },
                    Array.Empty<string>()
                },
            };
            c.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
            c.AddSecurityRequirement(security);
            c.CustomSchemaIds(x => x.FullName.Replace("Prechart.Events.", string.Empty, StringComparison.InvariantCultureIgnoreCase).Replace("Prechart.Service.", string.Empty, StringComparison.InvariantCultureIgnoreCase)
                .Replace("Prechart.Core.", string.Empty, StringComparison.InvariantCultureIgnoreCase).Replace(".", "_", StringComparison.InvariantCultureIgnoreCase).Replace("`1[[", "_", StringComparison.InvariantCultureIgnoreCase)
                .Replace(", ", "_", StringComparison.InvariantCultureIgnoreCase).Replace("_version=1_0_0_0_culture=neutral_publickeytoken=null]]", string.Empty, StringComparison.InvariantCultureIgnoreCase).ToLowerInvariant());
            c.CustomOperationIds(x =>
                x.TryGetMethodInfo(out var methodInfo)
                    ? $"{methodInfo.ReflectedType.FullName.Replace("Prechart.Service", string.Empty, StringComparison.InvariantCultureIgnoreCase).Replace(".Controllers.", "_", StringComparison.InvariantCultureIgnoreCase).Replace("Controller", string.Empty, StringComparison.InvariantCultureIgnoreCase)}_{methodInfo.Name}"
                        .Replace(".", string.Empty, StringComparison.InvariantCultureIgnoreCase).ToLowerInvariant()
                    : null);
            c.OperationFilter<DevelopmentOperationFilter>();
            c.OperationFilter<BinaryPayloadOperationFilter>();
            c.OperationFilter<FileResultContentTypeOperationFilter>();
            c.OperationFilter<JsonPatchExampleOperationFilter>();
            c.OperationFilter<ResultCodeOperationFilter>();
            c.DocumentFilter<DevelopmentDocumentFilter>();
        });

        services.AddSwaggerGenNewtonsoftSupport();

        return services;
    }

    public static IApplicationBuilder UseSwaggerSetup(this IApplicationBuilder app, bool isDevelopment = false)
    {
        app.UseSwagger(c =>
        {
            var scheme = isDevelopment ? "http" : "https";
            c.RouteTemplate = "service/swagger/{documentName}/swagger.json";
            c.PreSerializeFilters.Add((swaggerDoc, httpReq) => { swaggerDoc.Servers = new List<OpenApiServer> {new() {Url = $"{scheme}://{httpReq.Host.Value}"}}; });
        });
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "service/swagger";
            c.SwaggerEndpoint("v1/swagger.json", "Prechart Platform API");
        });

        return app;
    }
}
