using System;
using System.Linq;
using System.Reflection;
using Prechart.Service.Core.Utils;

namespace Prechart.Service.Core.Extensions
{
    public static class MassTransitExtensions
    {
        public static Type GetEventType(this Type consumerType) => consumerType.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(i => i.Name == "IConsumer`1")?.GenericTypeArguments[0];

        public static Type GetEventTypeMock(this Type consumerType) => TypeCreator.CreateFromInterfaceType(consumerType);
    }
}
