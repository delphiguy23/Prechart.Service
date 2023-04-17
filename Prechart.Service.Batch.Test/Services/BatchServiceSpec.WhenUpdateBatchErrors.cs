using FluentAssertions;
using NSubstitute;
using Prechart.Service.Batch.Repositories;
using Prechart.Service.Batch.Services;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using Xunit;

namespace Prechart.Service.Klant.Test.Service;

public partial class BatchServiceSpec
{
    public class WhenUpdateBatchErrors : BatchServiceSpec
    {
        private IFluentResults<bool> result;

        public When UpdateProgress => async () => result = await Subject.HandleAsync(new BatchService.UpdateBatchErrors
        {
            BatchId = "123456789",
            Errors = new List<string>(),
        }, CancellationToken.None);

        public class AndException : WhenUpdateBatchErrors
        {
            public And Exception => () => _repository.HandleAsync(Arg.Any<BatchRepository.UpdateBatchErrors>(), CancellationToken.None)
                .Returns<IFluentResults<bool>>(e => throw new Exception("exception"));

            [Fact]
            public void ThenShouldReturnFailure()
            {
                result.IsFailure().Should().BeTrue();
            }

            [Fact]
            public void ThenShouldReturnException()
            {
                result.Messages.Containing("exception").Should().BeTrue();
            }

            [Fact]
            public void ThenLoggerStart()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
            }
        }

        public class AndFalse : WhenUpdateBatchErrors
        {
            public And False => () => _repository.HandleAsync(Arg.Any<BatchRepository.UpdateBatchErrors>(), CancellationToken.None)
                .Returns<IFluentResults<bool>>(ResultsTo.Something(false));

            [Fact]
            public void ThenSuccess()
            {
                result.IsSuccess().Should().BeTrue();
            }

            [Fact]
            public void ThenFalse()
            {
                result.Value.Should().BeFalse();
            }
        }

        public class AndTrue : WhenUpdateBatchErrors
        {
            public And False => () => _repository.HandleAsync(Arg.Any<BatchRepository.UpdateBatchErrors>(), CancellationToken.None)
                .Returns<IFluentResults<bool>>(ResultsTo.Something(true));

            [Fact]
            public void ThenSuccess()
            {
                result.IsSuccess().Should().BeTrue();
            }

            [Fact]
            public void ThenTrue()
            {
                result.Value.Should().BeTrue();
            }
        }
    }
}
