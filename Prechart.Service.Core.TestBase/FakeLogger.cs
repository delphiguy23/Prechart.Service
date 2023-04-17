using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Prechart.Service.Core.TestBase
{
    public class FakeLogger<T> : ILogger<T>
    {
        public List<LogMessage> ReceivedLogMessages { get; set; } = new List<LogMessage>();

        public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();

        public bool IsEnabled(LogLevel logLevel) => throw new NotImplementedException();

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) => ReceivedLogMessages.Add(new LogMessage { LogLevel = logLevel, Message = state.ToString(), Exception = exception });
    }

    public class LogMessage
    {
        public LogLevel LogLevel { get; internal set; }
        public string Message { get; internal set; }
        public Exception Exception { get; internal set; }
    }
}
