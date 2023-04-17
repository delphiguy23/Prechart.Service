using FluentAssertions;
using NSubstitute;
using Prechart.Service.Batch.Repositories;
using Prechart.Service.Batch.Services;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Models.Batch;
using Xunit;

namespace Prechart.Service.Klant.Test.Service;

public partial class BatchServiceSpec
{
    public class WhenGetPendingBatchRecords : BatchServiceSpec
    {
        private IFluentResults<bool> result;

        public When GetPendingBatchRecords => async () => result = await Subject.HandleAsync(new BatchService.GetPendingBatchRecords
        {
            BatchName = "TestBatch",
        }, CancellationToken.None);

        public class AndException : WhenGetPendingBatchRecords
        {
            public And Exception => () => _repository.HandleAsync(Arg.Any<BatchRepository.GetPendingBatchRecords>(), CancellationToken.None)
                .Returns<IFluentResults<List<BatchProcess>>>(e => throw new Exception("exception"));

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

        public class AndFalse : WhenGetPendingBatchRecords
        {
            public And False => () => _repository.HandleAsync(Arg.Any<BatchRepository.GetPendingBatchRecords>(), CancellationToken.None)
                .Returns<IFluentResults<List<BatchProcess>>>(ResultsTo.Something(new List<BatchProcess>()));

            [Fact]
            public void ThenNotFound()
            {
                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }
        }

        public class AndTrue : WhenGetPendingBatchRecords
        {
            public And False => () => _repository.HandleAsync(Arg.Any<BatchRepository.GetPendingBatchRecords>(), CancellationToken.None)
                .Returns<IFluentResults<List<BatchProcess>>>(ResultsTo.Something(new List<BatchProcess> {_batchRecord}));

            [Fact]
            public void ThenSuccess()
            {
                result.IsSuccess().Should().BeTrue();
            }
        }
    }
}
