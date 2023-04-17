using FluentAssertions;
using MongoDB.Driver;
using Moq;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Globals.Models.Klant;
using Prechart.Service.Klant.Repository;
using Xunit;

namespace Prechart.Service.Klant.Test.Repository;

public partial class KlantRepositorySpec
{
    public class WhenUpsertKlants : KlantRepositorySpec
    {
        public class AndException : WhenUpsertKlants
        {
            [Fact]
            public async void ThenException()
            {
                //Arrange
                _collection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<KlantModel>>(),
                        It.IsAny<FindOptions<KlantModel>>(),
                        It.IsAny<CancellationToken>()))
                    .Callback(() => throw new Exception("exception"));

                //Act
                var result = await Subject.HandleAsync(new KlantRepository.UpsertKlants {Klants = _klantModels}, CancellationToken.None);

                //Assert
                _collection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<KlantModel>>(),
                    It.IsAny<FindOptions<KlantModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Upserting")).Should().BeTrue();
                result.IsFailure().Should().BeTrue();
                result.Messages.Containing("exception").Should().BeTrue();
            }
        }

        public class AndNoDataFound : WhenUpsertKlants
        {
            [Fact]
            public async void ThenInsertData()
            {
                //Arrange
                _mockAsyncCursorKlantModel.Setup(a => a.Current).Returns(_klantModelsEmpty);
                _mockAsyncCursorKlantModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                _mockAsyncCursorKlantModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _collection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<KlantModel>>(),
                        It.IsAny<FindOptions<KlantModel, KlantModel>>(),
                        It.IsAny<CancellationToken>())).ReturnsAsync(_mockAsyncCursorKlantModel.Object);

                _collection
                    .Setup(op => op.InsertOneAsync(
                        It.IsAny<KlantModel>(),
                        It.IsAny<InsertOneOptions>(),
                        It.IsAny<CancellationToken>())).Returns(Task.FromResult((object) null));

                //Act
                var result = await Subject.HandleAsync(new KlantRepository.UpsertKlants {Klants = _klantModels}, CancellationToken.None);

                //Assert
                _collection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<KlantModel>>(),
                    It.IsAny<FindOptions<KlantModel, KlantModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _collection.Verify(c => c.InsertOneAsync(
                    It.IsAny<KlantModel>(),
                    It.IsAny<InsertOneOptions>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
                result.IsSuccess().Should().BeTrue();
                result.Value.FirstOrDefault().KlantNaam.Should().Be("Test Klant");
            }
        }

        public class AndDataFound : WhenUpsertKlants
        {
            [Fact]
            public async void ThenUpdateData()
            {
                //Arrange
                _mockUpdateResult.Setup(_ => _.IsAcknowledged).Returns(true);
                _mockUpdateResult.Setup(_ => _.ModifiedCount).Returns(1);

                _mockAsyncCursorKlantModel.Setup(a => a.Current).Returns(_klantModels);
                _mockAsyncCursorKlantModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                _mockAsyncCursorKlantModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _collection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<KlantModel>>(),
                        It.IsAny<FindOptions<KlantModel, KlantModel>>(),
                        It.IsAny<CancellationToken>())).ReturnsAsync(_mockAsyncCursorKlantModel.Object);

                _collection
                    .Setup(op => op.FindOneAndUpdateAsync(
                        It.IsAny<FilterDefinition<KlantModel>>(),
                        It.IsAny<UpdateDefinition<KlantModel>>(),
                        It.IsAny<FindOneAndUpdateOptions<KlantModel, KlantModel>>(),
                        It.IsAny<CancellationToken>())).ReturnsAsync(_klantModelsForUpsert.FirstOrDefault());

                _collection
                    .Setup(op => op.UpdateOneAsync(
                        It.IsAny<FilterDefinition<KlantModel>>(),
                        It.IsAny<UpdateDefinition<KlantModel>>(),
                        It.IsAny<UpdateOptions>(),
                        It.IsAny<CancellationToken>())).ReturnsAsync(_mockUpdateResult.Object);
                //Act
                var result = await Subject.HandleAsync(new KlantRepository.UpsertKlants {Klants = _klantModelsForUpsert}, CancellationToken.None);

                //Assert
                _collection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<KlantModel>>(),
                    It.IsAny<FindOptions<KlantModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
                result.IsSuccess().Should().BeTrue();
                result.Value.FirstOrDefault().Id.Should().Be("633129fb572a926667ed8c5d".ToObjectId());
            }
        }
    }
}
