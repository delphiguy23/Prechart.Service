using FluentAssertions;
using NSubstitute;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Models.Klant;
using Prechart.Service.Klant.Repository;
using Prechart.Service.Klant.Service;
using Xunit;

namespace Prechart.Service.Klant.Test.Service;

public partial class KlantServiceSpec
{
    public class WhenGetKlant : KlantServiceSpec
    {
        private string _klantId = "633129fb572a926667ed8c5d";
        private string _klantName = "Test Klant";
        private string _taxNo = "123456789L01";
        private IFluentResults<KlantModel> result;

        public When GetKlants => async () => result = await Subject.HandleAsync(new KlantService.GetKlant
        {
            KlantId = _klantId,
            KlantName = _klantName,
            TaxNo = _taxNo,
        }, CancellationToken.None);

        public class AndEmptyRequests : WhenGetKlant
        {
            public And GetData => () =>
            {
                _klantId = string.Empty;
                _klantName = string.Empty;
                _taxNo = string.Empty;
            };

            [Fact]
            public void ThenShouldReturnException()
            {
                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }

            [Fact]
            public void ThenLoggerStart()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
            }
        }

        public class AndException : WhenGetKlant
        {
            public And GetData => () => _repository.HandleAsync(Arg.Any<KlantRepository.GetKlantByName>(), CancellationToken.None)
                .Returns<IFluentResults<KlantModel>>(e => throw new Exception("exception"));

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

        public class AndKlantByNameFound : WhenGetKlant
        {
            public And GetData => () =>
            {
                _klantId = string.Empty;
                _taxNo = string.Empty;

                _repository.HandleAsync(Arg.Any<KlantRepository.GetKlantByName>(), CancellationToken.None).Returns<IFluentResults<KlantModel>>(e => ResultsTo.Something(_klantModels.FirstOrDefault()));
            };

            [Fact]
            public void ThenShouldReturnSuccess()
            {
                result.IsSuccess().Should().BeTrue();
            }

            [Fact]
            public void ThenShouldReturnSome()
            {
                result.Value.KlantNaam.Should().Be(_klantName);
            }

            [Fact]
            public void ThenLoggerStart()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
            }
        }

        public class AndKlantByNameNotFound : WhenGetKlant
        {
            public And GetData => () =>
            {
                _klantId = string.Empty;
                _taxNo = string.Empty;

                _repository.HandleAsync(Arg.Any<KlantRepository.GetKlantByName>(), CancellationToken.None).Returns<IFluentResults<KlantModel>>(e => ResultsTo.NotFound<KlantModel>());
            };

            [Fact]
            public void ThenShouldReturnSome()
            {
                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }

            [Fact]
            public void ThenLoggerStart()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
            }
        }

        public class AndKlantByIdFound : WhenGetKlant
        {
            public And GetData => () =>
            {
                _klantName = string.Empty;
                _taxNo = string.Empty;

                _repository.HandleAsync(Arg.Any<KlantRepository.GetKlantById>(), CancellationToken.None).Returns<IFluentResults<KlantModel>>(e => ResultsTo.Success<KlantModel>(_klantModels.FirstOrDefault()));
            };

            [Fact]
            public void ThenShouldReturnSuccess()
            {
                result.IsSuccess().Should().BeTrue();
            }

            [Fact]
            public void ThenShouldReturnSome()
            {
                result.Value.Id.Should().Be(_klantId.ToObjectId());
            }

            [Fact]
            public void ThenLoggerStart()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
            }
        }

        public class AndKlantByIdNotFound : WhenGetKlant
        {
            public And GetData => () =>
            {
                _klantName = string.Empty;
                _taxNo = string.Empty;

                _repository.HandleAsync(Arg.Any<KlantRepository.GetKlantById>(), CancellationToken.None).Returns<IFluentResults<KlantModel>>(e => ResultsTo.NotFound<KlantModel>());
            };

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

        public class AndKlantByTaxNoFound : WhenGetKlant
        {
            public And GetData => () =>
            {
                _klantName = string.Empty;
                _klantId = string.Empty;

                _repository.HandleAsync(Arg.Any<KlantRepository.GetKlantByTaxNo>(), CancellationToken.None).Returns<IFluentResults<KlantModel>>(e => ResultsTo.Success<KlantModel>(_klantModels.FirstOrDefault()));
            };

            [Fact]
            public void ThenShouldReturnSuccess()
            {
                result.IsSuccess().Should().BeTrue();
            }

            [Fact]
            public void ThenShouldReturnSome()
            {
                result.Value.Werkgevers.FirstOrDefault(w => w == _taxNo).Should().Be(_taxNo);
            }

            [Fact]
            public void ThenLoggerStart()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
            }
        }

        public class AndKlantByTaxNoNotFound : WhenGetKlant
        {
            public And GetData => () =>
            {
                _klantName = string.Empty;
                _klantId = string.Empty;

                _repository.HandleAsync(Arg.Any<KlantRepository.GetKlantByTaxNo>(), CancellationToken.None).Returns<IFluentResults<KlantModel>>(e => ResultsTo.NotFound<KlantModel>());
            };

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

        public class AndNoneFound : WhenGetKlant
        {
            public And GetParams => () =>
            {
                _klantName = string.Empty;
                _klantId = string.Empty;
            };

            public And GetDataByTax => () => { _repository.HandleAsync(Arg.Any<KlantRepository.GetKlantByTaxNo>(), CancellationToken.None).Returns<IFluentResults<KlantModel>>(e => ResultsTo.NotFound<KlantModel>()); };

            public And GetDataById => () => { _repository.HandleAsync(Arg.Any<KlantRepository.GetKlantById>(), CancellationToken.None).Returns<IFluentResults<KlantModel>>(e => ResultsTo.NotFound<KlantModel>()); };

            public And GetDataByName => () => { _repository.HandleAsync(Arg.Any<KlantRepository.GetKlantByName>(), CancellationToken.None).Returns<IFluentResults<KlantModel>>(e => ResultsTo.NotFound<KlantModel>()); };

            [Fact]
            public void ThenShouldReturnNone()
            {
                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }

            [Fact]
            public void ThenShouldReturnSkipped()
            {
                result.Messages.Any(_ => _.Contains("Skipped")).Should().BeTrue();
            }

            [Fact]
            public void ThenLoggerStart()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
            }
        }
    }
}
