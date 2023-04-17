using System;
using System.Reflection;

namespace Prechart.Service.Core.Extensions
{
    public static class HangfireExtensions
    {
        public static string GetQueueName(this object theObject) => theObject.GetType().Assembly.GetQueueName();

        public static string GetQueueName(this Assembly assembly) => assembly.GetName().Name.ToLowerInvariant().Replace(".", string.Empty, StringComparison.InvariantCultureIgnoreCase).Replace("Prechart", string.Empty, StringComparison.InvariantCultureIgnoreCase);
    }
}
