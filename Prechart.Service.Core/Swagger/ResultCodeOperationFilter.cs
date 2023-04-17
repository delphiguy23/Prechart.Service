using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Prechart.Service.Core.Swagger
{
    public class ResultCodeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!operation.Responses.ContainsKey("400") && context.MethodInfo.GetParameters().Any())
            {
                operation.Responses.Add("400", new OpenApiResponse
                {
                    Description = "Invalid user input",
                });
            }

            var allowAnonymous = context.ApiDescription.CustomAttributes().FirstOrDefault(a => a is AllowAnonymousAttribute) != null;
            if (!operation.Responses.ContainsKey("401") && !allowAnonymous)
            {
                operation.Responses.Add("401", new OpenApiResponse
                {
                    Description = "Not authenticated, provide a valid token",
                });
            }

            if (!operation.Responses.ContainsKey("403") && !allowAnonymous)
            {
                operation.Responses.Add("403", new OpenApiResponse
                {
                    Description = "Not authorized",
                });
            }

            if (!operation.Responses.ContainsKey("500"))
            {
                operation.Responses.Add("500", new OpenApiResponse
                {
                    Description = "The request could not be processed succesfully",
                });
            }
        }
    }
}
