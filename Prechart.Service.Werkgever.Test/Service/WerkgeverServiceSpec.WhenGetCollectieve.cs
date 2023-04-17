using FluentAssertions;
using NSubstitute;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Werkgever.Repository;
using Prechart.Service.Werkgever.Service;
using Xunit;

namespace Prechart.Service.Werkgever.Test.Service;

public partial class WerkgeverServiceSpec
{
    public class WhenGetCollectieve : WerkgeverServiceSpec
    {
        private IFluentResults<List<CollectieveAangifteModel>> result;
        private string taxNo = "123456789L01";

        public When SyncFromSql => async () => result = await Subject.HandleAsync(new WerkgeverService.GetCollectieve
        {
            TaxNo = taxNo,
        }, CancellationToken.None);

        public class AndException : WhenGetCollectieve
        {
            public And Exception => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.GetCollectieve>(), CancellationToken.None)
                .Returns<IFluentResults<List<CollectieveAangifteModel>>>(e => throw new Exception("exception"));

            [Fact]
            public void ThenShouldReturnSuccess()
            {
                result.Status.Should().Be(FluentResultsStatus.Failure);
            }

            [Fact]
            public void ThenLoggerError()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
            }
        }

        public class AndNoTaxNo : WhenGetCollectieve
        {
            public Given TaxNo => () => taxNo = string.Empty;

            [Fact]
            public void ThenShouldReturnSome()
            {
                result.IsNotFoundOrBadRequest().Should().Be(true);
            }
        }

        public class AndNoDataFound : WhenGetCollectieve
        {
            public And NoData => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.GetCollectieve>(), CancellationToken.None)
                .Returns<IFluentResults<List<CollectieveAangifteModel>>>(e => ResultsTo.NotFound<List<CollectieveAangifteModel>>());

            [Fact]
            public void ThenShouldReturnNone()
            {
                result.Status.Should().Be(FluentResultsStatus.NotFound);
            }
        }

        public class AndDataFound : WhenGetCollectieve
        {
            public And Data => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.GetCollectieve>(), CancellationToken.None)
                .Returns<IFluentResults<List<CollectieveAangifteModel>>>(e => ResultsTo.Success(new List<CollectieveAangifteModel>
                {
                    new()
                    {
                        TaxNo = taxNo,
                        Periode = $"{DateTime.Now.Year}-{DateTime.Now.Month - 1:d2}",
                        ProcessedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                        CollectieveType = CollectieveType.Normaal,
                        TotLnLbPh = 0,
                        TotLnSv = 0,
                        TotPrlnAofAnwLg = 0,
                        TotPrlnAofAnwHg = 0,
                        TotPrlnAofAnwUit = 0,
                        TotPrlnAwfAnwLg = 0,
                        TotPrlnAwfAnwHg = 0,
                        TotPrlnAwfAnwHz = 0,
                        PrLnUfo = 0,
                        IngLbPh = 0,
                        EhPubUitk = 0,
                        EhGebrAuto = 0,
                        EhVut = 0,
                        EhOvsFrfWrkkstrg = 0,
                        AvZeev = 0,
                        VrlAvso = 0,
                        TotPrAofLg = 0,
                        TotPrAofHg = 0,
                        TotPrAofUit = 0,
                        TotOpslWko = 0,
                        TotPrGediffWhk = 0,
                        TotPrAwfLg = 0,
                        TotPrAwfHg = 0,
                        TotPrAwfHz = 0,
                        PrUfo = 0,
                        IngBijdrZvw = 0,
                        TotWghZvw = 0,
                        TotTeBet = 0,
                        TotGen = 0,
                        SaldoCorrectiesVoorgaandTijdvak = new SaldoCorrectiesVoorgaandTijdvakModel[]
                        {
                        },
                    },
                }));

            [Fact]
            public void ThenShouldReturnSome()
            {
                result.Status.Should().Be(FluentResultsStatus.Success);
            }

            [Fact]
            public void ThenShouldReturnCollectieve()
            {
                result.Value
                    .Where(c => c.CollectieveType == CollectieveType.Normaal)
                    .Where(c => c.TaxNo == taxNo)
                    .Where(c => c.Periode == $"{DateTime.Now.Year}-{DateTime.Now.Month - 1:d2}")
                    .Any()
                    .Should().BeTrue();
            }
        }
    }
}
