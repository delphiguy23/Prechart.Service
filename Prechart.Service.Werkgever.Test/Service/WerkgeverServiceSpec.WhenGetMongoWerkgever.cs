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
    public class WhenGetMongoWerkgever : WerkgeverServiceSpec
    {
        private IFluentResults<List<MongoWerkgeverModel>> result;

        public When GetMongoWerkgever => async () => result = await Subject.HandleAsync(new WerkgeverService.GetMongoWerkgever {Taxno = "123456789L01"}, CancellationToken.None);

        public class AndException : WhenGetMongoWerkgever
        {
            public And Exception => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.GetMongoWerkgever>(), CancellationToken.None)
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

        public class AndFailure : WhenGetMongoWerkgever
        {
            public And Failure => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.GetMongoWerkgever>(), CancellationToken.None)
                .Returns<IFluentResults<List<MongoWerkgeverModel>>>(e => ResultsTo.Failure<List<MongoWerkgeverModel>>("failure"));

            [Fact]
            public void ThenShouldReturnNone()
            {
                result.IsFailure().Should().Be(true);
            }
        }

        public class AndDataNone : WhenGetMongoWerkgever
        {
            public And Data => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.GetMongoWerkgever>(), CancellationToken.None)
                .Returns<IFluentResults<List<MongoWerkgeverModel>>>(e => ResultsTo.NotFound<List<MongoWerkgeverModel>>());

            [Fact]
            public void ThenShouldReturnNone()
            {
                result.IsNotFoundOrBadRequest().Should().Be(true);
            }
        }

        public class AndDataSomeWithoutWhk : WhenGetMongoWerkgever
        {
            public And Data => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.GetMongoWerkgever>(), CancellationToken.None)
                .Returns<IFluentResults<List<MongoWerkgeverModel>>>(e => ResultsTo.Success(
                    new List<MongoWerkgeverModel>
                    {
                        new()
                        {
                            Id = "633129fb572a926667ed8c5e".ToObjectId(),
                            Naam = "Test",
                            Sector = 0,
                            FiscaalNummer = "123456789L01",
                            LoonheffingenExtentie = "L01",
                            OmzetbelastingExtentie = "B01",
                            Actief = true,
                        },
                    }));

            [Fact]
            public void ThenShouldReturnSuccess()
            {
                result
                    .Value
                    .Any(f => f.FiscaalNummer == "123456789L01")
                    .Should()
                    .BeTrue();
            }

            [Fact]
            public void ThenShouldReturnSuccessWhk()
            {
                result
                    .Value
                    .FirstOrDefault(f => f.FiscaalNummer == "123456789L01")
                    .WhkPremies
                    .Should()
                    .BeNull();
            }
        }

        public class AndDataSomeWithWhk : WhenGetMongoWerkgever
        {
            public And Data => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.GetMongoWerkgever>(), CancellationToken.None)
                .Returns<IFluentResults<List<MongoWerkgeverModel>>>(e => ResultsTo.Success(
                    new List<MongoWerkgeverModel>
                    {
                        new()
                        {
                            Id = "633129fb572a926667ed8c5e".ToObjectId(),
                            Naam = "Test",
                            Sector = 0,
                            FiscaalNummer = "123456789L01",
                            LoonheffingenExtentie = "L01",
                            OmzetbelastingExtentie = "B01",
                            Actief = true,
                            WhkPremies = new List<MongoWhkPremie>
                            {
                                new()
                                {
                                    Id = "633129fb572a926667ed8c5f".ToObjectId(),
                                    WgaVastWerkgever = 0,
                                    WgaVastWerknemer = 0,
                                    FlexWerkgever = 0,
                                    FlexWerknemer = 0,
                                    ZwFlex = 0,
                                    Actief = true,
                                },
                            },
                        },
                    }));

            [Fact]
            public void ThenShouldReturnSuccess()
            {
                result
                    .Value
                    .FirstOrDefault(f => f.FiscaalNummer == "123456789L01")
                    .WhkPremies
                    .Any()
                    .Should()
                    .BeTrue();
            }
        }
    }
}
