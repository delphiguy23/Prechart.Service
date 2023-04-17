using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prechart.Service.Core.Trackers;

public abstract class BaseEntityTracker<T> : IEntityTracker
        where T : class
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BaseEntityTracker<T>> _logger;

        protected BaseEntityTracker(IPublishEndpoint publishEndpoint, ILogger<BaseEntityTracker<T>> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public abstract string Name { get; }

        public IList<Task> PublishedTasks { get; set; }

        public abstract IList<T> GetEvents(ChangeTracker changeTracker, HttpContext httpContext);

        public void SendEvents(ChangeTracker changeTracker, HttpContext httpContext) => InternalSendEvents(GetEvents(changeTracker, httpContext));

        private void InternalSendEvents(IList<T> events)
        {
            if (events?.Any() == true)
            {
                foreach (var @event in events)
                {
                    var task = _publishEndpoint.Publish<T>(@event).ContinueWith(
                      result =>
                      {
                          if (result.Exception != null)
                          {
                              _logger.LogError(result.Exception, "Sending {@EventType} {@Data}", typeof(T).ToString(), @event);
                          }
                      },
                      TaskScheduler.Default);

                    PublishedTasks?.Add(task);
                }
            }
        }
    }