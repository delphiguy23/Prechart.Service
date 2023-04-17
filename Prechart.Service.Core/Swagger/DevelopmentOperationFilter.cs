using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using Prechart.Service.Core.Models;

namespace Prechart.Service.Core.Swagger
{
    public class DevelopmentOperationFilter : IOperationFilter
    {
        private readonly IEnumerable<EventTypePair> eventTypes;

        private readonly IWebHostEnvironment hostingEnvironment;

        public DevelopmentOperationFilter(IEnumerable<EventTypePair> eventTypes, IWebHostEnvironment hostingEnvironment)
        {
            this.eventTypes = eventTypes;
            this.hostingEnvironment = hostingEnvironment;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.RelativePath.StartsWith("service/event/publish/{type}", StringComparison.InvariantCulture))
            {
                if (!hostingEnvironment.IsDevelopment())
                {
                    return;
                }

                foreach (var eventType in eventTypes)
                {
                    context.SchemaGenerator.GenerateSchema(eventType.EventMockType, context.SchemaRepository);
                }
            }
        }
    }
}
