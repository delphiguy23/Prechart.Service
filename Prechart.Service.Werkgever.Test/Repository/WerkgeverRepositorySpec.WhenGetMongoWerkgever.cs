using FluentAssertions;
using MongoDB.Driver;
using Moq;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Werkgever.Repository;
using Xunit;

namespace Prechart.Service.Werkgever.Test.Repository;

public partial class WerkgeverRepositorySpec
{
    public class WhenGetMongoWerkgever : WerkgeverRepositorySpec
    {
        public class AndException : WhenGetMongoWerkgever
        {
            [Fact]
            public async void ThenException()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                        It.IsAny<CancellationToken>()))
                    .Callback(() => throw new Exception("exception"));

                //Act
                var result = await Subject.HandleAsync(new WerkgeverRepository.GetMongoWerkgever {Taxno = "123456789L01"}, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
                result.IsFailure().Should().BeTrue();
                result.Messages.Any(m => m.Contains("exception")).Should().BeTrue();
            }
        }

        public class AndNoDataFound : WhenGetMongoWerkgever
        {
            [Fact]
            public async void ThenNone()
            {
                //Arrange

                List<MongoWerkgeverModel> _mongoWerkgeverlsNull = null;
                var _mockAsyncCursorWerkgeverModel = new Mock<IAsyncCursor<MongoWerkgeverModel>>();
                var _mockAsyncCursorMongoWerkgeverModel = new Mock<IAsyncCursor<MongoWerkgeverModel>>();

                _mockAsyncCursorMongoWerkgeverModel.Setup(a => a.Current).Returns(_mongoWerkgeverlsNull);
                _mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                _mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_mockAsyncCursorWerkgeverModel.Object);

                //Act
                var result = await Subject.HandleAsync(new WerkgeverRepository.GetMongoWerkgever {Taxno = "123456789L01"}, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }
        }

        public class AndDataFound : WhenGetMongoWerkgever
        {
            [Fact]
            public async void ThenSome()
            {
                //Arrange

                List<MongoWerkgeverModel> _mongoWerkgever = new()
                {
                    new MongoWerkgeverModel
                    {
                        Id = "633129fb572a926667ed8c5d".ToObjectId(),
                        FiscaalNummer = "123456789L01",
                        Actief = true,
                    },
                };

                var _mockAsyncCursorMongoWerkgeverModel = new Mock<IAsyncCursor<MongoWerkgeverModel>>();

                _mockAsyncCursorMongoWerkgeverModel.Setup(a => a.Current).Returns(_mongoWerkgever);
                _mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                _mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_mockAsyncCursorMongoWerkgeverModel.Object);

                //Act
                var result = await Subject.HandleAsync(new WerkgeverRepository.GetMongoWerkgever {Taxno = "123456789L01"}, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsSuccess().Should().BeTrue();
                result.Value.Should().BeAssignableTo<List<MongoWerkgeverModel>>().Which.Any(v => v.FiscaalNummer == "123456789L01" && v.Id == "633129fb572a926667ed8c5d".ToObjectId()).Should().BeTrue();
            }
        }

        public class AndDataWithcollectieveFound : WhenGetMongoWerkgever
        {
            [Fact]
            public async void ThenSome()
            {
                //Arrange

                List<MongoWerkgeverModel> _mongoWerkgever = new()
                {
                    new MongoWerkgeverModel
                    {
                        Id = "633129fb572a926667ed8c5d".ToObjectId(),
                        FiscaalNummer = "123456789L01",
                        Actief = true,
                        Collectieve = new List<CollectieveAangifteModel>
                        {
                            new()
                            {
                                TaxNo = "123456789L01",
                            },
                        },
                    },
                };

                var _mockAsyncCursorMongoWerkgeverModel = new Mock<IAsyncCursor<MongoWerkgeverModel>>();

                _mockAsyncCursorMongoWerkgeverModel.Setup(a => a.Current).Returns(_mongoWerkgever);
                _mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                _mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_mockAsyncCursorMongoWerkgeverModel.Object);

                //Act
                var result = await Subject.HandleAsync(new WerkgeverRepository.GetMongoWerkgever {Taxno = "123456789L01"}, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsSuccess().Should().BeTrue();
                result.Value.Should().BeAssignableTo<List<MongoWerkgeverModel>>().Which.Any(v => v.FiscaalNummer == "123456789L01" && v.Id == "633129fb572a926667ed8c5d".ToObjectId()).Should().BeTrue();
                result.Value.Should().BeAssignableTo<List<MongoWerkgeverModel>>().Which.FirstOrDefault()?.Collectieve.Any().Should().BeTrue();
            }
        }
    }
}