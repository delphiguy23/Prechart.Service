using MassTransit;

namespace Prechart.Service.Core.Events;

    public interface IWrappedMessage { }

    public interface IWrappedMessage<T> : IWrappedMessage
    {
        public MessageData<string> Content { get; }
    }
