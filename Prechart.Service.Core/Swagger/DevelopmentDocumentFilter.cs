using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Prechart.Service.Core.Models;
using Prechart.Service.Core.Scheduling;

namespace Prechart.Service.Core.Swagger
{
    public class DevelopmentDocumentFilter : IDocumentFilter
    {
        private readonly IEnumerable<EventTypePair> eventTypes;
        private readonly IEnumerable<IRecurringTask> recurringTasks;
        private readonly IWebHostEnvironment hostingEnvironment;

        public DevelopmentDocumentFilter(IEnumerable<EventTypePair> eventTypes, IEnumerable<IRecurringTask> recurringTasks, IWebHostEnvironment hostingEnvironment)
        {
            this.eventTypes = eventTypes;
            this.recurringTasks = recurringTasks;
            this.hostingEnvironment = hostingEnvironment;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths;

            if (!hostingEnvironment.IsDevelopment() && paths.ContainsKey("/account/token"))
            {
                paths.Remove("/account/token");
            }

            BuildEventPaths(paths);
            BuildSchedulePaths(paths);
        }

        private void BuildSchedulePaths(OpenApiPaths paths)
        {
            var key = "/service/schedule/run/{name}";
            if (paths.ContainsKey(key))
            {
                var eventPath = paths[key];
                paths.Remove(key);

                if (!hostingEnvironment.IsDevelopment())
                {
                    return;
                }

                foreach (var recurringTask in recurringTasks)
                {
                    var newkey = $"/service/schedule/run/{recurringTask.Name}";
                    var newPathItem = new OpenApiPathItem()
                    {
                        Operations = new Dictionary<OperationType, OpenApiOperation>
                        {
                            {
                                OperationType.Get,
                                new OpenApiOperation()
                                {
                                    OperationId = $"{eventPath.Operations[OperationType.Get].OperationId}_{recurringTask.Name}",
                                    Responses = eventPath.Operations[OperationType.Get].Responses,
                                    Tags = eventPath.Operations[OperationType.Get].Tags,
                                }
                            },
                        },
                    };

                    paths.Add(newkey, newPathItem);
                }
            }
        }

        private void BuildEventPaths(OpenApiPaths paths)
        {
            var key = "/service/event/publish/{type}";
            if (paths.ContainsKey(key))
            {
                var eventPath = paths[key];
                paths.Remove(key);

                if (!hostingEnvironment.IsDevelopment())
                {
                    return;
                }

                foreach (var eventType in eventTypes)
                {
                    var newkey = $"/service/event/publish/{eventType.EventMockType.FullName}";
                    var newPathItem = new OpenApiPathItem()
                    {
                        Operations = new Dictionary<OperationType, OpenApiOperation>
                        {
                            {
                                OperationType.Post,
                                new OpenApiOperation()
                                {
                                    OperationId = $"{eventPath.Operations[OperationType.Post].OperationId}_{eventType.EventMockType.FullName}",
                                    Responses = eventPath.Operations[OperationType.Post].Responses,
                                    Tags = eventPath.Operations[OperationType.Post].Tags,
                                    RequestBody = new OpenApiRequestBody
                                    {
                                        Content = new Dictionary<string, OpenApiMediaType>
                                        {
                                            {
                                                "application/json",
                                                new OpenApiMediaType
                                                {
                                                    Schema = new OpenApiSchema
                                                    {
                                                        Reference = new OpenApiReference
                                                        {
                                                            Type = ReferenceType.Schema,
                                                            Id = eventType.EventMockType.FullName,
                                                        },
                                                    },
                                                }
                                            },
                                        },
                                    },
                                }
                            },
                        },
                    };

                    paths.Add(newkey, newPathItem);
                }
            }
        }
    }
}
