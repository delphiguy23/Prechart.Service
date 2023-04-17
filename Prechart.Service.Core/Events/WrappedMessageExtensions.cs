using MassTransit;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Prechart.Service.Core.Events;

public static class WrappedMessageExtensions
    {
        public static async Task<TConcrete> GetMessageAsync<T, TConcrete>(this IWrappedMessage<T> message) => JsonConvert.DeserializeObject<TConcrete>(await message.Content.Value);

        public static async Task PublishWrappedMessage<T>(this IPublishEndpoint publishEndpoint, T message) => await publishEndpoint.Publish<IWrappedMessage<T>>(new { Content = JsonConvert.SerializeObject(message) });
    }
