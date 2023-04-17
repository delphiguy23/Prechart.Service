using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Minimumloon.Repositories;
using Prechart.Service.Minimumloon.Services;
using Xunit;

namespace Prechart.Service.Minimumloon.Test.Service;

public partial class MinimumloonServiceSpec
{
    public class WhenGetMinimumLoon : MinimumloonServiceSpec
    {
        private DateTime getDatum;
        private IFluentResults<List<Database.Models.Minimumloon>> Result;

        public When GetGetMinimumLoonService => async () => Result = await Subject.HandleAsync(new MinimumloonService.GetMinimumloon { Datum = getDatum }, CancellationToken.None);

        public class AndGetMinimumLoonIsSome : WhenGetMinimumLoon
        {
            public Given GetParamameters => () => { getDatum = DateTime.Now; };

            public And GetMinimumLoon => () => Repository.HandleAsync(Arg.Any<MinimumloonRepository.GetMinimumloon>(), CancellationToken.None).Returns(
                ResultsTo.Something(
                    new List<Database.Models.Minimumloon>
                    {
                        new()
                        {
                            Id = 0,
                            MinimumloonLeeftijd = 1,
                            MinimumloonPerDag = 1,
                        },
                    })
                    );

            [Fact]
            public void ThenResultShouldBeTrue()
            {
                Result.IsSuccess().Should().BeTrue();
            }

            [Fact]
            public void ThenResultShouldBeIsSome()
            {
                Result.Value.Should().BeEquivalentTo(
                   new List<Database.Models.Minimumloon>
                   {
                        new()
                        {
                            Id = 0,
                            MinimumloonLeeftijd = 1,
                            MinimumloonPerDag = 1,
                        },
                   });
            }
        }


        public class AndGetMinimumLoonNone : WhenGetMinimumLoon
        {
            public Given GetParamameters => () => { getDatum = DateTime.Now; };

            public And GetMinimumLoon => () => Repository.HandleAsync(Arg.Any<MinimumloonRepository.GetMinimumloon>(), CancellationToken.None).Returns(ResultsTo.NotFound<List<Database.Models.Minimumloon>>());


            [Fact]
            public void ThenResultShouldBeIsNoneFound()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }

        public class AndGetMinimumLoonExceptionFound : WhenGetMinimumLoon
        {
            public And Exception => () => Repository.HandleAsync(Arg.Any<MinimumloonRepository.GetMinimumloon>(), CancellationToken.None).Returns<IFluentResults<List<Database.Models.Minimumloon>>>(e => throw new Exception("Error"));


            public Given GetParamameters => () => { getDatum = DateTime.Now; };

            [Fact]
            public void ThenLoggerException()
            {
                Logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message.StartsWith("Error")).Should().BeTrue();
            }

            [Fact]
            public void ThenResultShouldBeIsExceptionFound()
            {
                Result.IsFailure().Should().BeTrue();
            }
        }
    }
}
