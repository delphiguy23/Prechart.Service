using FluentAssertions;
using NSubstitute;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Werkgever.Repository;
using Prechart.Service.Werkgever.Service;
using Xunit;

namespace Prechart.Service.Werkgever.Test.Service;

public partial class WerkgeverServiceSpec
{
    public class WhenSyncFromMongo : WerkgeverServiceSpec
    {
        private IFluentResults<bool> result;

        public When SyncFromMongo => async () => result = await Subject.HandleAsync(new WerkgeverService.SyncFromMongo
        {
            Werkgevers = _werkgevers,
        }, CancellationToken.None);

        public class AndException : WhenSyncFromMongo
        {
            public And Exception => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.UpsertFromMongoWerkgever>(), CancellationToken.None)
                .Returns<IFluentResults<int>>(e => throw new Exception("exception"));

            [Fact]
            public void ThenShouldReturnSuccess()
            {
                result.IsFailure().Should().Be(true);
            }

            [Fact]
            public void ThenLoggerError()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
            }
        }

        public class AndNoWerkgever : WhenSyncFromMongo
        {
            public And Data => () => _werkgevers = null;

            [Fact]
            public void ThenShouldReturnNone()
            {
                result.IsBadRequest().Should().Be(true);
            }
        }

        public class AndUpsert : WhenSyncFromMongo
        {
            public class AndNotUpserted : AndUpsert
            {
                public And Data => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.UpsertFromMongoWerkgever>(), CancellationToken.None).Returns<IFluentResults<int>>(e => ResultsTo.NotFound<int>());

                [Fact]
                public void ThenShouldReturnSuccess()
                {
                    result.IsSuccess().Should().Be(true);
                }

                [Fact]
                public void ThenShouldReturnTrue()
                {
                    result.Value.Should().Be(true);
                }
            }

            public class AndUpserted : AndUpsert
            {
                public And Data => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.UpsertFromMongoWerkgever>(), CancellationToken.None).Returns<IFluentResults<int>>(e => ResultsTo.Success(1));

                [Fact]
                public void ThenShouldReturnTrue()
                {
                    result.IsSuccess().Should().Be(true);
                }

                [Fact]
                public async Task ThenUpsertWhk()
                {
                    var werkgever = _werkgevers.FirstOrDefault();
                    await _repository.Received(1).HandleAsync(Verify.That<WerkgeverRepository.UpsertFromMongoWerkgeverWhk>(
                        i =>
                            i.Should()
                                .BeEquivalentTo(new
                                {
                                    Werkgever = werkgever,
                                    WerkgeverId = 1,
                                })
                    ), CancellationToken.None);
                }
            }
        }
    }
}
