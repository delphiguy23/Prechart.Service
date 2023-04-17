using FluentAssertions;
using NSubstitute;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Werkgever.Repository;
using Prechart.Service.Werkgever.Service;
using Xunit;

namespace Prechart.Service.Werkgever.Test.Service;

public partial class WerkgeverServiceSpec
{
    public class WhenUpsertMongoWerkgevers : WerkgeverServiceSpec
    {
        private readonly List<MongoWerkgeverModel> werkgevers = null;
        private IFluentResults<List<MongoWerkgeverModel>> result;

        public When UpsertMongoWerkgevers => async () => result = await Subject.HandleAsync(new WerkgeverService.UpsertMongoWerkgevers
        {
            Werkgevers = werkgevers,
        }, CancellationToken.None);

        public class AndException : WhenUpsertMongoWerkgevers
        {
            public And Exception => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.UpsertWerkgevers>(), CancellationToken.None)
                .Returns<IFluentResults<List<MongoWerkgeverModel>>>(e => throw new Exception("exception"));

            [Fact]
            public void ThenShouldReturnSuccess()
            {
                result.IsFailure().Should().BeTrue();
            }

            [Fact]
            public void ThenLoggerError()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
            }
        }

        public class AndFailure : WhenUpsertMongoWerkgevers
        {
            public And Failure => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.UpsertWerkgevers>(), CancellationToken.None)
                .Returns<IFluentResults<List<MongoWerkgeverModel>>>(e => ResultsTo.Failure<List<MongoWerkgeverModel>>("failure"));

            [Fact]
            public void ThenShouldReturnNone()
            {
                result.IsFailure().Should().BeTrue();
            }
        }

        public class AndDataNoKlant : WhenUpsertMongoWerkgevers
        {
            public And Data => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.UpsertWerkgevers>(), CancellationToken.None)
                .Returns<IFluentResults<List<MongoWerkgeverModel>>>(e => ResultsTo.Success(
                    new List<MongoWerkgeverModel>
                    {
                        new()
                        {
                            Id = "633129fb572a926667ed8c5e".ToObjectId(),
                            Actief = true,
                        },
                    }));

            [Fact]
            public void ThenShouldReturnNone()
            {
                result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndDataSomeKlant : WhenUpsertMongoWerkgevers
        {
            public And Data => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.UpsertWerkgevers>(), CancellationToken.None)
                .Returns<IFluentResults<List<MongoWerkgeverModel>>>(e => ResultsTo.Success(
                    new List<MongoWerkgeverModel>
                    {
                        new()
                        {
                            Id = "633129fb572a926667ed8c5e".ToObjectId(),
                            Actief = true,
                            FiscaalNummer = "123456789L01",
                            Klant = new WerkgeverKlantModel
                            {
                                KlantId = "633129fb572a926667ed8c5g",
                                KlantName = "Klant",
                            },
                        },
                    }));

            [Fact]
            public void ThenShouldReturnSuccess()
            {
                result.IsSuccess().Should().BeTrue();
            }

            [Fact]
            public void ThenShouldReturnData()
            {
                result
                    .Value
                    .Any(f => f.FiscaalNummer == "123456789L01")
                    .Should()
                    .BeTrue();
            }
        }
    }
}
