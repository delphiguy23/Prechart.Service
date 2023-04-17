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
    public class WhenUpdateKlantWerkgever : WerkgeverServiceSpec
    {
        private IFluentResults<bool> result;

        public When UpdateKlantWerkgever => async () => result = await Subject.HandleAsync(new WerkgeverService.UpdateKlantWerkgever
        {
            KlantId = "633129fb572a926667ed8c5e",
            KlantName = "Test",
            TaxNo = "123456789L01",
        }, CancellationToken.None);

        public class AndException : WhenUpdateKlantWerkgever
        {
            public And Exception => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.UpdateKlantWerkgever>(), CancellationToken.None)
                .Returns<IFluentResults<bool>>(e => throw new Exception("exception"));

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

        public class AndSome : WhenUpdateKlantWerkgever
        {
            [Fact]
            public async Task ThenHandler()
            {
                await _repository.Received(1).HandleAsync(Verify.That<WerkgeverRepository.UpdateKlantWerkgever>(v => v.Should()
                    .BeEquivalentTo(new
                    {
                        KlantId = "633129fb572a926667ed8c5e",
                        KlantName = "Test",
                        TaxNo = "123456789L01",
                    })), CancellationToken.None);
            }

            public class AndTrue : AndSome
            {
                public And Data => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.UpdateKlantWerkgever>(), CancellationToken.None)
                    .Returns<IFluentResults<bool>>(e => ResultsTo.Success(true));

                [Fact]
                public void ThenShouldReturnSome()
                {
                    result.IsSuccess().Should().BeTrue();
                }
            }

            public class AndFalse : AndSome
            {
                public And Data => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.UpdateKlantWerkgever>(), CancellationToken.None)
                    .Returns<IFluentResults<bool>>(e => ResultsTo.Failure(false));

                [Fact]
                public void ThenShouldReturnSome()
                {
                    result.IsFailure().Should().BeTrue();
                }
            }
        }
    }
}
