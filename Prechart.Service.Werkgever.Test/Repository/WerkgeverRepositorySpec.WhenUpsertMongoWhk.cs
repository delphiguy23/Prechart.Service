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
    public class WhenUpsertMongoWhk : WerkgeverRepositorySpec
    {
        public class AndException : WhenUpsertMongoWhk
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
                var result = await Subject.HandleAsync(new WerkgeverRepository.UpsertMongoWhk
                {
                    WerkgeverId = default,
                    Id = default,
                    WgaVastWerkgever = 0,
                    WgaVastWerknemer = 0,
                    FlexWerkgever = 0,
                    FlexWerknemer = 0,
                    ZwFlex = 0,
                    ActiefVanaf = default,
                    ActiefTot = default,
                    SqlId = null,
                    Actief = false,
                }, CancellationToken.None);

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

        public class AndNotFound : WhenUpsertMongoWhk
        {
            [Fact]
            public async void ThenNone()
            {
                //Arrange
                List<MongoWerkgeverModel> mongoWerkgeverlsNull = new();
                var mockAsyncCursorMongoWerkgeverModel = new Mock<IAsyncCursor<MongoWerkgeverModel>>();

                mockAsyncCursorMongoWerkgeverModel.Setup(a => a.Current).Returns(mongoWerkgeverlsNull);
                mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(mockAsyncCursorMongoWerkgeverModel.Object);

                //Act
                var result = await Subject.HandleAsync(new WerkgeverRepository.UpsertMongoWhk
                {
                    WerkgeverId = "633129fb572a926667ed8c5e".ToObjectId(),
                    Id = default,
                    WgaVastWerkgever = 0,
                    WgaVastWerknemer = 0,
                    FlexWerkgever = 0,
                    FlexWerknemer = 0,
                    ZwFlex = 0,
                    ActiefVanaf = default,
                    ActiefTot = default,
                    SqlId = null,
                    Actief = true,
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }
        }

        public class AndDoUpdate : WhenUpsertMongoWhk
        {
            [Fact]
            public async void ThenSome()
            {
                //Arrange
                List<MongoWerkgeverModel> mongoWerkgeverlsNotNull = new()
                {
                    new MongoWerkgeverModel
                    {
                        Id = "633129fb572a926667ed8c5e".ToObjectId(),
                        FiscaalNummer = "123456789L01",
                        DateCreated = new DateTime(2022, 1, 1),
                        Actief = true,
                        WhkPremies = new List<MongoWhkPremie>
                        {
                            new()
                            {
                                Id = "63437781b73780964600ba4d".ToObjectId(),
                                WgaVastWerkgever = 0,
                                WgaVastWerknemer = 0,
                                FlexWerkgever = 0,
                                FlexWerknemer = 0,
                                ZwFlex = 0,
                                ActiefVanaf = new DateTime(DateTime.Now.Year, 1, 1),
                                ActiefTot = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12)),
                                DateCreated = new DateTime(DateTime.Now.Year, 2, 1),
                                Actief = true,
                            },
                        },
                    },
                };

                var mockAsyncCursorMongoWerkgeverModel = new Mock<IAsyncCursor<MongoWerkgeverModel>>();

                mockAsyncCursorMongoWerkgeverModel.Setup(a => a.Current).Returns(mongoWerkgeverlsNotNull);
                mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(mockAsyncCursorMongoWerkgeverModel.Object);

                //Act
                var result = await Subject.HandleAsync(new WerkgeverRepository.UpsertMongoWhk
                {
                    WerkgeverId = "633129fb572a926667ed8c5e".ToObjectId(),
                    Id = "63437781b73780964600ba4d".ToObjectId(),
                    WgaVastWerkgever = 0,
                    WgaVastWerknemer = 0,
                    FlexWerkgever = 0,
                    FlexWerknemer = 0,
                    ZwFlex = 0,
                    ActiefVanaf = default,
                    ActiefTot = default,
                    SqlId = null,
                    Actief = true,
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindOneAndUpdateAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<UpdateDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOneAndUpdateOptions<MongoWerkgeverModel, MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsSuccess().Should().BeTrue();
            }
        }
    }
}