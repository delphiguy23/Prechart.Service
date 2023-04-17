using FluentAssertions;
using MongoDB.Driver;
using Moq;
using Prechart.Service.Batch.Repositories;
using Prechart.Service.Globals.Models.Batch;
using Xunit;

namespace Prechart.Service.Batch.Test.Repository;

public partial class BatchRepositorySpec
{
    public class WhenCreateBatchRecord : BatchRepositorySpec
    {
        public class AndException : WhenCreateBatchRecord
        {
            [Fact]
            public async void ThenException()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.InsertOneAsync(
                        It.IsAny<BatchProcess>(),
                        It.IsAny<InsertOneOptions>(),
                        It.IsAny<CancellationToken>()))
                    .Callback(() => throw new Exception("exception"));

                var result = await Subject.HandleAsync(new BatchRepository.CreateBatchRecord
                {
                    BatchRecord = _batchRecord,
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.InsertOneAsync(_batchRecord, null, default), Times.AtMostOnce);
            }
        }

        public class AndNullBatchRecords : WhenCreateBatchRecord
        {
            [Fact]
            public async void ThenFalse()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.InsertOneAsync(
                        It.IsAny<BatchProcess>(),
                        It.IsAny<InsertOneOptions>(),
                        It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult((object) null));

                var result = await Subject.HandleAsync(new BatchRepository.CreateBatchRecord
                {
                    BatchRecord = null,
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.InsertOneAsync(null, null, default), Times.Never);
                result.Value.Should().BeFalse();
            }
        }

        public class AndWithBatchRecords : WhenCreateBatchRecord
        {
            [Fact]
            public async void ThenTrue()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.InsertOneAsync(
                        It.IsAny<BatchProcess>(),
                        It.IsAny<InsertOneOptions>(),
                        It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult((object) null));

                var result = await Subject.HandleAsync(new BatchRepository.CreateBatchRecord
                {
                    BatchRecord = _batchRecord,
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.InsertOneAsync(_batchRecord, null, default), Times.AtMostOnce);
                result.Value.Should().BeTrue();
            }
        }
    }
}
