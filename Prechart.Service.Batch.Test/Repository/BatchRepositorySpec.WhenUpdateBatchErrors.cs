using FluentAssertions;
using MongoDB.Driver;
using Moq;
using Prechart.Service.Batch.Repositories;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Globals.Models.Batch;
using Xunit;

namespace Prechart.Service.Batch.Test.Repository;

public partial class BatchRepositorySpec
{
    public class WhenUpdateBatchErrors : BatchRepositorySpec
    {
        public class AndException : WhenUpdateStatus
        {
            [Fact]
            public async void ThenException()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.FindOneAndUpdateAsync(
                        It.IsAny<FilterDefinition<BatchProcess>>(),
                        It.IsAny<UpdateDefinition<BatchProcess>>(),
                        It.IsAny<FindOneAndUpdateOptions<BatchProcess, BatchProcess>>(),
                        It.IsAny<CancellationToken>())).Callback(() => throw new Exception("exception"));

                var result = await Subject.HandleAsync(new BatchRepository.UpdateBatchErrors
                {
                    BatchId = "6361bf258d18514754851449",
                    Errors = new List<string> {"error1", "error2"},
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindOneAndUpdateAsync(
                    It.IsAny<FilterDefinition<BatchProcess>>(),
                    It.IsAny<UpdateDefinition<BatchProcess>>(),
                    It.IsAny<FindOneAndUpdateOptions<BatchProcess, BatchProcess>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error).Should().BeTrue();
                result.IsFailure().Should().BeTrue();
                result.Messages.Containing("exception").Should().BeTrue();
            }
        }

        public class NullErrors : WhenUpdateStatus
        {
            [Fact]
            public async void ThenTrue()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.FindOneAndUpdateAsync(
                        It.IsAny<FilterDefinition<BatchProcess>>(),
                        It.IsAny<UpdateDefinition<BatchProcess>>(),
                        It.IsAny<FindOneAndUpdateOptions<BatchProcess, BatchProcess>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_batchRecord);

                var result = await Subject.HandleAsync(new BatchRepository.UpdateBatchErrors
                {
                    BatchId = "6361bf258d18514754851449",
                    Errors = null,
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindOneAndUpdateAsync(
                    It.IsAny<FilterDefinition<BatchProcess>>(),
                    It.IsAny<UpdateDefinition<BatchProcess>>(),
                    It.IsAny<FindOneAndUpdateOptions<BatchProcess, BatchProcess>>(),
                    It.IsAny<CancellationToken>()), Times.Never);

                result.IsSuccess().Should().BeTrue();
                result.Value.Should().BeFalse();
            }
        }

        public class EmptyErrors : WhenUpdateStatus
        {
            [Fact]
            public async void ThenTrue()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.FindOneAndUpdateAsync(
                        It.IsAny<FilterDefinition<BatchProcess>>(),
                        It.IsAny<UpdateDefinition<BatchProcess>>(),
                        It.IsAny<FindOneAndUpdateOptions<BatchProcess, BatchProcess>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_batchRecord);

                var result = await Subject.HandleAsync(new BatchRepository.UpdateBatchErrors
                {
                    BatchId = "6361bf258d18514754851449",
                    Errors = new List<string>(),
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindOneAndUpdateAsync(
                    It.IsAny<FilterDefinition<BatchProcess>>(),
                    It.IsAny<UpdateDefinition<BatchProcess>>(),
                    It.IsAny<FindOneAndUpdateOptions<BatchProcess, BatchProcess>>(),
                    It.IsAny<CancellationToken>()), Times.Never);

                result.IsSuccess().Should().BeTrue();
                result.Value.Should().BeFalse();
            }
        }

        public class WithErrors : WhenUpdateStatus
        {
            [Fact]
            public async void ThenTrue()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.FindOneAndUpdateAsync(
                        It.IsAny<FilterDefinition<BatchProcess>>(),
                        It.IsAny<UpdateDefinition<BatchProcess>>(),
                        It.IsAny<FindOneAndUpdateOptions<BatchProcess, BatchProcess>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_batchRecord);

                var result = await Subject.HandleAsync(new BatchRepository.UpdateBatchErrors
                {
                    BatchId = "6361bf258d18514754851449",
                    Errors = new List<string> {"error1", "error2"},
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindOneAndUpdateAsync(
                    It.IsAny<FilterDefinition<BatchProcess>>(),
                    It.IsAny<UpdateDefinition<BatchProcess>>(),
                    It.IsAny<FindOneAndUpdateOptions<BatchProcess, BatchProcess>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsSuccess().Should().BeTrue();
                result.Value.Should().BeTrue();
            }
        }
    }
}
