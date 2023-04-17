using System;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace Prechart.Service.Core.Helper;

public static class SerilogHelper
{
    public static LoggerConfiguration CreateSerilogLogger(string connectionString)
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Logger
            (
                lc => lc
                    .WriteTo.MongoDBCapped(
                        connectionString,
                        collectionName: "Serilogs",
                        restrictedToMinimumLevel: LogEventLevel.Information,
                        cappedMaxSizeMb: 1024,
                        cappedMaxDocuments: 100_000,
                        batchPostingLimit: 100,
                        period: new TimeSpan(0, 0, 1, 0))
                    .Enrich.WithExceptionDetails()
                    .Enrich.FromLogContext()
            );
    }
}
