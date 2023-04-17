using FluentAssertions;
using MongoDB.Driver;
using Moq;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Models.Klant;
using Prechart.Service.Klant.Repository;
using Xunit;

namespace Prechart.Service.Klant.Test.Repository;

public partial class KlantRepositorySpec
{
    public class WhenGetKlants : KlantRepositorySpec
    {
        public class AndException : WhenGetKlants
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
                var result = await Subject.HandleAsync(new KlantRepository.GetKlants(), CancellationToken.None);

                //Assert
                _collection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<KlantModel>>(),
                    It.IsAny<FindOptions<KlantModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
                result.IsFailure().Should().BeTrue();
                result.Messages.Containing("exception").Should().BeTrue();
            }
        }

        public class AndNoDataFound : WhenGetKlants
        {
            [Fact]
            public async void ThenNullDataFound()
            {
                //Arrange
                _mockAsyncCursorKlantModel.Setup(a => a.Current).Returns(_klantModelsNull);
                _mockAsyncCursorKlantModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                _mockAsyncCursorKlantModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _collection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<KlantModel>>(),
                        It.IsAny<FindOptions<KlantModel, KlantModel>>(),
                        It.IsAny<CancellationToken>())).ReturnsAsync(_mockAsyncCursorKlantModel.Object);

                //Act
                var result = await Subject.HandleAsync(new KlantRepository.GetKlants(), CancellationToken.None);

                //Assert
                _collection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<KlantModel>>(),
                    It.IsAny<FindOptions<KlantModel, KlantModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }

            [Fact]
            public async void ThenNoDataFound()
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

                //Act
                var result = await Subject.HandleAsync(new KlantRepository.GetKlants(), CancellationToken.None);

                //Assert
                _collection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<KlantModel>>(),
                    It.IsAny<FindOptions<KlantModel, KlantModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }
        }

        public class AndDataFound : WhenGetKlants
        {
            [Fact]
            public async void ThenDataFound()
            {
                //Arrange
                _mockAsyncCursorKlantModel.Setup(a => a.Current).Returns(_klantModels);
                _mockAsyncCursorKlantModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                _mockAsyncCursorKlantModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _collection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<KlantModel>>(),
                        It.IsAny<FindOptions<KlantModel>>(),
                        It.IsAny<CancellationToken>()
                    ))
                    .ReturnsAsync(_mockAsyncCursorKlantModel.Object);

                //Act
                var result = await Subject.HandleAsync(new KlantRepository.GetKlants(), CancellationToken.None);

                //Assert
                _collection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<KlantModel>>(),
                    It.IsAny<FindOptions<KlantModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
                result.IsSuccess().Should().BeTrue();
                result.Value.Any().Should().BeTrue();
            }
        }
    }
}
