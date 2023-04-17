using FluentAssertions;
using NSubstitute;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Werkgever.Database.Models;
using Prechart.Service.Werkgever.Repository;
using Prechart.Service.Werkgever.Service;
using Xunit;

namespace Prechart.Service.Werkgever.Test.Service;

public partial class WerkgeverServiceSpec
{
    public class WhenSyncFromSql : WerkgeverServiceSpec
    {
        private readonly List<MongoWerkgeverModel> werkgevers = null;
        private IFluentResults<bool> result;

        public When SyncFromSql => async () => result = await Subject.HandleAsync(new WerkgeverService.SyncFromSql(), CancellationToken.None);

        public class AndException : WhenSyncFromSql
        {
            public And Exception => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.GetSqlWerkgevers>(), CancellationToken.None)
                .Returns<IFluentResults<List<Database.Models.Werkgever>>>(e => throw new Exception("exception"));

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

        public class AndFailure : WhenSyncFromSql
        {
            public And Exception => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.GetSqlWerkgevers>(), CancellationToken.None)
                .Returns<IFluentResults<List<Database.Models.Werkgever>>>(e => ResultsTo.Failure<List<Database.Models.Werkgever>>("failure"));

            [Fact]
            public void ThenShouldReturnSome()
            {
                result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndNoData : WhenSyncFromSql
        {
            public And Data => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.GetSqlWerkgevers>(), CancellationToken.None)
                .Returns<IFluentResults<List<Database.Models.Werkgever>>>(e => ResultsTo.NotFound<List<Database.Models.Werkgever>>());

            [Fact]
            public void ThenShouldReturnNone()
            {
                result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndHasDataNoWhk : WhenSyncFromSql
        {
            public And Data => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.GetSqlWerkgevers>(), CancellationToken.None)
                .Returns<IFluentResults<List<Database.Models.Werkgever>>>(e => ResultsTo.Success(
                    new List<Database.Models.Werkgever>
                    {
                        new()
                        {
                            Id = 1,
                            WerkgeverMongoId = "633129fb572a926667ed8c5e",
                            KlantMongoId = "633129fb572a926667ed8c51",
                            Naam = "Test",
                            Sector = 0,
                            FiscaalNummer = "123456789L01",
                            LoonheffingenExtentie = "L01",
                            OmzetbelastingExtentie = "B01",
                            Actief = true,
                        },
                    }));

            [Fact]
            public void ThenShouldReturnSome()
            {
                result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndHasDataWithWhk : WhenSyncFromSql
        {
            public And DataWerkgever => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.GetSqlWerkgevers>(), CancellationToken.None)
                .Returns<IFluentResults<List<Database.Models.Werkgever>>>(e => ResultsTo.Success(
                    new List<Database.Models.Werkgever>
                    {
                        new()
                        {
                            Id = 1,
                            WerkgeverMongoId = "633129fb572a926667ed8c5e",
                            KlantMongoId = "633129fb572a926667ed8c51",
                            Naam = "Test",
                            Sector = 0,
                            FiscaalNummer = "123456789L01",
                            LoonheffingenExtentie = "L01",
                            OmzetbelastingExtentie = "B01",
                            Actief = true,
                        },
                    }));

            public And DataWerkgeverWhk => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.GetSqlWerkgeversWhkPremies>(), CancellationToken.None)
                .Returns<IFluentResults<List<WerkgeverWhkPremies>>>(e => ResultsTo.Success(
                    new List<WerkgeverWhkPremies>
                    {
                        new()
                        {
                            Id = 1,
                            WerkgeverId = 1,
                            WerkgeverWhkMongoId = "633129fb572a926667ed8c5e",
                            WgaVastWerkgever = 0,
                            WgaVastWerknemer = 0,
                            FlexWerkgever = 0,
                            FlexWerknemer = 0,
                            ZwFlex = 0,
                            Totaal = 0,
                            Actief = true,
                        },
                    }));

            [Fact]
            public void ThenShouldReturnSome()
            {
                result.IsSuccess().Should().BeTrue();
            }
        }
    }
}
