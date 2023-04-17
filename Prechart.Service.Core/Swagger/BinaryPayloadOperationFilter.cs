using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Prechart.Service.Core.Swagger
{
    public class BinaryPayloadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var requestAttribute = context.MethodInfo.GetCustomAttributes(typeof(BinaryPayloadAttribute), false)
                .Cast<BinaryPayloadAttribute>()
                .FirstOrDefault();

            if (requestAttribute == null)
            {
                return;
            }

            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    {
                        requestAttribute.ContentType,
                        new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "string",
                                Format = "binary",
                            },
                        }
                    },
                },
            };
        }
    }
}
