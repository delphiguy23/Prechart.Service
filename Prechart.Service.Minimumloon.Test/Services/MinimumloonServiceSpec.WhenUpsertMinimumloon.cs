using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.Service;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Minimumloon.Models;
using Prechart.Service.Minimumloon.Repositories;
using Prechart.Service.Minimumloon.Services;
using Xunit;

namespace Prechart.Service.Minimumloon.Test.Service;

public partial class MinimumloonServiceSpec
{
    public class WhenUpsertMinimumLoon : MinimumloonServiceSpec
    {
        private List<MinimumloonModel> minimumloon;
        private IFluentResults Result;

        public When GetUpsertMinimumLoonService => async () => Result = await Subject.HandleAsync(new MinimumloonService.UpsertMinimumLoon { Minimumloon = minimumloon }, CancellationToken.None);

        public class AndUpsertMinimumLoonNone : WhenUpsertMinimumLoon
        {
            public And GetUpsertMinimumLoon => () => Repository.HandleAsync(Arg.Any<MinimumloonRepository.UpsertMinimumLoon>(), CancellationToken.None).Returns(ResultsTo.NotFound<None>());

            [Fact]
            public void ThenResultShouldBeIsNone()
            {
                Result.IsNotFoundOrBadRequest().Should().BeTrue();
            }
        }

        public class AndUpsertMinimumLoonFound : WhenUpsertMinimumLoon
        {
            public And Data => () =>
            {
                minimumloon = new List<MinimumloonModel>
                {
                    new()
                    {
                        Id = 0,
                        MinimumloonLeeftijd = 1,
                        MinimumloonPerDag = 1,
                    },
                };
            };

            public And GetUpsertMinimumLoon => () => Repository.HandleAsync(Arg.Any<MinimumloonRepository.UpsertMinimumLoon>(), CancellationToken.None).Returns(ResultsTo.Success());

            [Fact]
            public void ThenResultShouldBeIsSome()
            {
                Result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndUpsertMinimumLoonExceptionFound : WhenUpsertMinimumLoon
        {
            public And Data => () =>
            {
                minimumloon = new List<MinimumloonModel>
                {
                    new()
                    {
                        Id = 0,
                        MinimumloonLeeftijd = 1,
                        MinimumloonPerDag = 1,
                    },
                };
            };

            public And Exception => () => Repository.HandleAsync(Arg.Any<MinimumloonRepository.UpsertMinimumLoon>(), CancellationToken.None).Returns<IFluentResults>(e => throw new Exception("Error"));


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
    }
}
