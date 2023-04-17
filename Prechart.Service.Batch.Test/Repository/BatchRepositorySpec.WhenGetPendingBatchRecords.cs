using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Prechart.Service.Batch.Repositories;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Globals.Models.Batch;
using Xunit;

namespace Prechart.Service.Batch.Test.Repository;

public partial class BatchRepositorySpec
{
    public class GetPendingBatchRecords : BatchRepositorySpec
    {
        public class AndException : GetPendingBatchRecords
        {
            [Fact]
            public async void ThenException()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<BatchProcess>>(),
                        It.IsAny<FindOptions<BatchProcess>>(),
                        It.IsAny<CancellationToken>()))
                    .Callback(() => throw new Exception("exception"));

                var result = await Subject.HandleAsync(new BatchRepository.GetPendingBatchRecords
                {
                    BatchName = string.Empty,
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<BatchProcess>>(),
                    It.IsAny<FindOptions<BatchProcess>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error).Should().BeTrue();
                result.IsFailure().Should().BeTrue();
                result.Messages.Containing("exception").Should().BeTrue();
            }
        }

        public class AndNoBatchName : GetPendingBatchRecords
        {
            [Fact]
            public async void ThenSome()
            {
                //Arrange
                List<BatchProcess> batchProcesses = new()
                {
                    new BatchProcess
                    {
                        Id = ObjectId.GenerateNewId(),
                        BatchName = "TestBatch1",
                        TotalTask = 1,
                        Status = BatchProcessStatus.ReadyForProcessing,
                        PublishedOn = new DateTime(),
                        Payload = string.Empty,
                    },
                    new BatchProcess
                    {
                        Id = ObjectId.GenerateNewId(),
                        BatchName = "TestBatch2",
                        TotalTask = 1,
                        Status = BatchProcessStatus.ReadyForProcessing,
                        PublishedOn = new DateTime(),
                        Payload = string.Empty,
                    },
                };

                var mockAsyncCursorBatchProcessModel = new Mock<IAsyncCursor<BatchProcess>>();

                mockAsyncCursorBatchProcessModel.Setup(a => a.Current).Returns(batchProcesses);
                mockAsyncCursorBatchProcessModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                mockAsyncCursorBatchProcessModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<BatchProcess>>(),
                        It.IsAny<FindOptions<BatchProcess>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(mockAsyncCursorBatchProcessModel.Object);

                var result = await Subject.HandleAsync(new BatchRepository.GetPendingBatchRecords
                {
                    BatchName = string.Empty,
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<BatchProcess>>(),
                    It.IsAny<FindOptions<BatchProcess>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsSuccess().Should().BeTrue();
                result.Value.Count.Should().Be(2);
            }
        }

        public class AndWithBatchName : GetPendingBatchRecords
        {
            [Fact]
            public async void ThenSome()
            {
                //Arrange
                List<BatchProcess> batchProcesses = new()
                {
                    new BatchProcess
                    {
                        Id = ObjectId.GenerateNewId(),
                        BatchName = "TestBatch1",
                        TotalTask = 1,
                        Status = BatchProcessStatus.ReadyForProcessing,
                        PublishedOn = new DateTime(),
                        Payload = string.Empty,
                    },
                };

                var mockAsyncCursorBatchProcessModel = new Mock<IAsyncCursor<BatchProcess>>();

                mockAsyncCursorBatchProcessModel.Setup(a => a.Current).Returns(batchProcesses);
                mockAsyncCursorBatchProcessModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                mockAsyncCursorBatchProcessModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<BatchProcess>>(),
                        It.IsAny<FindOptions<BatchProcess>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(mockAsyncCursorBatchProcessModel.Object);

                var result = await Subject.HandleAsync(new BatchRepository.GetPendingBatchRecords
                {
                    BatchName = "TestBatch1",
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<BatchProcess>>(),
                    It.IsAny<FindOptions<BatchProcess>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsSuccess().Should().BeTrue();
                result.Value.Any().Should().BeTrue();
                result.Value.FirstOrDefault().BatchName.Should().Be("TestBatch1");
            }
        }

        public class AndWithBatchNameNoData : GetPendingBatchRecords
        {
            [Fact]
            public async void ThenSome()
            {
                //Arrange
                List<BatchProcess> batchProcesses = new();

                var mockAsyncCursorBatchProcessModel = new Mock<IAsyncCursor<BatchProcess>>();

                mockAsyncCursorBatchProcessModel.Setup(a => a.Current).Returns(batchProcesses);
                mockAsyncCursorBatchProcessModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                mockAsyncCursorBatchProcessModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<BatchProcess>>(),
                        It.IsAny<FindOptions<BatchProcess>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(mockAsyncCursorBatchProcessModel.Object);

                var result = await Subject.HandleAsync(new BatchRepository.GetPendingBatchRecords
                {
                    BatchName = "TestBatch1",
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<BatchProcess>>(),
                    It.IsAny<FindOptions<BatchProcess>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsSuccess().Should().BeTrue();
                result.Value.Any().Should().BeFalse();
            }
        }
    }
}
