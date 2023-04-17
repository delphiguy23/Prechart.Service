using FluentAssertions;
using NSubstitute;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Interfaces.Werkgever;
using Prechart.Service.Globals.Models.Klant;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Klant.Repository;
using Prechart.Service.Klant.Service;
using Xunit;

namespace Prechart.Service.Klant.Test.Service;

public partial class KlantServiceSpec
{
    public class WhenUpsertKlants : KlantServiceSpec
    {
        private List<KlantModel> _klants;
        private IFluentResults<List<KlantModel>> result;

        public When UpsertKlants => async () => result = await Subject.HandleAsync(new KlantService.UpsertKlants
        {
            Klants = _klants,
        }, CancellationToken.None);

        public class AndEmptyRequests : WhenUpsertKlants
        {
            public And GetData => () => { _klants = _klantEmpty; };

            [Fact]
            public void ThenShouldReturnNone()
            {
                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }

            [Fact]
            public void ThenLoggerStart()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
            }
        }

        public class AndNullRequests : WhenUpsertKlants
        {
            public And GetData => () => { _klants = null; };

            [Fact]
            public void ThenShouldReturnNone()
            {
                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }

            [Fact]
            public void ThenLoggerStart()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
            }
        }

        public class AndException : WhenUpsertKlants
        {
            public And SetData => () => { _klants = _klantModels; };

            public And GetData => () => _repository.HandleAsync(Arg.Any<KlantRepository.UpsertKlants>(), CancellationToken.None)
                .Returns<IFluentResults<List<KlantModel>>>(e => throw new Exception("exception"));

            [Fact]
            public void ThenShouldReturnFailure()
            {
                result.IsFailure().Should().BeTrue();
            }

            [Fact]
            public void ThenShouldReturnException()
            {
                result.Messages.Any(_ => _.Contains("exception")).Should().BeTrue();
            }

            [Fact]
            public void ThenLoggerError()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
            }

            [Fact]
            public void ThenLoggerStart()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
            }
        }

        public class AndNothingUpserted : WhenUpsertKlants
        {
            public And SetData => () => { _klants = _klantModels; };

            public And GetData => () => _repository.HandleAsync(Arg.Any<KlantRepository.UpsertKlants>(), CancellationToken.None)
                .Returns<IFluentResults<List<KlantModel>>>(e => ResultsTo.NotFound<List<KlantModel>>());

            [Fact]
            public void ThenShouldReturnNone()
            {
                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }

            [Fact]
            public void ThenLoggerStart()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
            }
        }

        public class AndUpserted : WhenUpsertKlants
        {
            public And SetData => () => { _klants = _klantModels; };

            public And GetData => () => _repository.HandleAsync(Arg.Any<KlantRepository.UpsertKlants>(), CancellationToken.None)
                .Returns<IFluentResults<List<KlantModel>>>(e => ResultsTo.Something(_klantModels));

            [Fact]
            public void ThenShouldReturnSuccess()
            {
                result.IsSuccess().Should().BeTrue();
            }

            [Fact]
            public void ThenShouldReturnSome()
            {
                result.Value.FirstOrDefault().Id.Should().Be("633129fb572a926667ed8c5d".ToObjectId());
            }

            [Fact]
            public void ThenLoggerStart()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
            }

            [Fact]
            public async Task ThenPublishEvent()
            {
                await _publish.Received().Publish<IUpsertWerkgever>(
                    Verify.That<object>(o => o.Should().BeEquivalentTo(new UpsertWerkgeverModel
                    {
                        KlantId = "633129fb572a926667ed8c5d",
                        KlantName = "Test Klant",
                        TaxNo = "123456789L01",
                    })));
            }
        }
    }
}
