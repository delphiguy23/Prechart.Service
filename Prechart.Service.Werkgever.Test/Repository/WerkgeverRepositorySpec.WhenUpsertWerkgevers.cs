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
    public class WhenUpsertWerkgevers : WerkgeverRepositorySpec
    {
        public class AndException : WhenUpsertWerkgevers
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
                var result = await Subject.HandleAsync(new WerkgeverRepository.UpsertWerkgevers
                {
                    Werkgevers = new List<MongoWerkgeverModel>
                    {
                        new()
                        {
                            FiscaalNummer = "123456789L01",
                            Actief = true,
                        },
                    },
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

        public class AndDoInsertWithoutWhk : WhenUpsertWerkgevers
        {
            [Fact]
            public async void ThenSome()
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
                var result = await Subject.HandleAsync(new WerkgeverRepository.UpsertWerkgevers
                {
                    Werkgevers = new List<MongoWerkgeverModel>
                    {
                        new()
                        {
                            FiscaalNummer = "123456789L01",
                            Actief = true,
                        },
                    },
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsSuccess().Should().BeTrue();
                result.Value.Any(f => f.FiscaalNummer == "123456789L01").Should().BeTrue();
            }
        }

        public class AndDoInsertWithWhk : WhenUpsertWerkgevers
        {
            [Fact]
            public async void ThenSome()
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
                var result = await Subject.HandleAsync(new WerkgeverRepository.UpsertWerkgevers
                {
                    Werkgevers = new List<MongoWerkgeverModel>
                    {
                        new()
                        {
                            FiscaalNummer = "123456789L01",
                            Actief = true,
                            WhkPremies = new List<MongoWhkPremie>
                            {
                                new()
                                {
                                    WgaVastWerkgever = 1,
                                    WgaVastWerknemer = 1,
                                    FlexWerkgever = 1,
                                    FlexWerknemer = 1,
                                    ZwFlex = 1,
                                    ActiefVanaf = new DateTime(DateTime.Now.Year, 1, 1),
                                    ActiefTot = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12)),
                                    SqlId = 0,
                                    Actief = true,
                                },
                            },
                        },
                    },
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsSuccess().Should().BeTrue();
                result.Value.Any(f => f.FiscaalNummer == "123456789L01").Should().BeTrue();
                result.Value
                    .FirstOrDefault()
                    .WhkPremies
                    .Any(w => w.DateCreated.Date == DateTime.Now.Date)
                    .Should().BeTrue();
            }
        }

        public class AndDoUpdate : WhenUpsertWerkgevers
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
                var result = await Subject.HandleAsync(new WerkgeverRepository.UpsertWerkgevers
                {
                    Werkgevers = new List<MongoWerkgeverModel>
                    {
                        new()
                        {
                            FiscaalNummer = "123456789L01",
                            Actief = true,
                            WhkPremies = new List<MongoWhkPremie>
                            {
                                new()
                                {
                                    WgaVastWerkgever = 1,
                                    WgaVastWerknemer = 1,
                                    FlexWerkgever = 1,
                                    FlexWerknemer = 1,
                                    ZwFlex = 1,
                                    ActiefVanaf = new DateTime(DateTime.Now.Year, 1, 1),
                                    ActiefTot = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12)),
                                    SqlId = 0,
                                    Actief = true,
                                },
                            },
                            Collectieve = new List<CollectieveAangifteModel>
                            {
                                new()
                                {
                                    TaxNo = "123456789L01",
                                    Periode = "2022-01",
                                    CollectieveType = CollectieveType.Normaal,
                                },
                            },
                        },
                    },
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsSuccess().Should().BeTrue();

                result.Value.Any(f => f.FiscaalNummer == "123456789L01")
                    .Should()
                    .BeTrue();

                result.Value
                    .FirstOrDefault(f => f.FiscaalNummer == "123456789L01" && f.DateLastModified.Date == DateTime.Now.Date)
                    .Should()
                    .NotBeNull();

                result.Value
                    .FirstOrDefault(f => f.FiscaalNummer == "123456789L01")
                    .WhkPremies
                    .Any()
                    .Should()
                    .BeTrue();

                result.Value
                    .FirstOrDefault(f => f.FiscaalNummer == "123456789L01")
                    .Collectieve
                    .Any()
                    .Should()
                    .BeTrue();
            }
        }
    }
}