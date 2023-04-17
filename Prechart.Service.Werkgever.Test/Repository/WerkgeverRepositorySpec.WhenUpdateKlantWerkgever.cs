using FluentAssertions;
using MongoDB.Driver;
using Moq;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Werkgever.Repository;
using Xunit;

namespace Prechart.Service.Werkgever.Test.Repository;

public partial class WerkgeverRepositorySpec
{
    public class WhenUpdateKlantWerkgever : WerkgeverRepositorySpec
    {
        public class AndException : WhenGetMongoWerkgever
        {
            [Fact]
            public async void ThenException()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.FindOneAndUpdateAsync(
                        It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<UpdateDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<FindOneAndUpdateOptions<MongoWerkgeverModel, MongoWerkgeverModel>>(),
                        It.IsAny<CancellationToken>()))
                    .Callback(() => throw new Exception("exception"));

                //Act
                var result = await Subject.HandleAsync(new WerkgeverRepository.UpdateKlantWerkgever
                {
                    KlantId = "633129fb572a926667ed8c5d",
                    KlantName = "Test",
                    TaxNo = "123456789L01",
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindOneAndUpdateAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<UpdateDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOneAndUpdateOptions<MongoWerkgeverModel, MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
                result.IsFailure().Should().BeTrue();
                result.Messages.Any(m => m.Contains("exception")).Should().BeTrue();
            }
        }

        public class AndNoDataFound : WhenUpdateKlantWerkgever
        {
            [Fact]
            public async void ThenNone()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.FindOneAndUpdateAsync(
                        It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<UpdateDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<FindOneAndUpdateOptions<MongoWerkgeverModel, MongoWerkgeverModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new MongoWerkgeverModel());

                //Act
                var result = await Subject.HandleAsync(new WerkgeverRepository.UpdateKlantWerkgever
                {
                    KlantId = "633129fb572a926667ed8c5d",
                    KlantName = "Test",
                    TaxNo = "123456789L01",
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindOneAndUpdateAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<UpdateDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOneAndUpdateOptions<MongoWerkgeverModel, MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndDataFound : WhenUpdateKlantWerkgever
        {
            [Fact]
            public async void ThenSome()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.FindOneAndUpdateAsync(
                        It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<UpdateDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<FindOneAndUpdateOptions<MongoWerkgeverModel, MongoWerkgeverModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new MongoWerkgeverModel
                    {
                        Id = "633129fb572a926667ed8c5e".ToObjectId(),
                        Klant = new WerkgeverKlantModel
                        {
                            KlantId = "633129fb572a926667ed8c5d",
                            KlantName = "Test",
                        },
                        Actief = true,
                    });

                //Act
                var result = await Subject.HandleAsync(new WerkgeverRepository.UpdateKlantWerkgever
                {
                    KlantId = "633129fb572a926667ed8c5d",
                    KlantName = "Test",
                    TaxNo = "123456789L01",
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindOneAndUpdateAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<UpdateDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOneAndUpdateOptions<MongoWerkgeverModel, MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsSuccess().Should().BeTrue();
            }
        }
    }
}