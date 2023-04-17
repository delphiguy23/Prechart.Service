using Microsoft.AspNetCore.JsonPatch;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace Prechart.Service.Core.Swagger
{
    public class JsonPatchExampleOperationFilter : IOperationFilter
    {
        private const string JsonPatchMediaType = "application/json-patch+json";

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var jsonPatchParameter = context.MethodInfo.GetParameters().FirstOrDefault(p => p.ParameterType.GetInterfaces().Contains(typeof(IJsonPatchDocument)));
            var modelType = jsonPatchParameter?.ParameterType.GenericTypeArguments.FirstOrDefault();
            if (modelType != null && operation.RequestBody.Content.ContainsKey(JsonPatchMediaType))
            {
                AddExample(operation, modelType);
                RemoveNoneJsonPatchMediaTypes(operation);
            }
        }

        private static void RemoveNoneJsonPatchMediaTypes(OpenApiOperation operation)
        {
            var types = operation.RequestBody.Content.Keys.Where(k => k != JsonPatchMediaType);
            foreach (var type in types)
            {
                operation.RequestBody.Content.Remove(type);
            }
        }

        private static void AddExample(OpenApiOperation operation, Type modelType)
        {
            var exampleData = new OpenApiArray();
            exampleData.AddRange(modelType.GetProperties().Where(f => f.IsPubliclyWritable()).Select(f => new OpenApiObject
            {
                ["path"] = new OpenApiString($"/{f.Name}"),
                ["op"] = new OpenApiString("add"),
                ["value"] = ToValue(f.PropertyType),
            }));
            operation.RequestBody.Content[JsonPatchMediaType].Examples.Add(modelType.Name, new OpenApiExample
            {
                Value = exampleData,
            });
        }

        private static IOpenApiAny ToValue(Type propertyType)
        {
            if (propertyType.IsAssignableFrom(typeof(DateTime)))
            {
                return new OpenApiDateTime(DateTime.UtcNow);
            }

            if (propertyType.IsAssignableFrom(typeof(int)))
            {
                return new OpenApiInteger(1234);
            }

            if (propertyType.IsAssignableFrom(typeof(float)))
            {
                return new OpenApiFloat(12.34f);
            }

            if (propertyType.IsAssignableFrom(typeof(decimal)))
            {
                return new OpenApiFloat(12.34f);
            }

            if (propertyType.IsAssignableFrom(typeof(double)))
            {
                return new OpenApiDouble(1234.5678);
            }

            if (propertyType.IsAssignableFrom(typeof(long)))
            {
                return new OpenApiLong(12345678);
            }

            if (propertyType.IsAssignableFrom(typeof(bool)))
            {
                return new OpenApiBoolean(true);
            }

            if (propertyType.IsArray)
            {
                return new OpenApiArray();
            }

            if (propertyType.IsEnum)
            {
                return new OpenApiString(string.Join(",", propertyType.GetEnumNames()));
            }

            var nullableType = Nullable.GetUnderlyingType(propertyType);
            if (nullableType?.IsEnum == true)
            {
                return new OpenApiString(string.Join(",", nullableType.GetEnumNames()));
            }

            return new OpenApiString("text");
        }
    }
}
