using Serilog;

namespace Prechart.Service.Core.Helper;

public interface ISerilogHelper
{
    LoggerConfiguration CreateSerilogLogger();
}
