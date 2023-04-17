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
    public class WhenUpdateProgress : BatchRepositorySpec
    {
        public class AndException : WhenUpdateProgress
        {
            [Fact]
            public async void ThenException()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.UpdateOneAsync(
                        It.IsAny<FilterDefinition<BatchProcess>>(),
                        It.IsAny<UpdateDefinition<BatchProcess>>(),
                        It.IsAny<UpdateOptions>(),
                        It.IsAny<CancellationToken>())).Callback(() => throw new Exception("exception"));

                var result = await Subject.HandleAsync(new BatchRepository.UpdateProgress
                {
                    BatchId = "6361bf258d18514754851449",
                    CompletedTask = 1,
                    TotalTask = 1,
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.UpdateOneAsync(
                    It.IsAny<FilterDefinition<BatchProcess>>(),
                    It.IsAny<UpdateDefinition<BatchProcess>>(),
                    It.IsAny<UpdateOptions>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error).Should().BeTrue();
                result.IsFailure().Should().BeTrue();
                result.Messages.Containing("exception").Should().BeTrue();
            }
        }

        public class AndUpdated : WhenUpdateProgress
        {
            [Fact]
            public async void ThenTrue()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.UpdateOneAsync(
                        It.IsAny<FilterDefinition<BatchProcess>>(),
                        It.IsAny<UpdateDefinition<BatchProcess>>(),
                        It.IsAny<UpdateOptions>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new UpdateResult.Acknowledged(1, 1, null));

                var result = await Subject.HandleAsync(new BatchRepository.UpdateProgress
                {
                    BatchId = "6361bf258d18514754851449",
                    CompletedTask = 1,
                    TotalTask = 1,
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.UpdateOneAsync(
                    It.IsAny<FilterDefinition<BatchProcess>>(),
                    It.IsAny<UpdateDefinition<BatchProcess>>(),
                    It.IsAny<UpdateOptions>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsSuccess().Should().BeTrue();
                result.Value.Should().BeTrue();
            }
        }
    }
}
