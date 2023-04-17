using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.Service;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Minimumloon.Repositories;
using Prechart.Service.Minimumloon.Services;
using Xunit;

namespace Prechart.Service.Minimumloon.Test.Service;

public partial class MinimumloonServiceSpec
{
    public class WhenDeleteMinimumloon : MinimumloonServiceSpec
    {
        private IFluentResults Result;

        public When GetDeleteMinimumloonService => async () => Result = await Subject.HandleAsync(new MinimumloonService.DeleteMinimumloon { Datum = DateTime.Now, Leeftijd = 21 }, CancellationToken.None);

        public class AndDeleteMinimumloonNone : WhenDeleteMinimumloon
        {
            public And GetDeleteMinimumloon => () => Repository.HandleAsync(Arg.Any<MinimumloonRepository.DeleteMinimumloon>(), CancellationToken.None).Returns(ResultsTo.NotFound<None>());

            [Fact]
            public void ThenResultShouldBeIsNone()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }

        public class AndDeleteMinimumloonException : WhenDeleteMinimumloon
        {
            public And GetDeleteMinimumloon => () => Repository.HandleAsync(Arg.Any<MinimumloonRepository.DeleteMinimumloon>(), CancellationToken.None).Returns<IFluentResults>(e => throw new Exception("Error"));

            [Fact]
            public void ThenLoggerException()
            {
                Logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message.StartsWith("Error")).Should().BeTrue();
            }

            [Fact]
            public void ThenResultShouldBeIsNone()
            {
                Result.IsFailure().Should().BeTrue();
            }
        }

        public class AndGetTaxYearNotFound : WhenDeleteMinimumloon
        {
            public And GetDeleteMinimumloon => () => Repository.HandleAsync(Arg.Any<MinimumloonRepository.DeleteMinimumloon>(), CancellationToken.None).Returns(ResultsTo.Success());

            [Fact]
            public void ThenResultShouldBeIsNone()
            {
                Result.IsSuccess().Should().BeTrue();
            }
        }
    }
}
